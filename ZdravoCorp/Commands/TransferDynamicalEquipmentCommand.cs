using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Structure;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class TransferDynamicalEquipmentCommand : CommandBase
    {
        private readonly EquipmentRedistributionTableViewModel _equipmentRedistributionTableViewModel;

        public TransferDynamicalEquipmentCommand(EquipmentRedistributionTableViewModel equipmentRedistributionTableViewModel)
        {
            _equipmentRedistributionTableViewModel = equipmentRedistributionTableViewModel;
            _equipmentRedistributionTableViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {    return (_equipmentRedistributionTableViewModel.SelectedElementFrom is not null) 
                         && (_equipmentRedistributionTableViewModel.SelectedElementTo is not null)
                         && (_equipmentRedistributionTableViewModel.SelectedElementFrom.RoomID != _equipmentRedistributionTableViewModel.SelectedElementTo.RoomID)
                         && (_equipmentRedistributionTableViewModel.SelectedElementFrom.Type == _equipmentRedistributionTableViewModel.SelectedElementTo.Type)
                         && (_equipmentRedistributionTableViewModel.TransferQuantity != 0)
                         && (_equipmentRedistributionTableViewModel.TransferQuantity <= _equipmentRedistributionTableViewModel.SelectedElementFrom.Quantity);
        }

        public override void Execute(object? parameter)
        {
            DirectorDAO.TransferDynamicalEquipment(_equipmentRedistributionTableViewModel.SelectedElementFrom,
                                                    _equipmentRedistributionTableViewModel.SelectedElementTo,
                                                    _equipmentRedistributionTableViewModel.TransferQuantity);
            _equipmentRedistributionTableViewModel.EquipmentRedistributions = new ObservableCollection<EquipmentRedistributionViewModel>(EquipmentService.GetEquipmentRedistributions());
            _equipmentRedistributionTableViewModel.SelectedElementTo = null;
            _equipmentRedistributionTableViewModel.SelectedElementFrom = null;
            _equipmentRedistributionTableViewModel.TransferQuantity = 0;
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(_equipmentRedistributionTableViewModel.SelectedElementFrom) ||
                e.PropertyName == nameof(_equipmentRedistributionTableViewModel.SelectedElementTo) ||
                e.PropertyName == nameof(_equipmentRedistributionTableViewModel.TransferQuantity))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
