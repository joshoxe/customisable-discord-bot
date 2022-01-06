using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Database.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DiscordBot.Database.Repositories
{
    public class CommandResponseRepository : IRepository
    {
        private readonly IMongoCollection<CommandResponse> _commandCollection;

        public CommandResponseRepository(IOptions<CommandDatabaseSettings> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.DatabaseName);

            _commandCollection = mongoDatabase.GetCollection<CommandResponse>(config.Value.CommandResponseCollectionName);
        }

        public async Task<CommandResponse> GetById(string id)
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