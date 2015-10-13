using System.Collections.Generic;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class DeployStatsViewerViewModel : BindableBase, IDeployStatsViewerViewModel {
        int _connectors;
        int _services;
        int _sources;
        int _unknown;
        int _newResources;
        int _overrides;
        string _status;

        public  DeployStatsViewerViewModel()
        {
            Status = "";
        }



        #region Implementation of IDeployStatsViewerViewModel

        /// <summary>
        /// Services being deployed
        /// </summary>
        public int Connectors
        {
            get
            {
                return _connectors;
            }
            set
            {
                _connectors = value;
                OnPropertyChanged(()=>Connectors);
            }
        }
        /// <summary>
        /// Services Being Deployed
        /// </summary>
        public int Services
        {
            get
            {
                return _services;
            }
            set
            {
                _services = value;
                OnPropertyChanged(() => Services);

            }
        }
        /// <summary>
        /// Sources being Deployed
        /// </summary>
        public int Sources
        {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
                OnPropertyChanged(() => Sources);
            }
        }
        /// <summary>
        /// What is unknown is unknown to me
        /// </summary>
        public int Unknown
        {
            get
            {
                return _unknown;
            }
            set
            {
                _unknown = value;
                OnPropertyChanged(() => Unknown);
            }
        }
        /// <summary>
        /// The count of new resources being deployed
        /// </summary>
        public int NewResources
        {
            get
            {
                return _newResources;
            }
            set
            {
                _newResources = value;
                OnPropertyChanged(() => NewResources);
            }
        }
        /// <summary>
        /// The count of overidded resources
        /// </summary>
        public int Overrides
        {
            get
            {
                return _overrides;
            }
            set
            {
                _overrides = value;
                OnPropertyChanged(()=> Overrides);
            }
        }
        /// <summary>
        /// The status of the last deploy
        /// </summary>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged(() => Status);
            }
        }

        public void Calculate(IList<IExplorerTreeItem> items)
        {
        }

        public void Calculate(IExplorerTreeItem items, IExplorerTreeItem destination)
        {

        }

        #endregion
    }
}