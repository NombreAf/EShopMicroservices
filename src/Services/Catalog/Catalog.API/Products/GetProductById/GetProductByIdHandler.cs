
using Catalog.API.Exceptions;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductBydQuery(Guid Id) : IQuery<GetProductsByIdResult>;
    public record GetProductsByIdResult(Product Product);
    internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductBydQuery> logger) : IQueryHandler<GetProductBydQuery, GetProductsByIdResult>
    {
        public async Task<GetProductsByIdResult> Handle(GetProductBydQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByIdHandler.Handle called with {@Query}", query);

            var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

            if (product == null) {
                throw new ProductNotFoundException(query.Id);
            }

            return new GetProductsByIdResult(product);  
        }

    }
}
