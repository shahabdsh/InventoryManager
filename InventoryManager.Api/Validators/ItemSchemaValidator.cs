using FluentValidation;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;

namespace InventoryManager.Api.Validators
{
    public class ItemSchemaValidator : AbstractValidator<ItemSchema>
    {
        public ItemSchemaValidator() {
        }
    }
}
