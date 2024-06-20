using AenEnterprise.DataAccess.Repository;
using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.CookieStorage;
using AenEnterprise.DomainModel.PurchaseManagement;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.Interface;
using AenEnterprise.ServiceImplementations.Mapping;
using AenEnterprise.ServiceImplementations.Messaging.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Implementation
{
    public class InventoryService:IInventoryService
    {
        private readonly IProductRepository _productRepository;

        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseItemRepository _purchaseItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;
        private ICookieImplementation _cookieImplementation;
        public InventoryService(IProductRepository productRepository,
            IUnitRepository unitRepository,
            IUnitOfWork unitOfWork,
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseItemRepository purchaseItemRepository,
            ICookieImplementation cookieImplementation)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _unitRepository = unitRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseItemRepository = purchaseItemRepository;   
            _cookieImplementation = cookieImplementation;

        }

        public async Task<CreatePurchaseOrderResponse> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request)
        {
            try
            {
                string salesOrderId =_cookieImplementation.Get(CookieDataKey.PurchaseOrderId.ToString());
                CreatePurchaseOrderResponse response = new CreatePurchaseOrderResponse();
                Product product = await _productRepository.GetByIdAsync(request.ProductId);
                Unit unit = await _unitRepository.GetByIdAsync(request.UnitId);
                var purchaseOrders = await _purchaseOrderRepository.FindAllAsync();
                PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
                int LastPurchaseOrderId = purchaseOrders.Any() ? purchaseOrders.Last().Id : 0;
                if (purchaseOrder != null)
                {
                    if (await PurchaseItemExistsWithSameProduct(purchaseOrder, product, request.Price))
                    {
                        throw new InvalidOperationException("Selected Product already exists in current Purchase Order");
                    }
                    else
                    {
                        purchaseOrder.CreatePurchaseItem(purchaseOrder, product, request.Quantity, unit, request.Price, request.DiscountAmount, request.DiscountPercent);
                        await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
                    }
                }

                else
                {
                    PurchaseOrder newPurchaseOrder = new PurchaseOrder();
                    newPurchaseOrder.PurchaseDate = request.PurchaseDate;
                    newPurchaseOrder.PurchaseOrderNo = request.PurchaseOrderNo;
                    newPurchaseOrder.SupplierId = request.SupplierId;
                    if (LastPurchaseOrderId > 0)
                    {
                       newPurchaseOrder.PurchaseOrderNo = "PO-" + (LastPurchaseOrderId + 1).ToString();
                    }
                    else
                    {
                       newPurchaseOrder.PurchaseOrderNo = "PO-" + 1.ToString();
                    }
                    newPurchaseOrder.CreatePurchaseItem(newPurchaseOrder, product, request.Quantity, unit, request.Price, request.DiscountAmount, request.DiscountPercent);
                    await _purchaseOrderRepository.AddAsync(newPurchaseOrder);
                    _cookieImplementation.Set(CookieDataKey.PurchaseOrderId.ToString(), response.PurchaseOrder.Id.ToString(), 1);
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task<bool> PurchaseItemExistsWithSameProduct(PurchaseOrder purchaseOrder, Product product, decimal price)
        {
            var orderItems = await _purchaseItemRepository.FindAsync(i => i.PurchaseOrderId == purchaseOrder.Id && i.Product == product && i.Price == price);
            
            // Check if any order item with the same product exists in the sales order
            return orderItems.Any();
        }

        public async Task<GetAllProductReponse> GetAllProducts()
        {
            GetAllProductReponse response=new GetAllProductReponse();
            IEnumerable<Product> products =await _productRepository.FindAllAsync();
            response.Products = products.ConvertToProductViews();
            return response;
        }

        public async Task<GetAllUnitResponse> GetAllUnits()
        {
            GetAllUnitResponse response=new GetAllUnitResponse();
            IEnumerable<Unit> units =await _unitRepository.FindAllAsync();
            response.Units = units.ConvertToUnitViews();
            return response;
        }
    }
}
