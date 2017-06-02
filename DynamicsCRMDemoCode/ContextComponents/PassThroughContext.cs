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
        #region TracingService
        private ITracingService tracingService;
        public ITracingService TracingService
        {
            get { return tracingService; }
        }
        #endregion
        #region ServiceProvider
        private IServiceProvider serviceProvider;
        public IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
        }
        #endregion

        public PassThroughContext(IServiceProvider sp)
        {
            serviceProvider = sp;
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            tracingService = (ITracingService)sp.GetService(typeof(ITracingService));
        }

    }
}
