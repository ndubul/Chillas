using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Communication;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;
using Warewolf.Core;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.PluginService
{
    [Binding]
    public class PluginServiceSteps
    {
        private static PluginSourceDefinition _testPluginSourceDefinition;
        private static PluginSourceDefinition _demoPluginSourceDefinition;
        static PluginAction _pluginAction;
        static PluginAction _dbInsertDummyAction;

        [BeforeFeature("PluginService")]
        public static void SetupForSystem()
        {
            Utils.SetupResourceDictionary();
            var view = new ManagePluginServiceControl();
            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
            var mockPluginServiceModel = new Mock<IPluginServiceModel>();
            SetupModel(mockPluginServiceModel);

            var viewModel = new ManagePluginServiceViewModel(mockPluginServiceModel.Object, mockRequestServiceNameViewModel.Object);
            view.DataContext = viewModel;
            Utils.ShowTheViewForTesting(view);
            FeatureContext.Current.Add(Utils.ViewNameKey, view);
            FeatureContext.Current.Add("viewModel", viewModel);
            FeatureContext.Current.Add("model", mockPluginServiceModel);
            FeatureContext.Current.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);

        }

        private static void SetupModel(Mock<IPluginServiceModel> mockPluginServiceModel)
        {
            _demoPluginSourceDefinition = new PluginSourceDefinition
            {
                Name = "testingPluginSrc"
            };
            _testPluginSourceDefinition = new PluginSourceDefinition
            {
                Name = "IntegrationTestPluginNull"
            };
            mockPluginServiceModel.Setup(model => model.RetrieveSources()).Returns(new List<IPluginSource>
            {
                _demoPluginSourceDefinition,
                _testPluginSourceDefinition
            });
            mockPluginServiceModel.Setup(model => model.GetNameSpaces(It.IsAny<IPluginSource>())).Returns(new List<INamespaceItem>
            {
                new NamespaceItem(){FullName = "Unlimited Framework Plugins EmailPlugin"},
                new NamespaceItem(){FullName = "Dev2.PrimitiveTestDLL.TestClass"}
            });
            NameValue nameValue = new NameValue
            {
                Name = "data (System.Object)",
                Value = ""
            };
            _pluginAction = new PluginAction
            {
                FullName = "FetchStringvalue",
                Variables = new List<INameValue>
                {
                    nameValue
                },
                Inputs = new List<IServiceInput> { new ServiceInput("data", "") }
            };
            var pluginInputs = new List<IServiceInput>
            {
                new ServiceInput("data","")
            };
            _dbInsertDummyAction = new PluginAction
            {
                FullName = "FetchStringvalue",
                Inputs = pluginInputs
            };
            mockPluginServiceModel.Setup(model => model.GetActions(It.IsAny<IPluginSource>(), It.IsAny<INamespaceItem>())).Returns(new List<IPluginAction>
            {
                _pluginAction,
                _dbInsertDummyAction
            });
            var recordset = new Recordset
            {
                Name = ""
            };
            var countryID = new RecordsetField
            {
                Name = "Name",
                Alias = "Name"
            };
            var recordSetRec = new RecordsetRecord
            {
                Name = "Name",
                Label = "Name"
            };


            recordset.Fields.Add(countryID);
            recordset.Records.Add(recordSetRec);
            var pluginService = new PluginServiceDefinition
            {
                Source = _demoPluginSourceDefinition,
                Name = "IntegrationTestPluginNull",
                Inputs = new List<IServiceInput>(),
                OutputMappings = new List<IServiceOutputMapping>(),
                Action = _pluginAction
            };

            var serializer = new Dev2JsonSerializer();
            var serializedPluginService = serializer.Serialize(pluginService);

            mockPluginServiceModel.Setup(model => model.TestService(It.IsAny<IPluginService>()))
                .Returns(serializedPluginService);
        }

        [BeforeScenario("PluginService")]
        public void SetupForDatabaseService()
        {
            ScenarioContext.Current.Add("view", FeatureContext.Current.Get<ManagePluginServiceControl>(Utils.ViewNameKey));
            ScenarioContext.Current.Add("viewModel", FeatureContext.Current.Get<ManagePluginServiceViewModel>("viewModel"));
            ScenarioContext.Current.Add("requestServiceNameViewModel", FeatureContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
            ScenarioContext.Current.Add("model", FeatureContext.Current.Get<Mock<IPluginServiceModel>>("model"));

        }

        [Given(@"I open ""(.*)""")]
        public void GivenIOpen(string serviceName)
        {
            var pluginService = new PluginServiceDefinition { Name = serviceName, Source = _demoPluginSourceDefinition, Action = _pluginAction, Inputs = new List<IServiceInput>() };
            var dbOutputMapping = new ServiceOutputMapping("Name", "Name") { RecordSetName = "" };
            pluginService.OutputMappings = new List<IServiceOutputMapping> { dbOutputMapping };
            ScenarioContext.Current.Remove("viewModel");
            ScenarioContext.Current.Remove("requestServiceNameViewModel");
            var requestServiceNameViewModelMock = new Mock<IRequestServiceNameViewModel>();
            ScenarioContext.Current.Add("requestServiceNameViewModel", requestServiceNameViewModelMock);
            var viewModel = new ManagePluginServiceViewModel(ScenarioContext.Current.Get<Mock<IPluginServiceModel>>("model").Object, pluginService);
            ScenarioContext.Current.Add("viewModel", viewModel);
            var view = Utils.GetView<ManagePluginServiceControl>();
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

        [Given(@"I open New Plugin Service Connector")]
        public void GivenIOpenNewPluginServiceConnector()
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginServiceControl>(Utils.ViewNameKey);
            Assert.IsNotNull(sourceControl);
            Assert.IsNotNull(sourceControl.DataContext);
        }


        [Given(@"""(.*)"" tab is opened")]
        public void GivenTabIsOpened(string headerText)
        {
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.AreEqual(headerText, viewModel.Header);
        }

        [Given(@"Select a source is focused")]
        public void GivenSelectASourceIsFocused()
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            var isSelectSourceFocused = view.IsSelectSourceFocused();
            //Assert.IsTrue(isSelectSourceFocused);
        }

        [Given(@"all other steps are ""(.*)""")]
        public void GivenAllOtherStepsAre(string p0)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            const string State = "Disabled";
            Utils.CheckControlEnabled("2 Select a Namespace", State, view);
            Utils.CheckControlEnabled("3 Select an Action", State, view);
            Utils.CheckControlEnabled("4 Provide Test Values", State, view);
            Utils.CheckControlEnabled("5 Defaults and Mapping", State, view);
        }

        [When(@"Test Connection is ""(.*)""")]
        public void WhenTestConnectionIs(string p0)
        {
            var pluginServiceModel = ScenarioContext.Current.Get<Mock<IPluginServiceModel>>("model");
            SetupModel(pluginServiceModel);
            var view = Utils.GetView<ManagePluginServiceControl>();
            view.TestAction();
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.IsFalse(viewModel.IsTesting);
            Assert.AreEqual(String.Empty, viewModel.ErrorText);
        }

        [Then(@"the Test Connection is ""(.*)""")]
        [When(@"the Test Connection is ""(.*)""")]
        public void ThenTheTestConnectionIs(string p0)
        {
            var pluginServiceModel = ScenarioContext.Current.Get<Mock<IPluginServiceModel>>("model");
            SetupModel(pluginServiceModel);
            var view = Utils.GetView<ManagePluginServiceControl>();
            view.TestAction();
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.AreNotSame(String.Empty, viewModel.ErrorText);
        }

        [When(@"""(.*)"" is clicked")]
        public void WhenIsClicked(string name)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();

            switch (name)
            {
                case "Save":
                    view.Save();
                    break;
                case "Test":
                    view.TestAction();
                    break;
                case "Refresh":
                    view.RefreshAction();
                    break;
            }
        }

        [Then(@"the Save Dialog is opened")]
        public void ThenTheSaveDialogIsOpened()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify();
        }

        [Then(@"""(.*)"" is ""(.*)""")]
        [When(@"""(.*)"" is ""(.*)""")]
        [Given(@"""(.*)"" is ""(.*)""")]
        public void GivenIs(string name, string state)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            switch (name)
            {
                case "1 Select a Source":
                    Utils.CheckControlEnabled(name, state, view);
                    break;
                case "2 Select a Namespace":
                    Utils.CheckControlEnabled(name, state, view);
                    break;
                case "3 Select an Action":
                    Utils.CheckControlEnabled(name, state, view);
                    break;
                case "4 Provide Test Values":
                    Utils.CheckControlEnabled(name, state, view);
                    break;
                case "5 Defaults and Mapping":
                    Utils.CheckControlEnabled(name, state, view);
                    break;

            }
        }

        [Given(@"the ""(.*)"" button is clicked")]
        public void GivenTheButtonIsClicked(string p0)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            view.NewAction();
        }

        [Then(@"the ""(.*)"" tab is opened")]
        public void ThenTheTabIsOpened(string headerText)
        {
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.AreEqual(headerText, viewModel.Header);
        }

        [Then(@"""(.*)"" isopened in another tab")]
        public void ThenIsopenedInAnotherTab(string p0)
        {
            //var PluginServiceModel = ScenarioContext.Current.Get<Mock<IPluginServiceModel>>("model");
            //PluginServiceModel.Verify(a => a.EditSource(_demoPluginSourceDefinition));
        }

        [Given(@"""(.*)"" is selected as source")]
        public void GivenIsSelectedAsSource(string selectedSourceName)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            var selectedDataSource = view.GetSelectedPluginSource();
            Assert.AreEqual(selectedSourceName, selectedDataSource.Name);
        }

        [Then(@"""(.*)"" is selected as action")]
        public void ThenIsSelectedAsAction(string selectedActionName)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            var selectedAction = view.GetSelectedActionSource();
            Assert.AreEqual(selectedActionName, selectedAction.FullName);
        }

        [When(@"I save as ""(.*)""")]
        public void WhenISaveAs(string resourceName)
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ResourceName).Returns(new ResourceName("", resourceName));
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Returns(MessageBoxResult.OK);
            var managePluginServiceControl = ScenarioContext.Current.Get<ManagePluginServiceControl>(Utils.ViewNameKey);
            managePluginServiceControl.Save();
        }

        [Then(@"title is ""(.*)""")]
        public void ThenTitleIs(string title)
        {
            var viewModel = ScenarioContext.Current.Get<ManagePluginServiceViewModel>("viewModel");
            Assert.AreEqual(title, viewModel.Header);
        }

        [Given(@"input mappings are")]
        [Then(@"input mappings are")]
        public void GivenInputMappingsAre(Table table)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            var inputMappings = view.GetInputs();
            var i = 0;
            if (inputMappings == null)
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

        [Then(@"output mappings are")]
        public void ThenOutputMappingsAre(Table table)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            var outputMappings = ((ManagePluginServiceViewModel)view.DataContext).OutputMapping;

            if (outputMappings == null)
            {
                Assert.AreEqual(0, table.RowCount);
            }
            else
            {
                foreach (var output in outputMappings)
                {
                    var outputMapping = output;
                    if (outputMapping != null)
                    {
                    }
                }
            }
        }

        [When(@"I select ""(.*)"" as source")]
        public void WhenISelectAsSource(string pluginName)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            PluginSourceDefinition pluginSourceDefinition = new PluginSourceDefinition
            {
                Name = pluginName
            };
            view.SelectPluginSource(pluginSourceDefinition);
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.AreEqual(pluginName, viewModel.SelectedSource.Name);
        }

        [When(@"I select ""(.*)"" as namespace")]
        public void WhenISelectAsNamespace(string nameSpace)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            NamespaceItem nameValue = new NamespaceItem
            {
                FullName = nameSpace
            };
            view.SelectNamespace(nameValue);
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.AreEqual(nameSpace, viewModel.SelectedNamespace.FullName);
        }

        [When(@"I select ""(.*)"" as action")]
        public void WhenISelectAsAction(string actionName)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            PluginAction pluginAction = new PluginAction
            {
                FullName = actionName
            };
            view.SelectAction(pluginAction);
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.AreEqual(actionName, viewModel.SelectedAction.FullName);
        }

        [Given(@"I change the source to ""(.*)""")]
        public void GivenIChangeTheSourceTo(string pluginName)
        {
            var view = Utils.GetView<ManagePluginServiceControl>();
            PluginSourceDefinition pluginSourceDefinition = new PluginSourceDefinition
            {
                Name = pluginName
            };
            view.SelectPluginSource(pluginSourceDefinition);
            var viewModel = Utils.GetViewModel<ManagePluginServiceViewModel>();
            Assert.AreEqual(pluginName, viewModel.SelectedSource.Name);
        }




    }

}
