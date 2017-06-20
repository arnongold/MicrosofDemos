using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    public class CustomWorkflowActivityContext:IEndpointContext
    {
        public ITracingService TracingService { get; set; }
        public IOrganizationServiceFactory OrganizationServiceFactory { get; set; }
        public Guid CallingUserId { get; set; }
        public CustomWorkflowActivityContext(CodeActivityContext executionContext)
        {
            TracingService = executionContext.GetExtension<ITracingService>();
            OrganizationServiceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            CallingUserId = workflowContext.UserId;
        }

        public PassThroughContext ToPassthroughContext()
        {
            IOrganizationService organizationService = OrganizationServiceFactory.CreateOrganizationService(CallingUserId);
            var passThroughContext = new PassThroughContext(organizationService, TracingService);
            return passThroughContext;
        }
    }
}
