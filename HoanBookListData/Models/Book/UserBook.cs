using MongoDbConnection.BaseEntities;

namespace HoanBookListData.Models
{
    public class UserBook : BaseMongoEntity
    {
        public string UserId { get; set; }

        public string BookId { get; set; }

        public bool IsLiked { get; set; }

        public bool IsBookmarked { get; set; }
    }
}