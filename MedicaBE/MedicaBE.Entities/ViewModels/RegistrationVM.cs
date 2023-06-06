using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.ViewModels
{
    public class RegistrationVM
    {
        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        public string? LastName { get; set; }

        [BsonElement("phonenumber")]
        public long PhoneNumber { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Please Enter same Password and Confirm Password")]
        public string ConfirmPassword{ get; set; }
    }
}
