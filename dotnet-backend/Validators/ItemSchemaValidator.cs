using System.Linq;
using FluentValidation;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;

namespace InventoryManager.Api.Validators
{
    public class ItemSchemaValidator : AbstractValidator<ItemSchema>
    {
        public ItemSchemaValidator(IItemSchemaService itemSchemaService) {
            RuleFor(x => x.Name).NotEmpty()
                .Must((schema,name) =>
                    !itemSchemaService.Queryable()
                        .Any(s => s.Id != schema.Id && s.Name == name))
                .WithMessage(schema => $"Item schema name must be unique.");
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
