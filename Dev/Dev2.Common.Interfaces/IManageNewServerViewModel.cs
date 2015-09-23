﻿using System.Collections.Generic;
using System.Windows.Input;
using Dev2.Runtime.ServiceModel.Data;

namespace Dev2.Common.Interfaces
{

    public interface IManageNewServerViewModel
    {
        /// <summary>
        /// The server address that we are trying to connect to
        /// </summary>
        string Address { get; set; }
        /// <summary>
        ///  Windows or user or publlic
        /// </summary>
        AuthenticationType AuthenticationType { get; set; }
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
        /// The message that will be set if the test is either successful or not
        /// </summary>
        string TestMessage { get; }

        string AddressLabel { get; }

        string UserNameLabel { get; }

        string AuthenticationLabel { get; }

        string PasswordLabel { get; }

        string TestLabel { get; }

        /// <summary>
        /// Command for save/ok
        /// </summary>
        ICommand OkCommand { get; set; }

        /// <summary>
        /// Header text that is used on the view
        /// </summary>
        string HeaderText { get; set; }
        IList<IComputerName> ComputerNames { get; set; }
        IComputerName ServerName { get; set; }
    }
}