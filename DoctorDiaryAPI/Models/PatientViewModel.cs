using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class PatientViewModel
    {
        public Patient_Master Patient { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
    }
}