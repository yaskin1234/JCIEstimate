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
    using System.ComponentModel.DataAnnotations;
    
    public partial class Estimate
    {
        public System.Guid estimateUid { get; set; }
        public System.Guid locationUid { get; set; }
        public System.Guid ecmUid { get; set; }
        public System.Guid categoryUid { get; set; }
        public Nullable<bool> isActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Nullable<decimal> materialBid { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Nullable<decimal> laborBid { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Nullable<decimal> bondAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Nullable<decimal> total { get; set; }
        public string notes { get; set; }
        public Nullable<int> deliveryWeeks { get; set; }
        public Nullable<int> installationWeeks { get; set; }
        public System.Guid contractorUid { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ECM ECM { get; set; }
        public virtual Location Location { get; set; }
        public virtual Contractor Contractor { get; set; }
    }
}
