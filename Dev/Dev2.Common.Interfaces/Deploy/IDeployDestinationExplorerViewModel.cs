namespace Dev2.Common.Interfaces.Deploy
{

    public delegate void ServerSate(object sender, IServer server);
    public interface IDeployDestinationExplorerViewModel:IExplorerViewModel    
    {
        event ServerSate ServerStateChanged;

    }
}