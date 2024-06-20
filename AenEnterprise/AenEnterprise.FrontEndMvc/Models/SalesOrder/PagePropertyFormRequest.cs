namespace AenEnterprise.FrontEndMvc.Models.SalesOrder
{
    public class PagePropertyFormRequest
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int MaxPageToShow { get; set; }
    }
}
