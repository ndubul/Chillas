using System;
using System.Windows;
using System.Windows.Threading;
using Dev2.Common.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using Warewolf.Studio.Themes.Luna;
// ReSharper disable ObjectCreationAsStatement

namespace Warewolf.AcceptanceTesting.Core
{
    public static class Utils
    {
        public const string ViewNameKey = "view";
        public const string ViewModelNameKey = "viewModel";

        public static void ShowTheViewForTesting(IView view)
        {
            var window = new Window {Content = view};
            var app = Application.Current;
            
            app.MainWindow = window;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                var manageDatabaseSourceControl = Application.Current.MainWindow.Content as IView;
                Assert.IsNotNull(manageDatabaseSourceControl);
                Assert.IsNotNull(manageDatabaseSourceControl.DataContext);
                Application.Current.Shutdown();
            }));

            Application.Current.Run(Application.Current.MainWindow);
        }

        public static void SetupResourceDictionary()
        {
            new LunaTheme();
            Application app = Application.Current ?? new Application();
            ResourceDictionary themeDictionary = new ResourceDictionary { Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/Theme.xaml", UriKind.RelativeOrAbsolute) };
            //            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Themes/DockManagerGeneric.xaml");
            //            app.Resources.MergedDictionaries.Add(themeDictionary);

            //            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Themes/DataTemplates.xaml");
            //            app.Resources.MergedDictionaries.Add(themeDictionary);
            //            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Resources/ResourceDictionary.xaml");
            //            app.Resources.MergedDictionaries.Add(themeDictionary);
            //            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Themes/ActivityStyles.xaml");
            //            app.Resources.MergedDictionaries.Add(themeDictionary);
            foreach(var resourceDictionary in themeDictionary.MergedDictionaries)
            {
                app.Resources.MergedDictionaries.Add(resourceDictionary);
                var resourceDictionaries = resourceDictionary.MergedDictionaries;
                if(resourceDictionaries.Count > 0)
                {
                    foreach (var innerResourceDictionary in resourceDictionaries)
                    {
                        app.Resources.MergedDictionaries.Add(innerResourceDictionary);
                    }
                }
            }


            
            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/Images.xaml", UriKind.RelativeOrAbsolute);
            app.Resources.MergedDictionaries.Add(themeDictionary);
            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/Common/Styles.xaml", UriKind.RelativeOrAbsolute);
            app.Resources.MergedDictionaries.Add(themeDictionary);
            //themeDictionary.Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/Common/generic.xaml", UriKind.RelativeOrAbsolute);
            //app.Resources.MergedDictionaries.Add(themeDictionary);
            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Resources/Converters.xaml", UriKind.RelativeOrAbsolute);
            app.Resources.MergedDictionaries.Add(themeDictionary);
        }


        public static void SetupResourceDictionaryActivities()
        {
            new LunaTheme();
            Application app = Application.Current ?? new Application();
            ResourceDictionary themeDictionary = new ResourceDictionary { Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/Theme.xaml", UriKind.RelativeOrAbsolute) };
            //            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Themes/DockManagerGeneric.xaml");
            //            app.Resources.MergedDictionaries.Add(themeDictionary);

            //            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Themes/DataTemplates.xaml");
            //            app.Resources.MergedDictionaries.Add(themeDictionary);
            //            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf Studio;component/Resources/ResourceDictionary.xaml");
            //            app.Resources.MergedDictionaries.Add(themeDictionary);
            app.Resources.MergedDictionaries.Add(themeDictionary);
  

            foreach (var resourceDictionary in themeDictionary.MergedDictionaries)
            {
                app.Resources.MergedDictionaries.Add(resourceDictionary);
                var resourceDictionaries = resourceDictionary.MergedDictionaries;
                if (resourceDictionaries.Count > 0)
                {
                    foreach (var innerResourceDictionary in resourceDictionaries)
                    {
                        app.Resources.MergedDictionaries.Add(innerResourceDictionary);
                    }
                }
            }

                      themeDictionary.Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/ActivityStyles/ActivityStyles.xaml", UriKind.RelativeOrAbsolute);
            app.Resources.MergedDictionaries.Add(themeDictionary);
            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/ActivityStyles/ActivityDesignerStyles.xaml", UriKind.RelativeOrAbsolute);
            app.Resources.MergedDictionaries.Add(themeDictionary);
            themeDictionary.Source = new Uri("pack://application:,,,/Warewolf.Studio.Themes.Luna;component/ActivityStyles/ActivityResources.xaml", UriKind.RelativeOrAbsolute);
            app.Resources.MergedDictionaries.Add(themeDictionary);



        }

        public static T GetViewModel<T>()
        {
            var viewModel = ScenarioContext.Current.Get<T>(ViewModelNameKey);
            return viewModel;
        }

        public static T GetView<T>()
        {
            var view = ScenarioContext.Current.Get<T>(ViewNameKey);
            return view;
        }

        public static void CheckControlEnabled(string controlName, string enabledString, ICheckControlEnabledView view)
        {
            var isEnabled = String.Equals(enabledString, "Enabled", StringComparison.InvariantCultureIgnoreCase);
            var controlEnabled = view.GetControlEnabled(controlName);
            Assert.AreEqual(isEnabled, controlEnabled);
        }
    }
}