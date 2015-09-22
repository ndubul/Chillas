
using System.Windows;
using System.Windows.Data;
using Dev2.Common.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.Practices.Prism.Mvvm;

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
            AddressTextBox.Focus();
        }

        public bool IsAddressFocused()
        {
            return AddressTextBox.IsFocused;
        }

        public void EnterAddressName(string address)
        {
            AddressTextBox.Text = address;
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
            return AddressTextBox.Text;
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
