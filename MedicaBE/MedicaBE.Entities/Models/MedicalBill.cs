using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.Models
{
    public class MedicalBill
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? MedicalBillId { get; set; }
        public string RetailerName { get; set; }

        public string PatientName { get; set; }
        public string? Address { get; set; }
        public long ContactNumber { get; set; }
        public List<Medication> Medications { get; set; }

        public MedicalBill()
        {
            Medications = new List<Medication>();
        }
    }

    public class Medication
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        
    }
}
