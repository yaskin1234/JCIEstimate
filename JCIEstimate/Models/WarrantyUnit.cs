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
    
    public partial class WarrantyUnit
    {
        public WarrantyUnit()
        {
            this.WarrantyIssues = new HashSet<WarrantyIssue>();
        }
    
        public System.Guid warrantyUnitUid { get; set; }
        public System.Guid locationUid { get; set; }
        public string warrantyUnit1 { get; set; }
        public string warrantyUnitDescription { get; set; }
        public string metasysNumber { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual ICollection<WarrantyIssue> WarrantyIssues { get; set; }
    }
}
