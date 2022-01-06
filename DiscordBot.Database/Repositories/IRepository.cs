using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Database.Models;

namespace DiscordBot.Database.Repositories
{
    public interface IRepository
    {
        Task<CommandResponse> GetById(string id);
        Task<List<CommandResponse>> GetAll();
        Task Add(CommandResponse commandResponse);
    }
}