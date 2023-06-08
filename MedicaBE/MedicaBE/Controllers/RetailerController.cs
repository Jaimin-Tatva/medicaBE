using MedicaBE.Auth;
using MedicaBE.Entities.Auth;
using MedicaBE.Entities.Interface;

using MedicaBE.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicaBE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RetailerController : ControllerBase
    {
        public readonly IRetailerRepository _RetailRepo;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public RetailerController(IRetailerRepository RetailRepo, IConfiguration configuration, JwtTokenHelper jwtTokenHelper)
        {
            _RetailRepo = RetailRepo;
            _configuration = configuration;
            _jwtTokenHelper = jwtTokenHelper;
        }


        [HttpPost("Registration")]
        public IActionResult Register(RegistrationVM model)
        {
            if (ModelState.IsValid)
            {
                var user = _RetailRepo.Registration(model);
                if (user == null)
                {
                    return BadRequest("Mobile Number Already Exist!!");
                }
                return Ok("User registration successful!");
            }
            return BadRequest("Enter a Correct Fields!");
        }

        [HttpPost("Login")]
        public IActionResult Login(long PhoneNumber, string Password)
        {
            var user = new LoginVM();
            user.PhoneNumber = PhoneNumber;
            user.Password = Password;
            if (ModelState.IsValid)
            {
                var checkuser = _RetailRepo.Login(user);
                if (checkuser != null)
                {
                    var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
                    var token = _jwtTokenHelper.GenerateToken(jwtSettings, checkuser);
                    HttpContext.Session.SetString("Token", token);
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Enter a correct PhoneNumber and Password!!");
                }
            }
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "RetailerToken")]
        [HttpGet]
        public IActionResult RetailerList()
        {
            var getuser = _RetailRepo.getuserlist();
            return Ok(getuser);
        }
    }
}
