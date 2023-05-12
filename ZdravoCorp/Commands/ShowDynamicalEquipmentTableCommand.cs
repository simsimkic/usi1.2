using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.View.Table;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class ShowDynamicalEquipmentTableCommand : CommandBase
    {
        private readonly DirectorViewModel _directorViewModel;

        public ShowDynamicalEquipmentTableCommand(DirectorViewModel directorViewModel)
        {
            _directorViewModel = directorViewModel;
        }

        public override void Execute(object? parameter)
        {
            var equipmentFilterTableView = new DynamicalEquipmentTableView();
            equipmentFilterTableView.DataContext = new DynamicalEquipmentTableViewModel(_directorViewModel.Director.Hospital);
            equipmentFilterTableView.ShowDialog();
        }
    }
}
