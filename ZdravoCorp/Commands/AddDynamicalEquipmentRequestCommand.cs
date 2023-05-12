using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Structure;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class AddDynamicalEquipmentRequestCommand : CommandBase
    {
        private readonly DynamicalEquipmentTableViewModel _dynamicalEquipmentTableFormViewModel;


        public AddDynamicalEquipmentRequestCommand(DynamicalEquipmentTableViewModel dynamicalEquipmentTableFormViewModel)
        {
            _dynamicalEquipmentTableFormViewModel = dynamicalEquipmentTableFormViewModel;
            _dynamicalEquipmentTableFormViewModel.PropertyChanged+=OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return (_dynamicalEquipmentTableFormViewModel.SelectedEquipment is not null)&&(_dynamicalEquipmentTableFormViewModel.RequestedQuantity != 0) ;
        }

        public override void Execute(object? parameter)
        {
            EquipmentService.MakeDynamicalEquipmentRequest(_dynamicalEquipmentTableFormViewModel.SelectedEquipment.Type,_dynamicalEquipmentTableFormViewModel.RequestedQuantity);
            MessageBox.Show("Uspesno ste napravili zahtez za dobavljanje "+ _dynamicalEquipmentTableFormViewModel.RequestedQuantity.ToString()+" "+_dynamicalEquipmentTableFormViewModel.SelectedEquipment.Type.ToString());
            _dynamicalEquipmentTableFormViewModel.RequestedQuantity = 0;
            _dynamicalEquipmentTableFormViewModel.SelectedEquipment = null;
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(_dynamicalEquipmentTableFormViewModel.SelectedEquipment)||
                e.PropertyName == nameof(_dynamicalEquipmentTableFormViewModel.RequestedQuantity))
            {
                OnCanExecutedChanged();
            }
        }
  
    }
}
