using System;
using Discord;

namespace DiscordBot.Bot.Logging
{
    public interface ILogger
    {
        public event Action LogUpdated;

        public string LogMessages { get; set; }
        public void Log(LogMessage message);
    }
}