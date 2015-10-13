namespace Dev2.Common.Interfaces.Deploy
{
    public interface IDeploySourceExplorerViewModel:IExplorerViewModel
    {
        /// <summary>
        /// root and all children of selected items
        /// </summary>
        IExplorerTreeItem SelectedItems { get; set; }
    }
}