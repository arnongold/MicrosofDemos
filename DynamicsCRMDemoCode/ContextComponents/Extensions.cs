using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicsCRMDemoPlugin.ContextComponents;

namespace DynamicsCRMDemoPlugin.ContextComponents.Extensions
{
    public static class PluginContextExtensions
    {
        public static void Assign(this PluginContext pluginContext, EntityReference targetEntity, EntityReference assignee)
        {

            AssignRequest assign = new AssignRequest
            {
                Assignee = new EntityReference(assignee.LogicalName, assignee.Id),
                Target = new EntityReference(targetEntity.LogicalName, targetEntity.Id)
            };
            pluginContext.Service.Execute(assign);
        }

        public static void Share(this PluginContext pluginContext, EntityReference targetEntity, EntityReference shareWith, AccessRights accessRights)
        {
            // Grant the first user read access to the created lead.
            var grantAccessRequest = new GrantAccessRequest
            {
                PrincipalAccess = new PrincipalAccess
                {
                    AccessMask = accessRights,
                    Principal = shareWith
                },
                Target = targetEntity
            };

            pluginContext.Service.Execute(grantAccessRequest);
        }

        public static void Associate(this PluginContext pluginContext, string relationshipName, EntityReference entity1, EntityReference entity2)
        {
            var relationship = new Relationship(relationshipName);
            var relatedEntities = new EntityReferenceCollection();
            relatedEntities.Add(entity2);

            pluginContext.Service.Associate(entity1.LogicalName, entity1.Id, relationship, relatedEntities);
        }

        public static void Save(this PluginContext pluginContext, Entity targetEntity)
        {
            pluginContext.Context.UpdateObject(targetEntity);
            pluginContext.Service.Update(targetEntity);
        }
    }
}

