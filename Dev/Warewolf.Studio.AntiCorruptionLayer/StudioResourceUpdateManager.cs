﻿using System;
using System.Collections.Generic;
using System.Data;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.WebServices;
using Dev2.Controller;
using Dev2.Studio.Core.Interfaces;
using Warewolf.Studio.ServerProxyLayer;

namespace Warewolf.Studio.AntiCorruptionLayer
{
    public class StudioResourceUpdateManager : IStudioUpdateManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="controllerFactory"/> is <see langword="null" />.</exception>
        public StudioResourceUpdateManager(ICommunicationControllerFactory controllerFactory, IEnvironmentConnection environmentConnection)
        {
            if (controllerFactory == null)
            {
                throw new ArgumentNullException("controllerFactory");
            }
            if (environmentConnection == null)
            {
                throw new ArgumentNullException("environmentConnection");
            }

             UpdateManagerProxy = new UpdateProxy(controllerFactory, environmentConnection);

        }

        IUpdateManager UpdateManagerProxy { get; set; }

        public void Save(IServerSource serverSource)
        {

            UpdateManagerProxy.SaveServerSource(serverSource, GlobalConstants.ServerWorkspaceID);
        }

        public void Save(IPluginSource source)
        {
            UpdateManagerProxy.SavePluginSource(source, GlobalConstants.ServerWorkspaceID);
        }

        public void Save(IEmailServiceSource emailServiceSource)
        {
            UpdateManagerProxy.SaveEmailServiceSource(emailServiceSource, GlobalConstants.ServerWorkspaceID);
        }

        public void TestConnection(IServerSource serverSource)
        {
            UpdateManagerProxy.TestConnection(serverSource);
        }

        public string TestConnection(IEmailServiceSource emailServiceSource)
        {
            return UpdateManagerProxy.TestEmailServiceSource(emailServiceSource);
        }

        public void TestConnection(IWebServiceSource resource)
        {
            UpdateManagerProxy.TestConnection(resource);
        }

        public void TestConnection(ISharepointServerSource resource)
        {
            UpdateManagerProxy.TestConnection(resource);
        }

        public IList<string> TestDbConnection(IDbSource serverSource)
        {
            return UpdateManagerProxy.TestDbConnection(serverSource);
        }

        public void Save(IDbSource toDbSource)
        {
            UpdateManagerProxy.SaveDbSource(toDbSource, GlobalConstants.ServerWorkspaceID);

        }

        public void Save(IWebService model)
        {
            UpdateManagerProxy.SaveWebservice(model, GlobalConstants.ServerWorkspaceID);
        }

        public void Save(IWebServiceSource resource)
        {
            try
            {
                UpdateManagerProxy.SaveWebserviceSource(resource, GlobalConstants.ServerWorkspaceID);
                if (WebServiceSourceSaved != null)
                {
                    WebServiceSourceSaved(resource);
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public void Save(ISharepointServerSource resource)
        {
            try
            {
                UpdateManagerProxy.SaveSharePointServiceSource(resource, GlobalConstants.ServerWorkspaceID);
                if (SharePointServiceSourceSaved != null)
                {
                    SharePointServiceSourceSaved(resource);
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public void Save(IDatabaseService toDbSource)
        {
            UpdateManagerProxy.SaveDbService(toDbSource);

        }

        public DataTable TestDbService(IDatabaseService inputValues)
        {
            return UpdateManagerProxy.TestDbService(inputValues);
        }

        public string TestWebService(IWebService inputValues)
        {
            return UpdateManagerProxy.TestWebService(inputValues);
        }

        public event Action<IWebServiceSource> WebServiceSourceSaved;

        public event Action<ISharepointServerSource> SharePointServiceSourceSaved;

        public string TestPluginService(IPluginService inputValues)
        {
            return UpdateManagerProxy.TestPluginService(inputValues);
        }

        public void Save(IPluginService toDbSource)
        {
            UpdateManagerProxy.SavePluginService(toDbSource);
        }
    }
}