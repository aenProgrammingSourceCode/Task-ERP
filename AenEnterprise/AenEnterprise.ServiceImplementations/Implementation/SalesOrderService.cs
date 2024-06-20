using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using AenEnterprise.ServiceImplementations.Interface;
using AenEnterprise.ServiceImplementations.Mapping;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice;
using Azure.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DeliveryOrder;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DataAccess.Repository;
using AenEnterprise.DomainModel.UserDomain;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Azure;
using Microsoft.AspNetCore.Mvc;
using AenEnterprise.DataAccess;
using AutoMapper;
using AenEnterprise.ServiceImplementations.ViewModel;
using AenEnterprise.ServiceImplementations.Messaging.InventoryManagement;
using System.Globalization;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DispatchOrder;
using System.Net;
using AenEnterprise.DomainModel.CookieStorage;

namespace AenEnterprise.ServiceImplementations.Implementation
{
    public class SalesOrderService : ISalesOrderService
    {
        private AenEnterpriseDbContext _context;
        private ISalesOrderRepository _salesOrderRepository;
        private IUnitOfWork _unitOfWork;
        private IBankAccountRepository _bankAccountRepository;
        private IProductRepository _productRepository;
        private ISalesOrderStatusRepository _salesOrderStatusRepository;
        private IUnitRepository _unitRepository;
        private IOrderItemRepository _orderItemRepository;
        private readonly HttpRequest _request;
        private IInvoiceRepository _invoiceRepository;
        private ICustomerRepository _customerRepository;
        private IDeliveryOrderRepository _deliveryOrderRepository;
        private IDispatcheRepository _dispatcheRepository;
        private ICookieImplementation _cookieImplementation;
        public SalesOrderService(ISalesOrderRepository salesOrderRepository,
            IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork, IProductRepository productRepository,
            IUnitRepository unitRepository,
            ISalesOrderStatusRepository salesOrderStatusRepository,
            IOrderItemRepository orderItemRepository,
        IHttpContextAccessor httpContextAccessor,
            IInvoiceRepository invoiceRepository,
            ICustomerRepository customerRepository,
        IDeliveryOrderRepository deliveryOrderRepository,
        IDispatcheRepository dispatcheRepository,
        ICookieImplementation cookieImplementation)
        {
            _context = new AenEnterpriseDbContext();
            _orderItemRepository = orderItemRepository;
            _salesOrderStatusRepository = salesOrderStatusRepository;
            _unitRepository = unitRepository;
            _salesOrderRepository = salesOrderRepository;
            _unitOfWork = unitOfWork;
            _bankAccountRepository = bankAccountRepository;
            _productRepository = productRepository;
            _cookieImplementation = cookieImplementation;

            _request = httpContextAccessor.HttpContext.Request;
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _dispatcheRepository = dispatcheRepository;
        }
        public async Task<GetSalesOrderResponse> CreateSalesOrderAsync(CreateSalesOrderRequest request)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();
            Product product = await _productRepository.GetByIdAsync(request.ProductId);
            Customer customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            SalesOrderStatus salesOrderStatus = await _salesOrderStatusRepository.GetByIdAsync(request.SalesOrderStatusId);
            Unit unit = await _unitRepository.GetByIdAsync(request.UnitId);
            var salesOrders = await _salesOrderRepository.FindAllAsync();
            int LastOrderId = salesOrders.Any() ? salesOrders.Last().Id : 0;
            SalesOrder newSalesOrder = new SalesOrder();
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.Id);

            if (salesOrder != null)
            {
                if (await OrderItemExistsWithSameProduct(salesOrder, product, request.Price))
                {
                    throw new InvalidOperationException("Selected Product already exists in current order");
                }
                else
                {
                    salesOrder.CreateOrderItem(product, unit, request.Quantity, request.Price, request.DiscountPercent, request.TotalInvoiceQuantity, 1, true);
                }

                await _salesOrderRepository.UpdateAsync(salesOrder);
                response.SalesOrder = salesOrder.ConvertToSalesOrderView(1, true);

            }
            else
            {
                newSalesOrder.OrderedDate = request.OrderedDate;
                newSalesOrder.CreatedDate = DateTime.Now;
                newSalesOrder.CustomerId = customer.Id;
                newSalesOrder.Description = request.Description;
                newSalesOrder.SalesOrderStatusId = 1;
                newSalesOrder.DeliveryPlane = request.DeliveryPlane;

                newSalesOrder.CreateOrderItem(product, unit, request.Quantity, request.Price, request.DiscountPercent, request.TotalInvoiceQuantity, 1, true);
                if (LastOrderId > 0)
                {
                    newSalesOrder.SalesOrderNo = "SO-" + (LastOrderId + 1).ToString();
                }
                else
                {
                    newSalesOrder.SalesOrderNo = "SO-" + 1.ToString();
                }
                await _salesOrderRepository.AddAsync(newSalesOrder);
                response.SalesOrder = newSalesOrder.ConvertToSalesOrderView(1, true);
            }

            return response;
        }
        public async Task<bool> OrderItemExistsWithSameProduct(SalesOrder salesOrder, Product product, decimal price)
        {
            var orderItems = await _orderItemRepository.FindAsync(i => i.SalesOrderId == salesOrder.Id && i.Product == product && i.Price == price);

            // Check if any order item with the same product exists in the sales order
            return orderItems.Any();
        }

        public async Task<GetSalesOrderResponse> DeleteOrderItemAsync(DeleteOrderItemRequest request)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            OrderItem orderItem = await _orderItemRepository.GetByIdAsync(request.OrderItemId);
            
            await _orderItemRepository.RemoveAsync(orderItem);
            await _unitOfWork.SaveAsync();

            response.SalesOrder = salesOrder.ConvertToSalesOrderView(1, true);
            return response;
        }


        public async Task<GetSalesOrderResponse> DeleteSalesOrderBasedOnOrderLimitAsync(DeleteOrderItemRequest request)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();

            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            OrderItem orderItem = await _orderItemRepository.GetByIdAsync(request.OrderItemId);

            bool isSalesOrderExist = await salesOrder.IsMinimumOrderItemExistAsync();

            if (isSalesOrderExist)
            {
                await _orderItemRepository.RemoveAsync(orderItem);
            }
            else
            {
                await _salesOrderRepository.RemoveAsync(salesOrder);
                _cookieImplementation.Remove(CookieDataKey.SalesOrderId.ToString());
                response.SalesOrder = null; // Set response to null
                return response;
            }

            await _unitOfWork.SaveAsync();
            response.SalesOrder = salesOrder.ConvertToSalesOrderView(1, true);
            return response;
        }





        public async Task<GetSalesOrderResponse> DeleteSalesOrderAsync(DeleteOrderItemRequest request)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();

            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            await _salesOrderRepository.RemoveAsync(salesOrder);
            response.SalesOrder=salesOrder.ConvertToSalesOrderView(1, true);
            await _unitOfWork.SaveAsync();
            return response;
        }

        public async Task DeleteSalesOrderAndOrderItemAsync(int salesOrderId)
        {
            try
            {
                SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(salesOrderId);

                if (salesOrder != null)
                {
                    // Remove related order items
                    foreach (var orderItem in salesOrder.OrderItems.ToList())
                    {
                        await _orderItemRepository.RemoveAsync(orderItem);
                    }

                    // Remove sales order
                    await _salesOrderRepository.RemoveAsync(salesOrder);

                    // Save changes to the database
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                // Log or throw, depending on your requirements
                Console.WriteLine($"Error deleting sales order: {ex.Message}");
                throw;
            }
        }




        public async Task AddSalesOrdersAsync(int salesOrderId)
        {
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(salesOrderId);
            if (salesOrder != null)
            {
                salesOrder.SalesOrderStatusId = 2;
                await _salesOrderRepository.UpdateAsync(salesOrder);
                await _unitOfWork.SaveAsync();
            }
        }


        public async Task<GetAllSalesOrderResponse> GetAllUnApprovedOrderItems(SalesOrderSearchCriteriaRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();

            var salesOrderIdsWithStatus1 = await _context.SalesOrders
                .Where(so => so.OrderItems.Any(oi => oi.StatusId == 1))
                .Select(so => so.Id)
                .ToListAsync();

            IQueryable<SalesOrder> salesOrderByCriteria = _context.SalesOrders
                .Where(e =>
                    EF.Functions.Like(e.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.OrderItems.Any(oi =>
                        EF.Functions.Like(oi.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                    EF.Functions.Like(e.Invoices.FirstOrDefault().InvoiceNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.DeliveryOrders.FirstOrDefault().DeliveryOrderNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.DispatcheOrders.FirstOrDefault().DispatcheNo, $"%{request.CriteriaName}%")
                );

            IQueryable<SalesOrder> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.OrderedDate.Date >= request.DateFrom.Date && dt.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.OrderedDate).ThenByDescending(or=>or.Id).Where(so => salesOrderIdsWithStatus1.Contains(so.Id));

            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<SalesOrder> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.SalesOrders = paginateSalesOrder.ConvertToSalesOrderViews(1, true);
            response.TotalPages = totalPages;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            response.TotalCount = totalCount;

            await _unitOfWork.SaveAsync();

            return response;
        }




        public async Task<GetAllSalesOrderResponse> ApprovalStatusOrderItemsAsync(UpdateSalesOrderApprovalStatusRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
            IEnumerable<SalesOrder> salesOrders = await _salesOrderRepository.FindAllAsync();
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            var orderItem = await _orderItemRepository.GetByIdAsync(request.OrderItemId); // Assuming there's a method GetByIdAsync for order items

            try
            {
                if (salesOrder != null && orderItem != null)
                {
                    // Checking balance quantity against invoiced quantity
                    orderItem.IsFullyApproved = false;
                    orderItem.IsPartiallyApproved =true;
                    orderItem.StatusId = request.StatusId;

                    // Updating order item
                    await _orderItemRepository.UpdateAsync(orderItem);

                    await _salesOrderRepository.UpdateAsync(salesOrder);

                    // Saving changes in the unit of work
                    await _unitOfWork.SaveAsync();

                    // Converting sales orders to views
                    response.SalesOrders = salesOrders.ConvertToSalesOrderViews(2, true);
                    return response;
                }
                else
                {
                    // Handle scenario when sales order or order item is not found
                    // You might want to throw a specific exception or handle it accordingly
                    // For now, returning null response
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Logging or handling the exception
                // For now, rethrowing the exception with a custom error message
                throw new Exception("Sorry, SalesOrder not found", ex);
            }

        }

        public async Task<GetAllSalesOrderResponse>  GetAllApprovedOrderItems(SalesOrderSearchCriteriaRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();

            var salesOrderIdsWithStatus1 = await _context.SalesOrders
                .Where(so => so.OrderItems.Any(oi => oi.StatusId == 2 && oi.IsPartiallyApproved==true))
                .Select(so => so.Id)
                .ToListAsync();

            IQueryable<SalesOrder> salesOrderByCriteria = _context.SalesOrders
                .Where(e =>
                    EF.Functions.Like(e.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.OrderItems.Any(oi =>
                        EF.Functions.Like(oi.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                    EF.Functions.Like(e.Invoices.FirstOrDefault().InvoiceNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.DeliveryOrders.FirstOrDefault().DeliveryOrderNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.DispatcheOrders.FirstOrDefault().DispatcheNo, $"%{request.CriteriaName}%")
                );

            IQueryable<SalesOrder> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.OrderedDate.Date >= request.DateFrom.Date && dt.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.OrderedDate)
                .Where(so => salesOrderIdsWithStatus1.Contains(so.Id));

            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<SalesOrder> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.SalesOrders = paginateSalesOrder.ConvertToSalesOrderViews(2, true);
            response.TotalPages = totalPages;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            response.TotalCount = totalCount;

            await _unitOfWork.SaveAsync();

            return response;
        }


        public async Task<GetSalesOrderResponse> GetSalesOrderById(int salesOrderId)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(salesOrderId);
            response.SalesOrder = salesOrder.ConvertToSalesOrderView(1, true);
            return response;
        }
        public async Task CreateCustomInvoiceAsync(CreateInvoiceRequest request)
        {
            try
            {
                SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
                if (salesOrder != null && salesOrder.OrderItems != null)
                {
                    var invoices = await _invoiceRepository.FindAllAsync();
                    int LastInvoiceId = invoices.Any() ? invoices.Last().Id : 0;

                    Invoice invoice = new Invoice();
                    invoice.SalesOrderId = request.SalesOrderId;
                    if (LastInvoiceId > 0)
                    {
                        invoice.InvoiceNo = "INV-" + (LastInvoiceId + 1).ToString();
                    }
                    else
                    {
                        invoice.InvoiceNo = "INV-1";
                    }
                    await _invoiceRepository.AddAsync(invoice);
                    await AddOrderItem(request.orderItemIdToAdd, salesOrder, request.InvoiceQuantity);
                    await _salesOrderRepository.UpdateAsync(salesOrder);
                }
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                // Handle exception/log error
                // Rollback transaction if using explicit transactions
            }
        
        }

        private async Task AddOrderItem(IList<int> orderItemIdToAdd, SalesOrder salesOrder, decimal quantity)
        {
            foreach (int orderItemId in orderItemIdToAdd)
            {
                OrderItem orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
                if (orderItem != null)
                {
                    salesOrder.setOrderItem(orderItem, quantity);
                    await _orderItemRepository.UpdateAsync(orderItem);
                }
                else
                {
                    // Handle the case where the order item was not found (e.g., log an error or throw an exception).
                    // Depending on your application's requirements, you may choose to skip this order item or take other actions.
                }
            }
        }



        public async Task<GetAllInvoiceResponse> GetAllUnApprovedInvoice(SalesOrderSearchCriteriaRequest request)
        {
            GetAllInvoiceResponse response = new GetAllInvoiceResponse();

            var salesOrderIdWithStatus1 = await _context.Invoices
                .Where(so => so.InvoiceItems.Any(oi => oi.StatusId == 1))
                .Select(so => so.Id)
                .ToListAsync();

            IQueryable<Invoice> salesOrderByCriteria = _context.Invoices
                .Where(e =>
                    EF.Functions.Like(e.SalesOrder.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrder.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.SalesOrder.OrderItems.Any(oi =>
                        EF.Functions.Like(oi.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                    EF.Functions.Like(e.InvoiceNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrder.DeliveryOrders.FirstOrDefault().DeliveryOrderNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrder.DispatcheOrders.FirstOrDefault().DispatcheNo, $"%{request.CriteriaName}%")
                );

            IQueryable<Invoice> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.SalesOrder.OrderedDate.Date >= request.DateFrom.Date && dt.SalesOrder.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.SalesOrder.OrderedDate).Where(so => salesOrderIdWithStatus1.Contains(so.Id));

            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<Invoice> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.Invoices = paginateSalesOrder.ConvertToInvoiceViews(1,true);
            response.TotalPages = totalPages;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            response.TotalCount = totalCount;

            await _unitOfWork.SaveAsync();

            return response;
        }




        public async Task<GetAllSalesOrderResponse> ApprovalStatusInvoiceAsync(UpdateSalesOrderApprovalStatusRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
            IEnumerable<SalesOrder> salesOrders = await _salesOrderRepository.FindAllAsync();
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId); // Assuming there's a method GetByIdAsync for order items

            try
            {
                if (salesOrder != null && invoice != null)
                {
                    salesOrder.SalesOrderStatusId = 5;
                    invoice.StatusId = request.StatusId;
                    invoice..IsPartiallyApproved = true;
                    invoice.IsFullyApproved = true;
                    await _salesOrderRepository.UpdateAsync(salesOrder);
                    await _invoiceRepository.UpdateAsync(invoice);
                    await _unitOfWork.SaveAsync();

                    response.SalesOrders = salesOrders.ConvertToSalesOrderViews(1, true);
                    return response;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Sorry, SalesOrder not found", ex);
            }

        }

        public async Task<GetAllSalesOrderResponse> ApprovalStatusDeliveryOrderAsync(UpdateSalesOrderApprovalStatusRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
            IEnumerable<SalesOrder> salesOrders = await _salesOrderRepository.FindAllAsync();
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            var deliveryOrder = await _deliveryOrderRepository.GetByIdAsync(request.DeliveryOrderId); // Assuming there's a method GetByIdAsync for order items

            try
            {
                if (salesOrder != null && deliveryOrder != null)
                {
                    salesOrder.SalesOrderStatusId = 7;
                    deliveryOrder.StatusId = request.StatusId;
                    deliveryOrder.IsPartiallyApproved = true;
                    deliveryOrder.IsFullyApproved = true;
                    await _salesOrderRepository.UpdateAsync(salesOrder);
                    await _deliveryOrderRepository.UpdateAsync(deliveryOrder);
                    await _unitOfWork.SaveAsync();

                    response.SalesOrders = salesOrders.ConvertToSalesOrderViews(1, true);
                    return response;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Sorry, SalesOrder not found", ex);
            }

        }

        public async Task<GetAllSalesOrderResponse> GetAllApprovedInvoice(SalesOrderSearchCriteriaRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
            var salesOrderIdsWithStatus2 = await _context.SalesOrders
                .Where(so => so.Invoices.Any(oi => oi.StatusId == 2))
                .Select(so => so.Id)
                .ToListAsync();
            IQueryable<SalesOrder> salesOrderByCriteria = _context.SalesOrders
                .Where(e =>
                    EF.Functions.Like(e.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.Invoices.Any(oi =>
                        EF.Functions.Like(oi.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                      EF.Functions.Like(e.Invoices.FirstOrDefault().InvoiceNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.DeliveryOrders.FirstOrDefault().DeliveryOrderNo, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.DispatcheOrders.FirstOrDefault().DispatcheNo, $"%{request.CriteriaName}%")
                );
            //.Where(e => e.OrderItems.Any(oi => oi.IsFullyApproved) && e.OrderItems.All(oi => oi.StatusId == 2))
            //.AsQueryable();

            IQueryable<SalesOrder> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.OrderedDate.Date >= request.DateFrom.Date && dt.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.OrderedDate).Where(so => salesOrderIdsWithStatus2.Contains(so.Id));


            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<SalesOrder> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.SalesOrders = paginateSalesOrder.ConvertToApprovedInvoiceViews(2, true);
            response.TotalPages = totalPages;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            response.TotalCount = totalCount;

            await _unitOfWork.SaveAsync();

            return response;
        }

        public async Task<GetSalesOrderResponse> GetApprovedOrderItemDetailsAsync(int salesOrderId)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();

            // Retrieve the SalesOrder entity with Invoices navigation property eagerly loaded
            SalesOrder salesOrder = await _salesOrderRepository
                .GetByIdAsync(salesOrderId); // Adjust this method to load SalesOrder without Invoices

            // Check if the SalesOrder entity is retrieved
            if (salesOrder != null)
            {
                response.SalesOrder = salesOrder.ConvertToApprovedOrderItemView(2, true);
            }
            else
            {
                // Handle scenario where SalesOrder couldn't be loaded
                // For instance, log an error or set response.SalesOrder to null
                response.SalesOrder = null;
            }

            return response;
        }

        public async Task<GetInvoiceResponse> GetApprovedInvoiceDetailsAsync(int invoiceId)
        {
            GetInvoiceResponse response = new GetInvoiceResponse();
            Invoice invoice = await _invoiceRepository
                .GetByIdAsync(invoiceId); // Adjust this method to load SalesOrder without Invoices

            // Check if the SalesOrder entity is retrieved
            if (invoice != null)
            {
                response.Invoice =invoice.ConvertToInvoiceView(2, true);
            }
            else
            {
                // Handle scenario where SalesOrder couldn't be loaded
                // For instance, log an error or set response.SalesOrder to null
                response.Invoice = null;
            }

            return response;
        }

        public async Task<GetSalesOrderResponse> GetApprovedDeliveryOrderDetailsAsync(int salesOrderId)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();
            SalesOrder salesOrder = await _salesOrderRepository
                .GetByIdAsync(salesOrderId); // Adjust this method to load SalesOrder without Invoices

            // Check if the SalesOrder entity is retrieved
            if (salesOrder != null)
            {
                response.SalesOrder = salesOrder.ConvertToApprovedDeliveryOrderView(2, true);
            }
            else
            {
                // Handle scenario where SalesOrder couldn't be loaded
                // For instance, log an error or set response.SalesOrder to null
                response.SalesOrder = null;
            }

            return response;
        }



        public async Task<GetSalesOrderResponse> UpdateSalesOrder(UpdateSalesOrderRequest request)
        {
            GetSalesOrderResponse response = new GetSalesOrderResponse();
            Product product = await _productRepository.GetByIdAsync(request.ProductId);
            SalesOrderStatus salesOrderStatus = await _salesOrderStatusRepository.GetByIdAsync(request.SalesOrderStatusId);
            Unit unit = await _unitRepository.GetByIdAsync(request.UnitId);
            Customer customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.Id);
            salesOrder.CreatedDate = DateTime.Now;
            salesOrder.OrderedDate = request.OrderedDate;
            salesOrder.CustomerId = customer.Id;
            salesOrder.Description = request.Description;
            salesOrder.SalesOrderStatusId = 1;

            salesOrder.SalesOrderNo = "SO-" + request.Id.ToString();
            response.SalesOrder = salesOrder.ConvertToSalesOrderView(1, true);
            return response;
        }
        public async Task CreateInvoiceAsync(int salesOrderId)
        {
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(salesOrderId);
            if (salesOrder != null)
            {
                salesOrder = salesOrder.ConvertToInvoice(); // Use the extension method
                await _salesOrderRepository.UpdateAsync(salesOrder); // Update the modified salesOrder
                await _unitOfWork.SaveAsync(); // Ensure the unit of work supports asynchronous saving
            }
        }
        public async Task<GetAllSalesOrderResponse> UpdateInvoiceApprovalStatusAsync(UpdateSalesOrderApprovalStatusRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
            IEnumerable<SalesOrder> salesOrders = await _salesOrderRepository.FindAllAsync();

            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            if (salesOrder != null)
            {
                salesOrder.SalesOrderStatusId = request.SalesOrderStatusId;
                await _salesOrderRepository.UpdateAsync(salesOrder);
            }
            await _unitOfWork.SaveAsync();

            response.SalesOrders = salesOrders.ConvertToSalesOrderViews(1, true);
            return response;
        }
        public async Task<GetAllSalesOrderResponse> GetAllSalesOrdersBasket()
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
            IEnumerable<SalesOrder> salesOrders = await _salesOrderRepository.FindAllAsync();
            response.SalesOrders = salesOrders.ConvertToSalesOrderViews(1, true);
            await _unitOfWork.SaveAsync();
            return response;
        }
        public async Task CreateBankAccount()
        {
            BankAccount bankAccount = new BankAccount();
            bankAccount.Id = Guid.NewGuid();
            bankAccount.Deposit(2000, "Deposit");
            bankAccount.Withdraw(2000, "Widtraw important");
            await _bankAccountRepository.AddAsync(bankAccount);
            await _unitOfWork.SaveAsync();
        }
        public async Task CreateCustomDeliveryOrderAsync(CreateDeliveryOrderRequest request)
        {
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            Invoice invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            DeliveryOrder deliveryOrder = new DeliveryOrder
            {
                SalesOrderId = request.SalesOrderId,
                DeliveryOrderQuantity = request.DeliveryOrderQuantity,
                Price = request.Price,
                ProductId = request.ProductId,
                UnitId = request.UnitId,
                StatusId = 1,
                IsActive = true,
                InvoiceId = request.InvoiceId,
            };

            await _invoiceRepository.UpdateAsync(invoice);
            await _deliveryOrderRepository.AddAsync(deliveryOrder);
            await _salesOrderRepository.UpdateAsync(salesOrder);
            await _unitOfWork.SaveAsync();
        }


        public async Task CreateCustomDispatchAsync(CreateDispatchRequest request)
        {
            try
            {
                SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
                DeliveryOrder deliveryOrder = await _deliveryOrderRepository.GetByIdAsync(request.DeliveryOrderId);
                salesOrder.ConvertToDispatch(request);
                await _deliveryOrderRepository.UpdateAsync(deliveryOrder);
                await _salesOrderRepository.UpdateAsync(salesOrder);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while processing the custom dispatch.", ex);
            }

        }

        public async Task ProcessDispatch(int salesOrderId)
        {
            SalesOrder salesOrder=await _salesOrderRepository.GetByIdAsync(salesOrderId);
            if(salesOrder!=null)
            {
                foreach (var dispatchItem in salesOrder.DeliveryOrders)
                {
                    DispatcheOrder dispatcheOrder = new DispatcheOrder
                    {
                        ProductId = dispatchItem.ProductId,
                        Price = dispatchItem.Price,
                        UnitId = dispatchItem.UnitId,
                        DeliveryOrderId = dispatchItem.Id,
                    };
                    await _dispatcheRepository.AddAsync(dispatcheOrder);
                }
            }
        }


        public async Task<OrderItem> GetOrderItemForAsync(Product product)
        {
            var orderItems = await _orderItemRepository.FindAllAsync();
            var item = orderItems.FirstOrDefault(oi => oi.Product == product);
            return await Task.FromResult(item);
        }

        
        public async Task<bool> OrderItemContainsProductForInvoiceAsync(Product product)
        {
            return (await _orderItemRepository.FindAsync(i => i.Product == product)).Any();
        }
        public async Task<GetAllProductReponse> GetAllProductsAsync()
        {
            GetAllProductReponse response = new GetAllProductReponse();
            IEnumerable<Product> products = await _productRepository.FindAllAsync();
            response.Products = products.ConvertToProductViews();
            return response;
        }
        public Task<GetAllProductReponse> GetAllProducts()
        {
            throw new NotImplementedException();
        }
        public Task CreateInvoice(int salesOrderId)
        {
            throw new NotImplementedException();
        }

        //public async Task<GetAllSalesOrderResponse> GetAllApproveDeliveryOrder(SalesOrderSearchCriteriaRequest request)
        //{
        //    GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
        //    var salesOrderIdsWithStatus2 = await _context.SalesOrders.Where(so => so.DeliveryOrders.Any(oi => oi.StatusId == 2)).Select(so => so.Id).ToListAsync();
        //    IQueryable<SalesOrder> salesOrderByCriteria = _context.SalesOrders
        //        .Where(e =>
        //            EF.Functions.Like(e.Customer.Name, $"%{request.CriteriaName}%") ||
        //            EF.Functions.Like(e.SalesOrderNo, $"%{request.CriteriaName}%") ||
        //            e.Invoices.Any(oi =>
        //                EF.Functions.Like(oi.Product.Name, $"%{request.CriteriaName}%") ||
        //                EF.Functions.Like(oi.Price.ToString(), $"%{request.CriteriaName}%")
        //            ) ||
        //              EF.Functions.Like(e.Invoices.FirstOrDefault().InvoiceNo, $"%{request.CriteriaName}%") ||
        //            EF.Functions.Like(e.DeliveryOrders.FirstOrDefault().DeliveryOrderNo, $"%{request.CriteriaName}%") ||
        //            EF.Functions.Like(e.DispatcheOrders.FirstOrDefault().DispatcheNo, $"%{request.CriteriaName}%")
        //        );
        //    //.Where(e => e.OrderItems.Any(oi => oi.IsFullyApproved) && e.OrderItems.All(oi => oi.StatusId == 2))
        //    //.AsQueryable();

        //    IQueryable<SalesOrder> salesOrderByDateRange = salesOrderByCriteria
        //        .Where(dt => dt.OrderedDate.Date >= request.DateFrom.Date && dt.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
        //        .OrderByDescending(or => or.OrderedDate).Where(so => salesOrderIdsWithStatus2.Contains(so.Id));


        //    int totalCount = await salesOrderByDateRange.CountAsync();
        //    int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        //    int skipCount = (request.PageNumber - 1) * request.PageSize;

        //    IEnumerable<SalesOrder> paginateSalesOrder = await salesOrderByDateRange
        //        .Skip(skipCount)
        //        .Take(request.PageSize)
        //        .ToListAsync();

        //    //response.SalesOrders = paginateSalesOrder.ConvertToApprovedDeliveryOrderViews(2, true);
        //    response.TotalPages = totalPages;
        //    response.PageNumber = request.PageNumber;
        //    response.PageSize = request.PageSize;
        //    response.TotalCount = totalCount;

        //    await _unitOfWork.SaveAsync();

        //    return response;
        //}

        //public async Task<GetAllSalesOrderResponse> GetAllUnApproveDeliveryOrder(SalesOrderSearchCriteriaRequest request)
        //{
        //    GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
        //    var salesOrderIdsWithStatus2 = await _context.SalesOrders.Where(so => so.DeliveryOrders.Any(oi => oi.StatusId == 1)).Select(so => so.Id).ToListAsync();
        //    IQueryable<SalesOrder> salesOrderByCriteria = _context.SalesOrders
        //        .Where(e =>
        //            EF.Functions.Like(e.Customer.Name, $"%{request.CriteriaName}%") ||
        //            EF.Functions.Like(e.SalesOrderNo, $"%{request.CriteriaName}%") ||
        //            e.Invoices.Any(oi =>
        //                EF.Functions.Like(oi.Product.Name, $"%{request.CriteriaName}%") ||
        //                EF.Functions.Like(oi.Price.ToString(), $"%{request.CriteriaName}%")
        //            ) ||
        //            EF.Functions.Like(e.Invoices.FirstOrDefault().InvoiceNo, $"%{request.CriteriaName}%") ||
        //            EF.Functions.Like(e.DeliveryOrders.FirstOrDefault().DeliveryOrderNo, $"%{request.CriteriaName}%") ||
        //            EF.Functions.Like(e.DispatcheOrders.FirstOrDefault().DispatcheNo, $"%{request.CriteriaName}%")
        //        );
        //    //.Where(e => e.OrderItems.Any(oi => oi.IsFullyApproved) && e.OrderItems.All(oi => oi.StatusId == 2))
        //    //.AsQueryable();

        //    IQueryable<SalesOrder> salesOrderByDateRange = salesOrderByCriteria
        //        .Where(dt => dt.OrderedDate.Date >= request.DateFrom.Date && dt.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
        //        .OrderByDescending(or => or.OrderedDate).Where(so => salesOrderIdsWithStatus2.Contains(so.Id));


        //    int totalCount = await salesOrderByDateRange.CountAsync();
        //    int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        //    int skipCount = (request.PageNumber - 1) * request.PageSize;

        //    IEnumerable<SalesOrder> paginateSalesOrder = await salesOrderByDateRange
        //        .Skip(skipCount)
        //        .Take(request.PageSize)
        //        .ToListAsync();

        //    //response.SalesOrders = paginateSalesOrder.ConvertToPendingDeliveryOrderViews(1, true);
        //    response.TotalPages = totalPages;
        //    response.PageNumber = request.PageNumber;
        //    response.PageSize = request.PageSize;
        //    response.TotalCount = totalCount;

        //    await _unitOfWork.SaveAsync();

        //    return response;
        //}

    }
}
