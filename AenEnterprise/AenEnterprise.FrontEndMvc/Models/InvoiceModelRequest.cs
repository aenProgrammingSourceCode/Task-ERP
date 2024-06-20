namespace AenEnterprise.FrontEndMvc.Models
{
    public class InvoiceModelRequest
    {
        public decimal InvoiceQuantity { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
    }
}
