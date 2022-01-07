using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Database.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DiscordBot.Database.Repositories
{
    public class CommandRepository : IRepository
    {
        private readonly IMongoCollection<CommandResponse> _commandCollection;
        private IMongoDatabase _mongoDatabase;

        public CommandRepository(IOptions<CommandDatabaseSettings> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            _mongoDatabase = mongoClient.GetDatabase(config.Value.DatabaseName);

            _commandCollection = _mongoDatabase.GetCollection<CommandResponse>(config.Value.CommandResponseCollectionName);
        }

        public async Task<CommandResponse> GetById<T>(string id, string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            return await _commandCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<CommandResponse>> GetAll<T>(string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            return await _commandCollection.Find(c => true).ToListAsync();
        }

        public async Task Add<T>(CommandResponse commandResponse, string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            await _commandCollection.InsertOneAsync(commandResponse);
        }
    }
}