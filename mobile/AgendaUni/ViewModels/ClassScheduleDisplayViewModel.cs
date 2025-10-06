using AgendaUni.Models;
using AgendaUni.Common.Extensions;

namespace AgendaUni.ViewModels
{
    public class ClassScheduleDisplayViewModel : BaseViewModel
    {
        public ClassSchedule ClassSchedule { get; }

        public int Id => ClassSchedule.Id;
        public TimeSpan ClassTime => ClassSchedule.ClassTime;
        public string DayOfWeekInPortuguese => ClassSchedule.DayOfWeek.ToPortuguese();

        public ClassScheduleDisplayViewModel(ClassSchedule classSchedule)
        {
            ClassSchedule = classSchedule;
        }
    }
}