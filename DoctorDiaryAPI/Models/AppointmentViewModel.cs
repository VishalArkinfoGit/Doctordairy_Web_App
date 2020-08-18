using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public System.DateTime DateStart { get; set; }

        public System.DateTime DateEnd { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient name is Required.")]
        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Special Charactor and numbers not allowed.")]
        [Display(Name = "Patient name")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Mobile No. is Required.")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Mobile number")]
        [Display(Name = "Mobile No.")]
        public string PatientMobile { get; set; }

        [Required(ErrorMessage = "Please Select Relation.")]
        [StringLength(12, MinimumLength = 4)]
        [DataType(DataType.Text)]
        public string Relation { get; set; }

        public string Status { get; set; }


        [Required(ErrorMessage = "Please Select Time Slot.")]
        [StringLength(11, MinimumLength = 11)]
        [DataType(DataType.Text)]
        public string SessionId { get; set; }

        public System.DateTime CreatedDate { get; set; }


        public System.DateTime UpdatedDate { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public DoctorViewModel Doctor { get; set; }

        public AppointmentViewModel AppointmentModel_to_ViewModel(Appointment appointment)
        {
            return new AppointmentViewModel()
            {
                Id = appointment.Id,

                DateStart = appointment.DateStart,

                DateEnd = appointment.DateEnd,

                DoctorId = appointment.DoctorId,

                PatientName = appointment.PatientName,

                PatientMobile = appointment.PatientMobile,

                Relation = appointment.Relation,

                Status = appointment.Status,

                SessionId = appointment.SessionId,

                CreatedDate = appointment.CreatedDate
            };
        }

        public enum Relations
        {
            Self,
            Child,
            Wife,
            Gardian,
            Other
        }
    }
}