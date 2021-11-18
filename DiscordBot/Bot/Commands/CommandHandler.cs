using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Bot.Commands {
    public class CommandHandler : ICommandHandler {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _servicesProvider;

        // Retrieve client and CommandService instance via ctor
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider servicesProvider) {
            _commands = commands;
            _servicesProvider = servicesProvider;
            _client = client;
        }

        public async Task InstallCommandsAsync() {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(),
                _servicesProvider);
        }

        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if(message == null) return;

            int argPos = 0;

            if(!(message.HasCharPrefix('!', ref argPos) ||
                 message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
               message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _servicesProvider);
        }
    }
}