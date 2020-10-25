using HoanBookListData.MongoDb;

namespace HoanBookListData.Services
{
    public class BookService
    {
        private string ConnectionString { get; }

        public BookService(MongoDbContext mongoDb)
        {
            ConnectionString = mongoDb.ConnectionString;
        }

        public string GetConnectionString() => ConnectionString;
    }
}