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
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.View.Table
{
    /// <summary>
    /// Interaction logic for DelayableAppointmentsTableView.xaml
    /// </summary>
    public partial class DelayableAppointmentTableView : Window
    {
        public DelayableAppointmentTableView(UrgentAppointmentFormViewModel urgentAppointmentFormViewModel)
        {
            InitializeComponent();
            DataContext = new DelayableAppointmentTableViewModel(urgentAppointmentFormViewModel);
        }
    }
}
