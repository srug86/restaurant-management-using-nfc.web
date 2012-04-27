using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    class Product
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

        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public Product() { }

        public Product(string name, string category, string description, double price)
        {
            Name = name;
            Category = category;
            Description = description;
            Price = price;
        }
    }
}
