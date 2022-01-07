using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Database.Models
{
    public interface IResponse
    {
        string Id { get; set; }
        string Command { get; set; }
        Task ExecuteChanceCommand(ICommandContext ctx);
    }
}