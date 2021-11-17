using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace DiscordBot.Bot {
    public class BotService {
        private readonly IConfiguration _config;
        private DiscordSocketClient _client;
        private bool _started;
        private string _logMessages;
        public event Action LogUpdated;

        public string LogMessages {
            get => _logMessages;
            set {
                _logMessages = value;
                LogUpdated?.Invoke();
            }
        }

        public BotService(IConfiguration config) {
            _config = config;
        }

        public async Task MainAsync() {
            _started = true;
            _client = new DiscordSocketClient();

            _client.Log += Log;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            var token = _config.GetValue<string>("Bot:Token");

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            WriteLog(new LogMessage(LogSeverity.Info, "", "Logging in.."));
            await _client.LoginAsync(TokenType.Bot, token);
            WriteLog(new LogMessage(LogSeverity.Info, "", $"Starting bot.."));
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        public void StopBot() {
            _client.StopAsync();
            _started = false;
        }

        private Task Log(LogMessage message) {
            WriteLog(message);
            return Task.CompletedTask;
        }

        private void WriteLog(LogMessage message) {
            Console.WriteLine(message.ToString());
            if(message.Exception is CommandException cmdException) {
                LogMessages += $"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
                               + $" failed to execute in {cmdException.Context.Channel}.<br />";
                LogMessages += cmdException + "<br />";
            } else
                LogMessages += $"[General/{message.Severity}] {message}<br />";
        }

        public bool HasStarted() {
            return _started;
        }
    }
}