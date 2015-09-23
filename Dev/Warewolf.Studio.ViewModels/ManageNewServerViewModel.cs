using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.Threading;
using Dev2.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Warewolf.Studio.ViewModels
{
    public sealed class ManageNewServerViewModel : SourceBaseImpl<IServerSource>, IManageNewServerViewModel
    {
        string _userName;
        string _password;
        string _testMessage;
        string _address;
        bool _testPassed;
        AuthenticationType _authenticationType;

        #region Implementation of IInnerDialogueTemplate

        public IAsyncWorker AsyncWorker { get; set; }
        readonly IManageServerSourceModel _updateManager;
        CancellationTokenSource _token;

        string _resourceName;
        readonly string _warewolfserverName;
        string _headerText;
        IServerSource _serverSource;
        string _protocol;
        string _selectedPort;
        bool _testing;
        IComputerName _serverName;
        IList<IComputerName> _computerNames;

        public ManageNewServerViewModel(IManageServerSourceModel updateManager, IEventAggregator aggregator, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : base(ResourceType.ServerSource)
        {
            VerifyArgument.IsNotNull("executor", executor);
            VerifyArgument.IsNotNull("asyncWorker", asyncWorker);
            VerifyArgument.IsNotNull("updateManager", updateManager);
            VerifyArgument.IsNotNull("aggregator", aggregator);

            AsyncWorker = asyncWorker;
            Protocols = new[] { "https", "http" };
            Protocol = Protocols[0];

            Ports = new ObservableCollection<string> { "3143", "3142" };
            SelectedPort = Ports[0];
            _updateManager = updateManager;

            _warewolfserverName = updateManager.ServerName;
            Header = Resources.Languages.Core.ServerSourceNewHeaderLabel;
            HeaderText = Resources.Languages.Core.ServerSourceNewHeaderLabel;

            TestCommand = new DelegateCommand(TestConnection, CanTest);
            OkCommand = new DelegateCommand(SaveConnection, CanSave);
            CancelTestCommand = new DelegateCommand(CancelTest, CanCancelTest);
        }
        public ManageNewServerViewModel(IManageServerSourceModel updateManager, IRequestServiceNameViewModel requestServiceNameViewModel, IEventAggregator aggregator, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker, executor)
        {
            VerifyArgument.IsNotNull("requestServiceNameViewModel", requestServiceNameViewModel);
            RequestServiceNameViewModel = requestServiceNameViewModel;
            GetLoadComputerNamesTask(null);
        }
        public ManageNewServerViewModel(IManageServerSourceModel updateManager, IEventAggregator aggregator, IServerSource serverSource, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker, executor)
        {
            VerifyArgument.IsNotNull("serverSource", serverSource);
            _serverSource = serverSource;
            _warewolfserverName = updateManager.ServerName;

            Item = new ServerSource
            {
                AuthenticationType = _serverSource.AuthenticationType,
                ID = _serverSource.ID,
                Name = _serverSource.Name,
                Password = _serverSource.Password,
                ResourcePath = _serverSource.ResourcePath,
                ServerName = _serverSource.ServerName,
                UserName = _serverSource.UserName,
                Address = _serverSource.Address
            };

            GetLoadComputerNamesTask(() =>
                {
                    FromSource();
                    SetupHeaderTextFromExisting();
                }
            );
        }

        void SetupHeaderTextFromExisting()
        {
            if (_warewolfserverName != null)
            {
            }
            HeaderText = (_serverSource == null ? ResourceName : _serverSource.Name).Trim();

            Header = ((_serverSource == null ? ResourceName : _serverSource.Name));
        }

        void FromSource()
        {
            ResourceName = _serverSource.Name;
            AuthenticationType = _serverSource.AuthenticationType;
            UserName = _serverSource.UserName;
            ServerName = ComputerNames.FirstOrDefault(name => _serverSource.ServerName == name.Name);
            Address = GetAddressName();
            Password = _serverSource.Password;
            Header = ResourceName;
        }

        public IComputerName ServerName
        {
            get { return _serverName ?? new ComputerName(); }
            set
            {
                if (value != _serverName)
                {
                    _serverName = value;
                    OnPropertyChanged(() => ServerName);
                    OnPropertyChanged(() => Header);
                    Reset();
                    Address = GetAddressName();
                }
            }
        }

        void Reset()
        {
            TestPassed = false;
            TestMessage = "";
            TestFailed = false;
            Testing = false;
            ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
            ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
        }

        public override bool CanSave()
        {
            return TestPassed;
        }

        void GetLoadComputerNamesTask(Action additionalUiAction)
        {
            AsyncWorker.Start(() => _updateManager.GetComputerNames().Select(name => new ComputerName { Name = name } as IComputerName).ToList(), names =>
            {
                ComputerNames = names;
                if (additionalUiAction != null)
                {
                    additionalUiAction();
                }
            });
        }

        void SaveConnection()
        {
            if (_serverSource == null)
            {
                var res = RequestServiceNameViewModel.ShowSaveDialog();

                if (res == MessageBoxResult.OK)
                {
                    ResourceName = RequestServiceNameViewModel.ResourceName.Name;
                    var src = ToSource();
                    src.ResourcePath = RequestServiceNameViewModel.ResourceName.Path ?? RequestServiceNameViewModel.ResourceName.Name;
                    Save(src);
                    Item = src;
                    _serverSource = src;
                    SetupHeaderTextFromExisting();
                }
            }
            else
            {
                var src = ToSource();
                Save(src);
                Item = src;
                _serverSource = src;
                SetupHeaderTextFromExisting();
            }
        }

        void Save(IServerSource source)
        {
            _updateManager.Save(source);
        }
        public override void Save()
        {
            SaveConnection();
        }

        public override IServerSource ToModel()
        {
            if (Item == null)
            {
                Item = ToSource();
                return Item;
            }
            return new ServerSource
            {
                Name = Item.Name,
                Address = GetAddressName(),
                AuthenticationType = AuthenticationType,
                Password = Password,
                UserName = UserName,
                ID = Item.ID,
                ResourcePath = Item.ResourcePath
            };
        }

        IServerSource ToNewSource()
        {

            return new ServerSource
            {
                AuthenticationType = AuthenticationType,
                Address = GetAddressName(),
                Password = Password,
                UserName = UserName,
                Name = ResourceName,
                ID = _serverSource == null ? Guid.NewGuid() : _serverSource.ID
            };
        }

        IServerSource ToSource()
        {
            if (_serverSource == null)
                return new ServerSource
                {
                    Address = GetAddressName(),
                    AuthenticationType = AuthenticationType,
                    Name = ResourceName,
                    Password = Password,
                    ID = _serverSource == null ? Guid.NewGuid() : _serverSource.ID
                }
            ;
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                _serverSource.AuthenticationType = AuthenticationType;
                _serverSource.Address = GetAddressName();
                _serverSource.Password = Password;
                _serverSource.UserName = UserName;
                return _serverSource;
            }
        }

        public bool CanTest()
        {
            if (Testing)
                return false;
            if (String.IsNullOrEmpty(Address))
            {
                return false;
            }
            if (AuthenticationType == AuthenticationType.User)
            {
                return !String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password);
            }
            return true;
        }

        bool CanCancelTest()
        {
            return Testing;
        }

        void CancelTest()
        {
            if (_token != null)
            {
                if (!_token.IsCancellationRequested && _token.Token.CanBeCanceled)
                {
                    _token.Cancel();
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        Testing = false;
                        TestFailed = true;
                        TestPassed = false;
                        TestMessage = "Test Cancelled";
                    });
                }
            }
        }

        void TestConnection()
        {
            _token = new CancellationTokenSource();
            AsyncWorker.Start(SetupProgressSpinner, () =>
            {
                TestMessage = "Passed";
                TestFailed = false;
                TestPassed = true;
                Testing = false;
            },
            _token, exception =>
            {
                TestFailed = true;
                TestPassed = false;
                Testing = false;
                TestMessage = exception != null ? exception.Message : "Failed";
            });
        }

        void SetupProgressSpinner()
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                TestMessage = "";
                Testing = true;
                TestFailed = false;
                TestPassed = false;
            });
            _updateManager.TestConnection(ToNewSource());
        }

        /// <summary>
        /// Command for save/ok
        /// </summary>
        public ICommand OkCommand { get; set; }
        /// <summary>
        /// Command for cancel
        /// </summary>
        public ICommand CancelCommand { get; set; }
        public ICommand CancelTestCommand { get; set; }

        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                OnPropertyChanged(() => HeaderText);
                OnPropertyChanged(() => Header);
            }
        }
        public IList<IComputerName> ComputerNames
        {
            get
            {
                return _computerNames;
            }
            set
            {
                _computerNames = value;
                OnPropertyChanged(()=>ComputerNames);
            }
        }

        #endregion

        #region Implementation of INewServerDialogue

        /// <summary>
        /// The server address that we are trying to connect to
        /// </summary>
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(() => Address);
                    OnPropertyChanged(() => Header);
                    Reset();
                }
            }
        }
        /// <summary>
        ///  Windows or user or public
        /// </summary>
        public AuthenticationType AuthenticationType
        {
            get { return _authenticationType; }
            set
            {
                if (_authenticationType != value)
                {
                    _authenticationType = value;
                    OnPropertyChanged(() => AuthenticationType);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => UserAuthenticationSelected);
                    Reset();
                }
            }
        }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(() => UserName);
                    OnPropertyChanged(() => Header);
                    Reset();
                }
            }
        }
        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(() => Password);
                    OnPropertyChanged(() => Header);
                    Reset();
                }
            }
        }

        /// <summary>
        /// The message that will be set if the test is either successful or not
        /// </summary>
        public string TestMessage
        {
            get { return _testMessage; }
            // ReSharper disable UnusedMember.Local
            private set
            // ReSharper restore UnusedMember.Local
            {
                _testMessage = value;
                OnPropertyChanged(() => TestMessage);
                OnPropertyChanged(() => TestPassed);
            }
        }

        #endregion

        private string GetAddressName()
        {
            string addressName = null;
            if (ServerName != null)
            {
                addressName = GetAddressName(Protocol,ServerName.Name,SelectedPort);
            }
            return addressName;
        }

        string GetAddressName(string protocol, string serverName, string port)
        {
            return protocol + "://" + serverName + ":" + port;
        }

        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                _resourceName = value;
                OnPropertyChanged(ResourceName);
            }
        }
        public bool TestPassed
        {
            get { return _testPassed; }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => TestPassed);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }
        public bool TestFailed
        {
            get
            {
                return _testPassed;
            }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => TestFailed);
            }
        }
        public bool Testing
        {
            get
            {
                return _testing;
            }
            set
            {
                _testing = value;
                OnPropertyChanged(() => Testing);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                ViewModelUtils.RaiseCanExecuteChanged(CancelTestCommand);
            }
        }

        public bool UserAuthenticationSelected
        {
            get { return AuthenticationType == AuthenticationType.User; }
        }

        [ExcludeFromCodeCoverage]
        public string AddressLabel
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogAddressLabel;
            }
        }
        [ExcludeFromCodeCoverage]
        public string UserNameLabel
        {
            get
            {
                return Resources.Languages.Core.UserNameLabel;
            }
        }
        [ExcludeFromCodeCoverage]
        public string AuthenticationLabel
        {
            get
            {
                return Resources.Languages.Core.AuthenticationTypeLabel;
            }
        }
        [ExcludeFromCodeCoverage]
        public string PasswordLabel
        {
            get
            {
                return Resources.Languages.Core.PasswordLabel;

            }
        }
        [ExcludeFromCodeCoverage]
        public string TestLabel
        {
            get
            {
                return Resources.Languages.Core.TestConnectionLabel;
            }
        }
        [ExcludeFromCodeCoverage]
        public string CancelTestLabel
        {
            get
            {
                return Resources.Languages.Core.CancelTest;
            }
        }

        /// <summary>
        /// Test if connection is successful
        /// </summary>
        public ICommand TestCommand { get; set; }

        IRequestServiceNameViewModel RequestServiceNameViewModel { get; set; }
        public string Protocol
        {
            get
            {
                return _protocol;
            }
            set
            {
                if (_protocol != value)
                {
                    _protocol = value;
                    OnPropertyChanged(Protocol);
                    Reset();
                    if (Protocol == "https" && SelectedPort == "3142")
                    {
                        SelectedPort = "3143";
                    }
                    else if (Protocol == "http" && SelectedPort == "3143")
                    {
                        SelectedPort = "3142";
                    }
                }
            }
        }
        public string[] Protocols { get; set; }
        public ObservableCollection<string> Ports { get; set; }
        public string SelectedPort
        {
            get
            {
                return _selectedPort;
            }
            set
            {
                if (_selectedPort != value)
                {
                    _selectedPort = value;
                    OnPropertyChanged(() => SelectedPort);
                    Reset();
                }
            }
        }

        public string AddressToolTip
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogAddressToolTip;
            }
        }
        public string ProtocolToolTip
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogProtocolToolTip;
            }
        }
        public string PortToolTip
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogAddressPortTip;
            }
        }
        public string WindowsAuthenticationToolTip
        {
            get
            {
                return Resources.Languages.Core.WindowsAuthenticationToolTip;
            }
        }
        public string UserAuthenticationToolTip
        {
            get
            {
                return Resources.Languages.Core.UserAuthenticationToolTip;
            }
        }
        public string PublicToolTip
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogPublicToolTip;
            }
        }
        public string UserNameToolTip
        {
            get
            {
                return Resources.Languages.Core.UserNameToolTip;
            }
        }
        public string PasswordToolTip
        {
            get
            {
                return Resources.Languages.Core.PasswordToolTip;
            }
        }
        public string TestToolTip
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogsTestToolTip;
            }
        }
        public string SaveToolTip
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogAddressSaveTip;
            }
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var mainViewModel = CustomContainer.Get<IMainViewModel>();
            if (mainViewModel != null)
            {
                mainViewModel.HelpViewModel.UpdateHelpText(helpText);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return _headerText;
        }
    }

    public interface IManageServerSourceModel
    {
        IList<string> GetComputerNames();
        void TestConnection(IServerSource resource);

        void Save(IServerSource toDbSource);


        string ServerName { get; set; }
    }
}