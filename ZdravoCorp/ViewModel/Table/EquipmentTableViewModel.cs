using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Printing;
using System.Threading;

namespace ZdravoCorp.ViewModel.Table
{
    public class EquipmentTableViewModel : ViewModelBase
    {
        private readonly Hospital _hospital;
        public Hospital Hospital => _hospital;

        private ObservableCollection<StaticEquipmentViewModel> _eqipments;
        public ObservableCollection<StaticEquipmentViewModel> Equipments
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

        public EquipmentTableViewModel(Hospital hospital)
        {
            _eqipments = new ObservableCollection<StaticEquipmentViewModel>();
            _hospital = hospital;

            var staticEquipment = GetEquipmentWithQuantity(hospital.GetAllRooms());

            foreach (var o in staticEquipment)
            {
                _eqipments.Add(new StaticEquipmentViewModel(o));
            }


        }

        public List<StaticEquipment> GetEquipmentWithQuantity(IEnumerable<Room> rooms)
        {
            var equipments = rooms.SelectMany(s => s.StaticEquipmentBook).GroupBy(o => new { o.Type, o.Purpose })
                .Select(g => new { g.Key.Type, g.Key.Purpose, Quantity = g.Sum(o => o.Quantity) }).ToList();

            List<StaticEquipment> staticEquipments = new List<StaticEquipment>();
            foreach (var o in equipments)
            {
                var equipment = new StaticEquipment(o.Type, o.Purpose, o.Quantity);
                staticEquipments.Add(equipment);
            }
            return staticEquipments;

        }

    }
}