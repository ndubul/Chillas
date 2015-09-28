using System;
using System.Collections.Generic;
using System.Windows.Input;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Warewolf.Studio.AntiCorruptionLayer;
using Warewolf.Studio.Core;

namespace Warewolf.Studio.ViewModels
{
    public class ConnectControlViewModel : BindableBase, IConnectControlViewModel
    {
        bool _isConnected;
        bool _isConnecing;
        IServer _selectedConnection;

        public ConnectControlViewModel(IServer server, IEventAggregator aggregator)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            if (aggregator == null)
            {
                throw new ArgumentNullException("aggregator");
            }
            Server = server;
            Servers = new List<IServer>(Server.GetServerConnections());
            SelectedConnection = server;
            aggregator.GetEvent<ServerAddedEvent>().Subscribe(ServerAdded);
            EditConnectionCommand = new DelegateCommand(AllowConnectionEdit);
            ToggleConnectionStateCommand = new DelegateCommand(ConnectOrDisconnect);
        }

        void AllowConnectionEdit()
        {
            if (SelectedConnection == null)
            {
                return;
            }
            if (SelectedConnection.AllowEdit)
            {
                Edit();
            }
        }

        void ConnectOrDisconnect()
        {
            if (SelectedConnection == null)
            {
                return;
            }
            if (SelectedConnection.IsConnected)
            {
                Disconnect(SelectedConnection);
                IsConnected = false;
            }
            else
            {
                IsConnecting = true;
                IsConnected = false;
                Connect(SelectedConnection);
                IsConnected = true;
                IsConnecting = false;
            }
        }

        void ServerAdded(IServer server)
        {
            Servers.Add(server);
            OnPropertyChanged(() => Servers);
        }

        public IServer Server { get; set; }
        public IList<IServer> Servers { get; set; }
        public IServer SelectedConnection
        {
            get
            {
                return _selectedConnection;
            }
            set
            {
                _selectedConnection = value;
                OnPropertyChanged(() => SelectedConnection);
            }
        }
        public ICommand EditConnectionCommand { get; set; }
        public ICommand ToggleConnectionStateCommand { get; set; }
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                OnPropertyChanged(() => IsConnected);
            }
        }
        public bool IsConnecting
        {
            get
            {
                return _isConnecing;
            }
            set
            {
                _isConnecing = value;
                OnPropertyChanged(() => IsConnecting);
            }
        }

        public void Connect(IServer connection)
        {
            if (connection != null)
            {
                connection.Connect();

                if (ServerConnected != null)
                {
                    ServerConnected(this, connection);
                }
            }
        }

        public void Disconnect(IServer connection)
        {
            if (connection != null)
            {
                connection.Disconnect();
                if (ServerDisconnected != null)
                {
                    ServerDisconnected(this, connection);
                }
            }
        }

        public void Refresh()
        {
            if (SelectedConnection != null)
            {
                SelectedConnection.Load();
            }
        }

        public void Edit()
        {
            if (SelectedConnection != null)
            {
                var server = SelectedConnection as Server;
                var mainViewModel = CustomContainer.Get<IMainViewModel>();
                if (server != null)
                {
                    var environmentConnection = server.EnvironmentConnection;
                    mainViewModel.EditServer(new ServerSource
                    {
                        Address = environmentConnection.AppServerUri.ToString(),
                        ID = environmentConnection.ID,
                        AuthenticationType = environmentConnection.AuthenticationType,
                        UserName = environmentConnection.UserName,
                        Password = environmentConnection.Password,
                        Name = server.ResourceName,
                        ResourcePath = server.ResourcePath
                    });
                }
            }
        }

        public string ToggleConnectionToolTip
        {
            get { return Resources.Languages.Core.ConnectControlToggleConnectionToolTip; }
        }
        public string EditConnectionToolTip
        {
            get { return Resources.Languages.Core.ConnectControlEditConnectionToolTip; }
        }
        public string ConnectionsToolTip
        {
            get { return Resources.Languages.Core.ConnectControlConnectionsToolTip; }
        }
        public EventHandler<IServer> ServerConnected { get; set; }
        public EventHandler<IServer> ServerDisconnected { get; set; }
    }


}