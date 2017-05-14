using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicsCRMDemoPlugin.ContextComponents;
using DynamicsCRMDemoPlugin.Model;

namespace DynamicsCRMDemoPlugin.DataAccess.AccountDAL
{
    internal class AccountDAL : BaseDAL
    {
        public AccountDAL(PluginContext pluginContext) : base(pluginContext) { }

        public Boolean IsAccountActive(string accountName)
        {
            var accountStatus = pluginContext.Context.AccountSet.First(a => a.Name == accountName).StatusCode;
            return accountStatus.Value == 0;
        }
    }
}
