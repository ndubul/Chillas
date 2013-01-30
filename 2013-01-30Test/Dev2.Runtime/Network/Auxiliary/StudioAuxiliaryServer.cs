﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Network;

namespace Dev2.DynamicServices.Network.Auxiliary
{
    public sealed class StudioAuxiliaryServer : TCPServer<StudioAuxiliarySession>
    {
        private StudioNetworkServer _owner;
        private StudioAuxiliaryAccountProvider _accountProvider;

        public StudioAuxiliaryAccountProvider AccountProvider { get { return _accountProvider; } }

        public StudioAuxiliaryServer(StudioNetworkServer primaryServer)
            : base(primaryServer.Name + " (Auxiliary)", new StudioAuxiliaryInboundAuthenticationBroker())
        {
            _owner = primaryServer;
            ((StudioAuxiliaryInboundAuthenticationBroker)_authenticationBroker).Server = this;
            _accountProvider = new StudioAuxiliaryAccountProvider(null, this);
        }

        protected override void OnExecuteCommand(StudioAuxiliarySession context, ByteBuffer payload, Packet writer)
        {
            throw new NotSupportedException();
        }

        protected override string OnExecuteCommand(StudioAuxiliarySession context, string payload, Guid dataListID)
        {
            return _owner.Channel.ExecuteCommand(payload, Workspaces.WorkspaceRepository.Instance.ServerWorkspace, dataListID);
        }

        private sealed class StudioAuxiliaryInboundAuthenticationBroker : InboundSRPAuthenticationBroker
        {
            private StudioAuxiliaryServer _server;

            public StudioAuxiliaryServer Server { get { return _server; } set { _server = value; } }

            public StudioAuxiliaryInboundAuthenticationBroker()
            {
                _localIdentifier = new FourOctetUnion('D', 'E', 'V', '2').Int32;
                _localVersion = new Version(1, 0, 0, 0);
            }

            protected override InboundAuthenticationBroker OnInstantiate()
            {
                return new StudioAuxiliaryInboundAuthenticationBroker() { Server = _server };
            }

            protected override NetworkAccount ResolveAccount(string account)
            {
                return _server._accountProvider.GetAccount(account);
            }
        }
    }
}
