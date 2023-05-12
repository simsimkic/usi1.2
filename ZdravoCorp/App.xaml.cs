using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DAOFactory DaoFactory => DAOFactory.GetInstance();
        public App()
        {
            
        }

   
        protected override void OnStartup(StartupEventArgs e)
        {

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();
            PeriodicTaskAsync();
            base.OnStartup(e);
        }
        async Task PeriodicTaskAsync()
        {
            while (true)
            {
                ThreadService.CallAllThread();
                await Task.Delay(60000);
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DaoFactory.DoctorDAO.SaveAll();
            DaoFactory.PatientDAO.SaveAll();
            DaoFactory.NurseDAO.SaveAll();
            DaoFactory.DirectorDAO.SaveAll();
            DaoFactory.DoctorScheduleDAO.SaveAll();

        }

    }
}
