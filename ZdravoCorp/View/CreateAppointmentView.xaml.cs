using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoCorp.Model;
using ZdravoCorp.ViewModel;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for CreateAppointmentView.xaml
    /// </summary>
    public partial class CreateAppointmentView : Window
    {
        public CreateAppointmentView(Patient patient, ObservableCollection<AppointmentViewModel> appointments)
        {
            InitializeComponent();
            DataContext = new PatientAppointmentCreateFormViewModel(this, patient, appointments);
        }
    }
}
