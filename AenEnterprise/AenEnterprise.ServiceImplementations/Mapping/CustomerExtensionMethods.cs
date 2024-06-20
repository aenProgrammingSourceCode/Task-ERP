using AenEnterprise.DomainModel;
using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class CustomerExtensionMethods
    {
        public static CustomerView ConvertToCustomerView(this Customer customer)
        {
            var customerView = new CustomerView
            {
                Id = customer.Id,
                Name = customer.Name,
                Description = customer.Description,
                Address = customer.Address,
                MobileNo = customer.MobileNo,
            };


            return customerView;


        }
        public static IList<CustomerView> ConvertToCustomerViews(this IEnumerable<Customer> customers)
        {
            IList<CustomerView> customerViews = new List<CustomerView>();

            foreach (Customer customer in customers)
            {
                customerViews.Add(ConvertToCustomerView(customer));
            }

            return customerViews;
        }
    }
}
