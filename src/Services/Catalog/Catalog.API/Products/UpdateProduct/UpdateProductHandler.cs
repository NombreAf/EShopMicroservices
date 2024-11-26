using Catalog.API.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageUrl, decimal Price) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("Product ID is required");

            RuleFor(command => command.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(command => command.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(command => command.Category)
                .NotEmpty()
                .WithMessage("At least one category is required");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession session, UpdateProductCommandValidator validator) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
           

           
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

          
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(command.Id);
            }

           
            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageUrl = command.ImageUrl;
            product.Price = command.Price;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

           
            return new UpdateProductResult(true);
        }
    }
}
