using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    // 'Table' contiene la información de una mesa
    class Table
    {
        /* Atributos del objeto */
        private int id, status, capacity, guests;
        // Identificador
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        // Estado: (-1) Vacía, (0) Ocupada, (1) Esperando pedido, (2) Servida, (3) Cobrada
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        // Capacidad total
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
        // Número de comensales que la ocupan
        public int Guests
        {
            get { return guests; }
            set { guests = value; }
        }

        // DNI del cliente que la ocupa
        private string client;
        public string Client
        {
            get { return client; }
            set { client = value; }
        }

        /* Métodos constructores */
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
