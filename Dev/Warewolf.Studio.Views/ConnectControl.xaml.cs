
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Dev2.Common.Interfaces;
using Dev2.ConnectionHelpers;

namespace Warewolf.Studio.Views
{
    /// <summary>
    /// Interaction logic for ConnectControl.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ConnectControl
    {
        public ConnectControl()
        {
            InitializeComponent();
        }

        #region Automation ID's

        // ReSharper disable InconsistentNaming
        public string ServerComboBoxAutomationID
        {
            get { return (string)GetValue(ServerComboBoxAutomationIDProperty); }
            set { SetValue(ServerComboBoxAutomationIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ServerComboBoxAutomationID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServerComboBoxAutomationIDProperty =
            DependencyProperty.Register("ServerComboBoxAutomationID", typeof(string), typeof(ConnectControl), new PropertyMetadata("UI_ServerCbx_AutoID"));


        public string EditButtonAutomationID
        {
            get { return (string)GetValue(EditButtonAutomationIDProperty); }
            set { SetValue(EditButtonAutomationIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectButtonAutomationID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditButtonAutomationIDProperty =
            DependencyProperty.Register("EditButtonAutomationID", typeof(string), typeof(ConnectControl),
                new PropertyMetadata("UI_ServerEditBtn_AutoID"));

        public string ConnectButtonAutomationID
        {
            get { return (string)GetValue(ConnectButtonAutomationIDProperty); }
            set { SetValue(ConnectButtonAutomationIDProperty, value); }
        }
        public IServer SelectedServer
        {
            get
            {
                return (IServer)TheServerComboBox.SelectedItem;
            }
            set
            {
                _selectedServer = value;
            }
        }

        // Using a DependencyProperty as the backing store for ConnectButtonAutomationID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectButtonAutomationIDProperty =
            DependencyProperty.Register("ConnectButtonAutomationID", typeof(string), typeof(ConnectControl),
                new PropertyMetadata("UI_ConnectServerBtn_AutoID"));
        IServer _selectedServer;

        #endregion

        public IConnectControlEnvironment SelectServer(string server)
        {
            foreach(var item in TheServerComboBox.Items)
            {
                var env = item as IConnectControlEnvironment;
                if (env != null && env.DisplayName == server)
                    try
                    {
                        TheServerComboBox.SelectedItem = env;
                        return env;
                    }
                    catch(Exception)
                    {
                        return env;
                       
                    }
                  
            }
            return null;
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
