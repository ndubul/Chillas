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
namespace Warewolf.AcceptanceTesting.Explorer
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ExplorerFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Explorer.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Explorer", "In order to manage my service\r\nAs a Warewolf User\r\nI want explorer view of my res" +
                    "ources with management options", ProgrammingLanguage.CSharp, new string[] {
                        "Explorer"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "Explorer")))
            {
                Warewolf.AcceptanceTesting.Explorer.ExplorerFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Connected to localhost server")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void ConnectedToLocalhostServer()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Connected to localhost server", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Expand a folder")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void ExpandAFolder()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Expand a folder", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 13
 testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
 testRunner.And("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.When("I open \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
 testRunner.Then("I should see \"18\" children for \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Rename folder")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void RenameFolder()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rename folder", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 19
 testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
 testRunner.And("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.When("I rename \"localhost/Folder 2\" to \"Folder New\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 22
 testRunner.Then("I should see \"18\" children for \"Folder New\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.Then("I should see the path \"localhost/Folder New\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.Then("I should not see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Search explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void SearchExplorer()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search explorer", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
 testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
 testRunner.And("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.When("I search for \"Folder 3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 30
 testRunner.Then("I should see \"Folder 3\" only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 31
 testRunner.And("I should not see \"Folder 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.And("I should not see \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.And("I should not see \"Folder 4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.And("I should not see \"Folder 5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating Folder in localhost")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void CreatingFolderInLocalhost()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating Folder in localhost", ((string[])(null)));
#line 36
this.ScenarioSetup(scenarioInfo);
#line 38
   testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
   testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
   testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
   testRunner.When("I add \"MyNewFolder\" in \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
   testRunner.Then("I should see the path \"localhost/MyNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating And Deleting Folder in localhost")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void CreatingAndDeletingFolderInLocalhost()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating And Deleting Folder in localhost", ((string[])(null)));
#line 44
this.ScenarioSetup(scenarioInfo);
#line 46
  testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 47
  testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
  testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 49
  testRunner.When("I add \"MyNewFolder\" in \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 50
  testRunner.Then("I should see the path \"localhost/MyNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 51
  testRunner.And("I should see \"6\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
  testRunner.When("I delete \"localhost/MyNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 54
  testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 55
  testRunner.And("I should not see \"New Folder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating And Deleting Folder and Popup says cancel in localhost")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void CreatingAndDeletingFolderAndPopupSaysCancelInLocalhost()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating And Deleting Folder and Popup says cancel in localhost", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 60
  testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 61
  testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
  testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 63
  testRunner.When("I add \"MyOtherNewFolder\" in \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 64
  testRunner.Then("I should see the path \"localhost/MyOtherNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 65
  testRunner.And("I should see \"6\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.And("I choose to \"Cancel\" Any Popup Messages", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
  testRunner.Then("I should see \"6\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 70
  testRunner.When("I open \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 71
  testRunner.Then("I should see \"18\" children for \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 72
  testRunner.When("I create \"localhost/Folder 2/myNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 73
  testRunner.Then("I should see \"19\" children for \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 74
  testRunner.Then("I should see the path \"localhost/Folder 2/myNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
  testRunner.And("I choose to \"OK\" Any Popup Messages", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.When("I delete \"localhost/Folder 2/myNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 78
  testRunner.Then("I should see \"18\" children for \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 79
  testRunner.Then("I should not see the path \"localhost/Folder 2/myNewFolder\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deleting Resource in folders")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void DeletingResourceInFolders()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deleting Resource in folders", ((string[])(null)));
#line 83
this.ScenarioSetup(scenarioInfo);
#line 84
   testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 85
   testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 86
   testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 87
   testRunner.When("I open \"Folder 5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 88
   testRunner.And("I create the \"localhost/Folder 5/deleteresource\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
   testRunner.Then("I should see the path \"localhost/Folder 5/deleteresource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 90
   testRunner.When("I delete \"localhost/Folder 5/deleteresource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 91
   testRunner.Then("I should not see \"deleteresouce\" in \"Folder 5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deleting Resource in localhost Server")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void DeletingResourceInLocalhostServer()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deleting Resource in localhost Server", ((string[])(null)));
#line 93
this.ScenarioSetup(scenarioInfo);
#line 94
   testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 95
   testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 96
   testRunner.And("I create the \"localhost/Folder 1/Resource 1\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
   testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 98
   testRunner.Then("I should see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 99
   testRunner.When("I delete \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 100
   testRunner.Then("I should not see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Opening Versions in Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void OpeningVersionsInExplorer()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Opening Versions in Explorer", ((string[])(null)));
#line 105
this.ScenarioSetup(scenarioInfo);
#line 106
  testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 107
  testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 108
  testRunner.And("I create the \"localhost/Folder 1/Resource 1\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
  testRunner.Then("I should see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 110
  testRunner.And("I Setup  \"3\" Versions to be returned for \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
 testRunner.When("I Show Version History for \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 114
 testRunner.Then("I should see \"3\" versions with \"View\" Icons in \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 118
  testRunner.When("I Make \"localhost/Folder 1/Resource 1/v.1\" the current version of \"localhost/Fold" +
                    "er 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 119
 testRunner.Then("I should see \"4\" versions with \"View\" Icons in \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 121
 testRunner.When("I Delete Version \"localhost/Folder 1/Resource 1/v.1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 123
 testRunner.Then("I should see \"3\" versions with \"View\" Icons in \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Renaming Folder And Workflow Service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void RenamingFolderAndWorkflowService()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Renaming Folder And Workflow Service", ((string[])(null)));
#line 220
this.ScenarioSetup(scenarioInfo);
#line 221
 testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 222
 testRunner.And("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 223
 testRunner.When("I rename \"localhost/Folder 2\" to \"Folder New\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 224
 testRunner.Then("I should see \"18\" children for \"Folder New\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 225
 testRunner.When("I open \"Folder New\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 226
 testRunner.And("I create the \"localhost/Folder New/Resource 1\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 227
 testRunner.And("I create the \"localhost/Folder New/Resource 2\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 228
 testRunner.Then("I should see the path \"localhost/Folder New\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 229
 testRunner.Then("I should see the path \"localhost/Folder New/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 230
 testRunner.And("I should not see \"Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 231
 testRunner.And("I should not see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 232
 testRunner.When("I rename \"localhost/Folder New/Resource 1\" to \"WorkFlow1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 233
 testRunner.Then("I should see the path \"localhost/Folder New/WorkFlow1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 234
 testRunner.When("I rename \"localhost/Folder New/Resource 2\" to \"WorkFlow1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 235
 testRunner.Then("Conflict error message is occurs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Searching resources by using filter")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void SearchingResourcesByUsingFilter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Searching resources by using filter", ((string[])(null)));
#line 282
this.ScenarioSetup(scenarioInfo);
#line 283
  testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 284
  testRunner.And("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 285
  testRunner.When("I open \"Folder 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 286
  testRunner.And("I create the \"localhost/Folder 1/Resource 1\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 287
  testRunner.Then("I should see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 288
  testRunner.When("I search for \"Folder 1\" in explorer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 289
  testRunner.Then("I should see the path \"localhost/Folder 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 290
  testRunner.Then("I should not see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 291
  testRunner.Then("I should not see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 292
  testRunner.When("I search for \"Resource 1\" in explorer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 293
  testRunner.When("I open \"Folder 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 294
  testRunner.Then("I should see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Checking versions")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void CheckingVersions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Checking versions", ((string[])(null)));
#line 298
this.ScenarioSetup(scenarioInfo);
#line 299
  testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 300
  testRunner.When("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 301
  testRunner.And("I create the \"localhost/Folder 1/Resource 1\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 302
  testRunner.Then("I should see \"5\" folders", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 303
  testRunner.And("I Setup  \"3\" Versions to be returned for \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 304
  testRunner.When("I Show Version History for \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 305
  testRunner.Then("I should see \"3\" versions with \"View\" Icons in \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 306
  testRunner.When("I search for \"Resource 1\" in explorer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 307
  testRunner.Then("I should see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 308
  testRunner.Then("I should see \"3\" versions with \"View\" Icons in \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Clear filter")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Explorer")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Explorer")]
        public virtual void ClearFilter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Clear filter", ((string[])(null)));
#line 311
this.ScenarioSetup(scenarioInfo);
#line 312
  testRunner.Given("the explorer is visible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 313
  testRunner.And("I open \"localhost\" server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 314
  testRunner.When("I open \"Folder 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 315
  testRunner.And("I create the \"localhost/Folder 1/Resource 1\" of type \"WorkflowService\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 316
  testRunner.Then("I should see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 317
  testRunner.When("I search for \"Folder 1\" in explorer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 318
  testRunner.Then("I should see the path \"localhost/Folder 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 319
  testRunner.Then("I should not see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 320
  testRunner.Then("I should not see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 321
  testRunner.When("I search for \"Resource 1\" in explorer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 322
  testRunner.When("I open \"Folder 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 323
  testRunner.Then("I should see the path \"localhost/Folder 1/Resource 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 324
  testRunner.When("I clear \"Explorer\" Filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 325
  testRunner.Then("I should see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 326
  testRunner.Then("I should see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 327
  testRunner.Then("I should see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 328
  testRunner.Then("I should see the path \"localhost/Folder 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
