using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    // 'RecProduct' contiene la información de un producto recomendado
    public class RecProduct
    {
        /* Atributos del objeto */
        private string name, category, description;
        // Nombre del producto
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        // Categoría del producto
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        // Descripción del producto
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int times, discountedUnit;
        // Número de veces consumido por el cliente
        public int Times
        {
            get { return times; }
            set { times = value; }
        }
        // Número de productos de este tipo para obtener un descuento
        public int DiscountedUnit
        {
            get { return discountedUnit; }
            set { discountedUnit = value; }
        }

        // Descuento disponible para este producto
        private double discount;
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        /* Métodos constructores */
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

        // Dos 'RecProduct's son iguales si tienen el mismo nombre
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(RecProduct)) return false;
            RecProduct p = (RecProduct)obj;
            return name.Equals(p.Name);
        }
    }
}