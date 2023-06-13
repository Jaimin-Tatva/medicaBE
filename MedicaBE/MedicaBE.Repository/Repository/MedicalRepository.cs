
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using MedicaBE.Entities.Models;
using MedicaBE.Repository.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var filter = Builders<User>.Filter.Eq(x => x.UserId,UserID);
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
                lineCell.Padding = 5f;

                PdfPTable lineTable = new PdfPTable(1);
                lineTable.WidthPercentage = 100;
                lineTable.DefaultCell.Border = Rectangle.NO_BORDER;
                lineTable.AddCell(lineCell);

                document.Add(lineTable);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("Text: " + Text));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("PTO"));
                document.Close();
                


                return user;
            }
            return null;

        }


    }
}
