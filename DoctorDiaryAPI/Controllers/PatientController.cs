using DoctorDiaryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorDiaryAPI.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Login()
        {
            Session.Clear();
            PatientLoginViewModel obj = new PatientLoginViewModel();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Login(PatientLoginViewModel obj)
        {
            obj.dateTime = DateTime.Now;

            if (obj.OTP != null)
            {
                ReturnObject ro = new AppointmentAPIController().VerifyMobile(obj.MobileNo, obj.OTP);

                if (ro.status_code == 1)
                {
                    using (var db = new ddiarydbEntities())
                    {
                        var patient = db.Patient_Master.Where(x => x.Patient_contact == obj.MobileNo).FirstOrDefault();

                        if (patient != null)
                        {
                            Session["UserID"] = new EncryptDecrypt().Encrypt(patient.Patient_Id.ToString()); ;
                            Session["UserName"] = patient.Patient_name.ToString();
                        }
                        else
                        {
                            Session["UserMobile"] = obj.MobileNo;
                        }
                    }

                    return RedirectToAction("Details");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please Enter a valid OTP.");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please Enter a OTP.");
            }

            return View(obj);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id = 0, string status = "")
        {
            if (id == 0)
            {
                if (Session["UserID"] != null)
                {
                    id = int.Parse(new EncryptDecrypt().Decrypt(Session["UserID"].ToString()));
                }
                else
                {
                    //id = 31223;
                    return RedirectToAction("Login");
                }
            }

            PatientViewModel result = new PatientViewModel();

            using (var db = new ddiarydbEntities())
            {
                var patient = db.Patient_Master.Where(x => x.Patient_Id == id).FirstOrDefault();

                result = new MappingService().Map<Patient_Master, PatientViewModel>(patient);

                //result.Patient_photo = new PhotoPathTextService().IsAvailable(result.Patient_photo, result.Patient_name);

                var appointments = (from x in db.Appointments.AsEnumerable()
                                    where x.PatientId == id
                                    select x).OrderByDescending(x => x.DateStart).ToList<Appointment>();

                if (appointments != null)
                {
                    result.Appointments = new List<AppointmentViewModel>();
                    foreach (var item in appointments)
                    {
                        if (item.DateStart < DateTime.Now)
                        {
                            item.Status = "Cancel";
                            item.UpdatedDate = DateTime.Now;

                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                        var temp = new MappingService().Map<Appointment, AppointmentViewModel>(item);

                        var doctor = db.Doctor_Master.Where(x => x.Doctor_id == item.DoctorId).FirstOrDefault();
                        temp.Doctor = new MappingService().Map<Doctor_Master, DoctorViewModel>(doctor);
                        temp.Doctor.DoctorId_Encrypt = new EncryptDecrypt().Encrypt(doctor.Doctor_id.ToString());
                        temp.AppointmentId_Encrypt = new EncryptDecrypt().Encrypt(temp.Id.ToString());

                        result.Appointments.Add(temp);
                    }
                }
            }

            return View(result);
        }

        // GET: Patient/Create
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                if (Session["UserID"] != null)
                {
                    id = int.Parse(new EncryptDecrypt().Decrypt(Session["UserID"].ToString()));
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            PatientViewModel obj = new PatientViewModel();

            using (var db = new ddiarydbEntities())
            {
                var patient = db.Patient_Master.Where(x => x.Patient_Id == id).FirstOrDefault();

                obj = new MappingService().Map<Patient_Master, PatientViewModel>(patient);

                //obj.Patient_photo = new PhotoPathTextService().IsAvailable(obj.Patient_photo, obj.Patient_name);
            }

            return View(obj);
        }

        // POST: Patient/Edit
        [HttpPost]
        public ActionResult Edit(PatientViewModel obj)
        {
            try
            {
                csPatient csPatient = new MappingService().Map<PatientViewModel, csPatient>(obj);

                var res = new DoctorController().Update_Patient(csPatient);

                if (res.status_code == 1)
                {
                    return RedirectToAction("Details");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Oops something went wrong! ");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(obj);
        }

        [HttpGet]
        public ActionResult Update_Status(int id, string status)
        {
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
                        appointment.UpdatedDate = DateTime.Now;

                        db.Entry(appointment).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch
            {
            }
            return RedirectToAction("Details");
        }

    }

}
