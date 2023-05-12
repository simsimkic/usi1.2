using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using ZdravoCorp.Model;

namespace ZdravoCorp.Service
{
    internal class DoctorScheduleService
    {
        private DoctorSchedule _doctorSchedule;

        public DoctorScheduleService(DoctorSchedule doctorSchedule)
        {
            _doctorSchedule = doctorSchedule;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _doctorSchedule.Appointments.Values.SelectMany(x => x).ToList();
        }

        public List<Appointment> GetAllAppointments(DateOnly date)
        {
            return _doctorSchedule.Appointments[date];
        }

        public void UpdateAppointment(Appointment appointmentForUpdate)
        {
            foreach (var appointment in GetAllAppointments(appointmentForUpdate.TimeSlot.GetDate()))
            {
                if(appointment.Id == appointmentForUpdate.Id)
                {
                    appointment.TimeSlot = appointmentForUpdate.TimeSlot;
                    appointment.IsOperation = appointmentForUpdate.IsOperation;
                    break;
                }
            }
        }

        public void AddFree(TimeSlot timeSlot) 
        {
            _doctorSchedule.FreeTimeSlots[timeSlot.GetDate()].Add(timeSlot);
        }

        public void RemoveFree(TimeSlot timeSlot) 
        {
            _doctorSchedule.FreeTimeSlots[timeSlot.GetDate()].Remove(timeSlot);
        }

        public List<TimeSlot> GetAllFree(DateOnly date)
        {
            return _doctorSchedule.FreeTimeSlots[date];
        }

        public void ReserveFree(TimeSlot timeSlot)
        {
            foreach (TimeSlot free in GetAllFree(timeSlot.GetDate())) 
            {
                if (!free.Contains(timeSlot)) { continue; }

                var after = free.Split(new TimeSlot(timeSlot));
                var before = free;
                AdjustFree(before, after);
                
                return;
            }
        }

        public void FreeReserved(TimeSlot timeSlot)
        {
            _doctorSchedule.FreeTimeSlots[timeSlot.GetDate()].Add(timeSlot);
        }

        private void AdjustFree(TimeSlot before, TimeSlot after)
        {
            if (!before.IsShorterThan(15) && !after.IsShorterThan(15)) { AddFree(after); }
            else if (before.IsShorterThan(15) && !after.IsShorterThan(15)) { RemoveFree(before); AddFree(after); }
            else if (before.IsShorterThan(15) && after.IsShorterThan(15)) { RemoveFree(before); }
        }

        public TimeSlot GetFirstFree(TimeSlot span, int duration)
        {
            TimeSlot firstFree = null;

            var allFree = GetAllFree(span.GetDate());

            foreach (TimeSlot free in allFree) 
            {
                if (!span.OverlapsWith(free)) { continue; }

                if (free.GetOverlapDuration(span) < duration) { continue; }
                
                firstFree = span.GetOverlap(free);
                firstFree.CutTo(duration);

                break;
            }
            return firstFree;
        }

        public TimeSlot FindNextFree(TimeSlot restOfTheDay, TimeSlot workHours, int duration)
        {
            var nextFree = GetFirstFree(restOfTheDay, duration);

            while(nextFree is null)
            {
                workHours.AddDays(1);
                nextFree = GetFirstFree(workHours, duration);
            }

            return nextFree;
        }

        public List<KeyValuePair<int, Appointment>> GetDelayableAppointments(TimeSlot initialSpan, TimeSlot toDelaySpan, int duration, int numberOfDelayable)
        {
            var appointments = GetAllAppointments(initialSpan.GetDate());

            var delayable = appointments
                .Where(appointment => initialSpan.Contains(appointment.TimeSlot) && !appointment.TimeSlot.IsShorterThan(duration))
                .Select(appointment => new KeyValuePair<int, Appointment>(CalculateDelayedAppointmentStartTimeDiff(appointment, toDelaySpan), appointment))
                .OrderBy(pair => pair.Key)
                .Take(numberOfDelayable)
                .ToList();

            return delayable;
        }

        public List<TimeSlot> GetAllReserved(TimeSlot timeSlot)
        {
            // returns all taken timeslots in a given timeslot interval 

            return null;
        }

        public int CalculateDelayedAppointmentStartTimeDiff(Appointment appointment, TimeSlot span)
        {
            var delayed = GetFirstFree(span, appointment.TimeSlot.GetDuration());
            return appointment.TimeSlot.GetStartTimeDiff(delayed);
        }
        
        public List<TimeSlot> GetClosestTimeSlots(TimeSlot span, DateTime latestTime)
        {
            List<TimeSlot> closestTimeSlots = new List<TimeSlot>();

            closestTimeSlots = GetClosestInSpan(span, latestTime);
            if (closestTimeSlots.Count == 0)
            {
                span.From = new DateTime(span.From.Year, span.From.Month, span.From.Day, 8, 0, 0);
                span.To = new DateTime(span.To.Year, span.To.Month, span.To.Day, 20, 0, 0);

                closestTimeSlots = GetClosestInSpan(span, latestTime);
                if(closestTimeSlots.Count == 0) { return closestTimeSlots; }
            }
            closestTimeSlots = GetClosestOutOfSpan(span, latestTime);

            return closestTimeSlots;
        }
        public List<TimeSlot> GetClosestInSpan(TimeSlot span, DateTime latestTime)
        {
            List<TimeSlot> closestTimeSlots = new List<TimeSlot>();
            DateTime startDate = DateTime.Now.Date;
            while (startDate != latestTime.AddDays(1).Date)
            {
                var firstFree = GetFirstFree(span, 15);
                if (firstFree is not null)
                {
                    closestTimeSlots.Add(firstFree);
                    break;
                }
                span.From.AddDays(1);
                span.To.AddDays(1);
                startDate = startDate.AddDays(1);
            }
            return closestTimeSlots;
        }

        public List<TimeSlot> GetClosestOutOfSpan(TimeSlot span, DateTime latestTime)
        {
            List<TimeSlot> closestTimeSlots = new List<TimeSlot>();
            while (closestTimeSlots.Count != 3)
            {
                var firstFree = GetFirstFree(span, 15);
                if (firstFree is not null)
                {
                    closestTimeSlots.Add(firstFree);
                    break;
                }
                span.From.AddDays(1);
                span.To.AddDays(1);
            }
            return closestTimeSlots;
        }


        public void AddAppointment(Appointment appointment)
        {
            ReserveFree(appointment.TimeSlot);
            _doctorSchedule.Appointments[appointment.TimeSlot.GetDate()].Add(appointment);
        }

        public void UnscheduleAppointment(Appointment appointment) 
        {
            appointment.IsCanceled = true;
            _doctorSchedule.FreeTimeSlots[appointment.TimeSlot.GetDate()].Add(appointment.TimeSlot);
        }

        public void RemoveAppointment(Appointment appointment)
        {
            _doctorSchedule.Appointments[appointment.TimeSlot.GetDate()].Remove(appointment);
        }

        public void RescheduleAppointment(Appointment appointment, TimeSlot newTimeSlot)
        {
            RemoveAppointment(appointment);
            FreeReserved(appointment.TimeSlot);
            appointment.TimeSlot = newTimeSlot;
            AddAppointment(appointment);
        }
    }
}
