using MoiNdv.Framework.Context;
using System;
using System.Linq;

namespace MoiNdv.DataAccessLevel
{
    public class ContactDAL : BaseDAL
    {
        public ContactDAL(GlobalContext globalContext) : base(globalContext) { }

        public Boolean IsAccountActive(string contactName)
        {
            var contactStatus = Context.ContactSet.First(a => a.FullName == contactName).StatusCode;
            return contactStatus.Value == 0;

        }
    }
}
