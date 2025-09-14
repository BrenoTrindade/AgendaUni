namespace AgendaUni.Web.Models
{
    public class Tasks
    {
        public int ID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
        public string Caption { get; set; }
        public string Comment { get; set; }
    }
}