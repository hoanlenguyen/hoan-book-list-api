using MongoDB.Driver;
using MongoTutorialDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDb.DatabaseContext
{
    public class MongoDbContext
    {   
        public IMongoDatabase Database  {get; private set; }       

        public MongoDbContext(IMongoDbConnectionSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
        }
    }
}
