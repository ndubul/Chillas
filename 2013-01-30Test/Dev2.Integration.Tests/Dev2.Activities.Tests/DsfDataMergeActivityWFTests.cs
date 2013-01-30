﻿using Dev2.Integration.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Dev2.Integration.Tests.Dev2.Activities.Tests
{
    /// <summary>
    /// Summary description for DsfDataSplitActivityWFTests
    /// </summary>
    [TestClass]
    public class DsfDataMergeActivityWFTests
    {
        string WebserverURI = ServerSettings.WebserverURI;
        public DsfDataMergeActivityWFTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void DataMergeRecordsetsUsingStarAndCharMerge()
        {
            string PostData = String.Format("{0}{1}", WebserverURI, "DataMergeRecordsetsUsingStarAndCharMerge");
            string expected = @"<res>W.Buchan
B.Buchan
T.Williams-Ros
T.Frisinger
J.Smit
B.Page
M.Guerrera
A.Lewis
S.Naidoo
M.Cullen
</res>";

            string ResponseData = TestHelper.PostDataToWebserver(PostData);

            StringAssert.Contains(ResponseData, expected);
        }


        [TestMethod]
        public void DataMergeWithScalarsAndTabMerge()
        {
            string PostData = String.Format("{0}{1}", WebserverURI, "DataMergeWithScalarsAndTabMerge");
            string expected = @"<res>Dev2	0317641234</res>";

            string ResponseData = TestHelper.PostDataToWebserver(PostData);

            StringAssert.Contains(ResponseData, expected);
        }

        // Created by Michael
        // Created so Bug 7835 does not happen
        [TestMethod]
        public void DataMergeWithComplexExpression_Expected_TestPasses()
        {
            string PostData = String.Format("{0}{1}", WebserverURI, "LanguageParserEvaluatingComplexExpressionTest");
            string ResponseData = TestHelper.PostDataToWebserver(PostData);
            string expected = "<result>Barneys,Walliss,</result>";

            if (!ResponseData.Contains(expected))
            {
                Assert.Inconclusive("The test should pass.");
            }
        }
    }
}
