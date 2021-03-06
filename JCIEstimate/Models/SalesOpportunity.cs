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
    
    public partial class SalesOpportunity
    {
        public SalesOpportunity()
        {
            this.SalesOpportunityMilestones = new HashSet<SalesOpportunityMilestone>();
            this.SalesOpportunityTasks = new HashSet<SalesOpportunityTask>();
        }
    
        public System.Guid salesOpportunityUid { get; set; }
        public System.Guid salesTeamUid { get; set; }
        public System.Guid opportunityUid { get; set; }
    
        public virtual SalesTeam SalesTeam { get; set; }
        public virtual ICollection<SalesOpportunityMilestone> SalesOpportunityMilestones { get; set; }
        public virtual ICollection<SalesOpportunityTask> SalesOpportunityTasks { get; set; }
        public virtual Opportunity Opportunity { get; set; }
    }
}
