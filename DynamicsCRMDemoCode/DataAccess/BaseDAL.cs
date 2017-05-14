using DynamicsCRMDemoPlugin.ContextComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsCRMDemoPlugin.DataAccess
{
    abstract class BaseDAL
    {
        protected PluginContext pluginContext;
        public BaseDAL(PluginContext passedPluginContext)
        {
            this.pluginContext = passedPluginContext;
        }
    }
}
