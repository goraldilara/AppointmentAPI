namespace AppointmentAPI.Models
{
    public class Response
    {
        public Appointment appointment { get; set; }
        public string status { get; set; }
        public int bookingId { get; set; }

        public Response(Appointment appointment, string status, int bookingId)
        {
            this.appointment = appointment;
            this.status = status;
            this.bookingId = bookingId;
        }

        public Response(Appointment appointment, string status)
        {
            this.appointment = appointment;
            this.status = status;
        }

        public Response(string status)
        {
            this.status = status;
        }
    }
}
