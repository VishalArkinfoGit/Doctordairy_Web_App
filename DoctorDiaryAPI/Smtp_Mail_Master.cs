
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
    
public partial class Smtp_Mail_Master
{

    public int Id { get; set; }

    public int Port_No { get; set; }

    public string From_Email_Address { get; set; }

    public string Credential_Email_Address { get; set; }

    public string Credential_Password { get; set; }

    public string Display_name { get; set; }

    public string Host_name { get; set; }

    public Nullable<bool> EnableSSL { get; set; }

    public Nullable<bool> IsActive { get; set; }

}

}
