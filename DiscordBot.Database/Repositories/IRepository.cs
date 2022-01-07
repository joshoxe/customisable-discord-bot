using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Database.Models;

namespace DiscordBot.Database.Repositories
{
    public interface IRepository
    {
        Task<CommandResponse> GetById<T>(string id, string collectionName);
        Task<List<CommandResponse>> GetAll<T>(string collectionName);
        Task Add<T>(CommandResponse commandResponse, string collectionName);
    }
}