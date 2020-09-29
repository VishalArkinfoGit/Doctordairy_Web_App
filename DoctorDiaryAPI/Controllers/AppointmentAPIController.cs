using DoctorDiaryAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DoctorDiaryAPI.Controllers
{
    [System.Web.Http.Authorize]
    [RoutePrefix("api/AppointmentAPI")]
    public class AppointmentAPIController : ApiController
    {
        #region Appointment

        /// <summary>
        /// Purpose: Add a Appointment Details
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Created Appointment record </returns>
        /// <param name="appointment"> A Appointment Details </param>

        [HttpPost]
        [ActionName("Insert_Appointment")]
        public ReturnObject Insert_Appointment(Appointment appointment)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (var db = new ddiarydbEntities())
                {

                    if (db.Appointments.Any(x => x.DateStart == appointment.DateStart && x.DateEnd == appointment.DateEnd))
                    {
                        returnData.message = "This time slot booked.";
                        returnData.status_code = Convert.ToInt32(Status.AlreadyExists);
                        returnData.data1 = appointment;

                        return returnData;
                    }

                    appointment.CreatedDate = DateTime.Now;
                    appointment.UpdatedDate = DateTime.Now;

                    db.Appointments.Add(appointment);
                    db.SaveChanges();

                    Patient_Master patient = new Patient_Master();

                    patient = db.Patient_Master.Where(x => x.Patient_contact == appointment.PatientMobile).FirstOrDefault();

                    if (patient == null)
                    {
                        patient = new Patient_Master();
                        patient.Reg_Date = DateTime.Now;
                        patient.Patient_name = appointment.PatientName;
                        patient.Patient_contact = appointment.PatientMobile;
                        patient.relation = appointment.Relation;
                        patient.IsActive = false;
                        patient.User_Id = db.Doctor_Master.Where(x => x.Doctor_id == appointment.DoctorId).Select(x => x.User_id).FirstOrDefault();

                        db.Patient_Master.Add(patient);
                        db.SaveChanges();
                    }
                    appointment.PatientId = patient.Patient_Id;

                    db.Entry(appointment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    returnData.data1 = appointment;
                }

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }

            return returnData;
        }


        /// <summary>
        /// Purpose: Add a Appointment Details
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Created Appointment record </returns>
        /// <param name="appointment"> A Appointment Details </param>

        [HttpPost]
        [ActionName("Update_Appointment")]
        public ReturnObject Update_Appointment(Appointment appointment)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (var db = new ddiarydbEntities())
                {
                    var dbAppointment = db.Appointments.Where(x => x.Id == appointment.Id).FirstOrDefault();

                    if (db.Appointments.Any(x => x.Id != appointment.Id && x.DateStart == appointment.DateStart && x.DateEnd == appointment.DateEnd))
                    {
                        returnData.message = "This time slot booked.";
                        returnData.status_code = Convert.ToInt32(Status.AlreadyExists);
                        returnData.data1 = appointment;

                        return returnData;
                    }

                    dbAppointment.DateStart = appointment.DateStart;

                    dbAppointment.DateEnd = appointment.DateEnd;

                    dbAppointment.DoctorId = appointment.DoctorId;

                    dbAppointment.PatientId = appointment.PatientId;

                    dbAppointment.PatientName = appointment.PatientName;

                    dbAppointment.PatientMobile = appointment.PatientMobile;

                    dbAppointment.Relation = appointment.Relation;

                    dbAppointment.Status = appointment.Status;

                    dbAppointment.SessionId = appointment.SessionId;

                    dbAppointment.UpdatedDate = DateTime.Now;

                    db.Entry(dbAppointment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    returnData.data1 = dbAppointment;
                }

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }

            return returnData;
        }

        /// <summary>
        /// Purpose: Get Appointment Details
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Appointment Details </returns>
        /// <param name="id"> Appointment Id </param>

        //[HttpGet]
        //[ActionName("Get_Appointment")]
        //public ReturnObject Get_Appointment(int id)
        //{
        //    ReturnObject returnData = new ReturnObject();

        //    try
        //    {
        //        Appointment appointment = new Appointment();
        //        Doctor_Master doctor = new Doctor_Master();

        //        if (id > 0)
        //        {
        //            using (var db = new ddiarydbEntities())
        //            {
        //                appointment = (from s in db.Appointments
        //                               where s.Id == id
        //                               select s).FirstOrDefault();


        //                doctor = db.Doctor_Master.Where(x => x.Doctor_id == appointment.DoctorId).FirstOrDefault();

        //                doctor.usr = null;
        //            }

        //            returnData.message = "Successfull";
        //            returnData.status_code = Convert.ToInt32(Status.Sucess);
        //            returnData.data1 = appointment;
        //            returnData.data2 = doctor;
        //        }
        //        else
        //        {
        //            returnData.data1 = "Enter Doctor Id";
        //            returnData.message = "Oops something went wrong! ";
        //            returnData.status_code = Convert.ToInt32(Status.Failed);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex.Message, ex);
        //        returnData.data1 = ex;
        //        returnData.message = "Oops something went wrong! ";
        //        returnData.status_code = Convert.ToInt32(Status.Failed);
        //    }

        //    return returnData;
        //}

        /// <summary>
        /// Purpose: Get Appointment list
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Appointment list </returns>
        /// <param name="id"> Appointment Id </param>
        /// <param name="DoctorId"> Doctor Id </param>
        /// <param name="fromDate"> From Date </param>
        /// <param name="toDate"> To Date </param>

        [HttpGet]
        [ActionName("Get_Appointments")]
        public ReturnObject Get_Appointments(int id = 0, int DoctorId = 0, string fromDate = "", string toDate = "")
        {
            ReturnObject returnData = new ReturnObject();

            try
            {

                if (DoctorId > 0 && id == 0)
                {
                    using (var db = new ddiarydbEntities())
                    {
                        var doctor = db.Doctor_Master.Where(x => x.Doctor_id == DoctorId).FirstOrDefault();

                        var appointments = new List<Appointment>();

                        var FROMDATE = DateTime.Now;
                        var TODATE = DateTime.Now;

                        if (fromDate == "" && toDate == "")
                        {
                            FROMDATE = DateTime.Now;
                            TODATE = DateTime.Now;
                        }
                        else if (fromDate.Length > 0 && toDate == "")
                        {
                            FROMDATE = Convert.ToDateTime(fromDate);
                            TODATE = Convert.ToDateTime(fromDate);
                        }
                        else
                        {
                            FROMDATE = Convert.ToDateTime(fromDate);
                            TODATE = Convert.ToDateTime(toDate);
                        }

                        TimeSpan ssts = new TimeSpan(0, 0, 0);
                        FROMDATE = FROMDATE.Date + ssts;

                        TimeSpan ests = new TimeSpan(23, 59, 59);
                        TODATE = TODATE.Date + ests;


                        try
                        {
                            appointments = (from s in db.Appointments
                                            where s.DoctorId == DoctorId && s.DateStart >= FROMDATE && s.DateEnd <= TODATE
                                            select s).OrderByDescending(x => x.DateStart).ToList<Appointment>();

                        }
                        catch (Exception)
                        {
                            appointments = null;
                        }

                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        doctor.usr = new usr();
                        returnData.data1 = doctor;
                        returnData.data2 = appointments;
                    }

                }
                else if (id > 0 && DoctorId == 0)
                {

                    using (var db = new ddiarydbEntities())
                    {
                        var appointment = (from s in db.Appointments
                                           where s.Id == id
                                           select s).FirstOrDefault();

                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        returnData.data1 = appointment;
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

            return returnData;
        }

        /// <summary>
        /// Purpose: Update Appointment 
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Appointment Details </returns>
        /// <param name="id"> Appointment Id </param>
        /// <param name="status"> Status of Appointment</param>
        /// <param name="msg"> Message </param>

        [HttpPost]
        [ActionName("UpdateStatus_Appointment")]
        public ReturnObject Update_Appointment(int id, string status, string msg)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (var db = new ddiarydbEntities())
                {
                    var appointment = (from s in db.Appointments
                                       where s.Id == id
                                       select s).FirstOrDefault();

                    if (appointment.DateStart > DateTime.Now)
                    {
                        appointment.Status = status;
                    }
                    else
                    {
                        appointment.Status = "Cancel";
                    }

                    //appointment.CreatedDate = DateTime.Now;
                    appointment.UpdatedDate = DateTime.Now;

                    db.Entry(appointment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var sms = "";
                    var doctor = db.Doctor_Master.Where(x => x.Doctor_id == appointment.DoctorId).FirstOrDefault();

                    if (appointment.Status == "Accept")
                    {
                        sms = "Your Appointment is Accepted Successfully! Appointment booked with Dr." + doctor.Doctor_name + " on " +
                        appointment.DateStart.ToString("dd MMM yyyy") + " " + appointment.DateStart.ToString("hh:mm tt") + " to " + appointment.DateEnd.ToString("hh:mm tt");

                        Patient_Master patient = db.Patient_Master.Where(x => x.Patient_Id == appointment.PatientId).FirstOrDefault();

                        if (patient != null)
                        {
                            patient.IsActive = true;

                            db.Entry(patient).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                        DoctorPatient_Master doctorPatient = new DoctorPatient_Master();

                        doctorPatient = (from x in db.DoctorPatient_Master.AsEnumerable()
                                         where x.PatientId == appointment.PatientId && x.DoctorId == appointment.DoctorId
                                         select x).FirstOrDefault();

                        if (doctorPatient == null)
                        {
                            doctorPatient = new DoctorPatient_Master();
                            doctorPatient.DoctorId = appointment.DoctorId;
                            doctorPatient.PatientId = appointment.PatientId;
                            doctorPatient.IsActive = true;
                            doctorPatient.CreatedDate = DateTime.Now;
                            doctorPatient.UpdatedDate = DateTime.Now;

                            db.DoctorPatient_Master.Add(doctorPatient);
                            db.SaveChanges();
                        }
                        else
                        {
                            doctorPatient.UpdatedDate = DateTime.Now;

                            db.Entry(doctorPatient).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    else if (appointment.Status == "Cancel")
                    {
                        sms = "Your Appointment booked on " +
                        appointment.DateStart.ToString("dd MMM yyyy") + " " + appointment.DateStart.ToString("hh:mm tt") + " to " + appointment.DateEnd.ToString("hh:mm tt") +
                        " is Canceled by Dr." + doctor.Doctor_name + ".";
                        sms = msg != "" ? (sms + " Reason: " + msg) : sms;
                    }

                    SmsSend.Send(appointment.PatientMobile, sms);


                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    returnData.data1 = appointment;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }

            return returnData;
        }

        #endregion

        #region Doctor

        /// <summary>
        /// Purpose: Get Doctor Details
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Doctor Details </returns>
        /// <param name="id"> Doctor Id </param>

        [HttpGet]
        [ActionName("Get_Doctor")]
        public ReturnObject Get_Doctor(int id)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {

                using (var db = new ddiarydbEntities())
                {
                    Doctor_Master doctor = db.Doctor_Master.Where(x => x.Doctor_id == id).FirstOrDefault();

                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    returnData.data1 = doctor;

                }

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }

            return returnData;
        }


        /// <summary>
        /// Purpose: Find Doctor
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Doctor Details in List </returns>
        /// <param name="str"> Doctor Name </param>

        [HttpGet]
        [ActionName("FindDoctorsByString")]
        public ReturnObject FindDoctorsByString(string str = "")
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (var db = new ddiarydbEntities())
                {

                    var list = new List<DoctorViewModel>();
                    var datalist = (from doc in db.Doctor_Master
                                    where doc.IsActive == true && doc.Doctor_name.ToLower().StartsWith(str.ToLower())
                                    select doc).ToList();

                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    returnData.data1 = datalist;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }

            return returnData;
        }

        /// <summary>
        /// Purpose: Find Doctors Shift
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Doctor Shift Details </returns>
        /// <param name="id"> Doctor Id </param>

        [HttpGet]
        [ActionName("Get_DoctorsShift")]
        public ReturnObject Get_DoctorsShift(int id = 0)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (var db = new ddiarydbEntities())
                {
                    if (id > 0)
                    {
                        var docShifts = (from s in db.DoctorShifts
                                         where s.DoctorId == id
                                         select new
                                         {
                                             DoctorId = s.DoctorId,
                                             ObTime = s.Slot,
                                             MorningStart = s.MorningStart,
                                             MorningEnd = s.MorningEnd,
                                             AfternoorStart = s.AfternoonStart,
                                             AfternoonEnd = s.AfternoonEnd
                                         }).FirstOrDefault();

                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        returnData.data1 = docShifts;
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

            return returnData;
        }


        /// <summary>
        /// Purpose: Find Booked Slot
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Doctor Booked Slot Details </returns>
        /// <param name="id"> Doctor Id </param>
        /// <param name="DATE"> Date </param>

        [HttpGet]
        [ActionName("Get_DoctorBookedSlot")]
        public ReturnObject Get_DoctorBookedSlot(int id, DateTime DATE)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                using (var db = new ddiarydbEntities())
                {
                    if (id > 0)
                    {
                        var bookedSlot = (from a in db.Appointments
                                          where a.DoctorId == id && (a.DateStart.Day == DATE.Day) && (a.DateStart.Month == DATE.Month) && (a.DateStart.Year == DATE.Year)
                                          select a.SessionId).ToList();

                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        returnData.data1 = bookedSlot;
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

            return returnData;
        }

        #endregion

        /// <summary>
        /// Purpose: Find Patient details By Mobile Number
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Patient details </returns>
        /// <param name="mobile"> mobile number </param>

        [HttpGet]
        [ActionName("FindPatientByMobile")]
        public ReturnObject FindPatientByMobile(string mobile = "")
        {
            ReturnObject returnData = new ReturnObject();

            if (mobile.Length == 10)
            {

                try
                {
                    using (var db = new ddiarydbEntities())
                    {

                        var patient = (from x in db.Patient_Master
                                       where x.Patient_contact == mobile
                                       select x).FirstOrDefault();

                        if (patient != null)
                        {
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.Sucess);
                            returnData.data1 = new
                            {
                                patient.Patient_name,
                                patient.relation,
                                patient.Patient_contact
                            };
                        }
                        else
                        {
                            returnData.message = "Mobile no is not found.";
                            returnData.status_code = Convert.ToInt32(Status.NotFound);
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

            }
            else
            {
                returnData.message = "Please enter valid mobile no.";
                returnData.status_code = Convert.ToInt32(Status.Failed);
            }
            return returnData;
        }

        /// <summary>
        /// Purpose: Verify Mobile Number with OTP
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> Message : Successfull or not </returns>
        /// <param name="mobile"> mobile number </param>
        /// <param name="OTP"> OTP - 4 Digits number </param>

        [HttpGet]
        [ActionName("VerifyMobile")]
        public ReturnObject VerifyMobile(string mobile, string OTP)
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
                            result.OTP = "";
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.Sucess);
                            returnData.data1 = result;
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
            return returnData;
        }
    }
}
