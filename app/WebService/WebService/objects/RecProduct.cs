using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    public class RecProduct
    {
        private string name, category, description;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int times, discountedUnit;

        public int Times
        {
            get { return times; }
            set { times = value; }
        }

        public int DiscountedUnit
        {
            get { return discountedUnit; }
            set { discountedUnit = value; }
        }

        private double discount;

        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public RecProduct(string name)
        {
            Name = name;
            Category = "";
            Discount = 0.0;
            Times = DiscountedUnit = 0;
        }

        public RecProduct(string name, string category) {
            Name = name;
            Category = category;
            Discount = 0.0;
            Times = DiscountedUnit = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(RecProduct)) return false;
            RecProduct p = (RecProduct)obj;
            return name.Equals(p.Name);
        }
    }
}