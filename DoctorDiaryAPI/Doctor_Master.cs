
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace DoctorDiaryAPI
{

using System;
    using System.Collections.Generic;
    
public partial class Doctor_Master
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

    public Nullable<bool> IsActive { get; set; }

    public string Url { get; set; }



    public virtual usr usr { get; set; }

}

}
