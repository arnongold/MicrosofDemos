using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using DynamicsCRMDemoPlugin.Model;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    public class PluginContext
    {
        #region TracingService
        private ITracingService tracingService;
        public ITracingService TracingService
        {
            get { return tracingService; }
        }
        #endregion
        #region InputParameters
        private ParameterCollection inputParameters;
        public ParameterCollection InputParameters
        {
            get
            {
                return inputParameters;
            }
        }
        #endregion
        #region TargetEntity
        private Entity targetEntity;
        public Entity TargetEntity
        {
            get
            {
                return targetEntity;
            }
        }
        #endregion
        #region Relationship
        private Relationship relationship;
        public Relationship Relationship
        {
            get
            {
                return relationship;
            }
        }
        #endregion
        #region RelatedEntities
        private EntityReferenceCollection relatedEntities;
        public EntityReferenceCollection RelatedEntities
        {
            get
            {
                return relatedEntities;
            }
        }
        #endregion
        #region PreImageEntity
        private Entity preImageEntity;
        public Entity PreImageEntity
        {
            get
            {
                return preImageEntity;
            }
        }
        #endregion
        #region PostImageEntity
        private Entity postImageEntity;
        public Entity PostImageEntity
        {
            get
            {
                return postImageEntity;
            }
        }
        #endregion
        #region RelatedTargetEntitiy
        private EntityReference relatedTargetEntitiy;
        public EntityReference RelatedTargetEntitiy
        {
            get
            {
                return relatedTargetEntitiy;
            }
        }
        #endregion
        #region Assignee
        private EntityReference assignee;
        public EntityReference Assignee
        {
            get
            {
                return assignee;
            }
        }
        #endregion
        #region OragnizationService
        private IOrganizationService organiztionService;
        public IOrganizationService Service
        {
            get
            {
                return organiztionService;
            }
        }
        #endregion
        #region ServiceXontext
        private ServiceContext context;
        public ServiceContext Context
        {
            get
            {
                return context;
            }
        }
        #endregion 

        #region Constructor
        public PluginContext(IServiceProvider sp)
        {
            InitializeContext(sp);
        }
        public void InitializeContext(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            // Obtain the execution context from the service provider.
            IPluginExecutionContext pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            organiztionService = serviceFactory.CreateOrganizationService(pluginExecutionContext.UserId);
            inputParameters = pluginExecutionContext.InputParameters;

            if (pluginExecutionContext.InputParameters.Contains("Target") && pluginExecutionContext.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                targetEntity = (Entity)pluginExecutionContext.InputParameters["Target"];
                preImageEntity = (pluginExecutionContext.PreEntityImages.ContainsKey("PreImage")) ? (Entity)pluginExecutionContext.PreEntityImages["PreImage"] : null;
                postImageEntity = (pluginExecutionContext.PostEntityImages.ContainsKey("PostImage")) ? (Entity)pluginExecutionContext.PostEntityImages["PostImage"] : null;
            }

            if (pluginExecutionContext.InputParameters.Contains("Relationship") && pluginExecutionContext.InputParameters["Relationship"] is Relationship)
            {
                relationship = pluginExecutionContext.InputParameters.ContainsKey("Relationship") ? (Relationship)pluginExecutionContext.InputParameters["Relationship"] : null;
                relatedEntities = pluginExecutionContext.InputParameters.ContainsKey("RelatedEntities") ? (EntityReferenceCollection)pluginExecutionContext.InputParameters["RelatedEntities"] : null;
                relatedTargetEntitiy = pluginExecutionContext.InputParameters.ContainsKey("Target") ? (EntityReference)pluginExecutionContext.InputParameters["Target"] : null;
            }

            if (pluginExecutionContext.InputParameters.Contains("Assignee") && pluginExecutionContext.InputParameters["Assignee"] is EntityReference)
            {
                assignee = (EntityReference)pluginExecutionContext.InputParameters["Assignee"];
                relatedTargetEntitiy = pluginExecutionContext.InputParameters.ContainsKey("Target") ? (EntityReference)pluginExecutionContext.InputParameters["Target"] : null;
            }
            context = new ServiceContext(organiztionService);
        }
        #endregion Constructor



    }
}

