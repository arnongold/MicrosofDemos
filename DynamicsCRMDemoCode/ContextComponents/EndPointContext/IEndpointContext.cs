using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsCRMDemoPlugin.ContextComponents
{
    public interface IEndpointContext
    {
        PassThroughContext ToPassthroughContext();

    }
}
