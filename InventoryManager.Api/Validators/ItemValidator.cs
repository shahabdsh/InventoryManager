using FluentValidation;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;

namespace InventoryManager.Api.Validators
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator(IItemSchemaService itemSchemaService) {
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SchemaId).Must(id => itemSchemaService.GetOne(id) != null)
                .WithMessage(item => $"{nameof(ItemSchema)} with Id {item.SchemaId} does not exist.");
        }
    }
}
