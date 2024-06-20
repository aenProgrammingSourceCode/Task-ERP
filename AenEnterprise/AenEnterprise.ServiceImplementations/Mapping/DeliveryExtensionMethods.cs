using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class DeliveryExtensionMethods
    {
        public static DeliveryOrderView ConvertToDeliveryOrderView(this DeliveryOrder deliveryOrder, int statusId, bool isActive)
        {
            using (var context = new AenEnterpriseDbContext())
            {
                if (deliveryOrder == null)
                {
                    return null;
                }

                DeliveryOrder deliveryOrderWithItem;

                // Check if the DeliveryOrder is already being tracked by the DbContext
                var entry = context.ChangeTracker.Entries<DeliveryOrder>().FirstOrDefault(e => e.Entity.Id == deliveryOrder.Id);

                if (entry == null)
                {
                    // Ensure the Customer navigation property is loaded
                    deliveryOrderWithItem = context.DeliveryOrders
                        .AsNoTracking()
                        .Include(so => so.SalesOrder)
                            .ThenInclude(so => so.Customer)
                        .Include(d => d.DeliveryOrderItem)
                        .ThenInclude(d=>d.OrderItem)
                        .ThenInclude(p=>p.Product)
                        .FirstOrDefault(d => d.Id == deliveryOrder.Id);
                }
                else
                {
                    deliveryOrderWithItem = entry.Entity;
                }

                return new DeliveryOrderView
                {
                     Id = deliveryOrderWithItem.Id,
                    CustomerName = deliveryOrderWithItem.SalesOrder.Customer.Name,
                    CreatedDate = deliveryOrderWithItem.CreatedDate,
                    deliveryOrderNo = deliveryOrderWithItem.DeliveryOrderNo,
                    DeliveryOrderItems = deliveryOrderWithItem.DeliveryOrderItem
                        .Where(deli => deli.StatusId == statusId && deli.IsActive == isActive)
                        .Select(deliveryItem => new DeliveryOrderItemView
                        {
                            Id = deliveryItem.Id,
                            ProductName = deliveryItem.OrderItem.Product.Name,
                            Price = deliveryItem.OrderItem.Price,
                            DeliveryQuantity = deliveryItem.DeliveryQuantity,
                            BalanceQuantity=deliveryItem.BalanceQuantity,
                            DeliveryAmount = deliveryItem.DeliveryAmount,
                            DeliveryOrderId = deliveryItem.DeliveryOrderId,
                            OrderItemId=deliveryItem.OrderItemId
                        }).ToList(),
                };
            }
        }


        public static IList<DeliveryOrderView> ConvertToDeliveryOrderViews(this IEnumerable<DeliveryOrder> deliveryOrders, int statusId, bool isActive)
        {
            var deliveryOrderViews = new List<DeliveryOrderView>();

            foreach (var deliveryOrder in deliveryOrders)
            {
                var deliveryOrderView = ConvertToDeliveryOrderView(deliveryOrder, statusId, isActive);
                deliveryOrderViews.Add(deliveryOrderView);
            }

            return deliveryOrderViews;
        }
    }
}
