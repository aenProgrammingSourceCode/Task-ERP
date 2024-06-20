namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class SalesOrderUpdateApprovalStatusFormRequest
    {
        public int SalesOrderId { get; set; }
        public int SalesOrderStatusId { get; set; }
        public int OrderItemId { get; set; }
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }
        public int StatusId { get; set; }
        public int DeliveryOrderId { get; set; }
        public int DeliveryOrderItemId { get; set; }
    }
}

