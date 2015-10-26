﻿/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using System.Windows.Input;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Runtime.ServiceModel.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dev2.Common.Interfaces
{
    public interface IManageDatabaseSourceViewModel
    {
        /// <summary>
        /// The Database Server Type
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        NameValue ServerType { get; set; }
        /// <summary>
        ///  Windows or user or publlic
        /// </summary>
        AuthenticationType AuthenticationType { get; set; }

        /// <summary>
        /// The Database Server Name
        /// </summary>
        IComputerName ServerName { get; set; }

        /// <summary>
        /// The Database that the source is reading from
        /// </summary>
        string DatabaseName { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// Test if connection is successful
        /// </summary>
        ICommand TestCommand { get; set; }

        /// <summary>
        /// Cancel a test that has started
        /// </summary>
        ICommand CancelTestCommand { get; set; }
        /// <summary>
        /// The message that will be set if the test is either successful or not
        /// </summary>
        string TestMessage { get; }

        /// <summary>
        /// Localized text for the Server Type label
        /// </summary>
        string ServerTypeLabel { get; }

        /// <summary>
        /// Localized text for the UserName label
        /// </summary>
        string UserNameLabel { get; }

        /// <summary>
        /// Localized text for the Authentication Type label
        /// </summary>
        string AuthenticationLabel { get; }

        /// <summary>
        /// Localized text for the Password label
        /// </summary>
        string PasswordLabel { get; }

        /// <summary>
        /// Localized text for the Test label
        /// </summary>
        string TestLabel { get; }

        /// <summary>
        /// The localized text for the Database Server label
        /// </summary>
        string ServerLabel { get; }

        /// <summary>
        /// The localized text for the Database label
        /// </summary>
        string DatabaseLabel { get; }

        /// <summary>
        /// Command for save/ok
        /// </summary>
        ICommand OkCommand { get; set; }

        /// <summary>
        /// Header text that is used on the view
        /// </summary>
        string HeaderText { get; set; }

        /// <summary>
        /// Tooltip for the Windows Authentication option
        /// </summary>
        string WindowsAuthenticationToolTip { get; }

        /// <summary>
        /// Tooltip for the User Authentication option
        /// </summary>
        string UserAuthenticationToolTip { get; }

        /// <summary>
        /// Tooltip for the Database Server Type
        /// </summary>
        // ReSharper disable UnusedMember.Global
        string ServerTypeTool { get; }
        // ReSharper restore UnusedMember.Global

        /// <summary>
        /// List of database names for the user to choose from based on the server entered
        /// </summary>
        IList<string> DatabaseNames { get; set; }
        /// <summary>
        /// Cancel test display text
        /// </summary>
        string CancelTestLabel { get; }
        /// <summary>
        /// Has test passed
        /// </summary>
        bool TestPassed { get; set; }

        /// <summary>
        /// has test failed
        /// </summary>
        bool TestFailed { get; set; }
        /// <summary>
        /// IsTesting
        /// </summary>
        bool Testing { get; }
        /// <summary>
        /// Database Types avaialable 
        /// </summary>
        IList<NameValue> Types { get; set; }
        /// <summary>
        /// The name of the resource
        /// </summary>
        // ReSharper disable UnusedMemberInSuper.Global
        string ResourceName { get; set; }
        // ReSharper restore UnusedMemberInSuper.Global

        /// <summary>
        /// The authentications Type
        /// </summary>
        bool UserAuthenticationSelected { get; }


        IList<IComputerName> ComputerNames { get; set; }
    }

    public interface IComputerName
    {
        string Name { get; set; }
    }

    public interface IManageDatabaseSourceModel
    {

        IList<string> GetComputerNames();
        IList<string> TestDbConnection(IDbSource resource);
        void Save(IDbSource toDbSource);
        string ServerName { get; }


    }
}
