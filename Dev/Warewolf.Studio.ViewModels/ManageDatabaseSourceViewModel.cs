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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Threading;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Warewolf.Studio.ViewModels
{
    public class ManageDatabaseSourceViewModel : SourceBaseImpl<IDbSource>, IManageDatabaseSourceViewModel, IDisposable
    {
        public IAsyncWorker AsyncWorker { get; set; }
        private NameValue _serverType;
        private AuthenticationType _authenticationType;
        private IComputerName _serverName;
        private string _databaseName;
        private string _userName;
        private string _password;
        private string _testMessage;
        private IList<string> _databaseNames;
        IManageDatabaseSourceModel _updateManager;
        IEventAggregator _aggregator;
        IDbSource _dbSource;
        bool _testPassed;
        bool _testFailed;
        bool _testing;
        string _resourceName;
        CancellationTokenSource _token;
        IList<IComputerName> _computerNames;
        readonly string _warewolfserverName;
        string _headerText;
        private bool _isDisposed;


        private void PerformInitialise(IManageDatabaseSourceModel updateManager, IEventAggregator aggregator)
        {
            VerifyArgument.IsNotNull("updateManager", updateManager);
            VerifyArgument.IsNotNull("aggregator", aggregator);
            _updateManager = updateManager;
            _aggregator = aggregator;

            HeaderText = Resources.Languages.Core.DatabaseSourceServerNewHeaderLabel;
            Header = Resources.Languages.Core.DatabaseSourceServerNewHeaderLabel;
            TestCommand = new DelegateCommand(TestConnection, CanTest);
            OkCommand = new DelegateCommand(SaveConnection, CanSave);
            CancelTestCommand = new DelegateCommand(CancelTest, CanCancelTest);
            Testing = false;
            Types = new List<NameValue> { new NameValue { Name = "Microsoft SQL Server", Value = enSourceType.SqlDatabase.ToString() }, new NameValue() { Name = "MySql Database", Value = enSourceType.SqlDatabase.ToString() } };
            ServerType = Types[0];
            _testPassed = false;
            _testFailed = false;
            DatabaseNames = new List<string>();
            ComputerNames = new List<IComputerName>();
            
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

        public ManageDatabaseSourceViewModel(IAsyncWorker asyncWorker):base(ResourceType.DbSource)
        {
            AsyncWorker = asyncWorker;
        }

        public ManageDatabaseSourceViewModel(IManageDatabaseSourceModel updateManager, IRequestServiceNameViewModel requestServiceNameViewModel, IEventAggregator aggregator, IAsyncWorker asyncWorker):this(asyncWorker)
        {
            VerifyArgument.IsNotNull("requestServiceNameViewModel", requestServiceNameViewModel);
            PerformInitialise(updateManager, aggregator);
            RequestServiceNameViewModel = requestServiceNameViewModel;
            GetLoadComputerNamesTask(null);

        }
        public ManageDatabaseSourceViewModel(IManageDatabaseSourceModel updateManager, IEventAggregator aggregator, IDbSource dbSource, IAsyncWorker asyncWorker)
            : this(asyncWorker)
        {
            VerifyArgument.IsNotNull("dbSource", dbSource);
            PerformInitialise(updateManager, aggregator);
            _warewolfserverName = updateManager.ServerName??"";
            _dbSource = dbSource;
            Item = new DbSourceDefinition
            {
                AuthenticationType = _dbSource.AuthenticationType,
                DbName = _dbSource.DbName,
                Id = _dbSource.Id,
                Name = _dbSource.Name,
                Password = _dbSource.Password,
                Path = _dbSource.Path,
                ServerName = _dbSource.ServerName,
                UserName = _dbSource.UserName,
                Type = _dbSource.Type
            };
            GetLoadComputerNamesTask(() =>
            {
                FromDbSource();
                SetupHeaderTextFromExisting();
            });
            
        }

        void SetupHeaderTextFromExisting()
        {
            if (_warewolfserverName != null)
            {
            }
            HeaderText = Resources.Languages.Core.DatabaseSourceServerEditHeaderLabel  + (_dbSource == null ? ResourceName : _dbSource.Name).Trim();
           
            Header = ((_dbSource == null ? ResourceName : _dbSource.Name));
        }

        public override bool CanSave()
        {
            return TestPassed && !String.IsNullOrEmpty(DatabaseName);
        }

        public bool CanTest()
        {
            if (Testing)
                return false;
            if (ServerName != null && String.IsNullOrEmpty(ServerName.Name))
            {
                return false;
            }
            if (AuthenticationType == AuthenticationType.User)
            {
                return !String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password);
            }
            return true;
        }


        public void FromDbSource()
        {
            ResourceName = _dbSource.Name;
            AuthenticationType = _dbSource.AuthenticationType;
            UserName = _dbSource.UserName;
            ServerName = ComputerNames.FirstOrDefault(name => _dbSource.ServerName == name.Name);
            Password = _dbSource.Password;
            TestConnection();
            DatabaseName = _dbSource.DbName;
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
                if (!String.IsNullOrEmpty(value))
                {
                    SetupHeaderTextFromExisting();
                }
                OnPropertyChanged(_resourceName);
            }
        }

        public bool UserAuthenticationSelected
        {
            get { return AuthenticationType == AuthenticationType.User; }
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
                OnPropertyChanged(() => ComputerNames);
            }
        }

        void SaveConnection()
        {
            if (_dbSource == null)
            {
                var res = RequestServiceNameViewModel.ShowSaveDialog();

                if (res == MessageBoxResult.OK)
                {
                    _resourceName = RequestServiceNameViewModel.ResourceName.Name;
                    var src = ToDbSource();
                    src.Path = RequestServiceNameViewModel.ResourceName.Path ?? RequestServiceNameViewModel.ResourceName.Name;
                    Save(src);
                    _dbSource = src;
                    Item = src;
                    SetupHeaderTextFromExisting();
                 //   ResourceName = RequestServiceNameViewModel.ResourceName.Name;
                }
            }
            else
            {
                Save(ToDbSource());
            }
        }

        void Save(IDbSource toDbSource)
        {
            _updateManager.Save(toDbSource);
            Item = toDbSource;

        }



        void TestConnection()
        {

            _token = new CancellationTokenSource();
            AsyncWorker.Start(SetupProgressSpinner, a =>
            {
                DatabaseNames = a;
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
                DatabaseNames.Clear();
            });

            OnPropertyChanged(() => DatabaseNames);
        }

        IList<string> SetupProgressSpinner()
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                Testing = true;
                TestFailed = false;
                TestPassed = false;
            });
            return _updateManager.TestDbConnection(ToNewDbSource());
        }

        IDbSource ToNewDbSource()
        {
            return new DbSourceDefinition
            {
                AuthenticationType = AuthenticationType,
                ServerName = GetServerName(),
                Password = Password,
                UserName = UserName,
                Type = (enSourceType)Enum.Parse(typeof( enSourceType), ServerType.Value),
                Name = ResourceName,
                DbName = DatabaseName,
                Id = _dbSource == null ? Guid.NewGuid() : _dbSource.Id
            };
        }

        private string GetServerName()
        {
            string serverName =null ;
            if (ServerName != null)
            {
                serverName = ServerName.Name;
            }
            return serverName;
        }

        IDbSource ToDbSource()
        {
            if (_dbSource == null)
                return new DbSourceDefinition
                {
                    AuthenticationType = AuthenticationType,
                    ServerName = GetServerName(),
                    Password = Password,
                    UserName = UserName,
                    Type = (enSourceType)Enum.Parse(typeof( enSourceType), ServerType.Value),
                    Name = ResourceName,
                    DbName = DatabaseName,
                    Id = _dbSource == null ? Guid.NewGuid() : _dbSource.Id
                };
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                return new DbSourceDefinition
                {
                    AuthenticationType = AuthenticationType,
                    ServerName = GetServerName(),
                    Password = Password,
                    UserName = UserName,
                    Type = (enSourceType)Enum.Parse(typeof(enSourceType), ServerType.Value),
                    Name = ResourceName,
                    DbName = DatabaseName,
                    Id = _dbSource == null ? Guid.NewGuid() : _dbSource.Id
                };

            }
        }
        public override IDbSource ToModel()
        {
            if (Item == null)
            {
                Item = ToDbSource();
                return Item;
            }

            return new DbSourceDefinition
            {
                AuthenticationType = AuthenticationType,
                ServerName = GetServerName(),
                Password = Password,
                UserName = UserName,
                Type = (enSourceType)Enum.Parse(typeof(enSourceType), ServerType.Value),
                Name = ResourceName,
                DbName = DatabaseName,
                Id = _dbSource == null ? Guid.NewGuid() : _dbSource.Id
            };
        }

        IRequestServiceNameViewModel RequestServiceNameViewModel { get; set; }

        public IList<NameValue> Types { get; set; }

        public NameValue ServerType
        {
            get { return _serverType; }
            set
            {
                _serverType = value;
                OnPropertyChanged(() => ServerType);
                OnPropertyChanged(() => Header);
            }
        }

        public AuthenticationType AuthenticationType
        {
            get { return _authenticationType; }
            set
            {
                if (_authenticationType != value)
                {
                    _authenticationType = value;
                    if (_dbSource != null && _authenticationType == AuthenticationType.User && _authenticationType == _dbSource.AuthenticationType)
                    {    Password = _dbSource.Password;
                         UserName = _dbSource.UserName;
                    }
                    OnPropertyChanged(() => AuthenticationType);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => UserAuthenticationSelected);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
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
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {

                _databaseName = value;
                OnPropertyChanged(() => DatabaseName);
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

        public ICommand TestCommand { get; set; }

        public ICommand CancelTestCommand { get; set; }

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
                OnPropertyChanged(() => TestPassed);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);

            }


        }

        [ExcludeFromCodeCoverage]
        public string ServerTypeLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceTypeLabel;
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
                ViewModelUtils.RaiseCanExecuteChanged(CancelTestCommand);
                ViewModelUtils.RaiseCanExecuteChanged(TestCommand);
            }
        }

        [ExcludeFromCodeCoverage]
        public string ServerLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceServerLabel;
            }
        }

        [ExcludeFromCodeCoverage]
        public string DatabaseLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceDatabaseLabel;
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

        [ExcludeFromCodeCoverage]
        public string WindowsAuthenticationToolTip
        {
            get
            {
                return Resources.Languages.Core.WindowsAuthenticationToolTip;
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

        [ExcludeFromCodeCoverage]
        public string ServerTypeTool
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceTypeToolTip;
            }
        }

        public IList<string> DatabaseNames
        {
            get { return _databaseNames; }
            set
            {
                _databaseNames = value;
                OnPropertyChanged(() => DatabaseNames);
            }
        }

        public override void UpdateHelpDescriptor(string helpText)
            {
        }

        public bool IsEmpty { get { return ServerName != null && (String.IsNullOrEmpty(ServerName.Name) && AuthenticationType == AuthenticationType.Windows && String.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password)); } }


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

    public class ComputerName : IComputerName
    {
        public string Name { get; set; }
    }
}
