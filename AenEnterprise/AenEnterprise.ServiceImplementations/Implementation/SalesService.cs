using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel.CookieStorage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AenEnterprise.ServiceImplementations.Interface;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DispatchOrder;
using AenEnterprise.ServiceImplementations.Messaging.InventoryManagement;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel;
using AenEnterprise.ServiceImplementations.Mapping;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.AspNetCore.Mvc;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DeliveryOrderMessage;
 
using AenEnterprise.ServiceImplementations.ViewModel;
using AutoMapper;
using AenEnterprise.ServiceImplementations.Mapping.Automappers;

namespace AenEnterprise.ServiceImplementations.Implementation
{
    public class SalesService:ISalesOrderService
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
        private IInvoiceItemRepository _invoiceItemRepository;
        private ICustomerRepository _customerRepository;
        private IDeliveryOrderRepository _deliveryOrderRepository;
        private IDeliveryOrderItemRepository _deliveryItemOrderRepository;
        private IDispatcheRepository _dispatcheRepository;
        private ICookieImplementation _cookieImplementation;
        private IMapper _mapper;
        public SalesService(ISalesOrderRepository salesOrderRepository,
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
        ICookieImplementation cookieImplementation,
        IInvoiceItemRepository invoiceItemRepository,
        IDeliveryOrderItemRepository deliveryItemOrderRepository,
        IMapper mapper)
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
            _invoiceItemRepository = invoiceItemRepository;
            _customerRepository = customerRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _dispatcheRepository = dispatcheRepository;
            _deliveryItemOrderRepository = deliveryItemOrderRepository;
            _mapper = mapper;
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
                    orderItem.IsPartiallyApproved = true;
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
                .OrderByDescending(or => or.OrderedDate).ThenByDescending(or => or.Id).Where(so => salesOrderIdsWithStatus1.Contains(so.Id));

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

        public async Task<GetAllSalesOrderResponse> GetAllApprovedOrderItems(SalesOrderSearchCriteriaRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();

            var salesOrderIdsWithStatus1 = await _context.SalesOrders
                .Where(so => so.OrderItems.Any(oi => oi.StatusId == 2 && oi.IsPartiallyApproved == true))
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


        public async Task<CreateInvoiceResponse> CreateCustomInvoiceAsync(CreateInvoiceRequest request)
        {
            try
            {
                CreateInvoiceResponse response = new CreateInvoiceResponse();
                SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
                OrderItem orderItem = await _orderItemRepository.GetByIdAsync(request.OrderItemId);
                var lastOrderItem = salesOrder.LastOrderItem(request.OrderItemId);

                if (lastOrderItem != null)
                {
                    string invoiceId = _request.HttpContext.Session.GetString("InvoiceId");

                    if (invoiceId != null)
                    {
                        if (int.TryParse(invoiceId, out int parseInvoiceId))
                        {
                            Invoice invoice = await _invoiceRepository.GetByIdAsync(parseInvoiceId);
                            if (invoice != null)
                            {
                                invoice.CreateInvoiceItem(orderItem, request.InvoiceQuantity, 1);
                                await _invoiceRepository.UpdateAsync(invoice);
                                response.Invoice = invoice.ConvertToInvoiceView(1, true);
                            }
                        }
                    }
                    else
                    {
                        var invoices = await _invoiceRepository.FindAllAsync();
                        int LastInvoiceId = invoices.Any() ? invoices.Last().Id : 0;
                        Invoice newInvoice = new Invoice();
                        newInvoice.SalesOrderId = request.SalesOrderId;
                        if (LastInvoiceId > 0)
                        {
                            newInvoice.InvoiceNo = "INV-" + (LastInvoiceId + 1).ToString();
                        }
                        else
                        {
                            newInvoice.InvoiceNo = "INV-1";
                        }

                        newInvoice.CreateInvoiceItem(orderItem, request.InvoiceQuantity, 1);
                        await _invoiceRepository.AddAsync(newInvoice);

                        response.Invoice = newInvoice.ConvertToInvoiceView(1, true);

                        _request.HttpContext.Session.SetString("InvoiceId", response.Invoice.Id.ToString());
                    }

                    await AddOrderItem(request.orderItemIdToAdd, salesOrder, request.InvoiceQuantity);
                    await _salesOrderRepository.UpdateAsync(salesOrder);
                    await _unitOfWork.SaveAsync();
                }
                 
                return response;
            }
            catch (Exception ex)
            {
                return new CreateInvoiceResponse
                {
                    ErrorMessage = "An error occurred while processing the invoice."
                };
            }
        }
         

        public async Task<CreateDeliveryOrderResponse> CreateCustomDeliveryOrderAsync(CreateDeliveryOrderRequest request)
        {
            try
            {
                CreateDeliveryOrderResponse response = new CreateDeliveryOrderResponse();
                Invoice invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
                var orderItem = await _orderItemRepository.GetByIdAsync(request.OrderItemId);
                string DeliveryOrderId = _request.HttpContext.Session.GetString("DeliveryOrderId");

                if (DeliveryOrderId != null)
                {
                    if (int.TryParse(DeliveryOrderId, out int parseDeliveryId))
                    {
                        DeliveryOrder deliveryOrder = await _deliveryOrderRepository.GetByIdAsync(parseDeliveryId);
                        if (invoice != null)
                        {
                            deliveryOrder.CreateDeliveryOrderItem(orderItem,request.DeliveryQuantity);
                            await _deliveryOrderRepository.UpdateAsync(deliveryOrder);
                            response.DeliveryOrder = deliveryOrder.ConvertToDeliveryOrderView(1, true);
                        }
                    }
                }
                else
                {
                    var deliveries = await _deliveryOrderRepository.FindAllAsync();
                    int LastDOId = deliveries.Any() ? deliveries.Last().Id : 0;
                    DeliveryOrder newDeliveryOrder = new DeliveryOrder();
                    newDeliveryOrder.InvoiceId = request.InvoiceId;
                    
                    newDeliveryOrder.SalesOrderId = invoice.SalesOrderId;

                    if (LastDOId > 0)
                    {
                        newDeliveryOrder.DeliveryOrderNo = "DO-" + (LastDOId + 1).ToString();
                    }
                    else
                    {
                        newDeliveryOrder.DeliveryOrderNo = "DO-1";
                    }
                   
                    newDeliveryOrder.CreateDeliveryOrderItem(orderItem, request.DeliveryQuantity);
                    await _deliveryOrderRepository.AddAsync(newDeliveryOrder); // Add newDeliveryOrder to the repository
                    response.DeliveryOrder = newDeliveryOrder.ConvertToDeliveryOrderView(1, true);
                    _request.HttpContext.Session.SetString("DeliveryOrderId", response.DeliveryOrder.Id.ToString());
                }

                await AddInvoiceItem(request.invoiceItemIdToAdd, invoice, request.DeliveryQuantity);
                await _invoiceRepository.UpdateAsync(invoice);
                await _unitOfWork.SaveAsync(); // Save changes to the database
                return response;
            }
            catch (Exception)
            {
                throw;
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

        private async Task AddInvoiceItem(IList<int> invoiceItemIdToAdd, Invoice invoice, decimal quantity)
        {
            foreach (int invoiceItemId in invoiceItemIdToAdd)
            {
                InvoiceItem invoiceItem = await _invoiceItemRepository.GetByIdAsync(invoiceItemId);
                if (invoiceItem != null)
                {
                    invoice.setInvoiceItem(invoiceItem, quantity);
                    await _invoiceItemRepository.UpdateAsync(invoiceItem);
                }
                else
                {
                    // Handle the case where the order item was not found (e.g., log an error or throw an exception).
                    // Depending on your application's requirements, you may choose to skip this order item or take other actions.
                }
            }
        }

        private async Task AddDeliveryItem(IList<int> deliveryItemIdToAdd, DeliveryOrder deliveryOrder, decimal quantity)
        {
            foreach (int deliveryItemId in deliveryItemIdToAdd)
            {
                DeliveryOrderItem deliveryItem = await _deliveryItemOrderRepository.GetByIdAsync(deliveryItemId);
                if (deliveryItem != null)
                {
                    deliveryOrder.setDeliveryOrderItem(deliveryItem, quantity);
                    await _deliveryItemOrderRepository.UpdateAsync(deliveryItem);
                }
                else
                {
                    
                }
            }
        }

        public async Task<GetAllInvoiceResponse> GetAllUnApprovedInvoice(SalesOrderSearchCriteriaRequest request)
        {
            GetAllInvoiceResponse response = new GetAllInvoiceResponse();

            var invoiceIdsWithStatus1 = await _context.Invoices
                .Where(so => so.InvoiceItems.Any(inv => inv.StatusId == 1 && inv.IsActive==true))
                .Select(so => so.Id)
                .ToListAsync();

            IQueryable<Invoice> salesOrderByCriteria = _context.Invoices
                .Where(e =>
                    EF.Functions.Like(e.SalesOrder.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrder.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.InvoiceItems.Any(oi =>
                        EF.Functions.Like(oi.OrderItem.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.OrderItem.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                    EF.Functions.Like(e.InvoiceNo, $"%{request.CriteriaName}%") 
                );

            IQueryable<Invoice> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.SalesOrder.OrderedDate.Date >= request.DateFrom.Date && dt.SalesOrder.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.SalesOrder.OrderedDate)
                .Where(so => invoiceIdsWithStatus1.Contains(so.Id));

            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<Invoice> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.Invoices = paginateSalesOrder.ConvertToInvoiceViews(1, true);
            response.TotalPages = totalPages;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            response.TotalCount = totalCount;

            await _unitOfWork.SaveAsync();

            return response;
        }

        public async Task<GetAllInvoiceResponse> GetAllApprovedInvoice(SalesOrderSearchCriteriaRequest request)
        {
            GetAllInvoiceResponse response = new GetAllInvoiceResponse();

            var invoiceIdsWithStatus2 = await _context.Invoices
                .Where(so => so.InvoiceItems.Any(inv => inv.StatusId == 2 && inv.IsActive == true))
                .Select(so => so.Id)
                .ToListAsync();

            IQueryable<Invoice> salesOrderByCriteria = _context.Invoices
                .Where(e =>
                    EF.Functions.Like(e.SalesOrder.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.SalesOrder.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.InvoiceItems.Any(oi =>
                        EF.Functions.Like(oi.OrderItem.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.OrderItem.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                    EF.Functions.Like(e.InvoiceNo, $"%{request.CriteriaName}%")
                );

            IQueryable<Invoice> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.SalesOrder.OrderedDate.Date >= request.DateFrom.Date && dt.SalesOrder.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.SalesOrder.OrderedDate)
                .Where(so => invoiceIdsWithStatus2.Contains(so.Id));

            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<Invoice> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.Invoices = paginateSalesOrder.ConvertToInvoiceViews(2, true);
            response.TotalPages = totalPages;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            response.TotalCount = totalCount;

            await _unitOfWork.SaveAsync();

            return response;
        }

        public async Task<GetAllDeliveryOrderResponse> GetAllApproveDeliveryOrder(SalesOrderSearchCriteriaRequest request)
        {
            GetAllDeliveryOrderResponse response = new GetAllDeliveryOrderResponse();

            var deliveryIdsWithStatus2 = await _context.DeliveryOrders
                .Where(so => so.DeliveryOrderItem.Any(inv => inv.StatusId == 2 && inv.IsActive == true))
                .Select(so => so.Id)
                .ToListAsync();

            IQueryable<DeliveryOrder> salesOrderByCriteria = _context.DeliveryOrders
                .Include(d => d.SalesOrder)
        .ThenInclude(s => s.Customer)
    .Include(d => d.DeliveryOrderItem)
    .ThenInclude(doi => doi.OrderItem)  // Ensure OrderItem is loaded
            .ThenInclude(oi => oi.Product)
                .Where(e =>
                    EF.Functions.Like(e.Invoice.SalesOrder.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.Invoice.SalesOrder.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.Invoice.InvoiceItems.Any(oi =>
                        EF.Functions.Like(oi.OrderItem.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.OrderItem.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                    EF.Functions.Like(e.Invoice.InvoiceNo, $"%{request.CriteriaName}%")
                );

            IQueryable<DeliveryOrder> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.SalesOrder.OrderedDate.Date >= request.DateFrom.Date && dt.SalesOrder.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.SalesOrder.OrderedDate)
                .Where(so => deliveryIdsWithStatus2.Contains(so.Id));

            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<DeliveryOrder> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.DeliveryOrders = paginateSalesOrder.ConvertToDeliveryOrderViews(2, true);
            response.TotalPages = totalPages;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            response.TotalCount = totalCount;

            await _unitOfWork.SaveAsync();

            return response;
        }

        public async Task<GetAllDeliveryOrderResponse> GetAllUnApproveDeliveryOrder(SalesOrderSearchCriteriaRequest request)
        {
            GetAllDeliveryOrderResponse response = new GetAllDeliveryOrderResponse();

            var deliveryIdsWithStatus1 = await _context.DeliveryOrders
                .Where(so => so.DeliveryOrderItem.Any(inv => inv.StatusId == 1 && inv.IsActive == true))
                .Select(so => so.Id)
                .ToListAsync();

            IQueryable<DeliveryOrder> salesOrderByCriteria = _context.DeliveryOrders
                .Include(d => d.SalesOrder)
        .ThenInclude(s => s.Customer)
    .Include(d => d.DeliveryOrderItem)
    .ThenInclude(doi => doi.OrderItem)  // Ensure OrderItem is loaded
            .ThenInclude(oi => oi.Product)
                .Where(e =>
                    EF.Functions.Like(e.Invoice.SalesOrder.Customer.Name, $"%{request.CriteriaName}%") ||
                    EF.Functions.Like(e.Invoice.SalesOrder.SalesOrderNo, $"%{request.CriteriaName}%") ||
                    e.Invoice.InvoiceItems.Any(oi =>
                        EF.Functions.Like(oi.OrderItem.Product.Name, $"%{request.CriteriaName}%") ||
                        EF.Functions.Like(oi.OrderItem.Price.ToString(), $"%{request.CriteriaName}%")
                    ) ||
                    EF.Functions.Like(e.Invoice.InvoiceNo, $"%{request.CriteriaName}%")
                );

            IQueryable<DeliveryOrder> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.SalesOrder.OrderedDate.Date >= request.DateFrom.Date && dt.SalesOrder.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.SalesOrder.OrderedDate)
                .Where(so =>deliveryIdsWithStatus1.Contains(so.Id));

            int totalCount = await salesOrderByDateRange.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            IEnumerable<DeliveryOrder> paginateSalesOrder = await salesOrderByDateRange
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync();

            response.DeliveryOrders = paginateSalesOrder.ConvertToDeliveryOrderViews(1,true);
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
            SalesOrder salesOrder = await _salesOrderRepository.GetByIdAsync(salesOrderId); // Adjust this method to load SalesOrder without Invoices

            // Check if the SalesOrder entity is retrieved
            if (salesOrder != null)
            {
                response.SalesOrder = salesOrder.ConvertToSalesOrderView(2, true);
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
                .GetByIdAsync(invoiceId); 

            if (invoice != null)
            {
                response.Invoice = invoice.ConvertToInvoiceView(2, true);
            }
            else
            {
                // Handle scenario where SalesOrder couldn't be loaded
                // For instance, log an error or set response.SalesOrder to null
                response.Invoice = null;
            }

            return response;
        }

        public async Task<CreateDispatchResponse> CreateCustomDispatchAsync(CreateDispatchRequest request)
        {
            try
            {
                CreateDispatchResponse response = new CreateDispatchResponse();
                DeliveryOrder deliveryOrder = await _deliveryOrderRepository.GetByIdAsync(request.DeliveryOrderId);
                OrderItem orderItem = await _orderItemRepository.GetByIdAsync(request.OrderItemId);

                string DispatchId = _request.HttpContext.Session.GetString("DispatchId");

                if (DispatchId!= null)
                {
                    if (int.TryParse(DispatchId, out int parseDispatchId))
                    {
                        DispatcheOrder dispatchOrder = await _dispatcheRepository.GetByIdAsync(parseDispatchId);
                        if (dispatchOrder != null)
                        {
                            dispatchOrder.CreateDispatchItem(orderItem, request.VehicalNo, request.VehicalEmptyWeight, request.VehicalLoadWeight, request.DispatchQuantity, 1);
                            await _dispatcheRepository.UpdateAsync(dispatchOrder);
                            response.DispatcheOrder =dispatchOrder.ConvertToDispatchView(1,true);
                        }
                    }
                }
                else
                {
                    var dispatchs = await _dispatcheRepository.FindAllAsync();
                    int LastDispatchId = dispatchs.Any() ? dispatchs.Last().Id : 0;
                    DispatcheOrder newDispatch = new DispatcheOrder();
                    newDispatch.DeliveryOrderId = deliveryOrder.Id;
                    newDispatch.InvoiceId = deliveryOrder.InvoiceId;
                    newDispatch.SalesOrderId = deliveryOrder.SalesOrderId;

                    if (LastDispatchId > 0)
                    {
                        newDispatch.DispatcheNo = "GP-" + (LastDispatchId + 1).ToString();
                    }
                    else
                    {
                       newDispatch.DispatcheNo = "GP-1";
                    }

                    newDispatch.CreateDispatchItem(orderItem, request.VehicalNo,request.VehicalEmptyWeight,request.VehicalLoadWeight, request.DispatchQuantity,1);
                    await _dispatcheRepository.AddAsync(newDispatch);
                    response.DispatcheOrder =newDispatch.ConvertToDispatchView(1,true);
                    _request.HttpContext.Session.SetString("DispatchId", response.DispatcheOrder.Id.ToString());
                }
                
                await AddDeliveryItem(request.deliveryItemToAdd,deliveryOrder, request.DispatchQuantity);
                await _deliveryOrderRepository.UpdateAsync(deliveryOrder);
                await _unitOfWork.SaveAsync();
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GetDeliveryOrderResponse> GetApprovedDeliveryOrderDetailsAsync(int deliveryOrderId)
        {
            GetDeliveryOrderResponse response = new GetDeliveryOrderResponse();
            DeliveryOrder deliveryOrder = await _deliveryOrderRepository
                .GetByIdAsync(deliveryOrderId); // Adjust this method to load SalesOrder without Invoices

            // Check if the SalesOrder entity is retrieved
            if (deliveryOrderId!=null)
            {
                response.DeliveryOrder =deliveryOrder.ConvertToDeliveryOrderView(2, true);
            }
            else
            {
                // Handle scenario where SalesOrder couldn't be loaded
                // For instance, log an error or set response.SalesOrder to null
                response.DeliveryOrder=null;
            }

            return response;
        }

        public async Task<GetAllInvoiceResponse> ApprovalStatusInvoiceAsync(UpdateSalesOrderApprovalStatusRequest request)
        {
            GetAllInvoiceResponse response = new GetAllInvoiceResponse();
            Invoice invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            InvoiceItem invoiceItem = await _invoiceItemRepository.GetByIdAsync(request.InvoiceItemId);

            try
            {
                if (invoice != null)
                {
                    invoiceItem.StatusId = request.StatusId;
                    await _invoiceItemRepository.UpdateAsync(invoiceItem);
                    await _invoiceRepository.UpdateAsync(invoice);

                    response.Invoice = invoice.ConvertToInvoiceView(1,true);
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
             
            await _unitOfWork.SaveAsync();
            return response;
        }



        public async Task<GetAllDeliveryOrderResponse> ApprovalStatusDeliveryOrderAsync(UpdateSalesOrderApprovalStatusRequest request)
        {
            GetAllDeliveryOrderResponse response = new GetAllDeliveryOrderResponse();
            IEnumerable<DeliveryOrder> deliveryOrders =await _deliveryOrderRepository.FindAllAsync();
            var deliveryOrderItem = await _deliveryItemOrderRepository.GetByIdAsync(request.DeliveryOrderItemId);

            try
            {
                if (deliveryOrderItem!= null)
                {
                    deliveryOrderItem.StatusId = request.StatusId;
                    await _deliveryItemOrderRepository.UpdateAsync(deliveryOrderItem);
                    await _unitOfWork.SaveAsync();

                    response.DeliveryOrders =deliveryOrders.ConvertToDeliveryOrderViews(1, true);
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

        public Task<GetSalesOrderResponse> GetSalesOrderById(int salesOrderId)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllProductReponse> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<GetSalesOrderResponse> UpdateSalesOrder(UpdateSalesOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task CreateInvoice(int salesOrderId)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllSalesOrderResponse> UpdateInvoiceApprovalStatusAsync(UpdateSalesOrderApprovalStatusRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllSalesOrderResponse> GetAllSalesOrdersBasket()
        {
            throw new NotImplementedException();
        }

       
        public Task CreateBankAccount()
        {
            throw new NotImplementedException();
        }

        public Task<GetSalesOrderResponse> DeleteOrderItemAsync(DeleteOrderItemRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<GetSalesOrderResponse> DeleteSalesOrderAsync(DeleteOrderItemRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSalesOrderAndOrderItemAsync(int salesOrderId)
        {
            throw new NotImplementedException();
        }

        public Task<GetSalesOrderResponse> DeleteSalesOrderBasedOnOrderLimitAsync(DeleteOrderItemRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<GetAllSalesOrderResponse> GetAllApprovedOrderItemsSummary(SalesOrderSearchCriteriaRequest request)
        {
            GetAllSalesOrderResponse response = new GetAllSalesOrderResponse();
            IQueryable<SalesOrder> salesOrderByCriteria = _context.SalesOrders;
            IQueryable<Invoice> invoiceByCriteria = _context.Invoices;
            IQueryable<DispatcheOrder> dispatchByCriteria = _context.DispatchOrders;

            IQueryable<SalesOrder> salesOrderByDateRange = salesOrderByCriteria
                .Where(dt => dt.OrderedDate.Date >= request.DateFrom.Date && dt.OrderedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.OrderedDate);
            IQueryable<Invoice> invoiceByDateRange = invoiceByCriteria
                .Where(dt => dt.CreatedDate.Date >= request.DateFrom.Date && dt.CreatedDate.Date <= request.DateTo.Date.AddDays(1))
                .OrderByDescending(or => or.CreatedDate);
            IQueryable<DispatcheOrder> dispatchByDateRange = dispatchByCriteria
               .Where(dt => dt.CreatedDate.Date >= request.DateFrom.Date && dt.CreatedDate.Date <= request.DateTo.Date.AddDays(1))
               .OrderByDescending(or => or.CreatedDate);

            var flattenedSalesOrders = await salesOrderByDateRange
                .Select(so => new
                {
                    salesOrderQuantity = so.OrderItems.Where(oi => oi.StatusId == 2 || oi.StatusId==3).Sum(oi => oi.Quantity),
                    salesOrderAmount = so.OrderItems.Where(oi=>oi.StatusId==2 || oi.StatusId == 3).Sum(oi => oi.NetAmount),
                })
                .ToListAsync();

            var flattenedInvoices = await invoiceByDateRange
               .Select(so => new
               {
                   invoiceQuantity = so.InvoiceItems.Where(iv => iv.StatusId == 2 || iv.StatusId == 3).Sum(iv => iv.InvoiceQuantity),
                   invoiceAmount = so.InvoiceItems.Where(iv => iv.StatusId == 2 || iv.StatusId == 3).Sum(iv => iv.InvoiceAmount),
               })
               .ToListAsync();

            var flattenedDispatch = await dispatchByDateRange
               .Select(so => new
               {
                   dispatchQuantity = so.DispatchItems.Where(dp => dp.StatusId == 2 || dp.StatusId == 3).Sum(dp => dp.DispatchQuantity),
                   dispatchAmount = so.DispatchItems.Where(dp => dp.StatusId == 2 || dp.StatusId == 3).Sum(dp => dp.DispatchAmount),
               })
               .ToListAsync();

            // Calculating total quantity and total amount from the flattened query
            decimal totalSalesOrderQuantity = flattenedSalesOrders.Sum(so => so.salesOrderQuantity);
            decimal totalSalesOrderAmount = flattenedSalesOrders.Sum(so => so.salesOrderAmount);

            decimal totalInvoiceQuantity =flattenedInvoices.Sum(iv => iv.invoiceQuantity);
            decimal totalInvoiceAmount = flattenedInvoices.Sum(iv => iv.invoiceAmount);

            decimal totalDispatchQuantity = flattenedDispatch.Sum(dp => dp.dispatchQuantity);
            decimal totalDispatchAmount = flattenedDispatch.Sum(dp => dp.dispatchAmount);


            response.SalesOrders = salesOrderByDateRange.ConvertToSalesOrderViews(2, true);
            //response.TotalCount = totalCount;
            response.TotalSalesOrderQuantity = totalSalesOrderQuantity;
            response.TotalSalesOrderAmount = totalSalesOrderAmount;

            response.TotalInvoiceAmount = totalInvoiceAmount;
            response.TotalInvoiceQuantity = totalInvoiceQuantity;

            response.TotalDispatchAmount =totalDispatchAmount;
            response.TotalDispatchQuantity =totalDispatchQuantity;

            await _unitOfWork.SaveAsync();

            return response;
        }
    }
}
