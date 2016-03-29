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
    
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.ContractorUsers = new HashSet<ContractorUser>();
            this.WarrantyIssues = new HashSet<WarrantyIssue>();
            this.ProjectManagerAttachments = new HashSet<ProjectManagerAttachment>();
            this.ContractorSignoffs = new HashSet<ContractorSignoff>();
            this.ContractorSignoffFinals = new HashSet<ContractorSignoffFinal>();
            this.ContractorSignoffAttachments = new HashSet<ContractorSignoffAttachment>();
            this.ContractorSignoffFinalAttachments = new HashSet<ContractorSignoffFinalAttachment>();
            this.LocationIssues = new HashSet<LocationIssue>();
            this.LocationNotes = new HashSet<LocationNote>();
            this.WarrantyNotes = new HashSet<WarrantyNote>();
            this.PECCosts = new HashSet<PECCost>();
            this.Calendars = new HashSet<Calendar>();
            this.EquipmentNotes = new HashSet<EquipmentNote>();
            this.LocationIssues1 = new HashSet<LocationIssue>();
            this.ProjectUsers = new HashSet<ProjectUser>();
            this.Projects = new HashSet<Project>();
            this.ProjectAddendums = new HashSet<ProjectAddendum>();
            this.ProjectCalendarDayTasks = new HashSet<ProjectCalendarDayTask>();
            this.ProjectCalendars = new HashSet<ProjectCalendar>();
            this.ProjectRFIs = new HashSet<ProjectRFI>();
            this.ProjectRFIs1 = new HashSet<ProjectRFI>();
            this.ProjectRFIResponses = new HashSet<ProjectRFIResponse>();
            this.SalesTeamMembers = new HashSet<SalesTeamMember>();
            this.AspNetUsersExtensions = new HashSet<AspNetUsersExtension>();
            this.ProjectTaskLists = new HashSet<ProjectTaskList>();
            this.CommissionNotes = new HashSet<CommissionNote>();
            this.CommissionIssues = new HashSet<CommissionIssue>();
        }
    
        public string Id { get; set; }
        public string AllowableContractors { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<ContractorUser> ContractorUsers { get; set; }
        public virtual ICollection<WarrantyIssue> WarrantyIssues { get; set; }
        public virtual ICollection<ProjectManagerAttachment> ProjectManagerAttachments { get; set; }
        public virtual ICollection<ContractorSignoff> ContractorSignoffs { get; set; }
        public virtual ICollection<ContractorSignoffFinal> ContractorSignoffFinals { get; set; }
        public virtual ICollection<ContractorSignoffAttachment> ContractorSignoffAttachments { get; set; }
        public virtual ICollection<ContractorSignoffFinalAttachment> ContractorSignoffFinalAttachments { get; set; }
        public virtual ICollection<LocationIssue> LocationIssues { get; set; }
        public virtual ICollection<LocationNote> LocationNotes { get; set; }
        public virtual ICollection<WarrantyNote> WarrantyNotes { get; set; }
        public virtual ICollection<PECCost> PECCosts { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<EquipmentNote> EquipmentNotes { get; set; }
        public virtual ICollection<LocationIssue> LocationIssues1 { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ProjectAddendum> ProjectAddendums { get; set; }
        public virtual ICollection<ProjectCalendarDayTask> ProjectCalendarDayTasks { get; set; }
        public virtual ICollection<ProjectCalendar> ProjectCalendars { get; set; }
        public virtual ICollection<ProjectRFI> ProjectRFIs { get; set; }
        public virtual ICollection<ProjectRFI> ProjectRFIs1 { get; set; }
        public virtual ICollection<ProjectRFIResponse> ProjectRFIResponses { get; set; }
        public virtual ICollection<SalesTeamMember> SalesTeamMembers { get; set; }
        public virtual ICollection<AspNetUsersExtension> AspNetUsersExtensions { get; set; }
        public virtual ICollection<ProjectTaskList> ProjectTaskLists { get; set; }
        public virtual ICollection<CommissionNote> CommissionNotes { get; set; }
        public virtual ICollection<CommissionIssue> CommissionIssues { get; set; }
    }
}
