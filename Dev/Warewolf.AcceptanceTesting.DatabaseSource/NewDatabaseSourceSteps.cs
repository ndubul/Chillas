//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Windows;
//using Dev2.Common.Interfaces;
//using Dev2.Common.Interfaces.SaveDialog;
//using Dev2.Common.Interfaces.ServerProxyLayer;
//using Dev2.Runtime.ServiceModel.Data;
//using Dev2.Threading;
//using Microsoft.Practices.Prism.PubSubEvents;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using TechTalk.SpecFlow;
//using Warewolf.AcceptanceTesting.Core;
//using Warewolf.Studio.ServerProxyLayer;
//using Warewolf.Studio.ViewModels;
//using Warewolf.Studio.Views;
//
//namespace Warewolf.AcceptanceTesting.DatabaseService
//{
//    [Binding]
//    public class NewDatabaseSourceSteps
//    {
//        [BeforeFeature("CreatingNewDBSource")]
//        public static void SetupForSystem()
//        {
//            Utils.SetupResourceDictionary();
//            var databaseSourceControlView = new ManageDatabaseSourceControl();
//            var mockStudioUpdateManager = new Mock<IManageDatabaseSourceModel>();
//            mockStudioUpdateManager.Setup(model => model.GetComputerNames()).Returns(new List<string> { "TEST", "RSAKLFSVRGENDEV", "RSAKLFSVROTHER" });
//            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
//            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Returns(MessageBoxResult.OK);
//            var mockEventAggregator = new Mock<IEventAggregator>();
//            var manageDatabaseSourceControlViewModel = new ManageDatabaseSourceViewModel(mockStudioUpdateManager.Object,mockRequestServiceNameViewModel.Object, mockEventAggregator.Object,new SynchronousAsyncWorker());
//            databaseSourceControlView.DataContext = manageDatabaseSourceControlViewModel;
//            Utils.ShowTheViewForTesting(databaseSourceControlView);
//            FeatureContext.Current.Add(Utils.ViewNameKey, databaseSourceControlView);
//            FeatureContext.Current.Add(Utils.ViewModelNameKey, manageDatabaseSourceControlViewModel);
//            FeatureContext.Current.Add("updateManager", mockStudioUpdateManager);
//            FeatureContext.Current.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);
//        }
//
//        [BeforeScenario("CreatingNewDBSource")]
//        public void SetupForDatabaseSource()
//        {
//            ScenarioContext.Current.Add(Utils.ViewNameKey, FeatureContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey));
//            ScenarioContext.Current.Add("updateManager", FeatureContext.Current.Get<Mock<IManageDatabaseSourceModel>>("updateManager"));
//            ScenarioContext.Current.Add("requestServiceNameViewModel", FeatureContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
//            ScenarioContext.Current.Add(Utils.ViewModelNameKey, FeatureContext.Current.Get<ManageDatabaseSourceViewModel>(Utils.ViewModelNameKey));
//        }
//
//        [Given(@"I open New Database Source")]
//        public void GivenIOpenNewDatabaseSource()
//        {
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            Assert.IsNotNull(manageDatabaseSourceControl);
//            Assert.IsNotNull(manageDatabaseSourceControl.DataContext); 
//        }
//
//        [Given(@"I type Server as ""(.*)""")]
//        public void GivenITypeServerAs(string serverName)
//        {
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.EnterServerName(serverName);
//            var viewModel = ScenarioContext.Current.Get<ManageDatabaseSourceViewModel>("viewModel");
//            Assert.AreEqual(serverName,viewModel.ServerName.Name);
//        }
//
//        [Given(@"Database dropdown is ""(.*)""")]
//        [When(@"Database dropdown is ""(.*)""")]
//        [Then(@"Database dropdown is ""(.*)""")]
//        public void GivenDropdownIs(string visibility)
//        {
//            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;
//
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            var databaseDropDownVisibility = manageDatabaseSourceControl.GetDatabaseDropDownVisibility();
//            Assert.AreEqual(expectedVisibility,databaseDropDownVisibility);
//        }
//
//        [Given(@"""(.*)"" is ""(.*)""")]
//        [When(@"""(.*)"" is ""(.*)""")]
//        [Then(@"""(.*)"" is ""(.*)""")]
//        public void GivenIs(string controlName, string enabledString)
//        {
//            Utils.CheckControlEnabled(controlName, enabledString, ScenarioContext.Current.Get<ICheckControlEnabledView>(Utils.ViewNameKey));
//        }
//
//        [Given(@"I Select Authentication Type as ""(.*)""")]
//        [When(@"I Select Authentication Type as ""(.*)""")]
//        [Then(@"I Select Authentication Type as ""(.*)""")]
//        public void GivenISelectAuthenticationTypeAs(string authenticationTypeString)
//        {
//            var authenticationType = String.Equals(authenticationTypeString, "Windows",
//                StringComparison.InvariantCultureIgnoreCase)
//                ? AuthenticationType.Windows
//                : AuthenticationType.User;
//
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.SetAuthenticationType(authenticationType);
//        }
//
//
//        [Given(@"I select ""(.*)"" as Database")]
//        [When(@"I select ""(.*)"" as Database")]
//        [Then(@"I select ""(.*)"" as Database")]
//        public void WhenISelectAsDatabase(string databaseName)
//        {
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.SelectDatabase(databaseName);
//            var viewModel = ScenarioContext.Current.Get<ManageDatabaseSourceViewModel>("viewModel");
//            Assert.AreEqual(databaseName,viewModel.DatabaseName);
//        }
//
//        [When(@"I save the source")]
//        public void WhenISaveTheSource()
//        {
//            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
//            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.PerformSave();
//
//        }
//
//        [Then(@"Username field is ""(.*)""")]
//        public void ThenUsernameFieldIs(string visibility)
//        {
//            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;
//
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            var databaseDropDownVisibility = manageDatabaseSourceControl.GetUsernameVisibility();
//            Assert.AreEqual(expectedVisibility, databaseDropDownVisibility);
//        }
//
//        [Then(@"Password field is ""(.*)""")]
//        public void ThenPasswordFieldIs(string visibility)
//        {
//            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;
//
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            var databaseDropDownVisibility = manageDatabaseSourceControl.GetPasswordVisibility();
//            Assert.AreEqual(expectedVisibility, databaseDropDownVisibility);
//        }
//
//        [Given(@"I type Username as ""(.*)""")]
//        [When(@"I type Username as ""(.*)""")]
//        [Then(@"I type Username as ""(.*)""")]
//        public void WhenITypeUsernameAs(string userName)
//        {
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.EnterUserName(userName);
//            var viewModel = ScenarioContext.Current.Get<ManageDatabaseSourceViewModel>("viewModel");
//            Assert.AreEqual(userName,viewModel.UserName);
//        }
//
//        [Given(@"I type Password as ""(.*)""")]
//        [When(@"I type Password as ""(.*)""")]
//        [Then(@"I type Password as ""(.*)""")]
//        public void WhenITypePasswordAs(string password)
//        {
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.EnterPassword(password);
//            var viewModel = ScenarioContext.Current.Get<ManageDatabaseSourceViewModel>("viewModel");
//            Assert.AreEqual(password,viewModel.Password);
//        }
//
//        [Then(@"Test Connecton is ""(.*)""")]
//        [When(@"Test Connecton is ""(.*)""")]
//        public void ThenTestConnectonIs(string successString)
//        {
//            var mockUpdateManager = ScenarioContext.Current.Get<Mock<IManageDatabaseSourceModel>>("updateManager");
//            var isSuccess = String.Equals(successString, "Successful", StringComparison.InvariantCultureIgnoreCase);
//            if (isSuccess)
//            {
//                mockUpdateManager.Setup(manager => manager.TestDbConnection(It.IsAny<IDbSource>()))
//                    .Returns(new List<string> {"Dev2TestingDB"});
//            }
//            else
//            {
//                mockUpdateManager.Setup(manager => manager.TestDbConnection(It.IsAny<IDbSource>()))
//                    .Throws(new WarewolfTestException("Server not found", null));
//
//            }
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.PerformTestConnection();
//            Thread.Sleep(1000);
//        }
//
//        [When(@"the validation message as ""(.*)""")]
//        public void WhenTheValidationMessageAs(string validationMessage)
//        {
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            var errorMessage = manageDatabaseSourceControl.GetErrorMessage();
//            Assert.AreEqual(validationMessage,errorMessage);
//        }
//
//        [Then(@"the save dialog is opened")]
//        public void ThenTheSaveDialogIsOpened()
//        {
//            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
//            mockRequestServiceNameViewModel.Verify();
//        }
//
//        [AfterScenario("CreatingNewDBSource")]
//        public void Cleanup()
//        {
//            var mockUpdateManager = ScenarioContext.Current.Get<Mock<IManageDatabaseSourceModel>>("updateManager");
//            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
//            var mockEventAggregator = new Mock<IEventAggregator>();
//            var viewModel = new ManageDatabaseSourceViewModel(mockUpdateManager.Object, mockRequestServiceNameViewModel.Object, mockEventAggregator.Object, new SynchronousAsyncWorker());
//            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
//            manageDatabaseSourceControl.DataContext = viewModel;
//            FeatureContext.Current.Remove("viewModel");
//            FeatureContext.Current.Add("viewModel", viewModel);
//        }
//    }
//}
