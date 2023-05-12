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
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for DoctorsPatients.xaml
    /// </summary>
    public partial class DoctorsPatients : Window
    {
        public DoctorsPatients(Doctor doctor)
        {
            InitializeComponent();
            DataContext = new DoctorPatientsTableViewModel(doctor);
        }
    }
}
