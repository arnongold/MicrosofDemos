using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsCRMDemoPlugin.BusinessLogic
{
    internal class ValidationResult
    {
        public bool HasError { get; set; } = false;
        public string ErrorDescription { get; set; }

    }
}
