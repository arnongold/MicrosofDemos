using System;
using Microsoft.Xrm.Sdk;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    public class PluginContext:IEndpointContext
    {
        #region Properties
        public IServiceProvider ServiceProvider { get; private set; }
        public IPluginExecutionContext PluginExecutionContext { get; private set; }
        public ParameterCollection InputParameters { get; private set; }
        public ITracingService TracingService { get; private set; }
        public Entity TargetEntity { get; private set; }
        public Entity PreImageEntity { get; private set; }
        public Entity PostImageEntity { get; private set; }
        public Guid CallingUserId { get; set; }


        private IOrganizationServiceFactory organizationServiceFactory;
        private IPluginExecutionContext pluginExecutionContext;
        #endregion

        public PluginContext(IServiceProvider serviceProvider)
        {
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            InputParameters = pluginExecutionContext.InputParameters;
            CallingUserId = pluginExecutionContext.UserId;
            if (pluginExecutionContext.InputParameters.Contains("Target") && pluginExecutionContext.InputParameters["Target"] is Entity)
            {
                TargetEntity = (Entity)pluginExecutionContext.InputParameters["Target"];
                PreImageEntity = (pluginExecutionContext.PreEntityImages.ContainsKey("PreImage")) ? (Entity)pluginExecutionContext.PreEntityImages["PreImage"] : null;
                PostImageEntity = (pluginExecutionContext.PostEntityImages.ContainsKey("PostImage")) ? (Entity)pluginExecutionContext.PostEntityImages["PostImage"] : null;
            }

        }

        public PassThroughContext ToPassthroughContext()
        {
           
            IOrganizationService organizationService = organizationServiceFactory.CreateOrganizationService(CallingUserId);
            var passThroughContext = new PassThroughContext(organizationService,TracingService);
            return passThroughContext;
        }



    }
}

