using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CheckoutKata;

namespace TestCheckoutKata
{
    [TestClass]
    public class TestCheckout
    {
        static Dictionary<string, Dictionary<int, int>> SkuSpecialPriceMap = new Dictionary<string, Dictionary<int, int>>
        {
            {"A", new Dictionary<int, int>
                {
                    {3 ,130},
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

        static Dictionary<string, Dictionary<int, int>> SkuSpecialPriceMapExtraOffer = new Dictionary<string, Dictionary<int, int>>
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

        static Dictionary<string, int> SkuPriceMap = new Dictionary<string, int>
        {
            {"A", 50},
            {"B", 30},
            {"C", 20},
            {"D", 15}
        };

        // TestScenarios defines a map of standard scenarios for single sku scanning, with the key being the name of the
        // test itself and the value the expected result
        static Dictionary<string, int> TestScenarios = new Dictionary<string, int>
        {
            { "A",    50 },
            { "B",    30 },
            { "C",    20 },
            { "D",    15 },
            { "BB",   45 },
            { "BAB",  95 },
            { "DDAA", 130 },
            { "AAA",  130 },
            // the differences
            { "AAAA",       180 },
            { "AAAAA",      230 },
            { "AAAAAA",     260 },
            { "AAAAAAA",    310 },
            { "AAAAAAAA",   360 },
            { "AAAAAAAAB",  390 },
            { "AAAAAAAABB", 405 },
        };

        // TestScenariosExtraOffer defines a map of standard scenarios along with some scenarios affected by the extra special offer
        // - for single sku scanning, with the key being the name of the test itself and the value the expected result
        static Dictionary<string, int> TestScenariosExtraOffer = new Dictionary<string, int>
        {
            { "A",    50 },
            { "B",    30 },
            { "C",    20 },
            { "D",    15 },
            { "BB",   45 },
            { "BAB",  95 },
            { "DDAA", 130 },
            { "AAA",  130 },
            // the differences
            { "AAAA",       140 },
            { "AAAAA",      190 },
            { "AAAAAA",     240 },
            { "AAAAAAA",    270 },
            { "AAAAAAAAB",  310 },
            { "AAAAAAAABB", 325 },
        };

        static Dictionary<string, int> TestScenariosMultiple = new Dictionary<string, int>
        {
            { "C",     20 },
            { "CC",    40 },
        };

        public SkuPriceList GetSkuPriceList(Dictionary<string, int> skuPriceMap)
        {
            SkuPriceList skuPriceList = new SkuPriceList();

            foreach (var s in skuPriceMap)
            {
                skuPriceList.AddItem(s.Key, s.Value);
            }
            return skuPriceList;
        }

        public SkuSpecialPriceList GetSkuSpecialPriceList(Dictionary<string, Dictionary<int, int>> skuSpecialPriceMap)
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

        private struct ScanScenario
        {
            public string Name;
            public int Count;
            public bool ExpectErr;
        }

        private ScanScenario NewScanScenario(string name, int count, bool expectErr)
        {
            ScanScenario s = new ScanScenario();

            s.Name = name;
            s.Count = count;
            s.ExpectErr = expectErr;

            return s;
        }

        public void RunScenarios(Dictionary<string, int> skuPriceMap,Dictionary<string, Dictionary<int, int>> skuSpecialPriceMap, Dictionary<string, int> testScenarios)
        {
            Checkout checkout = new Checkout(this.GetSkuPriceList(skuPriceMap), this.GetSkuSpecialPriceList(skuSpecialPriceMap));

            foreach (var s in testScenarios)
            {
                foreach (var item in s.Key)
                {
                    checkout.Scan(item.ToString(), 1);
                }

                Assert.AreEqual(s.Value, checkout.GetTotalPrice());
            }
        }

        [TestMethod]
        public void TestScanValidations()
        {
            Checkout checkout = new Checkout(this.GetSkuPriceList(SkuPriceMap), this.GetSkuSpecialPriceList(SkuSpecialPriceMap));

            Dictionary<string, ScanScenario> scenarios = new Dictionary<string, ScanScenario>{
                {"empty sku name and any count",           this.NewScanScenario("", 1, true)},
                {"sku name only spaces and any count",     this.NewScanScenario(" ", 1, true)},
                {"valid sku name and negative count",      this.NewScanScenario("A", -1, true)},
                {"valid sku name and count equal to zero", this.NewScanScenario("A", 0, true)},
                {"valid sku name and valid count",         this.NewScanScenario("A", 1, false)},
                {"sku name with spaces and valid count",   this.NewScanScenario(" A ", 1, false)},
            };

            foreach (var s in scenarios)
            {
                try
                {
                    checkout.Scan(s.Value.Name, s.Value.Count);
                }
                catch (Exception ex)
                {
                    if (!s.Value.ExpectErr)
                    {
                        Assert.Fail(String.Format("Expected no exception, but instead got: '{0}', for scenario: '{1}'", ex.Message, s.Key));
                    }
                }
            }
        }

        public void TestGetTotalPriceWhenNoSkuIsScannedYet()
        {
            Checkout checkout = new Checkout(this.GetSkuPriceList(SkuPriceMap), this.GetSkuSpecialPriceList(SkuSpecialPriceMap));
            Assert.AreEqual(0, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void TestScanSkusAndGetTotalPriceSingleSku()
        {
            this.RunScenarios(SkuPriceMap, SkuSpecialPriceMap, TestScenarios);
            this.RunScenarios(SkuPriceMap, SkuSpecialPriceMapExtraOffer, TestScenariosExtraOffer);
        }

        [TestMethod]
        public void TestScanSkusAndGetTotalPriceMultipleSkus()
        {
            Checkout checkout = new Checkout(this.GetSkuPriceList(SkuPriceMap), this.GetSkuSpecialPriceList(SkuSpecialPriceMap));

            foreach (var s in TestScenariosMultiple)
            {
                checkout.Scan(s.Key[0].ToString(), s.Key.Length);
                Assert.AreEqual(s.Value, checkout.GetTotalPrice());
            }
        }
    }
}
