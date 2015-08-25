using System;
using System.Collections.Generic;
using System.Data;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.WebServices;

namespace Dev2.Common.Interfaces
{
    public interface IStudioUpdateManager
    {
        void Save(IServerSource serverSource);
        void Save(IDbSource toDbSource);
        void Save(IWebService model);
        void Save(IWebServiceSource model);
        void Save(IDatabaseService toDbSource);
        void Save(IPluginSource source);
        void Save(IEmailServiceSource emailServiceSource);
        void Save(ISharepointServerSource sharePointServiceSource);

        string TestConnection(IServerSource serverSource);
        void TestConnection(IWebServiceSource serverSource);
        void TestConnection(ISharepointServerSource sharePointServiceSource);
        string TestConnection(IEmailServiceSource emailServiceSourceSource);
        IList<string> TestDbConnection(IDbSource serverSource);
        DataTable TestDbService(IDatabaseService inputValues);
        string TestWebService(IWebService inputValues);

        event Action<IWebServiceSource> WebServiceSourceSaved;

        string TestPluginService(IPluginService inputValues);

        void Save(IPluginService toDbSource);
    }
}