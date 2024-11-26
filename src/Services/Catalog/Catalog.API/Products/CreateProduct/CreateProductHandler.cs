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

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");


            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("At least one category is required")
                .Must(c => c.Count <= 5).WithMessage("A maximum of 5 categories is allowed");


            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Invalid image URL format");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .PrecisionScale(18, 2, false)
                .WithMessage("Price must have a maximum of 18 digits in total, with up to 2 decimals");
        }
    }

    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IDocumentSession _session;
        private readonly IValidator<CreateProductCommand> _validator;

        public CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Category = command.Category,
                ImageUrl = command.ImageUrl,
                Price = command.Price,
            };

            try
            {
                _session.Store(product);
                await _session.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save product", ex);
            }

            return new CreateProductResult(product.Id);
        }
    }
}