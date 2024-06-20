using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DeliveryOrderMessage
{
    public class CreateDeliveryOrderResponse
    {
        public DeliveryOrderView DeliveryOrder { get; set; }

    }
}
