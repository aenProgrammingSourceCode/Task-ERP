using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DispatchOrder;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice;
using AenEnterprise.ServiceImplementations.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class InvoiceExtentionMethods
    {
        public static InvoiceView ConvertToInvoiceView(this Invoice invoice, int statusId, bool isActive)
        {
            using (var context = new AenEnterpriseDbContext())
            {
                if (invoice == null)
                {
                    return null;
                }

                Invoice invoiceWithInvoiceItems;

                // Check if the SalesOrder is already being tracked by the DbContext
                var entry = context.ChangeTracker.Entries<Invoice>().FirstOrDefault(e => e.Entity.Id == invoice.Id);

                if (entry == null)
                {
                    // Ensure the Customer navigation property is loaded
                    invoiceWithInvoiceItems = context.Invoices
                        .AsNoTracking()
                        .Include(so=>so.SalesOrder)
                        .ThenInclude(so=>so.Customer)
                        .Include(so => so.InvoiceItems)
                            .ThenInclude(item => item.OrderItem.Product)
                        .Include(so => so.InvoiceItems)
                            .ThenInclude(item => item.OrderItem.Unit)
                        .Include(so => so.DeliveryOrders)
                        .Include(so => so.DispatcheOrders) // Corrected property name
                        .FirstOrDefault(so => so.Id == invoice.Id);
                }
                else
                {
                    invoiceWithInvoiceItems = entry.Entity;
                }

                return new InvoiceView
                {
                    Id = invoiceWithInvoiceItems.Id,
                    CreatedDate = invoiceWithInvoiceItems.CreatedDate,
                    CustomerId = invoiceWithInvoiceItems.SalesOrder.Customer.Id,
                    CustomerName = invoiceWithInvoiceItems.SalesOrder.Customer.Name,
                    SalesOrderId = invoiceWithInvoiceItems.SalesOrderId,
                    InvoiceNo=invoiceWithInvoiceItems.InvoiceNo,
                    InvoiceItems = invoiceWithInvoiceItems.InvoiceItems
                        .Where(inv => inv.StatusId == statusId && inv.IsActive == isActive).Select(invoiceItem => new InvoiceItemView
                        {
                            Id = invoiceItem.Id,
                            ProductName = invoiceItem.OrderItem.Product.Name,
                            Price = invoiceItem.OrderItem.Price,
                            BalanceQuantity = invoiceItem.BalanceQuantity,
                            DeliveryQuantity = invoiceItem.DeliveryQuantity,
                            InvoiceQuantity = invoiceItem.InvoiceQuantity,
                            InvoiceAmount = invoiceItem.InvoiceAmount,
                            ProductId = invoiceItem.OrderItem.ProductId,
                            InvoiceId=invoiceItem.InvoiceId,
                            UnitId = invoiceItem.OrderItem.UnitId,
                            UnitName = invoiceItem.OrderItem.Unit.Name,
                            OrderItemId=invoiceItem.OrderItemId
                        }).ToList() 
                };
            }
        }
        public static IList<InvoiceView> ConvertToInvoiceViews(this IEnumerable<Invoice> invoices, int statusId, bool isActive)
        {
            var invoiceViews = new List<InvoiceView>();

            foreach (var invoice in invoices)
            {
                var invoiceView = ConvertToInvoiceView(invoice, statusId, isActive);
                invoiceViews.Add(invoiceView);
            }

            return invoiceViews;
        }
        public static async Task<bool> GetOrderItemBalanceQuantity(this SalesOrder salesOrder)
        {
            using (var context = new AenEnterpriseDbContext())
            {
                bool IsBalanceQuantityExist = await context.SalesOrders
                    .Where(so => so.Id == salesOrder.Id && so.OrderItems.Any(oi => oi.BalanceQuantity > 1))
                    .AnyAsync();

                return IsBalanceQuantityExist;
            }
        }

    }
}
    