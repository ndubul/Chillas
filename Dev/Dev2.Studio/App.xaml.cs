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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Help;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Common.Wrappers;
using Dev2.CustomControls.Progress;
using Dev2.Diagnostics.Debug;
using Dev2.Instrumentation;
using Dev2.Studio.Core;
using Microsoft.Practices.Prism.PubSubEvents;
using Warewolf.Core;
using Warewolf.Studio.Models.Help;
using Warewolf.Studio.Models.Toolbox;
using Warewolf.Studio.ViewModels.Help;
using Warewolf.Studio.ViewModels.ToolBox;
// ReSharper disable RedundantUsingDirective
using Dev2.Interfaces;
using Dev2.Utils;
using log4net.Config;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;
using Dev2.Studio.Core.AppResources.Browsers;
using Dev2.Studio.Core.Helpers;
using Dev2.Studio.Diagnostics;
using Dev2.Studio.ViewModels;
using Dev2.Util;
using Dev2.Views.Dialogs;


// ReSharper restore RedundantUsingDirective

// ReSharper disable CheckNamespace
namespace Dev2.Studio
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IApp
    {
        MainViewModel _mainViewModel;
        //This is ignored because when starting the studio twice the second one crashes without this line
        // ReSharper disable RedundantDefaultFieldInitializer
        // ReSharper disable NotAccessedField.Local
        // ReSharper disable RedundantDefaultFieldInitializer
        private Mutex _processGuard = null;
        // ReSharper restore RedundantDefaultFieldInitializer
        // ReSharper restore NotAccessedField.Local
        private AppExceptionHandler _appExceptionHandler;
        private bool _hasShutdownStarted;

        public App()
        {
            // PrincipalPolicy must be set to WindowsPrincipal to check roles.
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            _hasShutdownStarted = false;
            ShouldRestart = false;
            InitializeComponent();
            AppSettings.LocalHost = ConfigurationManager.AppSettings["LocalHostServer"];
        }

        public static bool IsAutomationMode
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        [PrincipalPermission(SecurityAction.Demand)]  // Principal must be authenticated
        protected override void OnStartup(StartupEventArgs e)
        {
            Tracker.StartStudio();
            bool createdNew;

            Task.Factory.StartNew(() =>
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Warewolf", "Feedback");
                    DirectoryHelper.CleanUp(path);
                    DirectoryHelper.CleanUp(Path.Combine(Path.GetTempPath(), "Warewolf", "Debug"));
                });

            // ReSharper disable once UnusedVariable
            var localprocessGuard = e.Args.Length > 0
                                        ? new Mutex(true, e.Args[0], out createdNew)
                                        : new Mutex(true, "Warewolf Studio", out createdNew);

            if (createdNew)
            {
                _processGuard = localprocessGuard;
            }
            else
            {
                Environment.Exit(Environment.ExitCode);
            }

         

            
            InitializeShell(e);
#if ! (DEBUG)
            var versionChecker = new VersionChecker();
            if(versionChecker.GetNewerVersion())
            {
                WebLatestVersionDialog dialog = new WebLatestVersionDialog();
                dialog.ShowDialog();
            }
#endif
        }

        public static ISplashView _splashView;

        private ManualResetEvent ResetSplashCreated;
        private Thread SplashThread;
        protected void InitializeShell(StartupEventArgs e)
        {
            ResetSplashCreated = new ManualResetEvent(false);

            SplashThread = new Thread(ShowSplash);
            SplashThread.SetApartmentState(ApartmentState.STA);
            SplashThread.IsBackground = true;
            SplashThread.Name = "Splash Screen";
            SplashThread.Start();
            ResetSplashCreated.WaitOne();
            new Bootstrapper().Start();
            
            base.OnStartup(e);
            _mainViewModel = MainWindow.DataContext as MainViewModel;
            if(_mainViewModel != null)
            {
               
                _splashView.CloseSplash();
                var settingsConfigFile = HelperUtils.GetStudioLogSettingsConfigFile();
                if (!File.Exists(settingsConfigFile))
                {
                    File.WriteAllText(settingsConfigFile, GlobalConstants.DefaultStudioLogFileConfig);
                }
                XmlConfigurator.ConfigureAndWatch(new FileInfo(settingsConfigFile));
                _appExceptionHandler = new AppExceptionHandler(this, _mainViewModel);
            }
            //MainWindow.Show();
            
        }


        private void ShowSplash()
        {
            // Create the window 

            var server = new Warewolf.Studio.AntiCorruptionLayer.Server(EnvironmentRepository.Instance.Source);
            server.Connect();
            CustomContainer.Register<IServer>(server);
            var toolBoxViewModel = new ToolboxViewModel(new ToolboxModel(server, server, null), new ToolboxModel(server, server, null));
            CustomContainer.Register<IToolboxViewModel>(toolBoxViewModel);
            var helpViewModel = new HelpWindowViewModel(new HelpDescriptorViewModel(new HelpDescriptor("", "<body>This is the default help</body>", null)), new HelpModel(new EventAggregator()));
            CustomContainer.Register<IHelpWindowViewModel>(helpViewModel);
            CustomContainer.Register<IEventAggregator>(new EventAggregator());
            var splashViewModel = new SplashViewModel(server, new ExternalProcessExecutor());

            SplashPage splashPage = new SplashPage();
            splashPage.DataContext = splashViewModel;
            _splashView = splashPage;
            // Show it 
            _splashView.Show(false);
            
            // Now that the window is created, allow the rest of the startup to run 
            if (ResetSplashCreated != null)
            {
                ResetSplashCreated.Set();
            }
            splashViewModel.ShowServerVersion();
            System.Windows.Threading.Dispatcher.Run();
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Tracker.Stop();

            // this is already handled ;)
            if (_mainViewModel != null)
            {
                _mainViewModel.PersistTabs(true);
            }
            ProgressFileDownloader.PerformCleanup(new DirectoryWrapper(), GlobalConstants.VersionDownloadPath, new FileWrapper());
            HasShutdownStarted = true;
            DebugDispatcher.Instance.Shutdown();
            try
            {
                base.OnExit(e);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // Best effort ;)
            }

            ForceShutdown();
        }

        void ForceShutdown()
        {
            if (ShouldRestart)
            {
                Task.Run(() => Process.Start(ResourceAssembly.Location, Guid.NewGuid().ToString()));
            }
            Environment.Exit(0);
        }

        #region Implementation of IApp

        #region Implementation of IApp

        public new void Shutdown()
        {
            try
            {
                base.Shutdown();
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // Best effort ;)
            }
            ForceShutdown();
        }

        #endregion

        #endregion

        public bool ShouldRestart { get; set; }

        public bool HasShutdownStarted
        {
            get
            {
                return Dispatcher.CurrentDispatcher.HasShutdownStarted || Dispatcher.CurrentDispatcher.HasShutdownFinished || _hasShutdownStarted;
            }
            set
            {
                _hasShutdownStarted = value;
            }
        }

        private void OnApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Tracker.TrackException(GetType().Name, "OnApplicationDispatcherUnhandledException", e.Exception);
            if (_appExceptionHandler != null)
            {
                e.Handled = HasShutdownStarted || _appExceptionHandler.Handle(e.Exception);
            }
            else
            {
                MessageBox.Show("Fatal Error : " + e.Exception);
            }
        }
    }
}
