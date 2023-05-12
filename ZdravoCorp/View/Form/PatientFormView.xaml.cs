using System.Windows;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for PatientFormView.xaml
    /// </summary>
    public partial class PatientFormView : Window
    {
        public PatientFormView(PatientTableViewModel patientTableViewModel)
        {
            InitializeComponent();
            DataContext = new PatientFormViewModel(this, patientTableViewModel);
        }
    }
}
