namespace MongoDbConnection.BaseEntities
{
    public class MongoMetaEntity : BaseMongoEntity
    {
        public Meta Meta { get; set; } = new Meta();
    }
}