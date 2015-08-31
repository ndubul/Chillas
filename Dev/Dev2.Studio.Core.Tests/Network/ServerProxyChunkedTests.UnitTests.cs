using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Security;
using Dev2.Communication;
using Dev2.Core.Tests.Utils;
using Dev2.Explorer;
using Dev2.Network;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.SignalR.Wrappers;
using Dev2.SignalR.Wrappers.New;
using Dev2.Threading;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev2.Core.Tests.Network
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public partial class ServerProxyChunkedTests
    {
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ServerProxy_Constructor")]
        public void ServerProxy_Constructor_DefaultConstruction_ShouldHaveEsbProxy()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------
            var serverProxy = new TestServerProxyWithChunking();
            //------------Assert Results-------------------------
            Assert.IsNotNull(serverProxy);
            Assert.IsNotNull(serverProxy.HubConnection);
            Assert.IsNotNull(serverProxy.EsbProxy);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ServerProxy_Constructor")]
        public void ServerProxy_Constructor_ParameterUserNamePasswordWebserverURI_ShouldHaveEsbProxy()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------
            var serverProxy = new TestServerProxyWithChunking("http://localhost:8080", "some user", "some password");
            //------------Assert Results-------------------------
            Assert.IsNotNull(serverProxy);
            Assert.IsNotNull(serverProxy.HubConnection);
            Assert.IsNotNull(serverProxy.EsbProxy);
            Assert.AreEqual("some user", serverProxy.UserName);
            Assert.AreEqual("some password", serverProxy.Password);
            Assert.AreEqual(AuthenticationType.User, serverProxy.AuthenticationType);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ServerProxy_ExecuteCommand")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ServerProxy_ExecuteCommand_WhenNullPayload_ExceptionThrown()
        {
            //------------Setup for test--------------------------
            var serverProxy = new TestServerProxyWithChunking();
            //------------Execute Test---------------------------
            serverProxy.ExecuteCommand(null, Guid.NewGuid());
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ServerProxy_Constructor")]
        public void ServerProxy_Constructor_DefaultConstruction_ShouldHaveEsbProxyWithSendMemoSubscription()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------
            var serverProxy = new TestServerProxyWithChunking();
            //------------Assert Results-------------------------
            var subscription = serverProxy.EsbProxy.Subscribe("SendMemo");
            Assert.IsNotNull(subscription);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ServerProxy_StateChange")]
        public void ServerProxy_StateChange_FromConnectedToReconnecting_IsAuthorizedFalse()
        {
            //------------Setup for test--------------------------
            bool _permissionsChangedFired = false;

            var serverProxy = new TestServerProxyWithChunking();
            serverProxy.PermissionsChanged += (sender, args) =>
            {
                _permissionsChangedFired = true;
            };
            bool authorisedBeforeStateChange = serverProxy.IsAuthorized;
            //------------Execute Test---------------------------
            serverProxy.CallHubConnectionChanged(new StateChangeWrapped(ConnectionStateWrapped.Connected, ConnectionStateWrapped.Reconnecting));
            //------------Assert Results-------------------------
            Assert.IsTrue(authorisedBeforeStateChange);
            Assert.IsFalse(serverProxy.IsAuthorized);
            Assert.IsTrue(_permissionsChangedFired);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ServerProxy_Constructor")]
        public void ServerProxy_Constructor_DefaultConstruction_ShouldHaveEsbProxyWithSendDebugStateSubscription()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------
            var serverProxy = new TestServerProxyWithChunking();
            //------------Assert Results-------------------------
            var subscription = serverProxy.EsbProxy.Subscribe("SendDebugState");
            Assert.IsNotNull(subscription);
        }

        [TestMethod, Timeout(5000)]
        [Owner("Hagashen Naidu")]
        [TestCategory("ServerProxy_Constructor")]
        public void ServerProxy_HandleItemAdded()
        {
            //------------Setup for test--------------------------
            var serverProxy = new ServerProxyWithoutChunking(new Uri("http://bob"));
            var serverGuid = Guid.NewGuid();
            var ItemGuid = Guid.Empty;
            try
            {
                serverProxy.Connect(serverGuid);
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {


            }
            //------------Execute Test---------------------------
            ServerExplorerItem item = new ServerExplorerItem("bob",Guid.Empty,ResourceType.DbService,null,Permissions.Administrator, "bob","","");
            serverProxy.ItemAddedMessageAction += explorerItem => { ItemGuid = explorerItem.ServerId; };
            Dev2JsonSerializer dev = new Dev2JsonSerializer();
            var output = dev.SerializeToBuilder(item);
            PrivateObject p = new PrivateObject(serverProxy);
            p.Invoke("OnItemAddedMessageReceived", output.ToString());
            Assert.AreNotEqual(ItemGuid,serverGuid);
            //------------Assert Results-------------------------
            var subscription = serverProxy.EsbProxy.Subscribe("SendDebugState");
            Assert.IsNotNull(subscription);
        }

        [TestMethod, Timeout(5000)]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("ServerProxy_Connect")]
        public void ServerProxy_ConnectSetsId()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------
            var serverProxy = new TestServerProxy();
            var x = Guid.NewGuid();
            try
            {
                serverProxy.Connect(x);
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {
                
             
            }
            
            //------------Assert Results-------------------------
            Assert.AreEqual(x,serverProxy.ID);
        }

        [TestMethod, Timeout(3000)]
        [Owner("Trevor Williams-Ros")]
        [TestCategory("ServerProxy_Wait")]
        public void ServerProxy_Wait_TaskThrowsHttpRequestException_ExceptionHandledAndTaskIsFaultedAndIsConnectedIsFalse()
        {
            //------------Setup for test--------------------------
            const string ExMessage = "Server unavailable.";
            var result = new StringBuilder();
            var task = new Task<string>(() =>
            {
                throw new HttpRequestException(ExMessage);
            });


            var serverProxy = new TestServerProxy
            {
                IsConnected = true
            };

            //------------Execute Test---------------------------
            serverProxy.TestWait(task, result);

            //------------Assert Results-------------------------
            StringAssert.Contains(result.ToString(), ExMessage);
            Assert.IsTrue(task.IsFaulted);
            Assert.IsFalse(serverProxy.IsConnected);
        }

        [TestMethod, Timeout(3000)]
        [Owner("Travis Frisinger")]
        [TestCategory("ServerProxy_Wait")]
        public void ServerProxy_Wait_TaskThrowsHttpClientException_ExceptionHandledAndTaskIsFaultedAndIsConnectedIsTrue()
        {
            //------------Setup for test--------------------------
            const string ExMessage = "StatusCode: 403";
            var result = new StringBuilder();
            var task = new Task<string>(() =>
            {
                throw new HttpClientException(ExMessage);
            });


            var serverProxy = new TestServerProxy
            {
                IsConnected = true
            };

            //------------Execute Test---------------------------
            serverProxy.TestWait(task, result);

            //------------Assert Results-------------------------
            StringAssert.Contains(result.ToString(), ExMessage);
            Assert.IsTrue(task.IsFaulted);
            Assert.IsTrue(serverProxy.IsConnected);
        }

        [TestMethod, Timeout(3000)]
        [Owner("Travis Frisinger")]
        [TestCategory("ServerProxy_Wait")]
        public void ServerProxy_Wait_TaskThrowsException_ExceptionHandledAndTaskIsFaultedAndIsConnectedIsTrue()
        {
            //------------Setup for test--------------------------
            const string ExMessage = "Unknown Error Occurred";
            var result = new StringBuilder();
            var task = new Task<string>(() =>
            {
                throw new Exception(ExMessage);
            });


            var serverProxy = new TestServerProxy
            {
                IsConnected = true
            };

            //------------Execute Test---------------------------
            serverProxy.TestWait(task, result);

            //------------Assert Results-------------------------
            StringAssert.Contains(result.ToString(), ExMessage);
            Assert.IsTrue(task.IsFaulted);
            Assert.IsTrue(serverProxy.IsConnected);
        }

        [TestMethod, Timeout(3000)]
        [Owner("Trevor Williams-Ros")]
        [TestCategory("ServerProxy_Wait")]
        public void ServerProxy_Wait_TaskThrowsReconnectingBeforeInvocationInvalidOperationException_ExceptionHandledAndTaskIsFaultedAndIsConnectedIsTrue()
        {
            //------------Setup for test--------------------------
            const string ExMessage = "Connection started reconnecting before invocation result was received";
            var result = new StringBuilder();
            var task = new Task<string>(() =>
            {
                throw new InvalidOperationException(ExMessage);
            });

            var serverProxy = new TestServerProxy
            {
                IsConnected = true
            };

            //------------Execute Test---------------------------
            serverProxy.TestWait(task, result);

            //------------Assert Results-------------------------
            StringAssert.Contains(result.ToString(), ExMessage);
            Assert.IsTrue(task.IsFaulted);
            Assert.IsTrue(serverProxy.IsConnected);
        }

    }

    internal class TestServerProxyWithChunking : ServerProxyWithChunking
    {
        // TODO: Move this constructor to a test class!!
        public TestServerProxyWithChunking(string uri, string userName, string password)
            : base(uri, userName, password)
        {
        }
        public TestServerProxyWithChunking()
            : base("http://localhost:8080", CredentialCache.DefaultCredentials, new SynchronousAsyncWorker())
        {

        }

        public void CallHubConnectionChanged(IStateChangeWrapped stateChange)
        {
            HubConnectionStateChanged(stateChange);
        }

        public void SetEsbProxy(IHubProxyWrapper hubProxy)
        {
            EsbProxy = hubProxy;
        }

        public T TestWait<T>(Task<T> task, StringBuilder result)
        {
            task.Start();
            return Wait(task, result, 10);
        }

        #region Overrides of ServerProxy

        protected override T Wait<T>(Task<T> task, StringBuilder result)
        {
            task.Start();
            return task.Result;
        }

        protected override void Wait(Task task)
        {
            task.Start();
        }

        #endregion
    }

    internal class TestHubConnectionChunked : HubConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection"/> class.
        /// </summary>
        /// <param name="url">The url to connect to.</param>
        public TestHubConnectionChunked(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection"/> class.
        /// </summary>
        /// <param name="url">The url to connect to.</param><param name="useDefaultUrl">Determines if the default "/signalr" path should be appended to the specified url.</param>
        public TestHubConnectionChunked(string url, bool useDefaultUrl)
            : base(url, useDefaultUrl)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection"/> class.
        /// </summary>
        /// <param name="url">The url to connect to.</param><param name="queryString">The query string data to pass to the server.</param>
        public TestHubConnectionChunked(string url, string queryString)
            : base(url, queryString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection"/> class.
        /// </summary>
        /// <param name="url">The url to connect to.</param><param name="queryString">The query string data to pass to the server.</param><param name="useDefaultUrl">Determines if the default "/signalr" path should be appended to the specified url.</param>
        public TestHubConnectionChunked(string url, string queryString, bool useDefaultUrl)
            : base(url, queryString, useDefaultUrl)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection"/> class.
        /// </summary>
        /// <param name="url">The url to connect to.</param><param name="queryString">The query string data to pass to the server.</param>
        public TestHubConnectionChunked(string url, IDictionary<string, string> queryString)
            : base(url, queryString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection"/> class.
        /// </summary>
        /// <param name="url">The url to connect to.</param><param name="queryString">The query string data to pass to the server.</param><param name="useDefaultUrl">Determines if the default "/signalr" path should be appended to the specified url.</param>
        public TestHubConnectionChunked(string url, IDictionary<string, string> queryString, bool useDefaultUrl)
            : base(url, queryString, useDefaultUrl)
        {
        }

        #region Overrides of HubConnection

        #endregion
    }


}