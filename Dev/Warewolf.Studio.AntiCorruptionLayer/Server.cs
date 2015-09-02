using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dev2;
using Dev2.AppResources.Repositories;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Infrastructure;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Controller;
using Dev2.Network;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Services.Security;
using Dev2.Threading;

namespace Warewolf.Studio.AntiCorruptionLayer
{
    public class Server : Resource,IServer
    {
        readonly Guid _serverId;
        readonly StudioServerProxy _proxyLayer;
        IList<IToolDescriptor> _tools;
        //IList<IToolDescriptor> _tools;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Server(string uri,string userName,string password):this(uri,new NetworkCredential(userName,password))
        {
            VerifyArgument.IsNotNull("userName", userName);
            VerifyArgument.IsNotNull("password", password);
        }

        public Server(Uri uri)
            : this(uri.ToString(), CredentialCache.DefaultNetworkCredentials)
        {
       
        }
        
        public Server(string uri,ICredentials credentials)
        {
            VerifyArgument.IsNotNull("uri",uri);
            VerifyArgument.IsNotNull("credentials", credentials);
            EnvironmentConnection = new ServerProxy(uri,credentials,new AsyncWorker());
            _serverId = Guid.NewGuid();
            _proxyLayer = new StudioServerProxy(new CommunicationControllerFactory(), EnvironmentConnection);
            UpdateRepository = new StudioResourceUpdateManager(new CommunicationControllerFactory(), EnvironmentConnection);
            EnvironmentConnection.PermissionsModified += RaisePermissionsModifiedEvent;
           // EnvironmentConnection.NetworkStateChanged += RaiseNetworkStateChangeEvent;
        }

        public Server(Dev2.Studio.Core.Interfaces.IEnvironmentConnection environmentConnection)
        {
            EnvironmentConnection = environmentConnection;
            _serverId = EnvironmentConnection.ServerID;
            _proxyLayer = new StudioServerProxy(new CommunicationControllerFactory(), EnvironmentConnection);
            UpdateRepository = new StudioResourceUpdateManager(new CommunicationControllerFactory(), EnvironmentConnection);
            EnvironmentConnection.PermissionsModified += RaisePermissionsModifiedEvent;
            ResourceName = EnvironmentConnection.DisplayName;
            
        }


        void ItemAdded(IExplorerItem obj)
        {
            RaiseItemAdded(obj);
        }

        void RaiseItemAdded(IExplorerItem explorerItem)
        {
           // if (ItemAddedEvent != null)
            {
            //    ItemAddedEvent(explorerItem);
            }
        }

        public string GetServerVersion()
        {
            if(!EnvironmentConnection.IsConnected)
            {
                EnvironmentConnection.Connect(Guid.Empty);
            }
            return ProxyLayer.AdminManagerProxy.GetServerVersion();
        }

//        void RaiseNetworkStateChangeEvent(object sender, System.Network.NetworkStateEventArgs e)
//        {
//            if(NetworkStateChanged!= null)
//            {
//                NetworkStateChanged(new NetworkStateChangedEventArgs(e));
//            }
//        }
//
        void RaisePermissionsModifiedEvent(object sender, List<WindowsGroupPermission> windowsGroupPermissions)
        {
//            if (PermissionsChanged != null)
//            {
//                PermissionsChanged(new PermissionsChangedArgs(e));
//            }
            Permissions = windowsGroupPermissions.Select(permission => permission as IWindowsGroupPermission).ToList();
        }

        #region Implementation of IServer

        public void Connect()
        {
            if(!EnvironmentConnection.IsConnected)
            {
                EnvironmentConnection.Connect(_serverId);
            }
        }

        //
//        public async Task<bool> Connect()
//        {
//            return await EnvironmentConnection.ConnectAsync(_serverId);
//        }
//
//        public List<IResource> Load()
//        {
//            return null;
//        }
//
        public async Task<IExplorerItem> LoadExplorer()
        {
            var result = await ProxyLayer.QueryManagerProxy.Load();
            ExplorerItems = result;
            return result;
        }
//
//        public IList<IServer> GetServerConnections()
//        {
//            return null;
//        }
//
        public IList<IToolDescriptor> LoadTools()
        {
            return _tools ?? (_tools = ProxyLayer.QueryManagerProxy.FetchTools());
        }

        public IExplorerRepository ExplorerRepository
        {
            get
            {
                return ProxyLayer;
            }
        }

        public IQueryManager QueryProxy
        {
            get
            {
                return _proxyLayer.QueryManagerProxy;
            }
        }
        
        public bool IsConnected()
        {
            return EnvironmentConnection.IsConnected;
        }

        public void ReloadTools()
        {
        }
//
//        public void Disconnect()
//        {
//            EnvironmentConnection.Disconnect();
//        }
//
//        public void Edit()
//        {
//        }

        public List<IWindowsGroupPermission> Permissions { get; set; }

       // public event PermissionsChanged PermissionsChanged;
       // public event NetworkStateChanged NetworkStateChanged;
       // public event ItemAddedEvent ItemAddedEvent;

        public IStudioUpdateManager UpdateRepository { get; private set; }
        public IExplorerItem ExplorerItems { get; set; }
        public StudioServerProxy ProxyLayer
        {
            get
            {
                return _proxyLayer;
            }
        }
        public Dev2.Studio.Core.Interfaces.IEnvironmentConnection EnvironmentConnection { get; private set; }

        #endregion

        #region Overrides of Resource

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return "localhost";
        }

        #endregion
    }
}