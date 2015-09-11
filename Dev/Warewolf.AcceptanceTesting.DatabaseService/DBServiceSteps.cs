﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;
using Warewolf.Core;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.DatabaseService
{
    [Binding]
    // ReSharper disable once InconsistentNaming
    public class DBServiceSteps
    {
        private static DbSourceDefinition _demoDbSourceDefinition;
        private static DbSourceDefinition _testDbSourceDefinition;
        private static DbAction _dbAction;
        private static DbAction _dbInsertDummyAction;

        [BeforeFeature("DBService")]
        public static void SetupForSystem()
        {
            Utils.SetupResourceDictionary();
            var view = new ManageDatabaseServiceControl();
            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
            var mockDbServiceModel = new Mock<IDbServiceModel>();
            SetupModel(mockDbServiceModel);
            var viewModel = new ManageDatabaseServiceViewModel(mockDbServiceModel.Object, mockRequestServiceNameViewModel.Object);
            view.DataContext = viewModel;

           

            FeatureContext.Current.Add(Utils.ViewNameKey, view);
            FeatureContext.Current.Add("viewModel", viewModel);
            FeatureContext.Current.Add("model",mockDbServiceModel);
            FeatureContext.Current.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);
           
        }

        private static void SetupModel(Mock<IDbServiceModel> mockDbServiceModel)
        {
            _demoDbSourceDefinition = new DbSourceDefinition
            {
                Name = "DemoDB",
                ServerName = "gendev"
            };
            _testDbSourceDefinition = new DbSourceDefinition
            {
                Name = "Testing DB",
                ServerName = "tfsbld"
            };
            mockDbServiceModel.Setup(model => model.RetrieveSources()).Returns(new List<IDbSource>
            {
                _demoDbSourceDefinition,
                _testDbSourceDefinition
            });
            _dbAction = new DbAction
            {
                Name = "dbo.ConverToint",
                Inputs = new List<IServiceInput> { new ServiceInput("charValue", "1") }
            };
            var dbInputs = new List<IServiceInput>
            {
                new ServiceInput("fname","Change"),
                new ServiceInput("lname","Test"),
                new ServiceInput("username","wolf"),
                new ServiceInput("password","Dev"),
                new ServiceInput("lastAccessDate","10/1/1990"),
            };
            _dbInsertDummyAction = new DbAction
            {
                Name = "dbo.InsertDummyUser",
                Inputs = dbInputs
            };
            mockDbServiceModel.Setup(model => model.GetActions(It.IsAny<IDbSource>())).Returns(new List<IDbAction>
            {
                _dbAction,
                _dbInsertDummyAction
            });
            var dataTable = new DataTable("dbo_ConverToInt");
            dataTable.Columns.Add("Result",typeof(int));
            dataTable.LoadDataRow(new object[] {"1"}, true);
            mockDbServiceModel.Setup(model => model.TestService(It.IsAny<IDatabaseService>()))
                .Returns(dataTable);
           
        }

        [BeforeScenario("DBService")]
        public void SetupForDatabaseService()
        {
            ScenarioContext.Current.Add("view", FeatureContext.Current.Get<ManageDatabaseServiceControl>(Utils.ViewNameKey));
            ScenarioContext.Current.Add("viewModel", FeatureContext.Current.Get<ManageDatabaseServiceViewModel>("viewModel"));
            ScenarioContext.Current.Add("requestServiceNameViewModel", FeatureContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
            ScenarioContext.Current.Add("model", FeatureContext.Current.Get<Mock<IDbServiceModel>>("model"));            
        }

        [Given(@"I click New Data Base Service Connector")]
        public void GivenIClickNewDataBaseServiceConnector()
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            Assert.IsNotNull(view);
            Assert.IsNotNull(view.DataContext);
            Assert.IsInstanceOfType(view.DataContext,typeof(ManageDatabaseServiceViewModel));
        }
        [Given(@"I click ""(.*)""")]
        public void GivenIClick(string p0)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            Assert.IsNotNull(view);
            Assert.IsNotNull(view.DataContext);
            Assert.IsInstanceOfType(view.DataContext, typeof(ManageDatabaseServiceViewModel));
        }


        [When(@"I select ""(.*)"" as data source")]
        public void WhenISelectAsDataSource(string dbSourceName)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            view.SelectDbSource(_demoDbSourceDefinition);
            var viewModel = Utils.GetViewModel<ManageDatabaseServiceViewModel>();
            Assert.AreEqual(dbSourceName,viewModel.SelectedSource.Name);
        }

        [Given(@"I select ""(.*)"" as the action")]
        [When(@"I select ""(.*)"" as the action")]
        [Then(@"I select ""(.*)"" as the action")]
        public void WhenISelectAsTheAction(string actionName)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            view.SelectDbAction(_dbAction);
            var viewModel = Utils.GetViewModel<ManageDatabaseServiceViewModel>();
            Assert.AreEqual(actionName,viewModel.SelectedAction.Name);
        }

        

        [When(@"I test the action")]
        public void WhenITestTheAction()
        {
            var dbServiceModel = ScenarioContext.Current.Get<Mock<IDbServiceModel>>("model");
            SetupModel(dbServiceModel);
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            view.TestAction();
            var viewModel = Utils.GetViewModel<ManageDatabaseServiceViewModel>();
            Assert.IsNotNull(viewModel.TestResults);
            
        }

        [When(@"I save")]
        public void WhenISave()
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            view.Save();
           
        }

        [Given(@"""(.*)"" tab is opened")]
        [When(@"""(.*)"" tab is opened")]
        [Then(@"""(.*)"" tab is opened")]
        public void ThenTabIsOpened(string tabName)
        {
            var viewModel = Utils.GetViewModel<ManageDatabaseServiceViewModel>();
            Assert.AreEqual(tabName,viewModel.Header);
        }

        [Given(@"Data Source is focused")]
        [When(@"Data Source is focused")]
        [Then(@"Data Source is focused")]
        public void ThenDataSourceIsFocused()
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            var isDataSourceFocused = view.IsDataSourceFocused();
            Assert.IsTrue(isDataSourceFocused);
        }
        [Then(@"""(.*)"" is ""(.*)""")]
        public void ThenIs(string name, string state)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            switch(name)
            {
                case "1 Data Source" :
                    Utils.CheckControlEnabled("name",state,view);
                    break;
                case "2 Select Action":
                    Utils.CheckControlEnabled("name", state, view);
                    break;
                case "3 Test Connector and Calculate Outputs":
                    Utils.CheckControlEnabled("name", state, view);
                    break;
                case "4 Edit Default and Mapping Names":
                    Utils.CheckControlEnabled("name", state, view);
                    break;

            }
        }

        [Given(@"inputs are")]
        [When(@"inputs are")]
        [Then(@"inputs are")]
        public void ThenInputsAre(Table table)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            var inputs = view.GetInputs();
            foreach (var input in inputs.SourceCollection)
            {
                var dbInput = input as IServiceInput;
                if (dbInput != null)
                {
                    Assert.AreEqual(dbInput.Value,table.Rows[0][dbInput.Name]);
                }
            }
        }

        [Given(@"outputs are")]
        [When(@"outputs are")]
        [Then(@"outputs are")]
        public void ThenOutputsAre(Table table)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            var outputs = view.GetOutputs();
            foreach (var output in outputs.SourceCollection)
            {
                var rowOutput = output as DataRowView;
                if (rowOutput != null)
                {
                    Assert.AreEqual(rowOutput[0].ToString(),table.Rows[0][1]);
                }
            }
        }

        [Given(@"input mappings are")]
        [When(@"input mappings are")]
        [Then(@"input mappings are")]
        public void ThenInputMappingsAre(Table table)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            var inputMappings = view.GetInputs();
            var i = 0;
            if (inputMappings.SourceCollection == null)
            {
                Assert.AreEqual(0,table.RowCount);
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
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            var outputMappings = view.GetOutputs();
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

        [Given(@"I open ""(.*)"" service")]
        public void GivenIOpenService(string serviceName)
        {
            var databaseService = new Warewolf.Core.DatabaseService { Name = serviceName, Source = _demoDbSourceDefinition, Action = _dbInsertDummyAction,Inputs = _dbInsertDummyAction.Inputs};
            var dbOutputMapping = new ServiceOutputMapping("UserID", "UserID") { RecordSetName = "dbo_InsertDummyUser" };
            databaseService.OutputMappings = new List<IServiceOutputMapping> { dbOutputMapping };
            ScenarioContext.Current.Remove("viewModel");
            ScenarioContext.Current.Remove("requestServiceNameViewModel");
            var requestServiceNameViewModelMock = new Mock<IRequestServiceNameViewModel>();
            ScenarioContext.Current.Add("requestServiceNameViewModel", requestServiceNameViewModelMock);
            var viewModel = new ManageDatabaseServiceViewModel(ScenarioContext.Current.Get<Mock<IDbServiceModel>>("model").Object, requestServiceNameViewModelMock.Object, databaseService);
            ScenarioContext.Current.Add("viewModel",viewModel);
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            try
            {
                view.DataContext = null;
                view.DataContext = viewModel;
            }
            catch
            {
                //Do nothing
            }
            
        }

        [Given(@"""(.*)"" is selected as the data source")]
        public void GivenIsSelectedAsTheDataSource(string selectedDataSourceName)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            var selectedDataSource = view.GetSelectedDataSource();
            Assert.AreEqual(selectedDataSourceName,selectedDataSource.Name);
        }

        [Given(@"Inspect Data Connector hyper link is ""(.*)""")]
        [When(@"Inspect Data Connector hyper link is ""(.*)""")]
        [Then(@"Inspect Data Connector hyper link is ""(.*)""")]
        public void ThenInspectDataConnectorHyperLinkIs(string p0)
        {
        }

        [Then(@"""(.*)"" is saved")]
        public void ThenIsSaved(string p0)
        {
            
        }

        [Then(@"Save Dialog is not opened")]
        public void ThenSaveDialogIsNotOpened()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify(model => model.ShowSaveDialog(),Times.Never());
        }

        [When(@"testing the action fails")]
        public void WhenExecutionFails()
        {
            var dbServiceModel = ScenarioContext.Current.Get<Mock<IDbServiceModel>>("model");
            dbServiceModel.Setup(model => model.TestService(It.IsAny<IDatabaseService>()))
                .Throws(new Exception("Test Failed"));
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            view.TestAction();
            var viewModel = Utils.GetViewModel<ManageDatabaseServiceViewModel>();
            Assert.IsNull(viewModel.TestResults);
        }

        [Given(@"""(.*)"" is selected as the action")]
        public void GivenIsSelectedAsTheAction(string selectedActionName)
        {
            var view = Utils.GetView<ManageDatabaseServiceControl>();
            var selectedAction = view.GetSelectedAction();
            Assert.AreEqual(selectedActionName,selectedAction.Name);

        }

        [Then(@"Save Dialog is opened")]
        public void ThenSaveDialogIsOpened()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify();
        }

    }
}
