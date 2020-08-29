using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class PatientViewModel
    {
        public string Patient_state { get; set; }

        public string Patient_photo { get; set; }

        public string Patient_name { get; set; }

        public string Patient_email { get; set; }

        public string Patient_contact { get; set; }

        public string Patient_city { get; set; }

        public string Patient_address { get; set; }

        public int Patient_Id { get; set; }

        public string Patient_Country { get; set; }

        public Nullable<System.DateTime> Reg_Date { get; set; }

        public Nullable<int> User_Id { get; set; }

        public string note { get; set; }

        public Nullable<decimal> age { get; set; }

        public string gender { get; set; }

        public string relation { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
    }
}