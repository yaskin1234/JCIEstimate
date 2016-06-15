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
    
    public partial class ProjectTaskList
    {
        public ProjectTaskList()
        {
            this.ProjectTaskList1 = new HashSet<ProjectTaskList>();
            this.ProjectTaskList11 = new HashSet<ProjectTaskList>();
        }
    
        public System.Guid projectTaskListUid { get; set; }
        public System.Guid projectUid { get; set; }
        public Nullable<System.Guid> projectTaskListUidAsParent { get; set; }
        public Nullable<System.Guid> projectTaskListUidAsPredecessor { get; set; }
        public string projectTask { get; set; }
        public int projectTaskSequence { get; set; }
        public System.DateTime projectTaskStartDate { get; set; }
        public int projectTaskDuration { get; set; }
        public string aspNetUserUidAsAssigned { get; set; }
        public bool isCompleted { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<ProjectTaskList> ProjectTaskList1 { get; set; }
        public virtual ProjectTaskList ProjectTaskList2 { get; set; }
        public virtual ICollection<ProjectTaskList> ProjectTaskList11 { get; set; }
        public virtual ProjectTaskList ProjectTaskList3 { get; set; }
        public virtual Project Project { get; set; }
    }
}
