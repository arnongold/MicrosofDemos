using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    /// <summary>
    /// This context only keeps the Service Provider in a local var
    /// It is used for BL to pass through the context to the DAL
    /// Only Tracing Service is available 
    /// </summary>
    public class PassThroughContext
    {
        private IOrganizationService OrganizationService;
        public ITracingService TracingService { get; private set; }

        public PassThroughContext(
            IOrganizationService organizationService,
            ITracingService tracingService)
        {
            OrganizationService = organizationService;
            TracingService = tracingService;
        }

        public DataAccessContext ToDataAccessContext()
        {
            var dataAccessContext = new DataAccessContext(OrganizationService,TracingService);
            return dataAccessContext;
        }

    }
}
