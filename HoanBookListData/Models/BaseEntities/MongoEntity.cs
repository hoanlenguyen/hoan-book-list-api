using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace HoanBookListData.Models.BaseEntities
{
    public class MongoEntity : BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [MaxLength(24)]
        public string Id { get; set; }
    }
}