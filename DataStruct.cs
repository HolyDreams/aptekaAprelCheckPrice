using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPrice
{
    internal class CatalogList
    {
        internal string Name { get; set; }
        internal string Company { get; set; }
        internal double? Price { get; set; }
        internal double? DiscontPrice { get; set; }
    }
}
