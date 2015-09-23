using System;
using System.Collections.Generic;
using System.Windows;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Threading;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;
using Warewolf.Studio.ServerProxyLayer;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.ServerSource
{
    [Binding]
    public class NewServerSourceSteps
    {
        [BeforeFeature("ServerSource")]
        public static void SetupForSystem()
        {
            Utils.SetupResourceDictionary();
            var manageServerControl = new ManageServerControl();
            var mockStudioUpdateManager = new Mock<IManageServerSourceModel>();
            mockStudioUpdateManager.Setup(model => model.ServerName).Returns("localhost");
            mockStudioUpdateManager.Setup(model => model.GetComputerNames()).Returns(new List<string> { "rsaklfhuggspc", "barney", "SANDBOX-1" });
            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockExecutor = new Mock<IExternalProcessExecutor>();

            var manageServerSourceViewModel = new ManageNewServerViewModel(mockStudioUpdateManager.Object, mockRequestServiceNameViewModel.Object, mockEventAggregator.Object, new SynchronousAsyncWorker(), mockExecutor.Object);
            manageServerControl.DataContext = manageServerSourceViewModel;
            Utils.ShowTheViewForTesting(manageServerControl);
            FeatureContext.Current.Add(Utils.ViewNameKey, manageServerControl);
            FeatureContext.Current.Add(Utils.ViewModelNameKey, manageServerSourceViewModel);
            FeatureContext.Current.Add("updateManager", mockStudioUpdateManager);
            FeatureContext.Current.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);
            FeatureContext.Current.Add("externalProcessExecutor", mockExecutor);
        }

        [BeforeScenario("ServerSource")]
        public void SetupForServerSource()
        {
            ScenarioContext.Current.Add(Utils.ViewNameKey, FeatureContext.Current.Get<ManageServerControl>(Utils.ViewNameKey));
            ScenarioContext.Current.Add("updateManager", FeatureContext.Current.Get<Mock<IManageServerSourceModel>>("updateManager"));
            ScenarioContext.Current.Add("requestServiceNameViewModel", FeatureContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
            ScenarioContext.Current.Add("externalProcessExecutor", FeatureContext.Current.Get<Mock<IExternalProcessExecutor>>("externalProcessExecutor"));
            ScenarioContext.Current.Add(Utils.ViewModelNameKey, FeatureContext.Current.Get<ManageNewServerViewModel>(Utils.ViewModelNameKey));
        }


        [Given(@"I open New Server Source")]
        public void GivenIOpenNewServerSource()
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            Assert.IsNotNull(manageServerControl);
            Assert.IsNotNull(manageServerControl.DataContext);
        }

        [Then(@"""(.*)"" tab is opened")]
        public void ThenTabIsOpened(string headerText)
        {
            var viewModel = Utils.GetViewModel<ManageNewServerViewModel>();
            Assert.AreEqual(headerText, viewModel.Header);
        }

        [Then(@"selected protocol is ""(.*)""")]
        public void ThenSelectedProtocolIs(string protocol)
        {
            var view = Utils.GetView<ManageServerControl>();
            view.SetProtocol(protocol);
            var viewModel = Utils.GetViewModel<ManageNewServerViewModel>();
            Assert.AreEqual(protocol, viewModel.Protocol);
        }

        [Then(@"server port is ""(.*)""")]
        public void ThenServerPortIs(int port)
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(port.ToString(), viewModel.SelectedPort);
            Assert.AreEqual(port.ToString(), manageServerControl.GetPort());
        }

        [Then(@"Authentication Type selected is ""(.*)""")]
        public void ThenAuthenticationTypeSelectedIs(string authenticationTypeString)
        {
            var authenticationType = String.Equals(authenticationTypeString, "User",
                StringComparison.OrdinalIgnoreCase)
                ? AuthenticationType.User
                : AuthenticationType.Windows;

            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageServerControl.SetAuthenticationType(authenticationType);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(authenticationType, viewModel.AuthenticationType);
        }


        [Given(@"""(.*)"" is ""(.*)""")]
        [When(@"""(.*)"" is ""(.*)""")]
        [Then(@"""(.*)"" is ""(.*)""")]
        public void ThenIs(string controlName, string enabledString)
        {
            Utils.CheckControlEnabled(controlName, enabledString, ScenarioContext.Current.Get<ICheckControlEnabledView>(Utils.ViewNameKey));
        }

        [Given(@"I type Server as ""(.*)""")]
        public void GivenITypeServerAs(string serverName)
        {
            if (serverName == "Incorrect")
            {

            }
            else
            {
                var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
                manageDatabaseSourceControl.EnterServerName(serverName);
                var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
                Assert.AreEqual(serverName, viewModel.ServerName.Name);
            }
        }


        [Given(@"I select protocol as ""(.*)""")]
        public void GivenISelectProtocolAs(string protocol)
        {
            var view = Utils.GetView<ManageServerControl>();
            view.SetProtocol(protocol);
            var viewModel = Utils.GetViewModel<ManageNewServerViewModel>();
            Assert.AreEqual(protocol, viewModel.Protocol);
        }

        [Given(@"I enter server port as ""(.*)""")]
        public void GivenIEnterServerPortAs(int port)
        {
            var view = Utils.GetView<ManageServerControl>();
            view.SetPort(port.ToString());
            var viewModel = Utils.GetViewModel<ManageNewServerViewModel>();
            Assert.AreEqual(port.ToString(), viewModel.SelectedPort);
        }

        [Given(@"Authentication Type as ""(.*)""")]
        public void GivenAuthenticationTypeAs(string authenticationTypeString)
        {
            AuthenticationType authenticationType;
            switch (authenticationTypeString)
            {
                case "User":
                    authenticationType = String.Equals(authenticationTypeString, "User", StringComparison.OrdinalIgnoreCase)
                        ? AuthenticationType.User : AuthenticationType.Windows;
                    break;
                case "Windows":
                    authenticationType = String.Equals(authenticationTypeString, "Windows", StringComparison.OrdinalIgnoreCase)
                        ? AuthenticationType.Windows : AuthenticationType.Public;
                    break;
                case "Public":
                    authenticationType = String.Equals(authenticationTypeString, "Public", StringComparison.OrdinalIgnoreCase)
                        ? AuthenticationType.Public : AuthenticationType.Windows;
                    break;
                default:
                    authenticationType = String.Equals(authenticationTypeString, "Windows", StringComparison.OrdinalIgnoreCase)
                        ? AuthenticationType.Windows : AuthenticationType.Public;
                    break;
            }

            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageServerControl.SetAuthenticationType(authenticationType);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(authenticationType, viewModel.AuthenticationType);
        }

        [Given(@"I open ""(.*)"" server source")]
        public void GivenIOpenServerSource(string p0)
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            var mockStudioUpdateManager = new Mock<IManageServerSourceModel>();
            mockStudioUpdateManager.Setup(model => model.ServerName).Returns("localhost");
            mockStudioUpdateManager.Setup(model => model.GetComputerNames()).Returns(new List<string> { "rsaklfhuggspc", "barney", "SANDBOX-1" });
            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockExecutor = new Mock<IExternalProcessExecutor>();

            var serverSourceDefinition = new Dev2.Common.Interfaces.Core.ServerSource
            {
                Name = "ServerSource",
                Address = "https://SANDBOX-1:3143",
                ServerName = "SANDBOX-1",
                AuthenticationType = AuthenticationType.User,
                UserName = "Integrationtester",
                Password = "I73573r0"
            };
            FeatureContext.Current["svrsrc"] = serverSourceDefinition;
            var manageServerSourceViewModel = new ManageNewServerViewModel(mockStudioUpdateManager.Object, mockEventAggregator.Object, serverSourceDefinition, new SynchronousAsyncWorker(), mockExecutor.Object);
            try
            {
                manageServerControl.DataContext = manageServerSourceViewModel;
            }
            catch(Exception)
            {
                //ignore stupid infragistics control
            }
            ScenarioContext.Current.Remove("viewModel");
            ScenarioContext.Current.Add("viewModel", manageServerSourceViewModel);
        }

        [Then(@"Server as ""(.*)""")]
        public void ThenServerAs(string server)
        {
            var svr = FeatureContext.Current.Get<IServerSource>("svrsrc");
            svr.ServerName = server;
            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectServer("server");
        }


        [When(@"I Test Connection to remote server")]
        public void WhenITestConnectionToRemoteServer()
        {
            var view = Utils.GetView<ManageServerControl>();
            view.TestAction();
        }

        [When(@"I enter Username as ""(.*)""")]
        public void WhenIEnterUsernameAs(string username)
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageServerControl.EnterUserName(username);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(username, manageServerControl.GetUsername());
        }

        [When(@"I enter Password as ""(.*)""")]
        public void WhenIEnterPasswordAs(string password)
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageServerControl.EnterPassword(password);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(password, viewModel.Password);
        }

        [Then(@"validation message is ""(.*)""")]
        public void ThenValidationMessageIs(string errorMsg)
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            var errorMessageFromControl = manageServerControl.GetErrorMessage();
            var errorMessageOnViewModel = viewModel.TestMessage;
            var isErrorMessageOnControl = errorMessageFromControl.Equals(errorMsg, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(isErrorMessageOnControl);
            if (string.IsNullOrWhiteSpace(errorMsg))
            {
                Assert.AreEqual(errorMsg, "");
            }
            else
            {
                var isErrorMessage = errorMessageOnViewModel.Equals(errorMsg, StringComparison.OrdinalIgnoreCase);
                Assert.IsTrue(isErrorMessage);
            }
        }

        [Then(@"the save dialog is opened")]
        [When(@"the save dialog is opened")]
        public void ThenTheSaveDialogIsOpened()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify();
        }

        [Then(@"server Username field is ""(.*)""")]
        public void ThenServerUsernameFieldIs(string visibility)
        {
            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;

            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            var userNameVisibility = manageServerControl.GetUsernameVisibility();
            Assert.AreEqual(expectedVisibility, userNameVisibility);
        }

        [Then(@"server Password field is ""(.*)""")]
        public void ThenServerPasswordFieldIs(string visibility)
        {
            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;

            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            var passwordVisibility = manageServerControl.GetPasswordVisibility();
            Assert.AreEqual(expectedVisibility, passwordVisibility);
        }

        [When(@"I save the server source")]
        [Then(@"I save the server source")]
        public void WhenISaveTheServerSource()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageServerControl.PerformSave();
        }

        [When(@"Test Connecton is ""(.*)""")]
        [Then(@"Test Connecton is ""(.*)""")]
        public void WhenTestConnectonIs(string successString)
        {
            var mockUpdateManager = ScenarioContext.Current.Get<Mock<IManageServerSourceModel>>("updateManager");
            var isSuccess = String.Equals(successString, "Passed", StringComparison.InvariantCultureIgnoreCase);
            var isLongRunning = String.Equals(successString, "Long Running", StringComparison.InvariantCultureIgnoreCase);
            if (isSuccess)
            {
                mockUpdateManager.Setup(manager => manager.TestConnection(It.IsAny<IServerSource>()));
            }
            else if (isLongRunning)
            {
                var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
                mockUpdateManager.Setup(manager => manager.TestConnection(It.IsAny<IServerSource>()));
                viewModel.AsyncWorker = new AsyncWorker();
            }
            else
            {
                mockUpdateManager.Setup(manager => manager.TestConnection(It.IsAny<IServerSource>()))
                    .Throws(new WarewolfTestException("Connection Error: Unauthorized", null));

            }
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageServerControl.TestAction();
        }


        [Then(@"server Username is ""(.*)""")]
        public void ThenServerUsernameIs(string username)
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(username, viewModel.UserName);
            Assert.AreEqual(username, manageServerControl.GetUsername());
        }

        [Then(@"server Password is is ""(.*)""")]
        public void ThenServerPasswordIsIs(string password)
        {
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(password, viewModel.Password);
            Assert.AreEqual(password, manageServerControl.GetPassword());
        }

        [Then(@"Authentication Type as ""(.*)""")]
        public void ThenAuthenticationTypeAs(string authenticationTypeString)
        {
            var authenticationType = String.Equals(authenticationTypeString, "Public",
                StringComparison.OrdinalIgnoreCase)
                ? AuthenticationType.Public
                : AuthenticationType.Windows;

            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            manageServerControl.SetAuthenticationType(authenticationType);
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(authenticationType, viewModel.AuthenticationType);
        }

        [Then(@"tab name is ""(.*)""")]
        public void ThenTabNameIs(string headerText)
        {
            var viewModel = ScenarioContext.Current.Get<ManageNewServerViewModel>("viewModel");
            Assert.AreEqual(headerText, viewModel.Header); 
        }

        [AfterScenario("ServerSource")]
        public void Cleanup()
        {
            var mockExecutor = new Mock<IExternalProcessExecutor>();
            var mockUpdateManager = ScenarioContext.Current.Get<Mock<IManageServerSourceModel>>("updateManager");
            mockUpdateManager.Setup(model => model.ServerName).Returns("localhost");
            mockUpdateManager.Setup(model => model.GetComputerNames()).Returns(new List<string> { "rsaklfhuggspc", "barney", "SANDBOX-1" });
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            var mockEventAggregator = new Mock<IEventAggregator>();
            var viewModel = new ManageNewServerViewModel(mockUpdateManager.Object, mockRequestServiceNameViewModel.Object, mockEventAggregator.Object, new SynchronousAsyncWorker(), mockExecutor.Object);
            var manageServerControl = ScenarioContext.Current.Get<ManageServerControl>(Utils.ViewNameKey);
            try
            {
                manageServerControl.DataContext = viewModel;
            }
            catch(Exception)
            {
                //ignore stupid infragistics control
            }
            FeatureContext.Current.Remove("viewModel");
            FeatureContext.Current.Add("viewModel", viewModel);
            FeatureContext.Current.Remove("externalProcessExecutor");
            FeatureContext.Current.Add("externalProcessExecutor", mockExecutor);
            ScenarioContext.Current.Remove("viewModel");
            ScenarioContext.Current.Add("viewModel", viewModel);

        }
    }
}
