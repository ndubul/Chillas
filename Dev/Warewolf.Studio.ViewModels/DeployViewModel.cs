using System.Collections.Generic;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class DeployViewModel:BindableBase, IDeployViewModel
    {
        IDeploySourceExplorerViewModel _source;
        readonly IDeployStatsViewerViewModel _stats;
        IDeployDestinationExplorerViewModel _destination;

        #region Implementation of IDeployViewModel

        public DeployViewModel(IDeployDestinationExplorerViewModel destination, IDeploySourceExplorerViewModel source,IEnumerable<IExplorerTreeItem> selectedItems,IDeployStatsViewerViewModel stats)
        {
            _destination = destination;
            _destination.SelectItemsForDeploy(selectedItems);
            _source = source;

            _stats = stats;
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
                OnPropertyChanged(()=>Source);
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
                OnPropertyChanged(()=>Destination);
            }
        }
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
