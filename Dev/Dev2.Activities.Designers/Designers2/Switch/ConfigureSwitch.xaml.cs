using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Mvvm;

namespace Dev2.Activities.Designers2.Switch
{
    /// <summary>
    /// Interaction logic for ConfigureSwitch.xaml
    /// </summary>
    public partial class ConfigureSwitch : UserControl, IView
    {
        public ConfigureSwitch()
        {
            InitializeComponent();
        }

        public void CheckSwitchVariableState(string state)
        {
            if(VariabletoSwitchon.IsEnabled != (state.ToLower()=="enabled"))
            {
                throw new Exception("State is not"+state);
            }

        }

        public void CheckDisplayState(string state)
        {
            if (DisplayText.IsEnabled != (state.ToLower() == "enabled"))
            {
                throw new Exception("State is not" + state);
            }

        }

        public void SetVariableToSwitchOn(string state)
        {
            VariabletoSwitchon.Text = state;

        }

        public string GetDisplayName()
        {
            return DisplayText.Text;
        }
    }
}
