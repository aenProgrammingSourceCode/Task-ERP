using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.Messaging.InventoryManagement;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DeliveryOrderMessage;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DispatchOrder;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging;
using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Interface
{
    public interface ISalesOrderService
    {
        Task<GetSalesOrderResponse> CreateSalesOrderAsync(CreateSalesOrderRequest request);
        Task AddSalesOrdersAsync(int salesOrderId);
        Task<GetAllSalesOrderResponse> ApprovalStatusOrderItemsAsync(UpdateSalesOrderApprovalStatusRequest request);
        Task<GetAllSalesOrderResponse> GetAllUnApprovedOrderItems(SalesOrderSearchCriteriaRequest request);
        Task<GetAllSalesOrderResponse> GetAllApprovedOrderItems(SalesOrderSearchCriteriaRequest request);
        Task<GetAllSalesOrderResponse> GetAllApprovedOrderItemsSummary(SalesOrderSearchCriteriaRequest request);
        //Task<GetAllSalesOrderResponse> GetAllUnApprovedInvoice(SalesOrderSearchCriteriaRequest request);
        Task<GetAllInvoiceResponse> GetAllUnApprovedInvoice(SalesOrderSearchCriteriaRequest request);
        Task<GetAllInvoiceResponse> GetAllApprovedInvoice(SalesOrderSearchCriteriaRequest request);
        Task<GetAllDeliveryOrderResponse> GetAllApproveDeliveryOrder(SalesOrderSearchCriteriaRequest request);
        Task<GetAllDeliveryOrderResponse> GetAllUnApproveDeliveryOrder(SalesOrderSearchCriteriaRequest request);
        Task<GetSalesOrderResponse> GetApprovedOrderItemDetailsAsync(int salesOrderId);
        Task<GetInvoiceResponse> GetApprovedInvoiceDetailsAsync(int invoiceId);
        Task<CreateDispatchResponse> CreateCustomDispatchAsync(CreateDispatchRequest request);
        Task<GetDeliveryOrderResponse> GetApprovedDeliveryOrderDetailsAsync(int deliveryOrderId);
        
        Task<GetAllInvoiceResponse> ApprovalStatusInvoiceAsync(UpdateSalesOrderApprovalStatusRequest request);
        Task<GetAllDeliveryOrderResponse> ApprovalStatusDeliveryOrderAsync(UpdateSalesOrderApprovalStatusRequest request);
        Task<GetSalesOrderResponse> GetSalesOrderById(int salesOrderId);
        Task<GetAllProductReponse> GetAllProducts();
        Task<GetSalesOrderResponse> UpdateSalesOrder(UpdateSalesOrderRequest request);
        Task CreateInvoice(int salesOrderId);
        Task<CreateInvoiceResponse> CreateCustomInvoiceAsync(CreateInvoiceRequest request);
        Task<GetAllSalesOrderResponse> UpdateInvoiceApprovalStatusAsync(UpdateSalesOrderApprovalStatusRequest request);
        Task<GetAllSalesOrderResponse> GetAllSalesOrdersBasket();
        Task<CreateDeliveryOrderResponse> CreateCustomDeliveryOrderAsync(CreateDeliveryOrderRequest request);
        Task CreateBankAccount();
        Task<GetSalesOrderResponse> DeleteOrderItemAsync(DeleteOrderItemRequest request);
        Task<GetSalesOrderResponse> DeleteSalesOrderAsync(DeleteOrderItemRequest request);
        Task DeleteSalesOrderAndOrderItemAsync(int salesOrderId);
        Task<GetSalesOrderResponse> DeleteSalesOrderBasedOnOrderLimitAsync(DeleteOrderItemRequest request);

    }
}
