namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class CreateSalesOrderFormRequest
    {
        public int Id { get; set; }
        public string SalesOrderNo { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime OrderedDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SalesAmount { get; set; }
        public int ApprovalId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal TotalInvoiceQuantity { get; set; }
        public decimal RemainingSalesQuantity { get; set; }
        public int CustomerId { get; set; }
        public string? DiscountPercent { get; set; }
        public string? DeliveryPlane { get; set; }
    }
}
