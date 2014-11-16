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
        public virtual DbSet<EstimateStatu> EstimateStatus { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ECM> ECMs { get; set; }
        public virtual DbSet<ExpenseConstruction> ExpenseConstructions { get; set; }
        public virtual DbSet<Interval> Intervals { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<EquipmentToDo> EquipmentToDoes { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<EquipmentTask> EquipmentTasks { get; set; }
        public virtual DbSet<ExpenseMiscellaneou> ExpenseMiscellaneous { get; set; }
        public virtual DbSet<ExpenseMiscellaneousProject> ExpenseMiscellaneousProjects { get; set; }
        public virtual DbSet<ExpensePercentage> ExpensePercentages { get; set; }
        public virtual DbSet<ToDoStatu> ToDoStatus { get; set; }
        public virtual DbSet<ProjectToDo> ProjectToDoes { get; set; }
        public virtual DbSet<Estimate> Estimates { get; set; }
        public virtual DbSet<ScopeOfWork> ScopeOfWorks { get; set; }
        public virtual DbSet<TestGrid> TestGrids { get; set; }
        public virtual DbSet<ExpenseMonthly> ExpenseMonthlies { get; set; }
        public virtual DbSet<EstimateException> EstimateExceptions { get; set; }
        public virtual DbSet<EstimateExceptionResponse> EstimateExceptionResponses { get; set; }
        public virtual DbSet<EstimateExclusion> EstimateExclusions { get; set; }
        public virtual DbSet<EstimateExclusionResponse> EstimateExclusionResponses { get; set; }
        public virtual DbSet<EstimateInclusion> EstimateInclusions { get; set; }
        public virtual DbSet<EstimateInclusionResponse> EstimateInclusionResponses { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<MilestoneAction> MilestoneActions { get; set; }
        public virtual DbSet<ProjectMilestone> ProjectMilestones { get; set; }
        public virtual DbSet<ProjectMilestoneAction> ProjectMilestoneActions { get; set; }
    }
}
