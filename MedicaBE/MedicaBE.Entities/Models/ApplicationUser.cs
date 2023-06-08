using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.Models
{
    public class ApplicationUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public long PhoneNumber { get; set; } = 0;
        //public string Key { get; set; } = null!;
        //public string Issuer { get; set; } = null!;
        //public string Audience { get; set; } = null!;

    }
}
