﻿
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.WebServices;

namespace Dev2.Common.Interfaces.WebService
{
    public interface IWebServiceModel
    {

        ICollection<IWebServiceSource> RetrieveSources();
        void CreateNewSource();
        void EditSource(IWebServiceSource selectedSource, IWorkSurfaceKey resourceModel);
        string TestService(IWebService inputValues);
        void SaveService(IWebService toModel);

        IStudioUpdateManager UpdateRepository { get; }
        IQueryManager QueryProxy { get; }
        ObservableCollection<IWebServiceSource> Sources { get; }

        string HandlePasteResponse(string current);
    }
}
