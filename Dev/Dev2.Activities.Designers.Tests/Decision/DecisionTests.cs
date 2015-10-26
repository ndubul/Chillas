
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dev2.Activities.Designers2.Decision;
using Dev2.Studio.Core.Activities.Utils;
using Dev2.TO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev2.Activities.Designers.Tests.Decision
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DecisionTests
    {
        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_Equal_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("=", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_NotContains_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Doesn't Contain", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_Contains_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Contains", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_NotEqual_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("<> (Not Equal)", true, false);

        }
        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_EndsWith_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Ends With", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_StartsWith_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Starts With", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_Regex_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Is Regex", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_GreaterThan_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled(">", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_LessThan_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("<", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_LessThanEqual_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("<=", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_GreaterThanEqual_RequiresCriteriaInput_IsCriteriaEnabledTrue()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled(">=", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_NotInRequiresCriteriaInputList_IsCriteriaEnabledFalseSearchCriteriaEmptyString()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Is Numeric", false, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_IndexObjectIsnotZero()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Is Numeric", false, false, -1);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_InRequiresCriteriaInputList_IsCriteriaEnabledFalseSearchCriteriaInBetween()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Is Between", true, false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_InRequiresCriteriaInputList_IsCriteriaEnabledFalseSearchCriteriaNotBetween()
        {
            Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled("Not Between", true, false);
        }

        void Verify_OnSearchTypeChanged_IsSearchCriteriaEnabled(string searchType, bool isSearchCriteriaEnabled, bool isSearchCriteriaBlank, int indexObject = 0)
        {
            //------------Setup for test--------------------------
            var decisionTO = new DecisionTO("xxxx","xxxx", searchType, 1);

            var items = new List<DecisionTO>
            {
                decisionTO
            };

            var viewModel = new DecisionDesignerViewModel(CreateModelItem(items));

            //------------Precondition---------------------------           
           

            //------------Execute Test---------------------------
            viewModel.SearchTypeUpdatedCommand.Execute(indexObject);
            //viewModel.SearchTypeUpdatedCommand.Execute(0);

            //------------Assert Results-------------------------
            Assert.AreEqual(isSearchCriteriaEnabled, decisionTO.IsSearchCriteriaVisible);
            Assert.AreEqual(isSearchCriteriaBlank, string.IsNullOrEmpty(decisionTO.SearchCriteria));
        }
        
        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_OnSearchTypeChanged")]
        public void DecisionDesignerViewModel_OnSearchTypeChanged_IndexOutOfBounds_DoesNothing()
        {
            Verify_OnSearchTypeChanged_IndexOutOfBounds_DoesNothing(-2);
            Verify_OnSearchTypeChanged_IndexOutOfBounds_DoesNothing(2);
            Verify_OnSearchTypeChanged_IndexOutOfBounds_DoesNothing(3);
        }

        void Verify_OnSearchTypeChanged_IndexOutOfBounds_DoesNothing(int index)
        {
            //------------Setup for test--------------------------
            var items = new List<DecisionTO>
            {
                new DecisionTO("xxxx","xxxx", "Equals", 1),
                new DecisionTO("yyyy","yyyy", "Contains", 2)
            };

            var viewModel = new DecisionDesignerViewModel(CreateModelItem(items));

      

            //------------Execute Test---------------------------
            viewModel.SearchTypeUpdatedCommand.Execute(index);

            //------------Assert Results-------------------------
            foreach (var dto in items)
            {
                Assert.IsTrue(dto.IsSearchCriteriaEnabled);
                Assert.IsFalse(string.IsNullOrEmpty(dto.SearchCriteria));
            }
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DecisionDesignerViewModel_Constructor")]
        public void DecisionDesignerViewModel_Constructor_PropertiesInitialized()
        {
            //------------Setup for test--------------------------
            var items = new List<DecisionTO>
            {
                new DecisionTO("xxxx","xxxx", "=", 1),
                new DecisionTO("yyyy","yyyy", "Contains", 2)
            };

            //------------Execute Test---------------------------
            var viewModel = new DecisionDesignerViewModel(CreateModelItem(items));

            //------------Assert Results-------------------------
            Assert.IsNotNull(viewModel.ModelItem);
            Assert.IsNotNull(viewModel.Collection);
            Assert.AreEqual("ResultsCollection", viewModel.CollectionName);
            Assert.AreEqual(0, viewModel.TitleBarToggles.Count);
        }

        //[TestMethod]
        //[Owner("Pieter Terblanche")]
        //[TestCategory("DecisionDesignerViewModel_ValidateThis")]
        //public void DecisionDesignerViewModel_ValidateThis_FieldsToSearchIsNotEmptyAndResultNotEmpty_DoesNotHaveErrors()
        //{
        //    //------------Setup for test--------------------------
        //    var items = new List<DecisionTO> { new DecisionTO("", "", "", 0) };
        //    var mi = CreateModelItem(items);
        //    mi.SetProperty("FieldsToSearch", "[[rec().set]]");
        //    mi.SetProperty("Result", "[[a]]");
        //    var viewModel = new DecisionDesignerViewModel(mi);
        //    SetDataListString(viewModel);
        //    //------------Execute Test---------------------------
        //    viewModel.Validate();

        //    //------------Assert Results-------------------------
        //    Assert.IsNull(viewModel.Errors);
        //}

        //[TestMethod]
        //[Owner("Pieter Terblanche")]
        //[TestCategory("DecisionDesignerViewModel_ValidateThis")]
        //public void DecisionDesignerViewModel_ValidateThis_FieldsToSearchAndResultIsEmptyOrWhiteSpace_DoesHaveErrors()
        //{
        //    //------------Setup for test--------------------------
        //    var items = new List<DecisionTO> { new DecisionTO("","", "", 0) };
        //    var mi = CreateModelItem(items);
        //    mi.SetProperty("FieldsToSearch", " ");
        //    mi.SetProperty("Result", " ");
        //    var viewModel = new DecisionDesignerViewModel(mi);
        //    SetDataListString(viewModel);
        //    //------------Execute Test---------------------------
        //    viewModel.Validate();

        //    //------------Assert Results-------------------------
        //    Assert.AreEqual(2, viewModel.Errors.Count);
        //    StringAssert.Contains(viewModel.Errors[0].Message, "'In Field(s)' cannot be empty or only white space");
        //    StringAssert.Contains(viewModel.Errors[1].Message, "'Result' cannot be empty or only white space");
        //}


        
        //[TestMethod]
        //[Owner("Pieter Terblanche")]
        //[TestCategory("DecisionDesignerViewModel_ValidateCollectionItem")]
        //public void DecisionDesignerViewModel_ValidateCollectionItem_ValidatesPropertiesOfTO()
        //{
        //    //------------Setup for test--------------------------
        //    var mi = ModelItemUtils.CreateModelItem(new DsfDecision());
        //    mi.SetProperty("DisplayName", "Find");
        //    mi.SetProperty("DisplayText", "[[a]]");
        //    mi.SetProperty("TrueArmText", "[[a]]");
        //    mi.SetProperty("FalseArmText", "[[a]]");

        //    var dto1 = new FindRecordsTO("", "Starts With", 0);
        //    var dto2 = new FindRecordsTO("", "Ends With", 1);
        //    var dto3 = new FindRecordsTO("", "Doesn't Start With", 2);
        //    var dto4 = new FindRecordsTO("", "Doesn't End With", 3);
        //    var dto5 = new FindRecordsTO("", "Is Between", 4);
        //    var dto6 = new FindRecordsTO("", "Is Not Between", 5);

        //    // ReSharper disable PossibleNullReferenceException
        //    var miCollection = mi.Properties["ResultsCollection"].Collection;
        //    var dtoModelItem1 = miCollection.Add(dto1);
        //    var dtoModelItem2 = miCollection.Add(dto2);
        //    var dtoModelItem3 = miCollection.Add(dto3);
        //    var dtoModelItem4 = miCollection.Add(dto4);
        //    var dtoModelItem5 = miCollection.Add(dto5);
        //    var dtoModelItem6 = miCollection.Add(dto6);
        //    // ReSharper restore PossibleNullReferenceException

        //    var viewModel = new DecisionDesignerViewModel(mi);
        //    SetDataListString(viewModel);
        //    //------------Execute Test---------------------------
        //    viewModel.Validate();

        //    //------------Assert Results-------------------------
        //    Assert.AreEqual(10, viewModel.Errors.Count);

        //    StringAssert.Contains(viewModel.Errors[0].Message, "'Match' cannot be empty");
        //    Verify_IsFocused(dtoModelItem1, viewModel.Errors[0].Do, "IsSearchCriteriaFocused");

        //    StringAssert.Contains(viewModel.Errors[1].Message, "'Match' cannot be empty");
        //    Verify_IsFocused(dtoModelItem2, viewModel.Errors[1].Do, "IsSearchCriteriaFocused");

        //    StringAssert.Contains(viewModel.Errors[2].Message, "'Match' cannot be empty");
        //    Verify_IsFocused(dtoModelItem3, viewModel.Errors[2].Do, "IsSearchCriteriaFocused");

        //    StringAssert.Contains(viewModel.Errors[3].Message, "'Match' cannot be empty");
        //    Verify_IsFocused(dtoModelItem4, viewModel.Errors[3].Do, "IsSearchCriteriaFocused");

        //    StringAssert.Contains(viewModel.Errors[4].Message, "'Match' cannot be empty");
        //    Verify_IsFocused(dtoModelItem5, viewModel.Errors[4].Do, "IsSearchCriteriaFocused");

        //    StringAssert.Contains(viewModel.Errors[5].Message, "'From' cannot be empty");
        //    Verify_IsFocused(dtoModelItem5, viewModel.Errors[5].Do, "IsFromFocused");

        //    StringAssert.Contains(viewModel.Errors[6].Message, "'To' cannot be empty");
        //    Verify_IsFocused(dtoModelItem5, viewModel.Errors[6].Do, "IsToFocused");

        //    StringAssert.Contains(viewModel.Errors[7].Message, "'Match' cannot be empty");
        //    Verify_IsFocused(dtoModelItem6, viewModel.Errors[7].Do, "IsSearchCriteriaFocused");

        //    StringAssert.Contains(viewModel.Errors[9].Message, "'To' cannot be empty");
        //    Verify_IsFocused(dtoModelItem6, viewModel.Errors[9].Do, "IsToFocused");
        //}

        static void SetDataListString(DecisionDesignerViewModel viewModel)
        {
            viewModel.GetDatalistString = () =>
            {
                const string trueString = "True";
                const string noneString = "None";
                var datalist = string.Format("<DataList><var Description=\"\" IsEditable=\"{0}\" ColumnIODirection=\"{1}\" /><a Description=\"\" IsEditable=\"{0}\" ColumnIODirection=\"{1}\" /><b Description=\"\" IsEditable=\"{0}\" ColumnIODirection=\"{1}\" /><h Description=\"\" IsEditable=\"{0}\" ColumnIODirection=\"{1}\" /><r Description=\"\" IsEditable=\"{0}\" ColumnIODirection=\"{1}\" /><rec Description=\"\" IsEditable=\"{0}\" ColumnIODirection=\"{1}\" ><set Description=\"\" IsEditable=\"{0}\" ColumnIODirection=\"{1}\" /></rec></DataList>", trueString, noneString);
                return datalist;
            };
        }

        void Verify_IsFocused(ModelItem modelItem, Action doError, string isFocusedPropertyName)
        {
            Assert.IsFalse(modelItem.GetProperty<bool>(isFocusedPropertyName));
            doError.Invoke();
            Assert.IsTrue(modelItem.GetProperty<bool>(isFocusedPropertyName));
        }

        static ModelItem CreateModelItem(IEnumerable<DecisionTO> items, string displayName = "Find")
        {
            var modelItem = ModelItemUtils.CreateModelItem(new DsfDecision());
            modelItem.SetProperty("DisplayName", displayName);

            return modelItem;
        }
    }
}
