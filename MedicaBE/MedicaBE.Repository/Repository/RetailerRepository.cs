using MedicaBE.Entities.Interface;
using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
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

        public RetailerRepository(DatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            collection = database.GetCollection<Retailer>(settings.CollectionNames["Retailer"]);
        }


        //private readonly IMongoCollection<Retailer> collection;

        //public RetailerRepository(IMongoDatabase database )
        //{
        //    collection = database.GetCollection<Retailer>("Retailer");

        //}

        public Retailer Registration(RegistrationVM model)
        {
            var newuser = new Retailer();
            {
                newuser.FirstName = model.FirstName;
                newuser.LastName = model.LastName;
                newuser.PhoneNumber = model.PhoneNumber;
                newuser.Password = model.Password;

            }
            collection.InsertOne(newuser);
            return newuser;
        }

        public Retailer Login(LoginVM model)
        {
            var filter = Builders<Retailer>.Filter.Eq(x => x.PhoneNumber, model.PhoneNumber) & Builders<Retailer>.Filter.Eq(x => x.Password, model.Password);
            var GetUser = collection.Find(filter).FirstOrDefault();

            if (GetUser != null)
            {
                return GetUser;
            }
            return null;
        }
    }
}
