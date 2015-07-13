using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Dev2.Common.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Infragistics.Controls.Editors.Primitives;
using Infragistics.Windows;
using Warewolf.Studio.ViewModels;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ManageDatabaseSourceControl.xaml
    /// </summary>
    public partial class ManageDatabaseSourceControl : IManageDatabaseSourceView, ICheckControlEnabledView
    {
        public ManageDatabaseSourceControl()
        {
            InitializeComponent();
        }

        public void EnterServerName(string serverName,bool add=false)
        {
            var comboEditorItem = ServerTextBox.Items.FirstOrDefault(item =>
            {
                var computerName = item.Data as ComputerName;
                if(computerName != null)
                {
                    return computerName.Name.Equals(serverName, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            });
            if(comboEditorItem==null&& add)
            {
                (ServerTextBox.ItemsSource as ICollection<IComputerName>).Add(new ComputerName(){Name= serverName});
                EnterServerName(serverName, false);
            }
            if(comboEditorItem != null)
            {
                try
                {
                    ServerTextBox.SelectedItem = comboEditorItem.Data;
                }
                catch(Exception)
                {
                    //Ignore exception running from test
                }
            }
            else
            {
                (DataContext as IManageDatabaseSourceViewModel).ServerName = new ComputerName(){Name= serverName};
            }
        }

        public Visibility GetDatabaseDropDownVisibility()
        {
            BindingExpression be = DatabaseComboxContainer.GetBindingExpression(VisibilityProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return DatabaseComboxContainer.Visibility;
        }

        public bool GetControlEnabled(string controlName)
        {
            switch (controlName)
            {
                case "Save":
                    return SaveButton.Command.CanExecute(null);
                case "Test Connection":
                    return TestConnectionButton.Command.CanExecute(null);
            }
            return false;
        }

        public void SetAuthenticationType(AuthenticationType authenticationType)
        {
            if (authenticationType == AuthenticationType.Windows)
            {
                WindowsRadioButton.IsChecked = true;
            }
            else
            {
                UserRadioButton.IsChecked = true;
            }
        }

        public void SelectDatabase(string databaseName)
        {
            try
            {
                DatabaseComboxBox.SelectedItem = databaseName;
            }
            catch(Exception)
            {
                //Stupid exception when running from tests
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

        public void PerformTestConnection()
        {
            TestConnectionButton.Command.Execute(null);
        }

        public void PerformSave()
        {
            SaveButton.Command.Execute(null);
        }

        public void EnterUserName(string userName)
        {
            UserNameTextBox.Text = userName;
        }

        public void EnterPassword(string password)
        {
            PasswordTextBox.Password = password;
        }

        public string GetErrorMessage()
        {
            return (DataContext as ManageDatabaseSourceViewModel).TestMessage;
        }


        // ReSharper disable once InconsistentNaming
        private void XamComboEditor_Loaded(object sender, RoutedEventArgs e)
        {
            SpecializedTextBox txt = Utilities.GetDescendantFromType(sender as DependencyObject, typeof(SpecializedTextBox), false) as SpecializedTextBox;
            if (txt != null)
            {
                txt.SelectAll();
                txt.Focus();
                var selectedItem = ServerTextBox.SelectedItem as ComputerName;
                if (selectedItem != null) txt.Text = selectedItem.Name;
                ServerTextBox.Style = Application.Current.TryFindResource("XamComboEditorStyle") as Style;
            }
        }

        #region Implementation of IComponentConnector

        /// <summary>
        /// Attaches events and names to compiled content. 
        /// </summary>
        /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
        public void Connect(int connectionId, object target)
        {
        }

        #endregion

        public void VerifyServerExistsintComboBox(string serverName)
        {
           
        }

        public  IEnumerable<string> GetServerOptions()
        {
           
          return new List<string>();
        }

        public string GetSelectedDbOption()
        {
            return ServerTypeComboBox.SelectedValue.ToString();
        }

        public void Test()
        {
            TestConnectionButton.Command.Execute(null);
        }

        public string GetUsername()
        {
            return UserNameTextBox.Text;
        }

        public object GetPassword()
        {
            return PasswordTextBox.Password;
        }

        public void Cancel()
        {
            
        }

        public string GetHeader()
        {
            return (DataContext as ManageDatabaseSourceViewModel).HeaderText;
        }
        public string  GetTabHeader()
        {
            return (DataContext as ManageDatabaseSourceViewModel).Header;
        }
        public void CancelTest()
        {
            CancelTestButton.Command.Execute(null);
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
