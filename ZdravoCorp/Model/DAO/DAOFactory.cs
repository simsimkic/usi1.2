using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace ZdravoCorp.Model.DAO
{
    public class DAOFactory
    {
        private const string PatientFileName = @"..\..\..\Data\patient.json";
        private const string DoctorFileName = @"..\..\..\Data\doctor.json";
        private const string DoctorScheduleFileName = @"..\..\..\Data\doctorSchedule.json";
        private const string DirectorFileName = @"..\..\..\Data\director.json";
        private const string NurseFileName = @"..\..\..\Data\nurse.json";
        private const string DynamicalEquipmentRequestFIleName = @"..\..\..\Data\dynamicalEquipmentRequest.json";
        private const string NotificationFileName = @"..\..\..\Data\notification.json";


        private DAO<Patient> _patientDAO;
        private DAO<Doctor> _doctorDAO;
        private DAO<Nurse> _nurseDAO;
        private DAO<DoctorSchedule> _doctorScheduleDAO;
        private DynamicalEquipmentRequestDAO _dynamicalRequestDAO;
        private DirectorDAO _directorDAO;
        private DAO<Notification> _notificationDAO;

        public DAO<Patient> PatientDAO { get { return _patientDAO; } }
        public DAO<Doctor> DoctorDAO { get { return _doctorDAO; } }

        public DirectorDAO DirectorDAO { get { return _directorDAO; } }
        public DynamicalEquipmentRequestDAO DynamicalRequestDAO { get { return _dynamicalRequestDAO; } }
        public DAO<Nurse> NurseDAO { get { return _nurseDAO; } }
        public DAO<DoctorSchedule> DoctorScheduleDAO { get { return _doctorScheduleDAO; } }
        public DAO<Notification> NotificationDAO { get { return _notificationDAO; } }

        

        private static DAOFactory _instance;
        private DAOFactory() 
        
        {
            _patientDAO = new DAO<Patient>(PatientFileName);
            _doctorDAO = new DAO<Doctor>(DoctorFileName);
            _directorDAO = new DirectorDAO(DirectorFileName);
            _nurseDAO = new DAO<Nurse>(NurseFileName);
            _doctorScheduleDAO = new DAO<DoctorSchedule>(DoctorScheduleFileName);
            _dynamicalRequestDAO = new DynamicalEquipmentRequestDAO(DynamicalEquipmentRequestFIleName);
            _notificationDAO = new DAO<Notification>(NotificationFileName);
        }

        public static DAOFactory GetInstance()
        {
            if(_instance is null)
            {
                _instance = new DAOFactory();
            }

            return _instance;
        }

        public bool IsUnique(string username)
        {
            return !_patientDAO.GetAll().Values.Any(u => u.Username == username) && !_doctorDAO.GetAll().Values.Any(u => u.Username == username);
        }

        public bool IsUnique(string username, int id)
        {
            return !_patientDAO.GetAll().Values.Any(u => u.Username == username && u.Id != id)
                   && !_doctorDAO.GetAll().Values.Any(u => u.Username == username && u.Id != id);
        }

    }
}
