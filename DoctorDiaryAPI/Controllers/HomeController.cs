using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoctorDiaryAPI;

namespace DoctorDiaryAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData.Clear();
            return View();
        }

        public ActionResult Dashboard()
        {
            TempData.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult Dashboard(Appointment appointment)
        {
            TempData["Mobile"] = appointment.PatientMobile;

            return RedirectToAction("TakeAppointment");
        }

        public JsonResult GenerateOTP()
        {
            string Result = string.Empty;
            string strMessage = string.Empty;
            string strNumber = string.Empty;

            strMessage = GenerateMsg();
            WebRequest request = WebRequest.Create("https://sms.com:27677/corporate_sms2/api/sendsms.jsp?msisdn=SimNumber&password=PWD&to=" + strNumber + "&text=" + strMessage + "&mask=Masking Name");
            request.Method = "POST";
            //request.ContentType = "text/xml";
            request.ContentType = "text/xml;charset=UTF-8";
            //request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            if (((HttpWebResponse)response).StatusDescription.Equals("OK"))
            {
                Result = "Message Send Successfully";
            }
            response.Close();


            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        private string GenerateMsg()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";
            string message = string.Empty;

            string characters = numbers;
            //if (rbType.SelectedItem.Value == "1")
            //{
            //    characters += alphabets + small_alphabets + numbers;
            //}

            string otp = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            message = "Your OTP is " + otp;
            return message;
        }

        [HttpGet]
        public ActionResult TakeAppointment()
        {
            if (TempData.ContainsKey("Mobile"))
            {
                var appointment = new Appointment()
                {
                    PatientMobile = TempData["Mobile"].ToString(),
                    DateStart = DateTime.Now.Date
                };
                using (var db = new ddiarydbEntities())
                {
                    if (db.DoctorShifts.Any())
                    {
                        var doctors = db.Doctor_Master.Where(x => x.Doctor_id > 0).Take(50).ToList();

                        ViewBag.Doctors = doctors;
                    }
                    else
                    {
                        ViewBag.Doctors = null;
                    }
                }

                return View(appointment);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult TakeAppointment(Appointment appointment)
        {
            TempData.Clear();

            appointment.CreatedDate = DateTime.Now;
            appointment.Status = "Pending";

            var startend = appointment.SessionId.Split('-');

            var date = Convert.ToDateTime(appointment.DateStart);

            var datetimeStart = date.ToShortDateString() + " " + startend[0];
            var datetimeEnd = date.ToShortDateString() + " " + startend[1];

            appointment.DateStart = DateTime.Parse(datetimeStart);
            appointment.DateEnd = DateTime.Parse(datetimeEnd);

            appointment.SessionId = appointment.DateStart.ToString("ddMMyyyyHHmm") + appointment.DateEnd.ToString("HHmm") + appointment.DoctorId.ToString();

            try
            {
                using (var db = new ddiarydbEntities())
                {
                    db.Appointments.Add(appointment);
                    db.SaveChanges();

                    var doctor = db.Doctor_Master.Where(x => x.Doctor_id == appointment.DoctorId).FirstOrDefault();

                    TempData["DoctorName"] = doctor.Doctor_name;
                }


                return RedirectToAction("RegisterAppointment", appointment);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something Wrong.!");
                return View(appointment);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RegisterAppointment(Appointment appointment)
        {
            return View(appointment);
        }
        public JsonResult GetDoctors()
        {
            var data = "";

            using (var db = new ddiarydbEntities())
            {
                if (db.DoctorShifts.Any())
                {
                    //var d = from s in db.DoctorShifts
                    //        join doc in db.Doctor_Master on s.DoctorId equals doc.Doctor_id
                    //        select new
                    //        {
                    //            DoctorId = doc.Doctor_id,
                    //            DoctorName = doc.Doctor_name,
                    //            Address = doc.Doctor_address,
                    //            ObTime = 10,
                    //            MorningStart = s.MorningStart,
                    //            MorningEnd = s.MorningEnd,
                    //            AfternoorStart = s.AfternoonStart,
                    //            AfternoonEnd = s.AfternoonEnd
                    //        };

                    //data = JsonConvert.SerializeObject(d);

                    var d = from s in db.DoctorShifts
                            join doc in db.Doctor_Master on s.DoctorId equals doc.Doctor_id
                            select new
                            {
                                DoctorId = doc.Doctor_id,
                                DoctorName = doc.Doctor_name,
                                Address = doc.Doctor_address
                            };

                    data = JsonConvert.SerializeObject(d);
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDoctorsShift(int id, string dateString)
        {
            var data = "";

            using (var db = new ddiarydbEntities())
            {
                if (db.DoctorShifts.Any())
                {
                    var docShifts = (from s in db.DoctorShifts
                                     where s.DoctorId == id
                                     select new
                                     {
                                         DoctorId = s.DoctorId,
                                         ObTime = 10,
                                         MorningStart = s.MorningStart,
                                         MorningEnd = s.MorningEnd,
                                         AfternoorStart = s.AfternoonStart,
                                         AfternoonEnd = s.AfternoonEnd
                                     }).FirstOrDefault();

                    var DATE = DateTime.Parse(dateString);

                    var bookedSlot = (from a in db.Appointments
                                      where a.DoctorId == id && (a.DateStart.Day == DATE.Day) && (a.DateStart.Month == DATE.Month) && (a.DateStart.Year == DATE.Year)
                                      select a.SessionId).ToList();

                    var x = new
                    {
                        docShifts,
                        bookedSlot
                    };

                    data = JsonConvert.SerializeObject(x);
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendOTP(string mobile)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (ddiarydbEntities db = new ddiarydbEntities())
                {

                    var result = db.OTPVerifications.FirstOrDefault(x => x.MobileNo == mobile);
                    if (result != null)
                    {
                        //returnData.message = "Successfull";
                        //returnData.status_code = Convert.ToInt32(Status.Sucess);
                        //returnData.data1 = result.OTP;

                        returnData = new DoctorController().SendOTP("", mobile, result.OTP);
                    }
                    else
                    {
                        string OTP = "";

                        Random r = new Random();

                        OTP = r.Next(1000, 9999).ToString();

                        returnData = new DoctorController().SendOTP("", mobile, OTP);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }

            return Json(JsonConvert.SerializeObject(returnData), JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyMobile(string mobile, string OTP)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (ddiarydbEntities db = new ddiarydbEntities())
                {
                    var result = db.OTPVerifications.FirstOrDefault(x => x.MobileNo == mobile);
                    if (result != null)
                    {
                        if (result.OTP == OTP)
                        {
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.Sucess);
                        }
                        else
                        {
                            returnData.message = "Please enter correct OTP.";
                            returnData.status_code = Convert.ToInt32(Status.Failed);
                        }
                    }
                    else
                    {
                        returnData.message = "Oops something went wrong!";
                        returnData.status_code = Convert.ToInt32(Status.Failed);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }
            return Json(JsonConvert.SerializeObject(returnData), JsonRequestBehavior.AllowGet);
        }
    }
}
