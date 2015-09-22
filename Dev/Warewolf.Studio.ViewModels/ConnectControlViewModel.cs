using System;
using System.Collections.Generic;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Warewolf.Studio.Core;

namespace Warewolf.Studio.ViewModels
{
    public class ConnectControlViewModel : BindableBase, IConnectControlViewModel
    {
        bool _isConnected;
        bool _isConnecing;
        IServer _selectedConnection;

        public ConnectControlViewModel(IServer server,IEventAggregator aggregator)
        {
            if(server == null)
            {
                throw new ArgumentNullException("server");
            }
            if(aggregator == null)
            {
                throw  new ArgumentNullException("aggregator");
            }
            Server = server;
            Servers = new List<IServer>(Server.GetServerConnections());
            SelectedConnection = server;
            aggregator.GetEvent<ServerAddedEvent>().Subscribe(ServerAdded);
            EditConnectionCommand = new DelegateCommand(Edit);
            ToggleConnectionStateCommand = new DelegateCommand(() =>
            {
                if(SelectedConnection == null)
                {
                    return;
                }
                if (SelectedConnection.IsConnected)
                {
                    SelectedConnection.Disconnect();
                }
                else
                {
                    IsConnecting = true;
                    IsConnected = false;
                    SelectedConnection.Connect();
                    IsConnected = true;
                    IsConnecting = false;
                }
            });
        }


        void ServerAdded(IServer server)
        {
            Servers.Add(server);
            OnPropertyChanged(()=>Servers);
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
                OnPropertyChanged(()=>SelectedConnection);
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
                OnPropertyChanged(()=>IsConnecting);
               
            }
        }

        public async void Connect(IServer connection)
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
                SelectedConnection.Edit();
            }
        }

        public string ToggleConnectionToolTip
        {
            get {   return Resources.Languages.Core.ConnectControlToggleConnectionToolTip;}
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
    }

    
}