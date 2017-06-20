using MoiNdv.DataAccessLevel;
using MoiNdv.DataModel;
using MoiNdv.Framework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiNdv.BusinessLogic
{
    public class ContactBL : BaseBL
    {
        public ContactBL(GlobalContext globalContext) : base(globalContext) { }


        public bool ValidateNewContact(Contact accountToValidate)
        {
            ContactDAL ContacDal = new ContactDAL(GlobalContext);

            ////Throw spcific Dal
            //ContacDal.Create<Contact>(new Contact());


            return ContacDal.IsAccountActive(accountToValidate.FullName);




        }
    }
}
