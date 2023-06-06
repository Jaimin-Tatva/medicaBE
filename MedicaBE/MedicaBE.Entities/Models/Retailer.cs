using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.Models
{
    public class Retailer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RetailerId { get; set; }

        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        public string? LastName { get; set; }

        [BsonElement("phonenumber")]
        public long PhoneNumber { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("avtar")]
        public string? Avtar { get; set; }

        [BsonElement("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("updatedat")]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("deletedat")]
        public DateTime? DeletedAt { get; set; }

        
    }
}
