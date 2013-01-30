﻿using Dev2;
using Dev2.DataList.Contract.Binary_Objects;
using Dev2.Tests.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace ActivityUnitTests.ActivityTests
{
    /// <summary>
    /// Summary description for BaseConvertActivityTests
    /// </summary>
    [TestClass]
    public class BaseConvertActivityTests : BaseActivityUnitTest
    {
        public BaseConvertActivityTests()
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Base Convert Cases

        [TestMethod]
        public void BaseConvert_ScalarToBase64_Expected_stringToBase64()
        {
            IList<BaseConvertTO> convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[testVar]]", "Text", "Base 64", "[[testVar]]", 1) };
            SetupArguments(@"<root><testVar>change this to base64</testVar></root>"
                          , ActivityStrings.CaseConvert_DLShape
                          , convertCollection
                          );
            IDSFDataObject result = ExecuteProcess();

            string expected = @"Y2hhbmdlIHRoaXMgdG8gYmFzZTY0";
            string actual = string.Empty;
            string error = string.Empty;
            GetScalarValueFromDataList(result.DataListID, "testVar", out actual, out error);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BaseConvert_RecsetWithIndexToHex_Expected_stringToHex()
        {
            IList<BaseConvertTO> convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[Recset(1).Field]]", "Text", "Hex", "[[Recset(1).Field]]", 1) };
            SetupArguments(
                            @"<root><Recset><Field>CHANGE THIS TO HEX</Field></Recset></root>"
                          , ActivityStrings.BaseConvert_DLShape
                          , convertCollection
                          );
            IDSFDataObject result = ExecuteProcess();

            string expected = @"0x4348414e4745205448495320544f20484558";
            string error = string.Empty;
            string actual = RetrieveAllRecordSetFieldValues(result.DataListID, "Recset", "Field", out error).First();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BaseConvert_RecsetWithNoIndexToBinary_Expected_stringToBinary()
        {
            IList<BaseConvertTO> convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[Recset().Field]]", "Text", "Binary", "[[Recset().Field]]", 1) };
            SetupArguments(
                            @"<root><Recset><Field>CHANGE THIS TO BINARY</Field></Recset></root>"
                          , ActivityStrings.BaseConvert_DLShape
                          , convertCollection
                          );
            IDSFDataObject result = ExecuteProcess();

            string expected = @"010000110100100001000001010011100100011101000101001000000101010001001000010010010101001100100000010101000100111100100000010000100100100101001110010000010101001001011001";
            string error = string.Empty;
            string actual = RetrieveAllRecordSetFieldValues(result.DataListID, "Recset", "Field", out error).First();

            Assert.AreEqual(expected, actual);
        }

        #endregion Base Convert Cases

        #region Language Tests

        [TestMethod]
        public void BaseConvert_RecsetWithStarToBinary_Expected_stringToBinary()
        {
            IList<BaseConvertTO> convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[Recset(*).Field]]", "Text", "Binary", "[[Recset(*).Field]]", 1) };
            
            SetupArguments(
                @"<root><Recset><Field>CHANGE THIS TO BINARY</Field></Recset><Recset><Field>New Text to change</Field></Recset><Recset><Field>Other new text to change</Field></Recset></root>"
              , ActivityStrings.BaseConvert_DLShape
              , convertCollection
              );
            IDSFDataObject result = ExecuteProcess();

            List<string> expected = new List<string> 
                { "010000110100100001000001010011100100011101000101001000000101010001001000010010010101001100100000010101000100111100100000010000100100100101001110010000010101001001011001"
                , "010011100110010101110111001000000101010001100101011110000111010000100000011101000110111100100000011000110110100001100001011011100110011101100101"
                , "010011110111010001101000011001010111001000100000011011100110010101110111001000000111010001100101011110000111010000100000011101000110111100100000011000110110100001100001011011100110011101100101"
                };
            string error = string.Empty;
            List<string> actual = RetrieveAllRecordSetFieldValues(result.DataListID, "Recset", "Field", out error);
            CollectionAssert.AreEqual(expected, actual, new ActivityUnitTests.Utils.StringComparer());


        }

        // Travis.Frisinger - 24.01.2013 - Bug 7916
        [TestMethod]
        public void Sclar_To_Base64_Back_To_Text_Expect_Original()
        {


            IList<BaseConvertTO> convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[test]]", "Text", "Base64", "[[test]]", 1), new BaseConvertTO("[[test]]", "Base64", "Text", "[[test]]", 2) }; 
            SetupArguments(
                @"<root><test>data</test></root>"
              , @"<root><test/></root>"
              , convertCollection
              );
            IDSFDataObject result = ExecuteProcess();

            string error;
            string actual;
            GetScalarValueFromDataList(result.DataListID, "test", out actual, out error);
            
            Assert.AreEqual("data", actual);
        }

        #endregion Language Tests

        #region Negative Tests

        [TestMethod]
        public void BaseConvert_ScalarToBase64_Expected_ErrorTag()
        {
            IList<BaseConvertTO> convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[testVar]]", "Text", "Base 64", "[[testVar]]", 1) };

            SetupArguments(
                            @"<root><testVar>0001010111010</testVar></root>"
                          , ActivityStrings.CaseConvert_DLShape
                          , convertCollection
                          );

            IDSFDataObject result = ExecuteProcess();
            Assert.IsTrue(_compiler.HasErrors(result.DataListID));
        }

        [TestMethod]
        public void BaseConvert_Convert_Binary_To_Text_With_Base64_Value_Expected_ErrorTag()
        {
            IList<BaseConvertTO> convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[testVar]]", "Binary", "Text", "[[testVar]]", 1) };

            SetupArguments(
                            @"<root><testVar>SGkgdGhpcyBpcyB0ZXh0 </testVar></root>"
                          , ActivityStrings.CaseConvert_DLShape
                          , convertCollection
                          );

            IDSFDataObject result = ExecuteProcess();
            string actual;
            string error;
            GetScalarValueFromDataList(result.DataListID, "Dev2System.Error", out actual, out error);

            Assert.AreEqual(@"<InnerError>Base Conversion Broker was expecting [ Binary ] but the data was not in this format</InnerError>", actual);        
        }

        #endregion Negative Tests

        #region GetWizardData Tests

        [TestMethod]
        public void GetWizardData_Expected_Correct_IBinaryDataList()
        {
            bool passTest = true;
            IList<BaseConvertTO> _convertCollection = new List<BaseConvertTO>() { new BaseConvertTO("[[testVar]]", "Text", "Base 64", "[[testVar]]", 1), new BaseConvertTO("[[testVar1]]", "Text", "Base 64", "[[testVar]]", 2) };

            DsfBaseConvertActivity testAct = new DsfBaseConvertActivity { ConvertCollection = _convertCollection };

            IBinaryDataList binaryDL = testAct.GetWizardData();
            var recsets = binaryDL.FetchRecordsetEntries();
            if (recsets.Count != 1)
            {
                passTest = false;
            }
            else
            {
                if (recsets[0].Columns.Count != 4)
                {
                    passTest = false;
                }
            }
            Assert.IsTrue(passTest);
        }

        #endregion GetWizardData Tests

        #region Private Test Methods

        private void SetupArguments(string currentDL, string testData, IList<BaseConvertTO> converCollection)
        {
            TestStartNode = new FlowStep
            {
                Action = new DsfBaseConvertActivity { ConvertCollection = converCollection }
            };

            CurrentDL = testData;
            TestData = currentDL;
        }

        #endregion Private Test Methods

    }
}
