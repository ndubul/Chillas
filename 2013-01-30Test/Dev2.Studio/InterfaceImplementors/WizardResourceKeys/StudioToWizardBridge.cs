﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dev2.Studio.Core.AppResources;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Models;
using Dev2.Studio.Core.Interfaces;
using System.Xml.Linq;
using Unlimited.Framework;

using Dev2.Studio.Core;
using Dev2.DataList.Contract;

namespace Dev2.Studio.InterfaceImplementors.WizardResourceKeys {
    public static class StudioToWizardBridge {

        /// <summary>
        /// Returns the correct wizard endpoint depending upon its type
        /// </summary>
        /// <param name="theModel"></param>
        /// <returns></returns>
        public static string SelectWizard(IResourceModel theModel) {
            string result = "Dev2ServiceDetails"; // defaults to the service wizard

            // else figure out which source wizard to open
            if (theModel.ResourceType == ResourceType.Source) {
                if(theModel.ServiceDefinition.IndexOf("Type=\"Plugin\"") > 0){
                    result = "PluginSourceManagement";
                }else if(theModel.ServiceDefinition.IndexOf("Type=\"SqlDatabase\"") > 0){
                    result = "DatabaseSourceManagement";
                }
            }

            return result;
        }

        /// <summary>
        /// Perform the translation between studio and server resouce types
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public static string ConvertStudioToWizardType(string resourceType, string serviceDef, string category) {
            string result = resourceType;

            if (resourceType == "DatabaseService")
            {
                result = "Database";
            }
            else if (resourceType == "ResourceService")
            {
                result = "Plugin";
            }
            else if (resourceType == "Service" && serviceDef.IndexOf("Type=\"Plugin\"") > 0)
            {
                result = "Plugin";
            }
            else if (resourceType == "Service" && serviceDef.IndexOf("Type=\"Plugin\"") < 0) {
                result = "Database";
            }
            else if (resourceType == "Source" && serviceDef.IndexOf("AssemblyLocation=") > 0)
            {
                result = "Plugin";
            }
            else if (resourceType == "Source" && serviceDef.IndexOf("AssemblyLocation=") < 0) {
                result = "Database";
            }
            else if (resourceType == "HumanInterfaceProcess" || category == "Webpage") {
                result = "Webpage";
            }else if(category == "Website"){
                result = "Website";
            }
            else if (resourceType == "WorkflowService") {
                result = "Workflow";
            }

            return result;
        }

        /// <summary>
        /// Builds up the POST data for editing a resource
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public static string BuildStudioEditPayload(string resourceType, IResourceModel rm) {
            StringBuilder result = new StringBuilder();
            string resType = StudioToWizardBridge.ConvertStudioToWizardType(resourceType, rm.ServiceDefinition, rm.Category);

            // add service name
            result.Append(ResourceKeys.Dev2ServiceName);
            result.Append("=");
            result.Append(rm.ResourceName);

            // add service type
            result.Append("&");
            result.Append(ResourceKeys.Dev2ServiceType);
            result.Append("=");
            result.Append(resType);

            // add category
            result.Append("&");
            result.Append(ResourceKeys.Dev2Category);
            result.Append("=");
            result.Append(rm.Category);

            // add help
            result.Append("&");
            result.Append(ResourceKeys.Dev2Help);
            result.Append("=");
            result.Append(rm.HelpLink); // rm.HelpLink

            // add icon
            result.Append("&");
            result.Append(ResourceKeys.Dev2Icon);
            result.Append("=");
            result.Append(rm.IconPath);

            // add comment
            result.Append("&");
            result.Append(ResourceKeys.Dev2Description);
            result.Append("=");
            result.Append(rm.Comment);

            // add tags
            result.Append("&");
            result.Append(ResourceKeys.Dev2Tags);
            result.Append("=");
            result.Append(rm.Tags);

            // add tooltip text -- ??
            //result.Append("&");
            //result.Append(ResourceKeys.Dev2TooltipText);
            //result.Append("=");
            //result.Append(rm.T);

            // ServiceDefinition
            // <Action Name="EmailService" Type="Plugin" SourceName="Email Plugin" SourceMethod="Send">

            string serviceDef = rm.ServiceDefinition;

            if (serviceDef.IndexOf("SourceName=") > 0) {
                // we have 
                string sourceName = DataListUtil.ExtractAttribute(serviceDef, "Action", "SourceName");
                string sourceMethod = DataListUtil.ExtractAttribute(serviceDef, "Action", "SourceMethod");

                // add source method
                result.Append("&");
                result.Append(ResourceKeys.Dev2SourceMethod);
                result.Append("=");
                result.Append(sourceMethod);

                // add source name
                result.Append("&");
                result.Append(ResourceKeys.Dev2SourceName);
                result.Append("=");
                result.Append(sourceName);

                result.Append("&");
                result.Append(ResourceKeys.Dev2StudioExe);
                result.Append("=");
                result.Append("yes");
            }
            else if (serviceDef.IndexOf("<Source") >= 0) {
                // we have a source to process 
                if (resType == "Plugin") {
                    result.Append("&");
                    result.Append(ResourceKeys.Dev2SourceManagementSource);
                    result.Append("=");
                    result.Append(rm.ResourceName.ToString());
                }
                else if (resType == "Database") {
                    result.Append("&");
                    result.Append(ResourceKeys.Dev2SourceManagementDatabaseSource);
                    result.Append("=");
                    result.Append(rm.ResourceName.ToString());                    
                }

                result.Append("&");
                result.Append(ResourceKeys.Dev2SourceName);
                result.Append("=");
                result.Append(rm.ResourceName.ToString());

                result.Append("&");
                result.Append(ResourceKeys.Dev2StudioExe);
                result.Append("=");
                result.Append("yes");

            }

            return result.ToString();
        }
    }
}
