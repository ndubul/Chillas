using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Dev2.Common.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.Core
{
    public class DllListingModel : BindableBase, IDllListingModel,IEquatable<DllListingModel>
    {
        private readonly IManagePluginSourceModel _updateManager;
        private bool _isExpanded;
        private bool _isVisible;
        private readonly IFileListing _dllListing;
        private ObservableCollection<IDllListingModel> _children;
        private string _filter;
        private bool _progressVisibility;
        private int _currentProgress;
        bool _isSelected;
        bool _isExpanderVisible;

        public DllListingModel(IManagePluginSourceModel updateManager, IFileListing dllListing)
        {
            _updateManager = updateManager;
            if (dllListing != null)
            {
                Name = dllListing.Name;
                FullName = dllListing.FullName;
                if (dllListing.Children != null && dllListing.Children.Count > 0)
                {
                    Children =
                        new AsyncObservableCollection<IDllListingModel>(
                            dllListing.Children.Select(input => new DllListingModel(_updateManager, input)));
                }
                IsDirectory = dllListing.IsDirectory;
                IsExpanderVisible = IsDirectory;
                IsVisible = true;
                _dllListing = dllListing;
                ExpandingCommand = new DelegateCommand(Expanding);
            }
        }

        private void Expanding()
        {
            if (Name == "GAC" && IsExpanded)
            {
                if (Children != null)
                {
                    var gacChildren = _children.ToList();
                    if (gacChildren.Count>5)
                    {
                        ProgressVisibility = true;
                        Children = new AsyncObservableCollection<IDllListingModel>(gacChildren.Take(5));
                        var allChildrenCount = gacChildren.Count;
                        TotalChildrenCount = allChildrenCount;
                        Task.Factory.StartNew(() =>
                        {
                            while (ChildrenCount < allChildrenCount)
                            {
                                var items = gacChildren.Skip(ChildrenCount).Take(25);
                                var col = Children as AsyncObservableCollection<IDllListingModel>;
                                if (col != null)
                                {
                                    col.AddRange(items.ToList());
                                }
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                                {
                                    CurrentProgress = (int)Math.Round((double)(100 * ChildrenCount) / TotalChildrenCount);
                                    OnPropertyChanged(() => ChildrenCount);
                                }));
                            }
                        });
                    }
                }
            }
        }

        public int TotalChildrenCount { get; set; }

        public string Name { get; set; }

        public ObservableCollection<IDllListingModel> Children
        {
            get
            {
                if (_children != null)
                {
                    return String.IsNullOrEmpty(_filter)
                        ? _children
                        : new AsyncObservableCollection<IDllListingModel>(_children.Where(a =>
                        {
                            var inner = a;
                            return inner != null && inner.IsVisible;
                        }));
                }

                return _children;
            }
            set
            {
                _children = value;
                OnPropertyChanged(() => Children);
            }
        }

        public bool ProgressVisibility
        {
            get { return _progressVisibility; }
            set
            {
                _progressVisibility = value;
                OnPropertyChanged(() => ProgressVisibility);
            }
        }

        public int ChildrenCount
        {
            get
            {
                if (Children != null)
                {
                    return Children.Count;
                }
                return 0;
            }
        }
        public int CurrentProgress
        {
            get { return _currentProgress; }
            set
            {
                if (_currentProgress != value)
                {
                    _currentProgress = value;
                    if (_currentProgress >= 100)
                    {
                        ProgressVisibility = false;
                    }
                    OnPropertyChanged(() => CurrentProgress);
                }
            }
        }

        public ICommand ExpandingCommand { get; set; }

        public string FullName { get; set; }

        ICollection<IFileListing> IFileListing.Children { get; set; }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    IsExpanded = true;
                    Expanding();
                }
                
                OnPropertyChanged(() => IsSelected);
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;

                if (_isExpanded && _updateManager != null && (Children == null || Children.Count == 0))
                {
                    var dllListings = _updateManager.GetDllListings(_dllListing);
                    if(dllListings != null)
                    {
                        Children =
                            new AsyncObservableCollection<IDllListingModel>(
                                dllListings.Select(input => new DllListingModel(_updateManager, input))
                                    .ToList());
                    }
                    IsExpanderVisible = Children != null && Children.Count > 0;
                }
                OnPropertyChanged(() => IsExpanded);
            }
        }

        public bool IsDirectory { get; set; }

        public bool IsExpanderVisible
        {
            get { return _isExpanderVisible; }
            set
            {
                _isExpanderVisible = value;
                OnPropertyChanged(() => IsExpanderVisible);
            }
        }

        public void Filter(string searchTerm)
        {
            _filter = searchTerm;
            if (_children != null)
            {
                foreach (var dllListing in _children)
                {
                    var dllListingModel = dllListing;
                    dllListingModel.Filter(searchTerm);
                }
            }
            if (String.IsNullOrEmpty(searchTerm) || Name == "FileSystem" || Name == "GAC" ||
                (_children != null && _children.Count > 0 &&
                 _children.Any(model => model.IsVisible)))
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant());
            }

            OnPropertyChanged(() => Children);
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged(() => IsVisible);
            }
        }

        #region Equality members

        public bool Equals(DllListingModel other)
        {
            return string.Equals(Name, other.Name) && string.Equals(FullName, other.FullName) && IsDirectory == other.IsDirectory;
        }


        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((DllListingModel)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsDirectory.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(DllListingModel left, DllListingModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DllListingModel left, DllListingModel right)
        {
            return !Equals(left, right);
        }

        #endregion
    }

    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection()
        {
            SuppressOnCollectionChanged = false;
        }

        public bool SuppressOnCollectionChanged { get; set; }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list)
        {
            SuppressOnCollectionChanged = true;
        }

        public void AddRange(IList<T> items)
        {
            if (null == items)
            {
                throw new ArgumentNullException("items");
            }


            if (items.Count > 0)
            {
                try
                {
                    SuppressOnCollectionChanged = true;
                    foreach (var item in items)
                    {
                        Add(item);
                    }

                }
                finally
                {
                    SuppressOnCollectionChanged = false;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this));
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {


            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the CollectionChanged event on the current thread
                RaiseCollectionChanged(e);
            }
            else
            {
                // Raises the CollectionChanged event on the creator thread
                _synchronizationContext.Send(RaiseCollectionChanged, e);
            }

        }

        private void RaiseCollectionChanged(object param)
        {
            if (!SuppressOnCollectionChanged)
            {
                // We are in the creator thread, call the base implementation directly
                base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
            }

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the PropertyChanged event on the current thread
                RaisePropertyChanged(e);
            }
            else
            {
                // Raises the PropertyChanged event on the creator thread
                _synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            if (!SuppressOnCollectionChanged)
            {

                // We are in the creator thread, call the base implementation directly
                base.OnPropertyChanged((PropertyChangedEventArgs)param);
            }
        }
    }
}
