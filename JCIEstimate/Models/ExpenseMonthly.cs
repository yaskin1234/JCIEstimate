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
    
    public partial class ExpenseMonthly
    {
        public System.Guid expenseMonthlyUid { get; set; }
        public System.Guid projectUid { get; set; }
        public string expenseMonthly1 { get; set; }
        public string expenseMonthlyDescription { get; set; }
        public decimal ratePerDay { get; set; }
        public int daysPerMonth { get; set; }
        public int projectDurationInMonths { get; set; }
        public Nullable<decimal> total { get; set; }
    
        public virtual Project Project { get; set; }
    }
}