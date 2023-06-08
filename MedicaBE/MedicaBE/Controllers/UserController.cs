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
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly IUniqueAttributesRepository _attributesRepository;

    public UserController(IUserRepository _iuserRepository, IConfiguration configuration, JwtTokenHelper jwtTokenHelper, IUniqueAttributesRepository uniqueAttributesRepository)
    {
        _userRepository = _iuserRepository;
        _configuration = configuration;
        _jwtTokenHelper = jwtTokenHelper;
        _attributesRepository = uniqueAttributesRepository;
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

            var check = _userRepository.ValidateUser(user);
            if (check == null)
            {
                return BadRequest("User not found .... !!!");
            }
            var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
            var token = _jwtTokenHelper.GenerateUserToken(jwtSettings, check);

            return Ok(token);

        }

    }

    // [Authorize(AuthenticationSchemes = "UserToken")]
    [HttpPost("Register user")]
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
            return Ok(result + "Registration successful");
        }
        return BadRequest("Not registered !!!");

    }

}

