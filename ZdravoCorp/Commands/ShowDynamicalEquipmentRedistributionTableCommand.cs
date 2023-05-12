using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.View.Table;
using ZdravoCorp.ViewModel.Table;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Structure;

namespace ZdravoCorp.Commands
{
    public class ShowDynamicalEquipmentRedistributionTableCommand : CommandBase
    {
        private readonly DirectorViewModel _directorViewModel;

        public ShowDynamicalEquipmentRedistributionTableCommand(DirectorViewModel directorViewModel)
        {
            _directorViewModel = directorViewModel;
        }

        public override void Execute(object? parameter)
        {
            var equipmentTableView = new DynamicalEquipmentRedistributionTableView();
            equipmentTableView.DataContext = new EquipmentRedistributionTableViewModel();
            equipmentTableView.ShowDialog();
        }
    }
}
