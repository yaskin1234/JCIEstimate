using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "ECM")]
        public System.Guid ecmUid { get; set; }

        [Display(Name = "Project Line Of Work")]
        public System.Guid projectLineOfWorkUid { get; set; }

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
        [Display(Name = "Material Bid")]
        public decimal materialBid { get; set; }
        [Display(Name = "Labor Bid")]
        public decimal laborBid { get; set; }
        [Display(Name = "Bond Amount")]
        public decimal bondAmount { get; set; }
        [Display(Name = "Notes")]
        public string notes { get; set; }
        [Display(Name = "Delivery Weeks")]
        public Nullable<int> deliveryWeeks { get; set; }
        [Display(Name = "Installation Weeks")]
        public Nullable<int> installationWeeks { get; set; }
    }
}