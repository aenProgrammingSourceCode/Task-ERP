using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.ServiceImplementations.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AenEnterprise.FrontEndMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ICustomerService _customerService;
        private readonly IMemoryCache _memoryCache;

        public CustomerController(IUnitOfWork unitOfWork,ICustomerService customerService, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _memoryCache= memoryCache;
        }

        [HttpGet("AllCustomer")]
        public async Task<ActionResult> GetCustomerAll()
        {
            var cacheKey = "CustomersData";
            var cachedData = _memoryCache.Get(cacheKey);

            var customerFromService = await _customerService.GetAllCustomer();
            return Ok(customerFromService);
        }

    }
}
