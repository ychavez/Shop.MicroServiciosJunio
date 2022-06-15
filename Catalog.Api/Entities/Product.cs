﻿using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public decimal Price { get; set; }
    }
}  
