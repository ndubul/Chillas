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
                return Deploy.IsEnabled ? "Enabled" : "Disabled";
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
            ((IDeployViewModel)DataContext).Source.SelectedEnvironment.AsList().Apply(a => { if (a.ResourcePath == path) a.IsFolderChecked = true; });
        }

        public void SelectDestinationServer(string servername)
        {
            DestinationConnectControl.SelectedServer = ((IDeployViewModel)DataContext).Destination.ConnectControlViewModel.Servers.FirstOrDefault(a => a.ResourceName == servername);
        }
    }
}
