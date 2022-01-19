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
        public AccountDAL(PassThroughContext passThroughContext) : base(passThroughContext) { }

        public Boolean IsAccountActive(string accountName)
        {
            var accountStatus = dataContext.Context.AccountSet.First(a => a.Name == accountName).StatusCode;
            return accountStatus.Value == 0;
        }

        public Account GetAccountbyID(Guid id)
        {
            var account = dataContext.Context.AccountSet.First(a => a.AccountId == id);
            return account;
        }
    }
}
