using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Structure;

namespace ZdravoCorp.ViewModel.Table
{
    public class DynamicalEquipmentTableViewModel : ViewModelBase
    {
        private readonly Hospital _hospital;
        public Hospital Hospital => _hospital;


        private ObservableCollection<DynamicalEquipmentViewModel> _eqipments;
        public ObservableCollection<DynamicalEquipmentViewModel> Equipments
        {
            get
            {
                return _eqipments;
            }
            set
            {
                _eqipments = value;
                OnPropertyChanged(nameof(Equipments));
            }
        }

        private DynamicalEquipmentViewModel _selectedEquipment;
        public DynamicalEquipmentViewModel SelectedEquipment
        {
            get
            {
                return _selectedEquipment;
            }
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged(nameof(SelectedEquipment));
            }
        }

        private int _requestedQuantity;
        public int RequestedQuantity
        {
            get
            {
                return _requestedQuantity;
            }
            set
            {
                _requestedQuantity = value;
                OnPropertyChanged(nameof(RequestedQuantity));
            }
        }
        public ICommand AddEquipmentRequestCommand { get; }
        public DynamicalEquipmentTableViewModel(Hospital hospital)
        {
            _hospital = hospital;
            _eqipments = new ObservableCollection<DynamicalEquipmentViewModel>(EquipmentService.GetDepletingDynamicalEquipment().Select(o => new DynamicalEquipmentViewModel(o)));
            AddEquipmentRequestCommand = new AddDynamicalEquipmentRequestCommand(this);
        }

    }


    
}
