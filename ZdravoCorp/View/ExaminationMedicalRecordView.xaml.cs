using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.ViewModel;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for ExaminationMedicalRecordView.xaml
    /// </summary>
    public partial class ExaminationMedicalRecordView : Window
    {
        public ExaminationMedicalRecordView(PatientViewModel patient, AppointmentViewModel appointment)
        {
            InitializeComponent();
            DataContext = new ExaminationMedicalRecordViewModel(patient, appointment);
        }
    }
}
