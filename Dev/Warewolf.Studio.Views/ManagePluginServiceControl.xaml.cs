using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ManagePluginServiceControl.xaml
    /// </summary>
    public partial class ManagePluginServiceControl : IView
    {
        public ManagePluginServiceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Attaches events and names to compiled content. 
        /// </summary>
        /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
        public void Connect(int connectionId, object target)
        {
        }
    }
}
