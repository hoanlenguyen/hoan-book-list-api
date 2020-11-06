using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoanBookListData.Models
{
    public class UserBook
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string BookId { get; set; }

        public bool IsLiked { get; set; }

        public bool IsBookmarked { get; set; }
    }
}