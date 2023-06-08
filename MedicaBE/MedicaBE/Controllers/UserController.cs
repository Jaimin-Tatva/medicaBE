using MedicaBE.Auth;
using MedicaBE.Entities.Auth;
using MedicaBE.Entities.ViewModels;
using MedicaBE.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicaBE.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenHelper _jwtTokenHelper;

    public UserController(IUserRepository _iuserRepository, IConfiguration configuration, JwtTokenHelper jwtTokenHelper)
        {
            userRepository = _iuserRepository;
           _configuration = configuration;
          _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost("User login")]
        public IActionResult UserLogin(UserLoginViewModel user)
        {
            if (!ModelState.IsValid)
            {

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);

            }
            else
            {

             var check = userRepository.ValidateUser(user);
            if(check == null)
            {
                return BadRequest("User not found .... !!!");
            }
            var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
            var token = _jwtTokenHelper.GenerateUserToken(jwtSettings, check);

            return Ok(token);

        }

        }

    [Authorize(AuthenticationSchemes = "Token2")]
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

