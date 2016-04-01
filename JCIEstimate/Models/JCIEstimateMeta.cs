using System;
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
    /// Project
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
        [Display(Name = "Project Manager")]        
        public string aspNetUserUidAsPM { get; set; }
        [Display(Name = "Down Payment")]
        public int downPayment { get; set; }
        [Display(Name = "Contract Amount")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public int contractAmount { get; set; }        
        [Display(Name = "Draw start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]       
        public DateTime startDate { get; set; }
        [Display(Name = "Draw Periods")]
        public int drawPeriods { get; set; }
    }


    /// <summary>
    /// ProjectAttachment
    /// </summary>
    [MetadataType(typeof(ProjectAttachmentMetaData))]
    public partial class ProjectAttachment
    {
    }

    public class ProjectAttachmentMetaData
    {
        [Display(Name = "Project Attachment")]
        public System.Guid projectAttachmentUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Project Attachment")]
        public string projectAttachment1 { get; set; }        
        public byte[] attachment { get; set; }
        public string fileType { get; set; }
        [Display(Name = "Document Name")]
        public string documentName { get; set; }
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
        [Display(Name = "ecmNumber"), DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode= true)]        
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
        [Display(Name = "Estimate Option")]
        public System.Guid estimateOptionUid { get; set; }        
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
        [DataType(DataType.MultilineText)]
        [Display(Name = "JCI Notes")]
        public string jciNote { get; set; }
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
    /// ExpenseMonthly
    /// </summary>
    [MetadataType(typeof(ExpenseMonthlyMetaData))]
    public partial class ExpenseMonthly
    {
    }

    public class ExpenseMonthlyMetaData
    {
        [Display(Name = "Monthly Expense")]
        public System.Guid expenseMonthlyUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Monthly Expense")]
        public string expenseMonthly1 { get; set; }
        [Display(Name = "Description")]
        public string expenseMonthlyDescription { get; set; }
        [Display(Name = "Rate"), DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal ratePerDay { get; set; }
        [Display(Name = "Quantity")]
        public int daysPerMonth { get; set; }
        [Display(Name = "Duration")]
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
    /// EquipmentAttributeType
    /// </summary>
    [MetadataType(typeof(EquipmentAttributeTypeMetaData))]
    public partial class EquipmentAttributeType
    {
    }

    public class EquipmentAttributeTypeMetaData
    {
        [Display(Name = "Equipment Attribute Type")]
        public System.Guid equipmentAttributeTypeUid { get; set; }
        [Display(Name = "Equipment Attribute Type")]
        public string equipmentAttributeType1 { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// EquipmentNoteType
    /// </summary>
    [MetadataType(typeof(EquipmentNoteTypeMetaData))]
    public partial class EquipmentNoteType
    {
    }

    public class EquipmentNoteTypeMetaData
    {
        [Display(Name = "Equipment Note Type")]
        public System.Guid equipmentNoteTypeUid { get; set; }
        [Display(Name = "Equipment Note Type")]
        public string equipmentNoteType1 { get; set; }
        [Display(Name = "Equipment Type Description")]
        public string equipmentNoteTypeDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// EquipmentAttachment
    /// </summary>
    [MetadataType(typeof(EquipmentAttachmentMetaData))]
    public partial class EquipmentAttachment
    {
    }

    public class EquipmentAttachmentMetaData
    {
        [Display(Name = "Equipment Attachment")]
        public System.Guid equipmentAttachmentUid { get; set; }
        [Display(Name = "Equipment")]
        public System.Guid equipmentUid { get; set; }
        [Display(Name = "Description")]
        public string equipmentAttachment1 { get; set; }
        [Display(Name = "File")]
        public byte[] attachment { get; set; }
        [Display(Name = "File Type")]
        public string fileType { get; set; }
        [Display(Name = "Document Name")]
        public string documentName { get; set; }

        public virtual Equipment Equipment { get; set; }
    }

    /// <summary>
    /// EquipmentNote
    /// </summary>
    [MetadataType(typeof(EquipmentNoteMetaData))]
    public partial class EquipmentNote
    {
    }

    public class EquipmentNoteMetaData
    {
        [Display(Name = "Equipment Note")]
        public System.Guid equipmentNoteUid { get; set; }
        [Display(Name = "Equipment")]
        public System.Guid equipmentUid { get; set; }
        public Nullable<System.Guid> equipmentNoteTypeUid { get; set; }
        [Display(Name = "Equipment Note")]
        public string equipmentNote1 { get; set; }
        [Display(Name = "Date")]
        public System.DateTime date { get; set; }
        [Display(Name = "Created By")]
        public string aspNetUserUidAsCreated { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual EquipmentNoteType EquipmentNoteType { get; set; }
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
        [Display(Name = "Heat Type")]
        public System.Guid heatTypeUid { get; set; }
        [Display(Name = "Control Type")]
        public System.Guid controlTypeUid { get; set; }
        [DisplayFormat(DataFormatString = "{0:###########}", ApplyFormatInEditMode = true)]
        [Display(Name = "JCI Tag")]        
        public string jciTag { get; set; }
        [Display(Name = "Owner Tag")]        
        public string ownerTag { get; set; }
        [Display(Name = "Manufacturer")]        
        public string manufacturer { get; set; }
        [Display(Name = "New Manufacturer")]
        public string newManufacturer { get; set; }
        [DataType(DataType.Date)]       
        [Display(Name = "Install Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> installDate { get; set; }
        [Display(Name = "Room")]        
        public string area { get; set; }
        [Display(Name = "Equipment Attribute Type")]
        public string equipmentAttributeTypeUid { get; set; }
        [Display(Name = "Serial Number")]
        public string serialNumber { get; set; }
        [Display(Name = "Model")]
        public string model { get; set; }
        [Display(Name = "New Serial Number")]
        public string newSerial { get; set; }
        [Display(Name = "New Model")]
        public string newModel { get; set; }
        [Display(Name = "Replacement For")]
        public string equipmentUidAsReplaced { get; set; }
        [Display(Name = "New To Site")]
        public bool isNewToSite { get; set; }
        [Display(Name = "Use Replacement")]
        public bool useReplacement { get; set; }        
        [Display(Name = "Price"), DisplayFormat(DataFormatString = "{0:C0}")]
        public bool price { get; set; }
        [Display(Name = "Metasys Number")]
        public bool metasysNumber { get; set; }

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

    ///// <summary>
    ///// ToDoStatu
    ///// </summary>
    //[MetadataType(typeof(ToDoStatuMetaData))]
    //public partial class ToDoStatu
    //{
    //}

    //public class ToDoStatuMetaData
    //{
    //    [Display(Name = "To Do Status")]
    //    public System.Guid toDoStatusUid { get; set; }
    //    [Display(Name = "To Do Status")]
    //    public string toDoStatus { get; set; }
    //    [Display(Name = "To Do Status Description")]
    //    public string toDoStatusDescription { get; set; }
    //    [Display(Name = "Behavior Indicator")]
    //    public string behaviorIndicator { get; set; }

    //}


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



    /// <summary>
    /// EstimateExclusion
    /// </summary>
    [MetadataType(typeof(EstimateExclusionMetaData))]
    public partial class EstimateExclusion
    {
    }

    public class EstimateExclusionMetaData
    {

        [Display(Name = "Estimate Exclusion")]
        public System.Guid estimateExclusionUid { get; set; }
        [Display(Name = "Estimate Exclusion ID")]
        public int estimateExclusionID { get; set; }
        [Display(Name = "Estimate")]
        public System.Guid estimateUid { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Estimate Exclusion")]
        public string estimateExclusion1 { get; set; }
    }

    /// <summary>
    /// EstimateExclusionResponse
    /// </summary>
    [MetadataType(typeof(EstimateExclusionResponseMetaData))]
    public partial class EstimateExclusionResponse
    {
    }

    public class EstimateExclusionResponseMetaData
    {
        [Display(Name = "Estimate Exclusion Response")]
        public System.Guid estimateExclusionResponseUid { get; set; }
        [Display(Name = "Estimate Exclusion Response ID")]
        public int estimateExclusionResponseID { get; set; }
        [Display(Name = "Estimate Exclusion")]
        public System.Guid estimateExclusionUid { get; set; }
        [Display(Name = "Estimate Exclusion Response")]
        public string estimateExclusionResponse1 { get; set; }
        [Display(Name = "Response Date")]
        public System.DateTime responseDate { get; set; }
    }


    /// <summary>
    /// EstimateInclusion
    /// </summary>
    [MetadataType(typeof(EstimateInclusionMetaData))]
    public partial class EstimateInclusion
    {
    }

    public class EstimateInclusionMetaData
    {
        [Display(Name = "Estimate Inclusion")]
        public System.Guid estimateInclusionUid { get; set; }
        [Display(Name = "Estimate Inclusion ID")]
        public int estimateInclusionID { get; set; }
        [Display(Name = "Estimate")]
        public System.Guid estimateUid { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Estimate Inclusion")]
        public string estimateInclusion1 { get; set; }        
    }

    /// <summary>
    /// EstimateInclusionResponse
    /// </summary>
    [MetadataType(typeof(EstimateInclusionResponseMetaData))]
    public partial class EstimateInclusionResponse
    {
    }

    public class EstimateInclusionResponseMetaData
    {
        [Display(Name = "Estimate Inclusion Response")]
        public System.Guid estimateInclusionResponseUid { get; set; }
        [Display(Name = "Estimate Inclusion Response ID")]
        public int estimateInclusionResponseID { get; set; }
        [Display(Name = "Estimate Inclusion")]
        public System.Guid estimateInclusionUid { get; set; }
        [Display(Name = "Estimate Inclusion Response")]
        public string estimateInclusionResponse1 { get; set; }
        [Display(Name = "Response Date")]
        public System.DateTime responseDate { get; set; }
    }

    /// <summary>
    /// EstimateException
    /// </summary>
    [MetadataType(typeof(EstimateExceptionMetaData))]
    public partial class EstimateException
    {
    }

    public class EstimateExceptionMetaData
    {
        [Display(Name = "Estimate Exception")]
        public System.Guid estimateExceptionUid { get; set; }
        [Display(Name = "Estimate Exception ID")]
        public int estimateExceptionID { get; set; }
        [Display(Name = "Estimate")]
        public System.Guid estimateUid { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Estimate Exception")]
        public string estimateException1 { get; set; }
    }

    /// <summary>
    /// EstimateExceptionResponse
    /// </summary>
    [MetadataType(typeof(EstimateExceptionResponseMetaData))]
    public partial class EstimateExceptionResponse
    {
    }

    public class EstimateExceptionResponseMetaData
    {
        [Display(Name = "Estimate Exception Response")]
        public System.Guid estimateExceptionResponseUid { get; set; }
        [Display(Name = "Estimate Exception Response ID")]
        public int estimateExceptionResponseID { get; set; }
        [Display(Name = "Estimate Exception")]
        public System.Guid estimateExceptionUid { get; set; }
        [Display(Name = "Estimate Exception Response")]
        public string estimateExceptionResponse1 { get; set; }
        [Display(Name = "Response Date")]
        public System.DateTime responseDate { get; set; }
    }

    /// <summary>
    /// Milestone
    /// </summary>
    [MetadataType(typeof(MilestoneMetaData))]
    public partial class Milestone
    {
    }
        
    public class MilestoneMetaData
    {
        [Display(Name = "Milestone")]
        public System.Guid milestoneUid { get; set; }
        [Display(Name = "Milestone")]
        public string milestone1 { get; set; }
        [Display(Name = "Milestone Description")]
        public string milestoneDescription { get; set; }
        [Display(Name = "Default List Order")]
        public int defaultListOrder { get; set; }
    }


    /// <summary>
    /// WarrantyStatus
    /// </summary>
    [MetadataType(typeof(WarrantyStatusMetaData))]
    public partial class WarrantyStatus
    {
    }

    public class WarrantyStatusMetaData
    {
        [Display(Name = "Warranty Status")]
        public System.Guid warrantyStatusUid { get; set; }                
        [Display(Name = "Warranty Status")]
        public string warrantyStatus { get; set; }
        [Display(Name = "Warranty Status Description")]
        public string warrantyStatusDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// WarrantyIssue
    /// </summary>
    [MetadataType(typeof(WarrantyIssueMetaData))]
    public partial class WarrantyIssue
    {
    }

    public class WarrantyIssueMetaData
    {
        [Display(Name = "Warranty Issue")]
        public System.Guid warrantyIssueUid { get; set; }
        [Display(Name = "Unit (Optional)")]        
        public System.Guid warrantyUnitUid { get; set; }        
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }
        [Display(Name = "Issue Status")]
        [Required]
        public System.Guid warrantyStatusUid { get; set; }        
        [Display(Name = "Room Location")]        
        public string warrantyIssueLocation { get; set; }        
        [Display(Name = "Issue Description")]        
        public string warrantyIssue1 { get; set; }
        [Display(Name = "Assigned To")]
        public string projectUserUid { get; set; }
        [DataType(DataType.DateTime)]       
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]        
        [Display(Name = "Date Created")]
        public System.DateTime date { get; set; }
    }


    /// <summary>
    /// WarrantyNote
    /// </summary>
    [MetadataType(typeof(WarrantyNoteMetaData))]
    public partial class WarrantyNote
    {
    }

    public class WarrantyNoteMetaData
    {
        [Display(Name = "Issue Note")]
        public System.Guid warrantyNoteUid { get; set; }
        [Display(Name = "Issue Description")]
        public System.Guid warrantyIssueUid { get; set; }
        [Display(Name = "Note Text")]
        public string warrantyNote1 { get; set; }
        [Display(Name = "Date Created")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]        
        public System.DateTime date { get; set; }
    }

    /// <summary>
    /// WarrantyUnit
    /// </summary>
    [MetadataType(typeof(WarrantyUnitMetaData))]
    public partial class WarrantyUnit
    {
    }

    public class WarrantyUnitMetaData
    {
        [Display(Name = "Warranty Unit")]
        public System.Guid warrantyUnitUid { get; set; }
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }
        [Display(Name = "Unit Number")]
        public string warrantyUnit1 { get; set; }
        [Display(Name = "Unit Description")]
        public string warrantyUnitDescription { get; set; }
    }

    /// <summary>
    /// WarrantyStatu
    /// </summary>
    [MetadataType(typeof(WarrantyStatuMetaData))]
    public partial class WarrantyStatu
    {
    }

    public class WarrantyStatuMetaData
    {
        [Display(Name = "Issue Status")]
        public System.Guid warrantyStatusUid { get; set; }
        [Display(Name = "Issue Status")]
        public string warrantyStatus { get; set; }
        [Display(Name = "Issue Status Description")]
        public string warrantyStatusDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// RfiType
    /// </summary>
    [MetadataType(typeof(RfiTypeMetaData))]
    public partial class RfiType
    {
    }

    public class RfiTypeMetaData
    {
        [Display(Name = "RFI Type")]
        public System.Guid rfiTypeUid { get; set; }
        [Display(Name = "RFI Type")]
        public string rfiType1 { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }

    }

    /// <summary>
    /// RfiStatu
    /// </summary>
    [MetadataType(typeof(RfiStatuMetaData))]
    public partial class RfiStatu
    {
    }

    public class RfiStatuMetaData
    {
        [Display(Name = "RFI Status")]
        public System.Guid rfiStatusUid { get; set; }
        [Display(Name = "RFI Status")]
        public string rfiStatus { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }

    }

    /// <summary>
    /// ProjectRFI
    /// </summary>
    [MetadataType(typeof(ProjectRFIMetaData))]
    public partial class ProjectRFI
    {
    }

    public class ProjectRFIMetaData
    {
        [Display(Name = "RFI")]
        public System.Guid projectRFIUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Date Created")]
        public System.DateTime dateCreated { get; set; }
        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }
        [Display(Name = "ECM")]
        public System.Guid ecmUid { get; set; }
        [Display(Name = "RFI Type")]
        public System.Guid rfiTypeUid { get; set; }
        [Display(Name = "Created By")]
        public string aspNetUserUidAsCreated { get; set; }
        [Display(Name = "Assigned To")]
        public string aspNetUserUidAsAssigned { get; set; }
        [Display(Name = "RFI")]
        public string projectRFI1 { get; set; }
        [Display(Name = "RFI ID")]
        public int projectRFIID { get; set; }

    }

    /// <summary>
    /// RfiStatu
    /// </summary>
    [MetadataType(typeof(ProjectAddendumMetaData))]
    public partial class ProjectAddendum
    {
    }

    public class ProjectAddendumMetaData
    {
        [Display(Name = "Addendum")]
        public System.Guid projectAddendumUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Date Created")]
        public System.DateTime dateCreated { get; set; }
        [Display(Name = "Created By")]
        public string aspNetUserUidAsCreated { get; set; }
        [Display(Name = "Addendum")]
        public string projectAddendum1 { get; set; }
        [Display(Name = "Addendum ID")]
        public int addendumId { get; set; }

    }

    /// <summary>
    /// ProjectManagerAttachment
    /// </summary>
    [MetadataType(typeof(ProjectManagerAttachmentMetaData))]
    public partial class ProjectManagerAttachment
    {
    }

    public class ProjectManagerAttachmentMetaData
    {
        [Display(Name = "Project Manager Attachment")]
        public System.Guid projectManagerAttachmentUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Created By")]
        public string aspNetUserUidAsCreated { get; set; }
        [Display(Name = "Date Created")]
        public Nullable<System.DateTime> dateCreated { get; set; }
        [Display(Name = "Description")]
        public string projectManagerAttachment1 { get; set; }
        [Display(Name = "Attachment")]
        public byte[] attachment { get; set; }
        [Display(Name = "File Type")]
        public string fileType { get; set; }
        [Display(Name = "Document Name")]
        public string documentName { get; set; }

    }

    /// <summary>
    /// ContractorSignoff
    /// </summary>
    [MetadataType(typeof(ContractorSignoffMetaData))]
    public partial class ContractorSignoff
    {
    }

    public class ContractorSignoffMetaData
    {
        [Display(Name = "Contractor Signoff")]
        public System.Guid contractorSignoffUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Created By")]
        public string aspNetUserUidAsCreated { get; set; }
        [Display(Name = "Date Created")]
        public Nullable<System.DateTime> dateCreated { get; set; }
        [Display(Name = "Signed By")]
        public string typedName { get; set; }
        [Display(Name = "Signoff Report")]
        public byte[] attachment { get; set; }
        [Display(Name = "File Type")]
        public string fileType { get; set; }
        [Display(Name = "Document Name")]
        public string documentName { get; set; }

    }


    /// <summary>
    /// ContractorDraw
    /// </summary>
    [MetadataType(typeof(ContractorDrawMetaData))]
    public partial class ContractorDraw
    {
    }

    public class ContractorDrawMetaData
    {
        [Display(Name = "Contractor Draw")]
        public System.Guid contractorDrawUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }
        [Display(Name = "Date Created")]
        public Nullable<System.DateTime> dateCreated { get; set; }

    }


    /// <summary>
    /// ContractorDraw
    /// </summary>
    [MetadataType(typeof(ContractorDrawScheduleMetaData))]
    public partial class ContractorDrawSchedule
    {
    }

    public class ContractorDrawScheduleMetaData
    {
        [Display(Name = "Contractor Draw Schedule")]
        public System.Guid contractorDrawScheduleUid { get; set; }
        [Display(Name = "Contractor Draw")]
        public System.Guid contractorDrawUid { get; set; }
        [Display(Name = "Draw Period")]
        public Nullable<int> drawPeriod { get; set; }
        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public Nullable<int> amount { get; set; }

    }


    /// <summary>
    /// ExpenseConstructionDraw
    /// </summary>
    [MetadataType(typeof(ExpenseConstructionDrawMetaData))]
    public partial class ExpenseConstructionDraw
    {
    }

    public class ExpenseConstructionDrawMetaData
    {
        [Display(Name = "Expense Construction Draw")]
        public System.Guid expenseConstructionDrawUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Expense Type")]
        public System.Guid expenseTypeUid { get; set; }
        [Display(Name = "Date Created")]
        public Nullable<System.DateTime> dateCreated { get; set; }

    }

    /// <summary>
    /// ExpenseType
    /// </summary>
    [MetadataType(typeof(ExpenseTypeMetaData))]
    public partial class ExpenseType
    {
    }

    public class ExpenseTypeMetaData
    {
        [Display(Name = "Expense Type")]
        public System.Guid expenseTypeUid { get; set; }
        [Display(Name = "Expense Type")]
        public string expenseType1 { get; set; }
        [Display(Name = "Expense Type Description")]
        public string expenseTypeDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// EquipmentCondition
    /// </summary>
    [MetadataType(typeof(EquipmentConditionMetaData))]
    public partial class EquipmentCondition
    {
    }

    public class EquipmentConditionMetaData
    {
        [Display(Name = "Equipment Condition")]
        public System.Guid equipmentConditionUid { get; set; }
        [Display(Name = "Equipment Condition")]
        public string equipmentCondition1 { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// ContractorSignoffFinal
    /// </summary>
    [MetadataType(typeof(ContractorSignoffFinalMetaData))]
    public partial class ContractorSignoffFinal
    {
    }

    public class ContractorSignoffFinalMetaData
    {
        [Display(Name = "Contractor Signoff Final")]
        public System.Guid contractorSignoffFinalUid { get; set; }
        [Display(Name = "Signed Name")]
        public string typedName { get; set; }
        [Display(Name = "Date Created")]
        public Nullable<System.DateTime> dateCreated { get; set; }
    }


    /// <summary>
    /// CompletionCategory
    /// </summary>
    [MetadataType(typeof(CompletionCategoryMetaData))]
    public partial class CompletionCategoryFinal
    {
    }

    public class CompletionCategoryMetaData
    {
        [Display(Name = "Completion Category")]
        public System.Guid completionCategoryUid { get; set; }
        [Display(Name = "Completion Category")]
        public string completionCategory1 { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }


    /// <summary>
    /// LocationCompletionCategory
    /// </summary>
    [MetadataType(typeof(LocationCompletionCategoryMetaData))]
    public partial class LocationCompletionCategoryFinal
    {
    }

    public class LocationCompletionCategoryMetaData
    {
        [Display(Name = "Location Completion Category")]
        public System.Guid locationCompletionCategoryUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }
        [Display(Name = "Completion Category")]
        public System.Guid completionCategoryUid { get; set; }
    }

    
    /// <summary>
    /// PECCost
    /// </summary>
    [MetadataType(typeof(PECCostMetaData))]
    public partial class PECCost
    {
    }

    public class PECCostMetaData
    {
        [Display(Name = "PEC Cost")]
        public System.Guid pecCostUid { get; set; }
        [Display(Name = "Email")]
        public string aspNetUserUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Week")]
        public System.Guid weekUid { get; set; }
        [Display(Name = "PEC Task")]
        public System.Guid pecTaskUid { get; set; }
        [Display(Name = "Expense Type")]
        public System.Guid pecExpenseTypeUid { get; set; }
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
    }

    /// <summary>
    /// PECExpenseType
    /// </summary>
    [MetadataType(typeof(PECExpenseTypeMetaData))]
    public partial class PECExpenseType
    {
    }

    public class PECExpenseTypeMetaData
    {
        [Display(Name = "PEC Expense Type")]
        public System.Guid pecExpenseTypeUid { get; set; }
        [Display(Name = "PEC Expense Type")]
        public string pecExpenseType1 { get; set; }
        [Display(Name = "Description")]
        public string pecExpenseTypeDescription { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// PECTask
    /// </summary>
    [MetadataType(typeof(PECTaskMetaData))]
    public partial class PECTask
    {
    }

    public class PECTaskMetaData
    {
        [Display(Name = "PEC Task")]
        public System.Guid pecTaskUid { get; set; }
        [Display(Name = "PEC Task")]
        public string pecTask1 { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// EquipmentAttributeTypeTask
    /// </summary>
    [MetadataType(typeof(EquipmentAttributeTypeTaskMetaData))]
    public partial class EquipmentAttributeTypeTask
    {
    }

    public class EquipmentAttributeTypeTaskMetaData
    {
        [Display(Name = "Equipment Type Task")]
        public System.Guid equipmentAttributeTypeTaskUid { get; set; }
        [Display(Name = "Equipment Type")]
        public System.Guid equipmentAttributeTypeUid { get; set; }
        [Display(Name = "Equipment Type Task")]
        public string equipmentAttributeTypeTask1 { get; set; }
        [Display(Name = "Description")]
        public string equipmentAttributeTypeTaskDescription { get; set; }
    }

    /// <summary>
    /// EquipmentTypeTaskAssignment
    /// </summary>
    [MetadataType(typeof(EquipmentTypeTaskAssignmentMetaData))]
    public partial class EquipmentTypeTaskAssignment
    {
    }

    public class EquipmentTypeTaskAssignmentMetaData
    {
        [Display(Name = "Equipment Type Task Assignment")]
        public System.Guid equipmentTypeTaskAssignmentUid { get; set; }
        [Display(Name = "ECM")]
        public System.Guid ecmUid { get; set; }
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }
        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }
        [Display(Name = "Equipment Type")]
        public System.Guid equipmentAttributeTypeUid { get; set; }
        [Display(Name = "Task")]
        public System.Guid equipmentAttributeTypeTaskUid { get; set; }
        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public Nullable<int> amount { get; set; }
    }
    /// <summary>
    /// EstmateOption
    /// </summary>
    [MetadataType(typeof(EstimateOptionMetaData))]
    public partial class EstimateOption
    {
    }

    public class EstimateOptionMetaData
    {
        [Display(Name = "Estimate Option")]
        public System.Guid estimateOptionUid { get; set; }
        [Display(Name = "Estimate Option")]
        public string EstimateOption1 { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// ContractorNoteType
    /// </summary>
    [MetadataType(typeof(ContractorNoteTypeMetaData))]
    public partial class ContractorNoteType
    {
    }

    public class ContractorNoteTypeMetaData
    {
        [Display(Name = "Note Type")]
        public System.Guid contractorNoteTypeUid { get; set; }
        [Display(Name = "Note Type")]
        public string contractorNoteType1 { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// ContractorNoteStatus
    /// </summary>
    [MetadataType(typeof(ContractorNoteStatuMetaData))]
    public partial class ContractorNoteStatu
    {
    }

    public class ContractorNoteStatuMetaData
    {
        [Display(Name = "Note Status")]
        public System.Guid contractorNoteStatusUid { get; set; }
        [Display(Name = "Note Status")]
        public string contractorNoteStatus { get; set; }
        [Display(Name = "Behavior Indicator")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// ContractorNote
    /// </summary>
    [MetadataType(typeof(ContractorNoteMetaData))]
    public partial class ContractorNote
    {
    }

    public class ContractorNoteMetaData
    {
        [Display(Name = "Note")]
        public System.Guid contractorNoteUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }
        [Display(Name = "Note Type")]
        public System.Guid contractorNoteTypeUid { get; set; }
        [Display(Name = "Note Status")]
        public System.Guid contractorNoteStatusUid { get; set; }
        [Display(Name = "Note")]
        public string contractorNote1 { get; set; }
        [Display(Name = "Denial Reason")]
        public string denialReason { get; set; }
    }

    /// <summary>
    /// MasterSchedule
    /// </summary>
    [MetadataType(typeof(MasterScheduleMetaData))]
    public partial class MasterSchedule
    {
        
    }

    public class MasterScheduleMetaData
    {
        [Display(Name = "Master Schedule")]
        public System.Guid masterScheduleUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Location")]
        public System.Guid locationUid { get; set; }
        [Display(Name = "Master Schedule")]
        public string masterSchedule1 { get; set; }
        [Display(Name = "Room")]
        public string room { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
    }


    /// <summary>
    /// ContractorSchedule
    /// </summary>
    [MetadataType(typeof(ContractorScheduleMetaData))]
    public partial class ContractorSchedule
    {

    }

    public class ContractorScheduleMetaData
    {
        [Display(Name = "Contractor Schedule")]
        public System.Guid contractorScheduleUid { get; set; }
        [Display(Name = "Master Schedule")]
        public System.Guid masterScheduleUid { get; set; }
        [Display(Name = "Contractor")]
        public System.Guid contractorUid { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> startDate { get; set; }
    }

    
    /// <summary>
    /// MasterSchedule
    /// </summary>
    [MetadataType(typeof(ContractorScheduleTaskMetaData))]
    public partial class ContractorScheduleTask
    {
        
    }

    public class ContractorScheduleTaskMetaData
    {
        [Display(Name = "Contractor Schedule Task")]
        public System.Guid contractorScheduleTaskUid { get; set; }
        [Display(Name = "Contractor Schedule")]
        public System.Guid contractorScheduleUid { get; set; }
        [Display(Name = "Master Schedule Task")]
        public System.Guid masterScheduleTaskUid { get; set; }
        [Display(Name = "Shift")]
        public Nullable<System.Guid> shiftUid { get; set; }
        [Display(Name = "Days To Complete")]
        public Nullable<int> daysToComplete { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Task Start Date")]
        public Nullable<System.DateTime> taskStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Task End Date")]
        public Nullable<System.DateTime> taskEndDate { get; set; }
    }

    /// <summary>
    /// SalesOpportunities
    /// </summary>
    [MetadataType(typeof(SalesOpportunitiesMetaData))]
    public partial class SalesOpportunities
    {

    }

    public class SalesOpportunitiesMetaData
    {
        [Display(Name = "Sales Opportunity")]
        public System.Guid salesOpportunityUid { get; set; }
        [Display(Name = "Sales Team")]
        public System.Guid salesTeamUid { get; set; }
        [Display(Name = "Opportunity")]
        public System.Guid opportunityUid { get; set; }
    }


    /// <summary>
    /// Opportunity
    /// </summary>
    [MetadataType(typeof(OpportunityMetaData))]
    public partial class Opportunity
    {

    }

    public class OpportunityMetaData
    {
        [Display(Name = "Opportunity")]
        public System.Guid opportunityUid { get; set; }
        [Display(Name = "Opportunity")]
        public string opportunity1 { get; set; }
        [Display(Name = "Opportunity Description")]
        public string opportunityDescription { get; set; }
        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> startDate { get; set; }
        [Display(Name = "Projected Project Cost")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public Nullable<int> projectedProjectSize { get; set; }
        [Display(Name = "Spent To Date")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public Nullable<int> spentToDate { get; set; }
    }


    /// <summary>
    /// SalesTeam
    /// </summary>
    [MetadataType(typeof(SalesTeamMetaData))]
    public partial class SalesTeam
    {

    }

    public class SalesTeamMetaData
    {
        [Display(Name = "Sales Team")]
        public System.Guid salesTeamUid { get; set; }
        [Display(Name = "Sales Team")]
        public string salesTeam1 { get; set; }
        [Display(Name = "Sales Team ID")]
        public string behaviorIndicator { get; set; }
    }

    /// <summary>
    /// SalesTeamMember
    /// </summary>
    [MetadataType(typeof(SalesTeamMemberMetaData))]
    public partial class SalesTeamMember
    {

    }

    public class SalesTeamMemberMetaData
    {
        [Display(Name = "Sales Team Member")]
        public System.Guid salesTeamMemberUid { get; set; }
        [Display(Name = "Sales Team Member")]
        public string aspNetUserUid { get; set; }
        [Display(Name = "Sales Team")]
        public System.Guid salesTeamUid { get; set; }
    }


    /// <summary>
    /// SalesOpportunityTasks
    /// </summary>
    [MetadataType(typeof(SalesOpportunityTaskMetaData))]
    public partial class SalesOpportunityTask
    {

    }

    public class SalesOpportunityTaskMetaData
    {
        [Display(Name = "Sales Opportunity Task")]
        public System.Guid salesOpportunityTaskUid { get; set; }
        [Display(Name = "Sales Opportunity")]
        public System.Guid salesOpportunityUid { get; set; }
        [Display(Name = "Sales Opportunity Task")]
        public string SalesOpportunityTask1 { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime week { get; set; }
        public bool isCompleted { get; set; }
    }

    /// <summary>
    /// SalesOpportunityTasks
    /// </summary>
    [MetadataType(typeof(ProjectTaskListMetaData))]
    public partial class ProjectTaskList
    {

    }

    public class ProjectTaskListMetaData
    {
        [Display(Name = "Task")]
        public System.Guid projectTaskListUid { get; set; }
        [Display(Name = "Project")]
        public System.Guid projectUid { get; set; }
        [Display(Name = "Category")]
        public Nullable<System.Guid> projectTaskListUidAsParent { get; set; }
        [Display(Name = "Task Predecessor")]
        public Nullable<System.Guid> projectTaskListUidAsPredecessor { get; set; }
        [Display(Name = "Task")]
        public string projectTask { get; set; }
        [Display(Name = "Sequence")]
        public int projectTaskSequence { get; set; }
        [Display(Name = "Start Date")]
        public System.DateTime projectTaskStartDate { get; set; }
        [Display(Name = "Duration")]
        public int projectTaskDuration { get; set; }
    }


    /// <summary>
    /// CommissionIssue
    /// </summary>
    [MetadataType(typeof(CommissionIssueMetaData))]
    public partial class CommissionIssue
    {
    }

    public class CommissionIssueMetaData
    {
        [Display(Name = "Commission Issue")]
        public System.Guid commissionIssueUid { get; set; }
        [Display(Name = "Equipment")]
        public System.Guid equipmentUid { get; set; }
        [Display(Name = "Status")]
        public System.Guid commissionStatusUid { get; set; }
        [Display(Name = "Assigned To")]
        public Nullable<System.Guid> projectUserUid { get; set; }
        [Display(Name = "Issue Description")]
        public string commissionIssue1 { get; set; }
        [Display(Name = "Created By")]
        public string aspNetUserUidAsCreated { get; set; }
        [Display(Name = "Date Created")]
        public System.DateTime date { get; set; }
    }
    

}
