﻿using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.Interface
{
    public interface IRetailerRepository
    {
        //public Retailer Registration(RegistrationVM model);

        public Retailer Registration(Retailer retailer);

        public Retailer Login(Retailer model);

        public List<Retailer> getuserlist();
    }
}
