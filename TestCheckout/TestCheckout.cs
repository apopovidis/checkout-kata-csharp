using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CheckoutKata;

namespace TestCheckoutKata
{
    [TestClass]
    public class TestCheckout
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
        };

        [TestMethod]
        public void TestScanSkusAndGetTotalPrice()
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

            foreach (var s in testScenariosMap)
            {
                foreach (var item in s.Key)
                {
                    checkout.Scan(item.ToString());
                }

                Assert.AreEqual(s.Value, checkout.GetTotalPrice());
            }
        }
    }
}
