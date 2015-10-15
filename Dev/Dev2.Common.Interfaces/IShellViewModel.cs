using System;
using System.Collections.Generic;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.PopupController;
using Dev2.Common.Interfaces.ServerProxyLayer;

namespace Dev2.Common.Interfaces
{
    public interface IShellViewModel
    {
        void EditResource(IDbSource selectedSource, IWorkSurfaceKey key = null);

        void EditResource(IPluginSource selectedSource, IWorkSurfaceKey key = null);

        void EditResource(IWebServiceSource selectedSource, IWorkSurfaceKey key = null);

        void EditResource(IDatabaseService selectedSource, IWorkSurfaceKey key = null);

        void EditResource(IEmailServiceSource selectedSource, IWorkSurfaceKey key = null);

        void NewResource(string resourceType,string resourcePath);

        string OpenPasteWindow(string current);

        IServer LocalhostServer { get; }

        void OpenResource(Guid resourceId, IServer server);

        void ShowPopup(IPopupMessage getDuplicateMessage);

        void SetActiveEnvironment(Guid environmentID);

        void SetActiveServer(IServer server);

        void Debug();

        void ShowAboutBox();

        void ShowDependencies(Guid resourceId, IServer server);

        void DeployResources(Guid sourceEnvironmentId,Guid destinationEnvironmentId,IList<Guid> resources  );

        void AddDeploySurface(IEnumerable<IExplorerTreeItem> items );
    }
}