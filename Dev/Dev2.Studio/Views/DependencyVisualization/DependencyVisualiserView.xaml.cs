
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Dev2.AppResources.DependencyVisualization;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Services.Events;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.ViewModels.DependencyVisualization;
using Dev2.Utils;
using Moq;
using Warewolf.Studio.ViewModels;

// ReSharper disable once CheckNamespace
namespace Dev2.Studio.Views.DependencyVisualization
{
    public partial class DependencyVisualiserView
    {
        readonly IEventAggregator _eventPublisher;
        ExplorerItemNodeViewModel _root;

        public DependencyVisualiserView()
            : this(EventPublishers.Aggregator)
        {
        }

        public DependencyVisualiserView(IEventAggregator eventPublisher)
        {
            InitializeComponent();
            VerifyArgument.IsNotNull("eventPublisher", eventPublisher);
            _eventPublisher = eventPublisher;
            SetupNodes(Visibility.Visible);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //2012.10.01: massimo.guerrera - Added for the click through on the dependency viewer
            if (e.ClickCount == 2)
            {
                ReleaseMouseCapture();
                FrameworkElement fe = e.OriginalSource as FrameworkElement;
                FrameworkContentElement fce = e.OriginalSource as FrameworkContentElement;
                object dataContext = null;

                if (fe != null)
                {
                    dataContext = fe.DataContext;
                }
                else if (fce != null)
                {
                    dataContext = fce.DataContext;
                }

                string resourceName = dataContext as string;

                if (string.IsNullOrEmpty(resourceName) && dataContext is Node)
                {
                    resourceName = (dataContext as Node).ID;
                }

                if (!string.IsNullOrEmpty(resourceName))
                {
                    var vm = DataContext as DependencyVisualiserViewModel;
                    if (vm != null)
                    {
                        IResourceModel resource = vm.ResourceModel.Environment.ResourceRepository.FindSingle(c => c.ResourceName == resourceName);
                        if (resource != null)
                        {
                            WorkflowDesignerUtils.EditResource(resource, _eventPublisher);
                        }
                    }
                }
            }

            e.GetPosition(this);
            //_scrollStartOffset.X = myScrollViewer.HorizontalOffset;
            //_scrollStartOffset.Y = myScrollViewer.VerticalOffset;

            // Update the cursor if scrolling is possible 
            //Cursor = (myScrollViewer.ExtentWidth > myScrollViewer.ViewportWidth) ||
            //    (myScrollViewer.ExtentHeight > myScrollViewer.ViewportHeight) ?
            //    Cursors.ScrollAll : Cursors.Arrow;

            CaptureMouse();
            base.OnPreviewMouseDown(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (IsMouseCaptured)
            {
                // Get the new mouse position. 
                Point mouseDragCurrentPoint = e.GetPosition(this);

                // Scroll to the new position. 
                //myScrollViewer.ScrollToHorizontalOffset(mouseDragCurrentPoint.X);
                //myScrollViewer.ScrollToVerticalOffset(mouseDragCurrentPoint.Y);
            }
            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Cursor = Cursors.Arrow;
                ReleaseMouseCapture();
            }
            base.OnPreviewMouseUp(e);
        }

        void SetupNodes(Visibility visibility)
        {
            var server = new Mock<IServer>().Object;
            var parent = new Mock<IExplorerItemViewModel>().Object;

            _root = new ExplorerItemNodeViewModel(server, parent)
            {
                ResourceName = "bob",
                TextVisibility = visibility,
                ResourceType = ResourceType.WorkflowService,
                IsMainNode = Visibility.Visible,
                Children = new ObservableCollection<IExplorerItemViewModel>()
                {
                    new ExplorerItemNodeViewModel(server,parent)
                    {
                        ResourceName = "child 1",
                         ResourceType = ResourceType.PluginService,
                         TextVisibility = visibility,
                        Children = new ObservableCollection<IExplorerItemViewModel>()
                        {
                          new ExplorerItemNodeViewModel(server,parent)
                            {
                                ResourceName = "granchild 1",
                                ResourceType = ResourceType.WorkflowService,
                                TextVisibility = visibility,
                                Children = new ObservableCollection<IExplorerItemViewModel>()
                                {
                    
                                }   
                             },
                           new ExplorerItemNodeViewModel(server,parent)
                            {

                                ResourceName = "granchild 2",
                                ResourceType = ResourceType.WorkflowService,
                                TextVisibility = visibility,
                                Children = new ObservableCollection<IExplorerItemViewModel>()
                                {
                    
                                }   
                             }
                         }
                      },
                    new ExplorerItemNodeViewModel(server,parent)
                    {
                        ResourceName = "child 2",
                         ResourceType = ResourceType.DbService,
                         TextVisibility = visibility,
                        Children = new ObservableCollection<IExplorerItemViewModel>()
                        {
                          new ExplorerItemNodeViewModel(server,parent)
                            {
                                ResourceName = "granchild 1",
                                TextVisibility = visibility,
                                Children = new ObservableCollection<IExplorerItemViewModel>()
                                {
                    
                                }   
                             }
                         }
                      }
                }
            };
            if (Nodes != null)
            {
                Nodes.ItemsSource = new ObservableCollection<ExplorerItemNodeViewModel>() { _root, _root.NodeChildren.First(), _root.NodeChildren.Last(), _root.NodeChildren.First().NodeChildren.First(), _root.NodeChildren.First().NodeChildren.Last() };
            }
        }

        void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            SetupNodes(WfDependsOn.IsChecked == true ? Visibility.Visible : Visibility.Collapsed);
        }

        void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Nodes.UpdateNodeArrangement();
        }
    }
}
