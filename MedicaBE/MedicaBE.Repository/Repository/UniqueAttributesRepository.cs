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

        public bool IsUserEmailUnique(string email)
        {
            var filter = Builders<User>.Filter.Eq("email", email);
            var count = _user.CountDocuments(filter);

            var result = _user.Find(x => x.Email == email).FirstOrDefault();

            return count == 0;
        }

        public bool IsUserPhoneUnique(long phonenumber)
        {
            var filter = Builders<User>.Filter.Eq("phonenumber", phonenumber);
            var count = _user.CountDocuments(filter);

            return count == 0;
        }
    }
}
