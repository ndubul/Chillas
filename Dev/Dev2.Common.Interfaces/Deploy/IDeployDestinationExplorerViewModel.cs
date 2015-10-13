using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Dev2.Common.Interfaces.Deploy
{
    public interface IDeployDestinationExplorerViewModel:IExplorerViewModel    
    {

        ICollection<IExplorerTreeItem> SelectedItems { get; }
        /// <summary>
        /// used to select a list of items from the explorer
        /// </summary>
        /// <param name="selectedItems"></param>
        void SelectItemsForDeploy(IEnumerable<IExplorerTreeItem> selectedItems);
    }
}