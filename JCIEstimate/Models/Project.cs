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
    
    public partial class Project
    {
        public Project()
        {
            this.ExpenseConstructions = new HashSet<ExpenseConstruction>();
            this.ExpenseMiscellaneousProjects = new HashSet<ExpenseMiscellaneousProject>();
            this.ExpensePercentages = new HashSet<ExpensePercentage>();
            this.ScopeOfWorks = new HashSet<ScopeOfWork>();
            this.ExpenseMonthlies = new HashSet<ExpenseMonthly>();
            this.ProjectMilestones = new HashSet<ProjectMilestone>();
            this.ProjectUsers = new HashSet<ProjectUser>();
            this.Locations = new HashSet<Location>();
            this.ProjectAttachments = new HashSet<ProjectAttachment>();
            this.ProjectRFIs = new HashSet<ProjectRFI>();
            this.ProjectAddendums = new HashSet<ProjectAddendum>();
            this.ProjectManagerAttachments = new HashSet<ProjectManagerAttachment>();
            this.ContractorSignoffs = new HashSet<ContractorSignoff>();
            this.ContractorDraws = new HashSet<ContractorDraw>();
            this.ECMs = new HashSet<ECM>();
        }
    
        public System.Guid projectUid { get; set; }
        public string project1 { get; set; }
        public string projectDescription { get; set; }
        public string aspNetUserUidAsPM { get; set; }
        public Nullable<int> projectDurationInMonths { get; set; }
    
        public virtual ICollection<ExpenseConstruction> ExpenseConstructions { get; set; }
        public virtual ICollection<ExpenseMiscellaneousProject> ExpenseMiscellaneousProjects { get; set; }
        public virtual ICollection<ExpensePercentage> ExpensePercentages { get; set; }
        public virtual ICollection<ScopeOfWork> ScopeOfWorks { get; set; }
        public virtual ICollection<ExpenseMonthly> ExpenseMonthlies { get; set; }
        public virtual ICollection<ProjectMilestone> ProjectMilestones { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<ProjectAttachment> ProjectAttachments { get; set; }
        public virtual ICollection<ProjectRFI> ProjectRFIs { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<ProjectAddendum> ProjectAddendums { get; set; }
        public virtual ICollection<ProjectManagerAttachment> ProjectManagerAttachments { get; set; }
        public virtual ICollection<ContractorSignoff> ContractorSignoffs { get; set; }
        public virtual ICollection<ContractorDraw> ContractorDraws { get; set; }
        public virtual ICollection<ECM> ECMs { get; set; }
    }
}
