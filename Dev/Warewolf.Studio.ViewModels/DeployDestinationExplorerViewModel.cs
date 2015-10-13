using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;

namespace Warewolf.Studio.ViewModels
{
    public class DeploySourceExplorerViewModel :ExplorerViewModel, IDeploySourceExplorerViewModel {
        readonly IDeployStatsViewerViewModel _statsArea;

        #region Implementation of IDeployDestinationExplorerViewModel


        public DeploySourceExplorerViewModel(IShellViewModel shellViewModel, Microsoft.Practices.Prism.PubSubEvents.IEventAggregator aggregator,IDeployStatsViewerViewModel statsArea):base(shellViewModel,aggregator)
        {
            _statsArea = statsArea;

            if (SelectedEnvironment != null)
            {
                UpdateItemForDeploy();
            }
            IsRefreshing = false;
            ShowConnectControl = false;
            SelectedEnvironmentChanged += DeploySourceExplorerViewModelSelectedEnvironmentChanged;
        }

        void DeploySourceExplorerViewModelSelectedEnvironmentChanged(object sender, IEnvironmentViewModel e)
        {
            UpdateItemForDeploy();
        }

        private void UpdateItemForDeploy()
        {
            SelectedEnvironment.AsList().Apply(a=>
            {
                a.CanDrag = false;
                a.CanRename = false;
                a.CanShowDependencies = false;
                a.CanCreateDbService = false;
                a.CanCreateDbSource = false;
                a.CanCreateDropboxSource = false;
                a.CanCreateEmailSource = false;
                a.CanCreateEmailSource = false;
                a.CanCreateFolder = false;
                a.CanCreatePluginService = false;
                a.CanCreatePluginSource = false;
                a.CanCreateWebService = false;
                a.CanCreateWebSource = false;
                a.CanCreateWorkflowService = false;
                a.SelectAction = (ax => { _statsArea.Calculate(SelectedItems.ToList()); });
            });
        }

        public ICollection<IExplorerTreeItem> SelectedItems
        {
            get
            {
                return  SelectedEnvironment!= null ?  SelectedEnvironment.AsList().Select(a=> a as IExplorerTreeItem).Where(a=>a.IsSelected).ToList():new List<IExplorerTreeItem>();
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

    public class DeployDestinationViewModel: ExplorerViewModelBase, IDeployDestinationExplorerViewModel
    {
        #region Implementation of IDeployDestinationExplorerViewModel

        public ICollection<IExplorerTreeItem> SelectedItems { get; private set; }

        /// <summary>
        /// used to select a list of items from the explorer
        /// </summary>
        /// <param name="selectedItems"></param>
        public void SelectItemsForDeploy(IEnumerable<IExplorerTreeItem> selectedItems)
        {
        }

        #endregion
    }
}