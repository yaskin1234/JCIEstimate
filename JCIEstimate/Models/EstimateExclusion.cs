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
    
    public partial class EstimateExclusion
    {
        public EstimateExclusion()
        {
            this.EstimateExclusionResponses = new HashSet<EstimateExclusionResponse>();
        }
    
        public System.Guid estimateExclusionUid { get; set; }
        public int estimateExclusionID { get; set; }
        public System.Guid estimateUid { get; set; }
        public string estimateExclusion1 { get; set; }
    
        public virtual Estimate Estimate { get; set; }
        public virtual ICollection<EstimateExclusionResponse> EstimateExclusionResponses { get; set; }
    }
}
