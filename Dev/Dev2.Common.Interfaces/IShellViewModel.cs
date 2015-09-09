using System;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.PopupController;
using Dev2.Common.Interfaces.ServerProxyLayer;

namespace Dev2.Common.Interfaces
{
    public interface IShellViewModel
    {
        void EditResource(IDbSource selectedSource, IWorkSurfaceKey key);

        void EditResource(IPluginSource selectedSource, IWorkSurfaceKey key);

        void EditResource(IWebServiceSource selectedSource, IWorkSurfaceKey key);

        void EditResource(IDatabaseService selectedSource, IWorkSurfaceKey key);

        void EditResource(IEmailServiceSource selectedSource, IWorkSurfaceKey key);

        void NewResource(string resourceType);

        string OpenPasteWindow(string current);

        IServer LocalhostServer { get; }

        void OpenResource(Guid resourceId, IServer server);

        void ShowPopup(IPopupMessage getDuplicateMessage);
    }
}