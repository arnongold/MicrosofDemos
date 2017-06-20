using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using MoiNdv.Framework.Context;
using System;
using System.Activities;
using System.Globalization;
using System.ServiceModel;

namespace MoiNdv.Framework.EndpointsComponents
{
    public abstract class WorkflowBase : CodeActivity, IEndpoint
    {
        protected class LocalContext
        {

            internal IWorkflowContext WorkfolwExecutionContext { get; private set; }

            internal ITracingService TracingService { get; private set; }

            internal IOrganizationServiceFactory OrganizationServiceFactory { get; private set; }

            private LocalContext() { }

            internal LocalContext(CodeActivityContext codeActivityContext)
            {
                TracingService = codeActivityContext.GetExtension<ITracingService>();
                WorkfolwExecutionContext = codeActivityContext.GetExtension<IWorkflowContext>();
                OrganizationServiceFactory = codeActivityContext.GetExtension<IOrganizationServiceFactory>();
            }
        }

        internal WorkflowBase(Type childClassName)
        {
            ChildClassName = childClassName.ToString();
        }

        protected GlobalContext GlobalContext { get; set; }

        protected string ChildClassName { get; private set; }

        protected override void Execute(CodeActivityContext executionContext)
        {

            LocalContext localcontext = new LocalContext(executionContext);

            IOrganizationService orgService = localcontext.OrganizationServiceFactory.CreateOrganizationService(localcontext.WorkfolwExecutionContext.UserId);
            ITracingService traceService = executionContext.GetExtension<ITracingService>();

            GlobalContext = new GlobalContext(orgService, traceService, EndpointTypeCode.Workfolw, localcontext.WorkfolwExecutionContext.UserId, localcontext.WorkfolwExecutionContext.InitiatingUserId, localcontext.WorkfolwExecutionContext.BusinessUnitId, localcontext.WorkfolwExecutionContext.Depth);

            GlobalContext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", this.ChildClassName));

            try
            {
                ExecuteCrmWorkflow(localcontext);

                return;
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                GlobalContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", e.ToString()));

                throw new InvalidPluginExecutionException("OrganizationServiceFault", e);
            }
            finally
            {
                GlobalContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exiting {0}.Execute()", this.ChildClassName));
            }
        }


        protected virtual void ExecuteCrmWorkflow(LocalContext localcontext)
        {
            // Do nothing. 
        }

        public T CreateBLInstance<T>() where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), GlobalContext);
        }
    }
}
