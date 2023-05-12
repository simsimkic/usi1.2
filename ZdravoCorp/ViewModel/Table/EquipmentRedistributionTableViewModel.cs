using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Structure;
using ZdravoCorp.Commands;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.ViewModel.Table
{
    public class EquipmentRedistributionTableViewModel : ViewModelBase
    {
        private ObservableCollection<EquipmentRedistributionViewModel> _equipmentRedistributions;
        public ObservableCollection<EquipmentRedistributionViewModel> EquipmentRedistributions
        {
            get
            {
                return _equipmentRedistributions;
            }
            set
            {
                _equipmentRedistributions = value;
                OnPropertyChanged(nameof(EquipmentRedistributions));
            }
        }
        private EquipmentRedistributionViewModel _selectedElementFrom;
        public EquipmentRedistributionViewModel SelectedElementFrom
        {
            get
            {
                return _selectedElementFrom;
            }
            set
            {
                _selectedElementFrom = value;
                OnPropertyChanged(nameof(SelectedElementFrom));
            }
        }
        private EquipmentRedistributionViewModel _selectedElementTo;
        public EquipmentRedistributionViewModel SelectedElementTo
        {
            get
            {
                return _selectedElementTo;
            }
            set
            {
                _selectedElementTo = value;
                OnPropertyChanged(nameof(SelectedElementTo));
            }
        }

        private int _transferQuantity;
        public int TransferQuantity
        {
            get
            {
                return _transferQuantity;
            }
            set
            {
                _transferQuantity = value;
                OnPropertyChanged(nameof(TransferQuantity));
            }
        }
        public ICommand TransferEquipmentCommand { get; }
        public EquipmentRedistributionTableViewModel()
        {
            _equipmentRedistributions = new ObservableCollection<EquipmentRedistributionViewModel>(EquipmentService.GetEquipmentRedistributions());
            TransferEquipmentCommand = new TransferDynamicalEquipmentCommand(this);
        }

    }

}
