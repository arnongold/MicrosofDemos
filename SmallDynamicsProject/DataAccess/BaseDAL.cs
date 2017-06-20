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
        protected DataAccessContext dataContext;
        public BaseDAL(PassThroughContext passedPluginContext)
        {
            this.dataContext = passedPluginContext.ToDataAccessContext();
        }
    }
}
