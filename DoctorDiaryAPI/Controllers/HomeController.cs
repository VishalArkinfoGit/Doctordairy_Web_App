using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoctorDiaryAPI;
using DoctorDiaryAPI.Models;

namespace DoctorDiaryAPI.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        #region Appointment Booking

        /// <summary>
        /// Purpose: Display View for book an appointment
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Booking View </returns>
        /// <param name="id"> Doctor Id </param>

        [HttpGet]
        public ActionResult Booking(string doctorId, string appointmentId = "")
        {
            AppointmentViewModel appointment = new AppointmentViewModel();

            using (var db = new ddiarydbEntities())
            {
                if (appointmentId == "")
                {
                    int Doctor_Id = int.Parse(new EncryptDecrypt().Decrypt(doctorId));

                    var doctor = db.Doctor_Master.Where(x => x.Doctor_id == Doctor_Id).FirstOrDefault();

                    if (doctor != null)
                    {
                        appointment.Doctor = new DoctorViewModel().DoctorModel_to_ViewModel(doctor);
                    }
                    else
                    {
                        appointment.Doctor = null;
                    }

                    appointment.DateStart = DateTime.Now;
                    appointment.Relation = "Self";
                }
                else
                {
                    appointment = new AppointmentViewModel();

                    appointment.Id = int.Parse(new EncryptDecrypt().Decrypt(appointmentId));

                    var dbAppointment = (from s in db.Appointments
                                         where s.Id == appointment.Id
                                         select s).FirstOrDefault();

                    appointment = new MappingService().Map<Appointment, AppointmentViewModel>(dbAppointment);

                    var doctor = db.Doctor_Master.Where(x => x.Doctor_id == dbAppointment.DoctorId).FirstOrDefault();

                    if (doctor != null)
                    {
                        appointment.Doctor = new DoctorViewModel().DoctorModel_to_ViewModel(doctor);
                    }
                    else
                    {
                        appointment.Doctor = null;
                    }
                }
            }

            return View(appointment);
        }


        /// <summary>
        /// Purpose: Save Details of an appointment
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Appointment Details </returns>
        /// <param name="appointment"> Appointment Details </param>

        [HttpPost]
        [ActionName("Booking")]
        public ActionResult Booking(AppointmentViewModel appointment)
        {
            appointment.Status = "Pending";

            var startend = appointment.SessionId.Split('-');

            var date = Convert.ToDateTime(appointment.DateStart);

            var datetimeStart = date.ToShortDateString() + " " + startend[0];
            var datetimeEnd = date.ToShortDateString() + " " + startend[1];

            appointment.DateStart = DateTime.Parse(datetimeStart);
            appointment.DateEnd = DateTime.Parse(datetimeEnd);

            appointment.SessionId = appointment.DateStart.ToString("ddMMyyyyHHmm") + appointment.DateEnd.ToString("HHmm");

            try
            {
                using (var db = new ddiarydbEntities())
                {
                    Appointment obj = new MappingService().Map<AppointmentViewModel, Appointment>(appointment);
                    obj.DoctorId = int.Parse(new EncryptDecrypt().Decrypt(appointment.Doctor.DoctorId_Encrypt));

                    //db.Appointments.Add(obj);
                    //db.SaveChanges();
                    AppointmentAPIController aController = new AppointmentAPIController();
                    ReturnObject returnObject = new ReturnObject();

                    if (appointment.Id != 0)
                    {
                        returnObject = aController.Update_Appointment(obj);
                    }
                    else
                    {
                        returnObject = aController.Insert_Appointment(obj);
                    }

                    try
                    {
                        if (returnObject != null)
                        {
                            Appointment a = (Appointment)returnObject.data1;

                            var doctor = db.Doctor_Master.Where(x => x.Doctor_id == obj.DoctorId).FirstOrDefault();

                            TempData["DoctorName"] = doctor.Doctor_name;

                            string id = new EncryptDecrypt().Encrypt(a.Id.ToString());

                            return Redirect("AppointmentDetails?id=" + id);
                        }

                        return Redirect("Booking?id=" + appointment.Doctor.DoctorId_Encrypt);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Something Wrong.!");
                        return View(appointment);
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Something Wrong.!");
                return View(appointment);
            }
        }

        #endregion


        #region Appointment Details

        /// <summary>
        /// Purpose: Display an appointment details
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Appointment Details View </returns>
        /// <param name="id"> Appointment Id </param>

        [HttpGet]
        [ActionName("AppointmentDetails")]
        public ActionResult AppointmentDetails(string id)
        {
            int aid = int.Parse(new EncryptDecrypt().Decrypt(id));

            Appointment appointment = new Appointment();

            if (aid > 0)
            {
                using (var db = new ddiarydbEntities())
                {
                    appointment = (from s in db.Appointments
                                   where s.Id == aid
                                   select s).FirstOrDefault();


                    var doctor = db.Doctor_Master.Where(x => x.Doctor_id == appointment.DoctorId).FirstOrDefault();

                    TempData["DoctorName"] = doctor.Doctor_name;
                }

            }

            return View(appointment);
        }


        /// <summary>
        /// Purpose: Display appointments details using Doctor id, Appointment Id, Date - From and To
        ///          Update status of Appointment
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Appointment Details </returns>
        /// <param name="id"> Doctor Id </param>
        /// <param name="AppointmentId"> Appointment Id </param>
        /// <param name="status"> Appointment status </param>
        /// <param name="fromDate"> From Date </param>
        /// <param name="toDate"> To Date </param>


        [HttpGet]
        [ActionName("Appointment")]
        public ActionResult Appointment(string id = "", int AppointmentId = 0, string status = "", string fromDate = "", string toDate = "", string msg = "")
        {
            int DoctorId = int.Parse(new EncryptDecrypt().Decrypt(id));

            DoctorAppointmentViewModel data = new DoctorAppointmentViewModel();

            AppointmentAPIController aController = new AppointmentAPIController();
            ReturnObject ro = new ReturnObject();

            using (var db = new ddiarydbEntities())
            {
                var doctor = db.Doctor_Master.Where(x => x.Doctor_id == DoctorId).FirstOrDefault();

                if (AppointmentId > 0)
                {
                    ro = aController.Update_Appointment(AppointmentId, status, msg);
                }

                var appointments = new List<Appointment>();

                ro = aController.Get_Appointments(0, DoctorId, fromDate, toDate);

                try
                {
                    appointments = (List<Appointment>)ro.data2;
                }
                catch (Exception)
                {
                    appointments = null;
                }

                if (doctor != null)
                {

                    data.Doctor = new DoctorViewModel().DoctorModel_to_ViewModel(doctor);

                    data.Appointments = new List<AppointmentViewModel>();
                    foreach (var item in appointments)
                    {
                        var temp = item.PatientName.Split(' ');
                        if (temp.Length > 1)
                        {
                            item.SessionId = temp[0].Substring(0, 1) + temp[1].Substring(0, 1);
                        }
                        else
                        {
                            item.SessionId = temp[0].Substring(0, 1);
                        }

                        data.Appointments.Add(new AppointmentViewModel().AppointmentModel_to_ViewModel(item));

                    }

                }
            }

            return View(data);
        }

        #endregion

        #region Doctor 

        /// <summary>
        /// Purpose: Display an Doctor Details
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Doctor Details </returns>
        /// <param name="id"> Doctor Id </param>

        [HttpGet]
        [ActionName("DoctorProfile")]
        public ActionResult DoctorProfile(string id)
        {
            int DoctorId = int.Parse(new EncryptDecrypt().Decrypt(id));

            DoctorViewModel data = new DoctorViewModel();

            using (var db = new ddiarydbEntities())
            {
                Doctor_Master doctor = db.Doctor_Master.Where(x => x.Doctor_id == DoctorId).FirstOrDefault();

                data = new DoctorViewModel().DoctorModel_to_ViewModel(doctor);
            }

            return View(data);
        }

        /// <summary>
        /// Purpose: Find Doctor
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Doctor Details in List </returns>
        /// <param name="str"> Doctor Name </param>

        public JsonResult GetDoctors(string str = "")
        {
            var data = "";
            AppointmentAPIController aController = new AppointmentAPIController();
            ReturnObject ro = new ReturnObject();

            if (str.Length > 2)
            {
                var list = new List<DoctorViewModel>();

                ro = aController.FindDoctorsByString(str);

                List<Doctor_Master> datalist = new List<Doctor_Master>();
                try
                {
                    datalist = (List<Doctor_Master>)ro.data1;

                    foreach (var doctor in datalist)
                    {
                        list.Add(new DoctorViewModel()
                        {
                            Doctor_id = 0,
                            Doctor_name = doctor.Doctor_name,
                            Doctor_address = doctor.Doctor_address,
                            DoctorId_Encrypt = new EncryptDecrypt().Encrypt(doctor.Doctor_id.ToString())
                        });
                    }

                }
                catch (Exception)
                {
                    datalist = null;
                }

                data = JsonConvert.SerializeObject(list);

            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Purpose: Find Doctors Shift
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Doctor Shift Details </returns>
        /// <param name="id"> Doctor Id </param>

        public JsonResult GetDoctorsShift(string DoctorId, string dateString)
        {
            int id = int.Parse(new EncryptDecrypt().Decrypt(DoctorId));

            var data = "";
            AppointmentAPIController aController = new AppointmentAPIController();
            ReturnObject ro = new ReturnObject();


            ro = aController.Get_DoctorsShift(id);

            try
            {
                var docShifts = ro.data1;

                var DATE = DateTime.Parse(dateString);

                ro = aController.Get_DoctorBookedSlot(id, DATE);

                var bookedSlot = ro.data1;

                var x = new
                {
                    docShifts,
                    bookedSlot
                };

                data = JsonConvert.SerializeObject(x);
            }
            catch (Exception ex)
            {
                var x = "";

                data = JsonConvert.SerializeObject(x);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Message

        /// <summary>
        /// Purpose: Send OTP
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> OTP </returns>
        /// <param name="mobile"> Mobile No. </param>

        public JsonResult SendOTP(string mobile)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (ddiarydbEntities db = new ddiarydbEntities())
                {

                    //var result = db.OTPVerifications.FirstOrDefault(x => x.MobileNo == mobile);
                    //if (result != null)
                    //{
                    //    //returnData.message = "Successfull";
                    //    //returnData.status_code = Convert.ToInt32(Status.Sucess);
                    //    //returnData.data1 = result.OTP;

                    //    returnData = new DoctorController().SendOTP("", mobile, result.OTP);
                    //}
                    //else
                    //{
                    string OTP = "";

                    Random r = new Random();

                    OTP = r.Next(1000, 9999).ToString();

                    returnData = new DoctorController().SendOTP("", mobile, OTP);
                    //}
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


        /// <summary>
        /// Purpose: Verify Mobile No.
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> OTP Verify or not </returns>
        /// <param name="mobile"> Mobile No. </param>
        /// <param name="OTP"> OTP </param>

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

        #endregion

        /// <summary>
        /// Purpose: Find Patient details By Mobile Number
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Patient details </returns>
        /// <param name="mobile"> mobile number </param>

        public JsonResult FindPatientByMobile(string mobile)
        {
            ReturnObject returnData = new ReturnObject();

            AppointmentAPIController aController = new AppointmentAPIController();

            returnData = aController.FindPatientByMobile(mobile);

            var data = new
            {
                message = returnData.message,
                status_code = returnData.status_code,
                data1 = ((returnData.data1 != null) ? JsonConvert.SerializeObject(returnData.data1) : "")
            };

            return Json(JsonConvert.SerializeObject(data), JsonRequestBehavior.AllowGet);

        }

        #region Encrypt and Decrypt

        /// <summary>
        /// Purpose: Encrypt entered value
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Encrypted string </returns>
        /// <param name="value"> Anything in String format to encrypt </param>

        public JsonResult EncryptData(string value)
        {
            EncryptDecrypt ed = new EncryptDecrypt();
            string encryptedString = ed.Encrypt(value);

            return Json(JsonConvert.SerializeObject(encryptedString), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Purpose: Decrypt entered value
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Decrypted or Original value </returns>
        /// <param name="str"> Encrypted string in String format to decrypt </param>

        public int DecryptData(string str)
        {
            EncryptDecrypt ed = new EncryptDecrypt();
            string decryptedString = ed.Decrypt(str);

            int id = int.Parse(decryptedString);

            return id;
        }

        #endregion
    }
}
