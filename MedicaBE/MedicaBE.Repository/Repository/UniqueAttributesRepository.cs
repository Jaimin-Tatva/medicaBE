using MedicaBE.Entities.Models;
using MedicaBE.Repository.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Repository
{
    public class UniqueAttributesRepository : IUniqueAttributesRepository
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoDatabase _database;
        public UniqueAttributesRepository(DatabaseSettings settings, IMongoClient mongoClient)
        {
             _database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = _database.GetCollection<User>(settings.CollectionNames["User"]);
        }

        public User IsUserEmailUnique(string email)
        {
            var result = _user.Find(userdata => userdata.Email == email).FirstOrDefault();
            return result;
        }

        public User IsUserPhoneUnique(long phonenumber)
        {
            var result = _user.Find(userdata => userdata.PhoneNumber == phonenumber).FirstOrDefault();
            return result;
        }
    }
}
