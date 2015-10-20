
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Deploy;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Explorer;
using Dev2.Factory;
using Dev2.Providers.Events;
using Dev2.Studio.AppResources.Comparers;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.ViewModels.WorkSurface;
using Dev2.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.Studio.AntiCorruptionLayer;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.Deploy
{
    [Binding]
    public class DeployTabSteps
    {


        [Given(@"I have deploy tab opened")]
        public void GivenIHaveDeployTabOpened()
        {
            Core.Utils.SetupResourceDictionary();
            var dest = new DeployDestinationViewModel(GetMockShellVM(),GetMockAggegator() );
            var stats = new DeployStatsViewerViewModel(dest);
            var vm = new SingleExplorerDeployViewModel(dest, new DeploySourceExplorerViewModel(GetMockShellVM(), GetMockAggegator(),GetStatsVM(dest)),new List<IExplorerTreeItem>(), stats, GetMockShellVM(), GetPopup());
           // var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DeployViewer) as WorkSurfaceKey, new DeployWorksurfaceViewModel(new EventAggregator(), vm, GetPopup(), new DeployView()));
            var view = new DeployView { DataContext = vm };
            ScenarioContext.Current["View"] = view;
            Core.Utils.ShowTheViewForTesting(view);
        }

        IPopupController GetPopup()
        {
            return new Mock<IPopupController>().Object;
        }

        IDeployStatsViewerViewModel GetStatsVM(IExplorerViewModel dest)
        {
            return new DeployStatsViewerViewModel(dest);
        }

        Microsoft.Practices.Prism.PubSubEvents.IEventAggregator GetMockAggegator()
        {
            return new Mock<Microsoft.Practices.Prism.PubSubEvents.IEventAggregator>().Object;
        }

        IShellViewModel GetMockShellVM()
        {
            var shell = new Mock<IShellViewModel>();
            shell.Setup(a => a.LocalhostServer).Returns(GetMockServer());
            return shell.Object;
        }

        IServer GetMockServer()
        {
            var server = new Mock<IServer>();
            server.Setup(a => a.LoadExplorer()).Returns(new Task<IExplorerItem>(() => { return CreateExplorerSourceItems(); }));
            server.Setup(a => a.GetServerConnections()).Returns(GetServers());
            server.Setup(a => a.DisplayName).Returns("LocalHost");
            server.Setup(a => a.ResourceName).Returns("LocalHost");
            return server.Object;
        }

        IList<IServer> GetServers()
        {
            var server = new Mock<IServer>();
            server.Setup(a => a.LoadExplorer()).Returns(new Task<IExplorerItem>(() => { return CreateExplorerSourceItems(); }));
            server.Setup(a => a.GetServerConnections()).Returns(GetServers());
            server.Setup(a => a.DisplayName).Returns("Remote");
            server.Setup(a => a.ResourceName).Returns("Remote");
            return new List<IServer>
            {
                server.Object
            };
        }

        IExplorerItem CreateExplorerSourceItems()
        {
            return new ServerExplorerItem()
            {
                DisplayName = "Examples",
                Children = new List<IExplorerItem>
                {
                    new ServerExplorerItem(){DisplayName = "Utility - Date and Time" , ResourcePath = "Examples\\Utility - Date and Time" }
                }
                
            };
        }

        [Given(@"selected Source Server is ""(.*)""")]
        public void GivenSelectedSourceServerIs(string p0)
        {
            Assert.IsTrue(GetView().SelectedServer.DisplayName.ToLower().Equals(p0.ToLower()));
        }
        DeployView GetView()
        {
            return (DeployView)ScenarioContext.Current["View"];
        }


        [When(@"selected Destination Server is ""(.*)""")]
        public void WhenSelectedDestinationServerIs(string d)
        {
            GetView().SelectDestinationServer(d);
        }
        [Given(@"selected Destination Server is ""(.*)""")]
        public void GivenSelectedDestinationServerIs(string d)
        {
            Assert.IsTrue(GetView().SelectedDestinationServer.DisplayName.ToLower().Equals(d.ToLower()));
        }


        [Then(@"the validation message is ""(.*)""")]
        public void ThenTheValidationMessageIs(string message)
        {
            Assert.IsTrue(GetView().ErrorMessage.ToLower().Equals(message.ToLower()));
        }

        [Then(@"""(.*)"" is ""(.*)""")]
        public void ThenIs(string p0, string p1)
        {
            Assert.AreEqual(GetView().CanDeploy,p1);
      
        }
        [Then(@"Select All Dependencies is ""(.*)""")]
        public void ThenSelectAllDependenciesIs(string p0)
        {
            Assert.AreEqual(GetView().CanSelectDependencies, p0);
        }

        [When(@"I select ""(.*)"" from Source Server")]
        public void WhenISelectFromSourceServer(string p0)
        {
            GetView().SelectPath(p0);
        }




    }
}
