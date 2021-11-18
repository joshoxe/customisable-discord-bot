using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Bot.Commands {
    public class InfoModule : ModuleBase<SocketCommandContext> {
        [Command("info")]
        [Summary("Gives information on the current bot version")]
        public Task InfoAsync() => ReplyAsync("This is truewrecks 3.0!");

        // Ban a user
        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        // make sure the user invoking the command can ban
        [RequireUserPermission(GuildPermission.BanMembers)]
        // make sure the bot itself can ban
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync("ok!");
        }
    }
}