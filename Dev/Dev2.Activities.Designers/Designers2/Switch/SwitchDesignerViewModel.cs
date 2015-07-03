using System.Activities.Presentation.Model;
using Dev2.Activities.Designers2.Core;
using Dev2.Common;
using Dev2.Data.SystemTemplates;
using Dev2.Data.SystemTemplates.Models;

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

            DisplayText = ds.DisplayText;
            SwitchVariable = ds.SwitchVariable;
        }

        public string SwitchVariable { get; set; }
        public string DisplayText { get; set; }

        public Dev2Switch Switch
        {
            get
            {
                var dev2Switch = new Dev2Switch();
                dev2Switch.DisplayText = DisplayText;
                dev2Switch.SwitchVariable = SwitchVariable;
                return dev2Switch;
            }
        }

        public override void Validate()
        {
        }
    }
}
