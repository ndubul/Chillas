using System.Activities.Presentation.Model;
using Dev2.Activities.Designers2.Core;
using Dev2.Common;
using Dev2.Data.SystemTemplates;
using Dev2.Data.SystemTemplates.Models;
using Dev2.Interfaces;

namespace Dev2.Activities.Designers2.Switch
{
    public class SwitchDesignerViewModel : ActivityDesignerViewModel
    {
        public SwitchDesignerViewModel(ModelItem mi):base(mi)
        {
            Initialize();
        }

        void Initialize()
        {
            var expressionText = ModelItem.Properties[GlobalConstants.SwitchExpressionTextPropertyText];
            ModelProperty switchCaseValue = ModelItem.Properties["Case"];
            Dev2Switch ds;
            if (expressionText != null && expressionText.Value != null)
            {
                ds = new Dev2Switch();
                var val = Utilities.ActivityHelper.ExtractData(expressionText.Value.ToString());
                if (!string.IsNullOrEmpty(val))
                {
                    ds.SwitchVariable = val;
                }
            }
            else
            {
                ds = DataListConstants.DefaultSwitch;
            }

            var displayName = ModelItem.Properties[GlobalConstants.DisplayNamePropertyText];
            if (displayName != null && displayName.Value != null)
            {
                ds.DisplayText = displayName.Value.ToString();
            }
            if (switchCaseValue != null)
            {
                string val = switchCaseValue.ComputedValue.ToString();
                ds.SwitchExpression = val;
            }
            DisplayText = ds.DisplayText;
            SwitchVariable = ds.SwitchVariable;
            SwitchExpression = ds.SwitchExpression;
        }

        public string SwitchExpression { get; set; }
        public string SwitchVariable { get; set; }
        public string DisplayText { get; set; }

        public Dev2Switch Switch
        {
            get
            {
                var dev2Switch = new Dev2Switch();
                dev2Switch.DisplayText = DisplayText;
                dev2Switch.SwitchVariable = SwitchVariable;
                dev2Switch.SwitchExpression = SwitchExpression;
                return dev2Switch;
            }
        }

        public override void Validate()
        {
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var mainViewModel = CustomContainer.Get<IMainViewModel>();
            if (mainViewModel != null)
            {
                mainViewModel.HelpViewModel.UpdateHelpText(helpText);
            }
        }
    }
}
