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

        static Dictionary<string, int> testScenariosMapMultiple = new Dictionary<string, int>
        {
            { "C",     20 },
            { "CC",    40 },
        };

        public SkuPriceList GetSkuPriceList()
        {
            SkuPriceList skuPriceList = new SkuPriceList();
            foreach (var s in skuPriceMap)
            {
                skuPriceList.AddItem(s.Key, s.Value);
            }
            return skuPriceList;
        }

        public SkuSpecialPriceList GetSkuSpecialPriceList()
        {
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

            return skuSpecialPriceList;
        }

        [TestMethod]
        public void TestScanSkusAndGetTotalPriceSingle()
        {
            Checkout checkout = new Checkout(this.GetSkuPriceList(), this.GetSkuSpecialPriceList());
            foreach (var s in testScenariosMap)
            {
                foreach (var item in s.Key)
                {
                    checkout.Scan(item.ToString(),1);
                }

                Assert.AreEqual(s.Value, checkout.GetTotalPrice());
            }
        }

        [TestMethod]
        public void TestScanSkusAndGetTotalPriceMultiple()
        {
            Checkout checkout = new Checkout(this.GetSkuPriceList(), this.GetSkuSpecialPriceList());
            foreach (var s in testScenariosMapMultiple)
            {
                checkout.Scan(s.Key[0].ToString(), s.Key.Length);
                Assert.AreEqual(s.Value, checkout.GetTotalPrice());
            }
        }
    }
}
