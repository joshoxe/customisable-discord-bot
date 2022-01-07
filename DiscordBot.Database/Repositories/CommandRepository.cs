using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Database.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DiscordBot.Database.Repositories
{
    public class CommandRepository : IRepository
    {
        private IMongoDatabase _mongoDatabase;

        public CommandRepository(IOptions<CommandDatabaseSettings> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            _mongoDatabase = mongoClient.GetDatabase(config.Value.DatabaseName);
        }

        public async Task<List<T>> GetAll<T>(string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            return await collection.Find<T>(c => true).ToListAsync();
        }

        public async Task Add<T>(T commandResponse, string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(commandResponse);
        }
    }
}