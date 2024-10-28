
namespace Catalog.API.Products
{
    public record CreateProductCommand(
        string Name,
        string Description,
        List<string> Category,
        string ImageUrl,
        decimal Price
    ) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,  
                Category = command.Category,
                ImageUrl = command.ImageUrl,
                Price = command.Price,
            };

            session.Store( product );
            await session.SaveChangesAsync(cancellationToken);   


            return new CreateProductResult(product.Id);
        }
    }
}
