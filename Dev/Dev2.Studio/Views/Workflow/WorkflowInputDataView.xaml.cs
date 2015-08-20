
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using System.Xml;
using Dev2.Data.Interfaces;
using Dev2.Studio.ViewModels.Workflow;
using Dev2.UI;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation;
using Infragistics.Controls.Grids;
using Infragistics.Controls.Grids.Primitives;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace Dev2.Studio.Views.Workflow
{
    /// <summary>
    /// Interaction logic for WorkflowInputDataWindow.xaml
    /// </summary>
    public partial class WorkflowInputDataView
    {
        public WorkflowInputDataView()
        {
            InitializeComponent();
            SetUpTextEditor();
            AddBlackOutEffect();
            _currentTab = InputTab.Grid;
        }

        private TextEditor _editor;
        private TextEditor _jsonEditor;
        private AbstractFoldingStrategy _foldingStrategy;
        private FoldingManager _foldingManager;
        DispatcherTimer _foldingUpdateTimer;
        Grid _blackoutGrid;
        InputTab _currentTab;

        private void SetUpTextEditor()
        {
            _editor = new TextEditor { SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML"), ShowLineNumbers = true, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
            _editor.SetValue(AutomationProperties.AutomationIdProperty, "UI_XMLEditor_AutoID");

            _jsonEditor = new TextEditor { SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript"), ShowLineNumbers = true, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
            _jsonEditor.SetValue(AutomationProperties.AutomationIdProperty, "UI_JsonEditor_AutoID");

            _foldingStrategy = new XmlFoldingStrategy();
            _foldingManager = FoldingManager.Install(_editor.TextArea);
            _editor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
            _jsonEditor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
            _foldingUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _foldingUpdateTimer.Tick += OnFoldingUpdateTimerOnTick;
            _foldingUpdateTimer.Start();
        }

        void OnFoldingUpdateTimerOnTick(object sender, EventArgs e)
        {
            if(_foldingStrategy != null && _foldingManager != null)
            {
                if(!String.IsNullOrEmpty(_editor.Document.Text))
                {
                    _foldingStrategy.UpdateFoldings(_foldingManager, _editor.Document);
                }
            }
        }

        private void ShowDataInOutputWindow(string input)
        {
            _editor.Text = input;
            XmlOutput.Content = _editor;
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = e.OriginalSource as TextBox;
            if(tb != null)
            {
                var dli = tb.DataContext as IDataListItem;
                var vm = DataContext as WorkflowInputDataViewModel;
                if(vm != null)
                {
                    vm.AddRow(dli);
                }
            }
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if(e.Source is TabControl)
            {
                var tabCtrl = e.Source as TabControl;
                var tabItem = tabCtrl.SelectedItem as TabItem;
                var vm = DataContext as WorkflowInputDataViewModel;
                if(vm != null)
                {
                    if(tabItem != null && tabItem.Header.ToString() == "XML")
                    {
                        switch(_currentTab)
                        {
                            case InputTab.Grid:
                                vm.SetXmlData();
                                ShowDataInOutputWindow(vm.XmlData);
                                break;
                            case InputTab.Json:
                                var xmlDocument = JsonConvert.DeserializeXmlNode(_jsonEditor.Text, "DataList");
                                vm.XmlData = xmlDocument.InnerXml;
                                vm.SetWorkflowInputData();
                                vm.SetXmlData();
                                ShowDataInOutputWindow(vm.XmlData);
                                break;
                        }
                        _currentTab = InputTab.Xml;
                    }
                    else if (tabItem != null && tabItem.Header.ToString() == "JSON")
                    {
                        
                        var xml = new XmlDocument();
                        switch(_currentTab)
                        {
                            case InputTab.Grid:
                                vm.SetXmlData();
                                if (vm.XmlData != null)
                                {
                                    xml.LoadXml(vm.XmlData);
                                }
                                break;
                            case InputTab.Xml:
                                if (!string.IsNullOrEmpty(_editor.Text))
                                {
                                    try
                                    {
                                        xml.LoadXml(_editor.Text);
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                                break;
                        }
                        
                        if(xml.FirstChild != null)
                        {
                            var json = JsonConvert.SerializeXmlNode(xml.FirstChild,Newtonsoft.Json.Formatting.Indented,true);
                            _jsonEditor.Text = json;
                        }
                        JsonOutput.Content = _jsonEditor;
                        _currentTab = InputTab.Json;
                    }
                    else
                    {
                        var xmlData = _editor.Text;
                        if (_currentTab == InputTab.Json)
                        {
                            var xmlDocument = JsonConvert.DeserializeXmlNode(_jsonEditor.Text, "DataList");
                            xmlData = xmlDocument.InnerXml;                           
                        }
                        vm.XmlData = xmlData;
                        vm.SetWorkflowInputData();
                        _currentTab = InputTab.Grid;
                    }
                }
            }
        }

        private void MenuItemAddRow(object sender, RoutedEventArgs e)
        {
            int indexToSelect;
            var vm = DataContext as WorkflowInputDataViewModel;

            if (vm != null && (vm.AddBlankRow(DataListInputs.ActiveItem as IDataListItem, out indexToSelect)))
            {
                DataListInputs.ActiveItem = indexToSelect;
                Dispatcher.BeginInvoke(new Action(FocusOnAddition), DispatcherPriority.ApplicationIdle);
            }
        }

        private void FocusOnAddition()
        {
            var row = GetSelectedRow(DataListInputs);
            if (row != null)
            {
                var intelbox = FindByName("txtValue", row) as IntellisenseTextBox;
                if (intelbox != null)
                {
                    intelbox.Focus();
                }
            }
        }

        private void MenuItemDeleteRow(object sender, RoutedEventArgs e)
        {
            int indexToSelect;
            var vm = DataContext as WorkflowInputDataViewModel;
            if (vm != null && (vm.RemoveRow(DataListInputs.ActiveItem as IDataListItem, out indexToSelect)))
            {
                DataListInputs.ActiveItem = indexToSelect;
            }
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as TextBox;
            if(tb != null)
            {
                tb.SelectAll();
            }
        }

        private void IntellisenseTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            int indexToSelect;
            if((e.Key == Key.Enter || e.Key == Key.Return) && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                var vm = DataContext as WorkflowInputDataViewModel;
                if (vm != null && (vm.AddBlankRow(DataListInputs.ActiveItem as IDataListItem, out indexToSelect)))
                {
                    DataListInputs.ActiveItem = indexToSelect;
                    Dispatcher.BeginInvoke(new Action(FocusOnAddition), DispatcherPriority.ApplicationIdle);
                }
                e.Handled = true;
            }
            else if(e.Key == Key.Delete && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                var vm = DataContext as WorkflowInputDataViewModel;
                if (vm != null && (vm.RemoveRow(DataListInputs.ActiveItem as IDataListItem, out indexToSelect)))
                {
                    DataListInputs.ActiveItem = indexToSelect;
                }

                e.Handled = true;
            }
        }

        private void DataListInputsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = GetSelectedRow(DataListInputs);
            if(row != null)
            {
                var intelbox = FindByName("txtValue", row) as IntellisenseTextBox;
                if(intelbox != null)
                {
                    intelbox.Focus();
                }
            }
        }

        private FrameworkElement FindByName(string name, FrameworkElement root)
        {
            if(root != null)
            {
                var tree = new Stack<FrameworkElement>();
                tree.Push(root);
                while(tree.Count > 0)
                {
                    FrameworkElement current = tree.Pop();
                    if(current.Name == name)
                        return current;

                    int count = VisualTreeHelper.GetChildrenCount(current);
                    for(int supplierCounter = 0; supplierCounter < count; ++supplierCounter)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(current, supplierCounter);
                        FrameworkElement item = child as FrameworkElement;
                        if(item != null)
                            tree.Push(item);
                    }
                }
            }
            return null;
        }

        public static CellsPanel GetSelectedRow(XamGrid grid)
        {
            if(grid.ActiveCell != null)
            {
                var row = grid.ActiveCell.Row;
                return row.Control;
            }
            return null;
        }

        private void ExecuteClicked(object sender, RoutedEventArgs e)
        {
            var tabItem = TabItems.SelectedItem as TabItem;
            var vm = DataContext as WorkflowInputDataViewModel;
            if(vm != null)
            {
                if(tabItem != null && tabItem.Header.ToString() == "XML")
                {
                    vm.XmlData = _editor.Text;
                    vm.SetWorkflowInputData();
                }
            }
            DestroyTimer();
        }

        void DestroyTimer()
        {
            if(_foldingUpdateTimer != null)
            {
                _foldingUpdateTimer.Tick -= OnFoldingUpdateTimerOnTick;
                _foldingUpdateTimer.Stop();
                _foldingUpdateTimer = null;
            }
        }

        void CancelClicked(object sender, RoutedEventArgs e)
        {
            DestroyTimer();
        }

        void DataListInputs_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Tag = e.Row.GetIndex();
        }

        void WorkflowInputDataView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {if (Mouse.LeftButton == MouseButtonState.Pressed)
            this.DragMove();
        }

        void WorkflowInputDataView_OnClosed(object sender, EventArgs e)
        {
            RemoveBlackOutEffect();
        }
        void RemoveBlackOutEffect()
        {
            Application.Current.MainWindow.Effect = null;
            var content = Application.Current.MainWindow.Content as Grid;
            if (content != null)
            {
                content.Children.Remove(_blackoutGrid);
            }
        }
        void AddBlackOutEffect()
        {
            var effect = new BlurEffect { Radius = 10, KernelType = KernelType.Gaussian, RenderingBias = RenderingBias.Quality };
            var content = Application.Current.MainWindow.Content as Grid;
            _blackoutGrid = new Grid();
            _blackoutGrid.Background = new SolidColorBrush(Colors.Black);
            _blackoutGrid.Opacity = 0.75;
            if (content != null)
            {
                content.Children.Add(_blackoutGrid);
            }
            Application.Current.MainWindow.Effect = effect;
        }
    }

    public enum InputTab
    {
        Grid,
        Xml,
        Json
    }
}
