using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.IntegrationTests.TestImplementations
{
    public class ItemNumber
    {
        public Guid Id { get; set; }
        public string VendorNumber { get; set; }
        public string VendorName { get; set; }
        public string InvoiceItemNumber { get; set; }
        public string JDENumber { get; set; }
    }
}
