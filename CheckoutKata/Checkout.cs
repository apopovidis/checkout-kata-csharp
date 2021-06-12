using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutKata
{
    interface ICheckout
    {
        void Scan(string name);
        int GetTotalPrice();
    }

    public class Checkout
    {
        private SkuPriceList skuPriceList;
        private SkuSpecialPriceList skuSpecialPriceList;
        private Dictionary<string, int> scannedSkusMap;

        public Checkout(SkuPriceList skuPriceList, SkuSpecialPriceList skuSpecialPriceList)
        {
            this.skuPriceList = skuPriceList;
            this.skuSpecialPriceList = skuSpecialPriceList;
            this.initScannedSkusMap();
        }

        public void Scan(string name)
        {
            char[] charsToTrim = { ' ' };
            if (name.Trim(charsToTrim) == "")
            {
                throw new ArgumentException(String.Format("{0} cannot be empty", name), "name");
            }

            if (!this.scannedSkusMap.ContainsKey(name))
            {
                throw new KeyNotFoundException(String.Format("sku with name {0} does not exist", name));
            }

            // initialized in constructor
            this.scannedSkusMap[name] += 1;
        }

        public int GetTotalPrice()
        {
            int res = 0;
            foreach (var item in this.scannedSkusMap)
            {
                res += this.getTotalPriceForItem(item.Key);
            }

            // in case the same object is called for different scenario
            this.initScannedSkusMap();

            return res;
        }

        public void initScannedSkusMap()
        {
            this.scannedSkusMap = new Dictionary<string, int>();
            foreach (var item in this.skuPriceList.GetItems())
            {
                this.scannedSkusMap[item.GetName()] = 0;
            }
        }

        private int getTotalPriceForItem(string name)
        {
            int count = this.scannedSkusMap[name],
                skuPriceForName = this.skuPriceList.GetSku(name).GetPrice();

            return count * skuPriceForName;
        }
    }
}
