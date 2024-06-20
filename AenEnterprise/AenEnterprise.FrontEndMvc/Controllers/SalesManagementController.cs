using Microsoft.AspNetCore.Mvc;

namespace AenEnterprise.FrontEndMvc.Controllers
{
    public class SalesManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateSalesOrder()
        {
            return View();
        }


        public IActionResult UnApproveSalesOrders()
        {
            return View();
        }

        public IActionResult ApprovedSalesOrders()
        {
            return View();
        }

        public IActionResult CreateInvoice()
        {
            return View();
        }

        public IActionResult UnApproveInvoice()
        {
            return View();
        }

        public IActionResult ApprovedInvoice()
        {
            return View();
        }

        public IActionResult CreateDeliveryOrder()
        {
            return View();
        }
        public IActionResult UnApproveDeliveryOrder()
        {
            return View();
        }
        public IActionResult ApproveDeliveryOrder()
        {
            return View();
        }
        public IActionResult CreateDispatch()
        {
            return View();
        }

        public IActionResult DispatchedList()
        {
            return View();
        }
        public IActionResult SalesSummary()
        {
            return View();
        }
    }
}
