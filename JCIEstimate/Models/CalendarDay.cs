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
    
    public partial class CalendarDay
    {
        public CalendarDay()
        {
            this.CalendarDayTasks = new HashSet<CalendarDayTask>();
        }
    
        public System.Guid calendarDayUid { get; set; }
        public System.Guid calendarUid { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    
        public virtual Calendar Calendar { get; set; }
        public virtual ICollection<CalendarDayTask> CalendarDayTasks { get; set; }
    }
}
