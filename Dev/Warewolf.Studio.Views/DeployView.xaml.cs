using System.Windows;
using System.Windows.Controls;
using Dev2.Common.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for DeployView.xaml
    /// </summary>
    public partial class DeployView : IView, ICheckControlEnabledView
    {
        public DeployView()
        {
            InitializeComponent();
        }

        void TestDefault_OnClick(object sender, RoutedEventArgs e)
        {
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

        #region Implementation of ICheckControlEnabledView

        public bool GetControlEnabled(string controlName)
        {
            return false;
        }

        #endregion
    }
}
