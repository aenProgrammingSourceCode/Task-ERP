using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class DispatcheOrder
    {
        private IList<DispatchItem> _dispatchItems;

        private string _dispatcheNo;
        public DispatcheOrder()
        {
            CreatedDate= DateTime.Now;
            _dispatchItems= new List<DispatchItem>();
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DispatcheNo { get => _dispatcheNo; set => _dispatcheNo = value; }
        public int SalesOrderId { get; set; }
        public int InvoiceId { get; set; }
        public int DeliveryOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public Invoice? Invoice { get; set; }
        public DeliveryOrder DeliveryOrder { get; set; }
        public IEnumerable<DispatchItem> DispatchItems { get => _dispatchItems;}

        public void CreateDispatchItem(OrderItem orderItem,string vehicalNo,decimal vehicalEmptyWeight, 
            decimal vehicalLoadWeight,decimal dispatchQuantity, int statusId)
        {
            _dispatchItems.Add(DispatchItemFactory.CreateDispatchItem(this,orderItem,vehicalNo,vehicalEmptyWeight,vehicalLoadWeight, dispatchQuantity,statusId,true));
        }

        
    }
}
