﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MedicaBE.Entities.ViewModels
{
    public class UserRegisterViewModel
    {
        [BsonElement("firstname")]
        [Required(ErrorMessage = "Firstname is Required")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        [Required(ErrorMessage = "Lastname is Required")]
        public string? LastName { get; set; }

        [BsonElement("phonenumber")]
        [Required(ErrorMessage = "Phone number is Required")]
        public long PhoneNumber { get; set; }

        [BsonElement("password")]
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(8, ErrorMessage = "Password should contain atleast 8 charachter")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password should contain atleast one Capital letter , one small case letter, one Digit and one special symbol")]
        public string Password { get; set; }

        [BsonElement("confirmpassword")]
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password", ErrorMessage = "Confirm Password should match the Password")]
        public string ConfirmPassword { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address !!!")]
        public string? Email { get; set; }

        [BsonElement("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}