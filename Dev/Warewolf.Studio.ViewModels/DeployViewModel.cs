using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Deploy;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class SingleExplorerDeployViewModel : BindableBase, IDeployViewModel
    {
        IDeploySourceExplorerViewModel _source;
        readonly IDeployStatsViewerViewModel _stats;
        IDeployDestinationExplorerViewModel _destination;
        IConnectControlViewModel _sourceconnectControlViewModel;
        IConnectControlViewModel _destinationConnectControlViewModel;
        string _unknownCount;
        string _sourcesCount;
        string _servicesCount;
        string _connectorsCount;
        string _newResourcesCount;
        string _overridesCount;
        bool _showConflicts;
        bool _isDeploying;
        bool _deploySuccessfull;
        string _conflictNewResourceText;
        IShellViewModel _shell;

        #region Implementation of IDeployViewModel

        public SingleExplorerDeployViewModel(IDeployDestinationExplorerViewModel destination, IDeploySourceExplorerViewModel source,IEnumerable<IExplorerTreeItem> selectedItems,IDeployStatsViewerViewModel stats,IShellViewModel shell) 
        {
            VerifyArgument.AreNotNull(new Dictionary<string, object> { { "destination", destination }, { "source", source }, { "selectedItems", selectedItems }, { "stats", stats } });
            _destination = destination;
            
            _source = source;
            _source.SelectItemsForDeploy(selectedItems);
            _stats = stats;
            _stats.CalculateAction = () => { ServicesCount = _stats.Services.ToString(); };
            SourceConnectControlViewModel = _source.ConnectControlViewModel;
            DestinationConnectControlViewModel = _destination.ConnectControlViewModel;

            DeployCommand = new DelegateCommand(Deploy);
            SelectDependenciesCommand = new DelegateCommand(SelectDependencies);
            NewResourcesViewCommand = new DelegateCommand(ViewNewResources);
            OverridesViewCommand = new DelegateCommand(ViewOverrides);

            ShowConflicts = false;
            ConnectorsCount = stats.Connectors.ToString();
            ServicesCount = stats.Services.ToString();
            SourcesCount = stats.Sources.ToString();
            UnknownCount = stats.Unknown.ToString();
            NewResourcesCount = stats.NewResources.ToString();
            OverridesCount = stats.Overrides.ToString();
            _shell = shell;
        }

        void ViewOverrides()
        {
            ShowConflicts = true;
            ConflictNewResourceText = "List of Overrides";
        }

        public string ConflictNewResourceText
        {
            get
            {
                return _conflictNewResourceText;
            }
            set
            {
                _conflictNewResourceText = value;
                OnPropertyChanged(() => ConflictNewResourceText);
            }
        }

        public bool ShowConflicts
        {
            get
            {
                return _showConflicts;
            }
            set
            {
                _showConflicts = value;
                OnPropertyChanged(() => ShowConflicts);
            }
        }

        void ViewNewResources()
        {
            ShowConflicts = true;
            ConflictNewResourceText = "List of New Resources";
        }

        void Deploy()
        {
            _shell.DeployResources(Source.Environments.First().Server.EnvironmentID, Destination.SelectedEnvironment.Server.EnvironmentID, Source.SelectedItems.Where(a=>a.ResourceType!= ResourceType.Folder).Select(a => a.ResourceId).ToList());
           

        }



        public void SelectDependencies()
        {
            if (Source != null && Source.SelectedEnvironment != null && Source.SelectedEnvironment.Server != null)
            {
                var guids = Source.SelectedEnvironment.Server.QueryProxy.FetchDependenciesOnList(Source.SelectedItems.Select(a => a.ResourceId));
                Source.SelectedEnvironment.AsList().Where(a => guids.Contains(a.ResourceId)).Apply(a=>a.IsResourceChecked=true);
            }
        }

        /// <summary>
        /// Used to indicate a successfull deploy has happened
        /// </summary>
        public bool DeploySuccessfull
        {
            get
            {
                return _deploySuccessfull;
            }
            private set
            {
                _deploySuccessfull = value;
                OnPropertyChanged(() => DeploySuccessfull);
            }
        }

        /// <summary>
        /// Used to indicate if a deploy is in progress
        /// </summary>
        public bool IsDeploying
        {
            get
            {
                return _isDeploying;
            }
            private set
            {
                _isDeploying = value;
                OnPropertyChanged(() => IsDeploying);
                OnPropertyChanged(() => CanDeploy);
            }
        }
        /// <summary>
        /// Can Deploy test to enable button
        /// </summary>
        public bool CanDeploy
        {
            get
            {
                return false;
            }
        }

        public string OverridesCount
        {
            get
            {
                return _overridesCount;
            }
            set
            {
                _overridesCount = value;
                OnPropertyChanged(() => OverridesCount);
            }
        }

        public string NewResourcesCount
        {
            get
            {
                return _newResourcesCount;
            }
            set
            {
                _newResourcesCount = value;
                OnPropertyChanged(() => NewResourcesCount);
            }
        }

        public string UnknownCount
        {
            get
            {
                return _unknownCount;
            }
            set
            {
                _unknownCount = value;
                OnPropertyChanged(() => UnknownCount);
            }
        }

        public string SourcesCount
        {
            get
            {
                return _sourcesCount;
            }
            set
            {
                _sourcesCount = value;
                OnPropertyChanged(() => SourcesCount);
            }
        }

        public string ServicesCount
        {
            get
            {
                return _servicesCount;
            }
            set
            {
                _servicesCount = value;
                OnPropertyChanged(() => ServicesCount);
            }
        }

        public string ConnectorsCount
        {
            get
            {
                return _connectorsCount;
            }
            set
            {
                _connectorsCount = value;
                OnPropertyChanged(() => ConnectorsCount);
            }
        }

        /// <summary>
        /// source connection
        /// </summary>
        public IConnectControlViewModel SourceConnectControlViewModel
        {
            get
            {
                return _sourceconnectControlViewModel;
            }
            set
            {
                if (Equals(value, _sourceconnectControlViewModel))
                {
                    return;
                }
                _sourceconnectControlViewModel = value;
                OnPropertyChanged(() => SourceConnectControlViewModel);
            }
        }
        /// <summary>
        /// destination connection
        /// </summary>
        public IConnectControlViewModel DestinationConnectControlViewModel
        {
            get
            {
                return _destinationConnectControlViewModel;
            }
            set
            {
                if (Equals(value, _destinationConnectControlViewModel))
                {
                    return;
                }
                _destinationConnectControlViewModel = value;
                OnPropertyChanged(() => DestinationConnectControlViewModel);
            }
        }
        /// <summary>
        /// Source Server
        /// </summary>
        public IDeploySourceExplorerViewModel Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                OnPropertyChanged("Source");
            }
        }
        /// <summary>
        /// Destination Server
        /// </summary>
        public IDeployDestinationExplorerViewModel Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                _destination = value;
                OnPropertyChanged("Destination");
            }
        }

        /// <summary>
        /// Overrides Hyperlink Clicked
        /// Must show list of New Resources conflicts
        /// </summary>
        public ICommand NewResourcesViewCommand { get; set; }
        /// <summary>
        /// Overrides Hyperlink Clicked
        /// Must show list of Override conflicts
        /// </summary>
        public ICommand OverridesViewCommand { get; set; }
        /// <summary>
        /// Deploy Button Clicked
        /// Must bring up conflict screen. Conflict screen can modify collection
        /// refresh explorer
        /// </summary>
        public ICommand DeployCommand { get; set; }
        /// <summary>
        /// Select All Dependencies. Recursive Select
        /// </summary>
        public ICommand SelectDependenciesCommand { get; set; }
        /// <summary>
        /// Stats area shows:
        ///     Service count
        ///     Workflow Count
        ///     Source Count
        ///     Unknown
        ///     New Resources in Destination
        ///     Overridden resource in Destination
        ///     Static steps of how to deploy
        /// </summary>
        public IDeployStatsViewerViewModel StatsViewModel { get; set; }

        #endregion

      
    }
}
