using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NailArtApp.Models
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("time")]
        public string Time { get; set; }

        [BsonElement("clientId")]
        public string ClientId { get; set; }

        [BsonElement("artistId")]
        public string ArtistId { get; set; }

        [BsonElement("serviceType")]
        public string ServiceType { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }
    }
}
