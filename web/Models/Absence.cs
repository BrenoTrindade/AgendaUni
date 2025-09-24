using System;

namespace AgendaUni.Web.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public DateTime AbsenceDate { get; set; }
        public string AbsenceReason { get; set; }
        public int ClassId { get; set; }
    }
}