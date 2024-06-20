namespace AenEnterprise.FrontEndMvc.Models.JsonDto
{
    public class JsonUpdateProductQtyRequests
    {
        public JsonUpdateProductQtyRequest[] Items { get; set; }
        public int salesOrderId { get; set; }
    }
}
