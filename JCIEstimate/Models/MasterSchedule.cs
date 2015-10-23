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
    
    public partial class MasterSchedule
    {
        public MasterSchedule()
        {
            this.ContractorSchedules = new HashSet<ContractorSchedule>();
            this.MasterScheduleTasks = new HashSet<MasterScheduleTask>();
        }
    
        public System.Guid masterScheduleUid { get; set; }
        public System.Guid projectUid { get; set; }
        public System.Guid locationUid { get; set; }
        public string masterSchedule1 { get; set; }
        public string room { get; set; }
        public string description { get; set; }
    
        public virtual ICollection<ContractorSchedule> ContractorSchedules { get; set; }
        public virtual Location Location { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<MasterScheduleTask> MasterScheduleTasks { get; set; }
    }
}
