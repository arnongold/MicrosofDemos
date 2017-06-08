using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsCRMDemoPlugin.ContextComponents.EndPointContext
{
    class StandAloneContext : IEndpointContext
    {
        public ITracingService TracingService { get; set; }
        private IOrganizationService OrganizationService;
        public StandAloneContext(string Url, string UserName, string Password,string autoType)
        {
            var connectionString = $"AuthType={autoType};Url={Url}; Username={UserName}; Password={Password}";
            TracingService = new ConsoleTracingService();
            CrmServiceClient crmConn = new CrmServiceClient(connectionString);
            OrganizationService = crmConn.OrganizationServiceProxy;
        }
        public PassThroughContext ToPassthroughContext()
        {
            var passThroughContext = new PassThroughContext(OrganizationService, TracingService);
            return passThroughContext;
        }
    }
}
