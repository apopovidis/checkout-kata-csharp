using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutKata
{
    class Program
    {
        static Dictionary<string, Dictionary<int, int>> skuSpecialPriceMap = new Dictionary<string, Dictionary<int, int>>
        {
            {"A", new Dictionary<int, int>
                {
                    {3 ,130},
                    {4, 140} // extra
                }
            },
            {"B", new Dictionary<int, int>
                {
                    {2 ,45},
                }
            },
            {"C", new Dictionary<int, int>{}},
            {"D", new Dictionary<int, int>{}}
        };

        static Dictionary<string, int> skuPriceMap = new Dictionary<string, int>
        {
            {"A", 50},
            {"B", 30},
            {"C", 20},
            {"D", 15}
        };

        static void Main(string[] args)
        {
        }
    }
}
