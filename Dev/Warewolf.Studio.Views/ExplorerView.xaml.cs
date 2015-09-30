using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Infragistics.Controls.Menus;
using Infragistics.DragDrop;
using Infragistics.Windows;

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

	    void ExplorerTree_OnPreviewDragOver(object sender, DragEventArgs e)
	    {
            var allowedEffects = e.AllowedEffects;
	    }

        private bool dropBefore = false;
        private bool dropAfter = false;
        private XamDataTreeNode parent = null;

	    void DragSource_OnDragOver(object sender, DragDropMoveEventArgs e)
	    {
            var drop = Utilities.GetAncestorFromType(e.DropTarget, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;
            var drag = Utilities.GetAncestorFromType(e.DragSource, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;

            if (drag != null && (drop != null && drag.Node.Manager.ParentNode != null && drop.Node.Manager.ParentNode != null))
            {
                if (drag.Node.Data.GetType() == drop.Node.Data.GetType() && drag.Node.Manager.ParentNode.Data.Equals(drop.Node.Manager.ParentNode.Data))
                {
                    parent = drag.Node.Manager.ParentNode;

                    if (e.GetPosition(e.DropTarget).Y < drop.ActualHeight / 2)
                    {
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropBeforeElem")).Visibility = Visibility.Visible;
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropAfterElem")).Visibility = Visibility.Collapsed;
                        dropBefore = true;
                        dropAfter = false;
                    }
                    else
                    {
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropAfterElem")).Visibility = Visibility.Visible;
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropBeforeElem")).Visibility = Visibility.Collapsed;
                        dropBefore = false;
                        dropAfter = true;
                    }
                }

            }
	    }

	    void DragSource_OnDragLeave(object sender, DragDropEventArgs e)
	    {
            var drop = Utilities.GetAncestorFromType(e.DropTarget, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;
            Reset(drop);
	    }

	    void DragSource_OnDrop(object sender, DropEventArgs e)
	    {
            var drop = Utilities.GetAncestorFromType(e.DropTarget, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;
            var drag = Utilities.GetAncestorFromType(e.DragSource, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;

            if (drag != null && (drop != null && drag.Node.Manager.ParentNode != null && drop.Node.Manager.ParentNode != null))
            {
                int dropIndex = drop.Node.Index;
                if (dropBefore)
                {
                    parent.Nodes.RemoveAt(drag.Node.Index);
                    if (dropIndex == 0)
                    {
                        parent.Nodes.Insert(0, drag.Node);
                    }
                    else
                    {
                        parent.Nodes.Insert(dropIndex, drag.Node);
                    }

                }

                if (dropAfter)
                {
                    parent.Nodes.RemoveAt(drag.Node.Index);
                    if (dropIndex == parent.Nodes.Count)
                    {
                        parent.Nodes.Add(drag.Node);
                    }
                    else
                    {
                        parent.Nodes.Insert(drop.Node.Index + 1, drag.Node);
                    }
                }

                Reset(drop);
            }
	    }
        private void Reset(XamDataTreeNodeControl drop)
        {
            if (drop != null)
            {
                ((Grid)Utilities.GetDescendantFromName(drop, "DropBeforeElem")).Visibility = Visibility.Collapsed;
                ((Grid)Utilities.GetDescendantFromName(drop, "DropAfterElem")).Visibility = Visibility.Collapsed;
                dropBefore = false;
                dropAfter = false;
                parent = null;
            }
        }
	}
}