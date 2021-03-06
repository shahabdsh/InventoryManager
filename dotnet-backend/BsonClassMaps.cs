﻿using System.Collections.Generic;
using InventoryManager.Api.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace InventoryManager.Api
{
    public static class BsonClassMaps
    {
        public static void Map()
        {
            BsonClassMap.RegisterClassMap<EntityBase>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<OwnedEntity>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.MapMember(c => c.OwnerId)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<Item>(cm =>
            {
                cm.SetDiscriminator(nameof(Item));
                cm.AutoMap();
                cm.MapMember(c => c.SchemaId)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<ItemSchema>(cm =>
            {
                cm.SetDiscriminator(nameof(ItemSchema));
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.SetDiscriminator(nameof(User));
                cm.AutoMap();
                cm.MapMember(c => c.RevokedTokens)
                    .SetDefaultValue(new List<string>());
            });
        }
    }
}
