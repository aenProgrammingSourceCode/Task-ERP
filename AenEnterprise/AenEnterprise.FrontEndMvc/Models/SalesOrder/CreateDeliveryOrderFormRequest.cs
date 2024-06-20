namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class CreateDeliveryOrderFormRequest
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public decimal DeliveryQuantity { get; set; }
        public decimal DeliveryAmount { get; set; }
        public int InvoiceId { get; set; }
        public int SalesOrderId { get; set; }
        public int SalesOrderStatusId { get; set; }
        public int UnitId { get; set; }
        public int OrderItemId { get; set; }
        public int InvoiceItemId { get; set; }
    }
}
