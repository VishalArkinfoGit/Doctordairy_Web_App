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
            return View();
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

                    return RedirectToAction("Index", "Home");
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

        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id = 0)
        {
            if (Session["UserID"] != null)
            {
                if (id == 0)
                {
                    id = int.Parse(new EncryptDecrypt().Decrypt(Session["UserID"].ToString()));
                }

                PatientViewModel result = new PatientViewModel();

                using (var db = new ddiarydbEntities())
                {
                    result.Patient = db.Patient_Master.Where(x => x.Patient_Id == id).FirstOrDefault();

                    string file = result.Patient.Patient_photo != null ? (@"" + result.Patient.Patient_photo) : @"c:\temp\test.txt";

                    if (!System.IO.File.Exists(file))
                    {
                        var temp = result.Patient.Patient_name.Split(' ');
                        if (temp.Length > 1)
                        {
                            result.Patient.Patient_photo = temp[0].Substring(0, 1) + temp[1].Substring(0, 1);
                        }
                        else
                        {
                            result.Patient.Patient_photo = temp[0].Substring(0, 1);
                        }
                    }

                    var appointments = (from x in db.Appointments.AsEnumerable()
                                        where x.PatientId == id
                                        select x).ToList<Appointment>();

                    if (appointments != null)
                    {
                        result.Appointments = new List<AppointmentViewModel>();
                        foreach (var item in appointments)
                        {

                            var temp = new MappingService().Map<Appointment, AppointmentViewModel>(item);

                            var doctor = db.Doctor_Master.Where(x => x.Doctor_id == item.DoctorId).FirstOrDefault();
                            temp.Doctor = new MappingService().Map<Doctor_Master, DoctorViewModel>(doctor);

                            result.Appointments.Add(temp);
                        }
                    }
                }

                return View(result);
            }
            else
            {
                return RedirectToAction("Login", "Patient");
            }
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Patient/Edit/5
        [HttpGet]
        public ActionResult Edit(int id, string status)
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

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }

}
