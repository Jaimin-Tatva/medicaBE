using MedicaBE.Entities.Interface;

using MedicaBE.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicaBE.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class RetailerController : ControllerBase
    {
        public readonly IRetailerRepository _RetailRepo;

        public RetailerController(IRetailerRepository RetailRepo)
        {
            _RetailRepo = RetailRepo;
        }

        [HttpPost("Registration")]
        public IActionResult Register(RegistrationVM model)
        {
            if (ModelState.IsValid)
            {
                _RetailRepo.Registration(model);
                return Ok("User registration successful!");
            }
            return BadRequest("Enter a Correct Fields!");
        }

        [HttpPut("Login")]
        public IActionResult Login(long PhoneNumber, string Password)
        {
            var user = new LoginVM();
            user.PhoneNumber = PhoneNumber;
            user.Password = Password;
            if (ModelState.IsValid) 
            {
                var checkuser = _RetailRepo.Login(user);
                if(checkuser != null)
                {
                    return Ok("Login SuccessFully!!");
                }
                else
                {
                    return BadRequest("Enter a correct PhoneNumber and Password!!");
                }
            }
            return Ok();
        }
    }
}
