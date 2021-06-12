using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutKata
{
    public class SkuPriceList
    {
        private List<SkuPrice> items = new List<SkuPrice>();

        public List<SkuPrice> GetItems()
        {
            return this.items;
        }

        public void AddItem(string name, int price)
        {
            char[] charsToTrim = { ' ' };
            if (name.Trim(charsToTrim) == "")
            {
                throw new ArgumentException(String.Format("{0} cannot be empty", name), "name");
            }

            if (price <= 0)
            {
                throw new ArgumentException(String.Format("{0} must be greater than zero", price), "price");
            }

            var sku = this.GetSku(name);
            if (sku == null)
            {
                this.items.Add(new SkuPrice(name, price));
            }
            else {
                throw new Exception(String.Format("sku with name {0} already exists", name));
            }
        }

        public SkuPrice GetSku(string name)
        {
            return this.items.Where(i => i.GetName() == name).FirstOrDefault();
        }

        public int GetSkuPrice(string name)
        {
            var sku = this.GetSku(name);
            if (sku == null)
            {
                throw new KeyNotFoundException(String.Format("sku with name {0} does not exist", name));
            }
            return sku.GetPrice();
        }
    }
}