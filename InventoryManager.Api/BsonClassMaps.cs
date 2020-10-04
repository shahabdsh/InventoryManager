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
            BsonClassMap.RegisterClassMap<Item>(cm => {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.MapExtraElementsMember(c => c.Properties);
            });
            
            BsonClassMap.RegisterClassMap<ItemSchema>(cm => {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}