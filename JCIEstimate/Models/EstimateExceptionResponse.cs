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
    
    public partial class EstimateExceptionResponse
    {
        public System.Guid estimateExceptionResponseUid { get; set; }
        public int estimateExceptionResponseID { get; set; }
        public System.Guid estimateExceptionUid { get; set; }
        public string estimateExceptionResponse1 { get; set; }
        public System.DateTime responseDate { get; set; }
    
        public virtual EstimateException EstimateException { get; set; }
    }
}
