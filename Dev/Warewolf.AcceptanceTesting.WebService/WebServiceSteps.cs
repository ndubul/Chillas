using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.WebService;
using Dev2.Common.Interfaces.WebServices;
using Dev2.Communication;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;
using Warewolf.Studio.Core.Infragistics_Prism_Region_Adapter;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.WebService
{
    [Binding]
    public class WebServiceSteps
    {
        private static WebServiceSourceDefinition _testWebServiceSourceDefinition;
        private static WebServiceSourceDefinition _demoWebServiceSourceDefinition;

        [BeforeFeature("WebService")]
        public static void SetupForSystem()
        {
            Utils.SetupResourceDictionary();
            var view = new ManageWebserviceControl();
            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
            var mockWebServiceModel = new Mock<IWebServiceModel>();
            SetupModel(mockWebServiceModel);
            var viewModel = new ManageWebServiceViewModel(mockWebServiceModel.Object, mockRequestServiceNameViewModel.Object);
            view.DataContext = viewModel;
            Utils.ShowTheViewForTesting(view);
            FeatureContext.Current.Add(Utils.ViewNameKey, view);
            FeatureContext.Current.Add(Utils.ViewModelNameKey, viewModel);
            FeatureContext.Current.Add("model", mockWebServiceModel);
            FeatureContext.Current.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);
        }

        private static void SetupModel(Mock<IWebServiceModel> mockWebServiceModel)
        {
            _demoWebServiceSourceDefinition = new WebServiceSourceDefinition
            {
                Name = "Dev2CountriesWebService"
            };
            _testWebServiceSourceDefinition = new WebServiceSourceDefinition
            {
                Name = "Testing Web Service Connector"
            };
            mockWebServiceModel.Setup(model => model.RetrieveSources()).Returns(new List<IWebServiceSource>
            {
                _demoWebServiceSourceDefinition,
                _testWebServiceSourceDefinition
            });
            var webService = new Dev2.Runtime.ServiceModel.Data.WebService();
            var recordset = new Recordset
            {
                Name = "UnnamedArrayData"
            };
            var countryID = new RecordsetField
            {
                Name = "CountryID",
                Alias = "CountryID"
            };
            var recordSetRec = new RecordsetRecord
            {
                Name = "CountryID",
                Label = "CountryID"
            };
            recordset.Fields.Add(countryID);
            recordset.Records.Add(recordSetRec);
            webService.Recordsets.Add(recordset);
            var serializer = new Dev2JsonSerializer();
            var serializedWebService = serializer.Serialize(webService);
            mockWebServiceModel.Setup(serviceModel => serviceModel.TestService(It.IsAny<IWebService>())).Returns(serializedWebService);
        }

        [BeforeScenario("WebService")]
        public void SetupForWebService()
        {
            ScenarioContext.Current.Add("view", FeatureContext.Current.Get<ManageWebserviceControl>(Utils.ViewNameKey));
            ScenarioContext.Current.Add("viewModel", FeatureContext.Current.Get<ManageWebServiceViewModel>("viewModel"));
            ScenarioContext.Current.Add("requestServiceNameViewModel", FeatureContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
            ScenarioContext.Current.Add("model", FeatureContext.Current.Get<Mock<IWebServiceModel>>("model"));
        }

        [Given(@"I click ""(.*)""")]
        public void GivenIClick(string p0)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            Assert.IsNotNull(view);
            Assert.IsNotNull(view.DataContext);
            Assert.IsInstanceOfType(view.DataContext, typeof(ManageWebServiceViewModel));
        }

        [Then(@"""(.*)"" tab is opened")]
        public void ThenTabIsOpened(string headerText)
        {
            var viewModel = ScenarioContext.Current.Get<IDockAware>("viewModel");
            Assert.AreEqual(headerText, viewModel.Header);
        }

        [Then(@"""(.*)"" is ""(.*)""")]
        [When(@"""(.*)"" is ""(.*)""")]
        [Given(@"""(.*)"" is ""(.*)""")]
        public void ThenIs(string name, string state)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            switch (name)
            {
                case "1 Select Request Method and Source ":
                    Utils.CheckControlEnabled("name", state, view);
                    break;
                case "2 Request":
                    Utils.CheckControlEnabled("name", state, view);
                    break;
                case "3 Variables":
                    Utils.CheckControlEnabled("name", state, view);
                    break;
                case "4 Response":
                    Utils.CheckControlEnabled("name", state, view);
                    break;

            }
        }

        [Then(@"""(.*)"" tool is ""(.*)""")]
        public void ThenToolIs(string name, string state)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            switch (name)
            {
                case "Paste":
                    Utils.CheckControlEnabled("name", state, view);
                    break;

            }
        }

        [When(@"I select ""(.*)"" as data source")]
        [Given(@"I select ""(.*)"" as data source")]
        public void WhenISelectAsDataSource(string webServiceName)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            view.SelectWebService(_demoWebServiceSourceDefinition);
            var viewModel = Utils.GetViewModel<ManageDatabaseServiceViewModel>();
            Assert.AreEqual(webServiceName, viewModel.SelectedSource.Name);
        }

        [When(@"I select Method ""(.*)""")]
        public void WhenISelectMethod(string method)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            WebRequestMethod requestMethod;
            Enum.TryParse(method, out requestMethod);
            view.SelectMethod(requestMethod);
            var viewModel = Utils.GetViewModel<ManageWebServiceViewModel>();
            Assert.AreEqual(method, viewModel.SelectedWebRequestMethod.ToString());
        }

        [Then(@"method is selected as ""(.*)""")]
        public void ThenMethodIsSelectedAs(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"input mappings are")]
        [When(@"input mappings are")]
        [Then(@"input mappings are")]
        public void ThenInputMappingsAre(Table table)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            var inputMappings = view.GetHeaders();
            var i = 0;
            if (inputMappings.SourceCollection == null)
            {
                Assert.AreEqual(0, table.RowCount);
            }
            else
            {
                foreach (var input in inputMappings.SourceCollection)
                {
                    var inputMapping = input as IServiceInput;
                    if (inputMapping != null)
                    {
                        Assert.AreEqual(inputMapping.Name, table.Rows.ToList()[i][0]);
                    }
                    i++;
                }
            }
        }

        [Given(@"output mappings are")]
        [When(@"output mappings are")]
        [Then(@"output mappings are")]
        public void ThenOutputMappingsAre(Table table)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            var outputMappings = view.GetOutputMappings();
            if (outputMappings.SourceCollection == null)
            {
                Assert.AreEqual(0, table.RowCount);
            }
            else
            {
                foreach (var output in outputMappings.SourceCollection)
                {
                    var outputMapping = output as IServiceOutputMapping;
                    if (outputMapping != null)
                    {
                    }
                }
            }
        }

        [Then(@"Save Dialog is opened")]
        public void ThenSaveDialogIsOpened()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify();
        }

        [When(@"I save")]
        public void WhenISave()
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            view.Save();
        }

        [When(@"Test Connection is ""(.*)""")]
        public void WhenTestConnectionIs(string p0)
        {
            var webServiceModel = ScenarioContext.Current.Get<Mock<IWebServiceModel>>("model");
            SetupModel(webServiceModel);
            var view = Utils.GetView<ManageWebserviceControl>();
            view.TestAction();
            var viewModel = Utils.GetViewModel<ManageWebServiceViewModel>();
            Assert.IsFalse(viewModel.IsTesting);
            Assert.AreEqual(String.Empty, viewModel.ErrorMessage);
        }


        [When(@"I select ""(.*)"" as Method")]
        public void WhenISelectAsMethod(string webServiceName)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            var selectedDataSource = view.GetSelectedWebService();
            Assert.AreEqual(webServiceName, selectedDataSource.Name);
        }

        [Then(@"Select Request Method & Source is focused")]
        public void ThenSelectRequestMethodSourceIsFocused()
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            var isDataSourceFocused = view.IsDataSourceFocused();
            Assert.IsTrue(isDataSourceFocused);
        }

        [Then(@"I save as ""(.*)""")]
        public void ThenISaveAs(string resourceName)
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ResourceName).Returns(new ResourceName("", resourceName));
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Returns(MessageBoxResult.OK);
            var manageWebserviceSourceControl = ScenarioContext.Current.Get<ManageWebserviceControl>(Utils.ViewNameKey);
            manageWebserviceSourceControl.Save();
        }

        [Then(@"title is ""(.*)""")]
        public void ThenTitleIs(string title)
        {
            var manageWebserviceControl = ScenarioContext.Current.Get<ManageWebserviceControl>(Utils.ViewNameKey);
            var viewModel = ScenarioContext.Current.Get<ManageWebserviceSourceViewModel>("viewModel");
            Assert.AreEqual(title, viewModel.HeaderText);
        }

        [Then(@"the response is loaded")]
        public void ThenTheResponseIsLoaded()
        {
            var model = ScenarioContext.Current.Get<Mock<IWebServiceModel>>("model");
            
        }

        [Then(@"request URL has ""(.*)""")]
        public void ThenRequestUrlHas(string url)
        {
            var manageWebserviceControl = ScenarioContext.Current.Get<ManageWebserviceControl>(Utils.ViewNameKey);
            Assert.AreEqual(url, manageWebserviceControl.GetUrl());
        }

        [Then(@"""(.*)"" selected as data source")]
        public void ThenSelectedAsDataSource(string selectedDataSourceName)
        {
            var view = Utils.GetView<ManageWebserviceControl>();
            var selectedDataSource = view.GetSelectedWebService();
            Assert.AreEqual(selectedDataSourceName, selectedDataSource.Name);
        }




    }
}
