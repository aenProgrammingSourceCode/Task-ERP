namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class CreateDispatchFormRequest
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public string VehicalNo { get; set; } = string.Empty;
        public decimal VehicalEmptyWeight { get; set; }
        public decimal VehicalLoadWeight { get; set; }
        public decimal DispatchQuantity { get; set; }
        public int UnitId { get; set; }
        public int SalesOrderId { get; set; }
        public int InvoiceId { get; set; }
        public int OrderItemId { get; set; }
        public int DeliveryOrderId { get; set; }
        public int ProductId { get; set; }
        public int DeliveryItemId { get; set; }
    }
}
