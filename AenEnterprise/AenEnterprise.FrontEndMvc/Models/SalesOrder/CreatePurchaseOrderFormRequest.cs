namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class CreatePurchaseOrderFormRequest
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseOrderNo { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalCost { get; set; }
    }
}
