
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using System.Windows;
using Dev2.Models;
using Dev2.Studio.ViewModels.Deploy;
using Microsoft.Practices.Prism.Mvvm;

// ReSharper disable once CheckNamespace
namespace Dev2.Studio.Views.Deploy
{
    /// <summary>
    /// Interaction logic for DeployView.xaml
    /// </summary>
    public partial class DeployView:IView
    {
        public DeployView()
        {
            InitializeComponent();
        }

        void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            if(frameworkElement != null)
            {
                ExplorerItemModel rtvm = frameworkElement.DataContext as ExplorerItemModel;
                if(rtvm != null)
                {
                    DeployViewModel viewModel = DataContext as DeployViewModel;
                    if(viewModel != null)
                    {
                        viewModel.SelectDependencies(new List<IExplorerItemModel> { rtvm });
                    }
                }
            }
        }

        public void SetSelectedSourceServer(string server)
        {
            SourceConnectControl.SelectServer(server);
        }

        public void SetSelectedDestinationServer(string server)
        {
            var servername = DestinationConnectControl.SelectServer(server);
            var deployViewModel = DataContext as DeployViewModel;
            if(deployViewModel != null)
            {
                deployViewModel.SelectedDestinationServer = servername.EnvironmentModel;
                deployViewModel.CalculateStats();
            }
        }

        public string GetValidationMessage()
        {
            var deployViewModel = DataContext as DeployViewModel;
            var s = deployViewModel != null && deployViewModel.ServersAreNotTheSame ? ServersNotSame.Text:"";
            return s;
        }

        public bool GetDeployState()
        {
            return Deploy.IsEnabled;
        }

        public bool GetSelectDependenciesState()
        {
            return Dependencies.IsEnabled;
        }

        public void SelectItemToDeploy(string resource)
        {
            SourceNavigationView.SelectItem(resource);
        }
    }
}
