﻿using System.IO;
using Dev2.Studio.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev2.Core.Tests.Helpers
{
    // PBI 9512 - 2013.06.07 - TWR: added
    [TestClass]
    public class LatestWebGetterTests
    {
        static string _testDir;

        [ClassInitialize]
        public static void MyClassInit(TestContext context)
        {
            _testDir = context.DeploymentDirectory;
        }

        [TestMethod]
        public void LatestWebGetterExpectedRaisesInvoked()
        {
            var getter = new LatestWebGetter();
            getter.Invoked += (sender, args) => Assert.IsTrue(true);
            getter.GetLatest("myfake.uri", "hello world");
        }

        [TestMethod]
        public void LatestWebGetterWithInvalidArgsExpectedDoesNothing()
        {
            var getter = new LatestWebGetter();
            getter.GetLatest("myfake.uri", "hello world");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void LatestWebGetterWithValidArgsExpectedReplacesFileContent()
        {
            var path = Path.GetTempFileName();

            //var path = Path.Combine(_testDir, Path.GetRandomFileName());
            Assert.IsFalse(File.Exists(path));

            var getter = new LatestWebGetter();
            getter.GetLatest("http://www.google.co.za", path);

            Assert.IsTrue(File.Exists(path),"Could not create  [ " + path + " ]");
        }
    }
}
