using MedicaBE.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Interface
{
    public interface IMedicalRepository
    {


        //public byte[] CreateBill(MedicalBill addbill);

        public User CreateBill(string UserID, string Text);
    }
}
