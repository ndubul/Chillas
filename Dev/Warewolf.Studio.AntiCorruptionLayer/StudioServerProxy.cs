﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Versioning;
using Dev2.Controller;
using Dev2.Studio.Core.Interfaces;
using Warewolf.Studio.ServerProxyLayer;

namespace Warewolf.Studio.AntiCorruptionLayer
{

    public class StudioServerProxy : IExplorerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public StudioServerProxy(ICommunicationControllerFactory controllerFactory,IEnvironmentConnection environmentConnection)
        {
            if (controllerFactory == null)
            {
                throw new ArgumentNullException("controllerFactory");
            }
            if (environmentConnection == null)
            {
                throw new ArgumentNullException("environmentConnection");
            }
            QueryManagerProxy = new QueryManagerProxy(controllerFactory, environmentConnection);
            UpdateManagerProxy = new ExplorerUpdateManagerProxy(controllerFactory,environmentConnection);
            VersionManager = new VersionManagerProxy(environmentConnection, controllerFactory); //todo:swap
            AdminManagerProxy = new AdminManagerProxy(controllerFactory, environmentConnection); //todo:swap
        }

        public AdminManagerProxy AdminManagerProxy { get; set; }

        public Dev2.Common.Interfaces.ServerProxyLayer.IVersionManager VersionManager { get; set; }
        public IQueryManager QueryManagerProxy { get; set; }
        public ExplorerUpdateManagerProxy UpdateManagerProxy { get; set; }


        public bool Rename(IExplorerItemViewModel vm, string newName)
        {
            UpdateManagerProxy.Rename(vm.ResourceId, newName);
            return true;            
        }

        public bool Move(IExplorerItemViewModel explorerItemViewModel, IExplorerItemViewModel destination)
        {
                UpdateManagerProxy.MoveItem(explorerItemViewModel.ResourceId,destination.ResourceId);
                return true;
        }

        public bool Delete(IExplorerItemViewModel explorerItemViewModel)
        {
            if (explorerItemViewModel.ResourceType == ResourceType.Version)
                VersionManager.DeleteVersion(explorerItemViewModel.ResourceId, explorerItemViewModel.VersionNumber);
            else
                UpdateManagerProxy.DeleteResource(explorerItemViewModel.ResourceId);
            return true;
        }



        public ICollection<IVersionInfo> GetVersions(Guid id)
        {
            return new List<IVersionInfo>(VersionManager.GetVersions(id).Select(a => a.VersionInfo));
        }

        public IRollbackResult Rollback(Guid resourceId, string version)
        {
           return  VersionManager.RollbackTo(resourceId,version);
        }

        public void CreateFolder(string parentPath, string name, Guid id)
        {
            UpdateManagerProxy.AddFolder(parentPath, name, id);
        }
//
//        #endregion
    }
}
