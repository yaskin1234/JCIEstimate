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
    
    public partial class EquipmentTask
    {
        public EquipmentTask()
        {
            this.EquipmentToDoes = new HashSet<EquipmentToDo>();
        }
    
        public System.Guid equipmentTaskUid { get; set; }
        public string equipmentTask1 { get; set; }
        public string equipmentTaskDescription { get; set; }
    
        public virtual ICollection<EquipmentToDo> EquipmentToDoes { get; set; }
    }
}
