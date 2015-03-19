﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Warewolf.AcceptanceTesting.Explorer.Deploy
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class DeployTabFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "DeployTab.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "DeployTab", "In order to Deploy resource.\r\nAs a warewolf user\r\nI want to Deploy aresource from" +
                    " one server to another server.", ProgrammingLanguage.CSharp, new string[] {
                        "Deploy"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "DeployTab")))
            {
                Warewolf.AcceptanceTesting.Explorer.Deploy.DeployTabFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deploy Tab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void DeployTab()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deploy Tab", ((string[])(null)));
#line 37
this.ScenarioSetup(scenarioInfo);
#line 38
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
  testRunner.When("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
  testRunner.Then("the validation message is \"Source and Destination cannot be the same\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
  testRunner.And("\"Deploy\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
  testRunner.And("\"Select All Dependencies\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deploy button is enabling when selecting resource in source side")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void DeployButtonIsEnablingWhenSelectingResourceInSourceSide()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deploy button is enabling when selecting resource in source side", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 46
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 47
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
     testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
  testRunner.When("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 50
  testRunner.Then("\"Deploy\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deploy is successfull")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void DeployIsSuccessfull()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deploy is successfull", ((string[])(null)));
#line 53
this.ScenarioSetup(scenarioInfo);
#line 54
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
     testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
  testRunner.And("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.When("I deploy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 59
  testRunner.Then("deploy is successfull", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 60
  testRunner.And("the validation message is \"Items deployed successfully\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
  testRunner.And("\"Examples\\Utility - Date and Time\" is visible on Destination Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Conflicting resources on Source and Destination server")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void ConflictingResourcesOnSourceAndDestinationServer()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Conflicting resources on Source and Destination server", ((string[])(null)));
#line 64
this.ScenarioSetup(scenarioInfo);
#line 65
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 66
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
     testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
  testRunner.And("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
  testRunner.And("I deploy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "#",
                        "Source Resource",
                        "Destination Resource"});
            table1.AddRow(new string[] {
                        "1",
                        "Utility - Date and Time",
                        "Utility - Date and Time"});
#line 70
  testRunner.Then("Resource exists in the destination server popup is shown", ((string)(null)), table1, "Then ");
#line 73
  testRunner.When("I click OK on Resource exists in the destination server popup", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
  testRunner.Then("deploy is successfull", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 75
  testRunner.And("the validation message is \"Items deployed successfully\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Conflicting resources on Source and Destination server deploy is not successful")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void ConflictingResourcesOnSourceAndDestinationServerDeployIsNotSuccessful()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Conflicting resources on Source and Destination server deploy is not successful", ((string[])(null)));
#line 77
this.ScenarioSetup(scenarioInfo);
#line 78
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
     testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
  testRunner.And("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
  testRunner.And("I deploy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "#",
                        "Source Resource",
                        "Destination Resource"});
            table2.AddRow(new string[] {
                        "1",
                        "Utility - Date and Time",
                        "Utility - Date and Time"});
#line 83
  testRunner.Then("Resource exists in the destination server popup is shown", ((string)(null)), table2, "Then ");
#line 86
  testRunner.When("I click Cancel on Resource exists in the destination server popup", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 87
  testRunner.Then("deploy is not successfull", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 88
  testRunner.And("the validation message is \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Select all Dependecies is selecting dependecies")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void SelectAllDependeciesIsSelectingDependecies()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Select all Dependecies is selecting dependecies", ((string[])(null)));
#line 91
this.ScenarioSetup(scenarioInfo);
#line 92
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 93
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
     testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
  testRunner.When("I select \"My Category\\Double Roll and Check\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 96
  testRunner.Then("\"Deploy\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 97
  testRunner.And("\"Select All Dependencies\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
  testRunner.When("I Select All Dependecies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 99
  testRunner.Then("\"My Category\\Double Roll\" from Source Server is \"Selected\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Mouse right click select Dependecies is selecting dependecies")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute()]
        public virtual void MouseRightClickSelectDependeciesIsSelectingDependecies()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Mouse right click select Dependecies is selecting dependecies", new string[] {
                        "ignore"});
#line 103
this.ScenarioSetup(scenarioInfo);
#line 104
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 105
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
     testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
  testRunner.When("I select \"My Category\\Double Roll and Check\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 108
  testRunner.Then("\"Deploy\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 109
  testRunner.When("I Select All Dependecies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 110
  testRunner.Then("\"My Category\\Double Roll\" is \"Selected\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 111
  testRunner.And("\"Deploy\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Filtering Resources on Destination side")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void FilteringResourcesOnDestinationSide()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Filtering Resources on Destination side", ((string[])(null)));
#line 115
this.ScenarioSetup(scenarioInfo);
#line 116
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 117
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
     testRunner.When("I type \"Utility - Date and Time\" in Destination Server filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 119
  testRunner.Then("\"Examples\\Utility - Date and Time\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 120
  testRunner.And("\"Examples\\Utility - Date and Time Difference\" from Destination Server is \"Visible" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
  testRunner.And("I select \"Examples\\Utility - Date and Time\" from Destination Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
  testRunner.When("I clear filter on Destination Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 123
  testRunner.Then("\"Examples\\Utility - Date and Time\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 124
  testRunner.And("\"Examples\\Data - Data - Data Split\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
  testRunner.And("\"Examples\\Control Flow - Switch\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
  testRunner.And("\"Examples\\Control Flow - Sequence\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
  testRunner.And("\"Examples\\File and Folder - Copy\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
  testRunner.And("\"Examples\\File and Folder - Create\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Filtering and clearing filter on source side")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void FilteringAndClearingFilterOnSourceSide()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Filtering and clearing filter on source side", ((string[])(null)));
#line 130
this.ScenarioSetup(scenarioInfo);
#line 131
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 132
  testRunner.And("selected Source Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
     testRunner.When("I type \"Utility - Date and Time\" in Source Server filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 134
  testRunner.Then("\"Examples\\Utility - Date and Time\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 135
  testRunner.And("\"Examples\\Data - Data - Data Split\" from Source Server is \"Not Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
  testRunner.And("\"Examples\\Control Flow - Switch\" from Source Server is \"Not Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
  testRunner.And("\"Examples\\Control Flow - Sequence\" from Source Server is \"Not Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
  testRunner.And("\"Examples\\File and Folder - Copy\" from Source Server is \"Not Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
  testRunner.And("\"Examples\\File and Folder - Create\" from Source Server is \"Not Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
  testRunner.When("I clear filter on Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 141
  testRunner.Then("\"Examples\\Utility - Date and Time\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 142
  testRunner.And("\"Examples\\Data - Data - Data Split\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 143
  testRunner.And("\"Examples\\Control Flow - Switch\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
  testRunner.And("\"Examples\\Control Flow - Sequence\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
  testRunner.And("\"Examples\\File and Folder - Copy\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
  testRunner.And("\"Examples\\File and Folder - Create\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deploy is successfull when filter is on on both sides")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void DeployIsSuccessfullWhenFilterIsOnOnBothSides()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deploy is successfull when filter is on on both sides", ((string[])(null)));
#line 149
this.ScenarioSetup(scenarioInfo);
#line 150
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 151
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 152
  testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 153
     testRunner.When("I type \"Utility - Date and Time\" in Destination Server filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 154
  testRunner.Then("\"Examples\\Utility - Date and Time\" from Source Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 155
  testRunner.And("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
  testRunner.When("I type \"Utility - Date and Time\" in Destination Server filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 157
  testRunner.Then("\"Examples\\Utility - Date and Time\" from Destination Server is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 158
  testRunner.And("\"Deploy\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 159
  testRunner.When("I deploy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "#",
                        "Source Resource",
                        "Destination Resource"});
            table3.AddRow(new string[] {
                        "1",
                        "Utility - Date and Time",
                        "Utility - Date and Time"});
#line 160
  testRunner.Then("Resource exists in the destination server popup is shown", ((string)(null)), table3, "Then ");
#line 163
  testRunner.When("I click OK on Resource exists in the destination server popup", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 164
  testRunner.Then("deploy is successfull", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 165
  testRunner.And("the validation message is \"Items deployed successfully\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Selected for deploy items type is showing on deploy tab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void SelectedForDeployItemsTypeIsShowingOnDeployTab()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Selected for deploy items type is showing on deploy tab", ((string[])(null)));
#line 168
this.ScenarioSetup(scenarioInfo);
#line 169
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 170
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
  testRunner.When("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 172
  testRunner.And("I select \"DB Service\\FetchPlayers\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 173
  testRunner.And("I select \"Remote\\Source\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 174
  testRunner.Then("Data Connectors is \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 175
  testRunner.And("Services is \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 176
  testRunner.And("Sources is \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deploy Summary is showing new and overiding resources")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void DeploySummaryIsShowingNewAndOveridingResources()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deploy Summary is showing new and overiding resources", ((string[])(null)));
#line 179
this.ScenarioSetup(scenarioInfo);
#line 180
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 181
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
  testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 183
  testRunner.When("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 184
  testRunner.Then("New Resource is \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 185
  testRunner.And("Override is \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 186
  testRunner.When("I select \"New\\New\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 187
  testRunner.Then("New Resource is \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 188
  testRunner.And("Override is \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 189
  testRunner.When("I Unselect \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 190
  testRunner.And("Override is \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Not allowing to deploy when source and destination servers are same")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void NotAllowingToDeployWhenSourceAndDestinationServersAreSame()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Not allowing to deploy when source and destination servers are same", ((string[])(null)));
#line 193
this.ScenarioSetup(scenarioInfo);
#line 194
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 195
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 196
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 197
  testRunner.When("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 198
  testRunner.Then("\"Deploy\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 199
  testRunner.And("the validation message is \"Source and Destination cannot be the same\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("One server with different names in both sides not allow to deploy")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void OneServerWithDifferentNamesInBothSidesNotAllowToDeploy()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("One server with different names in both sides not allow to deploy", ((string[])(null)));
#line 202
this.ScenarioSetup(scenarioInfo);
#line 203
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 204
  testRunner.And("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 205
  testRunner.And("selected Destination Server is \"Duplicate\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 206
  testRunner.When("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 207
  testRunner.Then("\"Deploy\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 208
  testRunner.And("the validation message is \"Source and Destination cannot be the same\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deploy is enabled when I change server after validation thrown")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void DeployIsEnabledWhenIChangeServerAfterValidationThrown()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deploy is enabled when I change server after validation thrown", ((string[])(null)));
#line 211
this.ScenarioSetup(scenarioInfo);
#line 212
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 213
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 214
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 215
  testRunner.When("I select \"Examples\\Utility - Date and Time\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 216
  testRunner.Then("\"Deploy\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 217
  testRunner.And("the validation message is \"Source and Destination cannot be the same\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 218
  testRunner.When("selected Destination Server is \"Remote\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 219
  testRunner.Then("\"Deploy\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 220
   testRunner.And("the validation message is \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Deploy a resource without dependency is showing popup")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DeployTab")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Deploy")]
        public virtual void DeployAResourceWithoutDependencyIsShowingPopup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deploy a resource without dependency is showing popup", ((string[])(null)));
#line 222
this.ScenarioSetup(scenarioInfo);
#line 223
     testRunner.Given("I have deploy tab opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 224
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 225
  testRunner.And("selected Destination Server is \"localhost\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 226
  testRunner.When("I select \"DB Services/FetchPlayers\" from Source Server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 227
  testRunner.Then("\"Deploy\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 228
  testRunner.And("\"Select All Dependencies\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 229
  testRunner.When("I deploy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 230
  testRunner.Then("\"Resource Dependencyr\" popup is shown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 231
  testRunner.When("I click \"Select All Dependecies\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 232
  testRunner.Then("\"DB Services/Dependency\" from Source Server is \"Selected\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 233
  testRunner.When("I deploy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 234
  testRunner.Then("deploy is successfull", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
