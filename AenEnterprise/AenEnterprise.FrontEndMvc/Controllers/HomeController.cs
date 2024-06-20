using AenEnterprise.FrontEndMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AenEnterprise.FrontEndMvc.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

      
        public IActionResult Index()
        {
            return View();
        }


       
        public IActionResult CategoryItems()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Users()
        {
            return View();
        }
        public IActionResult SalesOrderList()
        {
            return View();
        }
        public IActionResult SalesOrderDetails()
        {
            return View();
        }

        public IActionResult ApprovedSalesOrders()
        {
            return View();
        }

        public IActionResult CreateSalesOrder()
        {
            return View();
        }
        public IActionResult CreateInvoice()
        {
            return View();
        }

        public IActionResult LoginUser()
        {
            return View();
        }
        public IActionResult UserRegistration()
        {
            return View();
        }

        public IActionResult UnApprovedSalesOrder()
        {
            return View();
        }
        public IActionResult CreatePurchaseOrder()
        {

            return View();
        }

    }
}