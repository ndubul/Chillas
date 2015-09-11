using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Infrastructure;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Controller;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Services.Security;
using Dev2.Studio.Core;

namespace Warewolf.Studio.AntiCorruptionLayer
{
    public class Server : Resource,IServer
    {
        readonly Guid _serverId;
        readonly StudioServerProxy _proxyLayer;
        IList<IToolDescriptor> _tools;

        public Server(Dev2.Studio.Core.Interfaces.IEnvironmentModel environmentModel)
        {
            EnvironmentConnection = environmentModel.Connection;
            EnvironmentID = environmentModel.ID;
            _serverId = EnvironmentConnection.ServerID;
            _proxyLayer = new StudioServerProxy(new CommunicationControllerFactory(), EnvironmentConnection);
            UpdateRepository = new StudioResourceUpdateManager(new CommunicationControllerFactory(), EnvironmentConnection);
            EnvironmentConnection.PermissionsModified += RaisePermissionsModifiedEvent;
            ResourceName = EnvironmentConnection.DisplayName;
            EnvironmentConnection.NetworkStateChanged+=RaiseNetworkStateChangeEvent;
            EnvironmentConnection.ItemAddedMessageAction+=ItemAdded;
        }

        public Guid EnvironmentID { get; set; }
        public Guid? ServerID
        {
            get
            {
                return EnvironmentConnection.ServerID;
            }
        }

        void ItemAdded(IExplorerItem obj)
        {
            if (ItemAddedEvent != null)
            {
                ItemAddedEvent(obj);
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

        void RaiseNetworkStateChangeEvent(object sender, System.Network.NetworkStateEventArgs e)
        {
            if(NetworkStateChanged!= null)
            {
                NetworkStateChanged(new NetworkStateChangedEventArgs(e));
            }
        }

        void RaisePermissionsModifiedEvent(object sender, List<WindowsGroupPermission> windowsGroupPermissions)
        {
            if (PermissionsChanged != null)
            {
                PermissionsChanged(new PermissionsChangedArgs(windowsGroupPermissions.Cast<IWindowsGroupPermission>().ToList()));
            }
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
        public List<IResource> Load()
        {
            return null;
        }

        public async Task<IExplorerItem> LoadExplorer()
        {
            var result = await ProxyLayer.QueryManagerProxy.Load();
            ExplorerItems = result;
            return result;
        }

        public IList<IServer> GetServerConnections()
        {
            var environmentModels = EnvironmentRepository.Instance.ReloadServers();
            return environmentModels.Select(environmentModel => new Server(environmentModel)).Cast<IServer>().ToList();
        }

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

        public void Disconnect()
        {
            EnvironmentConnection.Disconnect();
        }

        public void Edit()
        {
        }

        public List<IWindowsGroupPermission> Permissions { get; set; }

        public event PermissionsChanged PermissionsChanged;
        public event NetworkStateChanged NetworkStateChanged;
        public event ItemAddedEvent ItemAddedEvent;

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
            return EnvironmentConnection.DisplayName;
        }

        #endregion
    }
}