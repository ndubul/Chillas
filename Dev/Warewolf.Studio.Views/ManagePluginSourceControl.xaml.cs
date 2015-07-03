using System.Windows.Controls;
using Infragistics.Controls.Menus;
using Warewolf.Studio.Core;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ManagePluginSourceControl.xaml
    /// </summary>
    public partial class ManagePluginSourceControl : UserControl
    {
        public ManagePluginSourceControl()
        {
            InitializeComponent();
        }

        private void ExplorerTree_OnNodeExpansionChanging(object sender, CancellableNodeExpansionChangedEventArgs e)
        {
            if (DataContext != null)
            {
                var node = e.Node.Data as DllListingModel;
                if (node != null)
                {
                    node.IsExpanded = e.Node.IsExpanded;
                    node.ExpandingCommand.Execute(null);
                }
            }
        }

        #region Implementation of IComponentConnector

        /// <summary>
        /// Attaches events and names to compiled content. 
        /// </summary>
        /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
        public void Connect(int connectionId, object target)
        {
        }

        #endregion
    }
}
