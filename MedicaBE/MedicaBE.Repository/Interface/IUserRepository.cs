using MedicaBE.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Repository.Interface;

public interface IUserRepository
{
    public int ValidateUser(UserLoginViewModel user);
    public int RegisterUser(UserRegisterViewModel user);
}

