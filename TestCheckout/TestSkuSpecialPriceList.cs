using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CheckoutKata;

namespace TestCheckoutKata
{
    [TestClass]
    public class TestSkuSpecialPriceList
    {
        private struct SkuSpecialPriceListScenario
        {
            public string Name;
            public int NumberOfUnits;
            public int TotalPrice;
            public bool ExpectErr;
        }

        private SkuSpecialPriceListScenario NewSkuSpecialPriceListScenario(string name, int numberOfUnits, int totalPrice, bool expectErr)
        {
            SkuSpecialPriceListScenario s = new SkuSpecialPriceListScenario();

            s.Name = name;
            s.NumberOfUnits = numberOfUnits;
            s.TotalPrice = totalPrice;
            s.ExpectErr = expectErr;

            return s;
        }

        [TestMethod]
        public void TestAddItemValidations()
        {
            SkuSpecialPriceList list = new SkuSpecialPriceList();

            Dictionary<string, SkuSpecialPriceListScenario> testScenariosMap = new Dictionary<string, SkuSpecialPriceListScenario>{
                {"empty name, any number of units and any total price",                 NewSkuSpecialPriceListScenario("", -1, -1, true)},
                {"name white spaces only, any number of units and any total price",     NewSkuSpecialPriceListScenario(" ", -1, -1, true)},
                {"valid name, negative number of units and any total price",            NewSkuSpecialPriceListScenario("A", -1, -1, true)},
                {"valid name, number of units equal to zero and any total price",       NewSkuSpecialPriceListScenario("A", 0, 0, true)},
                {"valid name, valid number of units and negative total price",          NewSkuSpecialPriceListScenario("A", 1, -1, true)},
                {"valid name, valid number of units and total price equal to zero",     NewSkuSpecialPriceListScenario("A", 1, 0, true)},
                {"valid name, valid number of units and valid total price",             NewSkuSpecialPriceListScenario("A", 1, 1, false)},
                {"name with white spaces, valid number of units and valid total price", NewSkuSpecialPriceListScenario(" B ", 2, 2, false)}
            };

            foreach (var s in testScenariosMap)
            {
                try
                {
                    list.AddItem(s.Value.Name, s.Value.NumberOfUnits, s.Value.TotalPrice);
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
            SkuSpecialPriceList list = new SkuSpecialPriceList();

            // add sku A for first time
            try
            {
                list.AddItem("A", 1, 1);
            }
            catch (Exception ex)
            {
                Assert.Fail(String.Format("Expected no exception, but instead got: '{0}'", ex.Message));
            }

            // add sku A again with a different number of units and different total price
            try
            {
                list.AddItem("A", 2, 2);
            }
            catch (Exception ex)
            {
                Assert.Fail(String.Format("Expected no exception, but instead got: '{0}'", ex.Message));
            }

            // add another sku A again with the same number of units and same total price
            try
            {
                list.AddItem("A", 2, 2);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(String.Format("sku with name {0}, numberOfUnits {1} and total price {2} already exists", "A", 2, 2), ex.Message);
            }
        }

        [TestMethod]
        public void TestGetSkus()
        {
            SkuSpecialPriceList list = new SkuSpecialPriceList();

            try
            {
                list.AddItem("A", 1, 1);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                list.AddItem("A", 2, 2);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                list.AddItem("A", 3, 3);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            // getting all sku A offers
            var items = list.GetSkus("A");
            Assert.AreEqual(3, items.Count);
            Assert.AreEqual("A", items[0].GetName());
            Assert.AreEqual(1, items[0].GetNumberOfUnits());
            Assert.AreEqual(1, items[0].GetTotalPrice());

            Assert.AreEqual("A", items[1].GetName());
            Assert.AreEqual(2, items[1].GetNumberOfUnits());
            Assert.AreEqual(2, items[1].GetTotalPrice());

            Assert.AreEqual("A", items[2].GetName());
            Assert.AreEqual(3, items[2].GetNumberOfUnits());
            Assert.AreEqual(3, items[2].GetTotalPrice());

            // getting all sku A offers with number of units
            items = list.GetSkus("A", 1);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("A", items[0].GetName());
            Assert.AreEqual(1, items[0].GetNumberOfUnits());
            Assert.AreEqual(1, items[0].GetTotalPrice());

            // getting all sku A offers with number of units and total price
            items = list.GetSkus("A", 1, 1);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("A", items[0].GetName());
            Assert.AreEqual(1, items[0].GetNumberOfUnits());
            Assert.AreEqual(1, items[0].GetTotalPrice());
        }

        [TestMethod]
        public void TestGetSkuSpecialPrice()
        {
            SkuSpecialPriceList list = new SkuSpecialPriceList();

            try
            {
                var sku = list.GetSkuSpecialPrice("A", 1);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(String.Format("sku with name {0} and numberOfUnits {1} does not exist", "A", 1), ex.Message);
            }

            try
            {
                list.AddItem("A", 1, 1);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                var totalPrice = list.GetSkuSpecialPrice("A", 1);
                Assert.AreEqual(1, totalPrice);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
