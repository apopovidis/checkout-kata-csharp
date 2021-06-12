using System;
using System.Collections.Generic;

namespace CheckoutKata
{
    public class SkuSpecialPrice
    {
        private string name;
        private int numberOfUnits;
        private int totalPrice;

        public SkuSpecialPrice(string name, int numberOfUnits, int totalPrice)
        {
            this.name = name;
            this.numberOfUnits = numberOfUnits;
            this.totalPrice = totalPrice;
        }

        public string GetName()
        {
            return this.name;
        }

        public int GetNumberOfUnits()
        {
            return this.numberOfUnits;
        }

        public int GetTotalPrice()
        {
            return this.totalPrice;
        }
    }
}