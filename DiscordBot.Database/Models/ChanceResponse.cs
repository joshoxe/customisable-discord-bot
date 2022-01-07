using System;
using System.Threading.Tasks;
using Discord.Commands;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscordBot.Database.Models
{
    public class ChanceResponse : IResponse
    {
        public string Response { get; set; }

        [BsonId] public string Id { get; set; }

        public string Command { get; set; }
        public double Chance { get; set; }

        public async Task ExecuteChanceCommand(ICommandContext ctx)
        {
            Console.WriteLine("Executed");
            var random = new Random();
            var target = random.NextDouble();

            if(target <= Chance)
            {
                await ctx.Channel.SendMessageAsync(Response);
            }
        }
    }
}