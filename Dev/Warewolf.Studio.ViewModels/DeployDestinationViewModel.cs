using System;
using System.Linq;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;

namespace Warewolf.Studio.ViewModels
{

    public class DeployDestinationViewModel : ExplorerViewModel, IDeployDestinationExplorerViewModel
    {
        bool _isLoading;

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

        #region Overrides of ExplorerViewModel

        public override sealed bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(() => IsLoading);
            }
        }

        #endregion

        #region Overrides of ExplorerViewModel

        public override void AfterLoad(Guid environmentID)
        {
            var environmentViewModel = _environments.FirstOrDefault(a => a.Server.EnvironmentID == environmentID);
            if (environmentViewModel != null)
            {
                SelectedEnvironment = environmentViewModel;

            }
            if(ServerStateChanged!=null)
            {
                ServerStateChanged(this, SelectedEnvironment.Server);
                ConnectControlViewModel.IsLoading = false;
            
            }
 
        }

        #endregion

        #endregion

        #region Implementation of IDeployDestinationExplorerViewModel

        public event ServerSate ServerStateChanged;

        #endregion
    }
}