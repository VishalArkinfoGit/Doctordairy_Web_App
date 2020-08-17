using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class DoctorAppointmentViewModel
    {
        public Doctor_Master Doctor { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}