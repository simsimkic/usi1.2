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
using ZdravoCorp.Model;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.View.Form
{
    /// <summary>
    /// Interaction logic for UrgentAppointmentFormView.xaml
    /// </summary>
    public partial class UrgentAppointmentFormView : Window
    {
        public UrgentAppointmentFormView(PatientViewModel selectedPatient)
        {
            InitializeComponent();
            DataContext = new UrgentAppointmentFormViewModel(selectedPatient);
        }
    }
}
