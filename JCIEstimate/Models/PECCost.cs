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
    
    public partial class PECCost
    {
        public System.Guid pecCostUid { get; set; }
        public string aspNetUserUid { get; set; }
        public System.Guid projectUid { get; set; }
        public System.Guid weekUid { get; set; }
        public System.Guid pecTaskUid { get; set; }
        public System.Guid pecExpenseTypeUid { get; set; }
        public int quantity { get; set; }
    
        public virtual PECExpenseType PECExpenseType { get; set; }
        public virtual Week Week { get; set; }
        public virtual PECTask PECTask { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Project Project { get; set; }
    }
}
