using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Interface
{
    public interface IEncryptDecryptPassword
    {
         string EncryptPassword(string clearText);
         string DecryptPassword(string cipherText);
    }
}
