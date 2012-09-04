using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    // 'Address' representa la dirección de un domicilio o un establecimiento
    public class Address
    {
        /* Atributos del objeto */
        private string street, number, town, state;
        // Calle
        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        // Número
        public string Number
        {
            get { return number; }
            set { number = value; }
        }
        // Localidad
        public string Town
        {
            get { return town; }
            set { town = value; }
        }
        // Provincia
        public string State
        {
            get { return state; }
            set { state = value; }
        }

        // Código postal
        private int zipCode;
        public int ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        // Método constructor
        public Address() { }
    }
}