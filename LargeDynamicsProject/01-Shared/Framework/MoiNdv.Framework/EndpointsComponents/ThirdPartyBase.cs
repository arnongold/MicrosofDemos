using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using MoiNdv.Framework.Context;
using System;
using System.Globalization;
using System.ServiceModel;

namespace MoiNdv.Framework.EndpointsComponents
{
    public class ThirdPartyBase : IEndpoint
    {

        public GlobalContext GlobalContext { get; private set; }

        protected string ChildClassName { get; private set; }

        public ThirdPartyBase(Type childClassName, string crmConnectionString)
        {
            ChildClassName = childClassName.ToString();

            CrmServiceClient crmServiceClient = new
                  CrmServiceClient(crmConnectionString);

            Execute(crmServiceClient);
        }

        private void Execute(CrmServiceClient crmServiceClient)
        {
            try
            {


                IOrganizationService orgService = (IOrganizationService)crmServiceClient.OrganizationWebProxyClient != null ? (IOrganizationService)crmServiceClient.OrganizationWebProxyClient : (IOrganizationService)crmServiceClient.OrganizationServiceProxy;
                ITracingService traceService = new ThirdPartyTracingService();

                GlobalContext = new GlobalContext(orgService, traceService, EndpointTypeCode.ThirdParty, crmServiceClient.CallerId);

                GlobalContext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", this.ChildClassName));

            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                GlobalContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", e.ToString()));

                throw new InvalidPluginExecutionException("OrganizationServiceFault", e);
            }
            finally
            {
                GlobalContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exiting {0}.Execute()", ChildClassName));
            }
        }

        public T CreateBLInstance<T>() where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), GlobalContext);
        }
    }
}
