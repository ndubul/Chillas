using System;
using System.Collections.Generic;
using System.Windows;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Threading;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;
using Warewolf.Studio.Core.Infragistics_Prism_Region_Adapter;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

// ReSharper disable InconsistentNaming
namespace Warewolf.AcceptanceTesting.PluginSource
{
    [Binding]
    public class PluginSourceSteps
    {
        [BeforeFeature("PluginSource")]
        public static void SetupForSystem()
        {
            Utils.SetupResourceDictionary();
            var sourceControl = new ManagePluginSourceControl();
            var mockStudioUpdateManager = new Mock<IManagePluginSourceModel>();
            mockStudioUpdateManager.Setup(model => model.ServerName).Returns("localhost");
            mockStudioUpdateManager.Setup(model => model.GetDllListings(null)).Returns(BuildBaseListing());
            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockExecutor = new Mock<IExternalProcessExecutor>();

            var viewModel = new ManagePluginSourceViewModel(mockStudioUpdateManager.Object, mockRequestServiceNameViewModel.Object, mockEventAggregator.Object, new SynchronousAsyncWorker());
            sourceControl.DataContext = viewModel;
            Utils.ShowTheViewForTesting(sourceControl);
            FeatureContext.Current.Add(Utils.ViewNameKey, sourceControl);
            FeatureContext.Current.Add(Utils.ViewModelNameKey, viewModel);
            FeatureContext.Current.Add("updateManager", mockStudioUpdateManager);
            FeatureContext.Current.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);
            FeatureContext.Current.Add("externalProcessExecutor", mockExecutor);
        }

        static IList<IDllListing> BuildBaseListing()
        {
            var listing = new List<IDllListing>();
            var fileSystemListing = new DllListing{FullName = "",IsDirectory = true,Name = "File System"};
            var gacListing = new DllListing{FullName = "",IsDirectory = true,Name = "GAC"};
            var cDrive = new DllListing
            {
                FullName = "C:\\", IsDirectory = true, Name = "C:\\",
                Children = new List<IDllListing>
                {
                    new DllListing { 
                        FullName = "C:\\Development", IsDirectory = true, Name = "Development" ,
                        Children = new List<IDllListing>
                        {
                            new DllListing
                            {
                                FullName = "C:\\Development\\Dev", IsDirectory = true, Name = "Dev",
                                Children = new List<IDllListing>
                                {
                                    new DllListing
                                    {
                                        FullName = "C:\\Development\\Dev\\Binaries", IsDirectory = true, Name = "Binaries",
                                        Children = new List<IDllListing>
                                        {
                                            new DllListing
                                            {
                                                FullName = "C:\\Development\\Dev\\Binaries\\MS Fakes", IsDirectory = true, Name = "MS Fakes",
                                                Children = new List<IDllListing>
                                                {
                                                    new DllListing
                                                    {
                                                        FullName = "C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools.Testing.Fakes.dll", IsDirectory = false, Name = "Microsoft.QualityTools.Testing.Fakes.dll"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var dDrive = new DllListing{FullName = "D:\\",IsDirectory = true,Name = "D:\\"};
            fileSystemListing.Children = new List<IDllListing>{cDrive,dDrive};
            listing.Add(fileSystemListing);
            listing.Add(gacListing);
            return listing;
        }

        [BeforeScenario("PluginSource")]
        public void SetupForPluginSource()
        {
            ScenarioContext.Current.Add(Utils.ViewNameKey, FeatureContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey));
            ScenarioContext.Current.Add("updateManager", FeatureContext.Current.Get<Mock<IManagePluginSourceModel>>("updateManager"));
            ScenarioContext.Current.Add("requestServiceNameViewModel", FeatureContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
            ScenarioContext.Current.Add(Utils.ViewModelNameKey, FeatureContext.Current.Get<ManagePluginSourceViewModel>(Utils.ViewModelNameKey));
        }

        [Given(@"I open New Plugin Source")]
        public void GivenIOpenNewPluginSource()
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            Assert.IsNotNull(sourceControl);
            Assert.IsNotNull(sourceControl.DataContext); 
        }

        [Then(@"""(.*)"" tab is opened")]
        public void ThenTabIsOpened(string headerText)
        {
            var viewModel = ScenarioContext.Current.Get<IDockAware>("viewModel");
            Assert.AreEqual(headerText, viewModel.Header);
        }

        [Then(@"title is ""(.*)""")]
        public void ThenTitleIs(string title)
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            var viewModel = ScenarioContext.Current.Get<ManagePluginSourceViewModel>("viewModel");
            Assert.AreEqual(title, viewModel.HeaderText);
            Assert.AreEqual(title, sourceControl.GetHeaderText());
        }

        [Given(@"""(.*)"" is ""(.*)""")]
        [When(@"""(.*)"" is ""(.*)""")]
        [Then(@"""(.*)"" is ""(.*)""")]
        public void GivenIs(string controlName, string enabledString)
        {
            Utils.CheckControlEnabled(controlName, enabledString, ScenarioContext.Current.Get<ICheckControlEnabledView>(Utils.ViewNameKey));
        }

        [Then(@"I click ""(.*)""")]
        public void ThenIClick(string itemName)
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            var dllListingModel = sourceControl.SelectItem(itemName);
            Assert.IsNotNull(dllListingModel);
            var viewModel = ScenarioContext.Current.Get<ManagePluginSourceViewModel>("viewModel");
            Assert.AreEqual(dllListingModel,viewModel.SelectedDll);
            Assert.IsTrue(viewModel.SelectedDll.IsExpanded);
        }

        [Then(@"local drive ""(.*)"" is visible")]
        public void ThenLocalDriveIsVisible(string itemName)
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            var isItemVisible = sourceControl.IsItemVisible(itemName);
            Assert.IsTrue(isItemVisible);
        }

        [When(@"I click")]
        public void WhenIClick(Table table)
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            foreach(var row in table.Rows)
            {
                var dllListingModel = sourceControl.SelectItem(row["Clicks"]);
                Assert.IsNotNull(dllListingModel);
            }
        }

        [Given(@"Assembly value is ""(.*)""")]
        [When(@"Assembly value is ""(.*)""")]
        [Then(@"Assembly value is ""(.*)""")]
        public void ThenAssemblyValueIs(string assemblyName)
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            var assemblyNameOnControl = sourceControl.GetAssemblyName();
            var isSameAsControl = assemblyName.Equals(assemblyNameOnControl, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(isSameAsControl);
            var viewModel = ScenarioContext.Current.Get<ManagePluginSourceViewModel>("viewModel");
            var assemblyNameOnViewModel = viewModel.SelectedDll.FullName;
            var isSameAsViewModel = assemblyName.Equals(assemblyNameOnViewModel, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(isSameAsViewModel);
        }

        [When(@"I change Assembly to ""(.*)""")]
        public void WhenIChangeAssemblyTo(string assemblyName)
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            sourceControl.SetAssemblyName(assemblyName);
            var viewModel = ScenarioContext.Current.Get<ManagePluginSourceViewModel>("viewModel");
            var assemblyNameOnViewModel = viewModel.AssemblyName;
            var isSameAsViewModel = assemblyName.Equals(assemblyNameOnViewModel, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(isSameAsViewModel);
        }


        [Given(@"file is selected")]
        public void GivenFileIsSelected()
        {
            ScenarioContext.Current.Pending();
        }


        [When(@"I save the source")]
        public void WhenISaveTheSource()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
            var manageWebserviceSourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            manageWebserviceSourceControl.PerformSave();

        }

        [Then(@"the save dialog is opened")]
        public void ThenTheSaveDialogIsOpened()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify();
        }

        [When(@"I save as ""(.*)""")]
        public void WhenISaveAs(string resourceName)
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ResourceName).Returns(new ResourceName("", resourceName));
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Returns(MessageBoxResult.OK);
            var manageWebserviceSourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            manageWebserviceSourceControl.PerformSave();

        }
        [Given(@"local drive ""(.*)"" is visible in File window")]
        public void GivenLocalDriveIsVisibleInFileWindow(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"local drive ""(.*)"" is visible in ""(.*)"" window")]
        public void GivenLocalDriveIsVisibleInWindow(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"GAC is selected")]
        public void GivenGACIsSelected()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"file is not selected")]
        public void GivenFileIsNotSelected()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"Assembly is ""(.*)""")]
        public void GivenAssemblyIs(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I open ""(.*)"" plugin source")]
        public void GivenIOpenPluginSource(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"GAC is not selected")]
        public void GivenGACIsNotSelected()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I open ""(.*)""")]
        public void WhenIOpen(string itemNameToOpen)
        {
            var sourceControl = ScenarioContext.Current.Get<ManagePluginSourceControl>(Utils.ViewNameKey);
            sourceControl.OpenItem(itemNameToOpen);
        }

        [When(@"GAC is not selected")]
        public void WhenGACIsNotSelected()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"Assembly is ""(.*)""")]
        public void WhenAssemblyIs(string p0)
        {
            ScenarioContext.Current.Pending();
        }


        [When(@"I save Plugin source")]
        public void WhenISavePluginSource()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I Search for ""(.*)""")]
        public void WhenISearchFor(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"files in ""(.*)"" is opened")]
        public void ThenFilesInIsOpened(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"GAC is not selected")]        
        public void ThenGACIsNotSelected()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Assembly is ""(.*)""")]
        public void ThenAssemblyIs(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
