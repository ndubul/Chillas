using Caliburn.Micro;
using Dev2.AppResources.Repositories;
using Dev2.Common.Interfaces.Threading;
using Dev2.ConnectionHelpers;
using Dev2.CustomControls.Connections;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Deploy;
using Dev2.Studio.ViewModels.Deploy;
using Dev2.Studio.Views.Deploy;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;

namespace Warewolf.AcceptanceTesting.Deploy
{
    [Binding]
    public class DeployTabSteps
    {
        [BeforeFeature("Deploy")]
        public static void SetupForFeature()
        {
      
            Utils.SetupResourceDictionary();
            
            var mockEventAggregator = new Mock<IEventAggregator>();
            var asyncWorker = new Mock<IAsyncWorker>();
            var envProvider = new Mock<IEnvironmentModelProvider>();
            var envRepo = new Mock<IEnvironmentRepository>();
            var resRepo = new Mock<IStudioResourceRepository>();
            IView deployView = new DeployView();
            var connectControlLeft = new Mock<IConnectControlViewModel>();
            var connectColtrolRight = new Mock<IConnectControlViewModel>();
            var mainConnectControl = new Mock<IConnectControlSingleton>();
            var calc = new Mock<IDeployStatsCalculator>();
            //IAsyncWorker asyncWorker, IEnvironmentModelProvider serverProvider, IEnvironmentRepository environmentRepository, IEventAggregator eventAggregator, IStudioResourceRepository studioResourceRepository, 
            //IConnectControlViewModel sourceConnectControlVm, IConnectControlViewModel destinationConnectControlVm, IDeployStatsCalculator deployStatsCalculator = null, Guid? resourceID = null, Guid? environmentID = null,IConnectControlSingleton connectControlSingleton = null)
            var viewModel = new DeployViewModel(asyncWorker.Object, envProvider.Object, envRepo.Object,mockEventAggregator.Object,resRepo.Object,connectColtrolRight.Object,connectControlLeft.Object,calc.Object,null,null,mainConnectControl.Object);
            deployView.DataContext = viewModel;
            FeatureContext.Current.Add("eventAggregator",mockEventAggregator);
            FeatureContext.Current.Add("asyncWorker", asyncWorker);
            FeatureContext.Current.Add("envProvider", envProvider);
            FeatureContext.Current.Add("envRepo", envRepo);
            FeatureContext.Current.Add("resRepo", resRepo);
            FeatureContext.Current.Add("deployView", deployView);
            FeatureContext.Current.Add("viewModel", viewModel);
            FeatureContext.Current.Add("calc", calc);
            Utils.ShowTheViewForTesting(deployView);
        }

        [Given(@"I have deploy tab opened")]
        public void GivenIHaveDeployTabOpened()
        {
            var view = ScenarioContext.Current.Get<DeployView>("deployView");
            Assert.IsNotNull(view);
        }

        [Given(@"selected Source Server is ""(.*)""")]
        public void GivenSelectedSourceServerIs(string server)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"selected Destination Server is ""(.*)""")]
        public void WhenSelectedDestinationServerIs(string server)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the validation message is ""(.*)""")]
        public void ThenTheValidationMessageIs(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"""(.*)"" is ""(.*)""")]
        public void ThenIs(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"selected Destination Server is ""(.*)""")]
        public void GivenSelectedDestinationServerIs(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I select ""(.*)"" from Source Server")]
        public void WhenISelectFromSourceServer(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I select ""(.*)"" from Source Server")]
        public void GivenISelectFromSourceServer(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I deploy")]
        public void WhenIDeploy()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"deploy is successfull")]
        public void ThenDeployIsSuccessfull()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"""(.*)"" is visible on Destination Server")]
        public void ThenIsVisibleOnDestinationServer(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I deploy")]
        public void GivenIDeploy()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Resource exists in the destination server popup is shown")]
        public void ThenResourceExistsInTheDestinationServerPopupIsShown(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I click OK on Resource exists in the destination server popup")]
        public void WhenIClickOKOnResourceExistsInTheDestinationServerPopup()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I click Cancel on Resource exists in the destination server popup")]
        public void WhenIClickCancelOnResourceExistsInTheDestinationServerPopup()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"deploy is not successfull")]
        public void ThenDeployIsNotSuccessfull()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I Select All Dependecies")]
        public void WhenISelectAllDependecies()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"""(.*)"" from Source Server is ""(.*)""")]
        public void ThenFromSourceServerIs(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I type ""(.*)"" in Source Server filter")]
        public void WhenITypeInSourceServerFilter(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I clear filter on Source Server")]
        public void WhenIClearFilterOnSourceServer()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I type ""(.*)"" in Destination Server filter")]
        public void WhenITypeInDestinationServerFilter(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I select ""(.*)"" from Source Server")]
        public void ThenISelectFromSourceServer(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"""(.*)"" from Destination Server is ""(.*)""")]
        public void ThenFromDestinationServerIs(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Data Connectors is ""(.*)""")]
        public void ThenDataConnectorsIs(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Services is ""(.*)""")]
        public void ThenServicesIs(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Sources is ""(.*)""")]
        public void ThenSourcesIs(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"New Resource is ""(.*)""")]
        public void ThenNewResourceIs(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Override is ""(.*)""")]
        public void ThenOverrideIs(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I Unselect ""(.*)"" from Source Server")]
        public void WhenIUnselectFromSourceServer(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"Override is ""(.*)""")]
        public void WhenOverrideIs(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"""(.*)"" popup is shown")]
        public void ThenPopupIsShown(string p0)
        {
            ScenarioContext.Current.Pending();
        }



//
//        [BeforeScenario("Deploy")]
//        public void SetupForScenerio()
//        {
//            ScenarioContext.Current.Add(Utils.ViewNameKey, FeatureContext.Current.Get<IDeployViewControl>(Utils.ViewNameKey));
//            ScenarioContext.Current.Add(Utils.ViewModelNameKey, FeatureContext.Current.Get<IDeployViewModel>(Utils.ViewModelNameKey));
//        }
//
//        [Given(@"I have deploy tab opened")]
//        public void GivenIHaveDeployTabOpened()
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            Assert.IsNotNull(view);
//        }
//
//        [Given(@"selected Source Server is ""(.*)""")]
//        [When(@"selected Source Server is ""(.*)""")]
//        public void GivenSelectedSourceServerIs(string sourceServerName)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.SelectSourceServer(sourceServerName);
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            Assert.AreEqual(viewModel.SelectedSourceModel.Server.ResourceName,sourceServerName);
//        }
//
//
//        [Given(@"selected Destination Server is ""(.*)""")]
//        [When(@"selected Destination Server is ""(.*)""")]
//        public void GivenSelectedDestinationServerIs(string destinationServer)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.SelectDestinationServer(destinationServer);
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            Assert.AreEqual(viewModel.SelectedDestinationModel.Server.ResourceName, destinationServer);
//        }
//
//        [Given(@"I select ""(.*)"" from Source Server")]
//        [When(@"I select ""(.*)"" from Source Server")]
//        public void GivenISelectFromSourceServer(string resourceName)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.SelectSourceResource(resourceName);
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            var selectedItems = viewModel.SelectedSourceModel.SelectedItems;
//            Assert.AreEqual(1,selectedItems.Count());
//        }
//
//        [Given(@"I deploy")]
//        [When(@"I deploy")]
//        public void GivenIDeploy()
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.Deploy();
//        }
//
//        [When(@"I click OK on Resource exists in the destination server popup")]
//        public void WhenIClickOKOnPopup()
//        {
//            var mockPopupController = ScenarioContext.Current.Get<Mock<IPopupController>>();
//            mockPopupController.Setup(controller => controller.Show(It.IsAny<IPopupMessage>())).Returns(MessageBoxResult.OK);
//        }
//
//        [When(@"I click Cancel on Resource exists in the destination server popup")]
//        public void WhenIClickCancelOnPopup()
//        {
//            var mockPopupController = ScenarioContext.Current.Get<Mock<IPopupController>>();
//            mockPopupController.Setup(controller => controller.Show(It.IsAny<IPopupMessage>())).Returns(MessageBoxResult.OK);
//        }
//        
//        [When(@"I Select All Dependecies")]
//        public void WhenISelectAllDependecies()
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.SelectAllDependencies();
//        }
//
//        [When(@"I type ""(.*)"" in Destination Server filter")]
//        public void WhenITypeInDestinationServerFilter(string filterTerm)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.EnterDestinationFilter(filterTerm);
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            Assert.AreEqual(filterTerm,viewModel.Destination.SearchText);
//        }
//
//        [When(@"I clear filter on Destination Server")]
//        public void WhenIClearFilterOnDestinationServer()
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.ClearDestinationFilter();
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            Assert.AreEqual(string.Empty, viewModel.Destination.SearchText);
//        }
//
//        [When(@"I type ""(.*)"" in Source Server filter")]
//        public void WhenITypeInSourceServerFilter(string filterTerm)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.EnterSourceFilter(filterTerm);
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            Assert.AreEqual(filterTerm, viewModel.Source.SearchText);
//        }
//
//        [When(@"I clear filter on Source Server")]
//        public void WhenIClearFilterOnSourceServer()
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            view.ClearSourceFilter();
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            Assert.AreEqual(string.Empty, viewModel.Source.SearchText);
//        }
//
//        [Then(@"the validation message as ""(.*)""")]
//        public void ThenTheValidationMessageAs(string validationMessage)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            string currentValidationMessage = view.GetCurrentValidationMessage();
//            Assert.AreEqual(validationMessage,currentValidationMessage);
//        }
//
//        [Then(@"deploy is successfull")]
//        public void ThenDeployIsSuccessfull()
//        {
//            ScenarioContext.Current.Pending();
//        }
//
//        [Then(@"""(.*)"" is visible on Destination Server")]
//        public void ThenIsVisibleOnDestinationServer(string resourceName)
//        {
//            //Is this going to be mocked?
//            ScenarioContext.Current.Pending();
//        }
//
//        [Then(@"Resource exists in the destination server popup is shown")]
//        public void ThenPopupIsShown(Table table)
//        {
//            //Get Pop?
//            ScenarioContext.Current.Pending();
//        }
//
//        [Then(@"deploy is not successfull")]
//        public void ThenDeployIsNotSuccessfull()
//        {
//            ScenarioContext.Current.Pending();
//        }
//
//        [Then(@"""(.*)"" from Source Server is ""(.*)""")]
//        public void ThenFromSourceServerIs(string resourceName, string state)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            if (state.ToLowerInvariant() == "visible")
//            {
//                bool isVisible = view.IsSourceResourceIsVisible(resourceName);
//                Assert.IsTrue(isVisible);
//            }
//            else if(state.ToLowerInvariant() == "selected")
//            {
//                bool isSelected = view.IsSourceResourceSelected(resourceName);
//                Assert.IsTrue(isSelected);
//            }
//        }
//
//        [Then(@"""(.*)"" from Destination Server is ""(.*)""")]
//        public void ThenFromDestinationServerIs(string resourceName, string state)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            if (state.ToLowerInvariant() == "visible")
//            {
//                bool isVisible = view.IsDestinationResourceIsVisible(resourceName);
//                Assert.IsTrue(isVisible);
//            }
//            else if (state.ToLowerInvariant() == "selected")
//            {
//                bool isSelected = view.IsDestinationResourceSelected(resourceName);
//                Assert.IsTrue(isSelected);
//            }
//        }
//
//        [Then(@"I select ""(.*)"" from Source Server")]
//        public void ThenISelectFromSourceServer(string resourceName)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            view.SelectSourceServerResource(resourceName);
//            Assert.AreEqual(resourceName,viewModel.SelectedSourceModel.SelectedItems);
//        }
//
//
//        [Then(@"Data Connectors is ""(.*)""")]
//        public void ThenDataConnectorsIs(int numberOfDataConnectors)
//        {
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            var deployStatsTos = viewModel.Stats.FirstOrDefault(to => to.Name == "Data Connector");         
//            Assert.IsNotNull(deployStatsTos);
//            Assert.AreEqual(numberOfDataConnectors,deployStatsTos.Description);
//        }
//
//        [Then(@"Services is ""(.*)""")]
//        public void ThenServicesIs(int numberOfServices)
//        {
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            var deployStatsTos = viewModel.Stats.FirstOrDefault(to => to.Name == "Services");
//            Assert.IsNotNull(deployStatsTos);
//            Assert.AreEqual(numberOfServices, deployStatsTos.Description);
//        }
//
//        [Then(@"Sources is ""(.*)""")]
//        public void ThenSourcesIs(int numberOfSources)
//        {
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            var deployStatsTos = viewModel.Stats.FirstOrDefault(to => to.Name == "Sources");
//            Assert.IsNotNull(deployStatsTos);
//            Assert.AreEqual(numberOfSources, deployStatsTos.Description);
//        }
//
//        [Then(@"New Resource is ""(.*)""")]
//        public void ThenNewResourceIs(int numberOfNewResources)
//        {
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            var deployStatsTos = viewModel.Stats.FirstOrDefault(to => to.Name == "New Resource");
//            Assert.IsNotNull(deployStatsTos);
//            Assert.AreEqual(numberOfNewResources, deployStatsTos.Description);
//        }
//
//        [Given(@"Override is ""(.*)""")]
//        [When(@"Override is ""(.*)""")]
//        [Then(@"Override is ""(.*)""")]
//        public void ThenOverrideIs(int numberOfOverridingResources)
//        {
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            var deployStatsTos = viewModel.Stats.FirstOrDefault(to => to.Name == "Override");
//            Assert.IsNotNull(deployStatsTos);
//            Assert.AreEqual(numberOfOverridingResources, deployStatsTos.Description);
//        }
//
//        [When(@"I Unselect ""(.*)"" from Source Server")]
//        public void WhenIUnselectFromSourceServer(string resourceName)
//        {
//            var view = Utils.GetView<IDeployViewControl>();
//            var viewModel = Utils.GetViewModel<IDeployViewModel>();
//            view.DeselectSourceServerResource(resourceName);
//            Assert.AreNotEqual(resourceName, viewModel.SelectedSourceModel.SelectedItems);
//        }
//
//        [Then(@"""(.*)"" popup is shown")]
//        public void ThenPopupIsShown(string p0)
//        {
//            ScenarioContext.Current.Pending();
//        }
//
    }
}
