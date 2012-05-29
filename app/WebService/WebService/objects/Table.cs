using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    class Table
    {
        private int id, status, capacity, guests;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public int Guests
        {
            get { return guests; }
            set { guests = value; }
        }

        private string client;

        public string Client
        {
            get { return client; }
            set { client = value; }
        }

        public Table() { }

        public Table(int id, int status, int capacity, int guests, string client)
        {
            Id = id;
            Status = status;
            Capacity = capacity;
            Guests = guests;
            Client = client;
        }
    }
}
