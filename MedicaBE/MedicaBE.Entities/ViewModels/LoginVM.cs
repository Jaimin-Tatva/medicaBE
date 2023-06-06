using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.ViewModels
{
    public class LoginVM
    {
        [BsonElement("phonenumber")]
        [Required]
        public long PhoneNumber { get; set; }

        [BsonElement("password")]
        [Required]
        public string Password { get; set; }
    }
}
