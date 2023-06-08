using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Interface
{
    public interface IUniqueAttributesRepository
    {
        public bool IsUserEmailUnique(string email);
        public bool IsUserPhoneUnique(long phonenumber);
    }
}
