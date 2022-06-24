using Existence.Grpc.Protos;
using Grpc.Core;

namespace Existence.Grpc.Services
{
    public class ProductService : ExistenceService.ExistenceServiceBase
    {

        public override async Task<ProductExistenceReply> CheckExistence(ProductRequest request, ServerCallContext context)
        {
            Console.WriteLine("Se realizo peticion");
            return new ProductExistenceReply { ProductQty = 89 };
        }
    }
}
