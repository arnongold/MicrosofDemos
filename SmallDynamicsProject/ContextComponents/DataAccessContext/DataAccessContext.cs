using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using DynamicsCRMDemoPlugin.Model;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    public class DataAccessContext
    {
        #region Properties
        public IOrganizationService OrganizationService { get; private set; }
        public ITracingService TracingService { get; private set; }
        public ServiceContext Context { get; private set; }
        #endregion

        #region Constructor
        public DataAccessContext(
            IOrganizationService organizationService,
            ITracingService tracingService)
        {
            OrganizationService = organizationService;
            TracingService = tracingService;
            Context = new ServiceContext(OrganizationService);
        }
        #endregion Constructor

    }
}

