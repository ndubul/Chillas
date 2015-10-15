using System;
using System.Collections.Generic;
using System.Linq;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Deploy;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class DeployStatsViewerViewModel : BindableBase, IDeployStatsViewerViewModel
    {
        readonly IExplorerViewModel _destination;
        int _connectors;
        int _services;
        int _sources;
        int _unknown;
        int _newResources;
        int _overrides;
        string _status;
        List<Conflict> _conflicts;
        IEnumerable<IExplorerTreeItem> _new;
        Action _calculateAction;

        public DeployStatsViewerViewModel(IExplorerViewModel destination)
        {
            VerifyArgument.IsNotNull("destination", destination);
            _destination = destination;
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
                OnPropertyChanged(() => Connectors);
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
                OnPropertyChanged(() => Overrides);
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
            if (items != null)
            {
                Connectors = items.Count(a => a.ResourceType >= ResourceType.DbService && a.ResourceType <= ResourceType.WebService);
                Services = items.Count(a => a.ResourceType == ResourceType.WorkflowService);
                Sources = items.Count(a => IsSource(a.ResourceType));
                Unknown = items.Count(a => a.ResourceType == ResourceType.Unknown);
                if (_destination.SelectedEnvironment != null)
                {
                    var conf = from b in _destination.SelectedEnvironment.AsList()
                               join explorerTreeItem in items on b.ResourceId equals explorerTreeItem.ResourceId
                               where b.ResourceType != ResourceType.Folder && explorerTreeItem.ResourceType != ResourceType.Folder
                               select new Conflict { SourceName = explorerTreeItem.ResourceName, DestinationName = b.ResourceName };

                    _conflicts = conf.ToList();
                    _new = items.Except(_destination.SelectedEnvironment.AsList());
                }
                else
                {
                    _conflicts = new List<Conflict>();
                    _new = new List<IExplorerTreeItem>();
                }

                Overrides = Conflicts.Count;
                NewResources = New.Count;
            }
            else
            {
                Connectors = 0;
                Services = 0;
                Sources = 0;
                Unknown = 0;
                _conflicts = new List<Conflict>();
                _new = new List<IExplorerTreeItem>();
            }

            OnPropertyChanged(() => Conflicts);
            OnPropertyChanged(() => New);
            if (CalculateAction != null)
            {
                CalculateAction();
            }
        }

        public IList<Conflict> Conflicts
        {
            get
            {
                return _conflicts.ToList();
            }
        }
        public IList<IExplorerTreeItem> New
        {
            get
            {
                return _new.Where(a => a.ResourceType != ResourceType.Folder).ToList();
            }
        }
        public Action CalculateAction
        {
            get
            {
                return _calculateAction;
            }
            set
            {
                _calculateAction = value;
            }
        }

        bool IsSource(ResourceType res)
        {
            return (res >= ResourceType.DbSource && res <= ResourceType.SharepointServerSource) || (res == ResourceType.DropboxSource);
        }
        #endregion
    }

}