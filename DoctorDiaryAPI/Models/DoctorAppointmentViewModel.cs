﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class DoctorAppointmentViewModel
    {
        public DoctorViewModel Doctor { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
    }
}