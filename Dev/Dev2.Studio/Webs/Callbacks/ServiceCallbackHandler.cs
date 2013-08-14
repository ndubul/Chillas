﻿using System;
using System.Linq;
using System.Xml.Linq;
using Caliburn.Micro;
using Dev2.DataList.Contract;
using Dev2.Services.Events;
using Dev2.Studio.Core;
using Dev2.Studio.Core.AppResources.Browsers;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.InterfaceImplementors.WizardResourceKeys;

namespace Dev2.Studio.Webs.Callbacks
{
    public class ServiceCallbackHandler : WebsiteCallbackHandler
    {
        bool _isEditingSource;
        string _returnUri;
        IEnvironmentModel _environmentModel = null;

        public ServiceCallbackHandler()
            : this(EnvironmentRepository.Instance)
        {
        }

        public ServiceCallbackHandler(IEnvironmentRepository currentEnvironmentRepository)
            : base(EventPublishers.Aggregator, currentEnvironmentRepository)
        {
        }

        public ServiceCallbackHandler(IEventAggregator eventPublisher, IEnvironmentRepository currentEnvironmentRepository, IShowDependencyProvider provider)
            : base(eventPublisher, currentEnvironmentRepository, null, provider)
        {
        }

        protected override void Save(IEnvironmentModel environmentModel, dynamic jsonObj)
        {
            _environmentModel = environmentModel;
            var getDynamicResourceType = jsonObj.ResourceType.Value;
            string resourceName = jsonObj.ResourceName.Value;
            if(getDynamicResourceType != null)
            {
                //2013.04.29: Ashley Lewis - PBI 8721 database source and plugin source wizards can be called from with their respective service wizards
                if(getDynamicResourceType == Data.ServiceModel.ResourceType.DbSource.ToString() || getDynamicResourceType == Data.ServiceModel.ResourceType.PluginSource.ToString())
                {
                    //2013.03.12: Ashley Lewis - BUG 9208
                    ReloadResource(environmentModel, resourceName, ResourceType.Source);
                }
                else
                {
                    ReloadResource(environmentModel, resourceName, ResourceType.Service);
                }
            }
            else
            {
                ReloadResource(environmentModel, resourceName, ResourceType.Service);
            }

        }

        protected override void Navigate(IEnvironmentModel environmentModel, string uri, dynamic jsonArgs, string returnUri)
        {
            if(environmentModel == null || environmentModel.ResourceRepository == null || jsonArgs == null)
            {
                return;
            }

            Guid dataListID;
            var relativeUri = "/services/DatabaseSourceManagement";
            var sourceName = jsonArgs.ResourceName.Value;
            var contextualResource = string.IsNullOrEmpty(sourceName)
                                         ? null
                                         : environmentModel.ResourceRepository.All().FirstOrDefault(r => r.ResourceName.Equals(sourceName, StringComparison.InvariantCultureIgnoreCase)) as IContextualResourceModel;
            if(contextualResource == null)
            {
                relativeUri += "?Dev2ServiceType=Database";
                dataListID = Guid.Empty;
            }
            else
            {
                ErrorResultTO errors;
                var args = StudioToWizardBridge.BuildStudioEditPayload(contextualResource.ResourceType.ToString(), contextualResource);
                dataListID = environmentModel.UploadToDataList(args, out errors);
            }

            Uri requestUri;
            if(!Uri.TryCreate(environmentModel.Connection.WebServerUri, relativeUri, out requestUri))
            {
                requestUri = new Uri(new Uri(StringResources.Uri_WebServer), relativeUri);
            }
            var uriString = Browser.FormatUrl(requestUri.AbsoluteUri, dataListID);

            _isEditingSource = true;
            _returnUri = returnUri;
            Navigate(uriString);
        }

        public override void Cancel()
        {
            if(_isEditingSource)
            {
                _isEditingSource = false;
                Navigate(_returnUri);
            }
            else
            {
                Close();
            }
        }

        public override void Dev2SetValue(string value)
        {
            if(_isEditingSource)
            {
                // DB source invokes this
                var xml = XElement.Parse(value);
                var sourceName = xml.ElementSafe("ResourceName");
                NavigateBack(sourceName);
            }
        }

        public override void Dev2ReloadResource(string resourceName, string resourceType)
        {
            if(_isEditingSource)
            {
                // DB source invoked this from new window
                NavigateBack(resourceName);
            }
        }

        #region NavigateBack

        void NavigateBack(string sourceName)
        {
            var uri = _returnUri;
            _isEditingSource = false;
            _returnUri = null;

            const string SourceParam = "sourceName=";
            var idx = uri.IndexOf(SourceParam, StringComparison.InvariantCultureIgnoreCase);
            if(idx > 0)
            {
                var start = idx + SourceParam.Length;
                var end = uri.IndexOf('&', start);
                end = end > 0 ? end : uri.Length;
                uri = uri.Remove(start, (end - start));
                uri = uri.Insert(start, sourceName);
            }
            else
            {
                uri += (uri.IndexOf('?') > 0 ? "&" : "?") + SourceParam + sourceName;
            }

            Navigate(uri);
            ReloadResource(_environmentModel, sourceName, ResourceType.Source);
        }

        #endregion

    }
}
