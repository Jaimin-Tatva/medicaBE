using System;
using RestSharp;
using System.Threading.Tasks;

namespace MedicaBE.Send_Pdf
{
    public class Send_pdf_whatsapp
    {
        public void send_pdf_file()
        {
            var url = "https://api.ultramsg.com/instance50771/messages/document";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", "obzujoi8r2rtwe4s");
            request.AddParameter("to", "+917874503745");
            request.AddParameter("filename", "Customer.pdf");
            request.AddParameter("caption", "Your report is here !!!");

            var pdffilepath = "Downloads/Daily-weeky diary_Bhakti.pdf";
            byte[] AsBytes = File.ReadAllBytes(pdffilepath);
            String AsBase64String = Convert.ToBase64String(AsBytes);
            request.AddParameter("document", AsBase64String);

            RestResponse response = client.Execute(request);
            var output = response.Content;
            Console.WriteLine(output);

        }

    }
}









