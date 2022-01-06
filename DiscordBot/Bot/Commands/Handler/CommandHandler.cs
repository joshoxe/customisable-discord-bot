using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Bot.Logging;
using DiscordBot.Database.Repositories;

namespace DiscordBot.Bot.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ILogger _logger;
        private readonly IRepository _repository;
        private readonly IServiceProvider _servicesProvider;

        // Retrieve client and CommandService instance via ctor
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider servicesProvider, IRepository repository, ILogger logger)
        {
            _commands = commands;
            _servicesProvider = servicesProvider;
            _repository = repository;
            _logger = logger;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(),
                _servicesProvider);

            await RegisterCustomCommands();
        }

        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            var argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context,
                argPos,
                _servicesProvider);
        }

        private async Task RegisterCustomCommands()
        {
            _logger.Log(new LogMessage(LogSeverity.Info, "", "Registering custom user commands.."));
            var customCommands = await _repository.GetAll();

            foreach (var command in customCommands)
            {
                _logger.Log(new LogMessage(LogSeverity.Info, "", $"Registering custom {command.Command} command.."));
                await _commands.CreateModuleAsync(command.Command, module => { module.AddCommand("", (ctx, args, services, cmd) => ctx.Channel.SendMessageAsync(command.Response), _ => { }); });
            }
        }
    }
}