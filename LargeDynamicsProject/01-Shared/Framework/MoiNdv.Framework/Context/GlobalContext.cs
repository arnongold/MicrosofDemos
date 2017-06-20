using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using MoiNdv.Framework.EndpointsComponents;
using System;

namespace MoiNdv.Framework.Context
{
    public class GlobalContext
    {

        protected int depth = 0;
        protected Guid userId = Guid.Empty;
        protected Guid initiatingUserId;
        protected Guid businessUnitId = Guid.Empty;
        protected WhoAmIResponse whoAmIResponse = null;
        protected GlobalContext globalContext;

        protected IOrganizationService OrganizationService { get; private set; }

        private ITracingService TracingService { get; set; }

        public EndpointTypeCode EndpointType { get; private set; }

        public Guid UserId
        {
            get
            {
                if (userId.Equals(Guid.Empty))
                {
                    userId = GetWhoAmIResponse().UserId;
                }

                return userId;
            }
        }

        public Guid InitiatingUserId
        {
            get
            {
                if (initiatingUserId.Equals(Guid.Empty))
                {
                    initiatingUserId = UserId;
                }

                return initiatingUserId;
            }

        }

        public Guid BusinessUnitId
        {
            get
            {
                if (businessUnitId.Equals(Guid.Empty))
                {
                    businessUnitId = GetWhoAmIResponse().BusinessUnitId;
                }

                return businessUnitId;
            }

        }

        public int Depth
        {
            get { return depth; }

        }

        public GlobalContext(GlobalContext globalContext)
        {
            OrganizationService = globalContext.OrganizationService;
            TracingService = globalContext.TracingService;
            EndpointType = globalContext.EndpointType;

            userId = globalContext.userId;
            initiatingUserId = globalContext.initiatingUserId;
            businessUnitId = globalContext.businessUnitId;
            depth = globalContext.depth;

        }

        public GlobalContext(IOrganizationService organizationService, ITracingService tracingService, EndpointTypeCode endpointType)
        {
            OrganizationService = organizationService;
            TracingService = tracingService;
            EndpointType = endpointType;
        }

        public GlobalContext(IOrganizationService organizationService, ITracingService tracingService, EndpointTypeCode endpointType, Guid userId) : this(organizationService, tracingService, endpointType)
        {
            this.userId = userId;
        }

        public GlobalContext(IOrganizationService organizationService, ITracingService tracingService, EndpointTypeCode endpointType, Guid userId, Guid businessUnitId) : this(organizationService, tracingService, endpointType, userId)
        {
            this.businessUnitId = businessUnitId;
        }

        public GlobalContext(IOrganizationService organizationService, ITracingService tracingService, EndpointTypeCode endpointType, Guid userId, Guid initiatingUserId, Guid businessUnitId, int depth) : this(organizationService, tracingService, endpointType, userId)
        {
            this.initiatingUserId = initiatingUserId;
            this.businessUnitId = businessUnitId;
            this.depth = depth;
        }

        public void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }


            TracingService.Trace(message);
        }

        private WhoAmIResponse GetWhoAmIResponse()
        {
            if (whoAmIResponse == null)
            {
                whoAmIResponse = ((WhoAmIResponse)OrganizationService.Execute(new WhoAmIRequest()));
            }

            return whoAmIResponse;

        }
    }
}
