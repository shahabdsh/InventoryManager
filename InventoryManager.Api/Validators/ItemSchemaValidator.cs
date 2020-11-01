using System.Linq;
using FluentValidation;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Validators
{
    public class ItemSchemaValidator : AbstractValidator<ItemSchema>
    {
        public ItemSchemaValidator() {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Properties).Must(list =>
                    list.Select(x => x.Name).Distinct().Count() == list.Count)
                .WithMessage(item => $"Item schema can not contain duplicate fields");
            RuleForEach(x => x.Properties).ChildRules(properties =>
            {
                properties.RuleFor(x => x.Type).IsInEnum();
                properties.RuleFor(x => x.Name).NotEmpty();
            });
        }
    }
}
