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
    
    public partial class ContractorNote
    {
        public System.Guid contractorNoteUid { get; set; }
        public System.Guid projectUid { get; set; }
        public System.Guid contractorUid { get; set; }
        public System.Guid contractorNoteTypeUid { get; set; }
        public System.Guid contractorNoteStatusUid { get; set; }
        public string contractorNote1 { get; set; }
        public string denialReason { get; set; }
    
        public virtual Contractor Contractor { get; set; }
        public virtual ContractorNoteStatu ContractorNoteStatu { get; set; }
        public virtual ContractorNoteType ContractorNoteType { get; set; }
        public virtual Project Project { get; set; }
    }
}
