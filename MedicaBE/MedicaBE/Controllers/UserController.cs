using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
using MedicaBE.Repository.Interface;
using MedicaBE.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicaBE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUniqueAttributesRepository _attributesRepository;
    private readonly IConfiguration _configuration;
    public UserController(IUserRepository iuserRepository, IUniqueAttributesRepository attributesRepository)
    {
        _userRepository = iuserRepository;
        _attributesRepository = attributesRepository;
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
            if(check != null)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest("User not found !!!");
            }
            
        }

    }

    [Authorize]
    [HttpPost("Register user")]
    public IActionResult RegisterUser(UserRegisterViewModel user)
    {
        bool email = _attributesRepository.IsUserEmailUnique(user.Email);
        bool phone = _attributesRepository.IsUserPhoneUnique(user.PhoneNumber);
      //  var repository = new AttributesRepository<User, string>(settings, mongoClient, "User");


        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }

        else if (!email)
        {
            return BadRequest("Email is already registered.");
        }

        else if (!phone)
        {
            return BadRequest("Phone number is already registered.");
        }
        else
        {
            var result = _userRepository.RegisterUser(user);
            return Ok(result);
        }

    }

}

