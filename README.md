# AppointmentAPI
An ASP.NET Core Web API based booking API that allows users to monitor available doctors, set and delete appointments

General info
This project is a backend project that has these 2 main operations: Booking/Canceling Appointments, Reaching available doctors' list

Technologies
The project is created with:

ASP.NET Core Web API (6.0)

Usage
The response of the backend can be obtained by:

POSTMAN/Swagger - 

POST /api/SetAppointment with Body that has the patient name and surname
POST /api/CancelAppointment with Body that has the booking ID
POST /api/FindDoctors shows doctor with Turkish nationality

![image](https://github.com/goraldilara/AppointmentAPI/assets/33300564/449c7c20-ffeb-4adc-8a9f-5b1af9560271)

