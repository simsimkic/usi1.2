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
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.View.Form
{
    /// <summary>
    /// Interaction logic for MedicalRecordView.xaml
    /// </summary>
    public partial class MedicalRecordFormView : Window
    {
        public MedicalRecordFormView(PatientViewModel selectedPatient)
        {
            InitializeComponent();
            DataContext = new MedicalRecordFormViewModel(selectedPatient);
        }
    }
}
