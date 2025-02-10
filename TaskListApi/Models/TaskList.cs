using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskListApi.Models
{
    public class TaskList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("ownerId")]
        public string OwnerId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }


        [BsonElement("sharedUserIds")]
        public List<string> SharedUserIds { get; set; } = new();
    }
}