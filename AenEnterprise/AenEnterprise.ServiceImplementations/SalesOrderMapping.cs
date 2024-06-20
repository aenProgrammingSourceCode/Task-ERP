using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel;
using AenEnterprise.ServiceImplementations.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations
{
    public class SalesOrderMapping:Profile
    {
        public SalesOrderMapping()
        {
            CreateMap<DeliveryOrder, DeliveryOrderView>().ReverseMap();
        }
    }
}
