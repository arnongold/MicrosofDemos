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
        public IServiceProvider ServiceProvider { get; private set; }
        public ITracingService TracingService { get; private set; }

        public PassThroughContext(IServiceProvider sp)
        {
            ServiceProvider = sp;
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            TracingService = (ITracingService)sp.GetService(typeof(ITracingService));
        }

    }
}
