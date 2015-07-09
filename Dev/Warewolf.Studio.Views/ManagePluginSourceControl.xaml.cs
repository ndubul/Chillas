using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Dev2.Common.Interfaces;
using Infragistics.Controls.Menus;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.Core;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ManagePluginSourceControl.xaml
    /// </summary>
    public partial class ManagePluginSourceControl : IView
    {
        public ManagePluginSourceControl()
        {
            InitializeComponent();
        }

        public string GetHeaderText()
        {
            BindingExpression be = HeaderTextBlock.GetBindingExpression(TextBlock.TextProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return HeaderTextBlock.Text;
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

        public IDllListingModel SelectItem(string itemName)
        {
            var xamDataTreeNode = ExplorerTree.Nodes.FirstOrDefault(node =>
            {
                var item = node.Data as IDllListingModel;
                if (item != null)
                {
                    if (item.Name.ToLowerInvariant().Contains(itemName.ToLowerInvariant()))
                    {
                        return true;
                    }
                }
                return false;
            });
            if (xamDataTreeNode != null)
            {
                xamDataTreeNode.IsExpanded = true;
            }
            return xamDataTreeNode == null ? null : xamDataTreeNode.Data as IDllListingModel;
        }

        public bool IsItemVisible(string itemName)
        {
            var xamDataTreeNode = ExplorerTree.ActiveNode.Manager.Nodes.FirstOrDefault(node =>
            {
                var item = node.Data as IDllListingModel;
                if (item != null)
                {
                    if (item.Name.ToLowerInvariant().Contains(itemName.ToLowerInvariant()))
                    {
                        return true;
                    }
                }
                return false;
            });
            return xamDataTreeNode != null;
        }

        public string GetAssemblyName()
        {
            BindingExpression be = AssemblyNameTextBox.GetBindingExpression(TextBlock.TextProperty);
            if (be != null)
            {
                be.UpdateTarget();
            }
            return AssemblyNameTextBox.Text;
        }

        public void PerformSave()
        {
            SaveButton.Command.Execute(null);
        }

        public void SetAssemblyName(string assemblyName)
        {
            AssemblyNameTextBox.Text = assemblyName;
            BindingExpression be = AssemblyNameTextBox.GetBindingExpression(TextBlock.TextProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }
    }
}
