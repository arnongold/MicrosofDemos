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
    public class Account_PreCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PassThroughContext context = new PassThroughContext(serviceProvider);
            var accountValidator = new AccountBL(context);

            // only if needed locally - create a full plugin context
            PluginContext localContext = new PluginContext(serviceProvider);
            var result = accountValidator.ValidateNewAccount(localContext.TargetEntity.ToEntity<Account>());

            if (result.HasError)
            {
                throw new InvalidPluginExecutionException(result.ErrorDescription);
            }

        }
    }
}
