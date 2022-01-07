using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Builders;
using Discord.WebSocket;
using DiscordBot.Bot.Commands;
using DiscordBot.Bot.Logging;
using Microsoft.Extensions.Configuration;

namespace DiscordBot.Bot {
    public class BotService {
        private readonly IConfiguration _config;
        private readonly ICommandHandler _commandHandler;
        private readonly DiscordSocketClient _client;
        private readonly ILogger _logger;
        private bool _started;
       

        public BotService(IConfiguration config, ICommandHandler commandHandler, DiscordSocketClient client, ILogger logger) {
            _config = config;
            _commandHandler = commandHandler;
            _client = client;
            _logger = logger;
        }

        public async Task MainAsync()
        {

            if (_started || _client.ConnectionState == ConnectionState.Connecting || _client.ConnectionState == ConnectionState.Connected)
                return;

            _started = true;

            _client.Log += Log;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            var token = _config.GetValue<string>("Bot:Token");

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            _logger.Log(new LogMessage(LogSeverity.Info, "", "Logging in.."));
            await _client.LoginAsync(TokenType.Bot, token);
            _logger.Log(new LogMessage(LogSeverity.Info, "", $"Starting bot.."));
            await _client.StartAsync();
            await _commandHandler.InstallCommandsAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        public void StopBot() {
            _client.StopAsync();
            _started = false;
        }

        private Task Log(LogMessage message) {
            _logger.Log(message);
            return Task.CompletedTask;
        }

        public bool HasStarted() {
            return _started;
        }
    }
}