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
        private bool _dropBefore;
        private bool _dropAfter;
        private XamDataTreeNode parent;

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

	    void DragSource_OnDragOver(object sender, DragDropMoveEventArgs e)
	    {
            var drop = Utilities.GetAncestorFromType(e.DropTarget, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;
            var drag = Utilities.GetAncestorFromType(e.DragSource, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;

            if (drag != null && (drop != null && drag.Node.Manager.ParentNode != null && drop.Node.Manager.ParentNode != null))
            {
                var dragType = drag.Node.Data.GetType();
                var dropType = drop.Node.Data.GetType();
                var dropParentNodeManager = drop.Node.Manager.ParentNode.Data as IExplorerItemViewModel;

                if (dropParentNodeManager != null && (dragType == dropType && dropParentNodeManager.CanDrop))
                {
                    parent = drag.Node.Manager.ParentNode;

                    if (e.GetPosition(e.DropTarget).Y < drop.ActualHeight / 2)
                    {
                        var destination = drop.Node.Data as IExplorerItemViewModel;
                        if (destination != null && !destination.CanDrop && !dropParentNodeManager.CanDrop)
                        {
                            ((Grid)Utilities.GetDescendantFromName(drop, "DropBeforeElem")).Visibility = Visibility.Collapsed;
                            ((Grid)Utilities.GetDescendantFromName(drop, "DropAfterElem")).Visibility = Visibility.Collapsed;
                            _dropBefore = false;
                            _dropAfter = false;
                            return;
                        }
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropBeforeElem")).Visibility = Visibility.Visible;
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropAfterElem")).Visibility = Visibility.Collapsed;
                        ((Grid)Utilities.GetDescendantFromName(drop, "main")).AllowDrop = true;
                        _dropBefore = true;
                        _dropAfter = false;

                    }
                    else
                    {
                        var destination = drop.Node.Data as IExplorerItemViewModel;
                        if (destination != null && !destination.CanDrop && !dropParentNodeManager.CanDrop)
                        {
                            ((Grid)Utilities.GetDescendantFromName(drop, "DropAfterElem")).Visibility = Visibility.Collapsed;
                            ((Grid)Utilities.GetDescendantFromName(drop, "DropBeforeElem")).Visibility = Visibility.Collapsed;
                            _dropBefore = false;
                            _dropAfter = false;
                            return;
                        }
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropAfterElem")).Visibility = Visibility.Visible;
                        ((Grid)Utilities.GetDescendantFromName(drop, "DropBeforeElem")).Visibility = Visibility.Collapsed;
                        ((Grid)Utilities.GetDescendantFromName(drop, "main")).AllowDrop = true;
                        _dropBefore = false;
                        _dropAfter = true;
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
                var destination = drop.Node.Data as IExplorerItemViewModel;
                var source = drag.Node.Data as IExplorerItemViewModel;
                if (source != null && destination != null)
                {
                    if (!destination.CanDrop || !source.CanDrag)
                    {
                        return;
                    }
                    if (!source.Move(destination))
                    {
                        //DO NOTHING
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
                _dropBefore = false;
                _dropAfter = false;
                parent = null;
            }
        }
	}
}