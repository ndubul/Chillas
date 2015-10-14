
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Interfaces;
using Microsoft.Practices.Prism;
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
        bool _allowConnection;
        ObservableCollection<IServer> _servers;

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
            LoadServers();

            SelectedConnection = server;
            var evt = aggregator.GetEvent<ServerAddedEvent>();
            if(evt != null)
            {
                evt.Subscribe(ServerAdded);
            }
            EditConnectionCommand = new DelegateCommand(AllowConnectionEdit);
            ToggleConnectionStateCommand = new DelegateCommand(ConnectOrDisconnect);
            if(Server.UpdateRepository != null)
            {
                Server.UpdateRepository.ServerSaved += UpdateRepositoryOnServerSaved;
            }
        }

        void UpdateRepositoryOnServerSaved()
        {
            LoadServers();
        }

        public void LoadServers()
        {
            var serverConnections = Server.GetServerConnections();
            var servers = new ObservableCollection<IServer> { CreateNewRemoteServerEnvironment() };
            servers.AddRange(serverConnections);
            if (Servers == null)
            {
                Servers = new ObservableCollection<IServer>();
            }
            var x = servers.Where(a => !Servers.Select(q => q.EnvironmentID).Contains(a.EnvironmentID));
            Servers.Clear();
            Servers.AddRange(x);
        }

        public IServer DefaultSelectedConnection { get; set; }

        IServer CreateNewRemoteServerEnvironment()
        {
            return new Server
            {
                ResourceName = Resources.Languages.Core.NewServerLabel,
                EnvironmentID = Guid.NewGuid()
            };
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

        public async void ConnectOrDisconnect()
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
                await Connect(SelectedConnection);
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
        public ObservableCollection<IServer> Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                _servers = value;
                OnPropertyChanged(() => Servers);
            }
        }
        public IServer SelectedConnection
        {
            get
            {
                return _selectedConnection;
            }
            set
            {
                if (value != null)
                {
                    var mainViewModel = CustomContainer.Get<IShellViewModel>();
                    if (value.ResourceName != null && value.ResourceName.Equals(Resources.Languages.Core.NewServerLabel))
                    {
                        if (mainViewModel != null)
                        {
                            mainViewModel.SetActiveEnvironment(_selectedConnection.EnvironmentID);
                            mainViewModel.NewResource("ServerSource", "");
                        }
                        IsConnected = false;
                        AllowConnection = false;

                    }
                    else
                    {
                        _selectedConnection = value;
                        AllowConnection = true;
                        if (_selectedConnection.ResourceName.Equals(Resources.Languages.Core.LocalhostLabel))
                        {
                            AllowConnection = false;
                        }
                        IsConnected = _selectedConnection.IsConnected;
                    }
                    if (mainViewModel != null)
                    {
                        if (_selectedConnection.IsConnected && !_selectedConnection.ResourceName.Equals(Resources.Languages.Core.NewServerLabel))
                        {
                            mainViewModel.SetActiveEnvironment(_selectedConnection.EnvironmentID);
                            mainViewModel.SetActiveServer(_selectedConnection);
                        }
                    }
                    OnPropertyChanged(() => SelectedConnection);
                if(SelectedEnvironmentChanged!=null)
                {
                    SelectedEnvironmentChanged(this,value.EnvironmentID);
                }
                }
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
        public bool AllowConnection
        {
            get
            {
                return _allowConnection;
            }
            set
            {
                _allowConnection = value;
                OnPropertyChanged(() => AllowConnection);
            }
        }

        public async Task<bool> Connect(IServer connection)
        {
            if (connection != null)
            {
                try
                {
                    var connected = await connection.ConnectAsync();
                    if (connected)
                    {
                        var mainViewModel = CustomContainer.Get<IShellViewModel>();
                        mainViewModel.SetActiveEnvironment(connection.EnvironmentID);
                    }
                    OnPropertyChanged(() => connection.IsConnected);
                    if (ServerConnected != null)
                    {
                        ServerConnected(this, connection);
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void Disconnect(IServer connection)
        {
            if (connection != null)
            {
                connection.Disconnect();
                OnPropertyChanged(() => connection.IsConnected);
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
                        ServerName = environmentConnection.WebServerUri.Host,
                        Name = server.ResourceName,
                        ResourcePath = server.ResourcePath
                    });
                }
            }
        }

        public event SelectedServerChanged SelectedEnvironmentChanged;

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