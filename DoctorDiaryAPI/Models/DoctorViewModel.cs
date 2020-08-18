using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class DoctorViewModel
    {
        public Nullable<System.DateTime> Reg_date { get; set; }

        public int User_id { get; set; }

        public string Doctor_state { get; set; }

        public string Doctor_photo { get; set; }

        public string Doctor_name { get; set; }

        public int Doctor_id { get; set; }

        public string Doctor_exp { get; set; }

        public string Doctor_email { get; set; }

        public string Doctor_degree { get; set; }

        public string Doctor_country { get; set; }

        public string Doctor_contact { get; set; }

        public string Doctor_city { get; set; }

        public string Doctor_address { get; set; }

        public string Clinic_name { get; set; }

        public string Gender { get; set; }
        public string Url { get; set; }

        public Nullable<bool> IsActive { get; set; }

        public string DoctorId_Encrypt { get; set; }

        public DoctorViewModel DoctorModel_to_ViewModel(Doctor_Master doctor)
        {
            return new DoctorViewModel()
            {
                Reg_date = doctor.Reg_date,

                User_id = 0,

                Doctor_state = doctor.Doctor_state,

                Doctor_photo = doctor.Doctor_photo,

                Doctor_name = doctor.Doctor_name,

                Doctor_id = 0,

                Doctor_exp = doctor.Doctor_exp,

                Doctor_email = doctor.Doctor_email,

                Doctor_degree = doctor.Doctor_degree,

                Doctor_country = doctor.Doctor_country,

                Doctor_contact = doctor.Doctor_contact,

                Doctor_city = doctor.Doctor_city,

                Doctor_address = doctor.Doctor_address,

                Clinic_name = doctor.Clinic_name,

                Gender = doctor.Gender,

                Url = doctor.Url,

                IsActive = doctor.IsActive,

                DoctorId_Encrypt = new EncryptDecrypt().Encrypt(doctor.Doctor_id.ToString())
            };
        }
    }
}