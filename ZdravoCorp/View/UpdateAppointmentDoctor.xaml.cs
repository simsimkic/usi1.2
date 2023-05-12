﻿using System;
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
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for UpdateAppointmentDoctor.xaml
    /// </summary>
    public partial class UpdateAppointmentDoctor : Window
    {
        public UpdateAppointmentDoctor(ObservableCollection<AppointmentViewModel> _appointments, AppointmentViewModel SelectedAppointment)
        {
            InitializeComponent();
            DataContext = new DoctorAppointmentUpdateFormViewModel(this, _appointments, SelectedAppointment);
        }
    }
}
