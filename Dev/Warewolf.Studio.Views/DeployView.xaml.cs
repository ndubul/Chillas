using System.Linq;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.ViewModels;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for DeployView.xaml
    /// </summary>
    public partial class DeployView : IView, ICheckControlEnabledView
    {
        string _errorMessage;
        string _canDeploy;
        string _canSelectDependencies;
        string _statusPassed;
        string _connectors;
        string _services;
        string _sources;
        string _new;
        int _overrides;

        public DeployView()
        {
            InitializeComponent();
        }

        public IServer SelectedServer
        {
            get
            {
                return (SourceNavigationView).SelectedServer;
            }
            set
            {
                (SourceNavigationView).SelectedServer = value;
            }
        }
        public IServer SelectedDestinationServer
        {
            get
            {
                return DestinationConnectControl.SelectedServer;
            }
            set
            {
                DestinationConnectControl.SelectedServer = value;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return ServersNotSame.Text;
            }
            set
            {
                ServersNotSame.Text = value;
            }
        }
        public string CanDeploy
        {
            get
            {
                return ((IDeployViewModel)DataContext).DeployCommand.CanExecute(null) ? "Enabled" : "Disabled";
            }
            set
            {
                Deploy.IsEnabled = value=="Enabled";
            }
        }
        public string CanSelectDependencies
        {
            get
            {
                return Dependencies.IsEnabled ? "Enabled" : "Disabled"; 
            }
            set
            {
                Dependencies.IsEnabled = value=="Enabled";
            }
        }
        public string StatusPassedMessage
        {
            get
            {

                return ((IDeployViewModel)DataContext).DeploySuccessMessage??"";
            }
            set
            {
                StatusPass.Text = value;
            }
        }
        public string Connectors
        {
            get
            {
                return ((IDeployViewModel)DataContext).ConnectorsCount;
            }
          
       }
        public string Services
        {
            get
            {
                return ((IDeployViewModel)DataContext).ServicesCount;
            }
        }
        public string Sources
        {
            get
            {
                return ((IDeployViewModel)DataContext).SourcesCount;
            }

        }
        public string New
        {
            get
            {
                return ((IDeployViewModel)DataContext).NewResourcesCount;
            }
        }
        public string Overrides
        {
            get
            {
                return ((IDeployViewModel)DataContext).OverridesCount;
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

        #region Implementation of ICheckControlEnabledView

        public bool GetControlEnabled(string controlName)
        {
            return false;
        }

        #endregion

        public void SelectPath(string path)
        {
            ((IDeployViewModel)DataContext).Source.SelectedEnvironment.AsList().Apply(a =>
            {
                if (a.ResourcePath == path) 
                    a.IsResourceUnchecked = true; 
                
            });
            ((IDeployViewModel)DataContext).StatsViewModel.Calculate(((IDeployViewModel)DataContext).Source.SelectedItems.ToList());
        }
        public void UnSelectPath(string path)
        {
            ((IDeployViewModel)DataContext).Source.SelectedEnvironment.AsList().Apply(a =>
            {
                if (a.ResourcePath == path)
                    a.IsResourceUnchecked = false;

            });
            ((IDeployViewModel)DataContext).StatsViewModel.Calculate(((IDeployViewModel)DataContext).Source.SelectedItems.ToList());
        }

        public void SelectDestinationServer(string servername)
        {
            DestinationConnectControl.SelectedServer = ((IDeployViewModel)DataContext).Destination.ConnectControlViewModel.Servers.FirstOrDefault(a => a.ResourceName == servername);
        }

        public void DeployItems()
        {
            ((IDeployViewModel)DataContext).DeployCommand.Execute(null);
        }

        public void SelectDependencies()
        {
            Dependencies.Command.Execute(null);
        }

        public string VerifySelectPath(string path)
        {
            var res = ((IDeployViewModel)DataContext).Source.SelectedEnvironment.AsList().FirstOrDefault(a =>  (a.ResourcePath ==path) && (a.IsResourceChecked ?? false));
            if(res==null)
            {
                return "Selected";
            }
            else
            {
                return "Not Selected";
            }
        }

        public void SetFilter(string filter)
        {
            ((IDeployViewModel)DataContext).Source.SearchText = filter;
        }

        public string VerifySelectPathVisibility(string path)
        {
            var res = ((IDeployViewModel)DataContext).Source.SelectedEnvironment.AsList().FirstOrDefault(a => (a.ResourcePath == path) );
            if (res == null)
            {
                return "Not Visible";
            }
            else 
            {
                if(!res.IsVisible)
                return "Not Visible";
                return "Visible";
            }
        }
    }
}
