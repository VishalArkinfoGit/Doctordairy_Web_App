
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
    
public partial class Package_Master
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Package_Master()
    {

        this.Package_Assigned = new HashSet<Package_Assigned>();

        this.Package_Requested = new HashSet<Package_Requested>();

    }


    public long Id { get; set; }

    public string Package_Name { get; set; }

    public Nullable<int> Price { get; set; }

    public int No_Of_Sms { get; set; }

    public string Days { get; set; }

    public bool isActive { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Package_Assigned> Package_Assigned { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Package_Requested> Package_Requested { get; set; }

}

}
