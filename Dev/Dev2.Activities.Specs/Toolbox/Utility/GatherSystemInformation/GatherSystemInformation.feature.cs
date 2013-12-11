﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18052
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

#region Designer generated code

using TechTalk.SpecFlow;

#pragma warning disable

namespace Dev2.Activities.Specs.Toolbox.Utility.GatherSystemInformation
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class GatherSystemInformationFeature
    {
        private static TechTalk.SpecFlow.ITestRunner testRunner;

#line 1 "GatherSystemInformation.feature"
#line hidden

        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            var featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"),
                                                                "GatherSystemInformation",
                                                                "In order to use system information\r\nAs a warewolf user\r\nI want a tool that I retr" +
                                                                "ieve system info", ProgrammingLanguage.CSharp,
                                                                ((string[]) (null)));
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
                 && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "GatherSystemInformation")))
            {
                FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(
            "Assign a system operating system into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemOperatingSystemIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system operating system into a scalar",
                                                                  ((string[]) (null)));
#line 7
            this.ScenarioSetup(scenarioInfo);
#line 8
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"OperatingSystem\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 9
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 10
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 11
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system service pack into a scalar")
        ]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemServicePackIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system service pack into a scalar",
                                                                  ((string[]) (null)));
#line 13
            this.ScenarioSetup(scenarioInfo);
#line 14
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"ServicePack\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 15
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 16
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 17
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system OS Bit Value into a scalar")
        ]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemOSBitValueIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system OS Bit Value into a scalar",
                                                                  ((string[]) (null)));
#line 19
            this.ScenarioSetup(scenarioInfo);
#line 20
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"OSBitValue\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 21
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 22
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"Int32\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 23
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system date time into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemDateTimeIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system date time into a scalar",
                                                                  ((string[]) (null)));
#line 25
            this.ScenarioSetup(scenarioInfo);
#line 26
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"FullDateTime\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 27
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 28
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"DateTime\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 29
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(
            "Assign a system Date Time Format into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemDateTimeFormatIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system Date Time Format into a scalar",
                                                                  ((string[]) (null)));
#line 31
            this.ScenarioSetup(scenarioInfo);
#line 32
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"DateTimeFormat\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 33
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 34
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 35
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(
            "Assign a system Disk Available into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemDiskAvailableIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system Disk Available into a scalar",
                                                                  ((string[]) (null)));
#line 37
            this.ScenarioSetup(scenarioInfo);
#line 38
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"DiskAvailable\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 39
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 40
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 41
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system Disk Total into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemDiskTotalIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system Disk Total into a scalar",
                                                                  ((string[]) (null)));
#line 43
            this.ScenarioSetup(scenarioInfo);
#line 44
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"DiskTotal\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 45
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 46
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 47
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(
            "Assign a system Physical Memory Available into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemPhysicalMemoryAvailableIntoAScalar()
        {
            var scenarioInfo =
                new TechTalk.SpecFlow.ScenarioInfo("Assign a system Physical Memory Available into a scalar",
                                                   ((string[]) (null)));
#line 49
            this.ScenarioSetup(scenarioInfo);
#line 50
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"PhysicalMemoryAvailable\"",
                             ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 51
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 52
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"Int32\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 53
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(
            "Assign a system Physical Memory Total into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemPhysicalMemoryTotalIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(
                "Assign a system Physical Memory Total into a scalar", ((string[]) (null)));
#line 55
            this.ScenarioSetup(scenarioInfo);
#line 56
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"PhysicalMemoryTotal\"",
                             ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 57
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 58
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"Int32\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 59
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system CPU Available into a scalar"
            )]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemCPUAvailableIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system CPU Available into a scalar",
                                                                  ((string[]) (null)));
#line 61
            this.ScenarioSetup(scenarioInfo);
#line 62
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"CPUAvailable\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 63
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 64
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 65
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system CPU Total into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemCPUTotalIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system CPU Total into a scalar",
                                                                  ((string[]) (null)));
#line 67
            this.ScenarioSetup(scenarioInfo);
#line 68
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"CPUTotal\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 69
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 70
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 71
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system Language into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemLanguageIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system Language into a scalar",
                                                                  ((string[]) (null)));
#line 73
            this.ScenarioSetup(scenarioInfo);
#line 74
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"Language\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 75
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 76
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 77
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system Region into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemRegionIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system Region into a scalar",
                                                                  ((string[]) (null)));
#line 79
            this.ScenarioSetup(scenarioInfo);
#line 80
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"Region\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 81
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 82
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 83
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system User Roles into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemUserRolesIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system User Roles into a scalar",
                                                                  ((string[]) (null)));
#line 85
            this.ScenarioSetup(scenarioInfo);
#line 86
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"UserRoles\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 87
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 88
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 89
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system User Name into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemUserNameIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system User Name into a scalar",
                                                                  ((string[]) (null)));
#line 91
            this.ScenarioSetup(scenarioInfo);
#line 92
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"UserName\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 93
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 94
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 95
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign a system Domain into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemDomainIntoAScalar()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign a system Domain into a scalar",
                                                                  ((string[]) (null)));
#line 97
            this.ScenarioSetup(scenarioInfo);
#line 98
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"Domain\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 99
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 100
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 101
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(
            "Assign a system Number Of Warewolf Agents into a scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignASystemNumberOfWarewolfAgentsIntoAScalar()
        {
            var scenarioInfo =
                new TechTalk.SpecFlow.ScenarioInfo("Assign a system Number Of Warewolf Agents into a scalar",
                                                   ((string[]) (null)));
#line 103
            this.ScenarioSetup(scenarioInfo);
#line 104
            testRunner.Given("I have a variable \"[[testvar]]\" and I selected \"NumberOfWarewolfAgents\"",
                             ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 105
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 106
            testRunner.Then("the value of the variable \"[[testvar]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 107
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Assign User Roles into a recordset")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "GatherSystemInformation")]
        public virtual void AssignUserRolesIntoARecordset()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assign User Roles into a recordset",
                                                                  ((string[]) (null)));
#line 109
            this.ScenarioSetup(scenarioInfo);
#line 110
            testRunner.Given("I have a variable \"[[my().roles]]\" and I selected \"UserRoles\"", ((string) (null)),
                             ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 111
            testRunner.When("the gather system infomartion tool is executed", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 112
            testRunner.Then("the value of the variable \"[[my(2).roles]]\" is a valid \"String\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 113
            testRunner.And("gather system info execution has \"NO\" error", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}

#pragma warning restore

#endregion