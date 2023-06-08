using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Interface
{
    public interface IJwtTokensRepository
    {
        public string CreateTokenForUser(User user);
    }
}
