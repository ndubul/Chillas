using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;

namespace Dev2.Common.Interfaces
{
    public interface IShellViewModel
    {
        void EditResource(IDbSource selectedSource);

        void EditResource(IPluginSource selectedSource);

        void EditResource(IWebServiceSource selectedSource);

        void EditResource(IDatabaseService selectedSource);

        void EditResource(IEmailServiceSource selectedSource);

        void NewResource(string resourceType);

        string OpenPasteWindow(string current);

        IServer LocalhostServer { get; }
    }
}