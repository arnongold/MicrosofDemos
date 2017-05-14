using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using DynamicsCRMDemoPlugin.ContextComponents;
using DynamicsCRMDemoPlugin.BusinessLogic.AccountBL;
using DynamicsCRMDemoPlugin.Model;

namespace DynamicsCRMDemoPlugin.PluginEndpoints
{
    public class SamplePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginContext context = new PluginContext(serviceProvider);
            var accountValidator = new AccountBL(context);

            var result = accountValidator.ValidateNewAccount(context.TargetEntity.ToEntity<Account>());
            if (result.ErrorCode != 0)
            {
                throw new InvalidPluginExecutionException(result.ErrorDescription);
            }

        }
    }
}
