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
    
    public partial class ProjectToDo
    {
        public System.Guid projectToDoUid { get; set; }
        public string projectToDo1 { get; set; }
        public string projectToDoDescription { get; set; }
        public System.Guid toDoStatusUid { get; set; }
        public System.DateTime dateCreated { get; set; }
        public Nullable<System.DateTime> dateResolved { get; set; }
    
        public virtual ToDoStatu ToDoStatu { get; set; }
    }
}
