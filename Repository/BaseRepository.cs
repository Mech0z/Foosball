using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;

namespace Repository
{
    public class BaseRepository<T>
    {
        protected static IMongoClient Client;
        protected static IMongoDatabase Database;
        private readonly string _collectionName;

        public BaseRepository(IOptions<ConnectionStringsSettings> settings, string collectionName)
        {
            Client = new MongoClient(settings.Value.DefaultConnectionMongoDB);

            Database = Client.GetDatabase(settings.Value.MongoDBDatabaseName);
            _collectionName = collectionName;
        }

        public IMongoCollection<T> Collection => Database.GetCollection<T>(_collectionName);
    }
}