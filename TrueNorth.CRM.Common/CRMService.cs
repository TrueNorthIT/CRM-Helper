using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueNorth.CRM.Common
{
    public abstract class CRMService
    {

        internal Lazy<IOrganizationService> serviceFactory { get; set; }
        public IOrganizationService service => serviceFactory.Value;

    }
}
