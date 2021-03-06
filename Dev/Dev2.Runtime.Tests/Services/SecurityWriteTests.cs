
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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Runtime.ESB.Management.Services;
using Dev2.Runtime.Security;
using Dev2.Services.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Dev2.Tests.Runtime.Services
{
    // ReSharper disable InconsistentNaming

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SecurityWriteTests
    {

        #region Execute

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SecurityWrite_Execute")]
        [ExpectedException(typeof(InvalidDataException))]
        public void SecurityWrite_Execute_NoSecuritySettingsValuePassed_ExceptionThrown()
        {
            //------------Setup for test--------------------------
            var securityWrite = new SecurityWrite();
            //------------Execute Test---------------------------
            securityWrite.Execute(new Dictionary<string, StringBuilder> { { "NoPermisisons", new StringBuilder("Something") } }, null);
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SecurityWrite_Execute")]
        [ExpectedException(typeof(InvalidDataException))]
        public void SecurityWrite_Execute_NoValuesPassed_ExceptionThrown()
        {
            //------------Setup for test--------------------------
            var securityWrite = new SecurityWrite();
            //------------Execute Test---------------------------
            securityWrite.Execute(null, null);
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SecurityWrite_Execute")]
        [ExpectedException(typeof(InvalidDataException))]
        public void SecurityWrite_Execute_SecuritySettingsValuePassedNotValidJSON_ExceptionThrown()
        {
            //------------Setup for test--------------------------
            var securityWrite = new SecurityWrite();
            //------------Execute Test---------------------------
            securityWrite.Execute(new Dictionary<string, StringBuilder> { { "SecuritySettings", new StringBuilder("Something") } }, null);
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SecurityWrite_Execute")]
        public void SecurityWrite_Execute_SecuritySettingsValuePassedValidJSON_ShouldWriteFile()
        {
            //------------Setup for test--------------------------
            var permission = new WindowsGroupPermission { Administrator = true, IsServer = true, WindowsGroup = Environment.UserName };
            var windowsGroupPermissions = new List<WindowsGroupPermission> { permission };
            var securitySettings = new SecuritySettingsTO(windowsGroupPermissions) { CacheTimeout = new TimeSpan(0, 2, 0) };
            var securitySettingsValue = JsonConvert.SerializeObject(securitySettings);
            var securityWrite = new SecurityWrite();
            //------------Execute Test---------------------------
            securityWrite.Execute(new Dictionary<string, StringBuilder> { { "SecuritySettings", new StringBuilder(securitySettingsValue) } }, null);
            //------------Assert Results-------------------------
            Assert.IsTrue(File.Exists("secure.config"));
            var fileData = File.ReadAllText("secure.config");
            Assert.IsFalse(fileData.StartsWith("{"));
            Assert.IsFalse(fileData.EndsWith("}"));
            Assert.IsFalse(fileData.Contains("IsServer"));
            File.Delete("secure.config");
        }

        #endregion Exeute

        #region HandlesType

        [TestMethod]
        public void SecurityWrite_HandlesType_ReturnsSecurityWriteService()
        {
            var esb = new SecurityWrite();
            var result = esb.HandlesType();
            Assert.AreEqual("SecurityWriteService", result);
        }

        #endregion

        #region CreateServiceEntry

        [TestMethod]
        public void SecurityWrite_CreateServiceEntry_ReturnsDynamicService()
        {
            var esb = new SecurityWrite();
            var result = esb.CreateServiceEntry();
            Assert.AreEqual(esb.HandlesType(), result.Name);
            Assert.AreEqual("<DataList><SecuritySettings ColumnIODirection=\"Input\"></SecuritySettings><Result/><Dev2System.ManagmentServicePayload ColumnIODirection=\"Both\"></Dev2System.ManagmentServicePayload></DataList>", result.DataListSpecification.ToString());
            Assert.AreEqual(1, result.Actions.Count);

            var serviceAction = result.Actions[0];
            Assert.AreEqual(esb.HandlesType(), serviceAction.Name);
            Assert.AreEqual(enActionType.InvokeManagementDynamicService, serviceAction.ActionType);
            Assert.AreEqual(esb.HandlesType(), serviceAction.SourceMethod);
        }

        #endregion

        [TestMethod]
        [Owner("Trevor Williams-Ros")]
        [TestCategory("SecurityWrite_Write")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SecurityWrite_Write_SecuritySettingsIsNull_ThrowsArgumentNullException()
        {
            //------------Setup for test--------------------------

            //------------Execute Test---------------------------
            SecurityWrite.Write(null);

            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Trevor Williams-Ros")]
        [TestCategory("SecurityWrite_Write")]
        public void SecurityWrite_Write_SecuritySettingsIsNotNull_PersistsSecuritySettings()
        {
            //------------Setup for test--------------------------
            if(File.Exists(ServerSecurityService.FileName))
            {
                File.Delete(ServerSecurityService.FileName);
            }

            var securitySettings = new SecuritySettingsTO();

            //------------Execute Test---------------------------
            SecurityWrite.Write(securitySettings);

            //------------Assert Results-------------------------
            Assert.IsTrue(File.Exists(ServerSecurityService.FileName));
            File.Delete(ServerSecurityService.FileName);
        }
    }
}
