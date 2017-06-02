using DynamicsCRMDemoPlugin.ContextComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsCRMDemoPlugin.BusinessLogic
{
    public abstract class BaseBL
    {
        protected PassThroughContext pluginContext;
        public BaseBL(PassThroughContext passedPluginContext)
        {
            this.pluginContext = passedPluginContext;
        }
    }
}
