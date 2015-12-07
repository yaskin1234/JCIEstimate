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


    public partial class CalendarDay
    {

        public IEnumerable<CalendarDayTask> GetTasksForCalendarDay(Guid? projectUid)
        {
            var tasks = from cc in this.CalendarDayTasks
                        where cc.calendarDayUid == calendarDayUid
                        where (projectUid != null && projectUid != Guid.Empty ? cc.projectUid == projectUid : 1 == 1)
                        orderby ( cc.Project == null ? null : cc.Project.project1), (cc.Location == null ? null : cc.Location.location1 )
                        select cc;
            return tasks;
        }

        public int GetTasksCountForCalendarDay(Guid? projectUid)
        {
            var tasks = from cc in this.CalendarDayTasks
                        where cc.calendarDayUid == calendarDayUid
                        where (projectUid != null && projectUid != Guid.Empty ? cc.projectUid == projectUid : 1 == 1)
                        orderby (cc.Project == null ? null : cc.Project.project1), (cc.Location == null ? null : cc.Location.location1)
                        select cc;
            return tasks.Count();
        }
    }

    public partial class ProjectCalendarDay
    {

        public IEnumerable<ProjectCalendarDayTask> GetTasksForProjectCalendarDay(Guid? projectUid)
        {
            var tasks = from cc in this.ProjectCalendarDayTasks
                        where cc.projectCalendarDayUid == projectCalendarDayUid
                        where (projectUid != null && projectUid != Guid.Empty ? cc.projectUid == projectUid : 1 == 1)
                        orderby (cc.Project == null ? null : cc.Project.project1), (cc.Location == null ? null : cc.Location.location1)
                        select cc;
            return tasks;
        }

        public int GetTasksCountForProjectCalendarDay(Guid? projectUid)
        {
            var tasks = from cc in this.ProjectCalendarDayTasks
                        where cc.projectCalendarDayUid == projectCalendarDayUid
                        where (projectUid != null && projectUid != Guid.Empty ? cc.projectUid == projectUid : 1 == 1)
                        orderby (cc.Project == null ? null : cc.Project.project1), (cc.Location == null ? null : cc.Location.location1)
                        select cc;
            return tasks.Count();
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