using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.Interface;
using AspNetCore.Reporting;
using AspNetCore.Reporting.ReportExecutionService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AenEnterprise.FrontEndMvc.Controllers
{
    public class SalesOrderReportController : Controller
    {
        string connectionString;
        private readonly IWebHostEnvironment _env;
        private readonly ISalesOrderService _salesOrderService;

        public SalesOrderReportController(IWebHostEnvironment env, ISalesOrderService salesOrderService)
        {
            connectionString= "Data Source=.\\SQLEXPRESS;Initial Catalog=AenDbEnterprise;Integrated Security=True;";
            _env = env;
            _salesOrderService = salesOrderService;
        }

        public IActionResult GetSalesOrder()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public FileContentResult GetSalesOrderById(int salesOrderId)
        {
           
            string query = "SELECT * FROM spGetOrderItemsBySalesOrderId WHERE @SalesOrderId=" + salesOrderId;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetOrderItemsBySalesOrderId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; ;
                    cmd.Parameters.Add("@SalesOrderId", SqlDbType.Int).Value = salesOrderId;
                    using (SqlDataAdapter sqlAdapt = new SqlDataAdapter(cmd))
                    {
                       
                        string mimeType = "application/pdf";
                        string reportPath = _env.WebRootPath + "\\Reports\\SalesOrderDetails.rdlc";
                        var localReport = new LocalReport(reportPath);

                        try
                        {
                            // Fetch data from the database and fill a DataTable
                            DataTable dataTable = new DataTable();
                            sqlAdapt.Fill(dataTable);

                            localReport.AddDataSource("DataSetSales", dataTable);

                            var result = localReport.Execute(RenderType.Pdf, 1, null, mimeType);
                            return File(result.MainStream, mimeType);

                            //this line if I want open report in current page
                            //return File(result.MainStream, mimeType, "SalesOrderReport.pdf");
                        }
                        catch (Exception ex)
                        {
                            // Log exceptions to a file or preferred logging mechanism
                            Console.WriteLine("Error: " + ex.Message);
                            throw; // Propagate the exception if needed
                        }
                    }
                }

            }
        }

        public FileContentResult GetInvoiceBySalesOrderId(int invoiceId)
        {
            string query = "SELECT * FROM spGetInvoiceBySalesOrderId WHERE @InvoiceId=" + invoiceId;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetInvoiceBySalesOrderId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; ;
                    cmd.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = invoiceId;
                    using (SqlDataAdapter sqlAdapt = new SqlDataAdapter(cmd))
                    {

                        string mimeType = "application/pdf";
                        string reportPath = _env.WebRootPath + "\\Reports\\InvoiceDetails.rdlc";
                        var localReport = new LocalReport(reportPath);

                        try
                        {
                            // Fetch data from the database and fill a DataTable
                            DataTable dataTable = new DataTable();
                            sqlAdapt.Fill(dataTable);

                            localReport.AddDataSource("DataSetSales", dataTable);

                            var result = localReport.Execute(RenderType.Pdf, 1, null, mimeType);
                            return File(result.MainStream, mimeType);

                            //this line if I want open report in current page
                            //return File(result.MainStream, mimeType, "SalesOrderReport.pdf");
                        }
                        catch (Exception ex)
                        {
                            // Log exceptions to a file or preferred logging mechanism
                            Console.WriteLine("Error: " + ex.Message);
                            throw; // Propagate the exception if needed
                        }
                    }
                }

            }
        }

        public FileContentResult GetDeliveryOrderBySalesOrderId(int salesOrderId)
        {
            string query = "SELECT * FROM spGetDeliveryOrderBySalesOrderId WHERE @SalesOrderId=" + salesOrderId;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetDeliveryOrderBySalesOrderId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; ;
                    cmd.Parameters.Add("@SalesOrderId", SqlDbType.Int).Value = salesOrderId;
                    using (SqlDataAdapter sqlAdapt = new SqlDataAdapter(cmd))
                    {

                        string mimeType = "application/pdf";
                        string reportPath = _env.WebRootPath + "\\Reports\\DeliveryOrderDetails.rdlc";
                        var localReport = new LocalReport(reportPath);

                        try
                        {
                            // Fetch data from the database and fill a DataTable
                            DataTable dataTable = new DataTable();
                            sqlAdapt.Fill(dataTable);

                            localReport.AddDataSource("DataSetSales", dataTable);

                            var result = localReport.Execute(RenderType.Pdf, 1, null, mimeType);
                            return File(result.MainStream, mimeType);

                            //this line if I want open report in current page
                            //return File(result.MainStream, mimeType, "SalesOrderReport.pdf");
                        }
                        catch (Exception ex)
                        {
                            // Log exceptions to a file or preferred logging mechanism
                            Console.WriteLine("Error: " + ex.Message);
                            throw; // Propagate the exception if needed
                        }
                    }
                }

            }
        }
        public FileContentResult SalesOrderDetails()
        {
            string query = "SELECT * FROM View_GetAllSalesOrderWithOrderItems";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlDataAdapter sqlAdapt = new SqlDataAdapter(query, conn))
                {
                    string mimeType = "application/pdf";
                    string reportPath = _env.WebRootPath + "\\Reports\\SalesOrderDetails.rdlc";
                    var localReport = new LocalReport(reportPath);

                    try
                    {
                        // Fetch data from the database and fill a DataTable
                        DataTable dataTable = new DataTable();
                        sqlAdapt.Fill(dataTable);


                     
                        localReport.AddDataSource("DataSetInvoice", dataTable);

                        var result = localReport.Execute(RenderType.Pdf, 1, null, mimeType);
                        return File(result.MainStream, mimeType);
                    }
                    catch (Exception ex)
                    {
                        // Log exceptions to a file or preferred logging mechanism
                        Console.WriteLine("Error: " + ex.Message);
                        throw; // Propagate the exception if needed
                    }
                }
            }
        }
    }
}

