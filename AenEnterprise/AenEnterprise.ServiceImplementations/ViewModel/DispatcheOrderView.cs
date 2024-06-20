using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
    public class DispatcheOrderView
    {
        
        public DispatcheOrderView()
        {
            dispatchItems =new List<DispatchItemView>();
        }
        public int Id { get; set; }
        
        public int SalesOrderId { get; set; }
        public int InvoiceId { get; set; }
        public int DeliveryOrderId { get; set; }
        public string DispatchOrderNo { get; set; }
        public int ProductId { get; set; }
        public List<DispatchItemView> dispatchItems { get; set; }
    }
}
