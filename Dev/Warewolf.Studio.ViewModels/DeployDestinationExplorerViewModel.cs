using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;

namespace Warewolf.Studio.ViewModels
{
    public class DeployDestinationExplorerViewModel :ExplorerViewModelBase, IDeploySourceExplorerViewModel {
        readonly IEnvironmentViewModel _environmentViewModel;
        IExplorerTreeItem _selectedItems;

        #region Implementation of IDeployDestinationExplorerViewModel


        public DeployDestinationExplorerViewModel(IEnvironmentViewModel environmentViewModel)
        {
            _environmentViewModel = environmentViewModel;
            environmentViewModel.SetPropertiesForDialog();
            UpdateItemForDeploy();
            Environments = new ObservableCollection<IEnvironmentViewModel>
            {
                environmentViewModel
            };

            IsRefreshing = false;
            ShowConnectControl = false;
        }

        private void UpdateItemForDeploy()
        {
            _environmentViewModel.AsList().Apply(a=>
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
            });
        }

        public ICollection<IExplorerTreeItem> SelectedItems
        {
            get
            {
                return  SelectedEnvironment!= null ?  SelectedEnvironment.AsList().Select(a=> a as IExplorerTreeItem).Where(a=>a.IsSelected).ToList():new List<IExplorerTreeItem>();
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