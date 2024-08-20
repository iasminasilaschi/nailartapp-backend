using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NailArt.Models
{
    public class NailDesign
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }
    }
}
