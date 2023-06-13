using AutoMapper;
using MedicaBE.Auth;
using MedicaBE.Entities.Auth;
using MedicaBE.Entities.Interface;
using MedicaBE.Entities.Models;
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
        private readonly IMapper _mapper;

        public RetailerController(IRetailerRepository RetailRepo, IConfiguration configuration, JwtTokenHelper jwtTokenHelper, IMapper mapper)
        {
            _RetailRepo = RetailRepo;
            _configuration = configuration;
            _jwtTokenHelper = jwtTokenHelper;
            _mapper = mapper;
        }


        //[HttpPost("Registration")]
        //public IActionResult Register(/*RegistrationVM model,*/ string FirstName, string LastName, long PhoneNumber, string Password, string ConfirmPassword)
        //{
        //    var model = new RegistrationVM();
        //    {
        //        model.FirstName = FirstName;
        //        model.LastName = LastName;
        //        model.PhoneNumber = PhoneNumber;
        //        model.Password = Password;
        //        model.ConfirmPassword = ConfirmPassword;
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var user = _RetailRepo.Registration(model);
        //        if (user == null)
        //        {
        //            return BadRequest("Mobile Number Already Exist!!");
        //        }
        //        return Ok("User registration successful!");
        //    }
        //    return BadRequest("Enter a Correct Fields!");
        //}

        [HttpPost("Registration")]
        public IActionResult Register( RegistrationVM model)
        {
            if (ModelState.IsValid)
            {
                var retailer = _mapper.Map<RegistrationVM,Retailer>(model);
                var registeredUser = _RetailRepo.Registration(retailer);
                if (registeredUser == null)
                {
                    return BadRequest("Mobile Number Already Exists!");
                }
                return Ok("User registration successful!");
            }
            return BadRequest("Enter correct fields!");
        }


        //[HttpPost("Login")]
        //public IActionResult Login(LoginVM user )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var checkuser = _RetailRepo.Login(user);
        //        if (checkuser != null)
        //        {
        //            var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
        //            var token = _jwtTokenHelper.GenerateToken(jwtSettings, checkuser);
        //            //HttpContext.Session.SetString("Token", token);
        //            HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
        //            {
        //                HttpOnly = true,
        //                Secure = true,
        //                Expires = DateTime.Now.AddMinutes(10),
        //                SameSite = SameSiteMode.Strict
        //            });
        //            return Ok(token);
        //        }
        //        else
        //        {
        //            return BadRequest("Retailer not found!!!");
        //        }
        //    }
        //    return Ok();
        //}

        [HttpPost("Login")]
        public IActionResult Login(LoginVM user)
        {
            if (ModelState.IsValid)
            {
                var userModel = _mapper.Map<LoginVM, Retailer>(user);
                var checkuser = _RetailRepo.Login(userModel);
                if (checkuser != null)
                {
                    var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
                    var token = _jwtTokenHelper.GenerateToken(jwtSettings, checkuser);
                    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.Now.AddMinutes(10),
                        SameSite = SameSiteMode.Strict
                    });
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Retailer not found!!!");
                }
            }
            return Ok();
        }


        [Authorize(AuthenticationSchemes = "RetailerToken")]
        [HttpGet("RetailerList")]
        public IActionResult RetailerList()
        {
            var authToken = HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(authToken))
            {
                return Unauthorized(); // Or return an appropriate error response
            }

            var getuser = _RetailRepo.getuserlist();
            return Ok(getuser);
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("AuthToken");
            return Ok("Logged out successfully");
        }
    }
}
