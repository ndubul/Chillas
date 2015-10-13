using System;
using System.Windows;
using Caliburn.Micro;
using Dev2.Activities.Designers2.Core.Help;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Interfaces;
using Dev2.Studio.ViewModels.WorkSurface;
using FontAwesome.WPF;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Dev2.ViewModels
{
    public class DeployWorksurfaceViewModel : BaseWorkSurfaceViewModel, IHelpSource, IStudioTab

    {
        readonly IPopupController _popupController;

        public DeployWorksurfaceViewModel(IEventAggregator eventPublisher, SingleExplorerDeployViewModel vm, IPopupController popupController,IView view)
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
                var mainViewModel = CustomContainer.Get<IMainViewModel>();
                if (mainViewModel != null)
                {
                    ViewModelUtils.RaiseCanExecuteChanged(mainViewModel.SaveCommand);
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
                return "";
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

        // ReSharper disable once UnusedMember.Global
        public ResourceType ResourceType
        {
            get
            {

                return ResourceType.Unknown;
            }
        }


        #region Implementation of IHelpSource

        public string HelpText { get; set; }
        public SingleExplorerDeployViewModel ViewModel { get; set; }
        public IView View { get; set; }

        #endregion

        #region Implementation of IStudioTab

        public bool IsDirty
        {
            get
            {
                return false;
            }
        }

        public bool DoDeactivate(bool showMessage)
        {

            return true;
        }

        #endregion
    }
}