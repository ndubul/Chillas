using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.WebServices;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.ViewModels;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ManageWebserviceControl.xaml
    /// </summary>
    public partial class ManageWebserviceControl : IView, ICheckControlEnabledView
    {
        public ManageWebserviceControl()
        {
            InitializeComponent();
            SourcesComboBox.Focus();
        }

        public void SelectMethod(WebRequestMethod requestName)
        {
            try
            {
                RequestTypes.SelectedItem = requestName;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }
        }

        public void SelectWebService(IWebServiceSource webServiceName)
        {
            try
            {
                SourcesComboBox.SelectedItem = webServiceName;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }
        }

        public void TestAction()
        {
            TestButton.Command.Execute(null);
        }

        public void EditAction()
        {
            EditButton.Command.Execute(null);
        }

        public void PasteAction()
        {
            PasteButton.Command.Execute(null);
        }

        public void Save()
        {
            SaveButton.Command.Execute(null);
        }

        public ItemCollection GetHeaders()
        {
            BindingExpression be = HeadersGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return HeadersGrid.ItemsSource as ItemCollection;
        }

        public string GetOutputs()
        {
            BindingExpression be = Response.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return Response.Text;
        }

        public bool IsDataSourceFocused()
        {
            return SourcesComboBox.IsFocused;
        }

        public IWebServiceSource GetSelectedWebService()
        {
            IWebServiceSource selectedSource = null;
            BindingExpression bindingExpression = SourcesComboBox.GetBindingExpression(Selector.SelectedItemProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateTarget();
                var manageWebServiceViewModel = bindingExpression.DataItem as IManageWebServiceViewModel;
                selectedSource = SourcesComboBox.SelectedItem as IWebServiceSource;
                if (manageWebServiceViewModel != null)
                {
                    if (selectedSource == null)
                    {
                        selectedSource = manageWebServiceViewModel.SelectedSource;
                    }
                }
            }
            return selectedSource;
        }

        public string GetUrl()
        {
            return ((ManageWebServiceViewModel)DataContext).SourceUrl;
        }

        public bool GetControlEnabled(string controlName)
        {
            switch (controlName)
            {
                case "Save":
                    return SaveButton.Command.CanExecute(null);
                case "Test":
                    return TestButton.Command.CanExecute(null);
                case "New":
                    return New.Command.CanExecute(null);
                case "Edit":
                    return EditButton.Command.CanExecute(null);
                case "1 Select Request Method and Source":
                    return SourcesComboBox.IsEnabled;

                case "2 Request Body" :
                    return ((ManageWebServiceViewModel)DataContext).RequestBodyEnabled ;
                case "2 Request":
                    {
                        BindingExpression be = RequestHeadersGrid.GetBindingExpression(VisibilityProperty);
                        if (be != null)
                        {
                            be.UpdateTarget();
                        }
                        return RequestHeadersGrid.Visibility == Visibility.Visible;
                    }
                case "3 Variables":
                    {
                        BindingExpression be = RequestVarGrid.GetBindingExpression(VisibilityProperty);
                        if (be != null)
                        {
                            be.UpdateTarget();
                        }
                        return RequestVarGrid.Visibility == Visibility.Visible;
                    }
                case "4 Response":
                    {
                        BindingExpression be = ResponseGrid.GetBindingExpression(VisibilityProperty);
                        if (be != null)
                        {
                            be.UpdateTarget();
                        }
                        return ResponseGrid.Visibility == Visibility.Visible;
                    }
                case "5 Defaults and Mapping":
                    {
                        BindingExpression be = MappingsGrid.GetBindingExpression(VisibilityProperty);
                        if (be != null)
                        {
                            be.UpdateTarget();
                        }
                        return MappingsGrid.Visibility == Visibility.Visible;
                    }
                case "Paste":
                    {

                        return PasteButton.IsEnabled;
                    }
            }
            return false;
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

        public string GetQueryString()
        {
            return ((ManageWebServiceViewModel)DataContext).RequestUrlQuery;
        }

        public void SetQueryString(string querystring)
        {
             RequestUrl.Text = querystring;
        }

        public string GetHeader()
        {
            return ((ManageWebServiceViewModel)DataContext).HeaderText;
        }

        public void SetHeader(string s, string querystring)
        {
             ((ManageWebServiceViewModel)DataContext).Headers.Add(new NameValue{ Name= s,Value = querystring});
        }

        public void SetBody(string body)
        {
            RequestBody.Text = body;
            ((ManageWebServiceViewModel)DataContext).RequestBody = body;
        }

        public object GetBody()
        {
            return RequestBody.Text;
        }
    }
}
