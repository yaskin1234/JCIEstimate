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
    
    public partial class HeatType
    {
        public HeatType()
        {
            this.Equipments = new HashSet<Equipment>();
        }
    
        public System.Guid heatTypeUid { get; set; }
        public string heatType1 { get; set; }
        public string heatTypeDescription { get; set; }
        public string behaviorIndicator { get; set; }
    
        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
