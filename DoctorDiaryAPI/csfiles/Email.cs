using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
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
                SmtpSection smtpSettings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                var fromAddress = smtpSettings.From;

                //created object of SmtpClient details and provides server details
                using (SmtpClient smtp = new SmtpClient())
                {
                    //smtp.Host = "mail.arkinfosoft.com";
                    smtp.Host = smtpSettings.Network.Host;
                    smtp.Port = smtpSettings.Network.Port;
                    smtp.EnableSsl = smtpSettings.Network.EnableSsl;
                    smtp.DeliveryMethod = smtpSettings.DeliveryMethod;
                    smtp.UseDefaultCredentials = smtpSettings.Network.DefaultCredentials;

                    //Server Credentials
                    NetworkCredential NC = new NetworkCredential();
                    NC.UserName = smtpSettings.Network.UserName;
                    NC.Password = smtpSettings.Network.Password;

                    //assigned credetial details to server
                    smtp.Credentials = NC;

                    //create sender address
                    MailAddress from = new MailAddress(fromAddress, "Doctor Diary");

                    //if (fromEmail != "")
                    //{
                    //    from = new MailAddress(fromAddress, "App User");
                    //}

                    //create receiver address
                    MailAddress receiver = new MailAddress(ToAdd, "Testing");

                    MailMessage message = new MailMessage(from, receiver);
                    message.Subject = emailSubject.Trim();
                    message.Body = emailBody.Trim();
                    message.IsBodyHtml = true;

                    if (filename != "")
                    {
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(filename);
                        message.Attachments.Add(attachment);
                    }

                    if (cc != "")
                    {
                        if (cc.Contains('|'))
                        {
                            var emails = cc.Split('|');

                            foreach (string emailAddress in emails)
                            {
                                message.CC.Add(emailAddress.Trim());
                            }
                        }
                        else if (cc.Contains('@'))
                        {
                            message.CC.Add(cc.Trim());
                        }
                    }

                    //sends the email
                    smtp.Send(message);

                    return true;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                ErrHandler.WriteError(ex.Message, ex);
                return false;
            }

            //try
            //{
            //    //var fromAddress = "ajaysinh.mail@gmail.com";  
            //    //var fromAddress = "harshal@arkinfosoft.com";
            //    var fromAddress = "doctordairy@arkinfosoft.com";
            //    var toAddress = ToAdd;
            //    //const string fromPassword = "Ajay@123456";  
            //    const string fromPassword = "Harshal@123";
            //    string subject = emailSubject;
            //    string body = emailBody;
            //    System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            //    if (filename != "")
            //    {
            //        System.Net.Mail.Attachment attachment;
            //        attachment = new System.Net.Mail.Attachment(filename);
            //        email.Attachments.Add(attachment);
            //    }

            //    email.To.Add(ToAdd);
            //    email.From = new MailAddress(fromAddress);
            //    email.Subject = emailSubject;
            //    email.Body = emailBody;
            //    email.IsBodyHtml = true;
            //    //if (cc != "" && cc!= "noreply.allowme@gmail.com" && cc!= "noreply@allowme-admin.com")
            //    //{
            //    //    email.CC.Add("matthew@allow-me.com.au");
            //    //    email.CC.Add(cc);
            //    //}               

            //    var smtp = new System.Net.Mail.SmtpClient();
            //    {
            //        smtp.Host = "smtp.gmail.com";
            //        //smtp.Host = "mail.arkinfosoft.com";
            //        smtp.Port = 587;// 25;
            //        smtp.EnableSsl = true;
            //        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //        smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
            //        smtp.UseDefaultCredentials = true;
            //        //smtp.Timeout = 20000;

            //    }
            //    smtp.Send(email);


            //    return true;

            //}
            //catch (Exception ex)
            //{
            //    //Console.WriteLine(ex.ToString());
            //    ErrHandler.WriteError(ex.Message, ex);
            //    return false;
            //}
        }
    }
}
