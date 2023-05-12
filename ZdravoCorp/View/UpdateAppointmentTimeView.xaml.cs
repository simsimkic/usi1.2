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
    /// Interaction logic for UpdateAppointmentTimeView.xaml
    /// </summary>
    public partial class UpdateAppointmentTimeView : Window
    {
        public UpdateAppointmentTimeView(ObservableCollection<AppointmentViewModel> appointments, AppointmentViewModel selectedAppointment)
        {
            InitializeComponent();
            DataContext = new PatientAppointmentUpdateFormViewModel(this, appointments, selectedAppointment);
        }
    }
}
