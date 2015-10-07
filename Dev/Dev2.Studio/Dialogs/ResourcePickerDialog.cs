
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
using System.Threading.Tasks;
using Caliburn.Micro;
using Dev2.AppResources.Repositories;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Infrastructure;
using Dev2.Common.Interfaces.Threading;
using Dev2.ConnectionHelpers;
using Dev2.Services.Events;
using Dev2.Studio.Core;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.ViewModels.Base;
using Dev2.Studio.Enums;
using Dev2.Studio.ViewModels.Workflow;
using Dev2.Studio.Views.Workflow;
using Dev2.Threading;
using Warewolf.Studio.AntiCorruptionLayer;
using Warewolf.Studio.ViewModels;

namespace Dev2.Dialogs
{
    // PBI 10652 - 2013.11.04 - TWR - Refactored from WorkflowDesignerViewModel.ViewPreviewDrop to enable re-use!

    public class ResourcePickerDialog : IResourcePickerDialog
    {
        readonly enDsfActivityType _activityType;

        public IExplorerViewModel SingleEnvironmentExplorerViewModel{get; private set;}
        IEnvironmentModel _environmentModel;
        IStudioResourceRepository _studio;
        IExplorerTreeItem _selectedResource;

        /// <summary>
        /// Creates a picker suitable for dropping from the toolbox.
        /// </summary>
        /// //todo:fix ctor for testing
        public ResourcePickerDialog(enDsfActivityType activityType)
            : this(activityType, null, EventPublishers.Aggregator, new AsyncWorker(), true, StudioResourceRepository.Instance, ConnectControlSingleton.Instance)
        {
        }

        /// <summary>
        /// Creates a picker suitable for picking from the given environment.
        /// </summary>
        public ResourcePickerDialog(enDsfActivityType activityType, IEnvironmentViewModel source)
            : this(activityType, source, EventPublishers.Aggregator, new AsyncWorker(), false, StudioResourceRepository.Instance, ConnectControlSingleton.Instance)
        {
        }

        public ResourcePickerDialog(enDsfActivityType activityType, IEnvironmentViewModel environmentViewModel, IEventAggregator eventPublisher, IAsyncWorker asyncWorker, bool isFromDrop, IStudioResourceRepository studioResourceRepository, IConnectControlSingleton connectControlSingleton)
        {
            VerifyArgument.IsNotNull("environmentRepository", environmentViewModel);
            VerifyArgument.IsNotNull("eventPublisher", eventPublisher);
            VerifyArgument.IsNotNull("asyncWorker", asyncWorker);
            VerifyArgument.IsNotNull("connectControlSingleton", connectControlSingleton);
            _studio = studioResourceRepository;

            SingleEnvironmentExplorerViewModel = new SingleEnvironmentExplorerViewModel(environmentViewModel, Guid.Empty);
            SingleEnvironmentExplorerViewModel.SelectedItemChanged += (sender, item) => { SelectedResource = item; };
            _activityType = activityType;
        }

        public static Task<IResourcePickerDialog> CreateAsync(enDsfActivityType activityType, IEnvironmentViewModel environmentViewModel, IEventAggregator eventPublisher, IAsyncWorker asyncWorker, bool isFromDrop, IStudioResourceRepository studioResourceRepository, IConnectControlSingleton connectControlSingleton)
        {
            var ret = new ResourcePickerDialog(activityType,environmentViewModel,eventPublisher,asyncWorker,isFromDrop,studioResourceRepository,connectControlSingleton);
            return ret.InitializeAsync(environmentViewModel);
        }
        public static Task<IResourcePickerDialog> CreateAsync(enDsfActivityType activityType, IEnvironmentViewModel source)
        {
            var ret = new ResourcePickerDialog(activityType, source, EventPublishers.Aggregator, new AsyncWorker(), false, StudioResourceRepository.Instance, ConnectControlSingleton.Instance);
            return ret.InitializeAsync(source);
        }

        protected  async Task<IResourcePickerDialog> InitializeAsync(IEnvironmentViewModel environmentViewModel)
        {
            await environmentViewModel.Connect();

            await environmentViewModel.LoadDialog("");
            switch(_activityType)
            {
                case enDsfActivityType.Workflow :
                    environmentViewModel.Filter(a => a.ResourceType == ResourceType.Folder || a.ResourceType == ResourceType.WorkflowService);
                    break;
                case enDsfActivityType.Source:
                    environmentViewModel.Filter(a => a.ResourceType == ResourceType.Folder || (a.ResourceType >= ResourceType.DbSource && a.ResourceType < ResourceType.Folder));
                    break;
                case enDsfActivityType.Service:
                    environmentViewModel.Filter(a => a.ResourceType == ResourceType.Folder || (a.ResourceType >= ResourceType.WorkflowService && a.ResourceType <= ResourceType.DbService));
                    break;
                
            }
            environmentViewModel.SelectAction = a => SelectedResource = a;
            return this;
        }

        public IExplorerTreeItem SelectedResource
        {
            get
            {
                return _selectedResource;
            }
            set
            {
                _selectedResource = value;
            }
        }

        public bool ShowDialog(IEnvironmentModel environmentModel = null)
        {
            DsfActivityDropViewModel dropViewModel;
            _environmentModel = environmentModel;
            return ShowDialog(out dropViewModel);
        }

        public void SelectResource(Guid id)
        {
           SingleEnvironmentExplorerViewModel.SelectItem(id);
        }

        public bool ShowDialog(out DsfActivityDropViewModel dropViewModel)
        {
            //if(SingleEnvironmentExplorerViewModel != null)
            //todo:expand
            
            dropViewModel = new DsfActivityDropViewModel(SingleEnvironmentExplorerViewModel, _activityType);
         
            var selected = SelectedResource;
            if (SelectedResource != null && selected != null)
            {
              
               // environmentModel.ResourceRepository.FindSingle(c => c.ID == resourceId, true) as IContextualResourceModel;
                dropViewModel.SelectedResourceModel = _environmentModel.ResourceRepository.FindSingle(c => c.ID == selected.ResourceId, true) as IContextualResourceModel;
    
            }
            var dropWindow = CreateDialog(dropViewModel);
            dropWindow.ShowDialog();
            if(dropViewModel.DialogResult == ViewModelDialogResults.Okay)
            {
                DsfActivityDropViewModel model = dropViewModel;
                SelectedResource = model.SelectedExplorerItemModel;
                return true;
            }
            SelectedResource = null;
            return false;
        }

        protected virtual IDialog CreateDialog(DsfActivityDropViewModel dataContext)
        {
            return new DsfActivityDropWindow { DataContext = dataContext };
        }

        public static enDsfActivityType DetermineDropActivityType(string typeName)
        {
            VerifyArgument.IsNotNull("typeName", typeName);

            if(typeName.Contains(GlobalConstants.ResourcePickerWorkflowString))
            {
                return enDsfActivityType.Workflow;
            }

            if(typeName.Contains(GlobalConstants.ResourcePickerServiceString))
            {
                return enDsfActivityType.Service;
            }

            return enDsfActivityType.All;
        }

        public static async Task<bool> ShowDropDialog(string typeName)

        {
            var activityType = DetermineDropActivityType(typeName);
            var environment = EnvironmentRepository.Instance.ActiveEnvironment;

            IServer server = new Server(environment);
            if (server.Permissions == null)
            {
                server.Permissions = new List<IWindowsGroupPermission>();
                server.Permissions.AddRange(environment.AuthorizationService.SecurityService.Permissions);
            }
            var env = new EnvironmentViewModel(server, CustomContainer.Get<IShellViewModel>(), true);
            var a = await ResourcePickerDialog.CreateAsync(activityType, env);
        
            var picker = a as ResourcePickerDialog;
            if(picker != null)
            {
                
                picker.ShowDialog();
            }
            else
            {
                throw new Exception("invalid resource picker");
            }
            return false;
        }
    }
}
