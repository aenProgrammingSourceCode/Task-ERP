using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping.Automappers
{
    public static class DeliveryOrderMapper
    {
        public static DeliveryOrderView ConvertToDeliveryOrderView(this DeliveryOrder deliveryOrder, IMapper mapper)
        {
            return mapper.Map<DeliveryOrder, DeliveryOrderView>(deliveryOrder);
        }

        public static IEnumerable<DeliveryOrderView> ConvertToDeliveryOrderViews(this IEnumerable<DeliveryOrder> deliveryOrders, IMapper mapper)
        {
            return mapper.Map<IEnumerable<DeliveryOrder>, IEnumerable<DeliveryOrderView>>(deliveryOrders);
        }
    }
}
