namespace AppointmentAPI.Models
{
    public class Appointment
    {
        public int VisitId { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string date { get; set; }
        public string PatientName { get; set; }
        public string PatientSurname { get; set;}
        public int hospitalId { get; set; }
        public string doctorId { get; set; }
        public int branchId { get; set; }

        public Appointment(int VisitId, string startTime, string endTime, string date, string PatientName, string PatientSurname, int hospitalId, string doctorId, int branchId)
        {
            this.VisitId = VisitId;
            this.startTime = startTime;
            this.endTime = endTime;
            this.date = date;
            this.PatientName = PatientName;
            this.PatientSurname = PatientSurname;
            this.hospitalId = hospitalId;
            this.doctorId = doctorId;
            this.branchId = branchId;
        }

        public Appointment()
        {
        }
    }
}
