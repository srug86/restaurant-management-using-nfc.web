using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    // 'OrderPrice' representa la información de cada ítem de una factura
    public class OrderPrice
    {
        /* Atributos del objeto */
        // Información del pedido
        private Order order;
        internal Order Order
        {
            get { return order; }
            set { order = value; }
        }

        private double price, discount, total;
        // Precio de cada producto
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        // Descuentos aplicados a los productos
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        // Importe total del pedido
        public double Total
        {
            get { return total; }
            set { total = value; }
        }

        // Método constructor
        public OrderPrice() { }
    }
}