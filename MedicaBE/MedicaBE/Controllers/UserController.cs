using AutoMapper;
using MedicaBE.Auth;
using MedicaBE.Entities.Auth;
using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
using MedicaBE.Repository.Interface;
using MedicaBE.Send_Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace MedicaBE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly IUniqueAttributesRepository _attributesRepository;
    private readonly IMapper _mapper;
    private readonly Send_pdf_whatsapp _send;

    public UserController(IUserRepository _iuserRepository, IConfiguration configuration, JwtTokenHelper jwtTokenHelper, IUniqueAttributesRepository uniqueAttributesRepository, IMapper mapper, Send_pdf_whatsapp send_report)
    {
        _userRepository = _iuserRepository;
        _configuration = configuration;
        _jwtTokenHelper = jwtTokenHelper;
        _attributesRepository = uniqueAttributesRepository;
        _mapper = mapper;
        _send = send_report;
    }

    [HttpPost("UserLogin")]
    [AllowAnonymous]
    public IActionResult UserLogin(UserLoginViewModel user)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }
        else
        {

            var check = _userRepository.ValidateUser(user);
            if (check == null)
            {
                return BadRequest("User not found .... !!!");
            }
            var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
            var token = _jwtTokenHelper.GenerateUserToken(jwtSettings, check);

            var userEntity = _mapper.Map<UserInformationViewModel>(check);

            var userInfoJson = JsonConvert.SerializeObject(userEntity);

            Response.Cookies.Append("userInfo", userInfoJson, new CookieOptions
            {

                 Expires = DateTime.UtcNow.AddDays(1),
                 Secure = true,
                 SameSite = SameSiteMode.Strict,
                 Path = "/"

            }); 

            return Ok(token);

        }

    }
    [HttpPost("sendpdf")]
    public void sendpdf()
    {
        _send.send_pdf_file();
    }

    [HttpPost("RegisterUser")]
    public IActionResult RegisterUser(UserRegisterViewModel user)
    {
        var email = _attributesRepository.IsUserEmailUnique(user.Email);
        var phone = _attributesRepository.IsUserPhoneUnique(user.PhoneNumber);
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }
        if (email != null && phone != null)
        {
            return BadRequest("Email and Phone are already registered.");
        }
        if (email != null)
        {
            return BadRequest("Email is already registered.");
        }
        if (phone != null)
        {
            return BadRequest("Phone is already registered.");
        }
        if (ModelState.IsValid)
        {
            var result = _userRepository.RegisterUser(user);
            if(result == 1)
            {
                return Ok("Registration successful");
            }
            if (result == 0)
            {
                return Ok("Registration failed");
            }

        }
        return BadRequest("Not registered !!!");

    }

    [HttpGet("GetUserList")]
    public List<User> GetUserList()
    {
        return _userRepository.GetUserList();      
    }

    [HttpGet("GetUserById")]
    [Authorize(AuthenticationSchemes = "UserToken")]
    public User GetUserById(string userId)
    {
        return _userRepository.GetUserById(userId);
    }

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("userInfo");
        return Ok("User logged out !!!");
    }



}

