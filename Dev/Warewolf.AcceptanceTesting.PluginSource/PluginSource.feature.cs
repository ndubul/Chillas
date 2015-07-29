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
namespace Warewolf.AcceptanceTesting.PluginSource
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class PluginSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PluginSource.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PluginSource", "In order to create plugins\r\nAs a Warewolf User\r\nI want to be able to select dlls " +
                    "as a source to be used", ProgrammingLanguage.CSharp, new string[] {
                        "PluginSource"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "PluginSource")))
            {
                Warewolf.AcceptanceTesting.PluginSource.PluginSourceFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("New Plugin Source File")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void NewPluginSourceFile()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("New Plugin Source File", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
 testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
 testRunner.Then("\"New Plugin Source\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
 testRunner.And("title is \"New Plugin Source\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.And("I click \"File System\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.Then("local drive \"C:\\\" is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.Then("local drive \"D:\\\" is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.When("I open \"C:\\\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
 testRunner.And("Assembly value is \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Clicks"});
            table1.AddRow(new string[] {
                        "Development"});
            table1.AddRow(new string[] {
                        "Dev"});
            table1.AddRow(new string[] {
                        "Binaries"});
            table1.AddRow(new string[] {
                        "MS Fakes"});
            table1.AddRow(new string[] {
                        "Microsoft.QualityTools.Testing.Fakes.dll"});
#line 27
 testRunner.When("I click", ((string)(null)), table1, "When ");
#line 34
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 35
 testRunner.And("Assembly value is \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools.Te" +
                    "sting.Fakes.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.When("I change Assembly to \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools" +
                    ".Testing.Fakes.dl\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 37
 testRunner.Then("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 38
 testRunner.When("I change Assembly to \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools" +
                    ".Testing.Fakes.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.When("I save as \"Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
    testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
 testRunner.Then("title is \"Edit Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 43
 testRunner.And("\"Testing Resource Save\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("New Plugin Source GAC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void NewPluginSourceGAC()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("New Plugin Source GAC", ((string[])(null)));
#line 46
this.ScenarioSetup(scenarioInfo);
#line 47
 testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 48
 testRunner.When("I click \"GAC\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
 testRunner.And("Assembly is \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
 testRunner.When("I Search for \"AuditPolicyGPMan\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
 testRunner.And("I click \"AuditPolicyGPManagedStubs.Interop, Version=6.1.0.0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
 testRunner.Then("Assembly value is \"GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Culture=neutra" +
                    "l, PublicKeyToken=b03f5f7f11d50a3a.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 54
 testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.When("I save Plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing Plugin Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void EditingPluginSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing Plugin Source", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 59
 testRunner.Given("I open \"Test\" plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
 testRunner.Then("title is \"Edit Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
 testRunner.And("\"GAC\" is \"visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.And("Assembly value is \"GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Culture=neutra" +
                    "l, PublicKeyToken=b03f5f7f11d50a3a.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.When("I click \"BDATunePIA, Version=6.1.0.0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
 testRunner.Then("Assembly value is \"GAC:BDATunePIA, Version=6.1.0.0, Culture=neutral, PublicKeyTok" +
                    "en=31bf3856ad364e35.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 66
 testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.When("I save Plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Change Plugin Source Assembly Input")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void ChangePluginSourceAssemblyInput()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Change Plugin Source Assembly Input", ((string[])(null)));
#line 70
this.ScenarioSetup(scenarioInfo);
#line 71
 testRunner.Given("I open \"Test File\" plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 72
 testRunner.Then("title is \"Edit Test File\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 73
 testRunner.And("\"File System\" is \"visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.And("\"C:\\\" is \"visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.And("Assembly value is \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools.Te" +
                    "sting.Fakes.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
 testRunner.When("I click \"Dev2.Common.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 78
 testRunner.Then("Assembly value is \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Dev2.Common.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 79
 testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
 testRunner.When("I save Plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Refresh New Plugin Source File")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void RefreshNewPluginSourceFile()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Refresh New Plugin Source File", ((string[])(null)));
#line 82
this.ScenarioSetup(scenarioInfo);
#line 83
 testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Clicks"});
            table2.AddRow(new string[] {
                        "File System"});
            table2.AddRow(new string[] {
                        "C:\\"});
            table2.AddRow(new string[] {
                        "Development"});
            table2.AddRow(new string[] {
                        "Dev"});
            table2.AddRow(new string[] {
                        "Binaries"});
            table2.AddRow(new string[] {
                        "MS Fakes"});
            table2.AddRow(new string[] {
                        "Microsoft.QualityTools.Testing.Fakes.dll"});
#line 84
 testRunner.When("I click", ((string)(null)), table2, "When ");
#line 93
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 94
 testRunner.When("I refresh the filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 95
 testRunner.And("Assembly value is \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools.Te" +
                    "sting.Fakes.dll\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
 testRunner.And("\"Microsoft.QualityTools.Testing.Fakes.dll\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Refresh New Plugin Source GAC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void RefreshNewPluginSourceGAC()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Refresh New Plugin Source GAC", ((string[])(null)));
#line 98
this.ScenarioSetup(scenarioInfo);
#line 99
 testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 100
 testRunner.When("I click \"GAC\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 101
 testRunner.And("I click \"GAC\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
 testRunner.When("I filter for \"BDATunePIA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 104
 testRunner.And("\"GAC:BDATunePIA, Version=6.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e3" +
                    "5.dll\" is \"visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.And("GAC only has one option in the tree", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
 testRunner.When("I refresh the filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 107
 testRunner.And("GAC only has one option in the tree", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("load all dependancies after filter cleared")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute()]
        public virtual void LoadAllDependanciesAfterFilterCleared()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("load all dependancies after filter cleared", new string[] {
                        "ignore"});
#line 114
this.ScenarioSetup(scenarioInfo);
#line 115
 testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 116
 testRunner.When("I click \"GAC\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 117
 testRunner.And("GAC tree is loading", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
 testRunner.And("I filter for \"BDATunePIA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
 testRunner.And("\"GAC:BDATunePIA, Version=6.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e3" +
                    "5.dll\" is \"visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
 testRunner.And("GAC only has one option in the tree", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
 testRunner.When("I clear the filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 122
 testRunner.Then("GAC has all option in the tree \"visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Search while GAC tree view is loading")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void SearchWhileGACTreeViewIsLoading()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search while GAC tree view is loading", ((string[])(null)));
#line 125
this.ScenarioSetup(scenarioInfo);
#line 126
 testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 127
 testRunner.When("I click \"GAC\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 128
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
 testRunner.When("I Search for \"vjslib\" before the tree view is completely loaded", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 130
 testRunner.And("I click \"vjslib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3" +
                    "a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
 testRunner.Then("Assembly value is \"GAC:vjslib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b" +
                    "03f5f7f11d50a3a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 132
 testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
 testRunner.When("I save Plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 134
 testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
