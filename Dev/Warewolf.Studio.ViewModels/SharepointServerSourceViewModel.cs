using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Dev2;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.Threading;
using Dev2.Data.ServiceModel;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Studio.Core.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Warewolf.Core;
using Warewolf.Studio.Models.Help;

namespace Warewolf.Studio.ViewModels
{
    public class SharepointServerSourceViewModel : SourceBaseImpl<ISharepointServerSource>, IManageSharePointSourceViewModel, IDisposable
    {
        public IAsyncWorker AsyncWorker { get; set; }
        public IExternalProcessExecutor Executor { get; set; }
        ISharepointServerSource _sharePointServiceSource;
        readonly IRequestServiceNameViewModel _saveDialog;
        readonly IEnvironmentModel _environment;
        readonly ISharePointSourceModel _updateManager;
        readonly IEventAggregator _aggregator;
        string _serverName;
        bool _isWindows;
        bool _isUser;
        string _userName;
        string _password;
        string _testResult;
        readonly string _warewolfserverName;
        IContextualResourceModel _resource;
        AuthenticationType _authenticationType;
        CancellationTokenSource _token;
        bool _testComplete;
        bool _isLoading;
        bool _testPassed;
        string _resourceName;
        bool _testing;
        string _headerText;
        string _testMessage;
        bool _testFailed;
        string _path;

        public SharepointServerSourceViewModel(ISharePointSourceModel updateManager, IEventAggregator aggregator, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : base(ResourceType.SharepointServerSource)
        {
            VerifyArgument.IsNotNull("executor", executor);
            VerifyArgument.IsNotNull("asyncWorker", asyncWorker);
            VerifyArgument.IsNotNull("updateManager", updateManager);
            VerifyArgument.IsNotNull("aggregator", aggregator);
            AsyncWorker = asyncWorker;
            Executor = executor;
            _updateManager = updateManager;
            _aggregator = aggregator;
            ServerName = String.Empty;
            _warewolfserverName = updateManager.ServerName;
            _authenticationType = AuthenticationType.Windows;
            IsWindows = true;
            HeaderText = Resources.Languages.Core.SharePointServiceNewHeaderLabel;
            Header = Resources.Languages.Core.SharePointServiceNewHeaderLabel;
            TestCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(TestConnection, CanTest);
            SaveCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(SaveConnection, CanSave);
            CancelTestCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(CancelTest, CanCancelTest);
        }

        public SharepointServerSourceViewModel(ISharePointSourceModel updateManager, IRequestServiceNameViewModel requestServiceNameViewModel, IEventAggregator aggregator, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker, executor)
        {
            VerifyArgument.IsNotNull("requestServiceNameViewModel", requestServiceNameViewModel);
            RequestServiceNameViewModel = requestServiceNameViewModel;
        }

        public SharepointServerSourceViewModel(ISharePointSourceModel updateManager, IEventAggregator aggregator, ISharepointServerSource sharePointServiceSource, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker, executor)
        {
            VerifyArgument.IsNotNull("sharePointServiceSource", sharePointServiceSource);
            _sharePointServiceSource = sharePointServiceSource;
            _warewolfserverName = updateManager.ServerName;
            FromSource(sharePointServiceSource);
        }

        void FromSource(ISharepointServerSource webServiceSource)
        {
            ResourceName = webServiceSource.Name;
            AuthenticationType = webServiceSource.AuthenticationType;
            UserName = webServiceSource.UserName;
            ServerName = webServiceSource.Server;
            Password = webServiceSource.Password;

        }

        public string TestResult
        {
            get
            {
                return _testResult;
            }
            set
            {
                _testResult = value;
                if (!_testResult.Contains("Failed"))
                {
                    TestComplete = true;
                }
                OnPropertyChanged("TestResult");
            }
        }

        SharepointSource CreateSharepointServerSource()
        {
            var source = new SharepointSource { Server = ServerName, UserName = UserName, Password = Password, AuthenticationType = AuthenticationType };
            return source;
        }

        void TestConnection()
        {
            _token = new CancellationTokenSource();
            AsyncWorker.Start(SetupProgressSpinner, () =>
            {
                TestMessage = "";
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

        public void Save(ISharepointServerSource source)
        {
            _updateManager.Save(source);
        }

        public string ServerName
        {
            get
            {
                return _serverName;
            }
            set
            {
                _serverName = value;
                OnPropertyChanged(() => ServerName);
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
            }
        }
        public bool IsWindows
        {
            get
            {
                return _isWindows;
            }
            set
            {
                if (value.Equals(_isWindows))
                {
                    return;
                }
                _isWindows = value;
                if (_isWindows)
                {
                    AuthenticationType = AuthenticationType.Windows;
                }

                OnPropertyChanged(() => IsWindows);
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
            }
        }
        public bool IsUser
        {
            get
            {
                return _isUser;
            }
            set
            {
                if (value.Equals(_isUser))
                {
                    return;
                }
                _isUser = value;
                if (_isUser)
                {
                    AuthenticationType = AuthenticationType.User;
                }
                OnPropertyChanged(() => IsUser);
            }
        }
        public bool TestFailed
        {
            get
            {
                return _testFailed;
            }
            set
            {
                _testFailed = value;
                OnPropertyChanged(() => TestFailed);
            }
        }
        public bool TestPassed
        {
            get { return _testPassed; }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => TestPassed);
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
            }
        }
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

        public IRequestServiceNameViewModel RequestServiceNameViewModel { get; set; }
        public ICommand TestCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CancelTestCommand { get; set; }
        public IContextualResourceModel Resource
        {
            get
            {
                return _resource;
            }
            set
            {
                _resource = value;
                var xaml = _resource.WorkflowXaml;
                if (xaml.IsNullOrEmpty() && _resource.ID != Guid.Empty)
                {
                    var message = _environment.ResourceRepository.FetchResourceDefinition(_environment, GlobalConstants.ServerWorkspaceID, _resource.ID, false);
                    xaml = message.Message;
                }
                if (!xaml.IsNullOrEmpty())
                {
                    UpdateBasedOnResource(new SharepointSource(xaml.ToXElement()));
                }
            }
        }

        public bool TestComplete
        {
            get { return _testComplete; }
            set
            {
                _testComplete = value;
                OnPropertyChanged("TestComplete");
                var command = SaveCommand as RelayCommand;
                if (command != null)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }
        public bool Testing
        {
            get
            {
                return _testing;
            }
            private set
            {
                _testing = value;

                OnPropertyChanged(() => Testing);
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(CancelTestCommand);
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(() => IsLoading);
            }
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

        void SaveConnection()
        {
            if (_sharePointServiceSource == null)
            {
                var res = RequestServiceNameViewModel.ShowSaveDialog();

                if (res == MessageBoxResult.OK)
                {
                    ResourceName = RequestServiceNameViewModel.ResourceName.Name;
                    var src = ToSource();
                    src.Path = RequestServiceNameViewModel.ResourceName.Path ?? RequestServiceNameViewModel.ResourceName.Name;
                    Save(src);
                    Item = src;
                    _sharePointServiceSource = src;
                    SetupHeaderTextFromExisting();
                }
            }
            else
            {
                var src = ToSource();
                Save(src);
                Item = src;
                _sharePointServiceSource = src;
                SetupHeaderTextFromExisting();
            }
        }

        void SetupHeaderTextFromExisting()
        {
            HeaderText = string.Format("{0} {1} ", Resources.Languages.Core.SharePointServiceEditHeaderLabel, (_sharePointServiceSource == null ? ResourceName : _sharePointServiceSource.Name).Trim());
            Header = string.Format("{0} - {1}", Resources.Languages.Core.SharePointServiceEditHeaderLabel, (_sharePointServiceSource == null ? ResourceName : _sharePointServiceSource.Name));
        }

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

        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                _resourceName = value;
                OnPropertyChanged(_resourceName);
            }
        }

        public bool UserAuthenticationSelected
        {
            get { return AuthenticationType == AuthenticationType.User; }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged(() => Path);
            }
        }

        ISharepointServerSource ToNewSource()
        {
            return new SharePointServiceSourceDefinition
            {
                AuthenticationType = AuthenticationType,
                Server = ServerName,
                Password = Password,
                UserName = UserName,
                Name = ResourceName,
                Id = _sharePointServiceSource == null ? Guid.NewGuid() : _sharePointServiceSource.Id
            };
        }
        ISharepointServerSource ToSource()
        {
            if (_sharePointServiceSource == null)
                return new SharePointServiceSourceDefinition
                {
                    AuthenticationType = AuthenticationType,
                    Server = ServerName,
                    Password = Password,
                    UserName = UserName,
                    Name = ResourceName,
                    Id = _sharePointServiceSource == null ? Guid.NewGuid() : _sharePointServiceSource.Id
                };
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                _sharePointServiceSource.AuthenticationType = AuthenticationType;
                _sharePointServiceSource.Server = ServerName;
                _sharePointServiceSource.Password = Password;
                _sharePointServiceSource.UserName = UserName;
                return _sharePointServiceSource;
            }
        }

        void UpdateBasedOnResource(SharepointSource sharepointSource)
        {
            ServerName = sharepointSource.Server;
            UserName = sharepointSource.UserName;
            Password = sharepointSource.Password;
            AuthenticationType = sharepointSource.AuthenticationType;

        }

        void SetupProgressSpinner()
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                Testing = true;
                TestFailed = false;
                TestPassed = false;
            });
            _updateManager.TestConnection(ToNewSource());
        }

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
                OnPropertyChanged(() => Header);
                TestPassed = false;
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
            }
        }
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
                OnPropertyChanged(() => Header);
                TestPassed = false;
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
            }
        }
        public AuthenticationType AuthenticationType
        {
            get
            {
                return _authenticationType;
            }
            set
            {
                if (_authenticationType != value)
                {
                    _authenticationType = value;
                    if (_authenticationType == AuthenticationType.Windows)
                    {
                        IsWindows = true;
                        OnPropertyChanged(() => IsWindows);
                    }
                    else
                    {
                        IsUser = true;
                        OnPropertyChanged(() => IsUser);
                    }
                    OnPropertyChanged(() => AuthenticationType);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => UserAuthenticationSelected);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public string ServerNameLabel
        {
            get
            {
                return Resources.Languages.Core.ServerNameLabel;
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
        /// Tooltip for the Windows Authentication option
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string AnonymousAuthenticationToolTip
        {
            get
            {
                return Resources.Languages.Core.AnonymousAuthenticationToolTip;
            }
        }

        [ExcludeFromCodeCoverage]
        public string UserAuthenticationToolTip
        {
            get
            {
                return Resources.Languages.Core.UserAuthenticationToolTip;
            }
        }

        #region Overrides of SourceBaseImpl<ISharepointServerSource>

        public override ISharepointServerSource ToModel()
        {
            if (Item == null)
            {
                Item = ToSource();
                return Item;
            }

            return new SharePointServiceSourceDefinition
            {
                Name = ResourceName,
                Server = ServerName,
                AuthenticationType = AuthenticationType,
                Id = Item.Id,
                Path = Path
            };
        }

        public bool CanTest()
        {
            if (Testing)
                return false;
            if (String.IsNullOrEmpty(ServerName))
            {
                return false;
            }
            if (AuthenticationType == AuthenticationType.User)
            {
                return !String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password);
            }
            return true;
        }

        public override bool CanSave()
        {
            return TestPassed;
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var helpDescriptor = new HelpDescriptor("", helpText, null);
            VerifyArgument.IsNotNull("helpDescriptor", helpDescriptor);
            _aggregator.GetEvent<HelpChangedEvent>().Publish(helpDescriptor);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}
