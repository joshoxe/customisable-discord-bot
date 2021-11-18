using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordBot.Bot.Commands {
    public interface ICommandHandler {
        Task InstallCommandsAsync();
        Task HandleCommandAsync(SocketMessage messageParam);
    }
}