﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.ViewModels;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ManageDatabaseServiceControl.xaml
    /// </summary>
    public partial class ManageDatabaseServiceControl : IView, ICheckControlEnabledView
    {
        public ManageDatabaseServiceControl()
        {
            InitializeComponent();
            SourcesComboBox.Focus();
        }

        public void SelectDbSource(IDbSource dbSourceName)
        {
            try
            {
                SourcesComboBox.SelectedItem = dbSourceName;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }
        }

        public void SelectDbAction(IDbAction actionName)
        {
            try
            {
                ActionsComboBox.SelectedItem = actionName;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {

            }
        }

        public void TestAction()
        {
            TestActionButton.Command.Execute(null);
        }

        public void Save()
        {
            var viewModel = DataContext as ManageDatabaseServiceViewModel;
            if (viewModel != null)
            {
                viewModel.SaveCommand.Execute(null);
            }
        }

        public bool IsDataSourceFocused()
        {
            return SourcesComboBox.IsFocused;
        }

        public ItemCollection GetInputs()
        {
            BindingExpression be = InputsList.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return InputsList.Items;
        }

        public ItemCollection GetOutputs()
        {
            BindingExpression be = OutputsDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return OutputsDataGrid.ItemsSource as ItemCollection;
        }

        public IDbSource GetSelectedDataSource()
        {
            IDbSource selectedDataSource = null;
            BindingExpression bindingExpression = SourcesComboBox.GetBindingExpression(Selector.SelectedItemProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateTarget();
                var manageDbServiceViewModel = bindingExpression.DataItem as IManageDbServiceViewModel;
                selectedDataSource = SourcesComboBox.SelectedItem as IDbSource;
                if (manageDbServiceViewModel != null)
                {
                    if (selectedDataSource == null)
                    {
                        selectedDataSource = manageDbServiceViewModel.SelectedSource;
                    }
                }

            }
            return selectedDataSource;
        }

        public IDbAction GetSelectedAction()
        {
            IDbAction selectedAction = null;
            BindingExpression bindingExpression = ActionsComboBox.GetBindingExpression(Selector.SelectedItemProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateTarget();
                var manageDbServiceViewModel = bindingExpression.DataItem as IManageDbServiceViewModel;
                selectedAction = ActionsComboBox.SelectedItem as IDbAction;
                if (manageDbServiceViewModel != null)
                {
                    if (selectedAction == null)
                    {
                        selectedAction = manageDbServiceViewModel.SelectedAction;
                    }
                }
            }
            return selectedAction;
        }

        public bool GetControlEnabled(string controlName)
        {
            switch (controlName)
            {
                case "Save":
                    var viewModel = DataContext as ManageDatabaseServiceViewModel;
                    return viewModel != null && viewModel.SaveCommand.CanExecute(null);
                case "Test":
                    return TestActionButton.Command.CanExecute(null);
                case "1 Data Source":
                    return SourcesComboBox.IsEnabled;
                case "2 Select Action":
                    {
                        BindingExpression be = SelectAnActionGrid.GetBindingExpression(VisibilityProperty);
                        if (be != null)
                        {
                            be.UpdateTarget();
                        }
                        return SelectAnActionGrid.Visibility == Visibility.Visible;
                    }
                case "3 Test Connector and Calculate Outputs":
                    {
                        BindingExpression be = TestActionGrid.GetBindingExpression(VisibilityProperty);
                        if (be != null)
                        {
                            be.UpdateTarget();
                        }
                        return TestActionGrid.Visibility == Visibility.Visible;
                    }
                case "4 Default and Mapping":
                    {
                        BindingExpression be = MappingsGrid.GetBindingExpression(VisibilityProperty);
                        if (be != null)
                        {
                            be.UpdateTarget();
                        }
                        return MappingsGrid.Visibility == Visibility.Visible;
                    }
            }
            return false;
        }

        private void ManageDatabaseServiceControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            SourcesComboBox.Focus();
        }

        public ItemCollection GetInputMappings()
        {
            return MappingsView.GetInputMappings();
        }

        public ItemCollection GetOutputMappings()
        {
            return MappingsView.GetOutputMappings();
        }

        /// <summary>
        /// Attaches events and names to compiled content. 
        /// </summary>
        /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
        public void Connect(int connectionId, object target)
        {
        }

        public static void Test()
        {
        }
    }
}
