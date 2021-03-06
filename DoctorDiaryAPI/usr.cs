
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
    
public partial class usr
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public usr()
    {

        this.Doctor_Master = new HashSet<Doctor_Master>();

        this.Injury_Photos_Master = new HashSet<Injury_Photos_Master>();

        this.monthly_sms = new HashSet<monthly_sms>();

        this.Package_Assigned = new HashSet<Package_Assigned>();

        this.Package_Requested = new HashSet<Package_Requested>();

        this.Patient_Master = new HashSet<Patient_Master>();

        this.Report_Problems_Mst = new HashSet<Report_Problems_Mst>();

        this.SMS_Master = new HashSet<SMS_Master>();

        this.Treatment_Master = new HashSet<Treatment_Master>();

    }


    public string Email { get; set; }

    public string Firstname { get; set; }

    public string Gender { get; set; }

    public int Id { get; set; }

    public string Lastname { get; set; }

    public string passwd { get; set; }

    public Nullable<int> AccountId { get; set; }

    public Nullable<bool> IsActive { get; set; }

    public string token_id { get; set; }

    public Nullable<System.DateTime> date { get; set; }

    public string Provider { get; set; }

    public string ProviderId { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Doctor_Master> Doctor_Master { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Injury_Photos_Master> Injury_Photos_Master { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<monthly_sms> monthly_sms { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Package_Assigned> Package_Assigned { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Package_Requested> Package_Requested { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Patient_Master> Patient_Master { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Report_Problems_Mst> Report_Problems_Mst { get; set; }

    public virtual SecurityAccount SecurityAccount { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SMS_Master> SMS_Master { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Treatment_Master> Treatment_Master { get; set; }

}

}
