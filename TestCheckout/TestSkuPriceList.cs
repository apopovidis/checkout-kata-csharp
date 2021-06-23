using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CheckoutKata;

namespace TestCheckoutKata
{
    [TestClass]
    public class TestSkuPriceList
    {
        private struct SkuPriceListScenario
        {
            public string Name;
            public int Price;
            public bool ExpectErr;
        }
    
        private SkuPriceListScenario NewSkuPriceListScenario(string name,int price,bool expectErr)
        {
            SkuPriceListScenario s = new SkuPriceListScenario();

            s.Name = name;
            s.Price = price;
            s.ExpectErr = expectErr;

            return s;
        }

        [TestMethod]
        public void TestAddItemValidations()
        {
            SkuPriceList list = new SkuPriceList();

            Dictionary<string, SkuPriceListScenario> testScenariosMap = new Dictionary<string, SkuPriceListScenario>{
                {"empty name and any price",               NewSkuPriceListScenario("", -1, true) },
                {"name white spaces only and any price",   NewSkuPriceListScenario(" ", -1, true)},
                {"valid name and negative price",          NewSkuPriceListScenario("A", -1, true)},
                {"valid name and price equal to zero",     NewSkuPriceListScenario("A", 0, true)},
                {"valid name and valid price",             NewSkuPriceListScenario("A", 1, false)},
                {"name with white spaces and valid price", NewSkuPriceListScenario(" B ", 2, false)},
                {"name already exists and valid price",    NewSkuPriceListScenario("A", 1, true)},
            };

            foreach (var s in testScenariosMap)
            {
                try {
                    list.AddItem(s.Value.Name, s.Value.Price);
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

        [TestMethod]
        public void TestAddExistingItem()
        {
            SkuPriceList list = new SkuPriceList();

            // add sku A for first time
            try
            {
                list.AddItem("A", 1);
            }
            catch (Exception ex)
            {
                Assert.Fail(String.Format("Expected no exception, but instead got: '{0}'", ex.Message));
            }

            // add sku A again
            try
            {
                list.AddItem("A", 1);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(String.Format("sku with name {0} already exists", "A"), ex.Message);
            }
        }

        [TestMethod]
        public void TestGetItems()
        {
            SkuPriceList list = new SkuPriceList();

            try
            {
                list.AddItem("A", 1);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                list.AddItem("B", 2);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            var items = list.GetItems();
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual("A", items[0].GetName());
            Assert.AreEqual(1, items[0].GetPrice());
            Assert.AreEqual("B", items[1].GetName());
            Assert.AreEqual(2, items[1].GetPrice());
        }
    }
}
