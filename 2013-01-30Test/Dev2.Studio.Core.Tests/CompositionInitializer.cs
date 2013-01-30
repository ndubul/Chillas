﻿#region

using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Windows;
using Caliburn.Micro;
using Dev2.Composition;
using Dev2.Core.Tests.ProperMoqs;
using Dev2.DataList.Contract;
using Dev2.Network;
using Dev2.Network.Execution;
using Dev2.Studio.Core;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.Services.System;
using Dev2.Studio.Core.ViewModels;
using Dev2.Studio.Core.ViewModels.Base;
using Dev2.Studio.Core.Wizards;
using Dev2.Studio.Core.Wizards.Interfaces;
using Dev2.Studio.Feedback;
using Dev2.Studio.ViewModels;
using Moq;

#endregion

namespace Dev2.Core.Tests
{
    internal static class CompositionInitializer
    {
        internal static ImportServiceContext DefaultInitialize()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });

            IMainViewModel mainViewModel = new MainViewModel();
            ImportService.AddExportedValueToContainer(mainViewModel);
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());
            ImportService.SatisfyImports(mainViewModel);

            return importServiceContext;
        }

        internal static ImportServiceContext EmptyInitialize()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeMockedMainViewModel()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });

            var mainViewModel = new Mock<IMainViewModel>();

            ImportService.AddExportedValueToContainer(mainViewModel.Object);
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeMockedMainViewModelOnly()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            var mainViewModel = new Mock<IMainViewModel>();

            ImportService.AddExportedValueToContainer(mainViewModel.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeMockedMainViewModelStudioCore()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new StudioCoreTestAggregateCatalog()
            });

            var mainViewModel = new Mock<IMainViewModel>();

            ImportService.AddExportedValueToContainer(mainViewModel.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForFrameworkSecurityProviderTests()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer<IPopUp>(new MoqPopup());
            ImportService.AddExportedValueToContainer<IDev2ConfigurationProvider>(new MoqConfigurationReader());

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeResourceWizardViewModelTests(IWebCommunication webCommunication)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer<IFrameworkSecurityContext>(new MockSecurityProvider(""));
            ImportService.AddExportedValueToContainer<IPopUp>(new MoqPopup());
            ImportService.AddExportedValueToContainer(webCommunication);
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());

            //IMainViewModel mainViewModel = new MainViewModel();
            //ImportService.AddExportedValueToContainer<IMainViewModel>(mainViewModel);
            //ImportService.SatisfyImports(mainViewModel);

            return importServiceContext;
        }

        internal static ImportServiceContext ExplorerViewModelTest()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });

            ImportService.AddExportedValueToContainer<IFrameworkSecurityContext>(new MockSecurityProvider(""));
            // setup env repo
            var repo = new Mock<IFrameworkRepository<IEnvironmentModel>>();
            repo.Setup(l => l.Load()).Verifiable();
            var model = new Mock<IEnvironmentModel>();
            repo.Setup(l => l.Save(model.Object)).Verifiable();
            IList<IEnvironmentModel> models = new List<IEnvironmentModel>();
            repo.Setup(l => l.All()).Returns(models);
            // set up window behavior
            var winBehavior = new Mock<IDev2WindowManager>();

            winBehavior.Setup(w => w.ShowDialog(null));
            ImportService.AddExportedValueToContainer(repo.Object);
            ImportService.AddExportedValueToContainer(winBehavior.Object);
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());

            return importServiceContext;
        }

        internal static ImportServiceContext DeployViewModelOkayTest()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });

            var mainViewModel = new Mock<IMainViewModel>();
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());
            ImportService.AddExportedValueToContainer(mainViewModel.Object);
            ImportService.AddExportedValueToContainer<IFrameworkSecurityContext>(new MockSecurityProvider(""));

            // setup env repo
            var repo = new Mock<IFrameworkRepository<IEnvironmentModel>>();
            repo.Setup(l => l.Load()).Verifiable();

            var model = new Mock<IEnvironmentModel>();
            repo.Setup(l => l.Save(model.Object)).Verifiable();

            IList<IEnvironmentModel> models = new List<IEnvironmentModel>();
            repo.Setup(l => l.All()).Returns(models);

            // set up window behavior
            var winBehavior = new Mock<IDev2WindowManager>();

            winBehavior.Setup(w => w.ShowDialog(It.IsAny<BaseViewModel>())).Callback<BaseViewModel>(v => v.DialogResult = ViewModelDialogResults.Okay);
            ImportService.AddExportedValueToContainer(repo.Object);
            ImportService.AddExportedValueToContainer(winBehavior.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext DeployViewModelCancelTest()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });

            var mainViewModel = new Mock<IMainViewModel>();
            ImportService.AddExportedValueToContainer(mainViewModel.Object);
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());
            ImportService.AddExportedValueToContainer<IFrameworkSecurityContext>(new MockSecurityProvider(""));

            // setup env repo
            var repo = new Mock<IFrameworkRepository<IEnvironmentModel>>();
            repo.Setup(l => l.Load()).Verifiable();

            var model = new Mock<IEnvironmentModel>();
            repo.Setup(l => l.Save(model.Object)).Verifiable();

            IList<IEnvironmentModel> models = new List<IEnvironmentModel>();
            repo.Setup(l => l.All()).Returns(models);

            // set up window behavior
            var winBehavior = new Mock<IDev2WindowManager>();

            winBehavior.Setup(w => w.ShowDialog(It.IsAny<BaseViewModel>())).Callback<BaseViewModel>(v => v.DialogResult = ViewModelDialogResults.Cancel);
            ImportService.AddExportedValueToContainer(repo.Object);
            ImportService.AddExportedValueToContainer(winBehavior.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeMockedWindowNavigationBehavior()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            // set up window behavior
            var winBehavior = new Mock<IDev2WindowManager>();
            ImportService.AddExportedValueToContainer(winBehavior.Object);
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());

            return importServiceContext;
        }

        internal static ImportServiceContext PopUpProviderForTestsWithMockMainViewModel()
        {
            return PopUpProviderForTestsWithMockMainViewModel(MessageBoxResult.OK);
        }

        internal static ImportServiceContext PopUpProviderForTestsWithMockMainViewModel(MessageBoxResult popupResult)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullStudioAggregateCatalog()
            });

            var mainViewModel = new Mock<IMainViewModel>();
            ImportService.AddExportedValueToContainer(mainViewModel.Object);
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());
            ImportService.AddExportedValueToContainer<IPopUp>(new MoqPopup(popupResult));

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForFeedbackActionTests(Mock<IPopUp> popup, Mock<IFeedBackRecorder> feedBackRecorder, Mock<IFeedbackInvoker> feedbackInvoker)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            var mainViewModel = new Mock<IMainViewModel>();
            var windowManager = new Mock<IDev2WindowManager>();
            ImportService.AddExportedValueToContainer(mainViewModel.Object);

            ImportService.AddExportedValueToContainer(popup.Object);
            ImportService.AddExportedValueToContainer(windowManager.Object);
            ImportService.AddExportedValueToContainer(feedBackRecorder.Object);
            ImportService.AddExportedValueToContainer(feedbackInvoker.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForFeedbackInvokerTests(Mock<IPopUp> popup)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            var mainViewModel = new Mock<IMainViewModel>();
            ImportService.AddExportedValueToContainer(mainViewModel.Object);

            ImportService.AddExportedValueToContainer(popup.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForDataListChannelTests(Mock<INetworkMessageBroker> networkMessageBroker, Mock<IStudioNetworkMessageAggregator> studioNetworkMessageAggregator)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(networkMessageBroker.Object);
            ImportService.AddExportedValueToContainer(studioNetworkMessageAggregator.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForDataListChannelTests(Mock<INetworkMessageBroker> networkMessageBroker, IStudioNetworkMessageAggregator studioNetworkMessageAggregator)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(networkMessageBroker.Object);
            ImportService.AddExportedValueToContainer(studioNetworkMessageAggregator);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForExecutionChannelMessaegRecievingTests(INetworkMessageBroker networkMessageBroker, IStudioNetworkMessageAggregator studioNetworkMessageAggregator)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(networkMessageBroker);
            ImportService.AddExportedValueToContainer(studioNetworkMessageAggregator);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForExecutionChannelTests(Mock<IExecutionStatusCallbackDispatcher> executionStatusCallbackDispatcher, Mock<INetworkMessageBroker> networkMessageBroker, Mock<IStudioNetworkMessageAggregator> studioNetworkMessageAggregator)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(executionStatusCallbackDispatcher.Object);
            ImportService.AddExportedValueToContainer(networkMessageBroker.Object);
            ImportService.AddExportedValueToContainer(studioNetworkMessageAggregator.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeForExecutionChannelTests(Mock<IExecutionStatusCallbackDispatcher> executionStatusCallbackDispatcher, INetworkMessageBroker networkMessageBroker, IStudioNetworkMessageAggregator studioNetworkMessageAggregator)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(executionStatusCallbackDispatcher.Object);
            ImportService.AddExportedValueToContainer(networkMessageBroker);
            ImportService.AddExportedValueToContainer(studioNetworkMessageAggregator);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeEmailFeedbackTest(Mock<ISystemInfoService> systemInfoService)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(systemInfoService.Object);
            ImportService.AddExportedValueToContainer<IPopUp>(new MoqPopup(MessageBoxResult.OK));
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeEmailFeedbackActionTest()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());
            ImportService.AddExportedValueToContainer<IPopUp>(new MoqPopup(MessageBoxResult.OK));

            return importServiceContext;
        }

        public static ImportServiceContext InitializeWithWindowManagerTest(Mock<ISystemInfoService> mockSysInfo, Mock<IDev2WindowManager> mockWindowManager)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(mockSysInfo.Object);
            ImportService.AddExportedValueToContainer(mockWindowManager.Object);
            ImportService.AddExportedValueToContainer<IPopUp>(new MoqPopup(MessageBoxResult.OK));
            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());

            return importServiceContext;
        }

        public static ImportServiceContext InitializeWithMockEventAggregator(Mock<IEventAggregator> mockEventAggregator)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });

            ImportService.AddExportedValueToContainer(mockEventAggregator.Object);
            ImportService.AddExportedValueToContainer<IPopUp>(new MoqPopup(MessageBoxResult.OK));
            return importServiceContext;
        }

        internal static ImportServiceContext InitializeTreeViewModelTests(Mock<IWizardEngine> wizardEngine)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>());

            ImportService.AddExportedValueToContainer(wizardEngine.Object);

            return importServiceContext;
        }

        internal static ImportServiceContext InitializeNavigationViewModelTests(Mock<IFrameworkRepository<IEnvironmentModel>> repo)
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;

            ImportService.Initialize(new List<ComposablePartCatalog>()
            {
                new FullTestAggregateCatalog()
            });

            ImportService.AddExportedValueToContainer<IEventAggregator>(new EventAggregator());
            ImportService.AddExportedValueToContainer(repo.Object);
            ImportService.AddExportedValueToContainer<IWizardEngine>(new WizardEngine());

            return importServiceContext;
        }
    }
}