using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Database.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DiscordBot.Database.Repositories
{
    public class CommandResponseRepository : IRepository
    {
        private readonly IOptions<CommandDatabaseSettings> _config;
        private readonly MongoClient _mongoClient;
        private readonly IMongoCollection<CommandResponse> _commandCollection;
        private readonly IMongoDatabase _mongoDatabase;

        public CommandResponseRepository(IOptions<CommandDatabaseSettings> config)
        {
            _config = config;

            _mongoClient = new MongoClient(config.Value.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(config.Value.DatabaseName);
            _commandCollection =
                _mongoDatabase.GetCollection<CommandResponse>(config.Value.CommandResponseCollectionName);
        }

        public async Task<CommandResponse> GetCommandResponseById(string id)
        {
            return await _commandCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<CommandResponse>> GetAll()
        {
            return await _commandCollection.Find(c => true).ToListAsync();
        }

        public async Task Add(CommandResponse commandResponse)
        {
            await _commandCollection.InsertOneAsync(commandResponse);
        }
    }
}