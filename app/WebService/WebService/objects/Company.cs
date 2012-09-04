using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    // 'Company' representa los datos de una empresa
    public class Company
    {
        /* Atributos del objeto */
        private string name, nif, email;
        // Nombre
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        // NIF
        public string NIF
        {
            get { return nif; }
            set { nif = value; }
        }
        // Correo electrónico
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private int phone, fax;
        // Teléfono
        public int Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        // Fax
        public int Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        // Método constructor
        public Company() { }
    }
}