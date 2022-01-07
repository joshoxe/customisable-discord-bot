using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Database.Models
{
    public class CommandResponse : IResponse
    {
        public string Id { get; set; }
        public string Command { get; set; }

        public async Task ExecuteChanceCommand(ICommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(Response);
        }

        public string Response { get; set; }
    }
}