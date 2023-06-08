using MedicaBE.Entities.Interface;
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
    public class RetailerRepository : IRetailerRepository
    {
        private readonly IMongoCollection<Retailer> collection;
        private readonly IEncryptDecryptPassword _encryptDecryptPassword;
        private readonly IConfiguration _configuration;

        public RetailerRepository(DatabaseSettings settings, IMongoClient mongoClient, IEncryptDecryptPassword encryptDecryptPassword, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            collection = database.GetCollection<Retailer>(settings.CollectionNames["Retailer"]);
            _encryptDecryptPassword = encryptDecryptPassword;
            _configuration = configuration;
        }

        public Retailer Registration(RegistrationVM model)
        {
            var filter = Builders<Retailer>.Filter.Eq(x => x.PhoneNumber, model.PhoneNumber);
            var count = collection.CountDocuments(filter);
            if(count > 0)
            {
                return null;
            }
            var newuser = new Retailer();
            {
                newuser.FirstName = model.FirstName;
                newuser.LastName = model.LastName;
                newuser.PhoneNumber = model.PhoneNumber;
                string encrypt = _encryptDecryptPassword.EncryptPassword(model.Password);
                newuser.Password = encrypt;

            }
            collection.InsertOne(newuser);
            return newuser;
        }

        public Retailer Login(LoginVM model)
        {
            string encryptPassword = _encryptDecryptPassword.EncryptPassword(model.Password);
            var filter = Builders<Retailer>.Filter.Eq(x => x.PhoneNumber, model.PhoneNumber) & Builders<Retailer>.Filter.Eq(x => x.Password, encryptPassword);
            var GetUser = collection.Find(filter).FirstOrDefault();

            if (GetUser != null)
            {
               
                return GetUser;
            }
            return null;
        }

        public List<Retailer> getuserlist()
        {
            var list = collection.Find(_ => true).ToList();
            return list;
        }
    }
}
