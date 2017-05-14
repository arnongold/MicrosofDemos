using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicsCRMDemoPlugin.ContextComponents;
using DynamicsCRMDemoPlugin.Model;
using DynamicsCRMDemoPlugin.DataAccess.AccountDAL;

namespace DynamicsCRMDemoPlugin.BusinessLogic.AccountBL
{
    internal class AccountBL:BaseBL
    {
        public AccountBL(PluginContext pluginContext) : base(pluginContext) { }
        public ValidationResult ValidateNewAccount(Account accountToValidate)
        {
 
            AccountDAL dal = new AccountDAL(pluginContext);
            if (dal.IsAccountActive(accountToValidate.Name))
            {
                return new ValidationResult()
                {
                    ErrorCode = 123,
                    ErrorDescription = "Invalid opration on a closed account"
                };
            }
            return new ValidationResult() { ErrorCode = 0 } ;
        }
    }
}
