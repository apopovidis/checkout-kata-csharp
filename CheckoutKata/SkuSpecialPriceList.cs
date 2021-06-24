using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutKata
{
    public class SkuSpecialPriceList
    {
        private List<SkuSpecialPrice> items = new List<SkuSpecialPrice>();

        public void AddItem(string name, int numberOfUnits, int totalPrice)
        {
            char[] charsToTrim = { ' ' };
            string n = name.Trim(charsToTrim);
            if (n == "")
            {
                throw new ArgumentException(String.Format("{0} cannot be empty", n), "name");
            }

            if (numberOfUnits <= 0)
            {
                throw new ArgumentException(String.Format("{0} must be greater than zero", numberOfUnits), "numberOfUnits");
            }

            if (totalPrice <= 0)
            {
                throw new ArgumentException(String.Format("{0} must be greater than zero", totalPrice), "totalPrice");
            }

            var skus = this.GetSkus(n, numberOfUnits, totalPrice);
            if (skus.Count == 0)
            {
                this.items.Add(new SkuSpecialPrice(n, numberOfUnits, totalPrice));
            }
            else {
                throw new Exception(String.Format("sku with name {0}, numberOfUnits {1} and total price {2} already exists", name, numberOfUnits, totalPrice));
            }
        }

        public List<SkuSpecialPrice> GetSkus(string name, int numberOfUnits, int totalPrice)
        {
            return this.items.Where(i => i.GetName() == name && i.GetNumberOfUnits() == numberOfUnits && i.GetTotalPrice() == totalPrice).ToList();
        }

        public List<SkuSpecialPrice> GetSkus(string name, int numberOfUnits)
        {
            return this.items.Where(i => i.GetName() == name && i.GetNumberOfUnits() == numberOfUnits).ToList();
        }

        public List<SkuSpecialPrice> GetSkus(string name)
        {
            return this.items.Where(i => i.GetName() == name).ToList();
        }

        public int GetSkuSpecialPrice(string name, int numberOfUnits)
        {
            var skus = this.GetSkus(name, numberOfUnits);
            if (skus.Count == 0)
            {
                throw new KeyNotFoundException(String.Format("sku with name {0} and numberOfUnits {1} does not exist", name, numberOfUnits));
            }
            return skus.FirstOrDefault().GetTotalPrice();
        }
    }
}