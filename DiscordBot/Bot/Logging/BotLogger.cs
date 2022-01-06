using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Bot.Logging
{
    public class BotLogger : ILogger
    {
        private string _logMessages;
        public event Action LogUpdated;

        public string LogMessages
        {
            get => _logMessages;
            set
            {
                _logMessages = value;
                LogUpdated?.Invoke();
            }
        }

        public void Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            if(message.Exception is CommandException cmdException)
            {
                LogMessages += $"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
                               + $" failed to execute in {cmdException.Context.Channel}.<br />";
                LogMessages += cmdException + "<br />";
            } else
                LogMessages += $"[General/{message.Severity}] {message}<br />";
        }
    }
}
