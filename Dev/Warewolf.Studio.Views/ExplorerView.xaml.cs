using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Infragistics.Controls.Menus;

namespace Warewolf.Studio.Views
{
	/// <summary>
	/// Interaction logic for ExplorerView.xaml
	/// </summary>
	public partial class ExplorerView : IExplorerView
	{
	    private readonly ExplorerViewTestClass _explorerViewTestClass;

	    public ExplorerView()
	    {
	        InitializeComponent();
	        _explorerViewTestClass = new ExplorerViewTestClass(this);            
	    }

	    public ExplorerViewTestClass ExplorerViewTestClass
	    {
	        get { return _explorerViewTestClass; }
	    }

        public IEnvironmentViewModel OpenEnvironmentNode(string nodeName)
        {
            return ExplorerViewTestClass.OpenEnvironmentNode(nodeName);
        }

        public List<IExplorerTreeItem> GetFoldersVisible()
	    {
	        return ExplorerViewTestClass.GetFoldersVisible();
	    }

        public IExplorerTreeItem OpenFolderNode(string folderName)
	    {
	        return ExplorerViewTestClass.OpenFolderNode(folderName);
	    }

        public IExplorerTreeItem OpenItem(string resourceName,string folderName)
        {
            return ExplorerViewTestClass.OpenItem(resourceName,folderName);
        }

	    public void Move(string originalPath, string destinationPath)
	    {
            ExplorerViewTestClass.Move(originalPath, destinationPath);
	    }

	    public int GetVisibleChildrenCount(string folderName)
	    {
	        return ExplorerViewTestClass.GetVisibleChildrenCount(folderName);
	    }

	    public void PerformFolderRename(string originalFolderName, string newFolderName)
	    {
	        ExplorerViewTestClass.PerformFolderRename(originalFolderName, newFolderName);
	    }

	    public void PerformSearch(string searchTerm)
	    {
	        ExplorerViewTestClass.PerformSearch(searchTerm);
	    }

	    public void AddNewFolder(string folder, string server)
	    {
            ExplorerViewTestClass.PerformFolderAdd(server, folder);
	    }

	    public void VerifyItemExists(string path)
	    {
            ExplorerViewTestClass.VerifyItemExists(path);
	    }

	    public void DeletePath(string path)
	    {
            ExplorerViewTestClass.DeletePath(path);
	    }

	    public void AddNewFolderFromPath(string path)
	    {
            ExplorerViewTestClass.PerformFolderAdd(path);
	    }

	    public void AddNewResource(string path, string itemType)
	    {
            ExplorerViewTestClass.PerformItemAdd(path);
	    }

	    public void AddResources(int resourceNumber, string path, string type, string name)
	    {
			ExplorerViewTestClass.AddChildren(resourceNumber, path, type, name);
	    }

	    public int GetResourcesVisible(string path)
	    {
            return ExplorerViewTestClass.GetFoldersResourcesVisible(path);
	    }

	    public void VerifyItemDoesNotExist(string path)
	    {
            ExplorerViewTestClass.VerifyItemDoesNotExist(path);
	    }

	    public void Refresh()
	    {
	        ExplorerViewTestClass.Reset();
	    }

	    public void Blur()
	    {
            if (Content != null)
            {
                //Effect = new BlurEffect(){Radius = 10};
                //Background = new SolidColorBrush(Colors.Black);
                Overlay.Visibility = Visibility.Visible;
                Overlay.Opacity = 0.75;
            }
	    }

	    void ExplorerTree_OnNodeDragDrop(object sender, TreeDropEventArgs e)
	    {
            
            var node = e.DragDropEventArgs.Data as XamDataTreeNode;

            if (node != null)
            {
                var dropped = e.DragDropEventArgs.DropTarget as XamDataTreeNodeControl;
                if (dropped != null)
                {
                    var destination = dropped.Node.Data as IExplorerItemViewModel;
                    var source = node.Data as IExplorerItemViewModel;
                    if (source != null && destination != null)
                    {
                        if (!destination.CanDrop || !source.CanDrag)
                        {
                            e.Handled = true;
                            return;
                        }
                        if (!source.Move(destination))
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
            e.Handled = true;
            
	    }

	    /// <summary>
	    /// Attaches events and names to compiled content. 
	    /// </summary>
	    /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
	    public void Connect(int connectionId, object target)
	    {
	    }


	    void ExplorerTree_OnInitializeNode(object sender, InitializeNodeEventArgs e)
	    {
	        var xamDataTreeNode = e.Node;
	        if(xamDataTreeNode == null)
	        {
	            return;
	        }
	        var dataItem = xamDataTreeNode.Data as IExplorerItemViewModel;
	        if(dataItem == null)
	        {
	            return;
	        }
	        if(!dataItem.IsRenaming)
	        {
	            return;
	        }
	        if(dataItem.ResourceName.StartsWith("New Folder"))
	        {
                ExplorerTree.EnterEditMode(xamDataTreeNode);	            
	        }
	    }

	    void UIElement_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
	    {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
	    }



	    void ExplorerTree_OnNodeExitedEditMode(object sender, NodeEventArgs e)
	    {
            var dataItem = e.Node.Data as IExplorerItemViewModel;
            if (dataItem == null)
            {
                return;
            }
            if (dataItem.IsRenaming && dataItem.ResourceName.StartsWith("New Folder"))
            {
                ExplorerTree.EnterEditMode(e.Node);
            }
	    }
	}
}