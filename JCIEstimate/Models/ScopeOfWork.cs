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
    
    public partial class ScopeOfWork
    {
        public System.Guid scopeOfWorkUid { get; set; }
        public System.Guid projectUid { get; set; }
        public string scopeOfWork1 { get; set; }
        public string scopeOfWorkDescription { get; set; }
        public byte[] document { get; set; }
    
        public virtual Project Project { get; set; }
    }
}
