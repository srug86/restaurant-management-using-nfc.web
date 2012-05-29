using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    class Order
    {
        private int id, tableID, amount, status;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int TableID
        {
            get { return tableID; }
            set { tableID = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private string product;

        public string Product
        {
            get { return product; }
            set { product = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public Order() { }

        public Order(int tableID, int amount, int status, string product, DateTime date)
        {
            TableID = tableID;
            Amount = amount;
            Status = status;
            Product = product;
            Date = date;
        }
    }
}