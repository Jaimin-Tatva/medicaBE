using MedicaBE.Entities.ViewModels;
using MedicaBE.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicaBE.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository _iuserRepository)
        {
            userRepository = _iuserRepository;
        }

        [HttpPost("User login")]
        public IActionResult UserLogin(UserLoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var check = userRepository.ValidateUser(user);
                return Ok(check);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

        }

        [HttpPost("Register user")]
        public IActionResult RegisterUser(UserRegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = userRepository.RegisterUser(user);
                return Ok(result);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

        }

    }

