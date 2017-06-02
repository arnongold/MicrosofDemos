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
    /// </summary>
    public class PassThroughContext
    {
        public IServiceProvider ServiceProvider { get; set; }
        public PassThroughContext(IServiceProvider sp)
        {
            ServiceProvider = sp;
        }

    }
}
