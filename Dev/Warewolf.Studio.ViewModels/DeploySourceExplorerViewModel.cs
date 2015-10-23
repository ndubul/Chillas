using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Deploy;
using Dev2.Common.Interfaces.Infrastructure;

namespace Warewolf.Studio.ViewModels
{
    public sealed class DeploySourceExplorerViewModel :ExplorerViewModelBase, IDeploySourceExplorerViewModel {
        readonly IDeployStatsViewerViewModel _statsArea;

        #region Implementation of IDeployDestinationExplorerViewModel

        readonly IShellViewModel _shellViewModel;
        readonly Action<IExplorerItemViewModel> _selectAction;
        bool _isLoading;

        public DeploySourceExplorerViewModel(IShellViewModel shellViewModel, Microsoft.Practices.Prism.PubSubEvents.IEventAggregator aggregator, IDeployStatsViewerViewModel statsArea)

        {


            if (shellViewModel == null)
			{
				throw new ArgumentNullException("shellViewModel");
			}
			var localhostEnvironment = CreateEnvironmentFromServer(shellViewModel.LocalhostServer, shellViewModel);
            _shellViewModel = shellViewModel;
	        _selectAction = SelectAction;
            Environments = new ObservableCollection<IEnvironmentViewModel> { localhostEnvironment };
            LoadEnvironment(localhostEnvironment);

			ConnectControlViewModel = new ConnectControlViewModel(shellViewModel.LocalhostServer, aggregator);
			ShowConnectControl = true;
            ConnectControlViewModel.ServerConnected+=ServerConnected;
            ConnectControlViewModel.ServerDisconnected += ServerDisconnected;
            _statsArea = statsArea;
            foreach(var environmentViewModel in _environments)
            {
                
                environmentViewModel.SelectAction = SelectAction;
            }

            if (ConnectControlViewModel.SelectedConnection != null)
            {
                UpdateItemForDeploy(ConnectControlViewModel.SelectedConnection.EnvironmentID);
            }
            IsRefreshing = false;
            ShowConnectControl = false;
           
            ConnectControlViewModel.SelectedEnvironmentChanged += DeploySourceExplorerViewModelSelectedEnvironmentChanged;
            IsDeploy = true;
        }

        void DeploySourceExplorerViewModelSelectedEnvironmentChanged(object sender, Guid environmentId)
        {
            UpdateItemForDeploy(environmentId);
        }

        #region Overrides of ExplorerViewModel

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

        public  void AfterLoad(Guid environmentID)
        {
            var environmentViewModel = _environments.FirstOrDefault(a=>a.Server.EnvironmentID == environmentID);
            if(environmentViewModel != null)
            {
                UpdateItemForDeploy( environmentViewModel.Server.EnvironmentID);
                environmentViewModel.SelectAll = (() => _statsArea.Calculate(environmentViewModel.AsList().Where(o => o.IsResourceChecked == true).Select(x=>x as IExplorerTreeItem).ToList()));
            }
            if(Preselected!=null && Preselected.Any())
            {
                var envId =Preselected.First().Server.EnvironmentID;
              if( envId != environmentID)
              {
                  ConnectControlViewModel.SelectedConnection = ConnectControlViewModel.Servers.FirstOrDefault(a => a.EnvironmentID == envId);
                  if(ConnectControlViewModel.SelectedConnection != null)
                  {
                      var server = ConnectControlViewModel.SelectedConnection;
                      if (server.Permissions == null)
                      {
                          server.Permissions = new List<IWindowsGroupPermission>();
 
                      }
                      ConnectControlViewModel.Connect(ConnectControlViewModel.SelectedConnection);
                  }
              }
              else
              {
                  SelectItemsForDeploy(Preselected);
                  Preselected = null; 
              }

            }
            ConnectControlViewModel.IsLoading = false;
        }

        #endregion

        public override ICollection<IEnvironmentViewModel> Environments
        {
            get
            {
                return new ObservableCollection<IEnvironmentViewModel>(_environments.Where(a => a.IsVisible));
            }
            set
            {
                _environments = value;
                OnPropertyChanged(() => Environments);
            }
        }

        private void UpdateItemForDeploy(Guid environmentId)
        {
            var environmentViewModel = _environments.FirstOrDefault(a=>a.Server.EnvironmentID==environmentId);
            if(environmentViewModel != null)
            {
                environmentViewModel.IsVisible = true;
                SelectedEnvironment = environmentViewModel;
                environmentViewModel.ShowContextMenu = false;
                environmentViewModel.AsList().Apply(a=>
                {
                    a.CanExecute = false;
                    a.CanEdit = false;
                    a.ShowContextMenu = false;
                    a.SelectAction = (SelectAction );
                    a.AllowResourceCheck = true;
                    a.CanDrop = false;
                    a.CanDrag = false;
                });
            }
            SelectedEnvironment.AllowResourceCheck = true;
            foreach (var env in _environments.Where(a => a.Server.EnvironmentID != environmentId))
            {
                env.IsVisible = false;
            }
           OnPropertyChanged(()=>Environments);
        }

        void SelectAction(IExplorerItemViewModel ax)
        {
            if(ax.ResourceType == ResourceType.Folder)
            {
                ax.Children.Apply(ay =>
                {
                    ay.IsResourceChecked = ax.IsResourceChecked;
                });
            }
            else
            {
                if (ax.Parent.ResourceType == ResourceType.Folder || ax.Parent.ResourceType == ResourceType.ServerSource)
                {
                    ax.Parent.IsFolderChecked = ax.IsResourceChecked;
                }
            }

           _statsArea.Calculate(SelectedItems.ToList());
        }

        public ICollection<IExplorerTreeItem> SelectedItems
        {
            get
            {
                return  SelectedEnvironment!= null ?  SelectedEnvironment.AsList().Select(a=> a as IExplorerTreeItem).Where(a=>a.IsResourceChecked.HasValue && a.IsResourceChecked.Value).ToList():new List<IExplorerTreeItem>();
            }
            set
            {
                foreach(var explorerTreeItem in value)
                {
                    Select(explorerTreeItem);
                }
            }
        }
        public IEnumerable<IExplorerTreeItem> Preselected { get; set; }

        void Select(IExplorerTreeItem explorerTreeItem)
        {
            var item= SelectedEnvironment != null ? SelectedEnvironment.AsList().FirstOrDefault(a=>a.ResourceId == explorerTreeItem.ResourceId):null;
           if(item!=null)
           {
               item.IsSelected = true;
           }
        }

        public void SelectItemsForDeploy(IEnumerable selectedItems)
        {
        }

        /// <summary>
        /// used to select a list of items from the explorer
        /// </summary>
        /// <param name="selectedItems"></param>
        public void SelectItemsForDeploy(IEnumerable<IExplorerTreeItem> selectedItems)
        {
            SelectedEnvironment.AsList().Apply(a=>a.IsResourceChecked=selectedItems.Contains(a));
        }

        #endregion



	    async void ServerConnected(object _, IServer server)
	    {
            var environmentModel = CreateEnvironmentFromServer(server, _shellViewModel);
            _environments.Add(environmentModel);
	        await environmentModel.Load(IsDeploy);
            OnPropertyChanged(() => Environments);
            AfterLoad(server.EnvironmentID);
	    }


	    public bool IsDeploy { get; set; }

	    void ServerDisconnected(object _, IServer server)
        {
            var environmentModel = _environments.FirstOrDefault(model => model.Server.EnvironmentID == server.EnvironmentID);
            if (environmentModel!=null)
            {
                _environments.Remove(environmentModel);
            }
            OnPropertyChanged(()=>Environments);
            AfterLoad(server.EnvironmentID);
        }

        async void LoadEnvironment(IEnvironmentViewModel localhostEnvironment,bool isDeploy = false)
		{
			await localhostEnvironment.Connect();
			await localhostEnvironment.Load(isDeploy);
            AfterLoad(localhostEnvironment.Server.EnvironmentID);
		}


		IEnvironmentViewModel CreateEnvironmentFromServer(IServer server, IShellViewModel shellViewModel)
		{
		    if(server != null && server.UpdateRepository != null)
		    {
		        server.UpdateRepository.ItemSaved += Refresh;
		    }
		    return new EnvironmentViewModel(server, shellViewModel,false,_selectAction);
		}

    }


}