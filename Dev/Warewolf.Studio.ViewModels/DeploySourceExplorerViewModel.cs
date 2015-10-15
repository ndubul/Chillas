using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Deploy;

namespace Warewolf.Studio.ViewModels
{
    public class DeploySourceExplorerViewModel :ExplorerViewModel, IDeploySourceExplorerViewModel {
        readonly IDeployStatsViewerViewModel _statsArea;

        #region Implementation of IDeployDestinationExplorerViewModel


        public DeploySourceExplorerViewModel(IShellViewModel shellViewModel, Microsoft.Practices.Prism.PubSubEvents.IEventAggregator aggregator, IDeployStatsViewerViewModel statsArea)
            : base(shellViewModel, aggregator,null,false)
        {
            _statsArea = statsArea;
            foreach(var environmentViewModel in _environments)
            {
                
                environmentViewModel.SelectAction = SelectAction;
            }
            if(_environments.Count>0)
            {
                LoadEnvironment(_environments.First(), true);
                SelectedEnvironment = _environments.First();
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

        public override void AfterLoad(Guid environmentID)
        {
            var environmentViewModel = _environments.FirstOrDefault(a=>a.Server.EnvironmentID == environmentID);
            if(environmentViewModel != null)
            {
                UpdateItemForDeploy( environmentViewModel.Server.EnvironmentID);
            }
   
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
                environmentViewModel.AsList().Apply(a=>
                {
                    a.CanExecute = false;
                    a.CanEdit = false;
                    a.ShowContextMenu = false;
                    a.SelectAction = (SelectAction );
                    a.AllowResourceCheck = true;
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
                if (ax.Parent.ResourceType == ResourceType.Folder)
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
            SelectedEnvironment.AsList().Apply(a=>a.IsSelected=selectedItems.Contains(a));
        }

        #endregion


    }
}