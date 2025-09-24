using System.ComponentModel.DataAnnotations;

namespace AgendaUni.Web.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int MaximumAbsences { get; set; }
    }
}