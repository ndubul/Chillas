using System;
using Dev2;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces;
using Dev2.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.Explorer
{    
    [Binding]    
    // ReSharper disable UnusedMember.Global
    public class ExplorerSteps      
    {
        [BeforeFeature("Explorer")]
        public static void SetupExplorerDependencies()
        {
            Utils.SetupResourceDictionary();
            var explorerView = new ExplorerView();
            var mockShellViewModel = new Mock<IShellViewModel>();
            var mockExplorerRepository = new Mock<IExplorerRepository>();
            mockExplorerRepository.Setup(repository => repository.CreateFolder(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()));
            mockExplorerRepository.Setup(repository => repository.Rename(It.IsAny<IExplorerItemViewModel>(), It.IsAny<string>())).Returns(true);
            mockShellViewModel.Setup(model => model.LocalhostServer).Returns(new ServerForTesting(mockExplorerRepository));
            var explorerViewModel = new ExplorerViewModel(mockShellViewModel.Object,new Mock<IEventAggregator>().Object );
            explorerView.DataContext = explorerViewModel;
            Utils.ShowTheViewForTesting(explorerView);
            FeatureContext.Current.Add(Utils.ViewNameKey, explorerView);
            FeatureContext.Current.Add(Utils.ViewModelNameKey, explorerViewModel);
        }
         
        [BeforeScenario("Explorer")]
        public void SetupForExplorer()
        {
            var explorerView = FeatureContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var explorerViewModel = FeatureContext.Current.Get<IExplorerViewModel>(Utils.ViewModelNameKey);
            ScenarioContext.Current.Add(Utils.ViewNameKey, explorerView);
            ScenarioContext.Current.Add(Utils.ViewModelNameKey, explorerViewModel);
            var mainViewModelMock = new Mock<IMainViewModel>();
            ScenarioContext.Current.Add("mainViewModel",mainViewModelMock);
        }

        [Given(@"the explorer is visible")]
        public void GivenTheExplorerIsVisible()
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            Assert.IsNotNull(explorerView);
            Assert.IsNotNull(explorerView.DataContext);            
        }

        [When(@"I Connected to Remote Server ""(.*)""")]
        public void WhenIConnectedToRemoteServer(string name)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            Assert.IsNotNull(explorerView.DataContext);    
            IExplorerViewModel explorerViewModel = (IExplorerViewModel)explorerView.DataContext;
            var server = new ServerForTesting(new Mock<IExplorerRepository>());
            server.ResourceName = name;
            //explorerViewModel.ConnectControlViewModel.Connect(server);            
        }

        [Given(@"I open ""(.*)"" server")]
        [When(@"I open ""(.*)"" server")]
        public void WhenIOpenServer(string serverName)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var environmentViewModel = explorerView.OpenEnvironmentNode(serverName);
            Assert.IsNotNull(environmentViewModel);
            Assert.IsTrue(environmentViewModel.IsExpanded);
        }

        [Given(@"I open ""(.*)""")]
        [When(@"I open ""(.*)""")]
        public void WhenIOpen(string folderName)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var environmentViewModel = explorerView.OpenFolderNode(folderName);
            Assert.IsNotNull(environmentViewModel);
        }

        [Given(@"I open '(.*)' in ""(.*)""")]
        [When(@"I open '(.*)' in ""(.*)""")]
        public void WhenIOpenIn(string resourceName,string folderName)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var environmentViewModel = explorerView.OpenItem(resourceName,folderName);
            Assert.IsNotNull(environmentViewModel);
        }


        [Then(@"""(.*)"" tab is opened")]
        public void WhenTabIsOpened(string resourceName)
        {
            
        }


        [Given(@"I should see ""(.*)"" folders")]
        [When(@"I should see ""(.*)"" folders")]
        [Then(@"I should see ""(.*)"" folders")]
        public void ThenIShouldSeeFolders(int numberOfFoldersVisible)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var explorerItemViewModels = explorerView.GetFoldersVisible();
            Assert.AreEqual(numberOfFoldersVisible,explorerItemViewModels.Count);
        }

        [Then(@"I should see ""(.*)"" children for ""(.*)""")]
        public void ThenIShouldSeeChildrenFor(int expectedChildrenCount, string folderName)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var childrenCount = explorerView.GetVisibleChildrenCount(folderName);
            Assert.AreEqual(expectedChildrenCount,childrenCount);
        }

        [When(@"I rename ""(.*)"" to ""(.*)""")]
        public void WhenIRenameTo(string originalFolderName, string newFolderName)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.PerformFolderRename(originalFolderName,newFolderName);            
        }

        [Then(@"Conflict error message is occurs")]
        public void ThenConflictErrorMessageIsOccurs()
        {
            //var boot = FeatureContext.Current.Get<UnityBootstrapperForExplorerTesting>("bootstrapper");
           // boot.PopupController.Verify(a => a.Show(It.IsAny<PopupMessage>()));
        }


        [Then(@"I should not see ""(.*)""")]
        public void ThenIShouldNotSee(string folderName)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var environmentViewModel = explorerView.OpenFolderNode(folderName);
            Assert.IsNull(environmentViewModel);
        }

        [Then(@"I should see ""(.*)"" only")]
        public void ThenIShouldSee(string folderName)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var environmentViewModel = explorerView.OpenFolderNode(folderName);
            Assert.IsNotNull(environmentViewModel);
        }

        [Then(@"I should see ""(.*)"" resources in ""(.*)""")]
        public void ThenIShouldSeeResourcesIn(int numberOfresources, string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var explorerItemViewModels = explorerView.GetResourcesVisible(path);
            Assert.AreEqual(numberOfresources, explorerItemViewModels);
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchTerm)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.PerformSearch(searchTerm);
        }

        [When(@"I search for ""(.*)"" in explorer")]
        public void WhenISearchForInExplorer(string searchTerm)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.PerformSearch(searchTerm);
        }

        [When(@"I clear ""(.*)"" Filter")]
        public void WhenIClearFilter(string p0)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.PerformSearch("");
        }

        [Then(@"I should see the path ""(.*)""")]
        public void ThenIShouldSeeThePath(string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.VerifyItemExists(path);
        }
        [Then(@"I should not see the path ""(.*)""")]
        public void ThenIShouldNotSeeThePath(string path)
        {
            bool found = false;
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey) as ExplorerView;
            try
            {
                // ReSharper disable once PossibleNullReferenceException
                explorerView.ExplorerViewTestClass.VerifyItemExists(path);
            }
            catch(Exception e)
            {
                if (e.Message.Contains("Folder or environment not found. Name"))
                    found = true;
            
            }
           Assert.IsTrue(found);
        }

        [Then(@"I setup (.*) resources in ""(.*)""")]
        public void ThenISetupResourcesIn(int resourceNumber, string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.AddResources(resourceNumber, path,"WorkflowService","Resource");
        }

        [When(@"I Add  ""(.*)"" ""(.*)"" to be returned for ""(.*)""")]
        public void WhenIAddToBeReturnedFor(int count, string type, string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.AddResources(count, path, type, "Resource");
        }

        [When(@"I Setup a resource  ""(.*)"" ""(.*)"" to be returned for ""(.*)"" called ""(.*)""")]
        public void WhenISetupAResourceToBeReturnedForCalled(int count, string type, string path, string name)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.AddResources(count, path, type,name);
        }

        [Then(@"""(.*)"" Context menu  should be ""(.*)"" for ""(.*)""")]
        public void ThenContextMenuShouldBeFor(string option, string visibility, string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey) as ExplorerView;
            // ReSharper disable PossibleNullReferenceException
            explorerView.ExplorerViewTestClass.VerifyContextMenu(option, visibility, path);
            // ReSharper restore PossibleNullReferenceException
        }

        [Then(@"I Create ""(.*)"" resources of Type ""(.*)"" in ""(.*)""")]
        public void ThenICreateResourcesOfTypeIn(int resourceNumber, string type, string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.AddResources(resourceNumber, path, type, "Resource");
        }

        [When(@"I Show Version History for ""(.*)""")]
        public void WhenIShowVersionHistoryFor(string path)
        {
//            var explorerView = ScenarioContext.Current.Get<IExplorerView>("explorerView") as ExplorerView;
//            var boot = FeatureContext.Current.Get<UnityBootstrapperForExplorerTesting>("bootstrapper");            
//          
//            if(explorerView != null)
//            {
//                // ReSharper disable MaximumChainedReferences
//                boot.ExplorerRepository.Setup(a => a.GetVersions(It.IsAny<Guid>())).Returns(new List<IVersionInfo>
//                {
//                    new VersionInfo(DateTime.Now,"bob","Leon","3",Guid.Empty,Guid.Empty),
//                    new VersionInfo(DateTime.Now,"bob","Leon","2",Guid.Empty,Guid.Empty),
//                    new VersionInfo(DateTime.Now,"bob","Leon","1",Guid.Empty,Guid.Empty)
//                });
//                // ReSharper restore MaximumChainedReferences
//                explorerView.ExplorerViewTestClass.ShowVersionHistory(path);
//            }
        }

        [Then(@"I should see ""(.*)"" versions with ""(.*)"" Icons in ""(.*)""")]
        public void ThenIShouldSeeVersionsWithIconsIn(int count, string iconVisible, string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey) as ExplorerView;
            if(explorerView != null)
            {
                var node = explorerView.ExplorerViewTestClass.VerifyItemExists(path);
                Assert.AreEqual(node.Nodes.Count,count);
                foreach(var node1 in node.Nodes)
                {
                    var itm = node1.Data as ExplorerItemViewModel;
                    // ReSharper disable PossibleNullReferenceException
                    Assert.IsFalse(itm.CanExecute);
                    Assert.AreEqual(itm.ResourceType,ResourceType.Version);
                    Assert.IsFalse(itm.CanEdit);
                    // ReSharper restore PossibleNullReferenceException
                }
            }
        }

        [When(@"I Make ""(.*)"" the current version of ""(.*)""")]
        public void WhenIMakeTheCurrentVersionOf(string versionPath, string resourcePath)
        {
//             var boot = FeatureContext.Current.Get<UnityBootstrapperForExplorerTesting>("bootstrapper");
//            // ReSharper disable once MaximumChainedReferences
//             boot.ExplorerRepository.Setup(a => a.Rollback(Guid.Empty, "1")).Returns(new RollbackResult
//             {
//                    DisplayName = "Resource 1" , 
//                     VersionHistory = new List<IExplorerItem>()
//                 }
//             );
//            // ReSharper disable once MaximumChainedReferences
//             boot.ExplorerRepository.Setup(a => a.GetVersions(It.IsAny<Guid>())).Returns(new List<IVersionInfo>
//             {
//                     new VersionInfo(DateTime.Now,"bob","Leon","4",Guid.Empty,Guid.Empty),
//                    new VersionInfo(DateTime.Now,"bob","Leon","3",Guid.Empty,Guid.Empty),
//                    new VersionInfo(DateTime.Now,"bob","Leon","2",Guid.Empty,Guid.Empty),
//                    new VersionInfo(DateTime.Now,"bob","Leon","1",Guid.Empty,Guid.Empty)
//                });
//            var explorerView = ScenarioContext.Current.Get<IExplorerView>("explorerView") as ExplorerView;
//            if(explorerView != null)
//            {
//                var tester = explorerView.ExplorerViewTestClass;
//                tester.PerformVersionRollback(versionPath);
//            }
        }

        [When(@"I Delete Version ""(.*)""")]
        public void WhenIDeleteVersion(string versionPath)
        {
//            var boot = FeatureContext.Current.Get<UnityBootstrapperForExplorerTesting>("bootstrapper");
//            boot.ExplorerRepository.Setup(a => a.Delete(It.IsAny<IExplorerItemViewModel>()));
//            // ReSharper disable once MaximumChainedReferences
//            boot.PopupController.Setup(a => a.Show(It.IsAny<IPopupMessage>())).Returns(MessageBoxResult.OK);
//            // ReSharper disable once MaximumChainedReferences
//            boot.ExplorerRepository.Setup(a => a.GetVersions(It.IsAny<Guid>())).Returns(new List<IVersionInfo>
//             {
//                    new VersionInfo(DateTime.Now,"bob","Leon","3",Guid.Empty,Guid.Empty),
//                    new VersionInfo(DateTime.Now,"bob","Leon","2",Guid.Empty,Guid.Empty),
//                    new VersionInfo(DateTime.Now,"bob","Leon","1",Guid.Empty,Guid.Empty)
//              });
//
//            var explorerView = ScenarioContext.Current.Get<IExplorerView>("explorerView") as ExplorerView;
//            if (explorerView != null)
//            {
//                var tester = explorerView.ExplorerViewTestClass;
//                tester.PerformVersionDelete(versionPath);
//            }    
        }

        [Then(@"I Setup  ""(.*)"" Versions to be returned for ""(.*)""")]
        public void ThenISetupVersionsToBeReturnedFor(int count, string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey) as ExplorerView;
            // ReSharper disable once PossibleNullReferenceException
            var tester = explorerView.ExplorerViewTestClass;
            tester.CreateChildNodes(count, path);
            ScenarioContext.Current.Add("versions", count);
        }

        [Then(@"I Setup  ""(.*)"" resources of Type ""(.*)"" in ""(.*)""")]
        public void ThenISetupResourcesOfTypeIn(int count, string path, string type)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I delete ""(.*)"" in ""(.*)"" server")]
        public void WhenIDeleteInServer(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I delete ""(.*)""")]
        public void WhenIDelete(string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            var mainViewModelMock = ScenarioContext.Current.Get<Mock<IMainViewModel>>("mainViewModel");
            
            if (ScenarioContext.Current.ContainsKey("popupResult"))
            {
                var popupResult = ScenarioContext.Current.Get<string>("popupResult");
                if (popupResult.ToLower() == "cancel")
                {
                    mainViewModelMock.Setup(model => model.ShowDeleteDialogForFolder(It.IsAny<string>())).Returns(false);
                }
                else
                    // ReSharper disable once MaximumChainedReferences
                    mainViewModelMock.Setup(model => model.ShowDeleteDialogForFolder(It.IsAny<string>())).Returns(true);
            }
            else
            {
                mainViewModelMock.Setup(model => model.ShowDeleteDialogForFolder(It.IsAny<string>())).Returns(true);
            }
            CustomContainer.DeRegister<IMainViewModel>();
            CustomContainer.Register(mainViewModelMock.Object);
            explorerView.DeletePath(path);
        }

        [Then(@"I choose to ""(.*)"" Any Popup Messages")]
        public void ThenIChooseToAnyPopupMessages(string result)
        {
            if (!ScenarioContext.Current.ContainsKey("popupResult")) 
                    ScenarioContext.Current.Add("popupResult",result);
            else
            {
                ScenarioContext.Current["popupResult"] = result;
            }
        }

        [When(@"I create ""(.*)""")]
        public void WhenICreate(string path)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.AddNewFolderFromPath(path);
        }
        [When(@"I add ""(.*)"" in ""(.*)""")]
        public void WhenIAddIn(string folder , string server)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.AddNewFolder(server, folder);
        }

        [Given(@"I create the ""(.*)"" of type ""(.*)""")]
        [When(@"I create the ""(.*)"" of type ""(.*)""")]
        [Then(@"I create the ""(.*)"" of type ""(.*)""")]
        public void WhenICreateTheOfType(string path, string type)
        {
            var explorerView = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            explorerView.AddNewResource(path, type);
        }

        [AfterScenario("Explorer")]
        public void AfterScenario()
        {
            var mockShellViewModel = new Mock<IShellViewModel>();
            var mockExplorerRepository = new Mock<IExplorerRepository>();
            mockExplorerRepository.Setup(repository => repository.CreateFolder(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()));
            mockExplorerRepository.Setup(repository => repository.Rename(It.IsAny<IExplorerItemViewModel>(), It.IsAny<string>())).Returns(true);
            mockShellViewModel.Setup(model => model.LocalhostServer).Returns(new ServerForTesting(mockExplorerRepository));
            var explorerViewModel = new ExplorerViewModel(mockShellViewModel.Object, new Mock<IEventAggregator>().Object);
            var view = ScenarioContext.Current.Get<IExplorerView>(Utils.ViewNameKey);
            view.DataContext = explorerViewModel;
            FeatureContext.Current.Remove(Utils.ViewModelNameKey);
            FeatureContext.Current.Add(Utils.ViewModelNameKey, explorerViewModel);
        }

    }
}
// ReSharper restore UnusedMember.Global