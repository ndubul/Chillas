﻿using System;
using Dev2.Studio.UI.Tests.Enums;
using Dev2.Studio.UI.Tests.UIMaps.Activities;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Dev2.Studio.UI.Tests.Tests.Activities
{
    /// <summary>
    /// Summary description for DbServiceTests
    /// </summary>
    [CodedUITest]
    public class WebServiceTests : UIMapBase
    {
        #region Fields


        #endregion

        #region Setup
        [TestInitialize]
        public void TestInit()
        {
            Init();
        }
        #endregion

        #region Cleanup
        [TestCleanup]
        public void MyTestCleanup()
        {
            TabManagerUIMap.CloseAllTabs();
        }
        #endregion

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("WebServiceTests_CodedUI")]
        public void WebServiceTests_CodedUI_EditService_ExpectErrorButton()
        {
            var newMapping = "ZZZ" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            UITestControl theTab = ExplorerUIMap.DoubleClickWorkflow("ErrorFrameworkTestWorkflow", "UI TEST");

            UITestControl service = WorkflowDesignerUIMap.FindControlByAutomationId(theTab, "FetchCities");

            DsfActivityUiMap activityUiMap = new DsfActivityUiMap(false) { Activity = service, TheTab = theTab };

            activityUiMap.ClickEdit();

            //Wizard actions

            WebServiceWizardUIMap.ClickMappingTab();
            WebServiceWizardUIMap.EnterDataIntoMappingTextBox(6, newMapping);
            WebServiceWizardUIMap.ClickSaveButton(2);
            ResourceChangedPopUpUIMap.ClickCancel();

            //Assert the the error button is there
            Assert.IsTrue(activityUiMap.IsFixErrorButtonShowing());
            //Click the fix errors button
            activityUiMap.ClickFixErrors();
            activityUiMap.ClickCloseMapping();
            //Assert that the fix errors button isnt there anymore
            Assert.IsFalse(activityUiMap.IsFixErrorButtonShowing());
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("WizardUiTests_WebServiceWizard")]
        public void WizardUiTests_WebServiceWizard_WebServiceInputMappings_ExpectedInputMappingsCreated()
        {
            string newGuid = Guid.NewGuid().ToString();
            string remove = newGuid.Remove(8);
            string newWebserviceName = "WebService" + remove;

            //Open wizard
            RibbonUIMap.ClickNewWebService();
            KeyboardCommands.SendTabs(2);
            KeyboardCommands.SendDownArrows(1);
            KeyboardCommands.SendTabs(8);
            KeyboardCommands.SendDownArrows(1);
            KeyboardCommands.SendTabs(4);
            KeyboardCommands.SelectAllText();
            KeyboardCommands.SendKey("?[[a]]=[[b]][[c]]&[[d]]=[[f]]");
            KeyboardCommands.SendLeftArrows(2);
            KeyboardCommands.SendKey("e");
            KeyboardCommands.SendTabs(2);
            KeyboardCommands.SendEnter(200);
            KeyboardCommands.SendTabs(1);
            KeyboardCommands.SendEnter(1000);
            KeyboardCommands.SendTabs(3);
            KeyboardCommands.SendKey(newWebserviceName);
            KeyboardCommands.SendTabs(1);
            KeyboardCommands.SendEnter(200);

            UITestControl theTab = RibbonUIMap.CreateNewWorkflow(1500);
            UITestControl activityControl = ExplorerUIMap.DragResourceOntoWorkflowDesigner(theTab, newWebserviceName, "Unassigned", ServiceType.Services);

            var activity = new DsfActivityUiMap(false) { Activity = activityControl, TheTab = theTab };

            Assert.AreEqual("a", activity.GetInputMappingToServiceValue(1));
            Assert.AreEqual("b", activity.GetInputMappingToServiceValue(2));
            Assert.AreEqual("c", activity.GetInputMappingToServiceValue(3));
            Assert.AreEqual("d", activity.GetInputMappingToServiceValue(4));
            Assert.AreEqual("fe", activity.GetInputMappingToServiceValue(5));

            ExplorerUIMap.RightClickDeleteResource(newWebserviceName, "Unassigned", ServiceType.Services, "localhost");
        }
    }
}
