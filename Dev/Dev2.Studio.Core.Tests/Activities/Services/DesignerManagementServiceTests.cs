﻿using System;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using Caliburn.Micro;
using Dev2.Composition;
using Dev2.Core.Tests.Utils;
using Dev2.Studio.Core.Activities.Services;
using Dev2.Studio.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.Core.Tests.Activities.Services
{
    [TestClass]
    public class DesignerManagementServiceTests
    {

        [TestInitialize]
        public void TestInit()
        {
            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;
            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });
        }

        [TestMethod]
        [TestCategory("DesignerManagementService_Constructor")]
        [Description("DesignerManagementService must throw null argument exception if root model is null.")]
        [Owner("Trevor Williams-Ros")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DesignerManagementService_UnitTest_ConstructorWithNullRootModel_ThrowsArgumentNullException()
        {
            var designerManagementService = new DesignerManagementService(null, null);
        }

        [TestMethod]
        [TestCategory("DesignerManagementService_Constructor")]
        [Description("DesignerManagementService must throw null argument exception if resource repository is null.")]
        [Owner("Trevor Williams-Ros")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DesignerManagementService_UnitTest_ConstructorWithNullResourceRepository_ThrowsArgumentNullException()
        {
            var rootModel = new Mock<IContextualResourceModel>();
            var designerManagementService = new DesignerManagementService(rootModel.Object, null);
        }


        [TestMethod]
        [TestCategory("DesignerManagementService_GetRootResourceModel")]
        [Description("DesignerManagementService GetRootResourceModel must return the same root model given to its constructor.")]
        [Owner("Trevor Williams-Ros")]
        public void DesignerManagementService_UnitTest_GetResourceModel_SameAsConstructorInstance()
        {
            Mock<IContextualResourceModel> resourceModel = Dev2MockFactory.SetupResourceModelMock();
            Mock<IResourceRepository> resourceRepository = Dev2MockFactory.SetupFrameworkRepositoryResourceModelMock(resourceModel, new List<IResourceModel>());

            var designerManagementService = new DesignerManagementService(resourceModel.Object, resourceRepository.Object);

            IContextualResourceModel expected = resourceModel.Object;
            IContextualResourceModel actual = designerManagementService.GetRootResourceModel();

            Assert.AreEqual(expected, actual);
        }
    }
}
