using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Database.Models;
using DiscordBot.Database.Repositories;

namespace DiscordBot.Bot.Commands
{
    public class CustomCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly IRepository _repository;
        private Task<List<CommandResponse>> _customCommands;

        public CustomCommandModule(IRepository repository)
        {
            _repository = repository;
            _customCommands = _repository.GetAll();
        }
    }
}