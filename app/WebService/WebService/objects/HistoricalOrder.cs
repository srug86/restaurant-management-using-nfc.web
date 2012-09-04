using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    // 'HOrder' define las características de un pedido histórico
    public class HistoricalOrder
    {
        /* Atributos del objeto */
        private String client, product;
        // Cliente solicitante
        public String Client
        {
            get { return client; }
            set { client = value; }
        }
        // Producto solicitado
        public String Product
        {
            get { return product; }
            set { product = value; }
        }

        // Cantidad de productos del mismo tipo
        private int amount;
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        // Fecha de solicitud
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        // Método constructor
        public HistoricalOrder() { }
    }
}