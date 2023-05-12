using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.ViewModel
{
    public  class DoctorViewModel : ViewModelBase
    {
        public  Doctor Doctor; 
        public string FullName => Doctor.FirstName + " " +Doctor.LastName;
        public Specialization Specialization => Doctor.Specialization;
        public DoctorViewModel(Doctor doctor)
        {
            Doctor = doctor;
        }
    }
}