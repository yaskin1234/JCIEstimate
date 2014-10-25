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
    
    public partial class Project
    {
        public Project()
        {
            this.Locations = new HashSet<Location>();
            this.ECMs = new HashSet<ECM>();
            this.ExpenseConstructions = new HashSet<ExpenseConstruction>();
            this.ExpenseMiscellaneous = new HashSet<ExpenseMiscellaneou>();
            this.ExpensePercentages = new HashSet<ExpensePercentage>();
            this.ExpenseTravels = new HashSet<ExpenseTravel>();
        }
    
        public System.Guid projectUid { get; set; }
        public string project1 { get; set; }
        public string projectDescription { get; set; }
    
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<ECM> ECMs { get; set; }
        public virtual ICollection<ExpenseConstruction> ExpenseConstructions { get; set; }
        public virtual ICollection<ExpenseMiscellaneou> ExpenseMiscellaneous { get; set; }
        public virtual ICollection<ExpensePercentage> ExpensePercentages { get; set; }
        public virtual ICollection<ExpenseTravel> ExpenseTravels { get; set; }
    }
}
