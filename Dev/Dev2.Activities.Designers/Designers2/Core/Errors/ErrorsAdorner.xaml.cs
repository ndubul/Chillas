
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
// ReSharper disable InconsistentNaming

namespace Dev2.Activities.Designers2.Core.Errors
{
    public partial class ErrorsAdorner
    {
        int _errorsCounter;

        public ErrorsAdorner(FrameworkElement adornedElement)
            : base(adornedElement)
        {
            InitializeComponent();
        }

        void ErrorTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if(textBlock != null)
            {
                textBlock.SetValue(AutomationProperties.AutomationIdProperty, "UI_Error" + _errorsCounter++ + "_AutoID");
            }
        }

        void ErrorsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            _errorsCounter = 0;
        }
    }
}
