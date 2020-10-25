using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManager.Api.Services
{
    public class ItemService : RepositoryService<Item>, IItemService
    {
        protected override string EntityCollectionName => "Items";

        public ItemService(IOptions<InventoryDatabaseSettings> settings) : base(settings)
        {
        }

        protected override IFindFluent<Item, Item> SimpleQueryFilter(string query)
        {
            var regex = new Regex($@"{query.Trim()}", RegexOptions.IgnoreCase);

            var builder = Builders<Item>.Filter;
            var filter = builder.Regex($"{nameof(Item.Name)}", regex) |
                         builder.Regex($"{nameof(Item.Properties)}.{nameof(ItemProperty.Value)}", regex);

            return Entities.Find(filter);
        }

        protected override IFindFluent<Item, Item> AdvancedQueryFilter(string query, List<BasicFilterDefinition> filterDefs)
        {
            // Todo: User can search by item type
            // Todo: Needs refactoring

            var builder = Builders<Item>.Filter;
            var propertyBuilder = Builders<ItemProperty>.Filter;

            var filter = builder.Empty;

            foreach (var filterDef in filterDefs)
            {
                var fieldRegex = new Regex($@"{filterDef.Field.Trim()}", RegexOptions.IgnoreCase);

                if (filterDef.Operation == BasicFilterOperation.Ctn ||
                    filterDef.Operation == BasicFilterOperation.Eq)
                {
                    var valueRegex = filterDef.Operation == BasicFilterOperation.Ctn ?
                        new Regex($@".*{filterDef.Value}.*", RegexOptions.IgnoreCase) :
                        new Regex(filterDef.Value, RegexOptions.IgnoreCase);

                    if (filterDef.Field.EqualsIgnoreCase(nameof(Item.Name)))
                    {
                        filter &= builder.Regex(nameof(Item.Name), valueRegex);
                    }
                    else if (filterDef.Field.EqualsIgnoreCase(nameof(Item.Quantity)))
                    {
                        filter &= builder.Eq(nameof(Item.Quantity), filterDef.Value);
                    }
                    else
                    {
                        if (filterDef.Value.EqualsIgnoreCase("false"))
                        {
                            filter &= builder.ElemMatch(nameof(Item.Properties),
                                propertyBuilder.Regex(nameof(ItemProperty.Key), fieldRegex) &
                                (propertyBuilder.Eq(nameof(ItemProperty.Value), "") |
                                 propertyBuilder.Eq(nameof(ItemProperty.Value), "false")));
                        }
                        else
                        {
                            filter &= builder.ElemMatch(nameof(Item.Properties),
                                propertyBuilder.Regex(nameof(ItemProperty.Key), fieldRegex) &
                                propertyBuilder.Regex(nameof(ItemProperty.Value), valueRegex));
                        }
                    }
                }
                else
                {
                    if (filterDef.Field.EqualsIgnoreCase(nameof(Item.Quantity)))
                    {
                        Func<FieldDefinition<Item, double>, double, FilterDefinition<Item>> op = filterDef.Operation switch
                        {
                            BasicFilterOperation.Lt => builder.Lt,
                            BasicFilterOperation.Lte => builder.Lte,
                            BasicFilterOperation.Gt => builder.Gt,
                            BasicFilterOperation.Gte => builder.Gte,
                            _ => builder.Eq
                        };

                        filter &= op(nameof(Item.Quantity), double.Parse(filterDef.Value));
                    }
                    else
                    {
                        Func<FieldDefinition<ItemProperty, double>, double, FilterDefinition<ItemProperty>> op = filterDef.Operation switch
                        {
                            BasicFilterOperation.Lt => propertyBuilder.Lt,
                            BasicFilterOperation.Lte => propertyBuilder.Lte,
                            BasicFilterOperation.Gt => propertyBuilder.Gt,
                            BasicFilterOperation.Gte => propertyBuilder.Gte,
                            _ => propertyBuilder.Eq
                        };

                        filter &= builder.ElemMatch(nameof(Item.Properties),
                            propertyBuilder.Regex(nameof(ItemProperty.Key), fieldRegex) &
                            op(nameof(ItemProperty.Value), double.Parse(filterDef.Value)));
                    }
                }
            }

            return Entities.Find(filter);
        }
    }
}
