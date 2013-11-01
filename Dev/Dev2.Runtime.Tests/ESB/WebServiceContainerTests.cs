﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Dev2.Common;
using Dev2.DataList.Contract;
using Dev2.DynamicServices;
using Dev2.DynamicServices.Test.XML;
using Dev2.Runtime.ESB.Execution;
using Dev2.Runtime.Hosting;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;using System.Diagnostics.CodeAnalysis;
using Moq;

namespace Dev2.Tests.Runtime.ESB
{
    [TestClass][ExcludeFromCodeCoverage]
    public class WebServiceContainerTests
    {
        static readonly XElement WebSourceWithInputsXml = XmlResource.Fetch("WebSource");
        static readonly XElement WebServiceWithInputsXml = XmlResource.Fetch("WebService");
        static readonly XElement WebServiceWithInputsResponseXml = XmlResource.Fetch("WebServiceResponse");

        static readonly XElement WebSourceWithoutInputsXml = XmlResource.Fetch("WebSourceWithoutInputs");
        static readonly XElement WebServiceWithoutInputsXml = XmlResource.Fetch("WebServiceWithoutInputs");
        const string WebServiceWithoutInputsResponse = "{'completed_in':0.015,'max_id':340107380383678465,'max_id_str':'340107380383678465','page':1,'query':'%40Dev2Test','refresh_url':'?since_id=340107380383678465&q=%40Dev2Test','results':[],'results_per_page':15,'since_id':0,'since_id_str':'0'}";
        static readonly XElement WebServiceWithoutInputsResponseXml = XmlResource.Fetch("WebServiceWithoutInputsResponse");

        #region HandlesOutputFormatting
        [TestMethod]
        public void WebServiceContainerServiceInputsExpectedServiceActionWithInputs()
        {
            //------------------------------------Setup -------------------------------------------------------------------------
            var sa = CreateServiceAction(WebServiceWithInputsXml, WebSourceWithInputsXml);
            //------------------------------------Execute-----------------------------------------------------------------------
            List<ServiceActionInput> serviceActionInputs = sa.ServiceActionInputs;
            //------------------------------------Assert------------------------------------------------------------------------
            Assert.AreEqual(2,serviceActionInputs.Count);
            Assert.AreEqual("CityName",serviceActionInputs[0].Source);
            Assert.AreEqual("CountryName",serviceActionInputs[1].Source);
        }

        #endregion

        #region Execute

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("WebserviceExecutionContainer_Execute")]
        public void WebserviceExecutionContainer_Execute_whenErrors_ExpectValidErrors()
        {
            //------------Setup for test--------------------------
            var container = CreateWebServiceContainer(WebServiceWithInputsXml, WebSourceWithInputsXml, WebServiceWithInputsResponseXml.ToString(), true);

            ErrorResultTO errors;

            //------------Execute Test---------------------------
            container.Execute(out errors);

            //------------Assert Results-------------------------
            Assert.AreEqual(1, errors.FetchErrors().Count);
            Assert.AreEqual("Faulty Things Happened", errors.FetchErrors()[0]);

        }

        [TestMethod]
        public void WebServiceContainerExecuteWithValidServiceHavingInputsExpectedExecutesService()
        {
            var container = CreateWebServiceContainer(WebServiceWithInputsXml, WebSourceWithInputsXml, WebServiceWithInputsResponseXml.ToString());

            ErrorResultTO errors;
            var dataListID = container.Execute(out errors);
            var compiler = DataListFactory.CreateDataListCompiler();

            var result = compiler.ConvertFrom(dataListID, DataListFormat.CreateFormat(GlobalConstants._XML), enTranslationDepth.Data, out errors);

            Assert.IsNotNull(result);

            var resultXml = XElement.Parse(result);

            var expectedRoot = (XElement)WebServiceWithInputsResponseXml.FirstNode;
            foreach(var actualNode in resultXml.Elements())
            {
                var actualName = actualNode.Name.LocalName;
                if(!actualName.StartsWith("Dev2System"))
                {
                    var expectedNode = expectedRoot.Element(actualName);
                    Assert.AreEqual(expectedNode.Value, actualNode.Value);
                }
            }
        }

        [TestMethod]
        public void WebServiceContainerExecuteWithValidServiceHavingNoInputsExpectedExecutesService()
        {
            var container = CreateWebServiceContainer(WebServiceWithoutInputsXml, WebSourceWithoutInputsXml, WebServiceWithoutInputsResponse);

            ErrorResultTO errors;
            var dataListID = container.Execute(out errors);
            var compiler = DataListFactory.CreateDataListCompiler();

            var result = compiler.ConvertFrom(dataListID, DataListFormat.CreateFormat(GlobalConstants._XML), enTranslationDepth.Data, out errors);

            Assert.IsNotNull(result);

            var resultXml = XElement.Parse(result);

            var expectedRoot = (XElement)WebServiceWithoutInputsResponseXml;
            foreach(var actualNode in resultXml.Elements())
            {
                var actualName = actualNode.Name.LocalName;
                if(!actualName.StartsWith("Dev2System"))
                {
                    var expectedNode = expectedRoot.Element(actualName);
                    Assert.AreEqual(expectedNode.Value, actualNode.Value);
                }
            }
        }

        [TestMethod]
        public void WebServiceContainerExecuteWhenThrowsErrorExpectErrorIsAddedToErrorsCollection()
        {
            var container = CreateWebServiceContainerThrowingException(WebServiceWithoutInputsXml, WebSourceWithoutInputsXml, WebServiceWithoutInputsResponse);

            ErrorResultTO errors;
            container.Execute(out errors);

            Assert.IsTrue(errors.HasErrors());
            Assert.AreEqual("Service Execution Error: Cannot Execute Web Request", errors.FetchErrors()[0]);
        }

        WebServiceContainerMock CreateWebServiceContainerThrowingException(XElement serviceXml, XElement sourceXml, string response)
        {
            ErrorResultTO errors;
            var compiler = DataListFactory.CreateDataListCompiler();
            var dataListID = compiler.ConvertTo(DataListFormat.CreateFormat(GlobalConstants._XML), "", "<DataList></DataList>", out errors);

            var dataObj = new Mock<IDSFDataObject>();
            dataObj.Setup(d => d.DataListID).Returns(dataListID);

            var workspace = new Mock<IWorkspace>();
            var esbChannel = new Mock<IEsbChannel>();

            var sa = CreateServiceAction(serviceXml, sourceXml);
            var container = new WebServiceContainerMockWithError(sa, dataObj.Object, workspace.Object, esbChannel.Object)
            {
                WebRequestRespsonse = response
            };
            return container;
        }

        #endregion

        #region CreateWebServiceContainer

        static WebServiceContainer CreateWebServiceContainer(XElement serviceXml, XElement sourceXml, string response, bool isFaulty = false)
        {
            ErrorResultTO errors;
            var compiler = DataListFactory.CreateDataListCompiler();
            var dataListID = compiler.ConvertTo(DataListFormat.CreateFormat(GlobalConstants._XML), "", "<DataList></DataList>", out errors);

            var dataObj = new Mock<IDSFDataObject>();
            dataObj.Setup(d => d.DataListID).Returns(dataListID);

            var workspace = new Mock<IWorkspace>();
            var esbChannel = new Mock<IEsbChannel>();

            var sa = CreateServiceAction(serviceXml, sourceXml);
            WebServiceContainer container = null;

            if (!isFaulty)
            {

                container = new WebServiceContainerMock(sa, dataObj.Object, workspace.Object, esbChannel.Object)
            {
                WebRequestRespsonse = response
            };
            }
            else
            {
                container = new FaultyWebServiceContainerMock(sa, dataObj.Object, workspace.Object, esbChannel.Object)
                {
                    WebRequestRespsonse = string.Empty
                };
            }

            return container;
        }
        #endregion

        #region CreateServiceAction

        static ServiceAction CreateServiceAction(XElement serviceXml, XElement sourceXml)
        {
            var graph = new DynamicObjectHelper().GenerateObjectGraphFromString(serviceXml.ToString());

            var ds = (DynamicService)graph[0];
            var sa = ds.Actions[0];
            sa.Source = new Source { ResourceDefinition = sourceXml.ToString() };
            return sa;
        }

        #endregion

    }

    internal class WebServiceContainerMockWithError : WebServiceContainerMock
    {
        public WebServiceContainerMockWithError(ServiceAction sa, IDSFDataObject dsfDataObject, IWorkspace workspace, IEsbChannel esbChannel)
            : base(sa,dsfDataObject,workspace,esbChannel)
        {
        }

        #region Overrides of WebServiceContainerMock

        protected override void ExecuteWebRequest(WebService service, out ErrorResultTO errors)
        {
            throw new Exception("Cannot Execute Web Request");
        }

        #endregion
    }
}
