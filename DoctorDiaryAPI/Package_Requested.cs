
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
    
public partial class Package_Requested
{

    public long Id { get; set; }

    public int User_Id { get; set; }

    public long Package_Id { get; set; }

    public Nullable<System.DateTime> Date { get; set; }

    public string isActive { get; set; }



    public virtual Package_Master Package_Master { get; set; }

    public virtual usr usr { get; set; }

}

}
