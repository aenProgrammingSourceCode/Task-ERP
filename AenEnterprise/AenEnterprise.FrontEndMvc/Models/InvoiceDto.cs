namespace AenEnterprise.FrontEndMvc.Models
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int SalesOrderId { get; set; }
        public int UnitId { get; set; }
        public decimal SalesPrice { get; set; }
        public int ApprovalId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
