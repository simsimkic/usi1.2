using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Model
{
    public class TimeSlot
    {
        private DateTime _from;
        private DateTime _to;

        public DateTime From { get => _from; set => _from = value; }
        public DateTime To { get => _to ; set => _to = value; }

        public TimeSlot()
        {
        }

        public TimeSlot(TimeSlot timeSlot)
        {
            _from = timeSlot.From;
            _to = timeSlot.To;
        }

        public TimeSlot(DateTime from, DateTime to)
        {
            _from = from;
            _to  = to;
        }

        public TimeSlot(DateTime to)
        {
            _from = DateTime.Now;
            _to = to;
        }

        // Maybe change to a static function instead of the constructor
        public TimeSlot(int hours)
        {
            _from = DateTime.Now;
            _to = DateTime.Now.AddHours(hours);
        }

        public void AddDays(int days)
        {
            _from.AddDays(days);
            _to.AddDays(days);
        }

        public bool IsBefore(TimeSlot timeSlot) 
        {
            return _to <= timeSlot.From;
        }

        public bool IsAfter(TimeSlot timeSlot) 
        {
            return _from >= timeSlot.To;
        }

        public bool OverlapsWith(TimeSlot timeSlot)
        {
            return (_to > timeSlot.From) && (_from < timeSlot.To);
        }

        // Maybe delete this function.
        public bool IsShorterThan(int duration) 
        {
            return (int)(_to - _from).TotalMinutes < duration;
        }

        public bool Contains(TimeSlot timeSlot)
        {
            return (_from <= timeSlot.From) && (_to >= timeSlot.To);
        }

        public DateOnly GetDate() 
        {
            return DateOnly.FromDateTime(_from);
        }

        public int GetDuration()
        {
            return (int)(_to - _from).TotalMinutes;
        }

        public int GetStartTimeDiff(TimeSlot timeSlot)
        {
            return (int) (_from - timeSlot.From).TotalMinutes;
        }

        public TimeSlot Split(TimeSlot timeSlot) 
        {
            var newTimeSlot = new TimeSlot(timeSlot.To, _to);
            _to = timeSlot.From;
            return newTimeSlot;
        }

        public OverlapType GetOverlapType(TimeSlot timeSlot) 
        {
            if (timeSlot.From < _from && timeSlot.To < _to) { return OverlapType.FRONT; }
            else if (timeSlot.To > _to && timeSlot.From > _from) { return OverlapType.BACK; }
            else if (timeSlot.From >= _from && timeSlot.To <= _to) { return OverlapType.MIDDLE; }

            return OverlapType.WHOLE;
        }

        public int GetOverlapDuration(TimeSlot timeSlot)
        {
            return GetOverlapType(timeSlot) switch
            {
                OverlapType.FRONT => (int)(timeSlot.To - _from).TotalMinutes,
                OverlapType.BACK => (int)(_to - timeSlot.From).TotalMinutes,
                OverlapType.MIDDLE => timeSlot.GetDuration(),
                _ => GetDuration(),
            };
        }

        public TimeSlot GetOverlap(TimeSlot timeSlot) 
        {
            return GetOverlapType(timeSlot) switch
            {
                OverlapType.FRONT => new TimeSlot(_from, timeSlot.To),
                OverlapType.BACK => new TimeSlot(timeSlot.From, _to),
                OverlapType.MIDDLE => new TimeSlot(timeSlot),
                _ => new TimeSlot(this),
            };
        }

        public void CutTo(int duration)
        {
            _to = _from.AddMinutes(duration);
        }
    }
}
