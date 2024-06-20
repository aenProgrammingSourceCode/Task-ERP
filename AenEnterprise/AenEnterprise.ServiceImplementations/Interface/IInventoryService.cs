using AenEnterprise.ServiceImplementations.Messaging.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Interface
{
    public interface IInventoryService
    {
       Task<GetAllProductReponse> GetAllProducts();
       Task<GetAllUnitResponse> GetAllUnits();
        Task<CreatePurchaseOrderResponse> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request);
    }
}
