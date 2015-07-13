using System.Windows;
using Caliburn.Micro;
using Dev2.Activities.Designers2.Core.Help;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Interfaces;
using Dev2.Studio.ViewModels.WorkSurface;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.ViewModels;

namespace Dev2.ViewModels
{
    public class SourceViewModel<T> : BaseWorkSurfaceViewModel, IHelpSource, IStudioTab
    {
        string _helpText;
        SourceBaseImpl<T> _vm;
        readonly IPopupController _popupController;

        public SourceViewModel(IEventAggregator eventPublisher, SourceBaseImpl<T> vm, IPopupController popupController,IView view)
            : base(eventPublisher)
        {
            ViewModel = vm;
            View = view;
            _popupController = popupController;
            ViewModel.PropertyChanged += (sender, args) =>
            {
                if(args.PropertyName == "Header")
                {
                    OnPropertyChanged("DisplayName");
                }
            };
        }

        public override object GetView(object context = null)
        {
            return View;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, ViewModel);
        }

        public override string DisplayName
        {
            get
            {
                return ViewModel.Header;
            }            
        }

        protected override void OnViewLoaded(object view)
        {
            var loadedView = view as IView;
            if (loadedView != null)
            {
                loadedView.DataContext = ViewModel;
                base.OnViewLoaded(loadedView);
            }
        }


        #region Implementation of IHelpSource

        public string HelpText
        {
            get
            {
                return _helpText;
            }
            set
            {
                _helpText = value;
            }
        }
        public SourceBaseImpl<T> ViewModel
        {
            get
            {
                return _vm;
            }
            set
            {
                _vm = value;
            }
        }
        public IView View { get; set; }

        #endregion

        #region Implementation of IStudioTab

        public bool DoDeactivate()
        {
            MessageBoxResult showSchedulerCloseConfirmation = _popupController.ShowSchedulerCloseConfirmation();
            if (showSchedulerCloseConfirmation == MessageBoxResult.Cancel || showSchedulerCloseConfirmation == MessageBoxResult.None)
            {
                return false;
            }
            if (showSchedulerCloseConfirmation == MessageBoxResult.No)
            {
                return true;
            }
            //if (ViewModel.IsOkEnabled )
            //{
            //    MessageBoxResult showSchedulerCloseConfirmation = _popupController.ShowSchedulerCloseConfirmation();
            //    if(showSchedulerCloseConfirmation == MessageBoxResult.Cancel || showSchedulerCloseConfirmation == MessageBoxResult.None)
            //    {
            //        return false;
            //    }
            //    if(showSchedulerCloseConfirmation == MessageBoxResult.No)
            //    {
            //        return true;
            //    }

            //}

            return true;
        }

        #endregion

        public SourceViewModel(IEventAggregator eventPublisher)
            : base(eventPublisher)
        {
        }
    }
}