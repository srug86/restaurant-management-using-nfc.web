using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    // 'Recommendation' contiene la recomendación para un cliente
    public class Recommendation
    {
        /* Atributos del objeto */
        // Frase de bienvenida
        private string opening;
        public string Opening
        {
            get { return opening; }
            set { opening = value; }
        }

        private List<RecProduct> usually, promotional, recommended;
        // Lista de productos más consumidos por el cliente
        internal List<RecProduct> Usually
        {
            get { return usually; }
            set { usually = value; }
        }
        // Lista de productos con alguna promoción
        internal List<RecProduct> Promotional
        {
            get { return promotional; }
            set { promotional = value; }
        }
        // Lista de productos recomendados según los gustos del cliente
        internal List<RecProduct> Recommended
        {
            get { return recommended; }
            set { recommended = value; }
        }

        // Método constructor
        public Recommendation()
        {
            Usually = new List<RecProduct>();
            Promotional = new List<RecProduct>();
            Recommended = new List<RecProduct>();
        }
    }
}