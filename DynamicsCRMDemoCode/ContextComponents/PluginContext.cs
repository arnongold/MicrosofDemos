using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using DynamicsCRMDemoPlugin.Model;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    public class PluginContext
    {
        #region Properties
        public IServiceProvider ServiceProvider { get; private set; }
        public IOrganizationService OrganizationService { get; private set; }
        public IPluginExecutionContext PluginExecutionContext { get; private set; }
        public IServiceEndpointNotificationService NotificationService { get; private set; }
        public ParameterCollection InputParameters { get; private set; }
        public ITracingService TracingService { get; private set; }
        public Entity TargetEntity { get; private set; }
        public Entity PreImageEntity { get; private set; }
        public Entity PostImageEntity { get; private set; }
        public ServiceContext Context { get; private set; }
        #endregion

        #region Constructor
        public PluginContext(IServiceProvider sp)
        {
            InitializeContext(sp);
        }
        public void InitializeContext(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            // Obtain the execution context from the service provider.
            IPluginExecutionContext pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            OrganizationService = serviceFactory.CreateOrganizationService(pluginExecutionContext.UserId);
            InputParameters = pluginExecutionContext.InputParameters;

            if (pluginExecutionContext.InputParameters.Contains("Target") && pluginExecutionContext.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                TargetEntity = (Entity)pluginExecutionContext.InputParameters["Target"];
                PreImageEntity = (pluginExecutionContext.PreEntityImages.ContainsKey("PreImage")) ? (Entity)pluginExecutionContext.PreEntityImages["PreImage"] : null;
                PostImageEntity = (pluginExecutionContext.PostEntityImages.ContainsKey("PostImage")) ? (Entity)pluginExecutionContext.PostEntityImages["PostImage"] : null;
            }

            Context = new ServiceContext(OrganizationService);
        }
        #endregion Constructor



    }
}

