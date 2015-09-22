using System;
using System.Collections.ObjectModel;
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
        Guid _id;
        
        public ManageNewServerViewModel(IManageServerSourceModel updateManager, IEventAggregator aggregator, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            :base(ResourceType.ServerSource)
        {
            VerifyArgument.IsNotNull("executor", executor);
            VerifyArgument.IsNotNull("asyncWorker", asyncWorker);
            VerifyArgument.IsNotNull("updateManager", updateManager);
            VerifyArgument.IsNotNull("aggregator", aggregator);

            Protocols = new[] { "https", "http" };
            Protocol = Protocols[0];
         
            Ports = new ObservableCollection<string> { "3143", "3142" };
            SelectedPort = Ports[0];
            _updateManager = updateManager;

            _warewolfserverName = updateManager.ServerName;
            Header = Resources.Languages.Core.ServerSourceNewHeaderLabel;
            HeaderText = Resources.Languages.Core.ServerSourceNewHeaderLabel;
            ID = Guid.NewGuid();
            IsValid = false;
            TestPassed = false;

            TestCommand = new DelegateCommand(TestConnection, CanTest);
            OkCommand = new DelegateCommand(SaveConnection, CanSave);
            CancelTestCommand = new DelegateCommand(CancelTest, CanCancelTest);
        }
        public ManageNewServerViewModel(IManageServerSourceModel updateManager, IRequestServiceNameViewModel requestServiceNameViewModel, IEventAggregator aggregator, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker,executor)
        {
            VerifyArgument.IsNotNull("requestServiceNameViewModel", requestServiceNameViewModel);
            RequestServiceNameViewModel = requestServiceNameViewModel;

        }
        public ManageNewServerViewModel(IManageServerSourceModel updateManager, IEventAggregator aggregator, IServerSource serverSource, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker, executor)
        {
            VerifyArgument.IsNotNull("serverSource", serverSource);
            _serverSource = serverSource;
            _warewolfserverName = updateManager.ServerName;
            SetupHeaderTextFromExisting();
            FromSource(serverSource);
        }

        void SetupHeaderTextFromExisting()
        {
            var serverName = _warewolfserverName;
            if (serverName.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                HeaderText = (_serverSource == null ? ResourceName : _serverSource.Name).Trim();
                Header = (_serverSource == null ? ResourceName : _serverSource.Name).Trim();
            }
            else
            {
                HeaderText = (_serverSource == null ? ResourceName : _serverSource.Name).Trim();
                Header = (_serverSource == null ? ResourceName : _serverSource.Name).Trim();
            }
        }

        void FromSource(IServerSource serverSource)
        {
            ResourceName = serverSource.Name;
            AuthenticationType = serverSource.AuthenticationType;
            UserName = serverSource.UserName;
            Address = serverSource.Address;
            Password = serverSource.Password;
            Header = ResourceName;
        }

        public override bool CanSave()
        {
            return TestPassed;
        }

        //public override void Save()
        //{
        //    var res = MessageBoxResult.OK;
        //    if (String.IsNullOrEmpty(ServerSource.Name))
        //        res = RequestServiceNameViewModel.ShowSaveDialog();
        //    if (res == MessageBoxResult.OK)
        //    {
        //        try
        //        {
        //            var source = ToModel();

        //            ServerSource = source;
        //            ServerSource.Name = RequestServiceNameViewModel.ResourceName.Name;
        //            ServerSource.ResourcePath = RequestServiceNameViewModel.ResourceName.Path;
        //            _updateManager.Save(source);
        //            HeaderText = SetToEdit(source);
        //            if (CloseAction != null)
        //                CloseAction.Invoke();
        //            ConnectControlSingleton.Instance.AddServerAndConnect(ServerSource);
        //        }
        //        catch (Exception err)
        //        {

        //            TestMessage = err.Message;
        //        }
        //    }

        //}

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
            }
            return new ServerSource
            {
                Address = Protocol + "://" + Address + ":" + SelectedPort,
                AuthenticationType = AuthenticationType,
                ID = ServerSource.ID == Guid.Empty ? Guid.NewGuid() : Item.ID,
                Name = ServerSource.Name,
                Password = Password
            };
        }

        IServerSource ToSource()
        {
            if (_serverSource == null)
            return new ServerSource
            {
                Address = Protocol + "://" + Address + ":" + SelectedPort,
                AuthenticationType = AuthenticationType,
                ID = ServerSource.ID == Guid.Empty ? Guid.NewGuid() : ServerSource.ID,
                Name = ServerSource.Name,
                Password = Password
            };
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                _serverSource.Address = Protocol + "://" + Address + ":" + SelectedPort;
                _serverSource.AuthenticationType = AuthenticationType;
                _serverSource.ID = _serverSource.ID == Guid.Empty ? Guid.NewGuid() : _serverSource.ID;
                _serverSource.Name = ServerSource.Name;
                _serverSource.Password = Password;
                return _serverSource;
            }
        }

        public Action CloseAction { get; set; }
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
                Testing = true;
                TestFailed = false;
                TestPassed = false;
            });
            _updateManager.TestConnection(ToSource());
        }

        /// <summary>
        /// called by outer when validating
        /// </summary>
        /// <returns></returns>
        public string Validate
        {

            get
            {
                IsValid = false;



                if (!Testrun)
                {
                    return Resources.Languages.Core.ServerSourceDialogNoTestMessage;
                }

                if (TestFailed)
                {
                    return Resources.Languages.Core.TestConnectionLabel; 
                }

                IsValid = true;
                return String.Empty;
            }



        }
        public bool Testrun { get; set; }
        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(() => ID);
            }
        }



        /// <summary>
        /// Is valid 
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Command for save/ok
        /// </summary>
        public ICommand OkCommand { get; set; }
        /// <summary>
        /// Command for cancel
        /// </summary>
        public ICommand CancelCommand { get; set; }
        public ICommand CancelTestCommand { get; set; }

        public bool CanClickOk
        {
            get
            {
                return Validate == "";
            }
        }

        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                OnPropertyChanged(() => HeaderText);
                OnPropertyChanged(() => Header);
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
                _address = value;

                OnPropertyChanged(() => Address);
                OnPropertyChanged(() => Validate);
                OnPropertyChanged(() => CanClickOk);
            }
        }
        /// <summary>
        ///  Windows or user or publlic
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
                    OnPropertyChanged(() => IsUserNameVisible);
                    Testrun = false;
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
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
                _userName = value;
                OnPropertyChanged(() => UserName);
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
                _password = value;
                OnPropertyChanged(() => Password);
            }
        }

        /// <summary>
        /// The message that will be set if the test is either successful or not
        /// </summary>
        public string TestMessage
        {
            get
            {
                return _testMessage;
            }

            set
            {
                _testMessage = value;
                OnPropertyChanged(() => TestMessage);
            }

        }

        #endregion

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
            get
            {
                return _testPassed;
            }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => TestPassed);
                OnPropertyChanged(() => CanClickOk);
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
                OnPropertyChanged(() => CanClickOk);
            }
        }
        public bool Testing
        {
            get
            {
                return _testPassed;
            }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => Testing);
                OnPropertyChanged(() => CanClickOk);
            }
        }

        public bool IsOkEnabled
        {
            get
            {
                return IsValid;
            }

        }

        public bool IsTestEnabled
        {
            get
            {
                return (Address.Length > 0);
            }

        }

        public bool IsUserNameVisible
        {
            get
            {
                return AuthenticationType == AuthenticationType.User;
            }

        }

        public bool IsPasswordVisible
        {
            get
            {
                return AuthenticationType == AuthenticationType.User;
            }

        }

        public string AddressLabel
        {
            get
            {
                return Resources.Languages.Core.ServerSourceDialogAddressLabel;
            }
        }

        public string UserNameLabel
        {
            get
            {
                return Resources.Languages.Core.UserNameLabel;
            }
        }

        public string AuthenticationLabel
        {
            get
            {
                return Resources.Languages.Core.AuthenticationTypeLabel;
            }
        }

        public string PasswordLabel
        {
            get
            {
                return Resources.Languages.Core.PasswordLabel;

            }
        }

        public string TestLabel
        {
            get
            {
                return Resources.Languages.Core.TestConnectionLabel;
            }
        }


        /// <summary>
        /// Test if connection is successful
        /// </summary>
        public ICommand TestCommand
        { get; set; }
        public IServerSource ServerSource
        {
            get
            {
                return _serverSource;
            }
            set
            {
                _serverSource = value;
            }
        }
        IRequestServiceNameViewModel RequestServiceNameViewModel { get; set; }
        public string Protocol
        {
            get
            {
                return _protocol;
            }
            set
            {
                _protocol = value;
                OnPropertyChanged(Protocol);
                if (Protocol == "https" && SelectedPort == "3142")
                {
                    SelectedPort = "3143";
                }
                else if(Protocol == "http" && SelectedPort == "3143")
                {
                    SelectedPort = "3142";
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
                    TestPassed = false;
                    OnPropertyChanged(()=>TestPassed);
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

        void TestConnection(IServerSource resource);

        void Save(IServerSource toDbSource);


        string ServerName { get; set; }
    }
}