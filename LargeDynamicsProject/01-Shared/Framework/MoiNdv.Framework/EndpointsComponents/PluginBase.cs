using Microsoft.Xrm.Sdk;
using MoiNdv.Framework.Context;
using System;
using System.Globalization;
using System.ServiceModel;

namespace MoiNdv.Framework.EndpointsComponents
{
    public abstract class PluginBase : IPlugin, IEndpoint
    {

        protected class LocalContext
        {

            public IPluginExecutionContext PluginExecutionContext { get; private set; }

            public IOrganizationServiceFactory OrganizationServiceFactory { get; private set; }

            public IServiceEndpointNotificationService NotificationService { get; private set; }

            public Entity TargetEntity { get; private set; }

            public Entity PreEntity { get; private set; }

            public Entity PostEntity { get; private set; }

            private LocalContext() { }

            internal LocalContext(IServiceProvider serviceProvider)
            {
                if (serviceProvider == null)
                {
                    throw new InvalidPluginExecutionException("serviceProvider");
                }

                PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                NotificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));

                OrganizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));


                if (PluginExecutionContext.InputParameters.Contains("Target") && PluginExecutionContext.InputParameters["Target"] is Entity)
                {

                    TargetEntity = (PluginExecutionContext.InputParameters.Contains("Target") &&
                                            PluginExecutionContext.InputParameters["Target"] is Entity)
                                            ? PluginExecutionContext.InputParameters["Target"] as Entity
                                            : null;

                    PreEntity = (PluginExecutionContext.PreEntityImages != null &&
                                        PluginExecutionContext.PreEntityImages.Contains("PreImage"))
                                        ? PluginExecutionContext.PreEntityImages["PreImage"]
                                        : null;

                    PostEntity = (PluginExecutionContext.PostEntityImages != null &&
                                         PluginExecutionContext.PostEntityImages.Contains("PostImage"))
                                         ? PluginExecutionContext.PostEntityImages["PostImage"]
                                         : null;
                }
            }
        }

        protected GlobalContext GlobalContext { get; set; }

        protected string ChildClassName { get; private set; }

        protected PluginBase(Type childClassName)
        {
            ChildClassName = childClassName.ToString();
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new InvalidPluginExecutionException("serviceProvider");
            }

            LocalContext localcontext = new LocalContext(serviceProvider);

            IOrganizationService orgService = localcontext.OrganizationServiceFactory.CreateOrganizationService(localcontext.PluginExecutionContext.UserId);
            ITracingService traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            GlobalContext = new GlobalContext(orgService, traceService, EndpointTypeCode.Plugin, localcontext.PluginExecutionContext.UserId, localcontext.PluginExecutionContext.InitiatingUserId, localcontext.PluginExecutionContext.BusinessUnitId, localcontext.PluginExecutionContext.Depth);

            GlobalContext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", this.ChildClassName));

            try
            {
                ExecuteCrmPlugin(localcontext);

                return;
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


        protected virtual void ExecuteCrmPlugin(LocalContext localcontext)
        {
            // Do nothing. 
        }

        public T CreateBLInstance<T>() where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), GlobalContext);
        }
    }
}
