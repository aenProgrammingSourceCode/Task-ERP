using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping.Mapsters
{
    public static class DeliveryOrderMapper
    {
        public static DeliveryOrderView ConvertToMapsterDeliveryOrderView(this DeliveryOrder deliveryOrder)
        {
            return deliveryOrder.Adapt<DeliveryOrderView>(); 
        }
        public static IEnumerable<DeliveryOrderView> ConvertToMapsterDeliveryOrderViews(this IEnumerable<DeliveryOrder> deliveryOrders)
        {
            return deliveryOrders.Adapt<IEnumerable<DeliveryOrderView>>();
        }
    }
}
