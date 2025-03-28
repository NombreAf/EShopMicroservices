﻿using Catalog.API.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Marten;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
        }
    }

    internal class DeleteProductCommandHandler
        (IDocumentSession session, DeleteProductCommandValidator validator)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
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

            
            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

         
            return new DeleteProductResult(true);
        }
    }
}
