﻿using System;
using Dev2.Studio.Core.AppResources.Browsers;
using Dev2.Studio.Factory;
using System.Windows;
using System.Windows.Threading;

namespace Dev2.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

#if DEBUG
        public static bool IsAutomationMode
        {
            get
            {
                return true;
            }           
        }
#else

        public static bool IsAutomationMode
        {
            get
            {
                return false;
            }
        }
#endif

        protected override void OnStartup(StartupEventArgs e) 
        {
            Browser.Startup();

            new Bootstrapper().Start();
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Browser.Shutdown();
            base.OnExit(e);
        }

        private void OnApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
        {
            ExceptionFactory.CreateViewModel(e.Exception).Show();
                e.Handled = true;
            //TODO Log
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "An unexpected unrecoverable exception has been encountered. The application will now shut down;");
                Current.Shutdown();
            }
        }
    }
}
