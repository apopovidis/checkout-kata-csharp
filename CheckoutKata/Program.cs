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

        static Dictionary<string, int> testScenariosMap = new Dictionary<string, int>
        {
            { "A",     50 },
            { "B",     30 },
            { "C",     20 },
            { "D",     15 },
            { "AAA",   130 },
            { "BB",    45 },
            { "BAB",   95 },
            { "AAAAA", 190 },
            { "DDAA",  130}
        };

        static void Main(string[] args)
        {
            SkuPriceList skuPriceList = new SkuPriceList();
            foreach (var s in skuPriceMap)
            {
                skuPriceList.AddItem(s.Key, s.Value);
            }

            SkuSpecialPriceList skuSpecialPriceList = new SkuSpecialPriceList();
            foreach (var s1 in skuSpecialPriceMap)
            {
                if (skuSpecialPriceMap[s1.Key].Count == 0)
                {
                    continue;
                }

                foreach (var s2 in skuSpecialPriceMap[s1.Key])
                {
                    skuSpecialPriceList.AddItem(s1.Key, s2.Key, s2.Value);
                }
            }

            Checkout checkout = new Checkout(skuPriceList, skuSpecialPriceList);

            bool pass = true;
            foreach (var s in testScenariosMap)
            {
                foreach (var name in s.Key)
                {
                    checkout.Scan(name.ToString(),1);
                }

                var result = checkout.GetTotalPrice();
                if (s.Value != result)
                {
                    Console.WriteLine("Failed at scenario {0}. Expected {1}, but got {2}", s.Key, s.Value, result);
                    pass = false;
                }
            }

            if (pass)
            {
                Console.WriteLine("PASS");
            }

            Console.ReadKey();
        }
    }
}
