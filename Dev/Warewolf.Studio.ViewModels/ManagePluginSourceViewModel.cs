
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Warewolf.Core;
using Warewolf.Studio.Core;
using Warewolf.Studio.Models.Help;

namespace Warewolf.Studio.ViewModels
{
    public class ManagePluginSourceViewModel : SourceBaseImpl<IPluginSource>, IDisposable, IManagePluginSourceViewModel
    {
        IDllListingModel _selectedDll;
        readonly IManagePluginSourceModel _updateManager;
        readonly IEventAggregator _aggregator;
        IPluginSource _pluginSource;
        string _resourceName;
        readonly string _warewolfserverName;
        string _headerText;
        private bool _isDisposed;
        List<IDllListingModel> _dllListings;
        bool _isLoading;
        string _searchTerm;
        private IDllListingModel _gacItem;
        string _assemblyName;

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public ManagePluginSourceViewModel(IManagePluginSourceModel updateManager, IEventAggregator aggregator,IAsyncWorker asyncWorker)
            : base(ResourceType.PluginSource)
        {
            VerifyArgument.IsNotNull("asyncWorker", asyncWorker);
            VerifyArgument.IsNotNull("updateManager", updateManager);
            VerifyArgument.IsNotNull("aggregator", aggregator);
            _updateManager = updateManager;
            _aggregator = aggregator;
            AsyncWorker = asyncWorker;
            HeaderText = Resources.Languages.Core.PluginSourceNewHeaderLabel;
            Header = Resources.Languages.Core.PluginSourceNewHeaderLabel;
            OkCommand = new DelegateCommand(Save, CanSave);
            CancelCommand = new DelegateCommand(() => CloseAction.Invoke());
            ClearSearchTextCommand = new DelegateCommand(() => SearchTerm = "");
            RefreshCommand = new DelegateCommand(PerformLoadAll);
            PerformLoadAll();
            _warewolfserverName = updateManager.ServerName;
            
        }

        public IAsyncWorker AsyncWorker { get; set; }

        public ICommand RefreshCommand { get; set; }

        public IDllListingModel GacItem
        {
            get { return _gacItem; }
            set
            {
                _gacItem = value;
                OnPropertyChanged(() => GacItem);
            }
        }

        void PerformLoadAll()
        {
            AsyncWorker.Start(() =>
            {
                IsLoading = true;
                var names = _updateManager.GetDllListings(null).Select(input => new DllListingModel(_updateManager, input)).ToList();
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    DllListings = new List<IDllListingModel>(names);
                    IsLoading = false;
                    if (DllListings != null && DllListings.Count > 1)
                    {
                        GacItem = DllListings[1];
                    }
                });
            });            
        }

        public ICommand ClearSearchTextCommand { get; set; }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(() => IsLoading);
            }
        }

        public string SearchTerm
        {
            get
            {
                return _searchTerm;
            }
            set
            {
                if (!value.Equals(_searchTerm))
                {
                    _searchTerm = value;
                    PerformSearch(_searchTerm);
                    OnPropertyChanged(() => SearchTerm);
                }
            }
        }

        void PerformSearch(string searchTerm)
        {
            if (DllListings != null)
            {
                foreach (var dllListingModel in DllListings)
                {
                    dllListingModel.Filter(searchTerm);
                }
                OnPropertyChanged(() => DllListings);
            }
        }

        public ICommand CancelCommand { get; set; }
        public Action CloseAction { get; set; }
        public List<IDllListingModel> DllListings
        {
            get
            {
                return _dllListings;
            }
            set
            {
                _dllListings = value;
                OnPropertyChanged(() => DllListings);
            }
        }

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public ManagePluginSourceViewModel(IManagePluginSourceModel updateManager, IRequestServiceNameViewModel requestServiceNameViewModel, IEventAggregator aggregator,IAsyncWorker asyncWorker)
            : this(updateManager, aggregator, asyncWorker)
        {
            VerifyArgument.IsNotNull("requestServiceNameViewModel", requestServiceNameViewModel);

            RequestServiceNameViewModel = requestServiceNameViewModel;
            Item = ToModel();
        }

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public ManagePluginSourceViewModel(IManagePluginSourceModel updateManager, IEventAggregator aggregator, IPluginSource pluginSource,IAsyncWorker asyncWorker)
            : this(updateManager, aggregator,asyncWorker)
        {
            VerifyArgument.IsNotNull("pluginSource", pluginSource);
            _pluginSource = pluginSource;
            SetupHeaderTextFromExisting();
            FromSource(pluginSource);
            ToItem();
        }

        void FromSource(IPluginSource pluginSource)
        {
//            var selectedDll = pluginSource.SelectedDll;
//            if (selectedDll != null)
//            {
//                if (selectedDll.FullName.StartsWith("GAC:"))
//                {
//                    var dllListingModel = DllListings.Find(model => model.Name == "GAC");
//                    dllListingModel.IsExpanded = true;
//                    var itemToSelect = dllListingModel.Children.FirstOrDefault(model => model.FullName == selectedDll.FullName);
//                    if (itemToSelect != null)
//                    {
//                        SelectedDll = itemToSelect;
//                        itemToSelect.IsSelected = true;
//                    }
//                }
//                else
//                {
//                    var fileSystem = selectedDll.FullName.Split('\\');
//
//                    foreach(var dir in fileSystem)
//                    {
//                        foreach (var item in DllListings)
//                        {
//                            if (item.Children != null && item.Name == dir)
//                            {
//                                DllListings.Find(model => model.Name == dir).IsExpanded = true;
//                            }
//                            else
//                            {
//                                if(item.Children != null)
//                                {
//                                    foreach(var child in item.Children)
//                                    {
//                                        if (child.Name.Contains('\\'))
//                                        {
//                                            child.Name = child.Name.Replace("\\", "");
//                                        }
//                                        DllListings.Find(model => child.Name == dir).IsExpanded = true;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            
            //Selected DLL Full Name - Split it by \
            //Selected DLL Full Name - Path.GetParts
            // FullName will Start with GAC then look in GAC
            // DllListings.Find(Name=="GAC").IsExpanded = true
            // Find the File System DllListings
            // Expand that item
            // Find the Drive expand it
            // In drive children find folder expand rinse and repeat till at file name
            SelectedDll = new DllListingModel(_updateManager, pluginSource.SelectedDll);
//            SelectedDll.IsSelected = true;
        }

        public IDllListingModel SelectedDll
        {
            get
            {
                return _selectedDll;
            }
            set
            {
                if (value == null) return;
                _selectedDll = value;
                OnPropertyChanged(() => SelectedDll);
                if(SelectedDll != null)
                {
                    AssemblyName = SelectedDll.FullName;
                    SelectedDll.IsExpanded = true;
                }
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }

        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
            set
            {
                _assemblyName = value;
                OnPropertyChanged(()=>AssemblyName);
            }
        }

        void SetupHeaderTextFromExisting()
        {
            var serverName = _warewolfserverName;
            if (serverName.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                HeaderText = string.Format("{0} {1}", Resources.Languages.Core.PluginSourceEditHeaderLabel, (_pluginSource == null ? ResourceName : _pluginSource.Name).Trim());
                Header = string.Format("{0}", ((_pluginSource == null ? ResourceName : _pluginSource.Name)));
            }
            else
            {
                HeaderText = string.Format("{0} {1} on {2}", Resources.Languages.Core.PluginSourceEditHeaderLabel, (_pluginSource == null ? ResourceName : _pluginSource.Name).Trim(), serverName);
                Header = string.Format("{0} - {1}", ((_pluginSource == null ? ResourceName : _pluginSource.Name)), serverName);
            }
        }
        bool CanSave()
        {
            return _selectedDll != null && !string.IsNullOrEmpty(AssemblyName) && HasChanged &&(AssemblyName.EndsWith(".dll") || AssemblyName.StartsWith("GAC:"));
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var helpDescriptor = new HelpDescriptor("", helpText, null);
            VerifyArgument.IsNotNull("helpDescriptor", helpDescriptor);
            _aggregator.GetEvent<HelpChangedEvent>().Publish(helpDescriptor);

        }


        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                _resourceName = value;
                if (!String.IsNullOrEmpty(value))
                {
                    SetupHeaderTextFromExisting();
                }
                OnPropertyChanged(_resourceName);
            }
        }

        void Save()
        {
            if (_pluginSource == null)
            {
                var res = RequestServiceNameViewModel.ShowSaveDialog();

                if (res == MessageBoxResult.OK)
                {
                    ResourceName = RequestServiceNameViewModel.ResourceName.Name;
                    var src = ToModel();
                    src.Id = Guid.NewGuid();
                    src.Path = RequestServiceNameViewModel.ResourceName.Path ?? RequestServiceNameViewModel.ResourceName.Name;
                    Save(src);
                    _pluginSource = src;
                    ToItem();
                    SetupHeaderTextFromExisting();
                }
            }
            else
            {
                Save(ToModel());
            }
        }

        void ToItem()
        {
            Item = new PluginSourceDefinition { Id = _pluginSource.Id, Name = _pluginSource.Name, Path = _pluginSource.Path, SelectedDll = _pluginSource.SelectedDll };
        }

        void Save(IPluginSource source)
        {
            _updateManager.Save(source);
        }


        public override sealed IPluginSource ToModel()
        {
            if(_pluginSource == null)
            {
                return new PluginSourceDefinition
                {
                    Name = ResourceName,
                    SelectedDll = _selectedDll,
                };
            }
            _pluginSource.SelectedDll = _selectedDll;
            return _pluginSource;
        }

        IRequestServiceNameViewModel RequestServiceNameViewModel { get; set; }

        public ICommand OkCommand { get; set; }

        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                OnPropertyChanged(() => HeaderText);
                OnPropertyChanged(() => Header);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_isDisposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                }

                // Dispose unmanaged resources.
                _isDisposed = true;
            }
        }

      
    }
}
