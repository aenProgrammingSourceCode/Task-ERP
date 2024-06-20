using AenEnterprise.DomainModel;
using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class ProductExtensionMethods
    {
        public static ProductView ConvertToProductView(this Product product)
        {
            var productView = new ProductView
            {
                Id = product.Id,
                Name = product.Name
                // Other properties specific to ProductView
            };

            return productView;
        }

        public static IList<ProductView> ConvertToProductViews(this IEnumerable<Product> products)
        {
            IList<ProductView> productViews = new List<ProductView>();

            foreach (Product product in products)
            {
                productViews.Add(ConvertToProductView(product));
            }

            return productViews;
        }
    }

}
