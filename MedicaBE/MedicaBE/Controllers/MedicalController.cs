using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MedicaBE.Entities.Models;
using MedicaBE.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MedicaBE.Controllers
{
    //[Authorize(AuthenticationSchemes = "RetailerToken")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalController : Controller
    {
        public readonly IMedicalRepository _MedicalRepo;
        private readonly IMapper _mapper;

        public MedicalController(IMedicalRepository MedicalRepo, IMapper mapper)
        {
            _MedicalRepo = MedicalRepo;
            _mapper = mapper;
        }


        //[HttpPost("CreateBill")]
        //public IActionResult CreateBill(MedicalBill addbill)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var bill = _MedicalRepo.CreateBill(addbill);
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PDFs", addbill.ContactNumber + ".pdf");
        //        using (var filestream = new FileStream(filePath, FileMode.Create))
        //        {
        //            bill.WriteTo(filestream);
        //            filestream.Write(bill, 0, bill.Length);
        //        }

        //        return File(bill, "application/pdf", addbill.ContactNumber + ".pdf");
        //    }
        //    return Ok();
        //}

        [HttpPost("CreateBill")]
        public IActionResult CreateBill(string UserID, string Text)
        {
            if (UserID != null && Text != null)
            {
                var bill = _MedicalRepo.CreateBill(UserID, Text);
                if(bill != null)
                {
                    return Ok("Medical Report Send SuccessFully!!");
                }
                return BadRequest("User not Found!!!");
            }
            return BadRequest("All Fields are Required!!");
        }

    }
}
