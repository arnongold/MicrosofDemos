using MoiNdv.BusinessLogic;
using MoiNdv.Framework.EndpointsComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiNdv.Crm.Plugins.Contact
{
    public class PostCreateContact : PluginBase
    {
        public PostCreateContact(string unsecure, string secure)
            : base(typeof(PostCreateContact))
        {

            // TODO: Implement your custom configuration handling.
        }

        protected override void ExecuteCrmPlugin(LocalContext localContext)
        {

            GlobalContext.Trace("PostCreateContact Executed()");

            ContactBL contactBl = CreateBLInstance<ContactBL>();

            var result = contactBl.ValidateNewContact(localContext.TargetEntity.ToEntity<DataModel.Contact>());

        }
    }
}
