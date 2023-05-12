using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Structure;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class TransferStaticEquipmentCommand : CommandBase
    { 
        private readonly StaticEquipmentRedistributionTableViewModel _equipmentRedistributionTableViewModel;

        public TransferStaticEquipmentCommand(StaticEquipmentRedistributionTableViewModel equipmentRedistributionTableViewModel)
        {
            _equipmentRedistributionTableViewModel = equipmentRedistributionTableViewModel;
            _equipmentRedistributionTableViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }


        public override bool CanExecute(object? parameter)
        {
            return (_equipmentRedistributionTableViewModel.SelectedElementFrom is not null)
            && (_equipmentRedistributionTableViewModel.SelectedElementTo is not null)
            && (_equipmentRedistributionTableViewModel.SelectedElementFrom.RoomID != _equipmentRedistributionTableViewModel.SelectedElementTo.RoomID)
            && (_equipmentRedistributionTableViewModel.SelectedElementFrom.Type == _equipmentRedistributionTableViewModel.SelectedElementTo.Type)
            && (_equipmentRedistributionTableViewModel.SelectedElementFrom.Purpose == _equipmentRedistributionTableViewModel.SelectedElementTo.Purpose)
            && (_equipmentRedistributionTableViewModel.TransferQuantity != 0)
            && (_equipmentRedistributionTableViewModel.TransferQuantity <= _equipmentRedistributionTableViewModel.SelectedElementFrom.Quantity);
        }

        public override void Execute(object? parameter)
        {
            TransferStaticEquipmentAsync();
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_equipmentRedistributionTableViewModel.SelectedElementFrom) ||
                e.PropertyName == nameof(_equipmentRedistributionTableViewModel.SelectedElementTo) ||
                e.PropertyName == nameof(_equipmentRedistributionTableViewModel.TransferQuantity) ||
                e.PropertyName == nameof(_equipmentRedistributionTableViewModel.TransferTime))
            {
                OnCanExecutedChanged();
            }
        }
            async Task TransferStaticEquipmentAsync()
            {
                var selectedFrom = _equipmentRedistributionTableViewModel.SelectedElementFrom;
                var selectedTo = _equipmentRedistributionTableViewModel.SelectedElementTo;
                var transferQuantity = _equipmentRedistributionTableViewModel.TransferQuantity;
                var transferTime = _equipmentRedistributionTableViewModel.TransferTime;
                
                MessageBox.Show("Prebacivanje " + selectedFrom.RoomType + " " + selectedFrom.Purpose + " iz sobe " + selectedFrom.RoomID.ToString() +
                    " u sobu " + selectedTo.RoomID.ToString() + " bice uradjeno za "+transferTime.ToString()+" minuta.");
                _equipmentRedistributionTableViewModel.UpdateEquipmentRedistributions();    
                
                await Task.Delay(transferTime * 60000);

                DirectorDAO.TransferStaticEquipment(selectedFrom, selectedTo, transferQuantity);
                _equipmentRedistributionTableViewModel.UpdateEquipmentRedistributions();


        }
    }
}
