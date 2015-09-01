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
namespace Warewolf.AcceptanceTesting.PluginService
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class PluginServiceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PluginService.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PluginService", "In order to use .net dlls\nAs a warewolf user\nI want to be able to create plugin s" +
                    "ervices", ProgrammingLanguage.CSharp, new string[] {
                        "PluginService"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "PluginService")))
            {
                Warewolf.AcceptanceTesting.PluginService.PluginServiceFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Opening Plugin Service Connector tab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginService")]
        public virtual void OpeningPluginServiceConnectorTab()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Opening Plugin Service Connector tab", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("I open New Plugin Service Connector", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("\"New Plugin Connector\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("Select a source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("\"1 Select a Source\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("\"2 Select a Namespace\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("\"3 Select an Action\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.And("\"4 Provide Test Values\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.And("\"Test\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
    testRunner.And("\"5 Defaults and Mapping\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input",
                        "Default Value",
                        "Required Field",
                        "Empty Null"});
#line 18
 testRunner.And("input mappings are", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias"});
#line 20
 testRunner.Then("output mappings are", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Create new Plugin Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginService")]
        public virtual void CreateNewPluginSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create new Plugin Source", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 25
 testRunner.Given("I open New Plugin Service Connector", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
 testRunner.And("\"New Plugin Connector\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.And("Select a source is focused", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
 testRunner.And("all other steps are \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.And("the \"New\" button is clicked", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.Then("\"New Plugin Source\" isopened in another tab", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating Plugin Service by selecting existing source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginService")]
        public virtual void CreatingPluginServiceBySelectingExistingSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating Plugin Service by selecting existing source", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 34
 testRunner.Given("I open New Plugin Service Connector", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 35
 testRunner.And("\"New Plugin Connector\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.When("I select \"testingPluginSrc\" as source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 37
 testRunner.And("\"2 Select a Namespace\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.And("\"3 Select an Action\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.When("I select \"Unlimited Framework Plugins EmailPlugin\" as namespace", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
 testRunner.Then("\"Select an action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
 testRunner.When("I select \"DummySent\" as action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
 testRunner.And("\"4 Provide Test Values\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
 testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.When("\"Test\" is clicked", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
 testRunner.And("Test Connection is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.Then("\"5 Edit Default and Mapping Names\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 47
 testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input",
                        "Default Value",
                        "Required Field",
                        "Empty Null"});
            table3.AddRow(new string[] {
                        "data",
                        "",
                        "Selected",
                        "Selected"});
#line 48
 testRunner.And("input mappings are", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias"});
            table4.AddRow(new string[] {
                        "Name",
                        "Name"});
#line 51
 testRunner.Then("output mappings are", ((string)(null)), table4, "Then ");
#line 54
 testRunner.When("\"Save\" is clicked", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 55
 testRunner.Then("the Save Dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Opening saved Plugin Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginService")]
        public virtual void OpeningSavedPluginService()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Opening saved Plugin Service", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 59
 testRunner.Given("I open \"IntegrationTestPluginNull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
 testRunner.And("\"Edit IntegrationTestPluginNull\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
 testRunner.And("\"testingPluginSrc\" is selected as source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.And("\"2 Select a Namespace\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.And("\"3 Select an Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.And("\"4 Provide Test Values\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.And("\"5 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
 testRunner.And("I change the source to \"PrimitivePlugintest\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.Then("\"2 Select a namespace\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 68
 testRunner.And("3 Select an Action VM is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
 testRunner.And("Provide Test Values VM is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
 testRunner.And("Edit Default and Mapping Names VM is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
    testRunner.When("I select \"Unlimited Framework Plugins EmailPlugin\" as namespace", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 73
 testRunner.Then("3 Select an Action VM is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 74
 testRunner.When("I select \"FetchStringvalue\" as action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 75
 testRunner.Then("Provide Test Values VM is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
 testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
 testRunner.When("Test Connection is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 78
 testRunner.Then("\"5 Default and Mapping\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 79
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input",
                        "Default Value",
                        "Required Field",
                        "Empty Null"});
            table5.AddRow(new string[] {
                        "data",
                        "",
                        "",
                        ""});
#line 80
 testRunner.And("input mappings are", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias"});
            table6.AddRow(new string[] {
                        "Name",
                        "Name"});
#line 83
 testRunner.Then("output mappings are", ((string)(null)), table6, "Then ");
#line 86
 testRunner.When("I save as \"IntegrationTestPluginNull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 87
    testRunner.Then("the Save Dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 88
    testRunner.Then("title is \"Edit IntegrationTestPluginNull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Refreshing plugin source action step")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginService")]
        public virtual void RefreshingPluginSourceActionStep()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Refreshing plugin source action step", ((string[])(null)));
#line 91
this.ScenarioSetup(scenarioInfo);
#line 92
 testRunner.Given("I open \"IntegrationTestPluginNull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 93
 testRunner.And("\"Edit IntegrationTestPluginNull\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
 testRunner.And("\"2 Select a Namespace\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
 testRunner.And("\"3 Select an Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
 testRunner.And("\"4 Provide Test Values\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
 testRunner.And("\"5 Edit Default and Mapping Names\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
 testRunner.When("\"Refresh\" is clicked", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 99
 testRunner.Then("\"3 Select an action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 100
 testRunner.When("I select \"FetchStringvalue\" as action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 101
 testRunner.And("\"4 Provide Test Values\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
 testRunner.Then("\"5 Default and Mapping\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 103
 testRunner.And("\"Test\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
 testRunner.When("Test Connection is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 105
 testRunner.And("\"5 Edit Default and Mapping Names\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input",
                        "Default Value",
                        "Required Field",
                        "Empty Null"});
            table7.AddRow(new string[] {
                        "data",
                        "",
                        "Checked",
                        "Checked"});
#line 107
 testRunner.And("input mappings are", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Output",
                        "Output Alias"});
            table8.AddRow(new string[] {
                        "Name",
                        "Name"});
#line 110
 testRunner.Then("output mappings are", ((string)(null)), table8, "Then ");
#line 113
 testRunner.When("\"Save\" is clicked", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 114
 testRunner.When("I save as \"Testing IntegrationTestPluginNull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 115
    testRunner.Then("the Save Dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 116
    testRunner.Then("title is \"Edit IntegrationTestPluginNull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Plugin service GetType test")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginService")]
        public virtual void PluginServiceGetTypeTest()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Plugin service GetType test", ((string[])(null)));
#line 120
this.ScenarioSetup(scenarioInfo);
#line 121
 testRunner.Given("I open New Plugin Service Connector", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 122
 testRunner.When("I select \"IntegrationTestPluginNull\" as source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 123
 testRunner.And("\"2 Select a Namespace\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.When("I select \"Unlimited Framework Plugins EmailPlugin\" as namespace", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 125
 testRunner.Then("\"3 Select an Action\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 126
 testRunner.And("\"GetType\" is not an Action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
