﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace JCIEstimate.Models
{
    /// <summary>
    /// Contractor
    /// </summary>
    [MetadataType(typeof(ContractorMetaData))]
    public partial class Contractor
    {
    }

    public class ContractorMetaData
    {
        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }

        [Display(Name = "Contractor")]
        public string contractorName { get; set; }

        [Display(Name = "Active?")]
        public string isActive { get; set; }

        [Display(Name = "Eng Scope Completed"), DisplayFormat(DataFormatString = "{0:d}")]
        public string engScopeCompleted { get; set; }

        [Display(Name = "Contractor Selected"), DisplayFormat(DataFormatString = "{0:d}")]
        public string contractorSelected { get; set; }
    }

    /// <summary>
    /// Category
    /// </summary>
    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {
    }

    public class CategoryMetaData
    {
        [Display(Name = "Category")]
        public System.Guid categoryUid { get; set; }

        [Display(Name = "Category")]
        public string category1 { get; set; }

        [Display(Name = "Category Description")]
        public string categoryDescription { get; set; }
        
    }


    /// <summary>
    /// AspNetUser
    /// </summary>
    [MetadataType(typeof(AspNetUserMetaData))]
    public partial class AspNetUser
    {
    }

    public class AspNetUserMetaData
    {
        [Display(Name = "Email Confirm")]
        public bool EmailConfirmed { get; set; }
        
        [Display(Name = "Password Hash")]
        public string PasswordHash { get; set; }
        
        [Display(Name = "Security Stamp")]
        public string SecurityStamp { get; set; }
        
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        
        [Display(Name = "User Name")]
        public string UserName { get; set; }        
    }

    /// <summary>
    /// Category
    /// </summary>
    [MetadataType(typeof(ContractorUserMetaData))]
    public partial class ContractorUser
    {
    }

    public class ContractorUserMetaData
    {
        [Display(Name = "Contractor User")]
        public System.Guid contractorUserUid { get; set; }

        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }

        [Display(Name = "User Name")]
        public string aspNetUserUid { get; set; }
    }


    /// <summary>
    /// ContractorContact
    /// </summary>
    [MetadataType(typeof(ContractorContactMetaData))]
    public partial class ContractorContact
    {
    }
    
    public class ContractorContactMetaData
    {
        [Display(Name = "Contractor Contact")]
        public System.Guid contractorContactUid { get; set; }                

        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }

        [Display(Name = "Job Title")]
        public string jobTitle { get; set; }

        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        public string lastName { get; set; }        
    }


    /// <summary>
    /// Eddress
    /// </summary>
    [MetadataType(typeof(EddressMetaData))]
    public partial class Eddress
    {
    }

    public class EddressMetaData
    {

        [Display(Name = "Eddress")]
        public System.Guid eddressUid { get; set; }

        [Display(Name = "Contractor Contact")]
        public System.Guid contractorContactUid { get; set; }        
        [Display(Name = "Eddress")]                
        public string eddress1 { get; set; }
        [Display(Name = "Primary?")]
        public Nullable<bool> isPrimary { get; set; }
    }

    /// <summary>
    /// EstmateStatus
    /// </summary>
    [MetadataType(typeof(EstimateStatusMetaData))]
    public partial class EstimateStatu
    {
    }

    public class EstimateStatusMetaData
    {
        [Display(Name = "Estimate Status")]
        public System.Guid estimateStatusUid { get; set; }

        [Display(Name = "Estimate Status")]
        public string estimateStatus { get; set; }

        [Display(Name = "Estimate Status Description")]
        public string estimateStatusDescription { get; set; }

        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// EstmateStatus
    /// </summary>
    [MetadataType(typeof(ProjectMetaData))]
    public partial class Project
    {
    }

    public class ProjectMetaData
    {
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }

        [Display(Name = "Project")]
        public string project1 { get; set; }

        [Display(Name = "Project Description")]
        public string projectDescription { get; set; }
    }

    /// <summary>
    /// Location
    /// </summary>
    [MetadataType(typeof(LocationMetaData))]
    public partial class Location
    {
    }

    public class LocationMetaData
    {
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }

        [Display(Name = "Location")]
        public string location1 { get; set; }

        [Display(Name = "Location Description")]        
        public string locationDescription { get; set; }

        [Display(Name = "Project")]        
        public System.Guid projectUid { get; set; }
    }

    /// <summary>
    /// ECM
    /// </summary>
    [MetadataType(typeof(ECMMetaData))]
    public partial class ECM
    {
    }

    public class ECMMetaData
    {

        [Display(Name = "ECM Full Name")]
        public string ecmString { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "ECM")]
        public System.Guid ecmUid { get; set; }
        [Display(Name = "ECM Description")]
        public string ecmDescription { get; set; }
        [Display(Name = "ECM Number")]
        public string ecmNumber { get; set; }
    }
    
    /// <summary>
    /// Estimate
    /// </summary>
    [MetadataType(typeof(EstimateMetaData))]
    public partial class Estimate
    {
    }

    
    public class EstimateMetaData
    {


        [Display(Name = "Estimate")]
        public System.Guid estimateUid { get; set; }
        [Display(Name = "Contractor")]                
        public System.Guid contractorUid { get; set; }
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }        
        [Display(Name = "ECM")]
        public System.Guid ecmUid { get; set; }
        [Display(Name = "Category")]
        public System.Guid categoryUid { get; set; }
        [Display(Name = "Estimate Status")]
        public System.Guid estimateStatusUid { get; set; }
        [Display(Name = "Active?")]
        public Nullable<bool> isActive { get; set; }        
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public decimal amount { get; set; }        
        [Display(Name = "Active Amount")]
        [DataType(DataType.Currency)]
        public decimal activeAmount { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Notes")]
        public string notes { get; set; }
    }


    /// <summary>
    /// ExpensePercentage
    /// </summary>
    [MetadataType(typeof(ExpensePercentageMetaData))]
    public partial class ExpensePercentage
    {
    }

    public class ExpensePercentageMetaData
    {      
        [Display(Name = "Expense Percentage")]
        public System.Guid expensePercentageUid { get; set; }

        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }

        [Display(Name = "Expense Percentage")]
        public string expensePercentage1 { get; set; }

        [Display(Name = "Description")]
        public string expensePercentageDescription { get; set; }

        [Display(Name = "Percentage"), DisplayFormat(DataFormatString="{0:p3}", ApplyFormatInEditMode= true)]
        public string percentage { get; set; }        
    }

    /// <summary>
    /// ExpenseConstruction
    /// </summary>
    [MetadataType(typeof(ExpenseConstructionMetaData))]
    public partial class ExpenseConstruction
    {
    }

    public class ExpenseConstructionMetaData
    {
        [Display(Name = "Expense Construction")]
        public System.Guid expenseConstructionUid { get; set; }

        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }

        [Display(Name = "Expense Construction")]
        public string expenseConstruction1 { get; set; }

        [Display(Name = "Description")]
        public string expenseConstructionDescription { get; set; }

        [Display(Name = "Rate"), DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal rate { get; set; }

        [Display(Name = "Interval")]
        public System.Guid intervalUid { get; set; }

        [Display(Name = "Quantity")]
        public int quantity { get; set; }

        [Display(Name = "Total"), DisplayFormat(DataFormatString = "{0:C0}")]        
        public Nullable<decimal> total { get; set; }
    }

    /// <summary>
    /// ExpenseMiscellaneou
    /// </summary>
    [MetadataType(typeof(ExpenseMiscellaneouMetaData))]
    public partial class ExpenseMiscellaneou
    {
    }

    public class ExpenseMiscellaneouMetaData
    {
        [Display(Name = "Misc Expense")]
        public System.Guid expenseMiscellaneousUid { get; set; }
        [Display(Name = "Misc Expense")]
        public string expenseMiscellaneous { get; set; }
        [Display(Name = "Description")]
        public string expenseMiscellaneousDescription { get; set; }  
    }

    /// <summary>
    /// ExpenseTravel
    /// </summary>
    [MetadataType(typeof(ExpenseTravelMetaData))]
    public partial class ExpenseTravel
    {
    }

    public class ExpenseTravelMetaData
    {
        [Display(Name = "Travel Expense")]
        public System.Guid expenseTravelUid { get; set; }
        [Display(Name = "project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Travel Expense")]
        public string expenseTravel1 { get; set; }
        [Display(Name = "Description")]
        public string expenseTravelDescription { get; set; }
        [Display(Name = "Rate Per Day"), DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal ratePerDay { get; set; }
        [Display(Name = "Days Per Month")]
        public int daysPerMonth { get; set; }
        [Display(Name = "Duration(months)")]
        public int projectDurationInMonths { get; set; }
        [Display(Name = "Total"), DisplayFormat(DataFormatString = "{0:C0}")]
        public Nullable<decimal> total { get; set; }

    }

    /// <summary>
    /// Interval
    /// </summary>
    [MetadataType(typeof(IntervalMetaData))]
    public partial class Interval
    {
    }

    public class IntervalMetaData
    {
        [Display(Name = "Interval")]
        public System.Guid intervalUid { get; set; }
        [Display(Name = "Interval")]
        public string interval1 { get; set; }
        [Display(Name = "Description")]
        public string intervalDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// Equipment
    /// </summary>
    [MetadataType(typeof(EquipmentMetaData))]
    public partial class Equipment
    {
        public Guid[] selectedTasks { get; set; }
        public MultiSelectList equipmetTasks { get; set; }
    }

    public class EquipmentMetaData
    {
        [Display(Name = "Equipment")]
        public System.Guid equipmentUid { get; set; }
        [Display(Name = "ECM")]
        public System.Guid ecmUid { get; set; }
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }
        [Display(Name = "Room")]
        public string room { get; set; }
        [Display(Name = "Equipment Type")]
        public System.Guid equipmentTypeUid { get; set; }
        [Display(Name = "Equipment Name")]
        public string equipment1 { get; set; }
        [Display(Name = "JCI Code")]
        public string jciCode { get; set; }
        [Display(Name = "Bar Code")]
        public string barCode { get; set; }

        public Guid[] selectedTasks { get; set; }
    }

    /// <summary>
    /// EquipmentTask
    /// </summary>
    [MetadataType(typeof(EquipmentTaskMetaData))]
    public partial class EquipmentTask
    {
    }

    public class EquipmentTaskMetaData
    {
        [Display(Name = "Equipment Task")]
        public System.Guid equipmentTaskUid { get; set; }
        [Display(Name = "Equipment Task")]
        public string equipmentTask1 { get; set; }
        [Display(Name = "Equipment Task Description")]
        public string equipmentTaskDescription { get; set; }
    }

    /// <summary>
    /// EquipmentToDo
    /// </summary>
    [MetadataType(typeof(EquipmentToDoMetaData))]
    public partial class EquipmentToDo
    {
    }

    public class EquipmentToDoMetaData
    {
        [Display(Name = "Equipment To Do")]
        public System.Guid equipmentToDoUid { get; set; }
        [Display(Name = "Equipment Name")]
        public System.Guid equipmentUid { get; set; }
        [Display(Name = "Equipment Task")]
        public System.Guid equipmentTaskUid { get; set; }
        [Display(Name = "Days To Complete")]
        public Nullable<decimal> daysToComplete { get; set; }
    
    }

    /// <summary>
    /// EquipmentType
    /// </summary>
    [MetadataType(typeof(EquipmentTypeMetaData))]
    public partial class EquipmentType
    {
    }

    public class EquipmentTypeMetaData
    {
        [Display(Name = "Equipment Type")]
        public System.Guid equipmentTypeUid { get; set; }
        [Display(Name = "Equipment Type")]
        public string equipmentType1 { get; set; }
        [Display(Name = "Equipment Type Description")]
        public string equipmentTypeDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }

    }

    /// <summary>
    /// ToDoStatu
    /// </summary>
    [MetadataType(typeof(ToDoStatuMetaData))]
    public partial class ToDoStatu
    {
    }

    public class ToDoStatuMetaData
    {
        [Display(Name = "To Do Status")]
        public System.Guid toDoStatusUid { get; set; }
        [Display(Name = "To Do Status")]
        public string toDoStatus { get; set; }
        [Display(Name = "To Do Status Description")]
        public string toDoStatusDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }

    }


    /// <summary>
    /// ProjectToDo
    /// </summary>
    [MetadataType(typeof(ProjectToDoMetaData))]
    public partial class ProjectToDo
    {
    }
    
    public class ProjectToDoMetaData
    {
        [Display(Name = "Project To Do")]
        public System.Guid projectToDoUid { get; set; }
        [Display(Name = "Project To Do")]
        public string projectToDo1 { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Project To Do Description")]
        public string projectToDoDescription { get; set; }
        [Display(Name = "To Do Status")]
        public System.Guid toDoStatusUid { get; set; }
        [Display(Name = "Date Created")]
        [DataType(DataType.Date)]       
        [Editable(false)]
        public Nullable<System.DateTime> dateCreated { get; set; }
        [Display(Name = "Date Resolved")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> dateResolved { get; set; }

    }


    
    /// <summary>
    /// ScopeOfWork
    /// </summary>
    [MetadataType(typeof(ScopeOfWorkMetaData))]
    public partial class ScopeOfWork
    {
    }

    public class ScopeOfWorkMetaData
    {

        [Display(Name = "Scope Of Work")]
        public System.Guid scopeOfWorkUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Category")]
        public System.Guid categoryUid { get; set; }
        [Display(Name = "Scope Of Work")]
        public string scopeOfWork1 { get; set; }
        [Display(Name = "Scope Of Work Description")]
        public string scopeOfWorkDescription { get; set; }
        [Display(Name = "Scope Document")]
        public byte[] document { get; set; }
        [Display(Name = "File Type")]
        public string fileType { get; set; }
        [Display(Name = "Document ID")]
        public string version { get; set; }
        [Display(Name = "Document Name")]
        public string documentName { get; set; }
    }




}