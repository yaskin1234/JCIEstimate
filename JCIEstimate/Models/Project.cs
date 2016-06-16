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
            this.CalendarDayTasks = new HashSet<CalendarDayTask>();
            this.ContractorDraws = new HashSet<ContractorDraw>();
            this.ContractorNotes = new HashSet<ContractorNote>();
            this.ContractorSignoffs = new HashSet<ContractorSignoff>();
            this.ContractorSignoffFinals = new HashSet<ContractorSignoffFinal>();
            this.ECMs = new HashSet<ECM>();
            this.ExpenseConstructions = new HashSet<ExpenseConstruction>();
            this.ExpenseConstructionDraws = new HashSet<ExpenseConstructionDraw>();
            this.ExpenseMiscellaneousProjects = new HashSet<ExpenseMiscellaneousProject>();
            this.ExpenseMonthlies = new HashSet<ExpenseMonthly>();
            this.ExpensePercentages = new HashSet<ExpensePercentage>();
            this.Locations = new HashSet<Location>();
            this.LocationCompletionCategories = new HashSet<LocationCompletionCategory>();
            this.MasterSchedules = new HashSet<MasterSchedule>();
            this.PECCosts = new HashSet<PECCost>();
            this.ProjectUsers = new HashSet<ProjectUser>();
            this.ProjectAddendums = new HashSet<ProjectAddendum>();
            this.ProjectAttachments = new HashSet<ProjectAttachment>();
            this.ProjectCalendarDayTasks = new HashSet<ProjectCalendarDayTask>();
            this.ProjectManagerAttachments = new HashSet<ProjectManagerAttachment>();
            this.ProjectRFIs = new HashSet<ProjectRFI>();
            this.ProjectScopes = new HashSet<ProjectScope>();
            this.ProjectTaskLists = new HashSet<ProjectTaskList>();
            this.ReportDefinitions = new HashSet<ReportDefinition>();
            this.ScopeOfWorks = new HashSet<ScopeOfWork>();
        }
    
        public System.Guid projectUid { get; set; }
        public string project1 { get; set; }
        public string projectDescription { get; set; }
        public string aspNetUserUidAsPM { get; set; }
        public Nullable<int> projectDurationInMonths { get; set; }
        public int downPayment { get; set; }
        public int contractAmount { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public int drawPeriods { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<CalendarDayTask> CalendarDayTasks { get; set; }
        public virtual ICollection<ContractorDraw> ContractorDraws { get; set; }
        public virtual ICollection<ContractorNote> ContractorNotes { get; set; }
        public virtual ICollection<ContractorSignoff> ContractorSignoffs { get; set; }
        public virtual ICollection<ContractorSignoffFinal> ContractorSignoffFinals { get; set; }
        public virtual ICollection<ECM> ECMs { get; set; }
        public virtual ICollection<ExpenseConstruction> ExpenseConstructions { get; set; }
        public virtual ICollection<ExpenseConstructionDraw> ExpenseConstructionDraws { get; set; }
        public virtual ICollection<ExpenseMiscellaneousProject> ExpenseMiscellaneousProjects { get; set; }
        public virtual ICollection<ExpenseMonthly> ExpenseMonthlies { get; set; }
        public virtual ICollection<ExpensePercentage> ExpensePercentages { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<LocationCompletionCategory> LocationCompletionCategories { get; set; }
        public virtual ICollection<MasterSchedule> MasterSchedules { get; set; }
        public virtual ICollection<PECCost> PECCosts { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<ProjectAddendum> ProjectAddendums { get; set; }
        public virtual ICollection<ProjectAttachment> ProjectAttachments { get; set; }
        public virtual ICollection<ProjectCalendarDayTask> ProjectCalendarDayTasks { get; set; }
        public virtual ICollection<ProjectManagerAttachment> ProjectManagerAttachments { get; set; }
        public virtual ICollection<ProjectRFI> ProjectRFIs { get; set; }
        public virtual ICollection<ProjectScope> ProjectScopes { get; set; }
        public virtual ICollection<ProjectTaskList> ProjectTaskLists { get; set; }
        public virtual ICollection<ReportDefinition> ReportDefinitions { get; set; }
        public virtual ICollection<ScopeOfWork> ScopeOfWorks { get; set; }
    }
}
