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
    
    public partial class Estimate
    {
        public Estimate()
        {
            this.EstimateExceptions = new HashSet<EstimateException>();
            this.EstimateExclusions = new HashSet<EstimateExclusion>();
            this.EstimateInclusions = new HashSet<EstimateInclusion>();
        }
    
        public System.Guid estimateUid { get; set; }
        public System.Guid contractorUid { get; set; }
        public System.Guid locationUid { get; set; }
        public System.Guid ecmUid { get; set; }
        public System.Guid categoryUid { get; set; }
        public System.Guid estimateStatusUid { get; set; }
        public Nullable<bool> isActive { get; set; }
        public decimal amount { get; set; }
        public decimal activeAmount { get; set; }
        public string notes { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual ECM ECM { get; set; }
        public virtual Location Location { get; set; }
        public virtual EstimateStatu EstimateStatu { get; set; }
        public virtual ICollection<EstimateException> EstimateExceptions { get; set; }
        public virtual ICollection<EstimateExclusion> EstimateExclusions { get; set; }
        public virtual ICollection<EstimateInclusion> EstimateInclusions { get; set; }
    }
}
