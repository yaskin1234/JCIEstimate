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
    
    public partial class EquipmentAttribute
    {
        public EquipmentAttribute()
        {
            this.EquipmentAttributeValues = new HashSet<EquipmentAttributeValue>();
        }
    
        public System.Guid equipmentAttributeUid { get; set; }
        public System.Guid equipmentAttributeTypeUid { get; set; }
        public System.Guid appDataTypeTypeUid { get; set; }
        public string equipmentAttribute1 { get; set; }
    
        public virtual AppDataType AppDataType { get; set; }
        public virtual EquipmentAttributeType EquipmentAttributeType { get; set; }
        public virtual ICollection<EquipmentAttributeValue> EquipmentAttributeValues { get; set; }
    }
}
