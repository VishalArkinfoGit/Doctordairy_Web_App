
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
    
public partial class Report_Problems_Mst
{

    public int Id { get; set; }

    public string Problem_Detail { get; set; }

    public Nullable<int> User_Id { get; set; }



    public virtual usr usr { get; set; }

}

}
