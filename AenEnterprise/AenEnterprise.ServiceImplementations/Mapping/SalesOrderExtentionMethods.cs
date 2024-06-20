using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel.UserDomain;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DispatchOrder;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice;
using AenEnterprise.ServiceImplementations.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class SalesOrderExtentionMethods
    {
        public static SalesOrderView ConvertToSalesOrderView(this SalesOrder salesOrder,int statusId,bool isActive)
        {
            using (var context = new AenEnterpriseDbContext())
            {
                if (salesOrder == null)
                {
                    return null;
                }

                SalesOrder salesOrderWithOrderItems;

                // Check if the SalesOrder is already being tracked by the DbContext
                var entry = context.ChangeTracker.Entries<SalesOrder>().FirstOrDefault(e => e.Entity.Id == salesOrder.Id);

                if (entry == null)
                {
                    // Ensure the Customer navigation property is loaded
                    if (salesOrder.Customer == null)
                    {
                        salesOrderWithOrderItems = context.SalesOrders
                            .AsNoTracking()
                            .Include(so => so.Customer)
                            .Include(so => so.OrderItems)
                            .ThenInclude(item => item.Product)
                            .Include(so => so.OrderItems)
                            .ThenInclude(item => item.Unit)
                            .Include(so => so.Invoices)
                            .Include(so => so.Invoices)
                            .Include(so => so.DeliveryOrders)
                            .Include(so => so.DispatcheOrders)
                            .FirstOrDefault(so => so.Id == salesOrder.Id);
                    }
                    else
                    {
                        salesOrderWithOrderItems = salesOrder;
                    }
                }
                else
                {
                    salesOrderWithOrderItems = entry.Entity;
                }

                return new SalesOrderView
                {
                    Id = salesOrderWithOrderItems.Id,
                    CreatedDate = salesOrderWithOrderItems.CreatedDate,
                    CustomerId = salesOrderWithOrderItems.CustomerId,
                    CustomerName = salesOrderWithOrderItems.Customer.Name,
                    Description = salesOrderWithOrderItems.Description,
                    OrderedDate = salesOrderWithOrderItems.OrderedDate,
                    SalesOrderNo=salesOrderWithOrderItems.SalesOrderNo,

                    OrderItems = salesOrderWithOrderItems.OrderItems.Where(oi => oi.StatusId == statusId && oi.IsActive == isActive).Select(item => new OrderItemView
                    {
                        Id = item.Id,
                        SalesOrderId = item.SalesOrderId,
                        ProductName = item.Product.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        UnitId = item.Unit.Id,
                        UnitName = item.Unit.Name,
                        Amount = item.Amount,
                        ProductId = item.Product.Id,
                        BalanceQuantity = item.BalanceQuantity,
                        InvoiceQuantity = item.InvoicedQuantity,
                        IsFullyApproved = item.IsFullyApproved,
                    }).ToList()
                };
            }
        }
        public static IList<SalesOrderView> ConvertToSalesOrderViews(this IEnumerable<SalesOrder> salesOrders, int statusId, bool isActive)
        {
            var salesOrderViews = new List<SalesOrderView>();

            foreach (var salesOrder in salesOrders)
            {
                var salesOrderView = ConvertToSalesOrderView(salesOrder,statusId, isActive);
                salesOrderViews.Add(salesOrderView);
            }

            return salesOrderViews;
        }
        public static async Task<bool> IsMinimumOrderItemExistAsync(this SalesOrder salesOrder)
        {
            using (var context = new AenEnterpriseDbContext())
            {
                var totalOrderItems = await context.SalesOrders
          .Where(so => so.OrderItems.Any(oi => oi.SalesOrderId == salesOrder.Id))
          .SelectMany(so => so.OrderItems)
          .CountAsync();

                return totalOrderItems > 1;
            }
        }
    }
}
