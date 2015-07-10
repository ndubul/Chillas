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
namespace Warewolf.AcceptanceTesting.DatabaseSource
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class NewDatabaseSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "New Database Source.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "New Database Source", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
                    "wo numbers", ProgrammingLanguage.CSharp, new string[] {
                        "CreatingNewDBSource"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "New Database Source")))
            {
                Warewolf.AcceptanceTesting.DatabaseSource.NewDatabaseSourceFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New DB Source General Testing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void CreatingNewDBSourceGeneralTesting()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New DB Source General Testing", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
   testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
   testRunner.Then("\"New Database Source\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 31
   testRunner.And("title is \"New Database Source\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
   testRunner.When("I type Server as \"RSAKLFSVR\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Options"});
            table1.AddRow(new string[] {
                        "RSAKLFSVRGENDEV"});
            table1.AddRow(new string[] {
                        "RSAKLFSVRSBSPDC"});
            table1.AddRow(new string[] {
                        "RSAKLFSVRTFSBLD"});
            table1.AddRow(new string[] {
                        "RSAKLFSVRWRWBLD"});
#line 33
   testRunner.Then("the intellisense containts these options", ((string)(null)), table1, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Options"});
            table2.AddRow(new string[] {
                        "Microsoft SQL Server"});
            table2.AddRow(new string[] {
                        "MySQL"});
#line 39
   testRunner.And("type options contains", ((string)(null)), table2, "And ");
#line 43
   testRunner.And("I type Select The Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
   testRunner.And("type options has \"Microsoft SQL Server\" as the default", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
   testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
   testRunner.Then("Username field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 49
   testRunner.And("Password field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
   testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 51
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
   testRunner.And("\"Cancel Test\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
   testRunner.When("I click \"Test Connection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 54
   testRunner.Then("\"Cancel Test\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 55
   testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 56
   testRunner.Then("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 57
   testRunner.Then("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 58
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
   testRunner.When("I save the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
   testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
   testRunner.And("I save the source as \"SavedDBSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
   testRunner.Then("\"SavedDBSource\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 63
   testRunner.And("title is \"Edit SavedDBSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
   testRunner.When("I type Server as \"RSA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
   testRunner.Then("\"SavedDBSource *\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 66
   testRunner.And("title is \"Edit SavedDBSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New DB Source as User Auth")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void CreatingNewDBSourceAsUserAuth()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New DB Source as User Auth", ((string[])(null)));
#line 70
this.ScenarioSetup(scenarioInfo);
#line 71
    testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 72
    testRunner.And("I type Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
    testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
    testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 75
    testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
    testRunner.And("\"Test Connection\" is \"Disbled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
    testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 79
    testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 81
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
    testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 83
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
    testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 85
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
    testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
    testRunner.When("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 88
    testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 89
    testRunner.When("I save the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 90
    testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect Server Address Doesnt Allow Save Windows Auth")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void IncorrectServerAddressDoesntAllowSaveWindowsAuth()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect Server Address Doesnt Allow Save Windows Auth", ((string[])(null)));
#line 92
this.ScenarioSetup(scenarioInfo);
#line 93
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 94
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
      testRunner.And("I type Server as \"RSAKLFSVRTFSBLD\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
      testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
      testRunner.And("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
      testRunner.Then("Username field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 100
      testRunner.And("Password field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 102
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
      testRunner.When("Test Connecton is \"Unsuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 104
      testRunner.And("the validation message as \"Server not found\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 106
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect Server Address Doesnt Allow Save User Auth")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void IncorrectServerAddressDoesntAllowSaveUserAuth()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect Server Address Doesnt Allow Save User Auth", ((string[])(null)));
#line 109
this.ScenarioSetup(scenarioInfo);
#line 110
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 111
      testRunner.And("I type Server as \"RSAKLFSVRTFSBLD\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
      testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
      testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 116
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
   testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 118
   testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 120
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
      testRunner.When("Test Connecton is \"Unsuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 122
      testRunner.And("the validation message as \"Server not found\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 125
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Testing as Windows and swaping it resets the test connection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void TestingAsWindowsAndSwapingItResetsTheTestConnection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Testing as Windows and swaping it resets the test connection", ((string[])(null)));
#line 129
this.ScenarioSetup(scenarioInfo);
#line 130
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 131
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
      testRunner.And("I type Server as \"RSAKLFSVRTFSBLD\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
      testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
      testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 138
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
      testRunner.And("\"Test Connection\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
      testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 142
      testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 143
      testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 144
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 146
      testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 147
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
      testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 149
      testRunner.When("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 150
      testRunner.And("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
      testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 152
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 153
      testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 154
      testRunner.Then("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 155
      testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
      testRunner.When("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 157
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 158
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 159
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 160
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 161
   testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing saved DB Source Remembers Previouse Auth Selection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void EditingSavedDBSourceRemembersPreviouseAuthSelection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing saved DB Source Remembers Previouse Auth Selection", ((string[])(null)));
#line 164
this.ScenarioSetup(scenarioInfo);
#line 165
 testRunner.Given("I open \"Edit Database Source - Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 166
    testRunner.And("Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 168
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 169
    testRunner.And("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
    testRunner.And("Username field is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
    testRunner.And("Password field is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 172
 testRunner.And("Database \"Dev2TestingDB\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 173
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 174
 testRunner.When("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 175
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 176
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 177
    testRunner.And("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 178
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 179
    testRunner.And("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 180
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
    testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
    testRunner.And("I select \"Dev2TestingDB2\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 183
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 184
    testRunner.When("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 185
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 186
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 187
 testRunner.And("Database \"Dev2TestingDB\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 188
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 189
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing saved DB Source Remembers credentials")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void EditingSavedDBSourceRemembersCredentials()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing saved DB Source Remembers credentials", ((string[])(null)));
#line 192
this.ScenarioSetup(scenarioInfo);
#line 193
 testRunner.Given("I open \"Edit Database Source - Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 194
    testRunner.And("Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 195
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 196
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 197
    testRunner.And("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 198
    testRunner.And("Username field is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 199
    testRunner.And("Password field is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 200
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 201
 testRunner.And("Database \"Dev2TestingDB\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 202
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 203
 testRunner.When("I Edit Server as \"RSAKLFSVRWRWBLD\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 204
 testRunner.Then("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 205
    testRunner.Then("Username is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 206
    testRunner.And("Password  is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 207
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 208
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 209
    testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 210
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 211
    testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 212
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing saved DB Source and canceling without saving")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void EditingSavedDBSourceAndCancelingWithoutSaving()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing saved DB Source and canceling without saving", ((string[])(null)));
#line 214
this.ScenarioSetup(scenarioInfo);
#line 215
 testRunner.Given("I open \"Edit Database Source - Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 216
    testRunner.And("Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 217
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 218
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 219
    testRunner.And("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 220
    testRunner.And("Username field is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 221
    testRunner.And("Password field is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 222
 testRunner.And("Database \"Dev2TestingDB\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 223
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 224
 testRunner.When("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 225
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 226
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 227
    testRunner.And("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 228
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 229
    testRunner.And("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 230
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 231
    testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 232
    testRunner.And("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 233
    testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 234
    testRunner.When("I Cancel the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 235
 testRunner.And("I open \"Edit Database Source - Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 236
 testRunner.Then("underlying Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 237
    testRunner.Then("underlying Username is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 238
    testRunner.And("underlying Password  is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 239
 testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Cancel Test")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void CancelTest()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel Test", ((string[])(null)));
#line 241
this.ScenarioSetup(scenarioInfo);
#line 242
   testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 243
   testRunner.When("I type Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 244
   testRunner.Then("the intellisense containts these options", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 245
   testRunner.And("Authentication Type is selected as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 246
   testRunner.And("Username field is \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 247
   testRunner.And("Password field is \"******\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 248
   testRunner.When("I click \"Test Connection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 249
   testRunner.And("I click \"Cancel Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 250
   testRunner.Then("validation message is \"Test Cancelled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 251
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 252
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
