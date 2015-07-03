using System;
using System.Net;
using System.Windows;
using Caliburn.Micro;
using Dev2.Activities.Designers2.Core.Help;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Common.Interfaces.Studio.ViewModels.Dialogues;
using Dev2.ConnectionHelpers;
using Dev2.Interfaces;
using Dev2.Studio.ViewModels.WorkSurface;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Dev2.Settings.Scheduler
{
    public class EmailSourceViewModel : BaseWorkSurfaceViewModel, IHelpSource, IStudioTab
    {
        string _helpText;
        ManageEmailSourceViewModel _vm;
        readonly IPopupController _popupController;
        public EmailSourceViewModel(IEventAggregator eventPublisher, ManageEmailSourceViewModel vm, IPopupController popupController)
            : base(new EventAggregator())
        {
            ViewModel = vm;
            _popupController = popupController;
        }
        public EmailSourceViewModel()
            : base(new EventAggregator())
        {
            IManageEmailSourceModel model = new ManageEmailSourceModel(CustomContainer.Get<IStudioUpdateManager>(),CustomContainer.Get<IQueryManager>(),"todo");
            var svr = CustomContainer.Get<IServer>();
            var nm = new RequestServiceNameViewModel(new EnvironmentViewModel(svr), new RequestServiceNameView(), Guid.NewGuid());
            ViewModel = new ManageEmailSourceViewModel(model,
               nm, CustomContainer.Get< Microsoft.Practices.Prism.PubSubEvents.IEventAggregator>());
            _popupController = CustomContainer.Get<IPopupController>();
        }


        public override object GetView(object context = null)
        {
            var view = new ManageEmailSourceControl();
            return view;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, ViewModel);
        }

        protected override void OnViewLoaded(object view)
        {
            var loadedView = view as ManageEmailSourceControl;
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
        public ManageEmailSourceViewModel ViewModel
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
            if (ViewModel.HasChanged )
            {
                MessageBoxResult showSchedulerCloseConfirmation = _popupController.ShowSchedulerCloseConfirmation();
                if(showSchedulerCloseConfirmation == MessageBoxResult.Cancel || showSchedulerCloseConfirmation == MessageBoxResult.None)
                {
                    return false;
                }
                if(showSchedulerCloseConfirmation == MessageBoxResult.No)
                {
                    return true;
                }

            }

            return true;
        }

        #endregion
    }
}