
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
    
public partial class DoctorShift
{

    public int Id { get; set; }

    public int DoctorId { get; set; }

    public string MorningStart { get; set; }

    public string MorningEnd { get; set; }

    public string AfternoonStart { get; set; }

    public string AfternoonEnd { get; set; }

    public int Slot { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public System.DateTime UpdatedDate { get; set; }

}

}
