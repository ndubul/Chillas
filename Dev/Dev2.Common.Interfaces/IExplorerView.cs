using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;

namespace Dev2.Common.Interfaces
{
    public interface IExplorerView : IView
    {
        IEnvironmentViewModel OpenEnvironmentNode(string nodeName);
        List<IExplorerTreeItem> GetFoldersVisible();
        IExplorerTreeItem OpenFolderNode(string folderName);
        int GetVisibleChildrenCount(string folderName);
        void PerformFolderRename(string originalFolderName, string newFolderName);
        void PerformSearch(string searchTerm);
        void AddNewFolder(string folder, string server);
        void VerifyItemExists(string path);
        void DeletePath(string path);
        void AddNewFolderFromPath(string path);
        void AddNewResource(string path, string itemType);
        void AddResources(int resourceNumber, string path, string type, string name);
        int GetResourcesVisible (string path);

        void VerifyItemDoesNotExist(string path);

        void Refresh();

        IExplorerTreeItem OpenItem(string resourceName, string folderName);

        void Move(string originalPath, string destinationPath);
    }
}
