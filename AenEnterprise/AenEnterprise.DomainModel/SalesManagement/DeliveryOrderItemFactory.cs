using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class DeliveryOrderItemFactory
    {
        public static DeliveryOrderItem CreateDeliveryOrderItem(DeliveryOrder deliveryOrder,OrderItem orderItem, decimal deliveryQuantity, int statusId, bool isActive)
        {
            return new DeliveryOrderItem(deliveryOrder, orderItem,deliveryQuantity, statusId,isActive);
        }
    }
}
