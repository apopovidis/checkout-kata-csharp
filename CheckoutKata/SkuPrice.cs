using System;
using System.Collections.Generic;

namespace CheckoutKata
{
    public class SkuPrice
    {
        private string name;
        private int price;

        public SkuPrice(string name, int price)
        {
            this.name = name;
            this.price = price;
        }

        public string GetName()
        {
            return this.name;
        }

        public int GetPrice()
        {
            return this.price;
        }
    }
}