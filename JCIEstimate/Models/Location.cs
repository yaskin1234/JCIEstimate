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
    
    public partial class Location
    {
        public Location()
        {
            this.Estimates = new HashSet<Estimate>();
            this.LocationIssues = new HashSet<LocationIssue>();
            this.WarrantyIssues = new HashSet<WarrantyIssue>();
            this.WarrantyUnits = new HashSet<WarrantyUnit>();
            this.Equipments = new HashSet<Equipment>();
            this.LocationCompletionCategories = new HashSet<LocationCompletionCategory>();
            this.EquipmentTypeTaskAssignments = new HashSet<EquipmentTypeTaskAssignment>();
        }
    
        public System.Guid locationUid { get; set; }
        public string location1 { get; set; }
        public string locationDescription { get; set; }
        public System.Guid projectUid { get; set; }
    
        public virtual ICollection<Estimate> Estimates { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<LocationIssue> LocationIssues { get; set; }
        public virtual ICollection<WarrantyIssue> WarrantyIssues { get; set; }
        public virtual ICollection<WarrantyUnit> WarrantyUnits { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        public virtual ICollection<LocationCompletionCategory> LocationCompletionCategories { get; set; }
        public virtual ICollection<EquipmentTypeTaskAssignment> EquipmentTypeTaskAssignments { get; set; }
    }
}
