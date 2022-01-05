using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiscordBot.Database.Models
{
    public class CommandResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Command { get; set; }
        public string Response { get; set; }
    }
}