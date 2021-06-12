using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutKata
{
    interface ICheckout
    {
        void Scan(string name,int count);
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

        public void Scan(string name,int count)
        {
            char[] charsToTrim = { ' ' };
            if (name.Trim(charsToTrim) == "")
            {
                throw new ArgumentException(String.Format("{0} cannot be empty", name), "name");
            }

            if (count <= 0)
            {
                throw new ArgumentException(String.Format("{0} must be greater than zero", count), "count");
            }

            if (!this.scannedSkusMap.ContainsKey(name))
            {
                throw new KeyNotFoundException(String.Format("sku with name {0} does not exist", name));
            }

            // initialized in constructor
            this.scannedSkusMap[name] += count;
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
            int total = 0,
                count = this.scannedSkusMap[name];

            int skuPriceForName = this.skuPriceList.GetSku(name).GetPrice();

            // if there is no offer for the name just do a total and return it
            var skuPriceListForName = this.skuSpecialPriceList.GetSkus(name);
            if (skuPriceListForName.Count == 0)
            {
                return count * skuPriceForName;
            }

            int index = 0;
            int[] skuOffersCounts = new int[skuPriceListForName.Count];
            foreach (var c in skuPriceListForName)
            {
                skuOffersCounts[index] = c.GetNumberOfUnits();
                index++;
            }

            // sort the sku offer counts so that we start from the highest offer
            Array.Sort(skuOffersCounts);

            var remainingCount = count;
            for (int i = skuOffersCounts.Length - 1; i >= 0; i--)
            {
                if (remainingCount < skuOffersCounts[i])
                {
                    continue;
                }

                var skuPriceListForNameAndNumberOfUnits = this.skuSpecialPriceList.GetSkus(name, skuOffersCounts[i]);
                int nextOfferPrice = skuPriceListForNameAndNumberOfUnits.FirstOrDefault().GetTotalPrice(),
                    reminder = remainingCount % skuOffersCounts[i];

                // the largest offer is satisfied so return
                if (reminder == 0)
                {
                    total = (remainingCount / skuOffersCounts[i]) * nextOfferPrice;
                    remainingCount = 0;
                    break;
                }

                // any other next offer
                total += ((remainingCount - reminder) / skuOffersCounts[i]) * nextOfferPrice;
                remainingCount = reminder;
            }

            // if we have remaining count still then add it to totals
            if (remainingCount > 0)
            {
                total += remainingCount * skuPriceForName;
            }

            return total;
        }
    }
}
