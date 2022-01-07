using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Bot.Logging;
using DiscordBot.Database.Models;
using DiscordBot.Database.Repositories;
using Microsoft.Extensions.Options;

namespace DiscordBot.Bot.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ILogger _logger;
        private readonly IOptions<CommandDatabaseSettings> _config;
        private readonly IRepository _repository;
        private readonly IServiceProvider _servicesProvider;

        // Retrieve client and CommandService instance via ctor
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider servicesProvider, IRepository repository, ILogger logger, IOptions<CommandDatabaseSettings> config)
        {
            _commands = commands;
            _servicesProvider = servicesProvider;
            _repository = repository;
            _logger = logger;
            _config = config;
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

            if (message.Author.IsBot)
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

            var customCommands = new List<IResponse>();
            customCommands.AddRange(await _repository.GetAll<CommandResponse>(_config.Value.CommandResponseCollectionName));
            customCommands.AddRange(await _repository.GetAll<ChanceResponse>(_config.Value.ChanceResponseCollectionName));
            customCommands.AddRange(await _repository.GetAll<RandomResponse>(_config.Value.RandomResponseCollectionName));

            foreach (var command in customCommands)
            {
                _logger.Log(new LogMessage(LogSeverity.Info, "", $"Registering custom {command.Command} command.."));
                await _commands.CreateModuleAsync(command.Command, module => { module.AddCommand("", (ctx, args, services, cmd) => command.ExecuteChanceCommand(ctx), _ => { }); });
            }
        }
    }
}