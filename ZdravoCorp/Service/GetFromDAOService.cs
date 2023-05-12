using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel;

namespace ZdravoCorp.Service
{
    public class GetFromDAOService
    {
        private static DAOFactory _daoFactory = DAOFactory.GetInstance();

        public static Patient GetPatientById(int id)
        {
            return _daoFactory.PatientDAO.GetById(id);
        }

        public static Doctor GetDoctorById(int id)
        {
            return _daoFactory.DoctorDAO.GetById(id);
        }

        public static DoctorSchedule GetScheduleById(int id)
        {
            return _daoFactory.DoctorScheduleDAO.GetById(id);
        }

        public static List<Patient> GetAllPatients()
        {
            return _daoFactory.PatientDAO.GetAll().Values.ToList();
        }

        public static Dictionary<int, Doctor> GetAllDoctors()
        {
            return _daoFactory.DoctorDAO.GetAll();
        }

        public static Dictionary<int, DoctorSchedule> GetAllDoctorSchedules()
        {
            return _daoFactory.DoctorScheduleDAO.GetAll();
        }

        public static Dictionary<int, DoctorSchedule> GetAllDoctorSchedulesBy(Specialization specialization)
        {
            var doctorSchedules = GetAllDoctorSchedules();
            var doctorSchedulesBySpecialization = GetAllDoctors()
                .Values
                .Where(doctor => doctor.Specialization == specialization)
                .ToDictionary(doctor => doctor.Id, doctor => GetScheduleById(doctor.Id));

            return doctorSchedulesBySpecialization;
        }
    }
}
