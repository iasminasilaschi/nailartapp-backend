﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NailArt.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
