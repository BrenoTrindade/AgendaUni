using System.Globalization;

namespace AgendaUni.Common.Extensions
{
    public static class DayOfWeekExtensions
    {
        private static readonly Dictionary<DayOfWeek, string> DiasDaSemana = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Sunday, "Domingo" },
            { DayOfWeek.Monday, "Segunda-feira" },
            { DayOfWeek.Tuesday, "Terça-feira" },
            { DayOfWeek.Wednesday, "Quarta-feira" },
            { DayOfWeek.Thursday, "Quinta-feira" },
            { DayOfWeek.Friday, "Sexta-feira" },
            { DayOfWeek.Saturday, "Sábado" }
        };

        public static string ToPortuguese(this DayOfWeek dayOfWeek)
        {
            return DiasDaSemana.TryGetValue(dayOfWeek, out var name) ? name : dayOfWeek.ToString();
        }
    }
}
