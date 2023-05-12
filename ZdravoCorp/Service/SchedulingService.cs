using System;
using System.Collections.Generic;

using System.Data;

using System.Diagnostics;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Service
{
    public class SchedulingService
    {
        public DAO<Doctor> _doctorDAO = DAOFactory.GetInstance().DoctorDAO;
        private static DAO<DoctorSchedule> _doctorScheduleDAO = DAOFactory.GetInstance().DoctorScheduleDAO;
        public SchedulingService() {}

        public static void CreateUrgentAppointment(Doctor doctor, Patient patient, TimeSlot timeSlot, bool isOperation)
        {
            var doctorSchedule = GetFromDAOService.GetScheduleById(doctor.Id);
            var doctorScheduleService = new DoctorScheduleService(doctorSchedule);
            doctorScheduleService.AddAppointment(new Appointment(doctor, patient, new TimeSlot(timeSlot), isOperation, doctorSchedule.GetNextAppointmentId()));
        }

        public TimeSlot GetTaken(Specialization specialization)
        {
            return null;
        }

        public static bool IsAvailable(Doctor doctor, TimeSlot timeSlot)
        {
            foreach(Appointment appointment in GetAllAppointments(doctor))
            {
                if (appointment.IsCanceled)
                {
                    continue;
                }
                if (timeSlot.OverlapsWith(appointment.TimeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsAvailable(Patient patient, TimeSlot timeSlot)
        {
            foreach (Appointment appointment in GetAllAppointments(patient))
            {
                if (appointment.IsCanceled)
                {
                    continue;
                }
                if (timeSlot.OverlapsWith(appointment.TimeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsAvailableForUpdate(Doctor doctor, TimeSlot timeSlot, Appointment appointmentToChange)
        {
            foreach (Appointment appointment in GetAllAppointments(doctor))
            {
                if (appointment.Id == appointmentToChange.Id || appointment.IsCanceled)
                {
                    continue;
                }
                if (timeSlot.OverlapsWith(appointment.TimeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsAvailableForUpdate(Patient patient, TimeSlot timeSlot, Appointment appointmentToChange)
        {
            foreach (Appointment appointment in GetAllAppointments(patient))
            {
                if (appointment.Id == appointmentToChange.Id || appointment.IsCanceled)
                {
                    continue;
                }
                if (timeSlot.OverlapsWith(appointment.TimeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Appointment> GetAllAppointments(Patient patient)
        {
            List<Appointment> allAppointments = new List<Appointment>();
            foreach (DoctorSchedule doctorSchedule in _doctorScheduleDAO.GetAll().Values)
            {
                var docSchedualService = new DoctorScheduleService(doctorSchedule);
                foreach (Appointment appointment in docSchedualService.GetAllAppointments())
                {
                    if (appointment.Patient is not null && patient.Id == appointment.Patient.Id)
                    {
                        allAppointments.Add(appointment);
                    }
                }
            }

            return allAppointments;
        }

        public static List<Appointment> GetAllAppointments(Doctor doctor)
        {
            List<Appointment> allAppointments = new List<Appointment>();

            if (_doctorScheduleDAO.GetAll().ContainsKey(doctor.Id))
            {
                foreach (List<Appointment> appointments in _doctorScheduleDAO.GetAll()[doctor.Id].Appointments.Values)
                {
                    allAppointments.AddRange(appointments);
                }
            }

            return allAppointments;
        }

        public static List<Appointment> GetAllAppointments(Patient patient, Doctor doctor)
        {
            List<Appointment> allAppointments = new List<Appointment>();
            foreach (DoctorSchedule doctorSchedule in _doctorScheduleDAO.GetAll().Values)
            {
                if(doctorSchedule.Id == doctor.Id)
                {
                    foreach(List<Appointment> appointments in doctorSchedule.Appointments.Values)
                    {
                        allAppointments.AddRange(appointments);
                    }
                }
            }

            return allAppointments;
        }
        public static List<TimeSlot> GetClosestTimeSlot(Doctor doctor, TimeSlot span, DateTime latestTime) 
        {
            List<TimeSlot> closestTimeSlot = new DoctorScheduleService(_doctorScheduleDAO.GetAll()[doctor.Id]).GetClosestTimeSlots(span, latestTime);
            if (closestTimeSlot.Count == 1 && span.Contains(closestTimeSlot[0]))
            {
                return closestTimeSlot;
            }
            foreach(DoctorSchedule doctorSchedule in _doctorScheduleDAO.GetAll().Values)
            {
                closestTimeSlot = new DoctorScheduleService(doctorSchedule).GetClosestTimeSlots(span, latestTime);
                if (closestTimeSlot.Count == 1 && span.Contains(closestTimeSlot[0])) { break; }
            }
            return closestTimeSlot;
        }

        public static Tuple<TimeSlot, Doctor> GetFirstFreeTimeSlotAndDoctor(Specialization specialization, TimeSlot span, int duration)
        {
            TimeSlot? firstFree = null;
            Doctor? freeDoctor = null;
            foreach (Doctor doctor in GetFromDAOService.GetAllDoctors().Values)
            {
                if (doctor.Specialization != specialization) { continue; };

                var doctorScheduleService = new DoctorScheduleService(GetFromDAOService.GetScheduleById(doctor.Id));
                firstFree = doctorScheduleService.GetFirstFree(span, duration);

                if (firstFree != null) { freeDoctor = doctor; break; }
            }

            if (firstFree == null || freeDoctor == null) { return null; }

            return new Tuple<TimeSlot, Doctor>(firstFree, freeDoctor);
        }

        // Change the way the arguments are passed  
        public static List<KeyValuePair<int, Appointment>> GetDelayableAppointments(Specialization specialization, TimeSlot initialSpan, TimeSlot toDelaySpan, int duration, int numberOfDelayable)
        {
            var delayable = GetFromDAOService.GetAllDoctorSchedulesBy(specialization).Values
                .SelectMany(ds => new DoctorScheduleService(ds).GetDelayableAppointments(initialSpan, toDelaySpan, duration, numberOfDelayable))
                .OrderBy(kv => kv.Key)
                .Take(numberOfDelayable)
                .ToList();
            return delayable;
        }

        public static List<Patient> GetAllPatients(Doctor doctor)
        {
            List<Appointment> allAppointments = GetAllAppointments(doctor);
            List<Patient> allPatients = new List<Patient>();

            foreach (Appointment appointment in allAppointments)
            {
                if (!allPatients.Any(p => p.Id == appointment.Patient.Id))
                {
                    allPatients.Add(appointment.Patient);
                }
            }

            return allPatients;
        }
    }
}
