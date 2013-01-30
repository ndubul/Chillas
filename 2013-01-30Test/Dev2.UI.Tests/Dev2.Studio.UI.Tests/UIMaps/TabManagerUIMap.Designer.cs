﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 11.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace Dev2.CodedUI.Tests.TabManagerUIMapClasses
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;


    [GeneratedCode("Coded UITest Builder", "11.0.50727.1")]
    public partial class TabManagerUIMap
    {

        public UIUI_TabManager_AutoIDTabList1 GetTabManager()
        {
            UIBusinessDesignStudioWindow2 theWindow = new UIBusinessDesignStudioWindow2();
            UIUI_TabManager_AutoIDTabList1 tabMgr = new UIUI_TabManager_AutoIDTabList1(theWindow);
            return tabMgr;
        }

        public UITestControl FindTabByName(string name)
        {
            #region Variable Declarations
            UIBusinessDesignStudioWindow2 theWindow = new UIBusinessDesignStudioWindow2();
            UIUI_TabManager_AutoIDTabList1 tabMgr = new UIUI_TabManager_AutoIDTabList1(theWindow);
            #endregion
            //string firstName = uIServiceDetailsTabPage.FriendlyName;
            UITestControl control = tabMgr.GetTab(name);

            return control;
        }

        private int GetChildrenCount()
        {
            #region Variable Declarations
            UIUI_TabManager_AutoIDTabList1 tabMgr = GetTabManager();
            #endregion
            int childCount = 0;
            UITestControl ctrl;
            if (tabMgr != null)
            {
                foreach (var child in tabMgr.GetChildren())
                {
                    if (child.ControlType.Name == "TabPage")
                    {
                        childCount = tabMgr.GetChildren().Count;
                    }
                    else
                    {
                        continue;
                    }
                }


            }
            return childCount;

        }

        private UIBusinessDesignStudioWindow2 uIBusinessDesignStudio;

        #region Properties

        private UIBusinessDesignStudioWindow2 UIBusinessDesignStudio
        {
            get
            {
                if (this.uIBusinessDesignStudio == null)
                {
                    this.uIBusinessDesignStudio = new UIBusinessDesignStudioWindow2();
                }

                return uIBusinessDesignStudio;
            }
        }

        #endregion Properties

        #region Base Classes

        public class UIBusinessDesignStudioWindow2 : WpfWindow
        {

            public UIBusinessDesignStudioWindow2()
            {
                #region Search Criteria
                this.SearchProperties[WpfWindow.PropertyNames.Name] = "Business Design Studio (DEV2\\" + Environment.UserName + ")";
                this.SearchProperties.Add(new PropertyExpression(WpfWindow.PropertyNames.ClassName, "HwndWrapper", PropertyExpressionOperator.Contains));
                this.WindowTitles.Add(TestBase.GetStudioWindowName());
                this.Find();
                #endregion
            }

            #region Properties

            public UIZea895fb767e54b00992Custom UIZea895fb767e54b00992Custom
            {
                get
                {
                    if ((this.mUIZea895fb767e54b00992Custom == null))
                    {
                        this.mUIZea895fb767e54b00992Custom = new UIZea895fb767e54b00992Custom(this);
                        this.Find();
                    }
                    return this.mUIZea895fb767e54b00992Custom;
                }
            }

            #endregion Properties

            #region Private Members

            private UIZea895fb767e54b00992Custom mUIZea895fb767e54b00992Custom;

            #endregion Private Members
        }

        private UITestControlCollection GetWorkflowNotSavedButtons()
        {
            // Workflow not saved...
            UITestControl theWindow = new UITestControl();
            theWindow.TechnologyName = "MSAA";
            theWindow.SearchProperties["Name"] = "Workflow not saved...";
            theWindow.SearchProperties["ControlType"] = "Window";
            theWindow.Find();

            UITestControlCollection saveDialogButtons = theWindow.GetChildren()[3].GetChildren();
            return saveDialogButtons;
        }
        public class UIUI_TabManager_AutoIDTabList1 : WpfTabList
        {

            public UIUI_TabManager_AutoIDTabList1(UITestControl searchLimitContainer) :
                base(searchLimitContainer)
            {
                #region Search Criteria
                this.SearchProperties[WpfTabList.PropertyNames.AutomationId] = "UI_TabManager_AutoID";
                this.WindowTitles.Add(TestBase.GetStudioWindowName());
                #endregion
            }

            public UITestControl GetTab(string childAutomationID)
            {
                WpfTabList theList = (WpfTabList)this;
                UITestControl childOfInterest = new UITestControl();
                UITestControlCollection tabList = theList.Tabs; // This lags for some reason
                foreach (WpfTabPage currentTapPage in tabList)
                {
                    if (currentTapPage.FriendlyName == childAutomationID)
                    {
                        childOfInterest = currentTapPage;
                        break;
                    }
                }
                return childOfInterest;
            }
        }

        [GeneratedCode("Coded UITest Builder", "11.0.50727.1")]
        public class UIZea895fb767e54b00992Custom : WpfCustom
        {

            public UIZea895fb767e54b00992Custom(UITestControl searchLimitContainer) :
                base(searchLimitContainer)
            {
                #region Search Criteria
                this.SearchProperties[UITestControl.PropertyNames.ClassName] = "Uia.SplitPane";
                this.SearchProperties["AutomationId"] = "Zea895fb767e54b009926eb2069050c5a";
                this.WindowTitles.Add(TestBase.GetStudioWindowName());
                this.Find();
                #endregion
            }

            #region Properties

            public UIUI_TabManager_AutoIDTabList1 UI_TabManager_AutoIDTabList
            {
                get
                {
                    if ((this.mUIUI_TabManager_AutoIDTabList == null))
                    {
                        this.mUIUI_TabManager_AutoIDTabList = new UIUI_TabManager_AutoIDTabList1(this);
                    }
                    return this.mUIUI_TabManager_AutoIDTabList;
                }
            }

            #endregion Properties

            #region Fields

            private UIUI_TabManager_AutoIDTabList1 mUIUI_TabManager_AutoIDTabList;

            #endregion Fields

        }

        #endregion Base Classes

        /// <summary>
        /// RecordedMethod1
        /// </summary>
        public void RecordedMethod1()
        {
            #region Variable Declarations
            WpfCustom uIA3b3b2ece40a4849ba77Custom = this.UIBusinessDesignStudioWindow.UIWorkflowItemPresenteCustom.UIFlowchartCustom.UIA3b3b2ece40a4849ba77Custom;
            #endregion

            // Click 'a3b3b2ec-e40a-4849-ba77-5c728e3aed95,828.5,731 828...' custom control
            Mouse.Click(uIA3b3b2ece40a4849ba77Custom, new Point(806, 232));
        }

        #region Properties
        public UIBusinessDesignStudioWindow UIBusinessDesignStudioWindow
        {
            get
            {
                if ((this.mUIBusinessDesignStudioWindow == null))
                {
                    this.mUIBusinessDesignStudioWindow = new UIBusinessDesignStudioWindow();
                }
                return this.mUIBusinessDesignStudioWindow;
            }
        }
        #endregion

        #region Fields
        private UIBusinessDesignStudioWindow mUIBusinessDesignStudioWindow;
        #endregion
    }

    [GeneratedCode("Coded UITest Builder", "11.0.50727.1")]
    public class UIBusinessDesignStudioWindow : WpfWindow
    {

        public UIBusinessDesignStudioWindow()
        {
            #region Search Criteria
            this.SearchProperties[WpfWindow.PropertyNames.Name] = TestBase.GetStudioWindowName();
            this.SearchProperties.Add(new PropertyExpression(WpfWindow.PropertyNames.ClassName, "HwndWrapper", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add(TestBase.GetStudioWindowName());
            #endregion
        }

        #region Properties
        public UIWorkflowItemPresenteCustom UIWorkflowItemPresenteCustom
        {
            get
            {
                if ((this.mUIWorkflowItemPresenteCustom == null))
                {
                    this.mUIWorkflowItemPresenteCustom = new UIWorkflowItemPresenteCustom(this);
                }
                return this.mUIWorkflowItemPresenteCustom;
            }
        }
        #endregion

        #region Fields
        private UIWorkflowItemPresenteCustom mUIWorkflowItemPresenteCustom;
        #endregion
    }

    [GeneratedCode("Coded UITest Builder", "11.0.50727.1")]
    public class UIWorkflowItemPresenteCustom : WpfCustom
    {

        public UIWorkflowItemPresenteCustom(UITestControl searchLimitContainer) :
            base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.ClassName] = "Uia.WorkflowItemPresenter";
            this.SearchProperties["AutomationId"] = "WorkflowItemPresenter";
            this.WindowTitles.Add(TestBase.GetStudioWindowName());
            #endregion
        }

        #region Properties
        public UIFlowchartCustom UIFlowchartCustom
        {
            get
            {
                if ((this.mUIFlowchartCustom == null))
                {
                    this.mUIFlowchartCustom = new UIFlowchartCustom(this);
                }
                return this.mUIFlowchartCustom;
            }
        }
        #endregion

        #region Fields
        private UIFlowchartCustom mUIFlowchartCustom;
        #endregion
    }

    [GeneratedCode("Coded UITest Builder", "11.0.50727.1")]
    public class UIFlowchartCustom : WpfCustom
    {

        public UIFlowchartCustom(UITestControl searchLimitContainer) :
            base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.ClassName] = "Uia.FlowchartDesigner";
            this.SearchProperties["AutomationId"] = "ServiceDetails(FlowchartDesigner)";
            this.WindowTitles.Add(TestBase.GetStudioWindowName());
            #endregion
        }

        #region Properties
        public WpfCustom UIA3b3b2ece40a4849ba77Custom
        {
            get
            {
                if ((this.mUIA3b3b2ece40a4849ba77Custom == null))
                {
                    this.mUIA3b3b2ece40a4849ba77Custom = new WpfCustom(this);
                    #region Search Criteria
                    this.mUIA3b3b2ece40a4849ba77Custom.SearchProperties[UITestControl.PropertyNames.ClassName] = "Uia.ConnectorWithoutStartDot";
                    this.mUIA3b3b2ece40a4849ba77Custom.SearchProperties["AutomationId"] = "a3b3b2ec-e40a-4849-ba77-5c728e3aed95,828.5,731 828.5,872 685,872";
                    this.mUIA3b3b2ece40a4849ba77Custom.WindowTitles.Add(TestBase.GetStudioWindowName());
                    #endregion
                }
                return this.mUIA3b3b2ece40a4849ba77Custom;
            }
        }
        #endregion

        #region Fields
        private WpfCustom mUIA3b3b2ece40a4849ba77Custom;
        #endregion
    }
}
