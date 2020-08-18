using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class PatientLoginViewModel
    {
        [Required(ErrorMessage = "Mobile No. is Required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Please Enter a valid Phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "OTP is Required.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Please Enter a valid OTP")]
        [RegularExpression(@"^\(?([0-9]{4})$", ErrorMessage = "Please Enter a valid OTP")]
        [Display(Name = "OTP")]
        public string OTP { get; set; }

        public DateTime dateTime { get; set; }
    }
}