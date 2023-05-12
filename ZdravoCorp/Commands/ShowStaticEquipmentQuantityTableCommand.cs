using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;
using ZdravoCorp.View.Table;

namespace ZdravoCorp.Commands
{
    public class ShowStaticEquipmentQuantityTableCommand : CommandBase
    {
        private readonly DirectorViewModel _directorViewModel;

        public ShowStaticEquipmentQuantityTableCommand(DirectorViewModel directorViewModel)
        {
            _directorViewModel = directorViewModel;

        }

        public override void Execute(object? parameter)
        {
            var equipmentFilterTableView = new StaticEquipmentQuantityTableView();
            equipmentFilterTableView.DataContext = new EquipmentFilterTableViewModel(_directorViewModel.Director.Hospital);
            equipmentFilterTableView.ShowDialog();
        }
    }
}
