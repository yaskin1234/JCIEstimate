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
    
    public partial class ProjectUser
    {
        public ProjectUser()
        {
            this.WarrantyIssues = new HashSet<WarrantyIssue>();
        }
    
        public System.Guid projectUserUid { get; set; }
        public string aspNetUserUid { get; set; }
        public System.Guid projectUid { get; set; }
        public bool isReceivingWarrantyEmail { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual ICollection<WarrantyIssue> WarrantyIssues { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
