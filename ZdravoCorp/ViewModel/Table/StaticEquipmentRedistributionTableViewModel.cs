using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Structure;

namespace ZdravoCorp.ViewModel.Table
{
    public class StaticEquipmentRedistributionTableViewModel:ViewModelBase
    {
        private ObservableCollection<StaticEquipmentRedistributionViewModel> _equipmentRedistributions;
        public ObservableCollection<StaticEquipmentRedistributionViewModel> EquipmentRedistributions
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
        private StaticEquipmentRedistributionViewModel _selectedElementFrom;
        public StaticEquipmentRedistributionViewModel SelectedElementFrom
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
        private StaticEquipmentRedistributionViewModel _selectedElementTo;
        public StaticEquipmentRedistributionViewModel SelectedElementTo
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
        private int _transferTime;
        public int TransferTime
        {
            get
            {
                return _transferTime;
            }
            set
            {
                _transferTime = value;
                OnPropertyChanged(nameof(TransferTime));
            }
        }
        public ICommand TransferEquipmentCommand { get; }
        public StaticEquipmentRedistributionTableViewModel()
        {
            _equipmentRedistributions = new ObservableCollection<StaticEquipmentRedistributionViewModel>
                (EquipmentService.GetStaticEquipmentRedistributions());
            TransferEquipmentCommand = new TransferStaticEquipmentCommand(this);
        }
        public void UpdateEquipmentRedistributions()
        {
            this.EquipmentRedistributions = new ObservableCollection<StaticEquipmentRedistributionViewModel>(EquipmentService.GetStaticEquipmentRedistributions());
            this.SelectedElementTo = null;
            this.SelectedElementFrom = null;
            this.TransferQuantity = 0;
            this.TransferTime = 0;
            
        }
    }
}

