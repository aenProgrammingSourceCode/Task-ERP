using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class DispatchItemFactory
    {
        public static DispatchItem CreateDispatchItem(DispatcheOrder dispatcheOrder,OrderItem orderItem,
            
            string vehicalNo, decimal vehicalEmptyWeight, decimal vehicalLoadWeight, decimal quantity,int statusId,bool isActive)
        {
            return new DispatchItem(dispatcheOrder,orderItem,vehicalNo,vehicalEmptyWeight,vehicalLoadWeight, quantity,statusId,isActive);
        }
    }
}
