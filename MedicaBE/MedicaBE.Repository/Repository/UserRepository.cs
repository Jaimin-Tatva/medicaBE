using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
using MedicaBE.Repository.Interface;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Repository
{
    public class UserRepository : IUserRepository

    {
        private readonly IMongoCollection<User> _user;
        private readonly IEncryptDecryptPassword _encryptDecryptPassword;
        private readonly IConfiguration _configuration;

        public UserRepository(DatabaseSettings settings, IMongoClient mongoClient, IEncryptDecryptPassword encryptDecryptPassword, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.CollectionNames["UserCollection"]);
            _encryptDecryptPassword = encryptDecryptPassword;
            _configuration = configuration;
        }

        public User ValidateUser(UserLoginViewModel user)
        {
            string encryptedPassword = _encryptDecryptPassword.EncryptPassword(user.Password);
            var result = _user.Find(userdetails => userdetails.PhoneNumber == user.PhoneNumber && userdetails.Password == encryptedPassword).FirstOrDefault();

            return result;
        }

        public int RegisterUser(UserRegisterViewModel user)
        {
            User userdetails = new User();
            userdetails.PhoneNumber = user.PhoneNumber;
            userdetails.FirstName = user.FirstName;
            userdetails.LastName = user.LastName;
            userdetails.Email = user.Email;
            userdetails.CreatedAt = DateTime.Now;
            string encryptedPassword = _encryptDecryptPassword.EncryptPassword(user.Password);
            userdetails.Password = encryptedPassword;
            try
            {
                _user.InsertOne(userdetails);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

    }
}
