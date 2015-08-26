﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Warewolf.AcceptanceTesting.DatabaseService
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class DBServiceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "DB Service.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "DB Service", "In order to manage my database services\r\nAs a Warewolf User\r\nI want to be shown t" +
                    "he database service setup", ProgrammingLanguage.CSharp, new string[] {
                        "DBService"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "DB Service")))
            {
                Warewolf.AcceptanceTesting.DatabaseService.DBServiceFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        public virtual void CreatingDBService()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating DB Service", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("I click \"New Data Base Service Connector\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.Then("\"New DB Connector\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 10
 testRunner.And("Data Source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("\"1 Data Source\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("\"2 Select Action\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("\"3 Test Connector and Calculate Outputs\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.When("I select \"DemoDB\" as data source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("\"2 Select Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
 testRunner.When("I select \"dbo.ConverToint\" as the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
 testRunner.Then("\"3 Test Connector and Calculate Outputs\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
 testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "charValue"});
            table1.AddRow(new string[] {
                        "1"});
#line 21
 testRunner.And("inputs are", ((string)(null)), table1, "And ");
#line 24
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.When("I test the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Recordset Name",
                        "Result"});
            table2.AddRow(new string[] {
                        "dbo_ConverToInt(1)",
                        "1"});
#line 26
 testRunner.Then("outputs are", ((string)(null)), table2, "Then ");
#line 29
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Inputs",
                        "Required Field",
                        "Empty is Null"});
            table3.AddRow(new string[] {
                        "charValue",
                        "",
                        ""});
#line 31
 testRunner.And("input mappings are", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias",
                        "Recordset Name"});
            table4.AddRow(new string[] {
                        "result",
                        "result",
                        "dbo_ConverToInt"});
#line 34
 testRunner.And("output mappings are", ((string)(null)), table4, "And ");
#line 37
 testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.Then("Save Dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Opening Saved DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        public virtual void OpeningSavedDBService()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Opening Saved DB Service", ((string[])(null)));
#line 42
this.ScenarioSetup(scenarioInfo);
#line 43
   testRunner.Given("I open \"InsertDummyUser\" service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 44
   testRunner.And("\"InsertDummyUser\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
   testRunner.And("Data Source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
   testRunner.And("\"DemoDB\" is selected as the data source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
   testRunner.And("\"dbo.InsertDummyUser\" is selected as the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
   testRunner.And("\"1 Data Source\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
   testRunner.And("\"2 Select Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
   testRunner.And("\"3 Test Connector and Calculate Outputs\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "fname",
                        "lname",
                        "username",
                        "password",
                        "lastAccessDate"});
            table5.AddRow(new string[] {
                        "Change",
                        "Test",
                        "wolf",
                        "Dev",
                        "10/1/1990"});
#line 51
   testRunner.And("inputs are", ((string)(null)), table5, "And ");
#line 54
   testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
   testRunner.And("\"4 Edit Default and Mapping Names\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Inputs",
                        "Default Value",
                        "Required Field",
                        "Empty is Null"});
            table6.AddRow(new string[] {
                        "fname",
                        "",
                        "",
                        ""});
            table6.AddRow(new string[] {
                        "lname",
                        "",
                        "",
                        ""});
            table6.AddRow(new string[] {
                        "username",
                        "",
                        "",
                        ""});
            table6.AddRow(new string[] {
                        "password",
                        "",
                        "",
                        ""});
            table6.AddRow(new string[] {
                        "lastAccessDate",
                        "",
                        "",
                        ""});
#line 56
   testRunner.And("input mappings are", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias",
                        "Recordset Name"});
            table7.AddRow(new string[] {
                        "UserID",
                        "UserID",
                        "dbo_InsertDummyUser"});
#line 63
   testRunner.And("output mappings are", ((string)(null)), table7, "And ");
#line 66
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing DB Service Mappings")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        public virtual void EditingDBServiceMappings()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing DB Service Mappings", ((string[])(null)));
#line 68
 this.ScenarioSetup(scenarioInfo);
#line 69
   testRunner.Given("I open \"InsertDummyUser\" service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 70
   testRunner.And("\"InsertDummyUser\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
   testRunner.And("Data Source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
   testRunner.And("\"DemoDB\" is selected as the data source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
   testRunner.And("\"dbo.InsertDummyUser\" is selected as the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
   testRunner.And("\"1 Data Source\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
   testRunner.And("\"2 Select Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
   testRunner.And("\"3 Test Connector and Calculate Outputs\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
   testRunner.And("Inspect Data Connector hyper link is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "fname",
                        "lname",
                        "username",
                        "password",
                        "lastAccessDate"});
            table8.AddRow(new string[] {
                        "Change",
                        "Test",
                        "wolf",
                        "Dev",
                        "10/1/1990"});
#line 79
   testRunner.When("inputs are", ((string)(null)), table8, "When ");
#line 82
   testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
   testRunner.When("I test the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Recordset Name",
                        "UserID"});
            table9.AddRow(new string[] {
                        "ddo_InsertDummyUser(1)",
                        "1"});
#line 85
   testRunner.Then("outputs are", ((string)(null)), table9, "Then ");
#line 88
   testRunner.And("\"4 Edit Default and Mapping Names\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Inputs",
                        "Default Value",
                        "Required Field",
                        "Empty is Null"});
            table10.AddRow(new string[] {
                        "fname",
                        "",
                        "",
                        ""});
            table10.AddRow(new string[] {
                        "lname",
                        "",
                        "",
                        ""});
            table10.AddRow(new string[] {
                        "username",
                        "",
                        "",
                        ""});
            table10.AddRow(new string[] {
                        "password",
                        "",
                        "",
                        ""});
            table10.AddRow(new string[] {
                        "lastAccessDate",
                        "",
                        "",
                        ""});
#line 89
   testRunner.When("input mappings are", ((string)(null)), table10, "When ");
#line 96
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias",
                        "Recordset Name"});
            table11.AddRow(new string[] {
                        "UserID",
                        "UserID",
                        "dbo_InsertDummyUser"});
#line 97
   testRunner.And("output mappings are", ((string)(null)), table11, "And ");
#line 100
   testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 101
   testRunner.Then("\"InsertDummyUser\" is saved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 102
   testRunner.And("Save Dialog is not opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing DB Service and Test Execution is unsuccesful")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        public virtual void EditingDBServiceAndTestExecutionIsUnsuccesful()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing DB Service and Test Execution is unsuccesful", ((string[])(null)));
#line 104
 this.ScenarioSetup(scenarioInfo);
#line 105
   testRunner.Given("I open \"InsertDummyUser\" service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 106
   testRunner.And("\"InsertDummyUser\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
   testRunner.And("Data Source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
   testRunner.And("\"DemoDB\" is selected as the data source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
   testRunner.And("\"dbo.InsertDummyUser\" is selected as the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
   testRunner.And("\"1 Data Source\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
   testRunner.And("\"2 Select Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
   testRunner.And("\"3 Test Connector and Calculate Outputs\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
   testRunner.And("Inspect Data Connector hyper link is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "fname",
                        "lname",
                        "username",
                        "password",
                        "lastAccessDate"});
            table12.AddRow(new string[] {
                        "Change",
                        "Test",
                        "wolf",
                        "Dev",
                        "10/1/1990"});
#line 114
   testRunner.And("inputs are", ((string)(null)), table12, "And ");
#line 117
   testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
   testRunner.When("testing the action fails", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 120
   testRunner.Then("\"4 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Inputs",
                        "Default Value",
                        "Required Field",
                        "Empty is Null"});
#line 121
   testRunner.And("input mappings are", ((string)(null)), table13, "And ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias",
                        "Recordset Name"});
#line 123
 testRunner.And("output mappings are", ((string)(null)), table14, "And ");
#line 125
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Refresh in select Action")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute()]
        public virtual void RefreshInSelectAction()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Refresh in select Action", new string[] {
                        "ignore"});
#line 129
this.ScenarioSetup(scenarioInfo);
#line 130
 testRunner.Given("I click New Data Base Service Connector", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 131
 testRunner.Then("\"New Database Connector\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 132
 testRunner.And("\"1 Data Source\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
 testRunner.And("\"2 Select Action\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
 testRunner.And("\"3 Test Connector and Calculate Outputs\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
 testRunner.When("I select \"DemoDB\" as data source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 138
 testRunner.Then("\"2 Select Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 139
 testRunner.And("the \"Refresh\" button is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
 testRunner.When("I select \"dbo.InsertDummyUser\" as the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 141
 testRunner.Then("\"3 Test Connector and Calculate Outputs\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "fname",
                        "lname",
                        "username",
                        "password",
                        "lastAccessDate"});
            table15.AddRow(new string[] {
                        "Test",
                        "Tester",
                        "wolf",
                        "Dev",
                        "10/1/1990"});
#line 142
 testRunner.And("inputs are", ((string)(null)), table15, "And ");
#line 145
   testRunner.And("I select the \"Refresh\" button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
   testRunner.Then("\"3 Test Connector and Calculate Outputs\" is reloaded", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 147
   testRunner.And("all inputs are cleared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
   testRunner.And("the \"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 149
   testRunner.And("the \"Save\" is disabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 150
   testRunner.When("I test the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Recordset Name",
                        "UserID"});
            table16.AddRow(new string[] {
                        "dbo.InsertDummyUser(1)",
                        "15486"});
#line 151
 testRunner.Then("outputs are", ((string)(null)), table16, "Then ");
#line 154
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
 testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 157
 testRunner.Then("Save Dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Changing Actions")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        public virtual void ChangingActions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Changing Actions", ((string[])(null)));
#line 159
this.ScenarioSetup(scenarioInfo);
#line 160
 testRunner.Given("I click new Database Connector", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 161
 testRunner.Then("\"new Database Connector\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 162
 testRunner.And("\"1 Data Source\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 163
 testRunner.And("\"2 Select Action\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
 testRunner.And("\"3 Test Connector and Calculate Outputs\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 166
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
 testRunner.And("Data Source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 168
 testRunner.When("I select \"DemoDB\" as data source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 169
 testRunner.Then("\"2 Select Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 170
 testRunner.When("I select \"dbo.InsertDummyUser\" as the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 171
 testRunner.Then("\"3 Test Connector and Calculate Outputs\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "fname",
                        "lname",
                        "username",
                        "password",
                        "lastAccessDate"});
            table17.AddRow(new string[] {
                        "Dummy",
                        "User",
                        "Test",
                        "password",
                        "12/05/2001"});
#line 172
 testRunner.And("inputs are", ((string)(null)), table17, "And ");
#line 175
 testRunner.When("I change the action from \"dbo.InsertDummyUser\" to \"dbo.ImportOrder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 176
 testRunner.Then("\"3 Test Connector and Calculate Outputs\" is reloaded", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 177
 testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "ProductId"});
            table18.AddRow(new string[] {
                        "1"});
#line 178
 testRunner.And("input is now", ((string)(null)), table18, "And ");
#line 181
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
 testRunner.When("I test the action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Recordset Name",
                        "Result"});
            table19.AddRow(new string[] {
                        "dbo.ImportOrder(1)",
                        "1"});
#line 183
 testRunner.Then("outputs are", ((string)(null)), table19, "Then ");
#line 186
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 187
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Inputs",
                        "Required Field",
                        "Empty is Null"});
            table20.AddRow(new string[] {
                        "charValue",
                        "",
                        ""});
#line 188
 testRunner.And("input mappings are", ((string)(null)), table20, "And ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias",
                        "Recordset Name"});
            table21.AddRow(new string[] {
                        "result",
                        "result",
                        "dbo.ImportOrder"});
#line 191
 testRunner.And("output mappings are", ((string)(null)), table21, "And ");
#line 194
 testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 195
 testRunner.Then("Save Dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating a new Data Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        public virtual void CreatingANewDataSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating a new Data Source", ((string[])(null)));
#line 198
this.ScenarioSetup(scenarioInfo);
#line 199
 testRunner.Given("I click New Data Base Service Connector", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 200
 testRunner.Then("\"New Database Connector\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 201
 testRunner.And("Data Source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 202
 testRunner.And("\"New\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 203
 testRunner.And("\"2 Select Action\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 204
 testRunner.And("\"3 Test Connector and Calculate Outputs\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 205
 testRunner.And("\"4 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 206
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 207
 testRunner.When("I select \"New\" as data source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 208
 testRunner.Then("New Data Source Dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Ensure recordset values can be saved to a variable")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DB Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DBService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute()]
        public virtual void EnsureRecordsetValuesCanBeSavedToAVariable()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Ensure recordset values can be saved to a variable", new string[] {
                        "ignore"});
#line 212
this.ScenarioSetup(scenarioInfo);
#line 213
 testRunner.Given("I have a new Workspace opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 214
 testRunner.And("I have a saved Data Connector called \"MyDataCon\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 215
 testRunner.And("\"MyDataCon\" returns [[dbo_GetCountries().CountryID]] and [[dbo_GetCountries().Des" +
                    "cription]]", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 216
 testRunner.When("I drop \"MyDataCon\" on the design surface", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 217
 testRunner.And("I open the Database Connector to a large view", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 218
 testRunner.When("I change  [[dbo_GetCountries().CountryID]] to \"[[variable]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 219
 testRunner.And("\"MyDataCon\" is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 220
 testRunner.Then("the workflow execution has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "",
                        ""});
            table22.AddRow(new string[] {
                        "[[variable]]",
                        "Murali,Murali,india,india"});
#line 221
 testRunner.And("the debug output is", ((string)(null)), table22, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
