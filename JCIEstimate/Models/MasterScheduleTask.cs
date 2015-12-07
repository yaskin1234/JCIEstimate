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
    
    public partial class MasterScheduleTask
    {
        public MasterScheduleTask()
        {
            this.ContractorScheduleTasks = new HashSet<ContractorScheduleTask>();
        }
    
        public System.Guid masterScheduleTaskUid { get; set; }
        public System.Guid masterScheduleUid { get; set; }
        public string taskName { get; set; }
        public Nullable<int> taskSequence { get; set; }
        public string masterScheduleIdAsPredecessors { get; set; }
    
        public virtual MasterSchedule MasterSchedule { get; set; }
        public virtual ICollection<ContractorScheduleTask> ContractorScheduleTasks { get; set; }
    }
}