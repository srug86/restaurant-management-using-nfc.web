using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    // 'Client' contiene la información de un cliente
    class Client
    {
        /* Atributos del objeto */
        string dni, name, surname;
        // DNI
        public string Dni
        {
            get { return dni; }
            set { dni = value; }
        }
        // Nombre
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        // Apellido(s)
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        int status, appearances;
        // Estado del cliente: (-1) Nuevo cliente, (0) No está, (1) Está, (2) Ha pagado
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        // Número de apariciones
        public int Appearances
        {
            get { return appearances; }
            set { appearances = value; }
        }

        /* Métodos constructores */
        public Client() { }

        public Client(string dni, string name, string surname, int status, int appearances)
        {
            Dni = dni;
            Name = name;
            Surname = surname;
            Status = status;
            Appearances = appearances;
        }
    }
}
