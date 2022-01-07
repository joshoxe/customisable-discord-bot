using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscordBot.Database.Models
{
    public class RandomResponse : IResponse
    {
        public List<string> Responses { get; set; }
        public string Response { get; set; }

        [BsonId] public string Id { get; set; }

        public string Command { get; set; }

        public async Task ExecuteChanceCommand(ICommandContext ctx)
        {
            var random = new Random();
            var randomIndex = random.Next(Responses.Count);
            var randomResponse = Responses[randomIndex];

            await ctx.Channel.SendMessageAsync(randomResponse);
        }
    }
}