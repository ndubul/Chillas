using System;
using System.Linq;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;

namespace Warewolf.Studio.ViewModels
{
    public class DeployDestinationViewModel : ExplorerViewModel, IDeployDestinationExplorerViewModel
    {
        #region Implementation of IDeployDestinationExplorerViewModel

        public DeployDestinationViewModel(IShellViewModel shellViewModel, Microsoft.Practices.Prism.PubSubEvents.IEventAggregator aggregator)
            : base(shellViewModel, aggregator)
        {

            ConnectControlViewModel.SelectedEnvironmentChanged += DeploySourceExplorerViewModelSelectedEnvironmentChanged;
            SelectedEnvironment = _environments.FirstOrDefault();
        }

        void DeploySourceExplorerViewModelSelectedEnvironmentChanged(object sender, Guid environmentid)
        {
            var environmentViewModel = _environments.FirstOrDefault(a => a.Server.EnvironmentID == environmentid);
            if (environmentViewModel != null)
            {
                SelectedEnvironment = environmentViewModel;
              
            }
        }

        #endregion
    }
}