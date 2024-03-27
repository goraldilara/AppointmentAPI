using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AppointmentAPI.Models.DoctorModel;
using Newtonsoft.Json;
using static AppointmentAPI.Models.VisitModel;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using AppointmentAPI.Models;
using System.Text;

namespace AppointmentAPI.Controllers
{
    
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        //constant value for api addresses
        Uri baseURL = new Uri("https://a93ced42-c421-4f38-a0ee-25fc667483c0.mock.pstmn.io");

        //lists and fields that are needed in the requests
        List<Doctor> doclist = new List<Doctor>();
        Doctor selectedDoc = null;
        List<Visit> visitlist = new List<Visit>();
        Appointment appointment = null;
        int bookingId = 0;

        //Set an appointment
        [Route("api/SetAppointment")]
        [HttpPost]
        public JsonResult CreateAppointment(string patientInfo)
        {
            try
            {
                string patientName = patientInfo.Split(' ')[0];
                string patientSurname = patientInfo.Split(" ")[1];

                using (var client = new HttpClient())
                {
                    //setting url to fetch doctors
                    Uri fetchDoctorsURL = new Uri(baseURL, "/fetchDoctors");
                    //getting json data from the url
                    var docResult = client.GetAsync(fetchDoctorsURL).Result;
                    var docJson = docResult.Content.ReadAsStringAsync().Result;

                    //deserialization of the json data that contains doctors' information
                    DoctorData docData = JsonConvert.DeserializeObject<DoctorData>(docJson);

                    //detection of existence of the data
                    if (docData.data == null)
                    {
                        //create a response object to return status and return it
                        Response responseObj = new Response("NO AVAILABLE DOCTOR");
                        return new JsonResult(responseObj);
                    }
                    else
                    {
                        //get doctor objects
                        doclist = docData.data;

                        //iterate in doctor list
                        foreach (var item in doclist)
                        {
                            //get data from doctor with accurate name
                            if (item.name.Equals("Yasemin Öztürk"))
                            {
                                selectedDoc = item;
                                break;
                            }
                        }
                    }

                    //setting url to fetch schedule of the selected doctor
                    Uri fetchScheduleURL = new Uri(baseURL, "/fetchSchedules?doctorId=" + selectedDoc.doctorId);

                    //setting url to try on false data
                    //Uri fetchScheduleURL = new Uri(baseURL, "/fetchSchedules?doctorId=" + 1);

                    //getting json data from the url
                    var scheduleResult = client.GetAsync(fetchScheduleURL).Result;
                    var scheduleJson = scheduleResult.Content.ReadAsStringAsync().Result;

                    //deserialization of the json data that comes from the endpoint
                    VisitData visitdata = JsonConvert.DeserializeObject<VisitData>(scheduleJson);
                    if (visitdata.data == null)
                    {
                        //create a response object to return status and return it
                        Response responseObj = new Response("NO AVAILABLE APPOINTMENT");
                        return new JsonResult(responseObj);
                    }
                    else
                    {
                        //get visit objects
                        visitlist = visitdata.data;

                        //iterate in visit list
                        foreach (var item in visitlist)
                        {
                            //get data from visit with given id
                            if (item.VisitId == 551231)
                            {
                                //creating the appointment object
                                appointment = new Appointment(item.VisitId, item.startTime.ToString("HH:mm"), item.endTime.ToString("HH:mm"), item.startTime.ToString("dd/MM/yyyy").Replace('.', '/'), patientName, patientSurname, selectedDoc.hospitalId, selectedDoc.doctorId, Convert.ToInt32(selectedDoc.branchId));
                                break;
                            }
                        }
                    }

                    //creating the url string with the accurate parameters
                    string appointmentURL = "VisitId=" + appointment.VisitId + "&startTime=" + appointment.startTime + "&endTime=" + appointment.endTime + "&date=" + appointment.date + "&PatientName=" + appointment.PatientName + "&PatientSurname=" + appointment.PatientSurname + "&hospitalId=" + appointment.hospitalId + "&doctorId=" + appointment.doctorId + "&branchId=" + appointment.branchId;

                    //setting url to POST operation of the appointment object
                    Uri bookVisitURL = new Uri(baseURL, "/bookVisit?" + appointmentURL);

                    //setting url to try on false data/without parameters
                    //Uri bookVisitURL = new Uri(baseURL, "/bookVisit?");

                    //creating the json object for POST operation
                    var newPostJson = JsonConvert.SerializeObject(appointment);
                    var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");

                    //get result of the post operation from api
                    var result = client.PostAsync(bookVisitURL, payload).Result.Content.ReadAsStringAsync().Result;
                    string resultStr = result.ToString();
                    resultStr = resultStr.Replace("\n", "").Replace(" ", "").Replace("\"", "").Replace("{", "").Replace("}", "");
                    bookingId = Convert.ToInt32(resultStr.Split(",")[1].Split(":")[1]);

                    //return response according to the status of the operation
                    if (result.Contains("true"))
                    {
                        Response responseObj = new Response(appointment, "true", bookingId);
                        return new JsonResult(responseObj);
                    }
                    else
                    {
                        Response responseObj = new Response(appointment, "false");
                        return new JsonResult(responseObj);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("False data type");
            }
            return null;
        }

        //Delete an appointment
        [Route("api/CancelAppointment")]
        [HttpPost]
        public JsonResult DeleteAppointment(int bookingId)
        {
            Uri deleteURL = new Uri(baseURL, "/bookVisit?BookingID=" + bookingId);

            using(var client = new HttpClient())
            {
                //creating the json object for POST operation
                var newPostJson = JsonConvert.SerializeObject(bookingId);
                var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");

                //get result of the post operation from api
                var result = client.PostAsync(deleteURL, payload).Result.Content.ReadAsStringAsync().Result;

                //return response according to the status of the operation
                if (result.Contains("true"))
                {
                    Response responseObj = new Response("true");
                    return new JsonResult(responseObj);
                }
                else
                {
                    Response responseObj = new Response("false");
                    return new JsonResult(responseObj);
                }
            }
        }

        //Get Doctor Data
        [Route("api/FindDoctors")]
        [HttpPost]
        public JsonResult FindDoctors()
        {
            //doctor list for Turkish doctors
            List<Doctor> doctors = new List<Doctor>();

            using(var client = new HttpClient())
            {
                //setting url to fetch doctors
                Uri fetchDoctorsURL = new Uri(baseURL, "/fetchDoctors");
                //getting json data from the url
                var docResult = client.GetAsync(fetchDoctorsURL).Result;
                var docJson = docResult.Content.ReadAsStringAsync().Result;

                //deserialization of the json data that contains doctors' information
                DoctorData docData = JsonConvert.DeserializeObject<DoctorData>(docJson);

                //detection of existence of the data
                if (docData.data == null)
                {
                    //create a response object to return status and return it
                    Response responseObj = new Response("NO DOCTOR");
                    return new JsonResult(responseObj);
                }
                else
                {
                    //get doctor objects
                    doclist = docData.data;

                    //write doctor data into a csv file in local
                    using(StreamWriter writer = new StreamWriter("DoctorsOutput.csv"))
                    {
                        //iterate in doctor list
                        foreach (var item in doclist)
                        {
                            //get data from doctors who are from Turkey
                            if (item.nationality.Equals("TUR"))
                            {
                                if (item.gender.Equals("Female"))
                                {
                                    item.gender = "Kadın";
                                }
                                else if (item.gender.Equals("Male"))
                                {
                                    item.gender = "Erkek";
                                }
                                else
                                {
                                    item.gender = "Diğer";
                                }
                                doctors.Add(item);
                                //add line to the csv file
                                writer.WriteLine($"{item.createdAt},{item.name},{item.gender},{item.hospitalName},{item.hospitalId},{item.specialtyId},{item.branchId},{item.nationality},{item.doctorId}");
                            }
                        }
                    }
                }
            }
            return new JsonResult(doctors);
        }
    }
}
