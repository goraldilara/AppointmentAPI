namespace AppointmentAPI.Models
{
    public class VisitModel
    {
        public class Visit
        {
            public int doctorId { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public int VisitId { get; set; }
            public string id { get; set; }
        }

        public class VisitData
        {
            public List<Visit> data { get; set;}
        }
    }
}
