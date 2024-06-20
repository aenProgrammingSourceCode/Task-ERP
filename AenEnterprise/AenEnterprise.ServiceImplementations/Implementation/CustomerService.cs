using AenEnterprise.DataAccess.Repository;
using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using AenEnterprise.ServiceImplementations.Interface;
using AenEnterprise.ServiceImplementations.Mapping;
using AenEnterprise.ServiceImplementations.Messaging.CustomerManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;

        }
        public async Task<GetAllCustomerResponse> GetAllCustomer()
        {
            GetAllCustomerResponse response = new GetAllCustomerResponse();
            IEnumerable<Customer> customers=await _customerRepository.FindAllAsync();
            response.Customers = customers.ConvertToCustomerViews();
            return response;
        }
    }
}
