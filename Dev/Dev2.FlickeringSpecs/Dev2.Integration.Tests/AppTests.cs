
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev2.Integration.Tests
{
    [TestClass]
    public class AppTests
    {
        private static string deployDir;
        const string ServerProcessName = "Warewolf Server.exe";
        const string StudioProcessName = "Warewolf Studio";

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            deployDir = testContext.TestDeploymentDir;
        }

        // NOTE : This test assumes that there is a server running as part of the integration test suite ;)
        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("LifecycleManager_StartServer")]
        public void LifecycleManager_StartServer_WhenAServerIsRunning_ExpectSecondServerToCrash()
        {
            int runningServerID = FetchRunningServerID();

            try
            {
                var serverPath = GetProcessPath(ServerProcessName);

                //Pre-assert
                Assert.IsTrue(File.Exists(serverPath), "Server not found at " + serverPath);

                // fire off process 
                Process p = new Process { StartInfo = { FileName = serverPath, RedirectStandardOutput = true, UseShellExecute = false } };
                p.OutputDataReceived += OutputHandler; // ensure we can grap the output ;)
                p.Start();
                p.BeginOutputReadLine();

                // Wait for Process to start, and get past the check for a duplicate process
                Thread.Sleep(30000);

                // kill any hanging instances ;)
                const string wmiQueryString = "SELECT ProcessId FROM Win32_Process WHERE Name LIKE 'Warewolf Server%'";
                using(var searcher = new ManagementObjectSearcher(wmiQueryString))
                {
                    using(var results = searcher.Get())
                    {
                        ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();

                        if(mo != null)
                        {
                            var id = mo.Properties["ProcessId"].Value.ToString();

                            int myID;
                            Int32.TryParse(id, out myID);

                            if(myID != runningServerID)
                            {
                                var proc = Process.GetProcessById(myID);

                                proc.Kill();
                            }
                        }
                    }
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch(Exception e)
            // ReSharper restore EmptyGeneralCatchClause
            {
                Assert.Fail(e.Message);
            }

            const string expected = "Critical Failure: Webserver failed to startException has been thrown by the target of an invocation.";

            StringAssert.Contains(outputData, expected);
        }

        private static string GetProcessPath(string processName)
        {
            var query = new SelectQuery(@"SELECT * FROM Win32_Process where Name LIKE '%" + processName + "%'");
            ManagementObjectCollection processes;
            //initialize the searcher with the query it is
            //supposed to execute
            using(ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                //execute the query
                processes = searcher.Get();
                if(processes.Count <= 0)
                {
                    return null;
                }
            }
            if(processes == null || processes.Count == 0)
            {
                return null;
            }
            return (from ManagementObject process in processes select (process.Properties["ExecutablePath"].Value ?? string.Empty).ToString()).FirstOrDefault();
        }

        #region Server Lifecycle Manager Test Utils

        private static string outputData = string.Empty;
        private static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output. 
            var data = outLine.Data;
            if(!String.IsNullOrEmpty(data))
            {
                outputData += data;
            }
        }

        private int FetchRunningServerID()
        {
            const string wmiQueryString = "SELECT ProcessId FROM Win32_Process WHERE Name LIKE 'Warewolf Server%'";
            using(var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                using(var results = searcher.Get())
                {
                    ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();

                    if(mo != null)
                    {
                        var id = mo.Properties["ProcessId"].Value.ToString();

                        int myID;
                        Int32.TryParse(id, out myID);

                        return myID;
                    }
                }
            }

            return 0;
        }

        #endregion
    }
}
