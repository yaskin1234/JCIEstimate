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
    
    public partial class ExpenseType
    {
        public ExpenseType()
        {
            this.ExpenseConstructions = new HashSet<ExpenseConstruction>();
            this.ExpenseConstructions1 = new HashSet<ExpenseConstruction>();
            this.ExpenseMonthlies = new HashSet<ExpenseMonthly>();
            this.ExpenseConstructionDraws = new HashSet<ExpenseConstructionDraw>();
        }
    
        public System.Guid expenseTypeUid { get; set; }
        public string expenseType1 { get; set; }
        public string expenseTypeDescription { get; set; }
        public string behaviorIndicator { get; set; }
    
        public virtual ICollection<ExpenseConstruction> ExpenseConstructions { get; set; }
        public virtual ICollection<ExpenseConstruction> ExpenseConstructions1 { get; set; }
        public virtual ICollection<ExpenseMonthly> ExpenseMonthlies { get; set; }
        public virtual ICollection<ExpenseConstructionDraw> ExpenseConstructionDraws { get; set; }
    }
}