﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JCIEstimateEntities : DbContext
    {
        public JCIEstimateEntities()
            : base("name=JCIEstimateEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<ContractorContact> ContractorContacts { get; set; }
        public virtual DbSet<ContractorUser> ContractorUsers { get; set; }
        public virtual DbSet<Eddress> Eddresses { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ExpenseConstruction> ExpenseConstructions { get; set; }
        public virtual DbSet<Interval> Intervals { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<ExpenseMiscellaneou> ExpenseMiscellaneous { get; set; }
        public virtual DbSet<ExpenseMiscellaneousProject> ExpenseMiscellaneousProjects { get; set; }
        public virtual DbSet<ExpensePercentage> ExpensePercentages { get; set; }
        public virtual DbSet<ToDoStatu> ToDoStatus { get; set; }
        public virtual DbSet<ProjectToDo> ProjectToDoes { get; set; }
        public virtual DbSet<ScopeOfWork> ScopeOfWorks { get; set; }
        public virtual DbSet<ExpenseMonthly> ExpenseMonthlies { get; set; }
        public virtual DbSet<EstimateException> EstimateExceptions { get; set; }
        public virtual DbSet<EstimateExceptionResponse> EstimateExceptionResponses { get; set; }
        public virtual DbSet<EstimateExclusion> EstimateExclusions { get; set; }
        public virtual DbSet<EstimateExclusionResponse> EstimateExclusionResponses { get; set; }
        public virtual DbSet<EstimateInclusion> EstimateInclusions { get; set; }
        public virtual DbSet<EstimateInclusionResponse> EstimateInclusionResponses { get; set; }
        public virtual DbSet<WarrantyStatu> WarrantyStatus { get; set; }
        public virtual DbSet<WarrantyNote> WarrantyNotes { get; set; }
        public virtual DbSet<WarrantyAttachment> WarrantyAttachments { get; set; }
        public virtual DbSet<WarrantyUnit> WarrantyUnits { get; set; }
        public virtual DbSet<EquipmentTask> EquipmentTasks { get; set; }
        public virtual DbSet<EquipmentToDo> EquipmentToDoes { get; set; }
        public virtual DbSet<EstimateStatu> EstimateStatus { get; set; }
        public virtual DbSet<EquipmentAttributeType> EquipmentAttributeTypes { get; set; }
        public virtual DbSet<AppDataType> AppDataTypes { get; set; }
        public virtual DbSet<EquipmentAttribute> EquipmentAttributes { get; set; }
        public virtual DbSet<EquipmentAttributeValue> EquipmentAttributeValues { get; set; }
        public virtual DbSet<EquipmentAttachment> EquipmentAttachments { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationIssue> LocationIssues { get; set; }
        public virtual DbSet<LocationNote> LocationNotes { get; set; }
        public virtual DbSet<WarrantyIssue> WarrantyIssues { get; set; }
        public virtual DbSet<EquipmentNoteType> EquipmentNoteTypes { get; set; }
        public virtual DbSet<EquipmentNote> EquipmentNotes { get; set; }
        public virtual DbSet<ProjectAttachment> ProjectAttachments { get; set; }
        public virtual DbSet<ProjectAttachmentType> ProjectAttachmentTypes { get; set; }
        public virtual DbSet<ProjectRFIResponse> ProjectRFIResponses { get; set; }
        public virtual DbSet<RfiType> RfiTypes { get; set; }
        public virtual DbSet<ProjectRFI> ProjectRFIs { get; set; }
        public virtual DbSet<RfiStatu> RfiStatus { get; set; }
        public virtual DbSet<ProjectAddendum> ProjectAddendums { get; set; }
        public virtual DbSet<ProjectManagerAttachment> ProjectManagerAttachments { get; set; }
        public virtual DbSet<ContractorTotalsByProject> ContractorTotalsByProjects { get; set; }
        public virtual DbSet<v__ContractorECMByCategory> v__ContractorECMByCategory { get; set; }
        public virtual DbSet<v__ExpensePercentage> v__ExpensePercentage { get; set; }
        public virtual DbSet<ContractorDraw> ContractorDraws { get; set; }
        public virtual DbSet<ContractorDrawSchedule> ContractorDrawSchedules { get; set; }
        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual DbSet<ExpenseConstructionDrawSchedule> ExpenseConstructionDrawSchedules { get; set; }
        public virtual DbSet<ExpenseConstructionDraw> ExpenseConstructionDraws { get; set; }
        public virtual DbSet<ECM> ECMs { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<ControlType> ControlTypes { get; set; }
        public virtual DbSet<HeatType> HeatTypes { get; set; }
        public virtual DbSet<EquipmentCondition> EquipmentConditions { get; set; }
        public virtual DbSet<ContractorSignoff> ContractorSignoffs { get; set; }
        public virtual DbSet<ContractorSignoffAttachment> ContractorSignoffAttachments { get; set; }
        public virtual DbSet<ContractorSignoffFinal> ContractorSignoffFinals { get; set; }
        public virtual DbSet<ContractorSignoffFinalAttachment> ContractorSignoffFinalAttachments { get; set; }
        public virtual DbSet<CompletionCategory> CompletionCategories { get; set; }
        public virtual DbSet<LocationCompletionCategory> LocationCompletionCategories { get; set; }
        public virtual DbSet<PECTask> PECTasks { get; set; }
        public virtual DbSet<Week> Weeks { get; set; }
        public virtual DbSet<PECExpenseType> PECExpenseTypes { get; set; }
        public virtual DbSet<PECCost> PECCosts { get; set; }
        public virtual DbSet<EquipmentAttributeTypeTask> EquipmentAttributeTypeTasks { get; set; }
        public virtual DbSet<EquipmentTypeTaskAssignment> EquipmentTypeTaskAssignments { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }
        public virtual DbSet<ProjectEstimateEmail__v> ProjectEstimateEmail__v { get; set; }
        public virtual DbSet<EstimateOption> EstimateOptions { get; set; }
        public virtual DbSet<Estimate> Estimates { get; set; }
        public virtual DbSet<ContractorNoteType> ContractorNoteTypes { get; set; }
        public virtual DbSet<ContractorNoteStatu> ContractorNoteStatus { get; set; }
        public virtual DbSet<ContractorNote> ContractorNotes { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<ContractorSchedule> ContractorSchedules { get; set; }
        public virtual DbSet<MasterSchedule> MasterSchedules { get; set; }
        public virtual DbSet<MasterScheduleTask> MasterScheduleTasks { get; set; }
        public virtual DbSet<ContractorScheduleTask> ContractorScheduleTasks { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<SalesOpportunityMilestone> SalesOpportunityMilestones { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<SalesOpportunity> SalesOpportunities { get; set; }
        public virtual DbSet<SalesTeam> SalesTeams { get; set; }
        public virtual DbSet<SalesTeamMember> SalesTeamMembers { get; set; }
        public virtual DbSet<SalesOpportunityTask> SalesOpportunityTasks { get; set; }
        public virtual DbSet<Opportunity> Opportunities { get; set; }
    }
}
