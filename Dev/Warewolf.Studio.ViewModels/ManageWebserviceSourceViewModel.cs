﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Threading;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Warewolf.Core;
using Warewolf.Studio.Models.Help;

namespace Warewolf.Studio.ViewModels
{
    public class ManageWebserviceSourceViewModel : SourceBaseImpl<IWebServiceSource>, IManageWebserviceSourceViewModel, IDisposable
    {
        public IAsyncWorker AsyncWorker { get; set; }
        public IExternalProcessExecutor Executor { get; set; }
        private AuthenticationType _authenticationType;
        private string _hostName;
        private string _userName;
        private string _password;
        private string _defaultQuery;
        private string _testMessage;
        private string _testDefault;
        // private string _header;
        readonly IManageWebServiceSourceModel _updateManager;
        readonly IEventAggregator _aggregator;
        IWebServiceSource _webServiceSource;
        bool _testPassed;
        bool _testFailed;
        bool _testing;
        bool _isHyperLinkEnabled;
        string _resourceName;
        CancellationTokenSource _token;
        readonly string _warewolfserverName;
        string _headerText;
        private bool _isDisposed;

        public ManageWebserviceSourceViewModel(IManageWebServiceSourceModel updateManager, IEventAggregator aggregator,IAsyncWorker asyncWorker,IExternalProcessExecutor executor)
            : base(ResourceType.WebSource)
        {
            VerifyArgument.IsNotNull("executor", executor);
            VerifyArgument.IsNotNull("asyncWorker", asyncWorker);
            VerifyArgument.IsNotNull("updateManager", updateManager);
            VerifyArgument.IsNotNull("aggregator", aggregator);
            AsyncWorker = asyncWorker;
            Executor = executor;
            _updateManager = updateManager;
            _aggregator = aggregator;
            _warewolfserverName = updateManager.ServerName;
            _authenticationType = AuthenticationType.Anonymous;
            HeaderText = Resources.Languages.Core.WebserviceNewHeaderLabel;
            Header = Resources.Languages.Core.WebserviceNewHeaderLabel;
            TestCommand = new DelegateCommand(TestConnection, CanTest);
            OkCommand = new DelegateCommand(SaveConnection, CanSave);
            CancelTestCommand = new DelegateCommand(CancelTest, CanCancelTest);
            ViewInBrowserCommand = new DelegateCommand(ViewInBrowser, CanViewInBrowser);

        }

        bool CanViewInBrowser()
        {
            return TestPassed;
        }

        void ViewInBrowser()
        {
            Executor.OpenInBrowser(new Uri(TestDefault));
        }

        public ManageWebserviceSourceViewModel(IManageWebServiceSourceModel updateManager, IRequestServiceNameViewModel requestServiceNameViewModel, IEventAggregator aggregator, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker,executor)
        {
            VerifyArgument.IsNotNull("requestServiceNameViewModel", requestServiceNameViewModel);
            RequestServiceNameViewModel = requestServiceNameViewModel;

        }
        public ManageWebserviceSourceViewModel(IManageWebServiceSourceModel updateManager, IEventAggregator aggregator, IWebServiceSource webServiceSource, IAsyncWorker asyncWorker, IExternalProcessExecutor executor)
            : this(updateManager, aggregator, asyncWorker, executor)
        {
            VerifyArgument.IsNotNull("webServiceSource", webServiceSource);
            _webServiceSource = webServiceSource;
            _warewolfserverName = updateManager.ServerName;
            SetupHeaderTextFromExisting();
            FromSource(webServiceSource);
        }

        void SetupHeaderTextFromExisting()
        {
            var serverName = _warewolfserverName;
            if(serverName.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                HeaderText = string.Format("{0} {1}", Resources.Languages.Core.WebserviceEditHeaderLabel, (_webServiceSource == null ? ResourceName : _webServiceSource.Name).Trim());
                Header = string.Format("{0}", ((_webServiceSource == null ? ResourceName : _webServiceSource.Name)));
            }
            else
            {
                HeaderText = string.Format("{0} {1} on {2}", Resources.Languages.Core.WebserviceEditHeaderLabel, (_webServiceSource == null ? ResourceName : _webServiceSource.Name).Trim(), serverName);
                Header = string.Format("{0} - {1}", ((_webServiceSource == null ? ResourceName : _webServiceSource.Name)),serverName);
            }
        }

        bool CanSave()
        {
            return TestPassed;
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

        public bool CanTest()
        {
            if (Testing)
                return false;
            if (String.IsNullOrEmpty(HostName))
            {
                return false;
            }
            if (AuthenticationType == AuthenticationType.User)
            {
                return !String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password);
            }
            return true;
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var helpDescriptor = new HelpDescriptor("", helpText, null);
            VerifyArgument.IsNotNull("helpDescriptor", helpDescriptor);
            _aggregator.GetEvent<HelpChangedEvent>().Publish(helpDescriptor);

        }

        void FromSource(IWebServiceSource webServiceSource)
        {
            ResourceName = webServiceSource.Name;
            AuthenticationType = webServiceSource.AuthenticationType;
            UserName = webServiceSource.UserName;
            DefaultQuery = webServiceSource.DefaultQuery;
            HostName = webServiceSource.HostName;
            Password = webServiceSource.Password;

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

        void SaveConnection()
        {
            if (_webServiceSource == null)
            {
                var res = RequestServiceNameViewModel.ShowSaveDialog();

                if (res == MessageBoxResult.OK)
                {
                    ResourceName = RequestServiceNameViewModel.ResourceName.Name;
                    var src = ToSource();
                    src.Path = RequestServiceNameViewModel.ResourceName.Path ?? RequestServiceNameViewModel.ResourceName.Name;
                    Save(src);
                    Item = src;
                    _webServiceSource = src;
                    SetupHeaderTextFromExisting();
                }
            }
            else
            {
                var src = ToSource();
                Save(src);
                Item = src;
                _webServiceSource = src;
                SetupHeaderTextFromExisting();
            }
        }

        void Save(IWebServiceSource source)
        {
            _updateManager.Save(source);

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
            _updateManager.TestConnection(ToNewSource());
        }

        IWebServiceSource ToNewSource()
        {

            return new WebServiceSourceDefinition
            {
                AuthenticationType = AuthenticationType,
                HostName = HostName,
                Password = Password,
                UserName = UserName,
                Name = ResourceName,
                DefaultQuery = DefaultQuery,
                Id = _webServiceSource == null ? Guid.NewGuid() : _webServiceSource.Id
            };
        }

        IWebServiceSource ToSource()
        {
            if (_webServiceSource == null)
                return new WebServiceSourceDefinition
                {
                    AuthenticationType = AuthenticationType,
                    HostName = HostName,
                    Password = Password,
                    UserName = UserName,
                    DefaultQuery = DefaultQuery,
                    Name = ResourceName,
                    Id = _webServiceSource == null ? Guid.NewGuid() : _webServiceSource.Id
                }
            ;
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                _webServiceSource.AuthenticationType = AuthenticationType;
                _webServiceSource.DefaultQuery = DefaultQuery;
                _webServiceSource.Password = Password;
                _webServiceSource.HostName = HostName;
                _webServiceSource.UserName = UserName;
                return _webServiceSource;

            }
        }

        public override IWebServiceSource ToModel()
        {
            if (Item == null)
            {
                Item = ToSource();
                return Item;
            }

            return new WebServiceSourceDefinition
            {
                Name = Item.Name,
                HostName = HostName,
                AuthenticationType = AuthenticationType,
                DefaultQuery = DefaultQuery,
                Id = Item.Id,
                Path = Item.Path
            };

        }

        IRequestServiceNameViewModel RequestServiceNameViewModel { get; set; }

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
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public string HostName
        {
            get { return _hostName; }
            set
            {
                if (value != _hostName)
                {
                    _hostName = value;
                    TestDefault = _hostName + "" + DefaultQuery;
                    OnPropertyChanged(() => HostName);
                    OnPropertyChanged(() => Header);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public string DefaultQuery
        {
            get { return _defaultQuery; }
            set
            {

                _defaultQuery = value;
                TestDefault = _hostName + "" + DefaultQuery;
                OnPropertyChanged(() => DefaultQuery);
                OnPropertyChanged(() => Header);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }


        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(() => UserName);
                OnPropertyChanged(() => Header);
                TestPassed = false;
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(() => Password);
                OnPropertyChanged(() => Header);
                TestPassed = false;
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }

        public ICommand CancelTestCommand { get; set; }

        public ICommand TestCommand { get; set; }

        public ICommand ViewInBrowserCommand { get; set; }

        public string TestDefault
        {
            get { return _testDefault; }
            set
            {
                _testDefault = value;
                OnPropertyChanged(() => TestDefault);
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
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


        public bool TestPassed
        {
            get { return _testPassed; }
            set
            {
                _testPassed = value;
                if (_testPassed)
                {
                    IsHyperLinkEnabled = true;
                }
                OnPropertyChanged(() => TestPassed);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                ViewModelUtils.RaiseCanExecuteChanged(ViewInBrowserCommand);

            }


        }

        public bool IsHyperLinkEnabled
        {
            get { return _isHyperLinkEnabled; }
            set
            {
                _isHyperLinkEnabled = value;
                OnPropertyChanged(() => IsHyperLinkEnabled);
            }
        }


        [ExcludeFromCodeCoverage]
        public string DefaultQueryLabel
        {
            get
            {
                return Resources.Languages.Core.DefaultQueryLabel;
            }
        }

        [ExcludeFromCodeCoverage]
        public string TestDefaultLabel
        {
            get { return Resources.Languages.Core.TestDefaultLabel; }
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

        [ExcludeFromCodeCoverage]
        public string ViewInBrowserLabel
        {
            get
            {
                return Resources.Languages.Core.ViewInBrowserLabel;
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
                ViewModelUtils.RaiseCanExecuteChanged(ViewInBrowserCommand);
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(CancelTestCommand);
            }
        }

        [ExcludeFromCodeCoverage]
        public string HostNameLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseWebserviceLabel;
            }
        }

        public ICommand OkCommand { get; set; }

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

        public bool IsEmpty { get { return String.IsNullOrEmpty(HostName) && AuthenticationType == AuthenticationType.Anonymous && String.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password); } }

        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_isDisposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    _token.Dispose();
                }

                // Dispose unmanaged resources.
                _isDisposed = true;
            }
        }
    }
}