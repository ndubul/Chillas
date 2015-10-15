using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace Dev2.Common.Interfaces.Deploy
{

    public delegate void ServerSate(object sender, IServer server);
    public interface IDeployDestinationExplorerViewModel:IExplorerViewModel    
    {
        event ServerSate ServerStateChanged;

    }
}