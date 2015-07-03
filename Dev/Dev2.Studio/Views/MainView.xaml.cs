
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Dev2.Studio.ViewModels;
using Dev2.Studio.ViewModels.Workflow;
using FontAwesome.WPF;
using Infragistics.Windows.DockManager;
using Infragistics.Windows.DockManager.Events;
using WinInterop = System.Windows.Interop;

// ReSharper disable CheckNamespace
namespace Dev2.Studio.Views
{
    public partial class MainView : IWin32Window
    {
        ContentPane _contentPane;
        private static bool _isSuperMaximising;
        private bool _isLocked;

        #region Constructor

        public MainView()
        {
            InitializeComponent();
            _isSuperMaximising = false;
            _isLocked = true;
            Loaded += OnLoaded;
            KeyDown += Shell_KeyDown;
            SourceInitialized += WinSourceInitialized;
        }

        #endregion Constructor

        void WinSourceInitialized(object sender, EventArgs e)
        {
            Maximise();
        }

        private void Maximise()
        {
            var handle = (new WinInterop.WindowInteropHelper(this)).Handle;
            var handleSource = WinInterop.HwndSource.FromHwnd(handle);
            if (handleSource == null)
                return;
            handleSource.AddHook(WindowProc);
        }

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                    if (!_isSuperMaximising)
                    {
                        WmGetMinMaxInfo(hwnd, lParam);
                        handled = true;
                    }
                    break;
            }

            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {

            var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            var currentScreen = Screen.FromHandle(hwnd);
            var workArea = currentScreen.WorkingArea;
            var monitorArea = currentScreen.Bounds;
            mmi.ptMaxPosition.x = Math.Abs(workArea.Left - monitorArea.Left);
            mmi.ptMaxPosition.y = Math.Abs(workArea.Top - monitorArea.Top);
            mmi.ptMaxSize.x = Math.Abs(workArea.Right - workArea.Left);
            mmi.ptMaxSize.y = Math.Abs(workArea.Bottom - workArea.Top);

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable InconsistentNaming
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;

            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private void Shell_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Home && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                
            }
            if (e.Key == Key.G && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                
            }
            if (e.Key == Key.I && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                
            }
            if (e.Key == Key.F11)
            {
                if (_isSuperMaximising)
                {
                    ExitSuperMaximisedMode();
                }
                else
                {
                    EnterSuperMaximisedMode();
                }
            }
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Dev2SplashScreen.Close(TimeSpan.FromSeconds(0.3));
        }

        //public void ClearToolboxSelection()
        //{
        //    if (Toolboxcontrol != null)
        //    {
        //        Toolboxcontrol.ClearSelection();
        //    }
        //}

        public void ClearToolboxSearch()
        {
            //if (Toolboxcontrol != null)
            //{
            //    Toolboxcontrol.ClearSearch();
            //}
        }

        #region Implementation of IWin32Window

        public IntPtr Handle
        {
            get
            {
                var interopHelper = new WinInterop.WindowInteropHelper(this);
                return interopHelper.Handle;
            }
        }

        #endregion

        void MainView_OnClosing(object sender, CancelEventArgs e)
        {
            MainViewModel mainViewModel = DataContext as MainViewModel;
            if (mainViewModel != null)
            {
                if (!mainViewModel.OnStudioClosing())
                {
                    e.Cancel = true;
                }

                if (mainViewModel.IsDownloading())
                {
                    e.Cancel = true;
                }
            }
        }



        public void DockManager_OnPaneDragEnded_(object sender, PaneDragEndedEventArgs e)
        {
            var contentPane = e.Panes[0];
            _contentPane = contentPane;
            UpdatePane(contentPane);
            var windows = System.Windows.Application.Current.Windows;
            foreach (var window in windows)
            {
                var actuallWindow = window as Window;
                if (actuallWindow != null)
                {
                    var windowType = actuallWindow.GetType();
                    if (windowType.FullName == "Infragistics.Windows.Controls.ToolWindowHostWindow")
                    {
                       actuallWindow.Activated -= ActuallWindowOnActivated;
                       actuallWindow.Activated += ActuallWindowOnActivated;
                    }
                }
            }
        }

        public void UpdatePane(ContentPane contentPane)
        {
            if(contentPane == null)
                throw new ArgumentNullException("contentPane");


            WorkflowDesignerViewModel workflowDesignerViewModel = contentPane.TabHeader as WorkflowDesignerViewModel;
            if (workflowDesignerViewModel != null && contentPane.ContentVisibility == Visibility.Visible)
                        {

                            contentPane.CloseButtonVisibility = Visibility.Visible;
                        }


            
        }

        //void ContentPaneOnPreviewDragEnter(object sender, System.Windows.DragEventArgs dragEventArgs)
        //{
        //    dragEventArgs.Handled = true;
        //}

        void ActuallWindowOnActivated(object sender, EventArgs eventArgs)
        {
            MainViewModel mainViewModel = DataContext as MainViewModel;
            if (mainViewModel != null && _contentPane != null)
            {
                WorkflowDesignerViewModel workflowDesignerViewModel = _contentPane.TabHeader as WorkflowDesignerViewModel;
                if (workflowDesignerViewModel != null && _contentPane.ContentVisibility == Visibility.Visible)
                {
                    mainViewModel.AddWorkSurfaceContext(workflowDesignerViewModel.ResourceModel);
                }

            }

        }


        private void SlidingMenuPane_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                //vm.MenuPanelWidth = e.NewSize.Width;
            }
        }

        private void DockManager_OnToolWindowLoaded(object sender, PaneToolWindowEventArgs e)
        {
            var resourceDictionary = System.Windows.Application.Current.Resources;
            var style = resourceDictionary["WarewolfToolWindow"] as Style;
            if (style != null)
            {
                var window = e.Window;
                window.UseOSNonClientArea = false;
                window.Style = style;
            }
        }

        private void EnterSuperMaximisedMode()
        {
            _isSuperMaximising = true;
            var dependencyObject = GetTemplateChild("PART_TITLEBAR");
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(VisibilityProperty, Visibility.Collapsed);
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
            }
        }

        private void CloseSuperMaximised(object sender, RoutedEventArgs e)
        {
            ExitSuperMaximisedMode();
        }

        private void ExitSuperMaximisedMode()
        {
            DoCloseExitFullScreenPanelAnimation();
            _isSuperMaximising = false;
            var dependencyObject = GetTemplateChild("PART_TITLEBAR");
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(VisibilityProperty, Visibility.Visible);
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
            }
        }

        private void DoCloseExitFullScreenPanelAnimation()
        {
            var storyboard = Resources["AnimateExitFullScreenPanelClose"] as Storyboard;
            if (storyboard != null)
            {
                storyboard.Begin();
            }
        }

        private void ShowFullScreenPanel_OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isSuperMaximising)
            {
                var storyboard = Resources["AnimateExitFullScreenPanelOpen"] as Storyboard;
                if (storyboard != null)
                {
                    storyboard.Begin();
                }
            }
            if (!_isLocked)
            {
                DoAnimateOpenTitleBar();
            }
        }

        private void DoAnimateOpenTitleBar()
        {
            var storyboard = Resources["AnimateOpenTitleBorder"] as Storyboard;
            if (storyboard != null)
            {
                var titleBar = GetTemplateChild("PART_TITLEBAR");
                storyboard.SetValue(Storyboard.TargetProperty, titleBar);
                storyboard.Begin();
            }
        }

        private void HideFullScreenPanel_OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isSuperMaximising)
            {
                DoCloseExitFullScreenPanelAnimation();
            }
            if (!_isLocked)
            {
                DoAnimateCloseTitle();
            }
        }

        private void DoAnimateCloseTitle()
        {
            var storyboard = Resources["AnimateCloseTitleBorder"] as Storyboard;
            if (storyboard != null)
            {
                var titleBar = GetTemplateChild("PART_TITLEBAR");
                storyboard.SetValue(Storyboard.TargetProperty, titleBar);
                storyboard.Begin();
            }
        }


        //void DockManager_OnPaneDragOver(object sender, PaneDragOverEventArgs e)
        //{
        //    if (e.DragAction.GetType() != typeof(MoveWindowAction))
        //    {
        //        var contentPane = e.Panes[0];
        //        MainViewModel mainViewModel = DataContext as MainViewModel;
        //        if (mainViewModel != null && contentPane != null)
        //        {
        //            var windows = Application.Current.Windows;
        //            foreach (var window in windows)
        //            {
        //                var actuallWindow = window as Window;
        //                if (actuallWindow != null)
        //                {
        //                    var windowType = actuallWindow.GetType();
        //                    if (windowType.FullName == "Infragistics.Windows.Controls.ToolWindowHostWindow")
        //                    {
        //                        WorkflowDesignerViewModel workflowDesignerViewModel = contentPane.TabHeader as WorkflowDesignerViewModel;
        //                        if (workflowDesignerViewModel != null && contentPane.ContentVisibility == Visibility.Visible)
        //                        {


        //                            PaneDragAction paneDragAction = e.DragAction;
        //                            if (paneDragAction is AddToGroupAction || paneDragAction is NewSplitPaneAction || paneDragAction is NewTabGroupAction)
        //                            {
        //                                e.IsValidDragAction = false;
        //                                e.Cursor = Cursors.No;
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void ContentControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        //void DockManager_OnPaneDragStarting(object sender, PaneDragStartingEventArgs e)
        //{
        //    _contentPane = e.Panes[0];
        //}

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount == 2)
            {
                ToggleWindowState();
            }
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        private void ToggleWindowState()
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void PART_SUPER_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            EnterSuperMaximisedMode();
        }

        private void PART_LOCK_Click(object sender, RoutedEventArgs e)
        {
            var dependencyObject = GetTemplateChild("PART_LOCK");
            if (dependencyObject != null)
            {
                var fontAwesome = new FontAwesome.WPF.FontAwesome();
                if (_isLocked)
                {
                    fontAwesome.Icon = FontAwesomeIcon.Unlock;
                    DoAnimateCloseTitle();
                }
                else
                {
                    fontAwesome.Icon = FontAwesomeIcon.Lock;
                }
                dependencyObject.SetValue(ContentProperty, fontAwesome);
                _isLocked = !_isLocked;
            }
        }
    }
}
