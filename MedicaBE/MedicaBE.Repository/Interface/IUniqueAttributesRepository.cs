using MedicaBE.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Interface
{
    public interface IUniqueAttributesRepository
    {
        public User IsUserEmailUnique(string email);
        public User IsUserPhoneUnique(long phonenumber);
    }
}
