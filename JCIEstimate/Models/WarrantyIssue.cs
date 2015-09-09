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
    
    public partial class WarrantyIssue
    {
        public WarrantyIssue()
        {
            this.WarrantyAttachments = new HashSet<WarrantyAttachment>();
            this.WarrantyNotes = new HashSet<WarrantyNote>();
        }
    
        public System.Guid warrantyIssueUid { get; set; }
        public Nullable<System.Guid> warrantyUnitUid { get; set; }
        public System.Guid warrantyStatusUid { get; set; }
        public string warrantyIssueLocation { get; set; }
        public string warrantyIssue1 { get; set; }
        public System.DateTime date { get; set; }
        public Nullable<System.Guid> projectUserUid { get; set; }
        public string aspNetUserUidAsCreated { get; set; }
        public Nullable<System.Guid> locationUid { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<WarrantyAttachment> WarrantyAttachments { get; set; }
        public virtual WarrantyStatu WarrantyStatu { get; set; }
        public virtual WarrantyUnit WarrantyUnit { get; set; }
        public virtual ICollection<WarrantyNote> WarrantyNotes { get; set; }
        public virtual ProjectUser ProjectUser { get; set; }
    }
}
