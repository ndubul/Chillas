﻿/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using Dev2.Common.Interfaces;

namespace Warewolf.Studio.ViewModels
{
    public class ManageNewServerSourceModel: IManageServerSourceModel
    {
        readonly IStudioUpdateManager _updateRepository;

        public ManageNewServerSourceModel(IStudioUpdateManager updateRepository, string serverName)
        {
            _updateRepository = updateRepository;

            ServerName = serverName;
            if (ServerName.Contains("("))
            {
                ServerName = serverName.Substring(0, serverName.IndexOf("(", StringComparison.Ordinal));
            }

        }

        #region Implementation of IManageServerSourceModel

        public void TestConnection(IServerSource resource)
        {
            _updateRepository.TestConnection(resource);
        }

        public void Save(IServerSource resource)
        {
            _updateRepository.Save(resource);
        }

        public string ServerName { get; set; }

        #endregion
    }
}
