using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DoctorDiaryAPI
{
    public class Email
    {
        public static bool MailSend(string ToAdd, string emailSubject, string emailBody, string filename, string cc)
        {
            try
            {
                //var fromAddress = "ajaysinh.mail@gmail.com";  
                var fromAddress = "harshal@arkinfosoft.com";
                var toAddress = ToAdd;
                //const string fromPassword = "Ajay@123456";  
                const string fromPassword = "Harshal@123";
                string subject = emailSubject;
                string body = emailBody;
                System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                if (filename != "")
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(filename);
                    email.Attachments.Add(attachment);
                }
               
                email.To.Add(toAddress);
                email.From = new MailAddress(fromAddress);
                email.Subject = emailSubject;
                email.Body = emailBody;
                email.IsBodyHtml = true;
                //if (cc != "" && cc!= "noreply.allowme@gmail.com" && cc!= "noreply@allowme-admin.com")
                //{
                //    email.CC.Add("matthew@allow-me.com.au");
                //    email.CC.Add(cc);
                //}               

                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    //smtp.Host = "mail.arkinfosoft.com";
                    smtp.Port = 587;// 25;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.UseDefaultCredentials = true;
                    //smtp.Timeout = 20000;

                }
                smtp.Send(email);

            
                return true;

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                ErrHandler.WriteError(ex.Message, ex);
                return false;
            }
        }
    }
}
