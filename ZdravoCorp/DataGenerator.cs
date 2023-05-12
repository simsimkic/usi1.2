using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Storage;
using ZdravoCorp.View;
using ZdravoCorp.Serializer;

namespace ZdravoCorp
{
    public class DataGenerator
    {
        private const string PatientFileName = @"..\..\..\Data\patient.json";
        private const string DoctorFileName = @"..\..\..\Data\doctor.json";
        private const string AppointmentFileName = @"..\..\..\Data\appointment.json";
        private const string DirectorFileName = @"..\..\..\Data\director.json";
        private const string NurseFileName = @"..\..\..\Data\nurse.json";

        private static List<string> _firstNames = new List<string> { "Ana", "Jovan", "Marko", "Mihajlo", "Milica", "Nikola", "Petar", "Sofija", "Stefan", "Tamara" };
        private static List<string> _lastNames = new List<string> { "Arsić", "Đorđević", "Ilić", "Janković", "Jovanović", "Kovačević", "Marković", "Petrović", "Stojanović", "Vuković" };
        private static Random Random = new Random();

        public Storage<Patient> Patients { get; set; }
        public Storage<Doctor> Doctors { get; set; }
        public Storage<Nurse> Nurses { get; set; }
        public Storage<Director> Director { get; set; }

        public DataGenerator() { }
        public DataGenerator(int amount) 
        {
            Patients = new Storage<Patient>(PatientFileName);
            Doctors = new Storage<Doctor>(DoctorFileName);
            Nurses = new Storage<Nurse>(NurseFileName);
            Director = new Storage<Director>(DirectorFileName);

            Patients.Save(Generate(id => GenerateRandomPatient(id), amount));
            Doctors.Save(Generate(id => GenerateRandomDoctor(id), amount));
            Nurses.Save(Generate(id => GenerateRandomNurse(id), amount));
            //Director.Save(Generate(() => GenerateRandomDirctor(), 1));


        }

        private Dictionary<int, T> Generate<T>(Func<int, T> generator, int length) where T : ISerializable
        {
            var data = new Dictionary<int, T>();
            int id = 1;
            while(data.Values.Count < length)
            {
                var obj = generator(id);
                data.Add(id, obj);
                id++;
            }
            return data;
        }

        private Patient GenerateRandomPatient(int id)
        {
            return new Patient(id, GenerateRandomString(7), _firstNames[Random.Next(_firstNames.Count)], _lastNames[Random.Next(_lastNames.Count)], GenerateRandomString(7), GenerateRandomMedicalRecord());
        }

        private Doctor GenerateRandomDoctor(int id)
        {
            return new Doctor(id, GenerateRandomString(7), _firstNames[Random.Next(_firstNames.Count)], _lastNames[Random.Next(_lastNames.Count)], GenerateRandomString(7), GenerateRandomEnum<Specialization>());
        }

        private Nurse GenerateRandomNurse(int id)
        {
            return new Nurse(id, GenerateRandomString(7), _firstNames[Random.Next(_firstNames.Count)], _lastNames[Random.Next(_lastNames.Count)], GenerateRandomString(7));
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
        private MedicalRecord GenerateRandomMedicalRecord()
        {   
            return new MedicalRecord(Random.Next(150, 200), Random.Next(50, 150), GenerateRandomEnumsList<Disease>(), GenerateRandomEnumsList<Alergy>());
        }

        private List<T> GenerateRandomEnumsList<T>() where T : Enum
        {
            var enums = new List<T>();
            var length = Random.Next(1, 11);

            while (enums.Count < length)
            {
                var value = (T)Enum.ToObject(typeof(T), Random.Next(Enum.GetValues(typeof(T)).Length));
                if (!enums.Contains(value))
                {
                    enums.Add(value);
                }
            }
            return enums;
        }

        private T GenerateRandomEnum<T>() where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), Random.Next(Enum.GetValues(typeof(T)).Length));
        }
        public static List<T> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        private List<StaticEquipment> GenerateAllEquipment()
        {
            List<StaticEquipment> allEquipment = new List<StaticEquipment>();
            Random random = new Random();

            foreach (StaticEquipmentType type in Enum.GetValues(typeof(StaticEquipmentType)))
            {
                int purposeCount = Enum.GetNames(typeof(EquipmentPurpose)).Length;
                EquipmentPurpose purpose = (EquipmentPurpose)random.Next(purposeCount);
                int quantity = new Random().Next(1, 11);

                    StaticEquipment equipment = new StaticEquipment(type, purpose, quantity);

                    allEquipment.Add(equipment);
                
            }

            return allEquipment;
        }
        private List<DynamicalEquipment> GenerateEquipment()
        {
            List<DynamicalEquipment> allEquipment = new List<DynamicalEquipment>();
            Random random = new Random();

            foreach (DynamicalEquipmentType type in Enum.GetValues(typeof(DynamicalEquipmentType)))
            {
                int quantity = new Random().Next(1, 11);

                DynamicalEquipment equipment = new DynamicalEquipment(type, quantity);

                allEquipment.Add(equipment);

            }
 
            return allEquipment;
        }

        private RoomBook GerenareRooms()
        {
            RoomBook roomBook = new RoomBook();
            roomBook.Add( new Room(001, RoomType.OPERATION_ROOM, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(002, RoomType.OPERATION_ROOM, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(003, RoomType.EXAMINATION_ROOM, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(004, RoomType.EXAMINATION_ROOM, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(005, RoomType.EXAMINATION_ROOM, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(006, RoomType.EXAMINATION_ROOM, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(007, RoomType.ROOM_FOR_THE_PATIENT, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(008, RoomType.ROOM_FOR_THE_PATIENT, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(009, RoomType.ROOM_FOR_THE_PATIENT, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(010, RoomType.ROOM_FOR_THE_PATIENT, GenerateAllEquipment(), GenerateEquipment()));
            roomBook.Add( new Room(011, RoomType.WAITING_ROOM, GenerateAllEquipment(), GenerateEquipment()));
            return roomBook;
        }
        private Director GenerateRandomDirctor()
        {
            return new Director(new Hospital(new Room(000,RoomType.WAREHOUSE,GenerateAllEquipment(),GenerateEquipment()), GerenareRooms() ),12,"nikola123","Nikola","Trajkovic","123");
        }


        public void GenerateDoctorSchedule(int days)
        {
            foreach (int id in DAOFactory.GetInstance().DoctorDAO.GetAll().Keys)
            {
                var doctorSchedule = new DoctorSchedule(id, GenerateFreeTimeSlots(days), GenerateAppointments(days), 0);
                DAOFactory.GetInstance().DoctorScheduleDAO.Add(doctorSchedule);

            }
        }

        private Dictionary<DateOnly, List<TimeSlot>> GenerateFreeTimeSlots(int days)
        {
            var today = DateTime.Today;
            var freeTimeSlots = new Dictionary<DateOnly, List<TimeSlot>>();
            for (int i = 0; i < days; i++)
            {
                var day = today.AddDays(i);
                var freeTimeSlotsList = new List<TimeSlot>();
                freeTimeSlotsList.Add(new TimeSlot(day.AddHours(8), day.AddHours(20)));

                freeTimeSlots.Add(DateOnly.FromDateTime(day), freeTimeSlotsList);
            }
            return freeTimeSlots;
        }

        private Dictionary<DateOnly, List<Appointment>> GenerateAppointments(int days)
        {
            var today = DateTime.Today;
            var freeTimeSlots = new Dictionary<DateOnly, List<Appointment>>();
            for (int i = 0; i < days; i++)
            {
                var day = today.AddDays(i);
                var freeTimeSlotsList = new List<Appointment>();

                freeTimeSlots.Add(DateOnly.FromDateTime(day), freeTimeSlotsList);
            }
            return freeTimeSlots;
        }

    }
}
