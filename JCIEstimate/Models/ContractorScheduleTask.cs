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
    
    public partial class ContractorScheduleTask
    {
        public System.Guid contractorScheduleTaskUid { get; set; }
        public System.Guid contractorScheduleUid { get; set; }
        public System.Guid masterScheduleTaskUid { get; set; }
        public Nullable<System.Guid> shiftUid { get; set; }
        public Nullable<System.DateTime> taskStartDate { get; set; }
        public Nullable<System.DateTime> taskEndDate { get; set; }
        public Nullable<int> daysToComplete { get; set; }
    
        public virtual ContractorSchedule ContractorSchedule { get; set; }
        public virtual MasterScheduleTask MasterScheduleTask { get; set; }
        public virtual Shift Shift { get; set; }
    }
}