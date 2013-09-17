﻿using System;
using System.Collections.Generic;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Wizards.Interfaces;
using Dev2.Workspaces;
using Unlimited.Framework;

namespace Dev2.Studio.Core.Interfaces
{
    public interface IResourceRepository : IFrameworkRepository<IResourceModel>
    {
        List<IResourceModel> ReloadResource(string resourceName, ResourceType resourceType, IEqualityComparer<IResourceModel> equalityComparer);
        void UpdateWorkspace(IList<IWorkspaceItem> workspaceItems);
        // ReSharper disable once InconsistentNaming
        void Rename(string resourceID, string newName);
        void DeployResource(IResourceModel resource);
        UnlimitedObject DeleteResource(IResourceModel resource);
        bool ResourceExist(IResourceModel resource);
        bool IsReservedService(string resourceName);
        bool IsWorkflow(string resourceName);
        void Add(IResourceModel resource);
        void ForceLoad();
 
        bool IsLoaded { get; set; } // BUG 9276 : TWR : 2013.04.19 - added IsLoaded check to prevent unnecessary loading of resources
        IWizardEngine WizardEngine { get; }
        void RefreshResource(Guid resourceID);
        bool IsInCache(Guid id);
        bool DoesResourceExistInRepo(IResourceModel resource);
        void RemoveFromCache(Guid resourceID);
        void RenameCategory(string oldCategory, string newCategory, ResourceType resourceType);
    }
}
