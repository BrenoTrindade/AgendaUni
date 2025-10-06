using AgendaUni.Models;
using System.Globalization;
using AgendaUni.Common.Extensions;

namespace AgendaUni.ViewModels
{
    public class ClassDisplayViewModel : BaseViewModel
    {
        private readonly Class _class;

        public Class Class => _class;

        public int Id => _class.Id;
        public string ClassName => _class.ClassName;
        public int MaximumAbsences => _class.MaximumAbsences;
        public ICollection<Absence> Absences => _class.Absences;

        public ClassDisplayViewModel(Class @class)
        {
            _class = @class;
        }

        public double AbsencePercentage
        {
            get
            {
                if (MaximumAbsences == 0) return 0;
                return (double)(_class.Absences?.Count ?? 0) / MaximumAbsences;
            }
        }

        public string NextClassInfo
        {
            get
            {
                var now = DateTime.Now;
                var today = now.DayOfWeek;
                var currentTime = now.TimeOfDay;

                var nextSchedule = _class.Schedules?
                                .Where(s => s.DayOfWeek != today || s.ClassTime > currentTime)
                                .OrderBy(s => ((int)s.DayOfWeek - (int)today + 7) % 7)
                                .ThenBy(s => s.ClassTime)
                                .FirstOrDefault();
                if (nextSchedule != null)
                {
                    string dayOfWeekInPortuguese = nextSchedule.DayOfWeek.ToPortuguese();
                    return $"{dayOfWeekInPortuguese} às {nextSchedule.ClassTime:hh\\:mm}";
                }
                return "Nenhum horário cadastrado";
            }
        }

        public string NextEventInfo
        {
            get
            {
                var now = DateTime.Now;
                var nextEvent = _class.Events?.OrderBy(s => s.EventDate).FirstOrDefault();

                if (nextEvent != null)
                {
                    return $"{nextEvent.Description} em {nextEvent.EventDate:dd/MM}";
                }
                return "Nenhum evento cadastrado";
            }
        }
    }
}
