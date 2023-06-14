
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using MedicaBE.Entities.Models;
using MedicaBE.Repository.Interface;
using MongoDB.Driver;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MedicaBE.Repository.Repository
{
    public class MedicalRepository : IMedicalRepository
    {
        private readonly IMongoCollection<MedicalBill> Billcollection;
        private readonly IMongoCollection<User> _user;


        public MedicalRepository(DatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            Billcollection = database.GetCollection<MedicalBill>(settings.CollectionNames["MedicalBill"]);
            _user = database.GetCollection<User>(settings.CollectionNames["User"]);

        }
        //public byte[] CreateBill(MedicalBill addbill)
        //{
        //    Billcollection.InsertOne(addbill);
        //    Document document = new Document();


        //    MemoryStream stream = new MemoryStream();
        //    PdfWriter writer = PdfWriter.GetInstance(document, stream);


        //    document.Open();


        //    Paragraph header = new Paragraph("Medical Bill");
        //    header.Alignment = Element.ALIGN_CENTER;
        //    header.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f);
        //    document.Add(header);

        //    DateTime currentDate = DateTime.Now;
        //    string formattedDate = currentDate.ToString("MMMM dd, yyyy");

        //    document.Add(new Paragraph("Patient Name: " + addbill.PatientName));
        //    document.Add(new Paragraph("Date:" + formattedDate));
        //    document.Add(new Paragraph("ContactNumber: " + addbill.ContactNumber));
        //    //document.Add(new Paragraph("Total Amount: $100.00"));
        //    document.Add(new Paragraph(" "));

        //    PdfPCell lineCell = new PdfPCell();
        //    lineCell.Border = Rectangle.NO_BORDER;
        //    lineCell.FixedHeight = 2f;

        //    PdfPTable lineTable = new PdfPTable(1);
        //    lineTable.WidthPercentage = 100;
        //    lineTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //    lineTable.AddCell(lineCell);

        //    document.Add(lineTable);


        //    PdfPTable table = new PdfPTable(3);
        //    table.AddCell("Name");
        //    table.AddCell("Manufacturer");
        //    table.AddCell("Price");

        //    foreach (var item in addbill.Medications)
        //    {
        //        table.AddCell(item.Name);
        //        table.AddCell(item.Manufacturer.ToString());
        //        table.AddCell(item.Price.ToString()); // Format price as currency

        //        // Add more cells or rows as needed based on your data structure
        //    }

        //    document.Add(table);

        //    document.Close();

        //    return stream.ToArray();

        //}

        public User CreateBill(string UserID, string Text)
        {
            var filter = Builders<User>.Filter.Eq(x => x.UserId, UserID);
            var user = _user.Find(filter).FirstOrDefault();
            if (user != null)
            {

                Document document = new Document();

                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                string currentTimeString = currentTime.ToString(@"hh\:mm\:ss\.fffffff");
                currentTimeString = currentTimeString.Replace(":", "");

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PDFs", $"{user.PhoneNumber}_{currentTimeString}.pdf");
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                Paragraph header = new Paragraph("Medical Bill");
                header.Alignment = Element.ALIGN_CENTER;
                header.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f);
                document.Add(header);

                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("MMMM dd, yyyy");

                document.Add(new Paragraph("Patient Name: " + user.FirstName + " " + user.LastName));
                document.Add(new Paragraph("Date:" + formattedDate));
                document.Add(new Paragraph("ContactNumber: " + user.PhoneNumber));
                document.Add(new Paragraph("Address:" + user.Address));
                document.Add(new Paragraph(" "));

                PdfPCell lineCell = new PdfPCell();
                lineCell.Border = Rectangle.BOTTOM_BORDER;
                lineCell.BorderColorBottom = BaseColor.BLACK;
                lineCell.PaddingTop = 500f;

                PdfPTable lineTable = new PdfPTable(1);
                lineTable.WidthPercentage = 100;
                lineTable.DefaultCell.Border = Rectangle.NO_BORDER;
                lineTable.AddCell(lineCell);

                document.Add(lineTable);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("Text: " + Text));


                Paragraph bottomParagraph = new Paragraph();
                float paddingSize = 400f; // Adjust this value to control the padding size

                bottomParagraph.Leading = paddingSize;
                bottomParagraph.Add("PTO");
                bottomParagraph.Alignment = Element.ALIGN_CENTER;

                // Add the bottom paragraph to the document
                document.Add(bottomParagraph);

                document.Close();

                var url = "https://api.ultramsg.com/instance50771/messages/document";
                var recipientNumber = "+91" + user.PhoneNumber;
                var client = new RestClient(url);

                var request = new RestRequest(url, Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", "obzujoi8r2rtwe4s");
                request.AddParameter("to", recipientNumber);
                request.AddParameter("filename", $"{user.PhoneNumber}_{currentTimeString}.pdf");
                //request.AddParameter("document", "https://file-example.s3-accelerate.amazonaws.com/documents/cv.pdf");
                request.AddParameter("caption", "document caption");

                //var pdffilepath = "Downloads/Daily-weeky diary_Bhakti.pdf";
                var pdffilepath = Path.Combine(Directory.GetCurrentDirectory(), "PDFs", $"{user.PhoneNumber}_{currentTimeString}.pdf");
                byte[] AsBytes = File.ReadAllBytes(pdffilepath);
                String AsBase64String = Convert.ToBase64String(AsBytes);
                request.AddParameter("document", AsBase64String);



                RestResponse response =  client.Execute(request);
                var output = response.Content;
                Console.WriteLine(output);



                //string accountSid = "ACacdae2678b424c0fa2b6634bcb083ab9";
                //string authToken = "5e5ebc46f635edd38bc69b72fa08ebb5";
                //    string twilioNumber = "+13393452130";
                //    string recipientNumber = "+91" + user.PhoneNumber;

                // Initialize the Twilio client
                //TwilioClient.Init(accountSid, authToken);

                //var messageOptions = new CreateMessageOptions(new PhoneNumber("whatsapp:+917874503745"))
                //{
                //    //messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                //    From = new PhoneNumber("whatsapp:+14155238886"),
                //    Body = "Your appointment is coming up on July 21 at 3PM"

                //};

                //var message = MessageResource.Create(messageOptions);
                //Console.WriteLine(message.Body);

                return user;
            }
            return null;

        }


    }
}
