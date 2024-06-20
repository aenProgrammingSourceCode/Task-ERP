using AenEnterprise.DomainModel;
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
    public static class SalesOrderMapping
    {
        public static SalesOrderView ConvertToSalesOrderView(this SalesOrder salesOrder, IMapper mapper)
        {
            return mapper.Map<SalesOrder, SalesOrderView>(salesOrder);
        }

        public static IEnumerable<SalesOrderView> ConvertToSalesOrderViews(this IEnumerable<SalesOrder> salesOrders, IMapper mapper)
        {
            return mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderView>>(salesOrders);
        }
    }

}
