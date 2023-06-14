using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserRepository(DatabaseSettings settings, IMongoClient mongoClient, IEncryptDecryptPassword encryptDecryptPassword, IConfiguration configuration, IMapper mapper)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.CollectionNames["User"]);
            _encryptDecryptPassword = encryptDecryptPassword;
            _configuration = configuration;
            _mapper = mapper;
        }


        public User ValidateUser(UserLoginViewModel user)
        {
            string encryptedPassword = _encryptDecryptPassword.EncryptPassword(user.Password);
            var result = _user.Find(userdetails => userdetails.PhoneNumber == user.PhoneNumber && userdetails.Password == encryptedPassword).FirstOrDefault();

            return result;
        }

        public int RegisterUser(UserRegisterViewModel user)
        {
            var userEntity = _mapper.Map<User>(user);

            userEntity.CreatedAt = DateTime.Now;
            string encryptedPassword = _encryptDecryptPassword.EncryptPassword(user.Password);
            userEntity.Password = encryptedPassword;

            try
            {
                _user.InsertOne(userEntity);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public List<User> GetUserList()
        {
            return _user.Find(_ => true).ToList();
        }

        public User GetUserById(string userid)
        {
            return _user.Find(user => user.UserId == userid).FirstOrDefault();
        }

    } 
}
