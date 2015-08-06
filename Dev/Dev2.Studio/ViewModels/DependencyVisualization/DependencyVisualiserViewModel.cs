
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Dev2.AppResources.DependencyVisualization;
using Dev2.AppResources.Repositories;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Services.Events;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.ViewModels.WorkSurface;
using Dev2.Studio.Views.DependencyVisualization;
using Dev2.ViewModels.DependencyVisualization;
using Microsoft.Practices.Prism.Mvvm;
using Moq;
using Warewolf.Studio.ViewModels;

// ReSharper disable CheckNamespace
namespace Dev2.Studio.ViewModels.DependencyVisualization
// ReSharper restore CheckNamespace
{
    public class DependencyVisualiserViewModel : BaseWorkSurfaceViewModel
    {
        readonly DependencyVisualiserView _view;
        private IContextualResourceModel _resourceModel;
        //private ObservableCollection<Graph> _graphs;

        public DependencyVisualiserViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public DependencyVisualiserViewModel(DependencyVisualiserView view)
            : base(EventPublishers.Aggregator)
        {
            _view = view;
        }

        //public ObservableCollection<Graph> Graphs
        //{
        //    get { return _graphs ?? (_graphs = new ObservableCollection<Graph>()); }
        //}

        private double _availableWidth;
        public double AvailableWidth
        {
            get
            {
                return _availableWidth;
            }
            set
            {
                if (_availableWidth.CompareTo(value) == 0)
                {
                    return;
                }

                _availableWidth = value;
             
                NotifyOfPropertyChange(() => AvailableWidth);
            }
        }

        public ResourceType ResourceType { get; set; }
        private double _availableHeight;
        ObservableCollection<ExplorerItemNodeViewModel> _allNodes;

        public double AvailableHeight
        {
            get
            {
                return _availableHeight;
            }
            set
            {
                if (_availableHeight.CompareTo(value) == 0)
                {
                    return;
                }

                _availableHeight = value;
                //BuildGraphs();
                NotifyOfPropertyChange(() => AvailableHeight);
            }
        }

        public IContextualResourceModel ResourceModel
        {
            get
            {
                return _resourceModel;
            }
            set
            {
                if (_resourceModel == value) return;

                _resourceModel = value;
                BuildGraphs();
                NotifyOfPropertyChange(() => ResourceModel);
                if (value != null)
                    NotifyOfPropertyChange(() => DisplayName);
            }
        }

        public bool GetDependsOnMe { get; set; }

        public override string DisplayName
        {
            get
            {
                return string.Format(GetDependsOnMe ? "{0}*Dependants"
                    : "{0}*Dependencies", ResourceModel.ResourceName);
            }
        }

        // NOTE: This method is invoked from DependencyVisualiser.xaml
        public void BuildGraphs()
        {
            //Graphs.Clear();

            var repo = ResourceModel.Environment.ResourceRepository;
            var repos = StudioResourceRepository.Instance;
            var graphData = repo.GetDependenciesXml(ResourceModel, GetDependsOnMe);

            if (graphData == null)
            {
                throw new Exception(string.Format(GlobalConstants.NetworkCommunicationErrorTextFormat, "GetDependenciesXml"));
            }

            var graphGenerator = new DependencyGraphGenerator();

            var graph = graphGenerator.BuildGraph(graphData.Message, ResourceModel.ResourceName, AvailableWidth, AvailableHeight);
            var acc = new List<ExplorerItemNodeViewModel>();
            var x = new ObservableCollection<ExplorerItemNodeViewModel>(GetItems(new List<Node>{graph.Nodes.FirstOrDefault()}, StudioResourceRepository.Instance, null, acc));
            AllNodes = new ObservableCollection<ExplorerItemNodeViewModel>(acc);
            
//Graphs.Add(graph);
        }
       

        public ObservableCollection<ExplorerItemNodeViewModel> AllNodes
        {
            get
            {
                return _allNodes;
            }
            set
            {
                _allNodes = value;
                NotifyOfPropertyChange(() => AllNodes);
            }
        }

        public ICollection<ExplorerItemNodeViewModel> GetItems(List<Node> nodes, IStudioResourceRepository repo, IExplorerItemNodeViewModel parent, List<ExplorerItemNodeViewModel> acc)
        {
            var server = CustomContainer.Get<IServer>();
            List<ExplorerItemNodeViewModel> items = new List<ExplorerItemNodeViewModel>(nodes.Count);
            foreach (var node in nodes)
            {
                var exploreritem = repo.FindItemById(Guid.Parse(node.ID));
                ExplorerItemNodeViewModel item = new ExplorerItemNodeViewModel(server, parent)
                {
                    ResourceName = exploreritem.DisplayName,
                    TextVisibility = true,
                    ResourceType = exploreritem.ResourceType,
                    IsMainNode = Visibility.Visible
                };

                if (node.NodeDependencies != null && node.NodeDependencies.Count > 0)
                    item.Children = new ObservableCollection<IExplorerItemViewModel>(GetItems(node.NodeDependencies, repo, item,acc).Select(a => a as IExplorerItemViewModel));
                else
                {
                   item.Children = new ObservableCollection<IExplorerItemViewModel>();
                }
                items.Add(item);
                //if (!acc.Exists(a => a.ResourceId == item.ResourceId))
                    acc.Add(item);
            }
            return items;
        }

        public bool TextVisibility { get; set; }
        public override object GetView(object context = null)
        {
            return _view;
        }

        protected override void OnViewLoaded(object view)
        {
            var loadedView = view as IView;
            if (loadedView != null)
            {
                loadedView.DataContext = this;
                base.OnViewLoaded(loadedView);
            }
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, this);
        }

    }
}
