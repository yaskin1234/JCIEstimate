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
    
    public partial class ContractorDraw
    {
        public ContractorDraw()
        {
            this.ContractorDrawSchedules = new HashSet<ContractorDrawSchedule>();
        }
    
        public System.Guid contractorDrawUid { get; set; }
        public System.Guid projectUid { get; set; }
        public System.Guid contractorUid { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
    
        public virtual Contractor Contractor { get; set; }
        public virtual ICollection<ContractorDrawSchedule> ContractorDrawSchedules { get; set; }
        public virtual Project Project { get; set; }
    }
}
