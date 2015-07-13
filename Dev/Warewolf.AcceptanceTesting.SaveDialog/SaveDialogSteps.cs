using System;
using System.Windows;
using System.Windows.Threading;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.SaveDialog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.AcceptanceTesting.Core;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.SaveDialog
{
    [Binding]
    public class SaveDialogSteps
    {
        [BeforeFeature("SaveDialog")]
        public static void SetupSaveDialogDependencies()
        {
            Utils.SetupResourceDictionary();
            var explorerRepository = new Mock<IExplorerRepository>();
            explorerRepository.Setup(repository => repository.Rename(It.IsAny<IExplorerItemViewModel>(), It.IsAny<string>())).Returns(true);
            var view = new RequestServiceNameView();
            var viewModel = new RequestServiceNameViewModel(new EnvironmentViewModel(new ServerForTesting(explorerRepository)), view,Guid.Empty);
            view.DataContext = viewModel;
            var window = new Window {Content = view};
            var app = Application.Current;
            app.MainWindow = window;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                var viewWindow = (RequestServiceNameView)Application.Current.MainWindow.Content;
                Assert.IsNotNull(viewWindow);
                Assert.IsNotNull(viewWindow.DataContext);
                Assert.IsInstanceOfType(viewWindow.DataContext, typeof(IRequestServiceNameViewModel));
                Application.Current.Shutdown();
            }));

            Application.Current.Run(Application.Current.MainWindow);
            FeatureContext.Current.Add("view",view);
            FeatureContext.Current.Add("viewModel",viewModel);
        }


        [BeforeScenario("SaveDialog")]
        public void SetupForSave()
        {
            var view = FeatureContext.Current.Get<IRequestServiceNameView>("view");
            var viewModel = FeatureContext.Current.Get<IRequestServiceNameViewModel>("viewModel");
            ScenarioContext.Current.Add("saveView", view);
            ScenarioContext.Current.Add("viewModel", viewModel);
            ScenarioContext.Current.Add("explorerView",view.GetExplorerView());

        }

        [Given(@"the Save Dialog is opened")]
        public void GivenTheSaveDialogIsOpened()
        {
            IRequestServiceNameView saveView;
            var gotView = ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsTrue(gotView);
            Assert.IsNotNull(saveView);
        }

        [Given(@"the ""(.*)"" server is visible in save dialog")]
        public void GivenTheServerIsVisibleInSaveDialog(string serverName)
        {
            IRequestServiceNameView saveView;
            var gotView = ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsTrue(gotView);
            saveView.HasServer(serverName);
        }

        [Given(@"I should see ""(.*)"" folders")]
        [When(@"I should see ""(.*)"" folders")]
        [Then(@"I should see ""(.*)"" folders")]
        public void GivenIShouldSeeFolders(int numberOfFoldersVisible)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            var explorerItemViewModels = saveView.GetFoldersVisible();
            Assert.AreEqual(numberOfFoldersVisible, explorerItemViewModels.Count);
        }

        [Given(@"I open ""(.*)"" in save dialog")]
        [When(@"I open ""(.*)"" in save dialog")]
        [Then(@"I open ""(.*)"" in save dialog")]
        public void WhenIOpenInSaveDialog(string folderName)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            saveView.OpenFolder(folderName);
        }

        [When(@"I create ""(.*)"" in ""(.*)""")]
        public void WhenICreateIn(string newFolderName, string rootPath)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            saveView.CreateNewFolder(newFolderName, rootPath);
        }


        [When(@"I enter name ""(.*)""")]
        public void WhenIEnterName(string serviceName)
        {
            IRequestServiceNameView saveView;
            IRequestServiceNameViewModel viewModel;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            ScenarioContext.Current.TryGetValue("viewModel", out viewModel);
            
            Assert.IsNotNull(saveView);
            var requestServiceNameViewModel = viewModel;
            Assert.IsNotNull(requestServiceNameViewModel);
            saveView.EnterName(serviceName);
            Assert.AreEqual(serviceName,requestServiceNameViewModel.Name);
        }

        [Then(@"Filter is ""(.*)""")]
        public void ThenFilterIs(string filter)
        {
            IRequestServiceNameView saveView;
            IRequestServiceNameViewModel viewModel;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            ScenarioContext.Current.TryGetValue("viewModel", out viewModel);

            Assert.IsNotNull(saveView);
            var requestServiceNameViewModel = viewModel;
            Assert.IsNotNull(requestServiceNameViewModel);
            saveView.Filter(filter);
        }

        [When(@"I save ""(.*)""")]
        public void WhenISave(string path)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            saveView.GetExplorerView().AddNewResource(path, "Workflow");
        }


        [Then(@"""(.*)"" is visible in ""(.*)""")]
        public void ThenIsVisibleIn(string resourceName, string folderName)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            var explorer = saveView.GetExplorerView();
            explorer.VerifyItemExists(folderName+"/"+resourceName);
        }

        [Then(@"save button is ""(.*)""")]
        public void ThenSaveButtonIs(string enabledString)
        {
            var enabled = enabledString.Equals("Enabled",StringComparison.OrdinalIgnoreCase);
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            var isSaveButtonEnabled = saveView.IsSaveButtonEnabled();
            Assert.AreEqual(enabled,isSaveButtonEnabled);
        }

        [Given(@"validation message is ""(.*)""")]
        [When(@"validation message is ""(.*)""")]
        [Then(@"validation message is ""(.*)""")]
        public void ThenValidationMessageIs(string expectedValidationMessage)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            var currentValidationMessage = saveView.GetValidationMessage();
            Assert.AreEqual(expectedValidationMessage,currentValidationMessage);
        }

        [When(@"I cancel")]
        public void WhenICancel()
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            saveView.Cancel();
        }

        [Then(@"""(.*)"" is not visible in ""(.*)""")]
        public void ThenIsNotVisibleIn(string resourceName, string folderName)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            var explorer = saveView.GetExplorerView();
            explorer.VerifyItemDoesNotExist(folderName + "/" + resourceName);
        }

        [When(@"I refresh the filter")]
        public void WhenIRefreshTheFilter()
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            var explorer = saveView.GetExplorerView();
            explorer.Refresh();
        }

        [When(@"I context menu ""(.*)"" ""(.*)"" on ""(.*)""")]
        public void WhenIContextMenuOn(string menuAction,string itemName, string path)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            saveView.PerformActionOnContextMenu(menuAction, itemName,path);
        }

        [When(@"I context menu ""(.*)"" ""(.*)"" to ""(.*)""")]
        public void WhenIContextMenuTo(string menuAction, string path, string itemName)
        {
            IRequestServiceNameView saveView;
            ScenarioContext.Current.TryGetValue("saveView", out saveView);
            Assert.IsNotNull(saveView);
            saveView.PerformActionOnContextMenu(menuAction, itemName, path);
        }


        [AfterScenario("SaveDialog")]
        public void CleanupForSave()
        {
            var view = ScenarioContext.Current.Get<IRequestServiceNameView>("saveView");
            var explorerRepository = new Mock<IExplorerRepository>();
            explorerRepository.Setup(repository => repository.Rename(It.IsAny<IExplorerItemViewModel>(), It.IsAny<string>())).Returns(true);
            var viewModel = new RequestServiceNameViewModel(new EnvironmentViewModel(new ServerForTesting(explorerRepository)), view, Guid.Empty);
            view.DataContext = viewModel;
            FeatureContext.Current.Remove("viewModel");
            FeatureContext.Current.Add("viewModel", viewModel);
        }
    }
}
