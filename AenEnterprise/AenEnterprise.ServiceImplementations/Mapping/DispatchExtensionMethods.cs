using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class DispatchExtensionMethods
    {
        public static DispatcheOrderView ConvertToDispatchView(this DispatcheOrder dispatcheOrder, int statusId, bool isActive)
        {
            using (var context = new AenEnterpriseDbContext())
            {
                if (dispatcheOrder == null)
                {
                    return null;
                }

                DispatcheOrder dispatchWithItem;

                // Check if the SalesOrder is already being tracked by the DbContext
                var entry = context.ChangeTracker.Entries<DispatcheOrder>().FirstOrDefault(e => e.Entity.Id ==dispatcheOrder.Id);

                if (entry == null)
                {
                    // Ensure the Customer navigation property is loaded

                    dispatchWithItem = context.DispatchOrders
                           .AsNoTracking()
                           .Include(disp => disp.DispatchItems)
                           .ThenInclude(d => d.OrderItem)
                        .ThenInclude(p => p.Product)
                           .FirstOrDefault(dorder => dorder.Id ==dispatcheOrder.Id);
                }  
                else
                {
                    dispatchWithItem = entry.Entity;
                }

                return new DispatcheOrderView
                {
                    Id = dispatchWithItem.Id,
                    DeliveryOrderId=dispatchWithItem.DeliveryOrderId,
                    InvoiceId=dispatchWithItem.InvoiceId,
                    SalesOrderId = dispatchWithItem.SalesOrderId,
                    DispatchOrderNo=dispatchWithItem.DispatcheNo,
                    dispatchItems = dispatchWithItem.DispatchItems.
                    Where(disp => disp.StatusId == statusId && disp.IsActive == isActive)
                    .Select(dispatchItem => new DispatchItemView
                    {
                        Id = dispatchItem.Id,
                        ProductName =dispatchItem.OrderItem.Product.Name,
                        Price = dispatchItem.OrderItem.Price,
                        DispatchQuantity =dispatchItem.DispatchQuantity,
                        VehicalEmptyWeight =dispatchItem.VehicalEmptyWeight,
                        VehicalLoadWeight =dispatchItem.VehicalLoadWeight,
                        VehicalNo=dispatchItem.VehicalNo,
                        OrderItemId=dispatchItem.OrderItemId 
                    }).ToList(),
                };
            }
        }
        public static IList<DispatcheOrderView> ConvertToDispatchViews(this IEnumerable<DispatcheOrder> dispatcheOrders, int statusId, bool isActive)
        {
            var dispatchViews = new List<DispatcheOrderView>();

            foreach (var dispatcheOrder in dispatcheOrders)
            {
                var dispatchOrder = ConvertToDispatchView(dispatcheOrder, statusId, isActive);
                dispatchViews.Add(dispatchOrder);
            }

            return dispatchViews;
        }
    }
}
