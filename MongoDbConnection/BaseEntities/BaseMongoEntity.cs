using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbConnection.BaseEntities
{
    public class BaseMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}