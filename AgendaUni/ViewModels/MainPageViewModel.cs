using AgendaUni.Models;
using AgendaUni.Services;
using Plugin.Maui.Calendar.Models;
using System.Globalization;
using System.Windows.Input;

namespace AgendaUni.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly AbsenceService _absenceService;
        private readonly ClassScheduleService _classScheduleService;
        private readonly ClassService _classService;
        private readonly EventService _eventService;
        public EventCollection Events { get; set; }
        public ICommand FilterCalendarCommand { get; }
        public bool ShowAllSelected { get; set; } = true;
        public bool ShowEventsSelected { get; set; }
        public bool ShowClassesSelected { get; set; }
        public bool ShowAbsencesSelected { get; set; }
        public CultureInfo Culture => new CultureInfo("pt-BR");

        List<ClassEvent> classEvents;
        public MainPageViewModel(ClassService classService, AbsenceService absenceService, ClassScheduleService classScheduleService, EventService eventService)
        {
            _classService = classService;
            _absenceService = absenceService;
            _classScheduleService = classScheduleService;
            _eventService = eventService;
            Events = new Plugin.Maui.Calendar.Models.EventCollection();
            FilterCalendarCommand = new Command<string>(FilterCalendar);
        }
        
        public class ClassEvent
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public EventType Type { get; set; }
        }
        public enum EventType
        {
            Absence,
            ClassSchedule,
            Event
        }
        private async void FilterCalendar(string filter)
        {
            Events.Clear();


            var classes = await _classService.GetAllClassesAsync();

            if (filter == "All")
            {
                ShowAllSelected = true;

                var absences = await _absenceService.GetAllAbsencesAsync();
                var schedules = await _classScheduleService.GetAllClassSchedulesAsync();
                var events = await _eventService.GetAllEventsAsync();

                AddClassSchedules(classes, schedules.ToList());
                AddAbsences(classes, absences.ToList());
                AddEvents(classes, events.ToList());
            }
            if(filter == "Events")
            {
                var events = await _eventService.GetAllEventsAsync();
                AddEvents(classes, events.ToList());
            }
            if (filter == "Classes")
            {
                var schedules = await _classScheduleService.GetAllClassSchedulesAsync();
                AddClassSchedules(classes, schedules.ToList());
            }
            if (filter == "Absences")
            {
                var absences = await _absenceService.GetAllAbsencesAsync();
                AddAbsences(classes, absences.ToList());
            }
            ShowAllSelected = filter == "All";
            ShowEventsSelected = filter == "Events";
            ShowClassesSelected = filter == "Classes";
            ShowAbsencesSelected = filter == "Absences";
            OnPropertyChanged(nameof(ShowAllSelected));
            OnPropertyChanged(nameof(ShowEventsSelected));
            OnPropertyChanged(nameof(ShowClassesSelected));
            OnPropertyChanged(nameof(ShowAbsencesSelected));
            
        }

        public override async Task OnAppearingAsync()
        {
            Events.Clear();
            var absences = await _absenceService.GetAllAbsencesAsync();
            var schedules = await _classScheduleService.GetAllClassSchedulesAsync();
            var classes = await _classService.GetAllClassesAsync();
            var events = await _eventService.GetAllEventsAsync();

            AddClassSchedules(classes, schedules.ToList());
            AddAbsences(classes, absences.ToList());
            AddEvents(classes, events.ToList());


            OnPropertyChanged(nameof(Events));
        }

        public void AddAbsences(List<Class> classes, List<Absence> absences)
        {
            foreach (var absence in absences)
            {
                var classObj = classes.FirstOrDefault(c => c.Id == absence.ClassId);
                var date = absence.AbsenceDate.Date;
                if (Events.ContainsKey(absence.AbsenceDate.Date))
                    classEvents = Events[date].Cast<ClassEvent>().ToList();
                else
                    classEvents = new List<ClassEvent>();

                classEvents.Add(
                        new ClassEvent
                        {
                            Name = $"Falta: {classObj?.ClassName ?? "Aula"}",
                            Description = $"Motivo: {absence.AbsenceReason}",
                            Type = EventType.Absence
                        }
                    );
                Events[date] = classEvents;
            }
        }
        public void AddEvents(List<Class> classes, List<Event> events)
        {
            foreach (var ev in events)
            {
                var classObj = classes.FirstOrDefault(c => c.Id == ev.ClassId);
                var date = ev.EventDate.Date;
                if (Events.ContainsKey(ev.EventDate.Date))
                    classEvents = Events[date].Cast<ClassEvent>().ToList();
                else
                    classEvents = new List<ClassEvent>();

                classEvents.Add(
                        new ClassEvent
                        {
                            Name = $"Evento: {classObj?.ClassName ?? "Aula"}",
                            Description = ev.Description,
                            Type = EventType.Event
                        }
                    );
                Events[date] = classEvents;
            }
        }
        public void AddClassSchedules(List<Class> classes, List<ClassSchedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                var classObj = classes.FirstOrDefault(c => c.Id == schedule.ClassId);
                var today = DateTime.Today;
                for (int i = 0; i < 30; i++)
                {
                    var date = today.AddDays(i);
                    if (date.DayOfWeek == schedule.DayOfWeek)
                    {
                        if (Events.ContainsKey(date.Date))
                            classEvents = Events[date].Cast<ClassEvent>().ToList();
                        else
                            classEvents = new List<ClassEvent>();

                        classEvents.Add(new ClassEvent
                        {
                            Name = classObj?.ClassName ?? "Aula",
                            Description = $"Horário: {schedule.ClassTime:hh\\:mm}",
                            Type = EventType.ClassSchedule
                        });
                        Events[date] = classEvents;
                    }
                }
            }
        }
    }
}
