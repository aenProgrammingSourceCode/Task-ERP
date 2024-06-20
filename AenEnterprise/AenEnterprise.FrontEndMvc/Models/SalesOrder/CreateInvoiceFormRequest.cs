namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class CreateInvoiceFormRequest
    {
        public int Id { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int UnitId { get; set; }
        public int OrderItemId { get; set; }
    }
}
