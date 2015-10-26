using System.Activities.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Services.Security;
using Dev2.Studio.Core;
using Infragistics.Controls.Menus;
using Infragistics.DragDrop;
using Infragistics.Windows;
using Warewolf.Studio.ViewModels;

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
        DataObject _dragData;
        IServer _selectedServer;

        public ExplorerView()
        {
            InitializeComponent();
            _explorerViewTestClass = new ExplorerViewTestClass(this);
        }

        public ExplorerViewTestClass ExplorerViewTestClass
        {
            get { return _explorerViewTestClass; }
        }
        public IServer SelectedServer
        {
            get
            {
                return ConnectControl.SelectedServer;
            }
            set
            {
                _selectedServer = value;
            }
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

        public IExplorerTreeItem OpenItem(string resourceName, string folderName)
        {
            return ExplorerViewTestClass.OpenItem(resourceName, folderName);
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
            if (xamDataTreeNode == null)
            {
                return;
            }
            var dataItem = xamDataTreeNode.Data as IExplorerItemViewModel;
            if (dataItem == null)
            {
                return;
            }
            if (!dataItem.IsRenaming)
            {
                return;
            }
            if (dataItem.ResourceName.StartsWith("New Folder"))
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
            Cursor grabCursor = Application.Current.TryFindResource("CursorGrab") as Cursor;
            Cursor grabbingCursor = Application.Current.TryFindResource("CursorGrabbing") as Cursor;

            Mouse.SetCursor(grabbingCursor);
            if (drag != null && (drop != null && drag.Node.Manager.ParentNode != null && drop.Node.Manager.ParentNode != null))
            {
                var dragType = drag.Node.Data.GetType();
                var dropType = drop.Node.Data.GetType();
                var destination = drop.Node.Data as IExplorerItemViewModel;

                if (destination != null && (dragType == dropType && destination.CanDrop))
                {
                    parent = drag.Node.Manager.ParentNode;

                    if (e.GetPosition(e.DropTarget).Y < drop.ActualHeight / 2)
                    {
                        if (!destination.CanDrop && !destination.CanDrop)
                        {
                            Mouse.SetCursor(Cursors.No);
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
                        if (!destination.CanDrop && !destination.CanDrop)
                        {
                            Mouse.SetCursor(Cursors.No);
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
                else
                {
                    Mouse.SetCursor(Cursors.No);
                }
            }
            else
            {
                Mouse.SetCursor(grabbingCursor);
                var dropActivity = Utilities.GetAncestorFromType(e.DropTarget, typeof(ContentControl), false) as ContentControl;
                var dragTool = Utilities.GetAncestorFromType(e.DragSource, typeof(XamDataTreeNodeControl), false) as XamDataTreeNodeControl;

                if (dropActivity != null)
                {
                    var dragData = new DataObject();
                    if (dragTool != null)
                    {
                        var context = dragTool.DataContext as XamDataTreeNodeDataContext;
                        if (context != null)
                        {
                            var dataContext = context.Data as ExplorerItemViewModel;

                            if (dataContext != null)
                            {
                                dragData.SetData(DragDropHelper.WorkflowItemTypeNameFormat, dataContext.ActivityName);
                                dragData.SetData(dataContext);
                            }
                            _dragData = dragData;
                            DragDrop.DoDragDrop(e.DragSource, dragData, DragDropEffects.Copy);
                            DragDropManager.EndDrag(true);
                        }
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

            var exp = DataContext as ExplorerViewModelBase;

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
                    IEnvironmentViewModel vm = GetEnv(source);
                    vm.IsConnecting = true;
                    if (destination.Children.Count >= 1)
                    {
                        var checkExists = destination.Children.FirstOrDefault(o => o.ResourceId == source.ResourceId);
                        if (checkExists == null)
                        {
                            if(exp != null)
                            {
                                exp.AllowDrag = false;
                            }
                                var moved = source.Move(destination).ContinueWith(async=>
                                {
                                    vm.IsConnecting = false;
                                    if(exp != null)
                                    {
                                        exp.AllowDrag = true;
                                    }
                                });
                            }
                        
                    }
                    else
                    {
                        var moved = source.Move(destination).ContinueWith(async =>
                        {
                            vm.IsConnecting = false;
                            if (exp != null)
                            {
                                exp.AllowDrag = true;
                            }
                        });
         
                    }
                }

                Reset(drop);
            }
            else
            {
                var target = e.DropTarget as ContentControl;
                if (target != null)
                {
                    DragDrop.DoDragDrop(e.DragSource, _dragData, DragDropEffects.Copy);
                }

                if (drop != null && drag != null)
                {
                    var destination = drop.Node.Data as IEnvironmentViewModel;
                    var source = drag.Node.Data as IExplorerItemViewModel;
                    IEnvironmentViewModel vm = GetEnv(source);
                    vm.IsConnecting = true;
                    if (source != null && destination != null)
                    {
                        if (!source.CanDrag)
                        {
                            return;
                        }
                        if (destination.Children.Count >= 1)
                        {
                            var checkExists = destination.Children.FirstOrDefault(o => o.ResourceId == source.ResourceId);
                            if (checkExists == null)
                            {
                                var moved = source.Move(destination).ContinueWith(async=>
                                {
                                    vm.IsConnecting = false;
                                    if(exp != null)
                                    {
                                        exp.AllowDrag = true;
                                    }
                                });
                          
                            }
                        }
                        else
                        {
                            var moved = source.Move(destination).ContinueWith(async=>
                            {
                                vm.IsConnecting = false;
                                if(exp != null)
                                {
                                    exp.AllowDrag = true;
                                }
                            });

                        }
                    }
                }
            }
        }

        IEnvironmentViewModel GetEnv(IExplorerTreeItem source)
        {
            var x = source;
            var env = source as IEnvironmentViewModel;
            if (env != null)
                return env;
            return GetEnv(x.Parent);
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

        void DragSource_OnDragStart(object sender, DragDropStartEventArgs e)
        {
            var textBlock = e.OriginalDragSource as FrameworkElement;
            if (textBlock != null)
            {
                var context = textBlock.DataContext as XamDataTreeNodeDataContext;
                if (context != null)
                {
                    var dataContext = context.Data as ExplorerItemViewModel;
                    if (dataContext != null)
                    {
                        var dragData = new DataObject();

                        if (dataContext.IsRenaming)
                        {
                            return;
                        }

                        var environmentModel = EnvironmentRepository.Instance.FindSingle(model => model.ID == dataContext.Server.EnvironmentID);
                        bool hasPermissionToDrag = true;
                        if (environmentModel != null && environmentModel.AuthorizationService != null)
                        {
                            bool canExecute = environmentModel.AuthorizationService.IsAuthorized(AuthorizationContext.Execute, dataContext.ResourceId.ToString());
                            bool canView = environmentModel.AuthorizationService.IsAuthorized(AuthorizationContext.View, dataContext.ResourceId.ToString());
                            hasPermissionToDrag = canExecute && canView;
                        }
                        if (hasPermissionToDrag)
                        {
                            if (dataContext.ResourceType <= ResourceType.WebService)
                            {
                                dragData.SetData(DragDropHelper.WorkflowItemTypeNameFormat, dataContext.ActivityName);
                                dragData.SetData(dataContext);
                            }
                            dragData.SetData(dataContext);
                        }
                        Mouse.SetCursor(Application.Current.TryFindResource("CursorGrabbing") as Cursor);
                        _dragData = dragData;
                    }
                }
            }

        }

        private Cursor _customCursor;
        void ExplorerTree_OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Copy)
            {
                if (_customCursor == null)
                    _customCursor = Application.Current.TryFindResource("CursorGrabbing") as Cursor;

                e.UseDefaultCursors = false;
                Mouse.SetCursor(_customCursor);
            }
            else
                e.UseDefaultCursors = true;            
            e.Handled = true;
        }

        void ExplorerTree_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}