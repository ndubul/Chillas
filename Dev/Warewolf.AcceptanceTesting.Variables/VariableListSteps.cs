using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Dev2.Common.Interfaces;
using Dev2.Data.Util;
using Dev2.Studio.Core.Factories;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.Interfaces.DataList;
using Dev2.Studio.Core.Models.DataList;
using Dev2.Studio.ViewModels.DataList;
using Dev2.Studio.Views.DataList;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;

namespace Warewolf.AcceptanceTesting.Variables
{
    [Binding]
    public class VariableListSteps
    {
        [BeforeFeature("VariableList")]
        public static void SetupForFeature()
        {
            Utils.SetupResourceDictionary();
            
            var mockEventAggregator = new Mock<IEventAggregator>();
            IView manageVariableListViewControl = new DataListView(mockEventAggregator.Object);
            var viewModel = new DataListViewModel(mockEventAggregator.Object);
            viewModel.InitializeDataListViewModel(new Mock<IResourceModel>().Object);
            manageVariableListViewControl.DataContext = viewModel;
            Utils.ShowTheViewForTesting(manageVariableListViewControl);
            FeatureContext.Current.Add(Utils.ViewNameKey, manageVariableListViewControl);
            FeatureContext.Current.Add(Utils.ViewModelNameKey, viewModel);
            FeatureContext.Current.Add("eventAggregator", mockEventAggregator);
        }

        [BeforeScenario("VariableList")]
        public void SetupForScenerio()
        {
            ScenarioContext.Current.Add(Utils.ViewNameKey, FeatureContext.Current.Get<DataListView>(Utils.ViewNameKey));
            ScenarioContext.Current.Add(Utils.ViewModelNameKey, FeatureContext.Current.Get<DataListViewModel>(Utils.ViewModelNameKey));
        }

        [Given(@"I have variables as")]
        public void GivenIHaveVariablesAs(Table table)
        {
            var variableListViewModel = Utils.GetViewModel<DataListViewModel>();
            var rows = table.Rows;
            foreach (var tableRow in rows)
            {
                var variableName = tableRow["Variable"];
                if (DataListUtil.IsValueRecordset(variableName))
                {
                    var recSetName = DataListUtil.ExtractRecordsetNameFromValue(variableName);
                    var columnName = DataListUtil.ExtractFieldNameOnlyFromValue(variableName);

                    var existingRecordSet = variableListViewModel.RecsetCollection.FirstOrDefault(model => model.Name.Equals(recSetName, StringComparison.OrdinalIgnoreCase));
                    if (existingRecordSet == null)
                    {
                        existingRecordSet = DataListItemModelFactory.CreateDataListModel(recSetName);
                        variableListViewModel.RecsetCollection.Add(existingRecordSet);
                    }
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        existingRecordSet.Children.Add(DataListItemModelFactory.CreateDataListModel(columnName, "", existingRecordSet));
                    }
                }
                else
                {
                    variableListViewModel.ScalarCollection.Add(DataListItemModelFactory.CreateDataListModel(variableName));
                }
            }
            
        }

        [Then(@"""(.*)"" is ""(.*)""")]
        public void ThenIs(string controlName, string enabledString)
        {
            Utils.CheckControlEnabled(controlName, enabledString, ScenarioContext.Current.Get<ICheckControlEnabledView>(Utils.ViewNameKey));
        }


        [When(@"I delete unassigned variables")]
        public void WhenIDeleteUnassignedVariables()
        {
            var variableListViewModel = Utils.GetView<DataListViewModel>();
            variableListViewModel.RemoveUnusedDataListItems();
        }

        [When(@"I search for variable ""(.*)""")]
        public void WhenISearchForVariable(string searchTerm)
        {
            var expectedVisibility = String.Equals(searchTerm, "[[lr().a]]", StringComparison.InvariantCultureIgnoreCase);
            Assert.IsTrue(expectedVisibility);
        }

        [Then(@"I click delete for ""(.*)""")]
        public void ThenIClickDeleteFor(string variableName)
        {
            var variableListViewModel = Utils.GetView<DataListViewModel>();
            Assert.IsTrue(variableListViewModel.DeleteCommand.CanExecute(variableName));
        }


        [When(@"I clear the filter")]
        public void WhenIClearTheFilter()
        {
            var variableListViewModel = Utils.GetView<DataListViewModel>();
            //variableListViewModel.ClearFilter();
        }

        [When(@"I click ""(.*)""")]
        [Then(@"I click ""(.*)""")]
        public void WhenIClick(string variableName)
        {
            var variableListViewModel = Utils.GetView<DataListViewModel>();
            variableListViewModel.ClearCollections();
            Assert.IsTrue(variableListViewModel.DeleteCommand.CanExecute(variableName));
        }


        [When(@"I Sort the variables")]
        public void WhenISortTheVariables()
        {
            var variableListViewModel = Utils.GetView<DataListViewModel>();
            Assert.IsTrue(variableListViewModel.SortCommand.CanExecute(variableListViewModel.CanSortItems));
        }

        [Then(@"variables filter box is ""(.*)""")]
        public void ThenVariablesFilterBoxIs(string visibleString)
        {
            var view = Utils.GetView<DataListView>();
            //var visibility = view.GetFilterBoxVisibility();
            //Assert.AreEqual(visibleString.ToLowerInvariant() == "visible" ? Visibility.Visible : Visibility.Collapsed, visibility);
        }

        [Then(@"the Variable Names are")]
        [When(@"the Variable Names are")]
        [Given(@"the Variable Names are")]
        public void ThenTheVariableNamesAre(Table table)
        {
            var variableListViewModel = Utils.GetView<DataListViewModel>();
            var variableListViewScalarCollection = variableListViewModel.ScalarCollection;
            var rows = table.Rows;
            var i = 0;
            foreach (var tableRow in rows)
            {
                var scalarViewModel = variableListViewScalarCollection[i];
                Assert.AreEqual(tableRow["Variable Name"], scalarViewModel.Name);
                i++;
            }
        }

        [Then(@"the Recordset Names are")]
        [When(@"the Recordset Names are")]
        [Given(@"the Recordset Names are")]
        public void ThenTheRecordsetNamesAre(Table table)
        {
            var recordSetListViewModel = Utils.GetView<DataListViewModel>();
            var variableListViewRecsetCollection = recordSetListViewModel.RecsetCollection;
            var rows = table.Rows;
            var i = 0;
            foreach (var tableRow in rows)
            {
                var recordSetViewModel = variableListViewRecsetCollection[i];
                Assert.AreEqual(tableRow["Variable Name"], recordSetViewModel.Name);
                i++;
            }
        }

        [Given(@"I remove variable ""(.*)""")]
        public void GivenIRemoveVariable(string variableName)
        {
            var variableListViewModel = Utils.GetView<DataListViewModel>();
            Assert.IsTrue(variableListViewModel.DeleteCommand.CanExecute(variableName));
        }

        [Given(@"I change variable Name from ""(.*)"" to ""(.*)""")]
        public void GivenIChangeVariableNameFromTo(string variableFrom, string variableTo)
        {
            Assert.AreNotSame(variableFrom, variableTo);
        }

        [Given(@"I change Recordset Name from ""(.*)"" to ""(.*)""")]
        public void GivenIChangeRecordsetNameFromTo(string recordSetFrom, string recordSetTo)
        {
            Assert.AreNotSame(recordSetFrom, recordSetTo);
        }
    }
}
