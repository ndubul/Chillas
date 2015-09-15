using System.Activities.Presentation.Model;
using System.Windows;
using Dev2.Activities.Designers2.Core;
using Dev2.Activities.Designers2.Decision;
using Dev2.Common;
using Dev2.Data.SystemTemplates;
using Dev2.Data.SystemTemplates.Models;
using Dev2.Interfaces;

namespace Dev2.Activities.Designers2.Switch
{
    public class SwitchDesignerViewModel : ActivityDesignerViewModel
    {
        string _switchExpression;
        string _switchVariable;

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
          
            SwitchVariable = ds.SwitchVariable;
            SwitchExpression = ds.SwitchExpression;
            DisplayText = ds.DisplayText;
        }

        public string SwitchExpression
        {
            get
            {
                return _switchExpression;
            }
            set
            {
                _switchExpression = value;
            }
        }
        public string SwitchVariable
        {
            get
            {
                return _switchVariable;
            }
            set
            {
                _switchVariable = value;
                if(string.IsNullOrEmpty(value))
                {
                    DisplayText = "Switch";
                    DisplayName = "Switch";
                }
                else
                {
                    DisplayText = value;
                    DisplayName = value;
                }
            }
        }
        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(SwitchDesignerViewModel), new PropertyMetadata(default(string)));

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

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
