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
    
    public partial class EquipmentTaskDetail
    {
        public EquipmentTaskDetail()
        {
            this.EquipmentTaskDetailItems = new HashSet<EquipmentTaskDetailItem>();
        }
    
        public System.Guid equipmentTaskDetailUid { get; set; }
        public System.Guid equipmentTaskUid { get; set; }
        public string equipmentTaskDetail1 { get; set; }
        public Nullable<int> sequence { get; set; }
        public bool isScheduleList { get; set; }
        public bool isEquipmentExpected { get; set; }
        public bool isWarrantyStart { get; set; }
    
        public virtual EquipmentTask EquipmentTask { get; set; }
        public virtual ICollection<EquipmentTaskDetailItem> EquipmentTaskDetailItems { get; set; }
    }
}
