﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dev2.Activities.Sharepoint;
using Dev2.Common.Interfaces.Infrastructure.SharedModels;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.TO;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Warewolf.Storage;

// ReSharper disable InconsistentNaming
namespace Dev2.Tests.Activities.Utils
{
    [TestClass]
    public class SharepointUtilsTests
    {
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_GetValidReadListItems")]
        public void SharepointUtils_GetValidReadListItems_NullList_EmptyList()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var validList = sharepointUtils.GetValidReadListItems(null);
            //------------Assert Results-------------------------
            Assert.IsNotNull(validList);
            Assert.AreEqual(0,validList.Count());
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_GetValidReadListItems")]
        public void SharepointUtils_GetValidReadListItems_WhereVariableNameNotNull_ListWithItem()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var validList = sharepointUtils.GetValidReadListItems(new List<SharepointReadListTo> { new SharepointReadListTo("Bob", "Title", "Title",""), new SharepointReadListTo(null, "Title", "Title","") });
            //------------Assert Results-------------------------
            Assert.IsNotNull(validList);
            var tos = validList as IList<SharepointReadListTo> ?? validList.ToList();
            Assert.AreEqual(1,tos.Count());
            Assert.AreEqual("Bob",tos[0].VariableName);
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_GetValidReadListItems")]
        public void SharepointUtils_GetValidReadListItems_WhereVariableNameNull_ListWithoutItem()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var validList = sharepointUtils.GetValidReadListItems(new List<SharepointReadListTo> { new SharepointReadListTo("Bob", "Title", "Title", ""), new SharepointReadListTo(null, "Title", "Title", "") });
            //------------Assert Results-------------------------
            Assert.IsNotNull(validList);
            var tos = validList as IList<SharepointReadListTo> ?? validList.ToList();
            Assert.AreEqual(1,tos.Count());
            Assert.AreEqual("Bob",tos[0].VariableName);
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_GetValidReadListItems")]
        public void SharepointUtils_GetValidReadListItems_WhereVariableNameEmpty_ListWithoutItem()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var validList = sharepointUtils.GetValidReadListItems(new List<SharepointReadListTo> { new SharepointReadListTo("Bob", "Title", "Title", ""), new SharepointReadListTo("", "Title", "Title", "") });
            //------------Assert Results-------------------------
            Assert.IsNotNull(validList);
            var tos = validList as IList<SharepointReadListTo> ?? validList.ToList();
            Assert.AreEqual(1,tos.Count());
            Assert.AreEqual("Bob",tos[0].VariableName);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_BuildCamlQuery")]
        public void SharepointUtils_BuildCamlQuery_NoFilters_ShouldBeCreateAllItemsQuery()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var camlQuery = sharepointUtils.BuildCamlQuery(new ExecutionEnvironment(), new List<SharepointSearchTo>(), new List<ISharepointFieldTo>(),0);
            //------------Assert Results-------------------------
            Assert.AreEqual(CamlQuery.CreateAllItemsQuery().ViewXml,camlQuery.ViewXml);
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_BuildCamlQuery")]
        public void SharepointUtils_BuildCamlQuery_NoValidFiltersFieldNameEmpty_ShouldBeCreateAllItemsQuery()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var camlQuery = sharepointUtils.BuildCamlQuery(new ExecutionEnvironment(), new List<SharepointSearchTo> { new SharepointSearchTo("", "Equal", "Bob", 1) }, new List<ISharepointFieldTo>(), 0);
            //------------Assert Results-------------------------
            Assert.AreEqual(CamlQuery.CreateAllItemsQuery().ViewXml, camlQuery.ViewXml);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_BuildCamlQuery")]
        public void SharepointUtils_BuildCamlQuery_NoValidFiltersValueToMatchEmpty_ShouldBeCreateAllItemsQuery()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var camlQuery = sharepointUtils.BuildCamlQuery(new ExecutionEnvironment(), new List<SharepointSearchTo> { new SharepointSearchTo("Title", "Equal", "", 1) }, new List<ISharepointFieldTo>(), 0);
            //------------Assert Results-------------------------
            Assert.AreEqual(CamlQuery.CreateAllItemsQuery().ViewXml, camlQuery.ViewXml);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_BuildCamlQuery")]
        public void SharepointUtils_BuildCamlQuery_NoValidFiltersFieldNameNull_ShouldBeCreateAllItemsQuery()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var camlQuery = sharepointUtils.BuildCamlQuery(new ExecutionEnvironment(), new List<SharepointSearchTo> { new SharepointSearchTo(null, "Equal", "Bob", 1) }, new List<ISharepointFieldTo>(), 0);
            //------------Assert Results-------------------------
            Assert.AreEqual(CamlQuery.CreateAllItemsQuery().ViewXml, camlQuery.ViewXml);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_BuildCamlQuery")]
        public void SharepointUtils_BuildCamlQuery_NoValidFiltersValueToMatchNull_ShouldBeCreateAllItemsQuery()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var camlQuery = sharepointUtils.BuildCamlQuery(new ExecutionEnvironment(), new List<SharepointSearchTo> { new SharepointSearchTo("Title", "Equal", null, 1) }, new List<ISharepointFieldTo>(), 0);
            //------------Assert Results-------------------------
            Assert.AreEqual(CamlQuery.CreateAllItemsQuery().ViewXml, camlQuery.ViewXml);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_BuildCamlQuery")]
        public void SharepointUtils_BuildCamlQuery_ValidFilter_HasQueryWithNoAnd()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var camlQuery = sharepointUtils.BuildCamlQuery(new ExecutionEnvironment(), new List<SharepointSearchTo> { new SharepointSearchTo("Title", "Equal", "Bob", 1) { InternalName = "Title" } }, new List<ISharepointFieldTo> { new SharepointFieldTo { InternalName = "Title", Type = SharepointFieldType.Text } }, 0);
            //------------Assert Results-------------------------
            Assert.AreEqual("<View><Query><Where><FieldRef Name=\"Title\"></FieldRef><Value Type=\"Text\">Bob</Value>"+Environment.NewLine+"</Where></Query></View>",camlQuery.ViewXml);
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_BuildCamlQuery")]
        public void SharepointUtils_BuildCamlQuery_ValidFilters_HasQueryWithAnd()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var camlQuery = sharepointUtils.BuildCamlQuery(new ExecutionEnvironment(), new List<SharepointSearchTo> { new SharepointSearchTo("Title", "Equal", "Bob", 1) { InternalName = "Title" }, new SharepointSearchTo("ID", "Equal", "1", 1) { InternalName = "ID" } }, new List<ISharepointFieldTo> { new SharepointFieldTo { InternalName = "Title", Type = SharepointFieldType.Text }, new SharepointFieldTo { InternalName = "ID", Type = SharepointFieldType.Number } }, 0);
            //------------Assert Results-------------------------
            Assert.AreEqual("<View><Query><Where><And><FieldRef Name=\"Title\"></FieldRef><Value Type=\"Text\">Bob</Value>" + Environment.NewLine + "<FieldRef Name=\"ID\"></FieldRef><Value Type=\"Integer\">1</Value>" + Environment.NewLine + "</And></Where></Query></View>", camlQuery.ViewXml);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_CastWarewolfValueToCorrectType")]
        public void SharepointUtils_CastWarewolfValueToCorrectType_Boolean_ShouldGiveBoolValue()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var boolValue = sharepointUtils.CastWarewolfValueToCorrectType("true", SharepointFieldType.Boolean);
            //------------Assert Results-------------------------
            Assert.IsInstanceOfType(boolValue,typeof(Boolean));
        } 
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_CastWarewolfValueToCorrectType")]
        public void SharepointUtils_CastWarewolfValueToCorrectType_Text_ShouldGiveStringValue()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var boolValue = sharepointUtils.CastWarewolfValueToCorrectType("Bob", SharepointFieldType.Text);
            //------------Assert Results-------------------------
            Assert.IsInstanceOfType(boolValue,typeof(String));
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_CastWarewolfValueToCorrectType")]
        public void SharepointUtils_CastWarewolfValueToCorrectType_Note_ShouldGiveStringValue()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var boolValue = sharepointUtils.CastWarewolfValueToCorrectType("Bob", SharepointFieldType.Note);
            //------------Assert Results-------------------------
            Assert.IsInstanceOfType(boolValue,typeof(String));
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_CastWarewolfValueToCorrectType")]
        public void SharepointUtils_CastWarewolfValueToCorrectType_Integer_ShouldGiveIntValue()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var boolValue = sharepointUtils.CastWarewolfValueToCorrectType("2", SharepointFieldType.Integer);
            //------------Assert Results-------------------------
            Assert.IsInstanceOfType(boolValue,typeof(Int32));
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_CastWarewolfValueToCorrectType")]
        public void SharepointUtils_CastWarewolfValueToCorrectType_Number_ShouldGiveIntValue()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var boolValue = sharepointUtils.CastWarewolfValueToCorrectType("2", SharepointFieldType.Number);
            //------------Assert Results-------------------------
            Assert.IsInstanceOfType(boolValue,typeof(Int32));
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_CastWarewolfValueToCorrectType")]
        public void SharepointUtils_CastWarewolfValueToCorrectType_Currency_ShouldGiveDecimalValue()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var boolValue = sharepointUtils.CastWarewolfValueToCorrectType("2.01", SharepointFieldType.Currency);
            //------------Assert Results-------------------------
            Assert.IsInstanceOfType(boolValue,typeof(Decimal));
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SharepointUtils_CastWarewolfValueToCorrectType")]
        public void SharepointUtils_CastWarewolfValueToCorrectType_Date_ShouldGiveDateValueValue()
        {
            //------------Setup for test--------------------------
            var sharepointUtils = new SharepointUtils();
            
            //------------Execute Test---------------------------
            var boolValue = sharepointUtils.CastWarewolfValueToCorrectType(DateTime.Now, SharepointFieldType.DateTime);
            //------------Assert Results-------------------------
            Assert.IsInstanceOfType(boolValue,typeof(DateTime));
        }
    }
}
