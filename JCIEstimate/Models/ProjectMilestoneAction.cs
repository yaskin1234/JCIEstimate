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
    
    public partial class ProjectMilestoneAction
    {
        public System.Guid projectMilestoneActionUid { get; set; }
        public System.Guid projectMilestoneUid { get; set; }
        public string projectMilestoneAction1 { get; set; }
        public string projectMilestoneActionDescription { get; set; }
        public Nullable<System.DateTime> plannedStartDate { get; set; }
        public Nullable<System.DateTime> actualStartDate { get; set; }
        public int duration { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public int listOrder { get; set; }
    
        public virtual ProjectMilestone ProjectMilestone { get; set; }
    }
}