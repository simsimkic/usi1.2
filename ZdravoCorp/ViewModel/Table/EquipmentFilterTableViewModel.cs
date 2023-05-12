using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.ViewModel.Table
{
    public class EquipmentFilterTableViewModel : EquipmentTableViewModel
    {

        private bool _operationRoom;
        public bool OperationRoom
        {
            get { return _operationRoom; }
            set
            {
                if (_operationRoom != value)
                {
                    _operationRoom = value;
                    OnPropertyChanged(nameof(OperationRoom));
                    UpdateSelectedOptions();
                }
            }
        }

        private bool _patientRoom;
        public bool PatientRoom
        {
            get
            {
                return _patientRoom;
            }
            set
            {
                _patientRoom = value;
                OnPropertyChanged(nameof(PatientRoom));
            }
        }

        private bool _examinationRoom;
        public bool ExaminationRoom
        {
            get { return _examinationRoom; }
            set
            {
                if (_examinationRoom != value)
                {
                    _examinationRoom = value;
                    OnPropertyChanged(nameof(ExaminationRoom));
                    UpdateSelectedOptions();
                }
            }
        }

        private bool _waitingRoom;
        public bool WaitingRoom
        {
            get { return _waitingRoom; }
            set
            {
                if (_waitingRoom != value)
                {
                    _waitingRoom = value;
                    OnPropertyChanged(nameof(WaitingRoom));
                    UpdateSelectedOptions();
                }
            }
        }

        private bool _warehouse;
        public bool Warehouse
        {
            get { return _warehouse; }
            set
            {
                if (_warehouse != value)
                {
                    _warehouse = value;
                    OnPropertyChanged(nameof(Warehouse));
                    UpdateSelectedOptions();
                }
            }
        }

        private bool _roomFurinture;
        public bool RoomFurinture
        {
            get { return _roomFurinture; }
            set
            {
                if (_roomFurinture != value)
                {
                    _roomFurinture = value;
                    OnPropertyChanged(nameof(RoomFurinture));
                    UpdateSelectedOptions();
                }
            }
        }

        private bool _examination;
        public bool Examination
        {
            get { return _examination; }
            set
            {
                if (_examination != value)
                {
                    _examination = value;
                    OnPropertyChanged(nameof(Examination));
                    UpdateSelectedOptions();
                }
            }
        }

        private bool _operation;
        public bool Operation
        {
            get { return _operation; }
            set
            {
                if (_operation != value)
                {
                    _operation = value;
                    OnPropertyChanged(nameof(Operation));
                    UpdateSelectedOptions();
                }
            }
        }

        private bool _hallway;
        public bool Hallway
        {
            get { return _hallway; }
            set
            {
                if (_hallway != value)
                {
                    _hallway = value;
                    OnPropertyChanged(nameof(Hallway));
                    UpdateSelectedOptions();
                }
            }
        }

        private string _search;
        public string Search
        {
            get
            {
                return _search;
            }
            set
            {
                _search = value.ToLower();
                OnPropertyChanged(nameof(Search));
                UpdateSelectedOptions();

            }
        }

        public EquipmentFilterTableViewModel(Hospital hospital) : base(hospital)
        {

        }
        private void UpdateSelectedOptions()
        {
            var selectedOptionRoomType = new List<RoomType>();
            var selectedOptionEquipmentPurpose = new List<EquipmentPurpose>();

            if (OperationRoom) selectedOptionRoomType.Add(RoomType.OPERATION_ROOM);
            if (PatientRoom) selectedOptionRoomType.Add(RoomType.ROOM_FOR_THE_PATIENT);
            if (ExaminationRoom) selectedOptionRoomType.Add(RoomType.EXAMINATION_ROOM);
            if (WaitingRoom) selectedOptionRoomType.Add(RoomType.WAITING_ROOM);
            if (Warehouse) selectedOptionRoomType.Add(RoomType.WAREHOUSE);
            if (RoomFurinture) selectedOptionEquipmentPurpose.Add(EquipmentPurpose.ROOM_FURNITURE);
            if (Examination) selectedOptionEquipmentPurpose.Add(EquipmentPurpose.EXAMINATION);
            if (Operation) selectedOptionEquipmentPurpose.Add(EquipmentPurpose.OPERATION);
            if (Hallway) selectedOptionEquipmentPurpose.Add(EquipmentPurpose.HALLWAY);
            UpdateEquipment(selectedOptionRoomType, selectedOptionEquipmentPurpose);

        }
        private void UpdateEquipment(List<RoomType> roomTypes, List<EquipmentPurpose> equipmentPurposes)
        {

            List<Room> rooms = new List<Room>(Hospital.GetAllRooms());

            rooms.Add(Hospital.GetWarehouse());

            List<Room> roomResult;

            List<StaticEquipment> filteredEquipment;
            if (roomTypes.Count != 0 && equipmentPurposes.Count != 0)
            {
                roomResult = rooms.Where(s => roomTypes.Contains(s.RoomType)).ToList();
                var staticEquipment = GetEquipmentWithQuantity(roomResult);
                filteredEquipment = staticEquipment.Where(s => equipmentPurposes.Contains(s.Purpose)).ToList();

            }
            else if (roomTypes.Count != 0)
            {
                roomResult = rooms.Where(s => roomTypes.Contains(s.RoomType)).ToList();
                filteredEquipment = GetEquipmentWithQuantity(roomResult);

            }
            else if (equipmentPurposes.Count != 0)
            {
                var staticEquipment = GetEquipmentWithQuantity(rooms);
                filteredEquipment = staticEquipment.Where(s => equipmentPurposes.Contains(s.Purpose)).ToList();

            }
            else
            {
                filteredEquipment = GetEquipmentWithQuantity(rooms);

            }

            List<StaticEquipment> researchedEquipment = new List<StaticEquipment>();
            if (!string.IsNullOrEmpty(Search))
            {
                researchedEquipment = filteredEquipment.Where(obj => obj.Purpose.ToString().ToLower().Contains(Search) || obj.Quantity.ToString().ToLower().Contains(Search) || obj.Type.ToString().ToLower().Contains(Search)).ToList();

            }
            else
            {
                researchedEquipment = filteredEquipment;
            }
            AddElemntToEquipments(researchedEquipment);


        }

        private void AddElemntToEquipments(List<StaticEquipment> equipments)
        {
            Equipments.Clear();
            foreach (var o in equipments)
            {
                Equipments.Add(new StaticEquipmentViewModel(o));
            }
        }
    }



}
