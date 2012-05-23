using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    public class HistoricalOrder
    {
        private String client, product;

        public String Client
        {
            get { return client; }
            set { client = value; }
        }

        public String Product
        {
            get { return product; }
            set { product = value; }
        }

        private int amount;

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public HistoricalOrder() { }
    }
}