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
            this.Equipments = new HashSet<Equipment>();
            this.Estimates = new HashSet<Estimate>();
            this.ProjectRFIs = new HashSet<ProjectRFI>();
        }
    
        public System.Guid ecmUid { get; set; }
        public decimal ecmNumber { get; set; }
        public string ecmDescription { get; set; }
        public System.Guid projectUid { get; set; }
        public string ecmString { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        public virtual ICollection<Estimate> Estimates { get; set; }
        public virtual ICollection<ProjectRFI> ProjectRFIs { get; set; }
    }
}
