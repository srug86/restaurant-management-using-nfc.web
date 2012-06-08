using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServices.objects;

namespace WebServices
{
    public class Recommender
    {
        public static Recommendation generateRecommendation(List<Product> products, List<HistoricalOrder> clientHistory, 
            string restaurant, int appearances, double discount, int discountedVisit)
        {
            Recommendation rec = new Recommendation();
            rec.Opening = generateOpening(restaurant, appearances, discount, discountedVisit);
            List<RecProduct> usual = generateListOfUsualProducts(products, clientHistory);
            rec.Usually = getMostConsumedProducts(usual);
            rec.Promotional = generateListOfPromotionalProducts(products);
            rec.Recommended = generateListOfRecommendedProducts(products, rec.Usually);
            return rec;
        }

        private static string generateOpening(string restaurant, int app, double disc, int discV)
        {
            string xml = "Bienvenido al restaurante \"" + restaurant + "\"\n";
            if (app == 0) xml += "Es su primera visita.\nSepa que utilizando el sistema NFC podra disfutar de descuentos y recomendaciones personalizadas.\n";
            else xml += "Gracias por utilizar el servicio NFC.\nEs su visita numero " + (app + 1) + ".\n";
            if (discV != 0 && disc != 0.0)
            {
                if (app % discV == 0) xml += "Dispone de un descuento final en su factura del " + disc + "%.";
                else xml += "La factura de la visita " + ((int)(app / discV) + 1) * discV + " dispondra de un descuento en su factura del " + disc + "%.";
            }
            return xml;
        }

        private static List<RecProduct> getMostConsumedProducts(List<RecProduct> usual)
        {
            List<RecProduct> mostUsual = new List<RecProduct>();
            List<string> categories = getProductCategories(usual);
            foreach (string category in categories)
            {
                int max = 0, p = -1;
                for (int i = 0; i < usual.Count; i++)
                    if (usual[i].Category.Equals(category) && usual[i].Times > max)
                    {
                        max = usual[i].Times;
                        p = i;
                    }
                if (p != -1)
                    mostUsual.Add(usual[p]);
            }
            return mostUsual;
        }

        private static List<RecProduct> generateListOfUsualProducts(List<Product> products, List<HistoricalOrder> clientHistory)
        {
            List<RecProduct> usual = new List<RecProduct>();
            foreach (HistoricalOrder order in clientHistory)
            {
                int i = usual.IndexOf(new RecProduct(order.Product));
                if (i == -1)
                {
                    RecProduct rp = new RecProduct(order.Product);
                    rp.Times = order.Amount;
                    usual.Add(rp);
                }
                else
                    usual[i].Times += order.Amount;
            }
            for (int i = 0; i < usual.Count; i++)
            {
                int j = products.IndexOf(new Product(usual[i].Name));
                if (j == -1) { usual.RemoveAt(i); i--; }
                else
                {
                    if (!products[j].Visible) { usual.RemoveAt(i); i--; }
                    else
                    {
                        usual[i].Category = products[j].Category;
                        usual[i].Discount = products[j].Discount;
                        usual[i].DiscountedUnit = products[j].DiscountedUnit;
                        usual[i].Description = products[j].Description;
                    }
                }
            }
            return usual;
        }

        private static List<string> getProductCategories(List<RecProduct> products)
        {
            List<string> categories = new List<string>();
            foreach (RecProduct p in products)
                if (categories.IndexOf(p.Category) == -1)
                    categories.Add(p.Category);
            return categories;
        }

        private static List<RecProduct> generateListOfPromotionalProducts(List<Product> products)
        {
            List<RecProduct> promotional = new List<RecProduct>();
            foreach (Product product in products)
                if (product.Discount > 0)
                {
                    RecProduct recP = new RecProduct(product.Name, product.Category);
                    recP.Discount = product.Discount;
                    recP.DiscountedUnit = product.DiscountedUnit;
                    promotional.Add(recP);
                }
            return promotional;
        }

        private static List<RecProduct> generateListOfRecommendedProducts(List<Product> products, List<RecProduct> mostUsual)
        {
            List<RecProduct> recommended = new List<RecProduct>();
            foreach (RecProduct mUP in mostUsual)
            {
                int similarity = 0, p = -1;
                if (mUP.Description != null)
                {
                    string[] keys = mUP.Description.Split(',');
                    for (int i = 0; i < products.Count; i++)
                    {
                        int auxSimil = 0;
                        if (products[i].Category.Equals(mUP.Category) &&
                            !products[i].Name.Equals(mUP.Name))
                        {
                            foreach (string key in products[i].Description.Split(','))
                                if (keys.Contains(key))
                                    auxSimil++;
                            if (auxSimil > similarity)
                            {
                                similarity = auxSimil;
                                p = i;
                            }
                        }
                    }
                    if (p != -1)
                        recommended.Add(new RecProduct(products[p].Name, products[p].Category));
                }
            }
            return recommended;
        }
    }
}