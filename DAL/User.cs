﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DAL
{
    public class User 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
    }
}
