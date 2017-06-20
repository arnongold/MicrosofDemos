using MoiNdv.DataAccessLevel;
using MoiNdv.Framework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiNdv.BusinessLogic
{
    public abstract class BaseBL
    {
        protected GlobalContext GlobalContext { get; private set; }

        public BaseBL(GlobalContext globalContext)
        {
            GlobalContext = globalContext;
           

        }
    }
}
