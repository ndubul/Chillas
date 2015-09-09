
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Dev2.AppResources.Repositories;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Common.ExtMethods;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.Help;
using Dev2.Common.Interfaces.PopupController;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Studio;
using Dev2.Common.Interfaces.Threading;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Common.Interfaces.WebServices;
using Dev2.ConnectionHelpers;
using Dev2.Data.ServiceModel;
using Dev2.Factory;
using Dev2.Helpers;
using Dev2.Instrumentation;
using Dev2.Interfaces;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Security;
using Dev2.Services.Events;
using Dev2.Services.Security;
using Dev2.Settings;
using Dev2.Settings.Scheduler;
using Dev2.Studio.AppResources.Comparers;
using Dev2.Studio.Controller;
using Dev2.Studio.Core;
using Dev2.Studio.Core.AppResources.Browsers;
using Dev2.Studio.Core.AppResources.DependencyInjection.EqualityComparers;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Factories;
using Dev2.Studio.Core.Helpers;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.Messages;
using Dev2.Studio.Core.Models;
using Dev2.Studio.Core.Utils;
using Dev2.Studio.Core.ViewModels;
using Dev2.Studio.Core.ViewModels.Base;
using Dev2.Studio.Core.Workspaces;
using Dev2.Studio.Enums;
using Dev2.Studio.Factory;
using Dev2.Studio.ViewModels.DependencyVisualization;
using Dev2.Studio.ViewModels.Help;
using Dev2.Studio.ViewModels.Workflow;
using Dev2.Studio.ViewModels.WorkSurface;
using Dev2.Studio.Views;
using Dev2.Studio.Views.DependencyVisualization;
using Dev2.Studio.Views.ResourceManagement;
using Dev2.Threading;
using Dev2.Utils;
using Dev2.ViewModels;
using Dev2.Views.Dialogs;
using Dev2.Views.DropBox;
using Dev2.Webs;
using Dev2.Webs.Callbacks;
using Dev2.Workspaces;
using Infragistics.Windows.DockManager.Events;
using ServiceStack.Common;
using Warewolf.Core;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

// ReSharper disable CheckNamespace
namespace Dev2.Studio.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class MainViewModel : BaseConductor<WorkSurfaceContextViewModel>, IMainViewModel,
                                        IHandle<DeleteResourcesMessage>,
                                        IHandle<DeleteFolderMessage>,
                                        IHandle<ShowDependenciesMessage>,
                                        IHandle<AddWorkSurfaceMessage>,
                                        IHandle<SetActiveEnvironmentMessage>,
                                        IHandle<ShowEditResourceWizardMessage>,
                                        IHandle<DeployResourcesMessage>,
                                        IHandle<ShowHelpTabMessage>,
                                        IHandle<ShowNewResourceWizard>,
                                        IHandle<RemoveResourceAndCloseTabMessage>,
                                        IHandle<SaveAllOpenTabsMessage>,
                                        IHandle<ShowReverseDependencyVisualizer>,
                                        IHandle<FileChooserMessage>,
                                        IHandle<DisplayMessageBoxMessage>, IShellViewModel
    {
        #region Fields

        private IEnvironmentModel _activeEnvironment;
        private Explorer.ExplorerViewModel _explorerViewModel;
        private WorkSurfaceContextViewModel _previousActive;
        private bool _disposed;

        private AuthorizeCommand<string> _newResourceCommand;
        private ICommand _addLanguageHelpPageCommand;
        private RelayCommand _deployAllCommand;
        private ICommand _deployCommand;
        private ICommand _displayAboutDialogueCommand;
        private ICommand _exitCommand;
        private AuthorizeCommand _settingsCommand;
        private AuthorizeCommand _schedulerCommand;
        private ICommand _showCommunityPageCommand;
        readonly IAsyncWorker _asyncWorker;
        private readonly bool _createDesigners;
        private ICommand _showStartPageCommand;
        bool _hasActiveConnection;
        bool _canDebug = true;
        bool _menuExpanded;

        #endregion

        #region Properties

        #region imports

        IWindowManager WindowManager { get; set; }

        public Common.Interfaces.Studio.Controller.IPopupController PopupProvider { private get; set; }

        public IEnvironmentRepository EnvironmentRepository { get; private set; }

        #endregion imports

        public bool CloseCurrent { get; private set; }

        public Explorer.ExplorerViewModel ExplorerViewModel
        {
            get { return _explorerViewModel; }
            set
            {
                if (_explorerViewModel == value) return;
                _explorerViewModel = value;
                NotifyOfPropertyChange(() => ExplorerViewModel);
            }
        }

        public IEnvironmentModel ActiveEnvironment
        {
            get { return _activeEnvironment; }
            set
            {
                if (!Equals(value, _activeEnvironment))
                {
                    _activeEnvironment = value;
                    OnActiveEnvironmentChanged();
                    NotifyOfPropertyChange(() => ActiveEnvironment);
                    DeployAllCommand.RaiseCanExecuteChanged();

                }
            }
        }

        IContextualResourceModel CurrentResourceModel
        {
            get
            {
                if (ActiveItem == null || ActiveItem.WorkSurfaceViewModel == null)
                    return null;

                return ResourceHelper
                    .GetContextualResourceModel(ActiveItem.WorkSurfaceViewModel);
            }
        }

        public IBrowserPopupController BrowserPopupController { get; private set; }


        #endregion

        void OnActiveEnvironmentChanged()
        {
            NewResourceCommand.UpdateContext(ActiveEnvironment);
            SettingsCommand.UpdateContext(ActiveEnvironment);
            SchedulerCommand.UpdateContext(ActiveEnvironment);
        }

        #region Commands

        public AuthorizeCommand EditCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.EditCommand;
            }
        }

        public AuthorizeCommand SaveCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                if (ActiveItem.WorkSurfaceKey.WorkSurfaceContext != WorkSurfaceContext.Workflow)
                {
                    var vm = ActiveItem.WorkSurfaceViewModel as IStudioTab;
                    if (vm != null)
                    {
                        return new AuthorizeCommand(AuthorizationContext.Any, o => vm.DoDeactivate(false), o => vm.IsDirty);
                    }
                }                
                return ActiveItem.SaveCommand;
            }
        }

        public AuthorizeCommand DebugCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.DebugCommand;
            }
        }

        public AuthorizeCommand QuickDebugCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.QuickDebugCommand;
            }
        }

        public AuthorizeCommand QuickViewInBrowserCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.QuickViewInBrowserCommand;
            }
        }
        public AuthorizeCommand ViewInBrowserCommand
        {
            get
            {
                if (ActiveItem == null)
                {
                    return new AuthorizeCommand(AuthorizationContext.None, p => { }, param => false);
                }
                return ActiveItem.ViewInBrowserCommand;
            }
        }

        public ICommand AddLanguageHelpPageCommand
        {
            get
            {
                return _addLanguageHelpPageCommand ??
                       (_addLanguageHelpPageCommand = new DelegateCommand(param => AddLanguageHelpWorkSurface()));
            }
        }

        public ICommand DisplayAboutDialogueCommand
        {
            get
            {
                return _displayAboutDialogueCommand ??
                       (_displayAboutDialogueCommand = new DelegateCommand(param => DisplayAboutDialogue()));
            }
        }

        public ICommand ShowStartPageCommand
        {
            get
            {
                return _showStartPageCommand ?? (_showStartPageCommand = new DelegateCommand(param => ShowStartPage()));
            }
        }

        public ICommand ShowCommunityPageCommand
        {
            get { return _showCommunityPageCommand ?? (_showCommunityPageCommand = new DelegateCommand(param => ShowCommunityPage())); }
        }

        public RelayCommand DeployAllCommand
        {
            get
            {
                return _deployAllCommand ?? (_deployAllCommand = new RelayCommand(param => DeployAll(),
                                                                     param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand =
                    new AuthorizeCommand(AuthorizationContext.Administrator, param => AddSettingsWorkSurface(), param => IsActiveEnvironmentConnected()));
            }
        }

        public AuthorizeCommand SchedulerCommand
        {
            get
            {
                return _schedulerCommand ?? (_schedulerCommand =
                    new AuthorizeCommand(AuthorizationContext.Administrator, param => AddSchedulerWorkSurface(), param => IsActiveEnvironmentConnected()));
            }
        }




        public AuthorizeCommand<string> NewResourceCommand
        {
            get
            {
                return _newResourceCommand ?? (_newResourceCommand =
                    new AuthorizeCommand<string>(AuthorizationContext.Contribute, NewResource, param => IsActiveEnvironmentConnected()));
            }
        }

        [ExcludeFromCodeCoverage]
        public ICommand ExitCommand
        {
            get
            {
                return _exitCommand ??
                       (_exitCommand =
                        new RelayCommand(param =>
                                         Application.Current.Shutdown(), param => true));
            }
        }

        public ICommand DeployCommand
        {
            get
            {
                return _deployCommand ??
                       (_deployCommand = new RelayCommand(param => AddDeployResourcesWorkSurface(CurrentResourceModel)));
            }
        }

        #endregion



        public IVersionChecker Version { get; private set; }
        IConnectControlSingleton ConnectControlSingl { get; set; }

        public bool HasActiveConnection
        {
            get
            {
                return _hasActiveConnection;
            }
            private set
            {
                _hasActiveConnection = value;
                NotifyOfPropertyChange(() => HasActiveConnection);
            }
        }

        #region ctor

        public MainViewModel()
            : this(EventPublishers.Aggregator, new AsyncWorker(), Core.EnvironmentRepository.Instance, new VersionChecker())
        {
        }

        public MainViewModel(IEventAggregator eventPublisher, IAsyncWorker asyncWorker, IEnvironmentRepository environmentRepository,
            IVersionChecker versionChecker, bool createDesigners = true, IBrowserPopupController browserPopupController = null,
            Common.Interfaces.Studio.Controller.IPopupController popupController = null, IWindowManager windowManager = null, IStudioResourceRepository studioResourceRepository = null, IConnectControlSingleton connectControlSingleton = null, Dev2.CustomControls.Connections.IConnectControlViewModel connectControlViewModel = null)
            : base(eventPublisher)
        {
            if (environmentRepository == null)
            {
                throw new ArgumentNullException("environmentRepository");
            }

            if (versionChecker == null)
            {
                throw new ArgumentNullException("versionChecker");
            }
            Version = versionChecker;
            ConnectControlSingl = connectControlSingleton ?? ConnectControlSingleton.Instance;

            VerifyArgument.IsNotNull("asyncWorker", asyncWorker);
            _asyncWorker = asyncWorker;
            _createDesigners = createDesigners;
            BrowserPopupController = browserPopupController ?? new ExternalBrowserPopupController();
            StudioResourceRepository = studioResourceRepository ?? Dev2.AppResources.Repositories.StudioResourceRepository.Instance;
            PopupProvider = popupController ?? new PopupController();
            WindowManager = windowManager ?? new WindowManager();
 
            EnvironmentRepository = environmentRepository;

            MenuPanelWidth = 60;
            _menuExpanded = false;

            if(ExplorerViewModel == null)
            {
                ExplorerViewModel = new Explorer.ExplorerViewModel(eventPublisher, asyncWorker, environmentRepository, StudioResourceRepository, ConnectControlSingl, this, false, enDsfActivityType.All, AddWorkspaceItems, connectControlViewModel);
            }

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            AddWorkspaceItems();
            ShowStartPage();
            DisplayName = "Warewolf" + string.Format(" ({0})", ClaimsPrincipal.Current.Identity.Name).ToUpperInvariant();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

        }

        IStudioResourceRepository StudioResourceRepository { get; set; }

        #endregion ctor

        #region IHandle

        public void Handle(ShowReverseDependencyVisualizer message)
        {
            Dev2Logger.Log.Debug(message.GetType().Name);
            if (message.Model != null)
            {
                AddReverseDependencyVisualizerWorkSurface(message.Model);
            }
        }

        public void Handle(SaveAllOpenTabsMessage message)
        {
            Dev2Logger.Log.Debug(message.GetType().Name);
            PersistTabs();
        }


        public void Handle(AddWorkSurfaceMessage message)
        {
            Dev2Logger.Log.Info(message.GetType().Name);
            AddWorkSurface(message.WorkSurfaceObject);

            if (message.ShowDebugWindowOnLoad)
            {
                if (ActiveItem != null && _canDebug)
                {
                    ActiveItem.DebugCommand.Execute(null);
                }
            }
        }

        public void Handle(DeleteResourcesMessage message)
        {
            Dev2Logger.Log.Info(message.GetType().Name);
            DeleteResources(message.ResourceModels, message.FolderName, message.ShowDialog, message.ActionToDoOnDelete);
        }

        public void Handle(DeleteFolderMessage message)
        {
            Dev2Logger.Log.Info(message.GetType().Name);
            var result = PopupProvider;
            if (ShowDeleteDialogForFolder(message.FolderName, result))
            {
                var actionToDoOnDelete = message.ActionToDoOnDelete;
                if (actionToDoOnDelete != null)
                {
                    actionToDoOnDelete();
                }
            }
            ExplorerViewModel.NavigationViewModel.UpdateSearchFilter();
        }

        public void Handle(SetActiveEnvironmentMessage message)
        {
            Dev2Logger.Log.Info(message.GetType().Name);
            var activeEnvironment = message.EnvironmentModel;
            SetActiveEnvironment(activeEnvironment);
            ExplorerViewModel.UpdateActiveEnvironment(ActiveEnvironment, message.SetFromConnectControl);
        }

        public void SetActiveEnvironment(IEnvironmentModel activeEnvironment)
        {
            ActiveEnvironment = activeEnvironment;
            EnvironmentRepository.ActiveEnvironment = ActiveEnvironment;
            ActiveEnvironment.AuthorizationServiceSet += (sender, args) => OnActiveEnvironmentChanged();
        }

        public void Handle(ShowDependenciesMessage message)
        {
            Dev2Logger.Log.Info(message.GetType().Name);
            var model = message.ResourceModel;
            var dependsOnMe = message.ShowDependentOnMe;
            ShowDependencies(dependsOnMe, model);
        }

        void ShowDependencies(bool dependsOnMe, IContextualResourceModel model)
        {
            var vm = new DependencyVisualiserViewModel(new DependencyVisualiserView(), dependsOnMe);
            vm.ResourceType = Common.Interfaces.Data.ResourceType.DependencyViewer;
            vm.ResourceModel = model;

            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DependencyVisualiser), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void Handle(ShowEditResourceWizardMessage message)
        {
            Dev2Logger.Log.Debug(message.GetType().Name);
            ShowEditResourceWizard(message.ResourceModel);
        }

        public void Handle(ShowHelpTabMessage message)
        {
            Dev2Logger.Log.Debug(message.GetType().Name);
            AddHelpTabWorkSurface(message.HelpLink);
        }

        public void Handle(RemoveResourceAndCloseTabMessage message)
        {
            Dev2Logger.Log.Debug(message.GetType().Name);
            if (message.ResourceToRemove == null)
            {
                return;
            }

            var wfscvm = FindWorkSurfaceContextViewModel(message.ResourceToRemove);
            if (message.RemoveFromWorkspace)
            {
                DeactivateItem(wfscvm, true);
            }
            else
            {
                base.DeactivateItem(wfscvm, true);
            }

            _previousActive = null;

        }

        public void Handle(DeployResourcesMessage message)
        {
            Dev2Logger.Log.Debug(message.GetType().Name);
            var key = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DeployResources);

            var exist = ActivateWorkSurfaceIfPresent(key);
            if (message.ViewModel != null)
            {
                var environmentModel = EnvironmentRepository.FindSingle(model => model.ID == message.ViewModel.EnvironmentId);
                message.ViewModel.ResourceType = Common.Interfaces.Data.ResourceType.DeployViewer;
                if(environmentModel != null)
                {
                    var resourceModel = environmentModel.ResourceRepository.FindSingle(model => model.ID == message.ViewModel.ResourceId);
                    
                    if(resourceModel != null)
                    {
                        resourceModel.ResourceType = (ResourceType)Common.Interfaces.Data.ResourceType.DeployViewer;
                        DeployResource = resourceModel as IContextualResourceModel;
                    }
                }
                if (!exist)
                {
                    AddAndActivateWorkSurface(WorkSurfaceContextFactory.CreateDeployViewModel(message.ViewModel));
                }
                else
                {
                    Dev2Logger.Log.Info("Publish message of type - " + typeof(SelectItemInDeployMessage));
                    EventPublisher.Publish(new SelectItemInDeployMessage(message.ViewModel.ResourceId, message.ViewModel.EnvironmentId));
                }
            }
        }

        public IContextualResourceModel DeployResource { get; set; }

        public void Handle(ShowNewResourceWizard message)
        {
            Dev2Logger.Log.Info(message.GetType().Name);


            NewResource(message.ResourceType, message.ResourcePath);
        }

        public void RefreshActiveEnvironment()
        {
            if (ActiveItem != null && ActiveItem.Environment != null)
            {
                Dev2Logger.Log.Debug("Publish message of type - " + typeof(SetActiveEnvironmentMessage));
                EventPublisher.Publish(new SetActiveEnvironmentMessage(ActiveItem.Environment));
            }
        }

        #endregion

        #region Private Methods

        private void TempSave(IEnvironmentModel activeEnvironment, string resourceType, string resourcePath)
        {
            string newWorflowName = NewWorkflowNames.Instance.GetNext();

            IContextualResourceModel tempResource = ResourceModelFactory.CreateResourceModel(activeEnvironment, resourceType,
                                                                                              resourceType);
            tempResource.Category = string.IsNullOrEmpty(resourcePath) ? "Unassigned\\" + newWorflowName : resourcePath + "\\" + newWorflowName;
            tempResource.ResourceName = newWorflowName;
            tempResource.DisplayName = newWorflowName;
            tempResource.IsNewWorkflow = true;
            StudioResourceRepository.AddResouceItem(tempResource);

            AddAndActivateWorkSurface(WorkSurfaceContextFactory.CreateResourceViewModel(tempResource));
            AddWorkspaceItem(tempResource);
        }

        private void DeployAll()
        {
            object payload = null;

            if (CurrentResourceModel != null && CurrentResourceModel.Environment != null)
            {
                payload = CurrentResourceModel.Environment;
            }
            else if (ActiveEnvironment != null)
            {
                payload = ActiveEnvironment;
            }

            AddDeployResourcesWorkSurface(payload);
        }

        private void DisplayAboutDialogue()
        {
            var factory = CustomContainer.Get<IDialogViewModelFactory>();
            WindowManager.ShowDialog(factory.CreateAboutDialog());
        }
        public void ShowAboutBox()
        {
            var server = CustomContainer.Get<IServer>();
            var splashViewModel = new SplashViewModel(server, new ExternalProcessExecutor());

            SplashPage splashPage = new SplashPage { DataContext = splashViewModel };
            ISplashView splashView = splashPage;
            splashViewModel.ShowServerVersion();
            // Show it 
            splashView.Show(true);
        }

        // Write CodedUI Test Because of Silly Chicken affect ;)
        private bool ShowRemovePopup(IWorkflowDesignerViewModel workflowVm)
        {
            var result = PopupProvider.Show(string.Format(StringResources.DialogBody_NotSaved, workflowVm.ResourceModel.ResourceName), StringResources.DialogTitle_NotSaved,
                                            MessageBoxButton.YesNoCancel, MessageBoxImage.Question, null);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    workflowVm.ResourceModel.Commit();
                    Dev2Logger.Log.Info("Publish message of type - " + typeof(SaveResourceMessage));
                    EventPublisher.Publish(new SaveResourceMessage(workflowVm.ResourceModel, false, false));
                    return true;
                case MessageBoxResult.No:
                    // We need to remove it ;)
                    var model = workflowVm.ResourceModel;
                    try
                    {
                        if (workflowVm.EnvironmentModel.ResourceRepository.DoesResourceExistInRepo(model) && workflowVm.ResourceModel.IsNewWorkflow)
                        {
                            DeleteResources(new List<IContextualResourceModel> { model }, "", false);
                        }
                        else
                        {
                            model.Rollback();
                        }
                    }
                    catch (Exception e)
                    {
                        Dev2Logger.Log.Info("Some clever chicken threw this exception : " + e.Message);
                    }

                    NewWorkflowNames.Instance.Remove(workflowVm.ResourceModel.ResourceName);
                    return true;
                default:
                    return false;
            }
        }

        private void DisplayResourceWizard(IContextualResourceModel resourceModel, bool isedit)
        {
            if (resourceModel == null)
            {
                return;
            }

            if (isedit && resourceModel.ServerResourceType == ResourceType.WorkflowService.ToString())
            {
                PersistTabs();
            }

            // we need to load it so we can extract the sourceID ;)
            if (resourceModel.WorkflowXaml == null)
            {
                resourceModel.Environment.ResourceRepository.ReloadResource(resourceModel.ID, resourceModel.ResourceType, ResourceModelEqualityComparer.Current, true);
            }
            switch (resourceModel.ServerResourceType)
            {
                case "DbSource":
                    EditDbSource(resourceModel);
                    break;
                case "WebSource":
                    EditWebSource(resourceModel);
                    break;
                case "PluginSource":
                    EditPluginSource(resourceModel);
                    break;
                case "DbService":
                    EditDbService(resourceModel);
                    break;
                case "WebService":
                    EditWebService(resourceModel);
                    break;
                case "PluginService":
                    EditPluginService(resourceModel);
                    break;
                case "EmailSource":
                    EditEmailSource(resourceModel);
                    break;
                case "SharepointServerSource":
                    EditSharePointSource(resourceModel);
                    break;
            }
            //WebController.DisplayDialogue(resourceModel, isedit);
        }

        void EditDbSource(IContextualResourceModel resourceModel)
        {
            var db = new DbSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new DbSourceDefinition()
            {
                AuthenticationType = db.AuthenticationType,
                DbName = db.DatabaseName,
                Id = db.ResourceID,
                Name = db.ResourceName,
                Password = db.Password,
                Path = db.ResourcePath,
                ServerName = db.Server,
                Type = enSourceType.SqlDatabase,
                UserName = db.UserID
            };
            EditResource(def);
        }
        void EditPluginSource(IContextualResourceModel resourceModel)
        {
            var db = new PluginSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new PluginSourceDefinition
            {
                 SelectedDll = new DllListing{FullName = db.AssemblyLocation, Name = db.AssemblyName, Children = new Collection<IFileListing>(),IsDirectory = false},
                 Id = db.ResourceID,
                 Name = db.ResourceName,
                 Path = db.ResourcePath
            };
            EditResource(def);
        }
        void EditWebSource(IContextualResourceModel resourceModel)
        {
            var db = new WebSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new WebServiceSourceDefinition()
            {
                AuthenticationType = db.AuthenticationType,
                DefaultQuery = db.DefaultQuery,
                Id = db.ResourceID,
                Name = db.ResourceName,
                Password = db.Password,
                Path = db.ResourcePath,
                HostName = db.Address,
                
                UserName = db.UserName
            };
            EditResource(def);
        }
        void EditSharePointSource(IContextualResourceModel resourceModel)
        {
            var db = new SharepointSource(resourceModel.WorkflowXaml.ToXElement());
            var def = new SharePointServiceSourceDefinition
            {
                AuthenticationType = db.AuthenticationType,
                Server = db.Server,
                Id = db.ResourceID,
                Name = db.ResourceName,
                Password = db.Password,
                Path = db.ResourcePath,
                UserName = db.UserName
            };
            EditResource(def);
        }

        void EditDbService(IContextualResourceModel resourceModel)
        {
            var dbsvc = new DbService(resourceModel.WorkflowXaml.ToXElement());
            var db = dbsvc.Source as DbSource;
           

            if(db != null)
            {
                var def = new DbSourceDefinition()
                {
                    AuthenticationType = db.AuthenticationType,
                    DbName = db.DatabaseName,
                    Id = db.ResourceID,
                    Name = db.ResourceName,
                    Password = db.Password,
                    Path = db.ResourcePath,
                    ServerName = db.Server,
                    Type = enSourceType.SqlDatabase,
                    UserName = db.UserID
                };

                var svc = new DatabaseService()
                {
                    Action = new DbAction()
                    {
                        Inputs = new List<IServiceInput>(dbsvc.Method.Parameters.Select(a => new ServiceInput(a.Name, a.Value??"")))
                        ,
                        Name = dbsvc.Method.Name
                    },
                    Id = dbsvc.ResourceID,
                    Inputs = new List<IServiceInput>(dbsvc.Method.Parameters.Select(a => new ServiceInput(a.Name, a.Value))),
                    Name = dbsvc.ResourceName,
                    OutputMappings = new IServiceOutputMapping[0],
                    Path = dbsvc.ResourcePath,
                    Source = def

                };
                EditResource(svc);
            }
         
        }
        void EditPluginService(IContextualResourceModel resourceModel)
        {
            var db = new PluginService(resourceModel.WorkflowXaml.ToXElement());
            var a = db.Method;
            var def = new PluginServiceDefinition
            {
                Id = db.ResourceID,
                Name = db.ResourceName,
                Path = db.ResourcePath,
                Action = new PluginAction()
                {
                    FullName = db.Namespace,
                    Inputs = a.Parameters.Select(x => new ServiceInput(x.Name, x.DefaultValue ?? "") { Name = x.Name, DefaultValue = x.DefaultValue, EmptyIsNull = x.EmptyToNull, RequiredField = x.IsRequired, TypeName = x.Type } as IServiceInput).ToList(),
                    Method = a.ExecuteAction,
                    Variables = a.Parameters.Select(x => new NameValue() { Name = x.Name + " (" + x.TypeName + ")", Value = "" } as INameValue).ToList(),
                    
                },
                Source = new PluginSourceDefinition()
                {
                    Id = db.Source.ResourceID,
                    Name = db.Source.ResourceName
                }
    
            };
            EditResource(def);
        }
        void EditWebService(IContextualResourceModel resourceModel)
        {
            var escapedXaml = resourceModel.ToServiceDefinition();
            var dbsvc = new WebService(escapedXaml.ToXElement());
            
            var db = dbsvc.Source as WebSource;

            if(db != null)
            {
                var def = new WebServiceDefinition
                {
                    Id = db.ResourceID,
                    Name = dbsvc.ResourceName,
                    Path = db.ResourcePath,
                    QueryString = dbsvc.RequestUrl,
                    Headers =  dbsvc.Headers,
                    PostData =  dbsvc.RequestBody,
                    
                    Source = new WebServiceSourceDefinition() { Id = dbsvc.ResourceID},
                    Inputs = new List<IServiceInput>() {},
                    OutputMappings = new List<IServiceOutputMapping>() { },
                    Method = (Common.Interfaces.WebRequestMethod)dbsvc.RequestMethod
                    
                };
                EditResource(def);
            }
        }

        void EditEmailSource(IContextualResourceModel resourceModel)
        {
            var db = new EmailSource(resourceModel.WorkflowXaml.ToXElement());

            var def = new EmailServiceSourceDefinition
            {
                Id = db.ResourceID,
                HostName = db.Host,
                Password = db.Password,
                UserName = db.UserName,
                Port = db.Port,
                Timeout = db.Timeout,
                ResourceName = db.ResourceName,
                EnableSsl = db.EnableSsl
            };
            EditResource(def);

        }

        public string OpenPasteWindow(string current)
        {
            var pasteView = new ManageWebservicePasteView();
            return pasteView.ShowView(current);
        }

        public IServer LocalhostServer
        {
            get
            {
                return CustomContainer.Get<IServer>(); 
            }           
        }

        public void OpenResource(Guid resourceId, IServer server)
        {
            
        }

        public void ShowPopup(IPopupMessage popupMessage)
        {
            PopupProvider.Show(popupMessage);
        }

        public void EditServer(IServerSource selectedServer)
        {
            var server = CustomContainer.Get<IServer>();
            var serverViewModel = new ManageNewServerViewModel(selectedServer, server.UpdateRepository, CustomContainer.Get<IRequestServiceNameViewModel>(), server.ResourceName, Guid.Empty);
            var vm = new SourceViewModel<IServerSource>(EventPublisher, serverViewModel, PopupProvider, new ManageServerControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebSource), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IDbSource selectedSource)
        {

            var server = CustomContainer.Get<IServer>();
            var dbSourceViewModel = new ManageDatabaseSourceViewModel(new ManageDatabaseSourceModel(server.UpdateRepository, server.QueryProxy, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator() ,selectedSource,_asyncWorker);
            var vm = new SourceViewModel<IDbSource>(EventPublisher, dbSourceViewModel, PopupProvider,new ManageDatabaseSourceControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.ServerSource),vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IPluginSource selectedSource)
        {

            var server = CustomContainer.Get<IServer>();
            var pluginSourceViewModel = new ManagePluginSourceViewModel(new ManagePluginSourceModel(server.UpdateRepository, server.QueryProxy, "") ,new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource,_asyncWorker);
            var vm = new SourceViewModel<IPluginSource>(EventPublisher, pluginSourceViewModel, PopupProvider, new ManagePluginSourceControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.PluginSource), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IWebServiceSource selectedSource)
        {

            var server = CustomContainer.Get<IServer>();
            var viewModel = new ManageWebserviceSourceViewModel(new ManageWebServiceSourceModel(server.UpdateRepository,  ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource,_asyncWorker,new ExternalProcessExecutor());
            var vm = new SourceViewModel<IWebServiceSource>(EventPublisher, viewModel, PopupProvider,new ManageWebserviceSourceControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebSource), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IWebService selectedSource)
        {

            var server = CustomContainer.Get<IServer>();
            var viewModel = new ManageWebServiceViewModel(new ManageWebServiceModel(server.UpdateRepository, server.QueryProxy, this,""), selectedSource);
            var vm = new SourceViewModel<IWebService>(EventPublisher, viewModel, PopupProvider, new ManageWebserviceControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebSource), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IPluginService selectedSource)
        {

            var server = CustomContainer.Get<IServer>();
            var viewModel = new ManagePluginServiceViewModel(new ManagePluginServiceModel(server.UpdateRepository, server.QueryProxy,this, ""),  selectedSource);
            var vm = new SourceViewModel<IPluginService>(EventPublisher, viewModel, PopupProvider, new ManagePluginServiceControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebSource), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IDatabaseService selectedSource)
        {

            var server = CustomContainer.Get<IServer>();
            var dbSourceViewModel = new ManageDatabaseServiceViewModel(new ManageDbServiceModel(server.UpdateRepository, server.QueryProxy,this, ""),null, selectedSource);
            var vm = new SourceViewModel<IDatabaseService>(EventPublisher, dbSourceViewModel, PopupProvider,new ManageDatabaseServiceControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbService), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(IEmailServiceSource selectedSource)
        {
            var server = CustomContainer.Get<IServer>();
            var emailSourceViewModel = new ManageEmailSourceViewModel(new ManageEmailSourceModel(server.UpdateRepository, server.QueryProxy, ""),  new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource);
            var vm = new SourceViewModel<IEmailServiceSource>(EventPublisher, emailSourceViewModel, PopupProvider, new ManageEmailSourceControl());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.ServerSource), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void EditResource(ISharepointServerSource selectedSource)
        {
            var server = CustomContainer.Get<IServer>();
            var viewModel = new SharepointServerSourceViewModel(new SharepointServerSourceModel(server.UpdateRepository, ""), new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), selectedSource, _asyncWorker, new ExternalProcessExecutor());
            var vm = new SourceViewModel<ISharepointServerSource>(EventPublisher, viewModel, PopupProvider, new SharepointServerSource());
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.ServerSource), vm);
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void NewResource(string resourceType)
        {
            NewResource(resourceType, "");
        }

        private void NewResource(string resourceType, string resourcePath)
        {
            if (resourceType == "Workflow")
            {
                TempSave(ActiveEnvironment, resourceType, resourcePath);
                if (View != null)
                {
                    View.ClearToolboxSearch();
                }
            }
            else if (resourceType == "EmailSource")
            {
                AddEmailWorkSurface(resourcePath);
            }
            else if(resourceType=="DropboxSource")
            {
                CreateOAuthType(ActiveEnvironment, resourceType, resourcePath);
            }
            else if(resourceType == "Server")
            {
                AddNewServerSourceSurface(resourcePath);
            }
            else if (resourceType == "DbSource")
            {
                AddNewDbSourceSurface(resourcePath);
            }
            else if (resourceType == "DatabaseService")
            {
                AddNewDbServiceSurface(resourcePath);
            }
            else if (resourceType == "WebSource")
            {
                AddNewWebSourceSurface(resourcePath);
            }
            else if (resourceType == "WebService")
            {
                AddNewWebServiceSurface(resourcePath);
            }
            else if (resourceType == "ResourceSource")
            {
                AddNewPluginSourceSurface(resourcePath);
            }
            else if (resourceType == "ResourceService")
            {
                AddNewPluginServiceSurface(resourcePath);
            }
            else if (resourceType == "SharepointServerSource")
            {
                AddNewSharePointServerSource(resourcePath);
            }
            else
            {
                var resourceModel = ResourceModelFactory.CreateResourceModel(ActiveEnvironment, resourceType);
                resourceModel.Category = string.IsNullOrEmpty(resourcePath) ? null : resourcePath;
                resourceModel.ID = Guid.Empty;
                DisplayResourceWizard(resourceModel, false);
            }
        }

        private async Task<IRequestServiceNameViewModel> GetSaveViewModel(IServer server, string resourcePath)
        {
            var item = StudioResourceRepository.FindItem(model => model.ResourcePath.Equals(resourcePath,StringComparison.OrdinalIgnoreCase));
            return await RequestServiceNameViewModel.CreateAsync(new EnvironmentViewModel(server, this), new RequestServiceNameView(), item.ResourceId);
        }

        async void AddNewServerSourceSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.ServerSource), new SourceViewModel<IServerSource>(EventPublisher, new ManageNewServerViewModel(new ServerSource(), server.UpdateRepository, saveViewModel, "", Guid.NewGuid()), PopupProvider, new ManageServerControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        async void AddNewDbSourceSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server,resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbSource), new SourceViewModel<IDbSource>(EventPublisher, new ManageDatabaseSourceViewModel(new ManageDatabaseSourceModel(server.UpdateRepository, server.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker), PopupProvider,new ManageDatabaseSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        async void AddNewDbServiceSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DbService), new SourceViewModel<IDatabaseService>(EventPublisher, new ManageDatabaseServiceViewModel(new ManageDbServiceModel(server.UpdateRepository, server.QueryProxy,this, ActiveEnvironment.Name), saveViewModel), PopupProvider,new ManageDatabaseServiceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        async void AddNewWebSourceSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebSource), new SourceViewModel<IWebServiceSource>(EventPublisher, new ManageWebserviceSourceViewModel(new ManageWebServiceSourceModel(server.UpdateRepository, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker, new ExternalProcessExecutor()), PopupProvider,new ManageWebserviceSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        async void AddNewWebServiceSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.WebService), new SourceViewModel<IWebService>(EventPublisher, new ManageWebServiceViewModel(new ManageWebServiceModel(server.UpdateRepository, server.QueryProxy, this, ActiveEnvironment.Name), saveViewModel), PopupProvider,new ManageWebserviceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        async void AddNewPluginSourceSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.PluginSource), new SourceViewModel<IPluginSource>(EventPublisher, new ManagePluginSourceViewModel(new ManagePluginSourceModel(server.UpdateRepository, server.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker), PopupProvider,new ManagePluginSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        async void AddNewPluginServiceSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.PluginService), new SourceViewModel<IPluginService>(EventPublisher, new ManagePluginServiceViewModel(new ManagePluginServiceModel(server.UpdateRepository, server.QueryProxy, this, ActiveEnvironment.Name), saveViewModel), PopupProvider,new ManagePluginServiceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        async void AddNewSharePointServerSource(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.SharepointServerSource), new SourceViewModel<ISharepointServerSource>(EventPublisher, new SharepointServerSourceViewModel(new SharepointServerSourceModel(server.UpdateRepository, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator(), _asyncWorker, new ExternalProcessExecutor()), PopupProvider, new SharepointServerSource()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);
        }

        public void CreateOAuthType(IEnvironmentModel activeEnvironment, string resourceType, string resourcePath, bool shouldAuthorise = true)
        {
            var resource = ResourceModelFactory.CreateResourceModel(ActiveEnvironment, resourceType);
            SaveDropBoxSource(activeEnvironment, resourceType, resourcePath, resource, shouldAuthorise);
        }

        public IDropboxFactory DropboxFactory
        {
            private get{return _dropboxFactory??new DropboxFactory();}
            set{_dropboxFactory = value;}
        }
        void SaveDropBoxSource(IEnvironmentModel activeEnvironment, string resourceType, string resourcePath, IContextualResourceModel resource, bool shouldAuthorise)
        {
            DropBoxViewWindow drop = new DropBoxViewWindow();
            DropBoxHelper helper = new DropBoxHelper(drop, activeEnvironment, resourceType, resourcePath);
            DropBoxSourceViewModel vm = new DropBoxSourceViewModel(new NetworkHelper(), helper, DropboxFactory, shouldAuthorise) { Resource = resource };
            drop.DataContext = vm;
            var showDialog = ShowDropboxAction(drop, vm);
            if (showDialog != null && showDialog.Value && vm.HasAuthenticated && vm.Resource.ID == Guid.Empty)
            {
                ShowSaveDialog(vm.Resource, activeEnvironment, vm.Key, vm.Secret);
            }
            else if (showDialog != null && showDialog.Value && vm.HasAuthenticated && vm.Resource.ID != Guid.Empty)
            {

                // ReSharper disable once MaximumChainedReferences
                var dropBoxSource = new OauthSource { Key = vm.Key, Secret = vm.Secret, ResourceName = vm.Resource.ResourceName, ResourcePath = vm.Resource.Category, IsNewResource = true, ResourceID = vm.Resource.ID }.ToStringBuilder();
                ActiveEnvironment.ResourceRepository.SaveResource(ActiveEnvironment, dropBoxSource, GlobalConstants.ServerWorkspaceID);
 
            }
        }

        public Action<IContextualResourceModel, IEnvironmentModel, string, string> ShowSaveDialog
        {
            private get { return _showSaveDialog ?? SaveDialogHelper.ShowNewOAuthsourceSaveDialog; }
            set { _showSaveDialog = value; }
        }
        
        public Func<DropBoxViewWindow, DropBoxSourceViewModel, bool?> ShowDropboxAction
        {
            private get { return _showDropAction ?? ((drop,vm) => drop.ShowDialog()); }
            set{_showDropAction = value;}
       
        }
        private void ShowEditResourceWizard(object resourceModelToEdit)
        {
            var resourceModel = resourceModelToEdit as IContextualResourceModel;
            if (resourceModel != null && resourceModel.ServerResourceType.EqualsIgnoreCase("OauthSource"))
            {
                SaveDropBoxSource(ActiveEnvironment, "DropboxSource", resourceModel.Category, resourceModel, true);
            }
            else
            DisplayResourceWizard(resourceModel, true);
        }


        #endregion Private Methods

        #region Public Methods

        public async void ShowStartPage()
        {

            ActivateOrCreateUniqueWorkSurface<HelpViewModel>(WorkSurfaceContext.StartPage);
            WorkSurfaceContextViewModel workSurfaceContextViewModel = Items.FirstOrDefault(c => c.WorkSurfaceViewModel.DisplayName == "Start Page" && c.WorkSurfaceViewModel.GetType() == typeof(HelpViewModel));
            if (workSurfaceContextViewModel != null)
            {
                await ((HelpViewModel)workSurfaceContextViewModel.WorkSurfaceViewModel).LoadBrowserUri(Version.CommunityPageUri);
            }
        }

        public void ShowCommunityPage()
        {
            BrowserPopupController.ShowPopup(StringResources.Uri_Community_HomePage);
        }

        #region Overrides of ViewAware

        protected override void OnViewAttached(object view, object context)
        {
            if (View == null)
            {
                View = view as MainView;
            }
        }

        MainView View { get; set; }

        public void UpdatePane(IContextualResourceModel model)
        {
            // workflows only
            // Workflow that has this resource open
            // 

            }

        public void ClearToolboxSelection()
        {
            
            if (View != null)
            {
                //View.ClearToolboxSelection();
            }
        }

        #endregion

        public bool IsActiveEnvironmentConnected()
        {
            if (ActiveEnvironment == null)
            {
                return false;
            }

            HasActiveConnection = ActiveItem != null && ActiveItem.IsEnvironmentConnected();
            return ((ActiveEnvironment != null) && (ActiveEnvironment.IsConnected) && (ActiveEnvironment.CanStudioExecute));
        }

        void AddDependencyVisualizerWorkSurface(IContextualResourceModel resource)
        {
            if (resource == null)
                return;

            ActivateOrCreateWorkSurface<DependencyVisualiserViewModel>
                (WorkSurfaceContext.DependencyVisualiser, resource,
                 new[] { new Tuple<string, object>("GetDependsOnMe",false),new Tuple<string, object>("ResourceModel", resource)
                                 });
        }

        void AddReverseDependencyVisualizerWorkSurface(IContextualResourceModel resource)
        {
            if (resource == null)
                return;

            ActivateOrCreateWorkSurface<DependencyVisualiserViewModel>
                (WorkSurfaceContext.ReverseDependencyVisualiser, resource,
                 new[]
                     {
                         new Tuple<string, object>("GetDependsOnMe", true),
                         new Tuple<string, object>("ResourceModel", resource)
                     });
        }

        [ExcludeFromCodeCoverage] //Excluded due to needing a parent window
        void AddSettingsWorkSurface()
        {
            ActivateOrCreateUniqueWorkSurface<SettingsViewModel>(WorkSurfaceContext.Settings);
        }

        [ExcludeFromCodeCoverage] //Excluded due to needing a parent window
        void AddSchedulerWorkSurface()
        {
            ActivateOrCreateUniqueWorkSurface<SchedulerViewModel>(WorkSurfaceContext.Scheduler);
        }

        [ExcludeFromCodeCoverage] //Excluded due to needing a parent window
        async void AddEmailWorkSurface(string resourcePath)
        {
            var server = CustomContainer.Get<IServer>();
            var saveViewModel = await GetSaveViewModel(server, resourcePath);
            var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.EmailSource), new SourceViewModel<IEmailServiceSource>(EventPublisher, new ManageEmailSourceViewModel(new ManageEmailSourceModel(server.UpdateRepository, server.QueryProxy, ActiveEnvironment.Name), saveViewModel, new Microsoft.Practices.Prism.PubSubEvents.EventAggregator()), PopupProvider, new ManageEmailSourceControl()));
            AddAndActivateWorkSurface(workSurfaceContextViewModel);

        }

        async void AddHelpTabWorkSurface(string uriToDisplay)
        {
            if (!string.IsNullOrWhiteSpace(uriToDisplay))
                ActivateOrCreateUniqueWorkSurface<HelpViewModel>
                    (WorkSurfaceContext.Help,
                     new[] { new Tuple<string, object>("Uri", uriToDisplay) });
            WorkSurfaceContextViewModel workSurfaceContextViewModel = Items.FirstOrDefault(c => c.WorkSurfaceViewModel.DisplayName == "Help" && c.WorkSurfaceViewModel.GetType() == typeof(HelpViewModel));
            if (workSurfaceContextViewModel != null)
            {
                await ((HelpViewModel)workSurfaceContextViewModel.WorkSurfaceViewModel).LoadBrowserUri(uriToDisplay);
            }
        }

        async void AddLanguageHelpWorkSurface()
        {
            var path = FileHelper.GetFullPath(StringResources.Uri_Studio_Language_Reference_Document);
            ActivateOrCreateUniqueWorkSurface<HelpViewModel>(WorkSurfaceContext.LanguageHelp
                                                             , new[] { new Tuple<string, object>("Uri", path) });
            WorkSurfaceContextViewModel workSurfaceContextViewModel = Items.FirstOrDefault(c => c.WorkSurfaceViewModel.DisplayName == "Language Help" && c.WorkSurfaceViewModel.GetType() == typeof(HelpViewModel));
            if (workSurfaceContextViewModel != null)
            {
                await ((HelpViewModel)workSurfaceContextViewModel.WorkSurfaceViewModel).LoadBrowserUri(path);
            }
        }

        #endregion

        #region Overrides

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    OnDeactivate(true);
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #region Overrides of ConductorBaseWithActiveItem<WorkSurfaceContextViewModel>

        #endregion

        #region Overrides of ConductorBaseWithActiveItem<WorkSurfaceContextViewModel>

        protected override void ChangeActiveItem(WorkSurfaceContextViewModel newItem, bool closePrevious)
        {
            if (_previousActive != null)
            {
                if (newItem != null)
                {
                    if (newItem.DataListViewModel != null)
                    {
                        string errors;
                        newItem.DataListViewModel.ClearCollections();
                        newItem.DataListViewModel.CreateListsOfIDataListItemModelToBindTo(out errors);
                    }
                }

            }
            base.ChangeActiveItem(newItem, closePrevious);
            RefreshActiveEnvironment();
        }

        #endregion

        public override void DeactivateItem(WorkSurfaceContextViewModel item, bool close)
        {
            if (item == null)
            {
                return;
            }

            bool success = true;
            if (close)
            {
                success = CloseWorkSurfaceContext(item, null);
            }

            if (success)
            {
                if (_previousActive != item && Items.Contains(_previousActive))
                {
                    ActivateItem(_previousActive);
                }

                base.DeactivateItem(item, close);
                item.Dispose();
                CloseCurrent = true;
            }
            else
            {
                CloseCurrent = false;
            }
        }


        // Process saving tabs and such when exiting ;)
        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                PersistTabs();
            }

            base.OnDeactivate(close);
        }

        protected override void OnActivationProcessed(WorkSurfaceContextViewModel item, bool success)
        {
            if (success)
            {
                if (item != null)
                {
                    var wfItem = item.WorkSurfaceViewModel as IWorkflowDesignerViewModel;
                    if (wfItem != null)
                    {
                        AddWorkspaceItem(wfItem.ResourceModel);
                    }
                }
                NotifyOfPropertyChange(() => EditCommand);
                NotifyOfPropertyChange(() => SaveCommand);
                NotifyOfPropertyChange(() => DebugCommand);
                NotifyOfPropertyChange(() => QuickDebugCommand);
                NotifyOfPropertyChange(() => QuickViewInBrowserCommand);
                NotifyOfPropertyChange(() => ViewInBrowserCommand);
                if (MenuViewModel != null)
                {
                    MenuViewModel.SaveCommand = SaveCommand;
                    MenuViewModel.ExecuteServiceCommand = DebugCommand;
            }
            }
            base.OnActivationProcessed(item, success);
        }

        public ICommand SaveAllCommand
        {
            get
            {
                return new DelegateCommand(SaveAll);
            }
        }

        void SaveAll(object obj)
        {
            for(int index = Items.Count-1; index >= 0; index--)
            {
                var workSurfaceContextViewModel = Items[index];
                ActivateItem(workSurfaceContextViewModel);
                var workSurfaceContext = workSurfaceContextViewModel.WorkSurfaceKey.WorkSurfaceContext;
                if(workSurfaceContext == WorkSurfaceContext.Workflow)
                {
                    if(workSurfaceContextViewModel.CanSave())
                    {
                        workSurfaceContextViewModel.Save();                        
                    }
                }
                else
                {
                    var vm = workSurfaceContextViewModel.WorkSurfaceViewModel;
                    var viewModel = vm as IStudioTab;
                    if(viewModel != null)
                    {
                        viewModel.DoDeactivate(true);                        
                    }
                }
            }
        }

        public override void ActivateItem(WorkSurfaceContextViewModel item)
        {
            _previousActive = ActiveItem;
            if(_previousActive != null)
            {
                if(_previousActive.DebugOutputViewModel != null)
                {
                    _previousActive.DebugOutputViewModel.PropertyChanged-=DebugOutputViewModelOnPropertyChanged;
                }
            }
            base.ActivateItem(item);
            if (item == null || item.ContextualResourceModel == null) return;
            if(item.DebugOutputViewModel != null)
            {
                item.DebugOutputViewModel.PropertyChanged += DebugOutputViewModelOnPropertyChanged;
            }
            if (ExplorerViewModel != null)
            {
                ExplorerViewModel.BringItemIntoView(item);
            }
        }

        void DebugOutputViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsProcessing")
            {
                if (MenuViewModel != null)
                {
                    if(ActiveItem.DebugOutputViewModel != null)
                    {
                        MenuViewModel.IsProcessing = ActiveItem.DebugOutputViewModel.IsProcessing;
                    }
                }
            }
        }

        #endregion
        #region Resource Deletion

        private bool ConfirmDeleteAfterDependencies(ICollection<IContextualResourceModel> models)
        {
            if (!models.Any(model => model.Environment.ResourceRepository.HasDependencies(model)))
            {
                return true;
            }


            if (models.Count > 1)
            {
                new DeleteFolderDialog().ShowDialog();
                return false;
            }
            if (models.Count == 1)
            {
                var model = models.FirstOrDefault();
                var dialog = new DeleteResourceDialog(model);
                dialog.ShowDialog();
                if (dialog.OpenDependencyGraph)
                {
                    ShowDependencies(false,model);
                }
                return false;
            }
            return true;
        }

        private bool ConfirmDelete(ICollection<IContextualResourceModel> models, string folderName)
        {
            bool confirmDeleteAfterDependencies = ConfirmDeleteAfterDependencies(models);
            if (confirmDeleteAfterDependencies)
            {
                Common.Interfaces.Studio.Controller.IPopupController result = new PopupController();
                if (models.Count > 1)
                {
                    var contextualResourceModel = models.FirstOrDefault();
                    if (contextualResourceModel != null)
                    {
                        var folderBeingDeleted = folderName;
                        return ShowDeleteDialogForFolder(folderBeingDeleted, result);
                    }
                }
                if (models.Count == 1)
                {
                    var contextualResourceModel = models.FirstOrDefault();
                    if (contextualResourceModel != null)
                    {
                        var deletionName = folderName;
                        var description = "";
                        if (string.IsNullOrEmpty(deletionName))
                        {
                            deletionName = contextualResourceModel.ResourceName;
                            description = contextualResourceModel.ResourceType.GetDescription();
                        }
                        var deletePrompt = String.Format(StringResources.DialogBody_ConfirmDelete, deletionName,
                            description);
                        var deleteAnswer = PopupProvider.Show(deletePrompt, StringResources.DialogTitle_ConfirmDelete,
                            MessageBoxButton.YesNo, MessageBoxImage.Warning, null);

                        var shouldDelete = deleteAnswer == MessageBoxResult.Yes;
                        return shouldDelete;
                    }
                }
            }
            return false;
        }

        bool ShowDeleteDialogForFolder(string folderBeingDeleted, Common.Interfaces.Studio.Controller.IPopupController result)
        {
            var deletePrompt = String.Format(StringResources.DialogBody_ConfirmFolderDelete, folderBeingDeleted);
            var deleteAnswer = result.Show(deletePrompt, StringResources.DialogTitle_ConfirmDelete, MessageBoxButton.YesNo, MessageBoxImage.Warning, null);
            var confirmDelete = deleteAnswer == MessageBoxResult.Yes;
            return confirmDelete;
        }

        public bool ShowDeleteDialogForFolder(string folderBeingDeleted)
        {
            return ShowDeleteDialogForFolder(folderBeingDeleted,PopupProvider);
        }

        private void DeleteResources(ICollection<IContextualResourceModel> models, string folderName, bool showConfirm = true, System.Action actionToDoOnDelete = null)
        {
            if (models == null || (showConfirm && !ConfirmDelete(models, folderName)))
            {
                return;
            }

            foreach (var contextualModel in models)
            {
                if (contextualModel == null)
                {
                    continue;
                }

                DeleteContext(contextualModel);

                if (contextualModel.Environment.ResourceRepository.DeleteResource(contextualModel).HasError)
                {
                    return;
                }
                //If its deleted from loalhost, and is a server, also delete from repository
                if (contextualModel.Environment.IsLocalHost)
                {
                    if (contextualModel.ResourceType == ResourceType.Source)
                    {
                        if (contextualModel.ServerResourceType == "Server")
                        {
                            var environment = EnvironmentRepository.Get(contextualModel.ID);

                            if (environment != null)
                            {
                                Dev2Logger.Log.Debug("Publish message of type - " + typeof(EnvironmentDeletedMessage));
                                EventPublisher.Publish(new EnvironmentDeletedMessage(environment));
                                EnvironmentRepository.Remove(environment);
                            }
                        }
                    }
                }
                if (actionToDoOnDelete != null)
                {
                    actionToDoOnDelete();
                }
            }
            ExplorerViewModel.NavigationViewModel.UpdateSearchFilter();
        }

        #endregion delete

        #region WorkspaceItems management

        public double MenuPanelWidth { get; set; }

        private void SaveWorkspaceItems()
        {
            _getWorkspaceItemRepository().Write();
        }

        private void AddWorkspaceItem(IContextualResourceModel model)
        {
            _getWorkspaceItemRepository().AddWorkspaceItem(model);
        }

        readonly Func<IWorkspaceItemRepository> _getWorkspaceItemRepository = () => WorkspaceItemRepository.Instance;

        private void RemoveWorkspaceItem(IDesignerViewModel viewModel)
        {
            _getWorkspaceItemRepository().Remove(viewModel.ResourceModel);
        }

        protected virtual void AddWorkspaceItems()
        {
            if (EnvironmentRepository == null) return;

            HashSet<IWorkspaceItem> workspaceItemsToRemove = new HashSet<IWorkspaceItem>();
            // ReSharper disable ForCanBeConvertedToForeach
            for(int i = 0; i < _getWorkspaceItemRepository().WorkspaceItems.Count; i++)
            // ReSharper restore ForCanBeConvertedToForeach
            {
                //
                // Get the environment for the workspace item
                //
                IWorkspaceItem item = _getWorkspaceItemRepository().WorkspaceItems[i];
                Dev2Logger.Log.Info(string.Format("Start Proccessing WorkspaceItem: {0}", item.ServiceName));
                IEnvironmentModel environment = EnvironmentRepository.All().Where(env => env.IsConnected).TakeWhile(env => env.Connection != null).FirstOrDefault(env => env.ID == item.EnvironmentID);

                if (environment == null || environment.ResourceRepository == null)
                {
                    Dev2Logger.Log.Info("Environment Not Found");
                    if (environment != null && item.EnvironmentID == environment.ID)
                    {
                        workspaceItemsToRemove.Add(item);
                    }
                }
                if (environment != null)
                {
                    Dev2Logger.Log.Info(string.Format("Proccessing WorkspaceItem: {0} for Environment: {1}", item.ServiceName, environment.DisplayName));
                    if (environment.ResourceRepository != null)
                    {
                        environment.ResourceRepository.LoadResourceFromWorkspace(item.ID, item.WorkspaceID);
                        var resource = environment.ResourceRepository.All().FirstOrDefault(rm =>
                        {
                            var sameEnv = true;
                            if (item.EnvironmentID != Guid.Empty)
                            {
                                sameEnv = item.EnvironmentID == environment.ID;
                            }
                            return rm.ID == item.ID && sameEnv;
                        }) as IContextualResourceModel;

                        if (resource == null)
                        {
                            workspaceItemsToRemove.Add(item);
                        }
                        else
                        {
                            Dev2Logger.Log.Info(string.Format("Got Resource Model: {0} ", resource.DisplayName));
                            var fetchResourceDefinition = environment.ResourceRepository.FetchResourceDefinition(environment, item.WorkspaceID, resource.ID, false);
                            resource.WorkflowXaml = fetchResourceDefinition.Message;
                            resource.IsWorkflowSaved = item.IsWorkflowSaved;
                            resource.OnResourceSaved += model => _getWorkspaceItemRepository().UpdateWorkspaceItemIsWorkflowSaved(model);
                            AddWorkSurfaceContextFromWorkspace(resource);
                        }
                    }
                }
                else
                {
                    workspaceItemsToRemove.Add(item);
                }
            }

            foreach (IWorkspaceItem workspaceItem in workspaceItemsToRemove)
            {
                _getWorkspaceItemRepository().WorkspaceItems.Remove(workspaceItem);
            }
        }

        #endregion

        #region Tab Management

        public void AddDeployResourcesWorkSurface(object input)
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DeployResources);
            bool exist = ActivateWorkSurfaceIfPresent(key);
            DeployResource = input as IContextualResourceModel;
            if (exist)
            {
                if (input is IContextualResourceModel)
                {
                    Dev2Logger.Log.Info("Publish message of type - " + typeof(SelectItemInDeployMessage));
                    EventPublisher.Publish(
                        new SelectItemInDeployMessage((input as IContextualResourceModel).ID,
                            (input as IContextualResourceModel).Environment.ID));
                }
            }
            else
            {
                WorkSurfaceContextViewModel context = WorkSurfaceContextFactory.CreateDeployViewModel(input);
                Items.Add(context);
                ActivateItem(context);
                Tracker.TrackEvent(TrackerEventGroup.Deploy, TrackerEventName.Opened);
            }
        }

        private void DeleteContext(IContextualResourceModel model)
        {
            var context = FindWorkSurfaceContextViewModel(model);
            if (context == null)
            {
                return;
            }

            context.DeleteRequested = true;
            base.DeactivateItem(context, true);
        }

        private void CreateAndActivateUniqueWorkSurface<T>
            (WorkSurfaceContext context, Tuple<string, object>[] initParms = null)
            where T : IWorkSurfaceViewModel
        {
            WorkSurfaceContextViewModel ctx = WorkSurfaceContextFactory.Create<T>(context, initParms);
            AddAndActivateWorkSurface(ctx);
        }

        private void CreateAndActivateWorkSurface<T>
            (WorkSurfaceKey key, Tuple<string, object>[] initParms = null)
            where T : IWorkSurfaceViewModel
        {
            WorkSurfaceContextViewModel ctx = WorkSurfaceContextFactory.Create<T>(key, initParms);
            AddAndActivateWorkSurface(ctx);
        }

        private void ActivateOrCreateWorkSurface<T>(WorkSurfaceContext context, IContextualResourceModel resourceModel,
                                                          Tuple<string, object>[] initParms = null)
            where T : IWorkSurfaceViewModel
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateKey(context, resourceModel);
            bool exists = ActivateWorkSurfaceIfPresent(key, initParms);

            if (!exists)
            {
                CreateAndActivateWorkSurface<T>(key, initParms);
            }
        }

        private void ActivateOrCreateUniqueWorkSurface<T>(WorkSurfaceContext context,
                                                          Tuple<string, object>[] initParms = null)
            where T : IWorkSurfaceViewModel
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateKey(context);
            bool exists = ActivateWorkSurfaceIfPresent(key, initParms);

            if (!exists)
            {
                if (typeof(T) == typeof(SettingsViewModel))
                {
                    Tracker.TrackEvent(TrackerEventGroup.Settings, TrackerEventName.Opened);
                }

                CreateAndActivateUniqueWorkSurface<T>(context, initParms);
            }
        }

        private bool ActivateWorkSurfaceIfPresent(IContextualResourceModel resource,
                                                  Tuple<string, object>[] initParms = null)
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateKey(resource);
            return ActivateWorkSurfaceIfPresent(key, initParms);
        }

        bool ActivateWorkSurfaceIfPresent(WorkSurfaceKey key, Tuple<string, object>[] initParms = null)
        {
            WorkSurfaceContextViewModel currentContext = FindWorkSurfaceContextViewModel(key);

            if (currentContext != null)
            {
                if (initParms != null)
                    PropertyHelper.SetValues(
                        currentContext.WorkSurfaceViewModel, initParms);

                ActivateItem(currentContext);
                return true;
            }
            return false;
        }

        public bool IsWorkFlowOpened(IContextualResourceModel resource)
        {
            return FindWorkSurfaceContextViewModel(resource) != null;
        }
        public void UpdateWorkflowLink(IContextualResourceModel resource, string newPath, string oldPath)
        {
            var x = (FindWorkSurfaceContextViewModel(resource));
            if (x != null)
            {
             
            
            var path = oldPath.Replace('\\', '/');
            var b = x.WorkSurfaceViewModel as WorkflowDesignerViewModel;
                if (b != null)
            {
                b.UpdateWorkflowLink(b.DisplayWorkflowLink.Replace(path, newPath.Replace('\\', '/')));   
            }
            }
        }

        WorkSurfaceContextViewModel FindWorkSurfaceContextViewModel(WorkSurfaceKey key)
        {
            return Items.FirstOrDefault(
                c => WorkSurfaceKeyEqualityComparer.Current.Equals(key, c.WorkSurfaceKey));
        }

        public WorkSurfaceContextViewModel FindWorkSurfaceContextViewModel(IContextualResourceModel resource)
        {
            var key = WorkSurfaceKeyFactory.CreateKey(resource);
            return FindWorkSurfaceContextViewModel(key);
        }

        void AddWorkSurfaceContextFromWorkspace(IContextualResourceModel resourceModel)
        {
            AddWorkSurfaceContextImpl(resourceModel, true);
        }

        public void AddWorkSurfaceContext(IContextualResourceModel resourceModel)
        {
            AddWorkSurfaceContextImpl(resourceModel, false);
        }

        private async void AddWorkSurfaceContextImpl(IContextualResourceModel resourceModel, bool isLoadingWorkspace)
        {
            if (resourceModel == null)
            {
                return;
            }

            //Activates if exists
            var exists = IsInOpeningState(resourceModel) || ActivateWorkSurfaceIfPresent(resourceModel);

            var workSurfaceKey = WorkSurfaceKeyFactory.CreateKey(resourceModel);

            if (exists)
            {
                return;
            }

            _canDebug = false;

            if (!isLoadingWorkspace)
            {
                OpeningWorkflowsHelper.AddWorkflow(workSurfaceKey);
            }

            //This is done for when the app starts up because the item isnt open but it must load it from the server or the user will lose all thier changes
            IWorkspaceItem workspaceItem = _getWorkspaceItemRepository().WorkspaceItems.FirstOrDefault(c => c.ID == resourceModel.ID);
            if(workspaceItem == null)
            {
                await resourceModel.Environment.ResourceRepository.ReloadResourceAsync(resourceModel.ID, resourceModel.ResourceType, ResourceModelEqualityComparer.Current, true);
            }

            // NOTE: only if from server ;)
            if (!isLoadingWorkspace)
            {
                resourceModel.IsWorkflowSaved = true;
            }

            AddWorkspaceItem(resourceModel);
            AddAndActivateWorkSurface(_getWorkSurfaceContextViewModel(resourceModel, _createDesigners) as WorkSurfaceContextViewModel);

            OpeningWorkflowsHelper.RemoveWorkflow(workSurfaceKey);
            _canDebug = true;
        }

        readonly Func<IContextualResourceModel, bool, IWorkSurfaceContextViewModel> _getWorkSurfaceContextViewModel = (resourceModel, createDesigner) =>
            {
                // ReSharper disable ConvertToLambdaExpression
                return WorkSurfaceContextFactory.CreateResourceViewModel(resourceModel, createDesigner);
                // ReSharper restore ConvertToLambdaExpression
            };

        private bool IsInOpeningState(IContextualResourceModel resource)
        {
            WorkSurfaceKey key = WorkSurfaceKeyFactory.CreateKey(resource);
            return OpeningWorkflowsHelper.FetchOpeningKeys().Any(c => WorkSurfaceKeyEqualityComparer.Current.Equals(key, c));
        }

        private void AddAndActivateWorkSurface(WorkSurfaceContextViewModel context)
        {
            if (context != null)
            {
                Items.Add(context);
                ActivateItem(context);
            }
        }

        private void AddWorkSurface(IWorkSurfaceObject obj)
        {
            TypeSwitch.Do(obj, TypeSwitch.Case<IContextualResourceModel>(AddWorkSurfaceContext));
        }

        public void TryRemoveContext(IContextualResourceModel model)
        {
            WorkSurfaceContextViewModel context = FindWorkSurfaceContextViewModel(model);
            if (context != null)
            {
                context.DeleteRequested = true;
                DeactivateItem(context, true);
            }
        }

        /// <summary>
        ///     Saves all open tabs locally and writes the open tabs the to collection of workspace items
        /// </summary>
        public void PersistTabs(bool isStudioShutdown = false)
        {
            SaveWorkspaceItems();
            for(var index = 0; index < Items.Count; index++)
            {
                var ctx = Items[index];
                if (ctx.IsEnvironmentConnected())
                {
                    ctx.Save(true, isStudioShutdown);
                }
                if (index == Items.Count - 1)
                {
                }
            }
        }

        public bool CloseWorkSurfaceContext(WorkSurfaceContextViewModel context, PaneClosingEventArgs e)
        {
            bool remove = true;
            if (context != null)
            {
                if (!context.DeleteRequested)
                {
                    var vm = context.WorkSurfaceViewModel;
                    if (vm != null && vm.WorkSurfaceContext == WorkSurfaceContext.Workflow)
                    {
                        var workflowVm = vm as IWorkflowDesignerViewModel;
                        if (workflowVm != null)
                        {
                            IContextualResourceModel resource = workflowVm.ResourceModel;
                            if (resource != null)
                            {
                                remove = !resource.IsAuthorized(AuthorizationContext.Contribute) || resource.IsWorkflowSaved;

                                if (!remove)
                                {
                                    remove = ShowRemovePopup(workflowVm);
                                }

                                if (remove)
                                {
                                    if (resource.IsNewWorkflow)
                                    {
                                        NewWorkflowNames.Instance.Remove(resource.ResourceName);
                                    }
                                    RemoveWorkspaceItem(workflowVm);
                                    Items.Remove(context);
                                    workflowVm.Dispose();
                                    if (_previousActive != null && _previousActive.WorkSurfaceViewModel == vm)
                                        _previousActive = null;
                                    Dev2Logger.Log.Info("Publish message of type - " + typeof(TabClosedMessage));
                                    EventPublisher.Publish(new TabClosedMessage(context));
                                    if (e != null)
                                    {
                                        e.Cancel = true;
                                    }
                                }
                                else if (e != null)
                                {
                                    e.Handled = true;
                                    e.Cancel = false;
                                }
                            }
                        }
                    }
                    else if (vm != null && vm.WorkSurfaceContext == WorkSurfaceContext.Settings)
                    {
                        var settingsViewModel = vm as SettingsViewModel;
                        if (settingsViewModel != null)
                        {
                            remove = settingsViewModel.DoDeactivate(true);
                            if (remove)
                            {
                                settingsViewModel.Dispose();
                            }
                        }
                    }
                    else if (vm != null && vm.WorkSurfaceContext == WorkSurfaceContext.Scheduler)
                    {
                        var schedulerViewModel = vm as SchedulerViewModel;
                        if (schedulerViewModel != null)
                        {
                            remove = schedulerViewModel.DoDeactivate(true);
                            if (remove)
                            {
                                schedulerViewModel.Dispose();
                            }
                        }
                    }
                    else
                    {
                        var tab = vm as IStudioTab;
                        if(tab != null)
                        {
                            remove = tab.DoDeactivate(true);
                            if (remove)
                            {
                                tab.Dispose();
                            }
                        }
                    }
                }
            }

            return remove;
        }

        public bool OnStudioClosing()
        {
            List<WorkSurfaceContextViewModel> workSurfaceContextViewModels = Items.ToList();
            foreach (WorkSurfaceContextViewModel workSurfaceContextViewModel in workSurfaceContextViewModels)
            {
                var vm = workSurfaceContextViewModel.WorkSurfaceViewModel;
                if (vm != null)
                {
                    if (vm.WorkSurfaceContext == WorkSurfaceContext.Settings)
                    {
                        var settingsViewModel = vm as SettingsViewModel;
                        if (settingsViewModel != null && settingsViewModel.IsDirty)
                        {
                            ActivateItem(workSurfaceContextViewModel);
                            bool remove = settingsViewModel.DoDeactivate(true);
                            if (!remove)
                            {
                                return false;
                            }
                        }
                    }
                    else if (vm.WorkSurfaceContext == WorkSurfaceContext.Scheduler)
                    {
                        var schedulerViewModel = vm as SchedulerViewModel;
                        if (schedulerViewModel != null && schedulerViewModel.SelectedTask != null && schedulerViewModel.SelectedTask.IsDirty)
                        {
                            ActivateItem(workSurfaceContextViewModel);
                            bool remove = schedulerViewModel.DoDeactivate(true);
                            if (!remove)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            CloseRemoteConnections();
            return true;
        }

        private void CloseRemoteConnections()
        {
            var connected = EnvironmentRepository.All().Where(a => a.IsConnected);
            foreach (var environmentModel in connected)
            {
                environmentModel.Disconnect();
            }
        }

        #endregion

        //public void Handle(FileChooserMessage message)
        //{
        //    RootWebSite.ShowFileChooser(ActiveEnvironment, message);
        //}

        public Func<bool> IsBusyDownloadingInstaller;
        Func<DropBoxViewWindow, DropBoxSourceViewModel, bool?> _showDropAction;
        Action<IContextualResourceModel, IEnvironmentModel, string, string> _showSaveDialog;
        IDropboxFactory _dropboxFactory;
        IMenuViewModel _menuViewModel;

        public bool IsDownloading()
        {
            return IsBusyDownloadingInstaller != null && IsBusyDownloadingInstaller();
        }

        #region Implementation of IHandle<DisplayMessageBoxMessage>

        public void Handle(DisplayMessageBoxMessage message)
        {
            PopupProvider.Show(message.Message, message.Heading, MessageBoxButton.OK, message.MessageBoxImage, "");
        }

        #endregion

        public async Task<bool> CheckForNewVersion()
        {
            var hasNewVersion = await Version.GetNewerVersionAsync();
            return hasNewVersion;
        }

        public async void DisplayDialogForNewVersion()
        {
            var hasNewVersion = await CheckForNewVersion();
            if (hasNewVersion)
            {
                var dialog = new WebLatestVersionDialog();
                dialog.ShowDialog();
            }
        }


        public bool MenuExpanded
        {
            get
            {
                return _menuExpanded;
            }
            set
            {
                _menuExpanded = value;
                NotifyOfPropertyChange(() => MenuExpanded);
            }
        }
        public IMenuViewModel MenuViewModel
        {
            get
            {
                return _menuViewModel ?? (_menuViewModel = new MenuViewModel(this));
            }
        }
        public IToolboxViewModel ToolboxViewModel
        {
            get
            {
                var toolboxViewModel = CustomContainer.Get<IToolboxViewModel>();
                return toolboxViewModel;
            }
        }
        public IHelpWindowViewModel HelpViewModel
        {
            get
            {
                var helpViewModel = CustomContainer.Get<IHelpWindowViewModel>();
                return helpViewModel;
            }
        }

        #region Implementation of IHandle<FileChooserMessage>

        public void Handle(FileChooserMessage message)
        {
            var emailAttachmentView = new ManageEmailAttachmentView();

            emailAttachmentView.ShowView(message.SelectedFiles.ToList());
            var emailAttachmentVm = emailAttachmentView.DataContext as EmailAttachmentVm;
            if(emailAttachmentVm != null && emailAttachmentVm.Result == MessageBoxResult.OK)
            {
                message.SelectedFiles = emailAttachmentVm.GetAttachments();
            }
        }

        #endregion
    }
}
