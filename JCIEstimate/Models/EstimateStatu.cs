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
    
    public partial class EstimateStatu
    {
        public EstimateStatu()
        {
            this.Estimates = new HashSet<Estimate>();
        }
    
        public System.Guid estimateStatusUid { get; set; }
        public string estimateStatus { get; set; }
        public string estimateStatusDescription { get; set; }
        public string behaviorIndicator { get; set; }
        public string rowColor { get; set; }
    
        public virtual ICollection<Estimate> Estimates { get; set; }
    }
}
