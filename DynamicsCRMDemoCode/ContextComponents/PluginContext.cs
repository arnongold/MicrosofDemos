using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using DynamicsCRMDemoPlugin.Model;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    public class PluginContext
    {
        #region Members
        private Entity targetEntity;
        private Relationship relationship;
        private EntityReferenceCollection relatedEntities;
        private Entity preImageEntity;
        private Entity postImageEntity;
        private EntityReference relatedTargetEntitiy;
        private EntityReference assignee;
        private IOrganizationService organiztionService;
        private ServiceContext context;
        private ParameterCollection inputParameters;
        #endregion Members

        #region Properties

        public ParameterCollection InputParameters
        {
            get
            {
                return inputParameters;
            }
        }
        public Entity TargetEntity
        {
            get
            {
                return targetEntity;
            }
        }
        public Relationship Relationship
        {
            get
            {
                return relationship;
            }
        }
        public EntityReferenceCollection RelatedEntities
        {
            get
            {
                return relatedEntities;
            }
        }
        public Entity PreImageEntity
        {
            get
            {
                return preImageEntity;
            }
        }
        public Entity PostImageEntity
        {
            get
            {
                return postImageEntity;
            }
        }
        public EntityReference RelatedTargetEntitiy
        {
            get
            {
                return relatedTargetEntitiy;
            }
        }

        public EntityReference Assignee
        {
            get
            {
                return assignee;
            }
        }
        public IOrganizationService Service
        {
            get
            {
                return organiztionService;
            }
        }

        public ServiceContext Context
        {
            get
            {
                return context;
            }
        }
        #endregion Properties

        #region Constructor
        public PluginContext(IServiceProvider sp)
        {
            InitializeContext(sp);
        }

        public void InitializeContext(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
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

        #region helper functions

        public string GetOptionsetText(Entity entity, IOrganizationService service, string optionsetName, int optionsetValue)
        {
            string optionsetSelectedText = string.Empty;
            try
            {
                RetrieveOptionSetRequest retrieveOptionSetRequest =
                    new RetrieveOptionSetRequest
                    {
                        Name = optionsetName
                    };

                // Execute the request.
                RetrieveOptionSetResponse retrieveOptionSetResponse =
                    (RetrieveOptionSetResponse)service.Execute(retrieveOptionSetRequest);

                // Access the retrieved OptionSetMetadata.
                OptionSetMetadata retrievedOptionSetMetadata = (OptionSetMetadata)retrieveOptionSetResponse.OptionSetMetadata;

                // Get the current options list for the retrieved attribute.
                OptionMetadata[] optionList = retrievedOptionSetMetadata.Options.ToArray();
                foreach (OptionMetadata optionMetadata in optionList)
                {
                    if (optionMetadata.Value == optionsetValue)
                    {
                        optionsetSelectedText = optionMetadata.Label.UserLocalizedLabel.Label.ToString();
                        break;
                    }
                }
            }
            catch (Exception)
            {


                throw;
            }
            return optionsetSelectedText;
        }

        /// <summary>
        /// Assosiate massage: Get Entity 2 reference from ”RelatedEntities” Key from context (Judge)
        /// </summary>
        /// <returns>Related entity from association</returns>
        public Entity GetEntityReferenceFromRelatedEntitiesKey()
        {
            Entity relatedEntity = null;
            EntityReferenceCollection relatedEntities = null;
            EntityReference relatedEntityReference = null;
            if (this.InputParameters.Contains("RelatedEntities") && this.InputParameters["RelatedEntities"] is EntityReferenceCollection)
            {
                relatedEntities = this.InputParameters["RelatedEntities"] as EntityReferenceCollection;
                if (relatedEntities.Count > 0)
                {
                    relatedEntityReference = relatedEntities[0];
                    relatedEntity = new Entity(relatedEntities[0].LogicalName, relatedEntities[0].Id);
                }
            }
            return relatedEntity;
        }

        /// <summary>
        ///  Associate message: Get Entity 1 reference from “Target” Key from context (Position)
        /// </summary>     
        public Entity GetEntityReferenceFromTargetKey()
        {
            EntityReference targetEntityReference = null;
            Entity target = null;
            if (this.InputParameters.Contains("Target") && this.InputParameters["Target"] is EntityReference)
            {
                targetEntityReference = (EntityReference)this.InputParameters["Target"];
                target = new Entity(targetEntityReference.LogicalName, targetEntityReference.Id);
            }
            return target;
        }
        #endregion

    }
}

