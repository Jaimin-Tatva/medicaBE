using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.ViewModels;

    public class UserLoginViewModel
    {
        [BsonElement("phonenumber")]
        [Required(ErrorMessage = "Phone number is Required")]
        public long PhoneNumber { get; set; }

        [BsonElement("password")]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

}

