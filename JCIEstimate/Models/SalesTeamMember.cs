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
    
    public partial class SalesTeamMember
    {
        public System.Guid salesTeamMemberUid { get; set; }
        public string aspNetUserUid { get; set; }
        public System.Guid salesTeamUid { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual SalesTeam SalesTeam { get; set; }
    }
}