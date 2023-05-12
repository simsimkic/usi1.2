using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.View.Table;

namespace ZdravoCorp.Service
{
    public static class NotificationService
    {
        public static void NotifyDoctor(int id)
        {
            var notifications= DAOFactory.GetInstance().NotificationDAO.GetAll().Values.Where(o => o.DoctorID == id).ToList();
            if (notifications.Any())
            {
                foreach (Notification notification in notifications)
                {
                    if (notification.Delayed is null) { ShowNewMessage(notification); continue; }
                    ShowDelayedMessage(notification);
                }
            }
        }

        public static void NotifyPatient(int id)
        {
            var notifications = DAOFactory.GetInstance().NotificationDAO.GetAll().Values.Where(o => o.PatientID == id).ToList();
            if (notifications.Any())
            {
                foreach (Notification notification in notifications)
                {
                    if (notification.Delayed is null) { ShowNewMessage(notification); continue; }
                    ShowDelayedMessage(notification);
                }
            }
        }

        public static void CreatDelayedAppointmentNotification(int doctorId,int patientId, TimeSlot initial, TimeSlot delayed)
        {
            DAOFactory.GetInstance().NotificationDAO.Add(new Notification(doctorId, patientId, initial, delayed));
        }

        public static void CreateNewAppointmentNotification(int doctorId, int patientId, TimeSlot timeSlot)
        {
            DAOFactory.GetInstance().NotificationDAO.Add(new Notification(doctorId, patientId, timeSlot, null));
        }

        public static void ShowDelayedMessage(Notification notification)
        {
            MessageBox.Show($"Odložen vam je pregled sa {notification.Initial.From} na {notification.Delayed.From}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowNewMessage(Notification notification)
        {
            MessageBox.Show($"Zakazan vam je pregled za {notification.Initial.From}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }
}
