using MongoDB.Driver;

namespace HoanBookListData.MongoDb
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; private set; }

        public string ConnectionString { get; private set; }

        public MongoDbContext(IMongoDbConnectionSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);

            ConnectionString = settings.ConnectionString;
        }
    }
}