//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JCIEstimate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ECM
    {
        public ECM()
        {
            this.Estimates = new HashSet<Estimate>();
        }
    
        public System.Guid ecmUid { get; set; }
        public string ecmNumber { get; set; }
        public string ecmDescription { get; set; }
        public string ecmString { get; set; }
        public Nullable<System.Guid> projectLineOfWorkUid { get; set; }
    
        public virtual ProjectLineOfWork ProjectLineOfWork { get; set; }
        public virtual ICollection<Estimate> Estimates { get; set; }
    }
}
