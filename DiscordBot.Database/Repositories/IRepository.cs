using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Database.Models;

namespace DiscordBot.Database.Repositories
{
    public interface IRepository
    {
        Task<List<T>> GetAll<T>(string collectionName);
        Task Add<T>(T commandResponse, string collectionName);
    }
}