using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JCIEstimate.Models
{
    public partial class MasterSchedule
    {
        public IEnumerable<MasterScheduleTask> GetTasksForMaster()
        {
            return this.MasterScheduleTasks.Where(c => c.masterScheduleUid == this.masterScheduleUid).OrderBy(c=>c.taskSequence);
        }        
    }

    public partial class ContractorSchedule
    {       

        public IEnumerable<ContractorScheduleTask> GetTasksForContractor()
        {
            return this.ContractorScheduleTasks.Where(c => c.contractorScheduleUid == contractorScheduleUid).OrderBy(c=>c.MasterScheduleTask.taskSequence);
        }
    }

    public partial class Equipment
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        public IEnumerable<EquipmentToDo> GetToDosForEquipment()
        {
            return this.EquipmentToDoes.Where(c => c.equipmentUid == this.equipmentUid);
        }

        public IEnumerable<EquipmentAttribute> GetAttributesForEquipment()
        {
            return db.EquipmentAttributes.Where(c => c.equipmentAttributeTypeUid == this.equipmentAttributeTypeUid);
        }

        public EquipmentAttributeValue GetEquipmentAttributeValueForEquipmentAttribute(Guid equipmentAttributeUid)
        {
            return db.EquipmentAttributeValues.Where(c => c.equipmentAttributeUid == equipmentAttributeUid).Where(c=>c.equipmentUid == this.equipmentUid).FirstOrDefault();
        }

        public string isTaskForEquipment(Guid equipmentTaskUid)
        {
            foreach (var toDo in this.GetToDosForEquipment())
            {
                if (toDo.equipmentTaskUid == equipmentTaskUid)
                {
                    return "CHECKED";
                }
            }
            return "";
        }
    }
}