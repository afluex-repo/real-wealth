using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace RealWealth
{
    public static class BLMail
    {
        public static string SendMail(string To, string Subject, string Body, bool IsHTML)
        {
            try
            {
                var fromAddress = new MailAddress("coustomer.RealWealth@gmail.com");
                var toAddress = new MailAddress(To);

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
                {

                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, "RealWealth@2022")

                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = IsHTML,
                    Subject = Subject,
                    Body = Body
                })
                    smtp.Send(message);
                return "Mail Sent Successfully";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public static void SendRegistrationMail(string UserName, string LoginId, string Password,  string Subject, string MailId)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            string str = string.Empty;
            string MailText = string.Empty;
            //message.From = new MailAddress("coustomer.RealWealth@gmail.com");
            message.From = new MailAddress("developer2.afluex@gmail.com");
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplateRegistration.html")))
            {
                MailText = reader.ReadToEnd();
            }
            MailText = MailText.Replace("[UserName]", UserName);
            MailText = MailText.Replace("[LoginId]", LoginId);
            MailText = MailText.Replace("[Password]", Password);
            message.Subject = Subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = MailText;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("developer2.afluex@gmail.com", "deve@486");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            message.To.Add(new MailAddress(MailId));
            smtp.Send(message);
        }
        public static void SendActivationMail(string UserName, string LoginId, string Password, string Subject, string MailId)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            string str = string.Empty;
            string MailText = string.Empty;
            message.From = new MailAddress("coustomer.RealWealth@gmail.com");
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplateActivation.html")))
            {
                MailText = reader.ReadToEnd();
            }
            MailText = MailText.Replace("[UserName]", UserName);
            MailText = MailText.Replace("[LoginId]", LoginId);
            MailText = MailText.Replace("[Password]", Password);
            message.Subject = Subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = MailText;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("coustomer.RealWealth@gmail.com", "RealWealth@2022");
            smtp.Credentials = new NetworkCredential("coustomer.RealWealth@gmail.com", "RealWealth@2022");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            message.To.Add(new MailAddress(MailId));
            smtp.Send(message);
        }
    }
}
