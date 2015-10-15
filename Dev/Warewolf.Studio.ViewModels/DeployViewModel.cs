using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Deploy;
using Dev2.Common.Interfaces.Studio.Controller;
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
        bool _serversAreNotTheSame;
        bool _canSelectDependencies;
        readonly IPopupController _popupController;
        bool _showNewItemsList;
        bool _showConflictItemsList;
        IList<Conflict> _conflictItems;
        IList<IExplorerTreeItem> _newItems;

        #region Implementation of IDeployViewModel

        public SingleExplorerDeployViewModel(IDeployDestinationExplorerViewModel destination, IDeploySourceExplorerViewModel source, IEnumerable<IExplorerTreeItem> selectedItems, IDeployStatsViewerViewModel stats, IShellViewModel shell, IPopupController popupController)
        {
            VerifyArgument.AreNotNull(new Dictionary<string, object> { { "destination", destination }, { "source", source }, { "selectedItems", selectedItems }, { "stats", stats }, { "popupController", popupController } });
            _destination = destination;
            _popupController = popupController;

            _source = source;
            _source.SelectItemsForDeploy(selectedItems);
            _stats = stats;
            _shell = shell;
            _stats.CalculateAction = () =>
            {
                ConnectorsCount = _stats.Connectors.ToString();
                ServicesCount = _stats.Services.ToString();
                SourcesCount = _stats.Sources.ToString();
                UnknownCount = _stats.Unknown.ToString();
                NewResourcesCount = _stats.NewResources.ToString();
                OverridesCount = _stats.Overrides.ToString();
                ConflictItems = _stats.Conflicts;
                NewItems = _stats.New;
                ShowConflicts = false;
            };
            SourceConnectControlViewModel = _source.ConnectControlViewModel;
            DestinationConnectControlViewModel = _destination.ConnectControlViewModel;

            SourceConnectControlViewModel.SelectedEnvironmentChanged += UpdateServerCompareChanged;
            DestinationConnectControlViewModel.SelectedEnvironmentChanged += UpdateServerCompareChanged;

            DeployCommand = new DelegateCommand(Deploy, () => CanDeploy);
            SelectDependenciesCommand = new DelegateCommand(SelectDependencies, () => CanSelectDependencies);
            NewResourcesViewCommand = new DelegateCommand(ViewNewResources);
            OverridesViewCommand = new DelegateCommand(ViewOverrides);

            ShowConflicts = false;
        }

        public bool CanSelectDependencies
        {
            get
            {
                return Source.SelectedItems.Count > 0;
            }
        }

        public IList<IExplorerTreeItem> NewItems
        {
            get
            {
                return _newItems;
            }
            set
            {
                _newItems = value;
                OnPropertyChanged(() => NewItems);
            }
        }

        public IList<Conflict> ConflictItems
        {
            get
            {
                return _conflictItems;
            }
            set
            {
                _conflictItems = value;
                OnPropertyChanged(()=>ConflictItems);
            }
        }

        void UpdateServerCompareChanged(object sender, Guid environmentid)
        {
            ServersAreNotTheSame = DestinationConnectControlViewModel.SelectedConnection.EnvironmentID != SourceConnectControlViewModel.SelectedConnection.EnvironmentID;
            ShowConflicts = false;
            ConnectorsCount = _stats.Connectors.ToString();
            ServicesCount = _stats.Services.ToString();
            SourcesCount = _stats.Sources.ToString();
            UnknownCount = _stats.Unknown.ToString();
            NewResourcesCount = _stats.NewResources.ToString();
            OverridesCount = _stats.Overrides.ToString();
        }

        void ViewOverrides()
        {
            ShowConflicts = true;
            ConflictNewResourceText = "List of Overrides";
            ShowNewItemsList = false;
            ShowConflictItemsList = true;
        }

        public bool ShowConflictItemsList
        {
            get
            {
                return _showConflictItemsList;
            }
            set
            {
                _showConflictItemsList = value;
                OnPropertyChanged(() => ShowConflictItemsList);
            }
        }

        public bool ShowNewItemsList
        {
            get
            {
                return _showNewItemsList;
            }
            set
            {
                _showNewItemsList = value;
                OnPropertyChanged(() => ShowNewItemsList);
            }
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
            ShowNewItemsList = true;
            ShowConflictItemsList = false;
        }

        void Deploy()
        {
            bool canDeploy = false;
            if (ConflictItems.Count >= 1)
            {
                var msgResult = _popupController.ShowDeployConflict(ConflictItems.Count);
                if (msgResult == MessageBoxResult.Yes)
                {
                    canDeploy = true;
                }
            }
            if (!canDeploy)
            {
                ViewOverrides();
            }
            else
            {
                var selected = Source.SelectedItems.Where(a => a.ResourceType != ResourceType.Folder);
                var notfolders = selected.Select(a => a.ResourceId).ToList();
                _shell.DeployResources(Source.Environments.First().Server.EnvironmentID, Destination.SelectedEnvironment.Server.EnvironmentID, notfolders);
            }
        }

        public void SelectDependencies()
        {
            if (Source != null && Source.SelectedEnvironment != null && Source.SelectedEnvironment.Server != null)
            {
                var guids = Source.SelectedEnvironment.Server.QueryProxy.FetchDependenciesOnList(Source.SelectedItems.Select(a => a.ResourceId));
                Source.SelectedEnvironment.AsList().Where(a => guids.Contains(a.ResourceId)).Apply(a => a.IsResourceChecked = true);
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
                if (Source == null)
                {
                    return false;

                }
                if (Source.SelectedEnvironment == null || !Source.SelectedEnvironment.IsConnected)
                {
                    return false;
                }
                if (Destination == null)
                {
                    return false;

                }
                if (Destination.SelectedEnvironment == null || !Destination.SelectedEnvironment.IsConnected)
                {
                    return false;
                }
                if (Source.SelectedItems == null || Source.SelectedItems.Count <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// check is source and destination are the same
        /// </summary>
        public bool ServersAreNotTheSame
        {
            get
            {
                return _serversAreNotTheSame;
            }
            set
            {
                _serversAreNotTheSame = value;
                OnPropertyChanged(() => ServersAreNotTheSame);
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
                RaiseCanExecuteDependencies();
                OnPropertyChanged(() => OverridesCount);
            }
        }

        void RaiseCanExecuteDependencies()
        {
            var delegateCommand = SelectDependenciesCommand as DelegateCommand;
            if (delegateCommand != null)
            {
                delegateCommand.RaiseCanExecuteChanged();
            }

            delegateCommand = DeployCommand as DelegateCommand;
            if (delegateCommand != null)
            {
                delegateCommand.RaiseCanExecuteChanged();
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
                RaiseCanExecuteDependencies();
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
                if (!Equals(_source, value))
                {
                    _source = value;
                    ShowConflicts = false;
                }
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
