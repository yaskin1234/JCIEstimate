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
    
    public partial class ContractorSignoffFinalAttachment
    {
        public System.Guid contractorSignoffFinalAttachmentUid { get; set; }
        public System.Guid contractorSignoffFinalUid { get; set; }
        public string aspNetUserUidAsCreated { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public string contractorSignoffFinalAttachment1 { get; set; }
        public byte[] attachment { get; set; }
        public string fileType { get; set; }
        public string documentName { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ContractorSignoffFinal ContractorSignoffFinal { get; set; }
    }
}
