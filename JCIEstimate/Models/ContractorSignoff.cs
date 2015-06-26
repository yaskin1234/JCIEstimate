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
    
    public partial class ContractorSignoff
    {
        public ContractorSignoff()
        {
            this.ContractorSignoffAttachments = new HashSet<ContractorSignoffAttachment>();
        }
    
        public System.Guid contractorSignoffUid { get; set; }
        public System.Guid projectUid { get; set; }
        public string aspNetUserUidAsCreated { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public string typedName { get; set; }
        public byte[] attachment { get; set; }
        public string fileType { get; set; }
        public string documentName { get; set; }
        public System.Guid contractorUid { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<ContractorSignoffAttachment> ContractorSignoffAttachments { get; set; }
        public virtual Contractor Contractor { get; set; }
    }
}
