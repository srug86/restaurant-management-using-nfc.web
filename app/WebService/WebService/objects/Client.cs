using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    class Client
    {
        string dni, name, surname;

        public string Dni
        {
            get { return dni; }
            set { dni = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        int status, appearances;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public int Appearances
        {
            get { return appearances; }
            set { appearances = value; }
        }

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
