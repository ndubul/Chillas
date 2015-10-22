
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Deploy;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Explorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.Deploy
{
    class DeploySourceExplorerViewModelForTesting : DeploySourceExplorerViewModel
    {
        public IList<IExplorerItemViewModel> Children { get;  set; }

        public DeploySourceExplorerViewModelForTesting(IShellViewModel shellViewModel, Microsoft.Practices.Prism.PubSubEvents.IEventAggregator aggregator, IDeployStatsViewerViewModel statsArea)
            : base(shellViewModel, aggregator, statsArea)
        {

        }

        #region Overrides of DeploySourceExplorerViewModel

        protected override void LoadEnvironment(IEnvironmentViewModel localhostEnvironment, bool isDeploy = false)
        {
            localhostEnvironment.Children = new ObservableCollection<IExplorerItemViewModel>(Children ?? new List<IExplorerItemViewModel> { CreateExplorerVMS() });
            PrivateObject p = new PrivateObject(localhostEnvironment);
            p.SetField("_isConnected",true);
            localhostEnvironment.ResourceId = Guid.NewGuid();
            AfterLoad(localhostEnvironment.Server.EnvironmentID);
        }

        IExplorerItemViewModel CreateExplorerVMS()
        {
            ExplorerItemViewModel ax = null;
            ax = new ExplorerItemViewModel(new Mock<IServer>().Object, null, (a => { }), new Mock<IShellViewModel>().Object)
            {
                ResourceName = "Examples",
                ResourcePath = "Utility - Date and Time",
                ResourceId = Guid.NewGuid(),
                Children = new ObservableCollection<IExplorerItemViewModel>
                {
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "Utility - Date and Time", ResourcePath = "Examples\\Utility - Date and Time",ResourceType = ResourceType.WorkflowService },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.Parse("7CC8CA4E-8261-433F-8EF1-612DE003907C"),ResourceName = "bob", ResourcePath = "Examples\\bob" },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.Parse("5C8B5660-CE6E-4D22-84D8-5B77DC749F70"),ResourceName = "bob", ResourcePath = "sqlServers\\DemoDB" },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "Data - Data - Data Split", ResourcePath = "Examples\\Data - Data - Data Split" },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "Control Flow - Switch", ResourcePath = "Examples\\Control Flow - Switch" },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "Control Flow - Sequence", ResourcePath = "Examples\\Control Flow - Sequence" },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "File and Folder - Copy", ResourcePath = "Examples\\File and Folder - Copy" },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "File and Folder - Create", ResourcePath = "Examples\\File and Folder - Create" },
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "FetchPlayers", ResourcePath = "DB Service\\FetchPlayers",ResourceType = ResourceType.DbService},                  
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "Source", ResourcePath = "Remote\\Source",ResourceType = ResourceType.DbSource},
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) {ResourceId = Guid.NewGuid(),ResourceName = "NameIdConflict", ResourcePath = "Examples\\NameIdConflict",ResourceType = ResourceType.DbSource},

                
                }
            };
            ax.Children.Apply(a=>a.Parent = ax);
            return ax;
        }

        #endregion
    }

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
            localhostEnvironment.ResourceId = Guid.NewGuid();
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

                }
            };
            return ax;
        }

        #endregion
    }
    [Binding]
    public class DeployTabSteps
    {


        [Given(@"I have deploy tab opened")]
        public void GivenIHaveDeployTabOpened()
        {
            Core.Utils.SetupResourceDictionary();
            var shell = GetMockShellVM(true);
            var dest = new DeployDestinationViewModelForTesting(GetMockShellVM(), GetMockAggegator());
            var stats = new DeployStatsViewerViewModel(dest);
            var src = new DeploySourceExplorerViewModelForTesting(shell, GetMockAggegator(), GetStatsVM(dest)) { Children = new List<IExplorerItemViewModel> { CreateExplorerVMS() } };
            var vm = new SingleExplorerDeployViewModel(dest, src, new List<IExplorerTreeItem>(), stats, shell, GetPopup());
           // var workSurfaceContextViewModel = new WorkSurfaceContextViewModel(WorkSurfaceKeyFactory.CreateKey(WorkSurfaceContext.DeployViewer) as WorkSurfaceKey, new DeployWorksurfaceViewModel(new EventAggregator(), vm, GetPopup(), new DeployView()));
            var view = new DeployView { DataContext = vm };
            ScenarioContext.Current["View"] = view;
            Core.Utils.ShowTheViewForTesting(view);
        }

        IPopupController GetPopup()
        {
            var popup =  new Mock<IPopupController>();
            //popup.Setup(a => a.ShowDeployConflict(It.IsAny<int>())).Returns(MessageBoxResult.OK);
            ScenarioContext.Current["Popup"] = popup; 
            return popup.Object;
        }

        IDeployStatsViewerViewModel GetStatsVM(IExplorerViewModel dest)
        {
            return new DeployStatsViewerViewModel(dest);
        }

        Microsoft.Practices.Prism.PubSubEvents.IEventAggregator GetMockAggegator()
        {
            return new Mock<Microsoft.Practices.Prism.PubSubEvents.IEventAggregator>().Object;
        }

        IShellViewModel GetMockShellVM(bool setContext = false)
        {
            var shell = new Mock<IShellViewModel>();
            shell.Setup(a => a.LocalhostServer).Returns(GetMockServer());
            if(setContext)
                ScenarioContext.Current["Shell"] = shell; 
            return shell.Object;
        }

        IServer GetMockServer()
        {
            var server = new Mock<IServer>();
            var qp = new Mock<IQueryManager>();
            qp.Setup(a => a.FetchDependenciesOnList(It.IsAny<IEnumerable<Guid>>())).Returns(new List<Guid> { Guid.Parse("5C8B5660-CE6E-4D22-84D8-5B77DC749F70") });
            server.Setup(a => a.LoadExplorer()).Returns(new Task<IExplorerItem>(() => { return CreateExplorerSourceItems(); }));
            server.Setup(a => a.GetServerConnections()).Returns(GetServers());
            server.Setup(a => a.DisplayName).Returns("LocalHost");
            server.Setup(a => a.ResourceName).Returns("LocalHost");
            server.Setup(a => a.IsConnected).Returns(true);
            server.Setup(a => a.ResourceID).Returns(Guid.NewGuid());
            server.Setup(a => a.EnvironmentID).Returns(Guid.NewGuid());
            server.Setup(a => a.QueryProxy).Returns(qp.Object);
            return server.Object;
        }

        IList<IServer> GetServers()
        {
            var server = new Mock<IServer>();
            server.Setup(a => a.LoadExplorer()).Returns(new Task<IExplorerItem>(() => { return CreateExplorerSourceItems(); }));
            server.Setup(a => a.DisplayName).Returns("Remote Connection Integration");
            server.Setup(a => a.ResourceName).Returns("Remote Connection Integration");
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

        IExplorerItemViewModel CreateExplorerVMS()
        {
            ExplorerItemViewModel ax=null;
            ax = new ExplorerItemViewModel(new Mock<IServer>().Object, null, (a => { }), new Mock<IShellViewModel>().Object)
            {
                ResourceName = "Examples", Children = new ObservableCollection<IExplorerItemViewModel>
                {
                    new ExplorerItemViewModel(new Mock<IServer>().Object, ax, (a => { }), new Mock<IShellViewModel>().Object) { ResourceName = "Utility - Date and Time", ResourcePath = "Examples\\Utility - Date and Time" }
                }
            };
            return ax;
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
        Mock<IPopupController> GetPopupFromContext()   
        {
            return (Mock<IPopupController>)ScenarioContext.Current["Popup"];
        }
        Mock<IShellViewModel> GetShell()
        {
            return (Mock<IShellViewModel>)ScenarioContext.Current["Shell"];
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
            
            Assert.IsTrue(GetView().ErrorMessage.ToLower().Equals(message.ToLower())
                ||GetView().StatusPassedMessage.ToLower().Equals(message.ToLower()));
        }

        [Then(@"""(.*)"" is ""(.*)""")]
        public void ThenIs(string p0, string p1)
        {
            Assert.AreEqual(GetView().CanDeploy,p1);
      
        }
        [When(@"I Select All Dependecies")]
        public void WhenISelectAllDependecies()
        {
            GetView().SelectDependencies();
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

        [When(@"I Unselect ""(.*)"" from Source Server")]
        public void WhenIUnselectFromSourceServer(string p0)
        {
            GetView().UnSelectPath(p0);
        }

        [Then(@"""(.*)"" from Source Server is ""(.*)""")]
        public void ThenFromSourceServerIs(string p0, string p1)
        {
           Assert.AreEqual( GetView().VerifySelectPath(p0),p1);
        }

        [When(@"I type ""(.*)"" in Source Server filter")]
        public void WhenITypeInSourceServerFilter(string filter)
        {
            GetView().SetFilter(filter);
        }


        [Then(@"visibility of ""(.*)"" from Source Server is ""(.*)""")]
        public void ThenVisibilityOfFromSourceServerIs(string p0, string p1)
        {
            Assert.AreEqual(GetView().VerifySelectPathVisibility(p0), p1);
        }


        [When(@"I deploy")]
        public void WhenIDeploy()
        {
            GetView().DeployItems();
        }

        [Then(@"deploy is successfull")]
        public void ThenDeployIsSuccessfull()
        {
            GetShell().Verify(a=>a.DeployResources(It.IsAny<Guid>(),It.IsAny<Guid>(),It.IsAny<List<Guid>>()));
        }

        [Then(@"deploy is not successfull")]
        public void ThenDeployIsNotSuccessfull()
        {
            GetShell().Verify(a => a.DeployResources(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<List<Guid>>()),Times.Never());
        }


        [Then(@"Resource exists in the destination server popup is shown")]
        public void ThenResourceExistsInTheDestinationServerPopupIsShown(Table table)
        {
         
        }

        [When(@"I click Cancel on Resource exists in the destination server popup")]
        public void WhenIClickCancelOnResourceExistsInTheDestinationServerPopup()
        {
               GetPopupFromContext().Setup(a=>a.ShowDeployConflict(It.IsAny<int>())).Returns(MessageBoxResult.Cancel);
        }
        [When(@"I click OK on Resource exists in the destination server popup")]
        public void WhenIClickOKOnResourceExistsInTheDestinationServerPopup()
        {
            GetPopupFromContext().Setup(a => a.ShowDeployConflict(It.IsAny<int>())).Returns(MessageBoxResult.OK);
        }
        [Then(@"the User is prompted to ""(.*)"" one of the resources")]
        public void ThenTheUserIsPromptedToOneOfTheResources(string p0)
        {
            GetPopupFromContext().Verify(a=>a.ShowDeployNameConflict(It.IsAny<string>()));
        }


        [Then(@"Data Connectors is ""(.*)""")]
        public void ThenDataConnectorsIs(string p0)
        {
            Assert.AreEqual(GetView().Connectors , p0);
        }

        [Then(@"Services is ""(.*)""")]
        public void ThenServicesIs(string p0)
        {
            Assert.AreEqual(GetView().Services, p0);
        }


        [Then(@"Sources is ""(.*)""")]
        public void ThenSourcesIs(string p0)
        {
            Assert.AreEqual(GetView().Sources, p0);
        }
        [Then(@"New Resource is ""(.*)""")]
        public void ThenNewResourceIs(string p0)
        {
           Assert.AreEqual(GetView().New, p0);
        }
        [Then(@"Override is ""(.*)""")]
        public void ThenOverrideIs(string p0)
        {
            Assert.AreEqual(GetView().Overrides, p0);
        }


    }
}
