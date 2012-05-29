using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    public class Recommendation
    {
        private string opening;

        public string Opening
        {
            get { return opening; }
            set { opening = value; }
        }

        private List<RecProduct> usually, promotional, recommended;

        internal List<RecProduct> Usually
        {
            get { return usually; }
            set { usually = value; }
        }

        internal List<RecProduct> Promotional
        {
            get { return promotional; }
            set { promotional = value; }
        }

        internal List<RecProduct> Recommended
        {
            get { return recommended; }
            set { recommended = value; }
        }

        public Recommendation()
        {
            Usually = new List<RecProduct>();
            Promotional = new List<RecProduct>();
            Recommended = new List<RecProduct>();
        }
    }
}