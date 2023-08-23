using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPrice
{
    internal static class Methods
    {

        internal static double?[] CheckPrice(IElement element)
        {
            double? discountPrice = double.Parse(new String(element.QuerySelector(".moneyprice__roubles").TextContent.Where(Char.IsDigit).ToArray()));
            double price = (double)discountPrice;
            string qw;
            if (element.QuerySelector(".card-price__nodisc") != null)
            {
                price = double.Parse(element.QuerySelector(".card-price__nodisc > s").TextContent.Replace('.', ','));
            }
            else
            {
                discountPrice = null;
            }
            return new double?[] { discountPrice, price };
        }
    }
}
