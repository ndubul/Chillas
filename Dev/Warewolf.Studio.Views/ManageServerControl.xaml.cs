
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Dev2.Common.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Infragistics.Controls.Editors;
using Infragistics.Controls.Editors.Primitives;
using Infragistics.Windows;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.ViewModels;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ManageServerControl.xaml
    /// </summary>
    public partial class ManageServerControl : IView, ICheckControlEnabledView
    {
        public ManageServerControl()
        {
            InitializeComponent();
        }

        // ReSharper disable once InconsistentNaming
        private void XamComboEditor_Loaded(object sender, RoutedEventArgs e)
        {
            SpecializedTextBox txt = Utilities.GetDescendantFromType(sender as DependencyObject, typeof(SpecializedTextBox), false) as SpecializedTextBox;
            if (txt != null)
            {
                txt.SelectAll();
                txt.Focus();
                var selectedItem = AddressTextBox.SelectedItem as ComputerName;
                if (selectedItem != null) txt.Text = selectedItem.Name;
                AddressTextBox.Style = Application.Current.TryFindResource("XamComboEditorStyle") as Style;
            }
        }

        public void SelectServer(string serverName)
        {
            try
            {
                EnterServerName(serverName);
            }
            catch (Exception)
            {
                //Stupid exception when running from tests
            }
        }

        public void EnterServerName(string serverName, bool add = false)
        {
            if(AddressTextBox.Items != null)
            {
                var comboEditorItem = AddressTextBox.Items.FirstOrDefault(item =>
                {
                    var computerName = item.Data as ComputerName;
                    if (computerName != null)
                    {
                        return computerName.Name.Equals(serverName, StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                });
                if (comboEditorItem == null && add)
                {
                    var computerNames = AddressTextBox.ItemsSource as ICollection<IComputerName>;
                    if (computerNames != null)
                    {
                        computerNames.Add(new ComputerName { Name = serverName });
                    }
                    EnterServerName(serverName);
                }
                if (comboEditorItem != null)
                {
                    try
                    {
                        AddressTextBox.SelectedItem = comboEditorItem.Data;
                        BindingExpression be = AddressTextBox.GetBindingExpression(XamComboEditor.SelectedItemProperty);
                        if (be != null)
                        {
                            be.UpdateSource();
                        }
                        var manageDatabaseSourceViewModel = DataContext as IManageNewServerViewModel;

                        if (manageDatabaseSourceViewModel != null)
                        {
                            if (manageDatabaseSourceViewModel.ServerName.Name == null)
                            {
                                manageDatabaseSourceViewModel.ServerName = new ComputerName { Name = serverName };
                            }
                        }                        
                    }
                    catch (Exception)
                    {
                        //Ignore exception running from test
                    }
                }
                else
                {
                    var manageDatabaseSourceViewModel = DataContext as IManageNewServerViewModel;
                    if (manageDatabaseSourceViewModel != null)
                    {
                        manageDatabaseSourceViewModel.ServerName = new ComputerName { Name = serverName };
                    }
                }
            }
            else
            {
                var manageDatabaseSourceViewModel = DataContext as IManageNewServerViewModel;
                if (manageDatabaseSourceViewModel != null)
                {
                    manageDatabaseSourceViewModel.ServerName = new ComputerName { Name = serverName };
                }
            }
        }

        public void EnterUserName(string username)
        {
            UsernameTextBox.Text = username;
        }

        public void EnterPassword(string password)
        {
            PasswordTextBox.Password = password;
        }

        public string GetProtocol()
        {
            return ProtocolItems.SelectedItem.ToString();
        }

        public string GetAddress()
        {
            return AddressTextBox.SelectedItem.ToString();
        }

        public string GetPort()
        {
            return PortTextBox.Text;
        }

        public string GetUsername()
        {
            return UsernameTextBox.Text;
        }

        public string GetPassword()
        {
            return PasswordTextBox.Password;
        }

        public void SetAuthenticationType(AuthenticationType authenticationType)
        {
            switch(authenticationType)
            {
                case AuthenticationType.Windows:
                    WindowsRadioButton.IsChecked = true;
                    break;
                case AuthenticationType.User:
                    UserRadioButton.IsChecked = true;
                    break;
                case AuthenticationType.Public:
                    PublicRadioButton.IsChecked = true;
                    break;
                default:
                    WindowsRadioButton.IsChecked = true;
                    break;
            }
        }

        public void SetProtocol(string protocol)
        {
            try
            {
                if (ProtocolItems.Items.Count == 0)
                {
                    BindingExpression be = ProtocolItems.GetBindingExpression(ItemsControl.ItemsSourceProperty);
                    if (be != null)
                    {
                        be.UpdateTarget();
                    }
                    ProtocolItems.DataContext = DataContext;                    
                }
                ProtocolItems.SelectedItem = protocol;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {

            }
        }

        public void SetPort(string port)
        {
            try
            {
                PortTextBox.Text = port;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {

            }
        }

        public void PerformSave()
        {
            var viewModel = DataContext as ManageNewServerViewModel;
            if (viewModel != null)
            {
                viewModel.OkCommand.Execute(null);
            }
        }

        public Visibility GetUsernameVisibility()
        {
            BindingExpression be = UserNamePasswordContainer.GetBindingExpression(VisibilityProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return UserNamePasswordContainer.Visibility;
        }

        public Visibility GetPasswordVisibility()
        {
            BindingExpression be = UserNamePasswordContainer.GetBindingExpression(VisibilityProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return UserNamePasswordContainer.Visibility;
        }
        public void TestAction()
        {
            TestConnectionButton.Command.Execute(null);
        }

        public string GetErrorMessage()
        {
            BindingExpression be = ErrorTextBlock.GetBindingExpression(TextBlock.TextProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return ErrorTextBlock.Text;
        }

        /// <summary>
        /// Attaches events and names to compiled content. 
        /// </summary>
        /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
        public void Connect(int connectionId, object target)
        {
        }

        #region Implementation of ICheckControlEnabledView

        public bool GetControlEnabled(string controlName)
        {
            switch (controlName)
            {
                case "Save":
                    var viewModel = DataContext as IManageNewServerViewModel;
                    return viewModel != null && viewModel.OkCommand.CanExecute(null);
                case "Test":
                    return TestConnectionButton.Command.CanExecute(null);
            }
            return false;
        }

        #endregion
    }
}
