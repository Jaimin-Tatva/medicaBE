using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
using MedicaBE.Repository.Interface;
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
        private readonly IJwtTokensRepository _jwtTokensRepository;

        public UserRepository(DatabaseSettings settings, IMongoClient mongoClient, IEncryptDecryptPassword encryptDecryptPassword, IJwtTokensRepository jwtTokensRepository)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.CollectionNames["User"]);
            _encryptDecryptPassword = encryptDecryptPassword;
            _jwtTokensRepository = jwtTokensRepository;
        }

        public string ValidateUser(UserLoginViewModel user)
        {
            string encryptedPassword = _encryptDecryptPassword.EncryptPassword(user.Password);
            var result = _user.Find(userdetails => userdetails.PhoneNumber == user.PhoneNumber && userdetails.Password == encryptedPassword).FirstOrDefault();
            if (result != null)
            {
                string token =  _jwtTokensRepository.CreateTokenForUser(result);
                return token;
            }
            return null;

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
