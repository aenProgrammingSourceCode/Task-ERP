namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class SalesOrderSearchCriteriaFormRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CriteriaName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        
    }
}
