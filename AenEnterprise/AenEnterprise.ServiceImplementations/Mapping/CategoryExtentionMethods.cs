using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.UserDomain;
using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class CategoryExtentionMethods
    {
        public static CategoryView ConvertToCategoryView(this Category category)
        { 
                var categoryView = new CategoryView
                {
                    Id = category.Id,
                    Name = category.Name,
                   Products = category.Products.Select(product => new ProductView
                   {
                       // Map properties of the Product to the CustomerView as needed
                       Id = product.Id,
                       Name = product.Name,
                       // Add other properties here
                   }).ToList(),
                };

                return categoryView;
            

        }
        public static IList<CategoryView> ConvertToCategoryViews(this IEnumerable<Category> categories)
        {
            IList<CategoryView> categoryViews = new List<CategoryView>();

            foreach (Category category in categories)
            {
                
                categoryViews.Add(ConvertToCategoryView(category));
            }

            return categoryViews;
        }
    }
}
