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
    
    public partial class EstimateInclusion
    {
        public EstimateInclusion()
        {
            this.EstimateInclusionResponses = new HashSet<EstimateInclusionResponse>();
        }
    
        public System.Guid estimateInclusionUid { get; set; }
        public int estimateInclusionID { get; set; }
        public System.Guid estimateUid { get; set; }
        public string estimateInclusion1 { get; set; }
    
        public virtual Estimate Estimate { get; set; }
        public virtual ICollection<EstimateInclusionResponse> EstimateInclusionResponses { get; set; }
    }
}