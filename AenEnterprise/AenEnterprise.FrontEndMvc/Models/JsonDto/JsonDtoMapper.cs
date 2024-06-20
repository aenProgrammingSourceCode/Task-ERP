using AenEnterprise.FrontEndMvc.Models.JsonDto;
using AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AenEnterprise.FrontEndMvc.Models.JsonDto
{
    public static class JsonDtoMapper
    {
        public static IList<UpdateProductQuantityRequest> ConvertToProductRequest(this JsonUpdateProductQtyRequests jsonRequests)
        {
            return jsonRequests.Items.ConvertToProductRequests();
        }

        public static IList<UpdateProductQuantityRequest> ConvertToProductRequests(this JsonUpdateProductQtyRequest[] JsonRequests)
        {
            IList<UpdateProductQuantityRequest> productRequests = new List<UpdateProductQuantityRequest>();
            int i = 0;
            for (i = 0; i < JsonRequests.Length; i++)
            {
                productRequests.Add(JsonRequests[i].ConvertToProductRequest());
            }
            return productRequests;
        }

        public static UpdateProductQuantityRequest ConvertToProductRequest(this JsonUpdateProductQtyRequest request)
        {
            UpdateProductQuantityRequest updateProductQuantityRequest = new UpdateProductQuantityRequest();
            updateProductQuantityRequest.ProductId = request.ProductId;
            updateProductQuantityRequest.NewQuantity = request.Qty;

            return updateProductQuantityRequest;
        }
    }

}

