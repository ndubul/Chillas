using System.Windows;
using Caliburn.Micro;
using Dev2.Activities.Designers2.Core.Help;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Interfaces;
using Dev2.Studio.ViewModels.WorkSurface;
using Warewolf.Studio.Views;

namespace Dev2.ViewModels
{
    public class NewPluginSourceViewModel : BaseWorkSurfaceViewModel, IHelpSource, IStudioTab
    {
        string _helpText;
        IManagePluginSourceViewModel _vm;
        readonly IPopupController _popupController;
        string _displayName;

        public NewPluginSourceViewModel(IEventAggregator eventPublisher, IManagePluginSourceViewModel vm, IPopupController popupController)
            : base(eventPublisher)
        {
            ViewModel = vm;
            _popupController = popupController;            
        }

        public override object GetView(object context = null)
        {
            var view = new ManagePluginSourceControl();           
            return view;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, ViewModel);
        }

        public override string DisplayName
        {
            get
            {
                return ViewModel.ResourceName;
            }
            set
            {
                _displayName = value;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            var loadedView = view as ManagePluginSourceControl;
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
        public IManagePluginSourceViewModel ViewModel
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
    }
}