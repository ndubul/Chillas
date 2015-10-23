using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Warewolf.Studio.ViewModels;

namespace Warewolf.AcceptanceTesting.Deploy
{
    class DeployDestinationViewModelForTesting : DeployDestinationViewModel
    {
        public IList<IExplorerItemViewModel> Children { get; set; }

        public DeployDestinationViewModelForTesting(IShellViewModel shellViewModel, Microsoft.Practices.Prism.PubSubEvents.IEventAggregator aggregator)
            : base(shellViewModel, aggregator)
        {

        }

        #region Overrides of DeploySourceExplorerViewModel

        protected override void LoadEnvironment(IEnvironmentViewModel localhostEnvironment, bool isDeploy = false)
        {
            localhostEnvironment.Children = new ObservableCollection<IExplorerItemViewModel>(Children ?? new List<IExplorerItemViewModel> { CreateExplorerVMS() });
            PrivateObject p = new PrivateObject(localhostEnvironment);
            p.SetField("_isConnected", true);
            localhostEnvironment.ResourceId = Guid.Empty;
            AfterLoad(localhostEnvironment.Server.EnvironmentID);
        }

        IExplorerItemViewModel CreateExplorerVMS()
        {
            ExplorerItemViewModel ax = null;
            ax = new ExplorerItemViewModel(new Mock<IServer>().Object, null, (a => { }), new Mock<IShellViewModel>().Object)
            {
                ResourceName = "Examples",
                ResourcePath = "Examples",
                ResourceId = Guid.NewGuid(),
                Children = new ObservableCollection<IExplorerItemViewModel>
                {
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "Utility - Date and Times", ResourcePath = "Examples\\Utility - Date and Time" }
                    ,             new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.Parse("7CC8CA4E-8261-433F-8EF1-612DE003907C"),ResourceName = "bob", ResourcePath = "Examples\\bob" }
                    ,new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "NameIdConflict", ResourcePath = "Examples\\NameIdConflict",ResourceType = ResourceType.DbSource},
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.Parse("9CC8CA4E-8261-433F-8EF1-612DE003907C"),ResourceName = "DifferentNameSameID", ResourcePath = "Examples\\DifferentNameSameID",ResourceType = ResourceType.DbSource},

                }
            };
            return ax;
        }

        #endregion
    }
}