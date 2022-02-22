using System;

namespace HRIS.Models
{
    public enum ClassType { Lecture, Tutorial, Practical, Workshop }

    class UnitClass
    {
        public UnitClass(
            string unit_code,
            Campus campus,
            DayOfWeek day,
            TimeSpan start,
            TimeSpan end,
            ClassType type,
            string room,
            int staff,
            string gname,
            string fname
        )
        {
            UnitCode = unit_code;
            Campus = campus;
            Day = day;
            Start = start;
            End = end;
            Type = type;
            Room = room;
            Staff = staff;
            GivenName = gname;
            FamilyName = fname;
        }
        public UnitClass(string unit_code, DayOfWeek day, TimeSpan start, TimeSpan end, string room)
        {
            UnitCode = unit_code;
            Day = day;
            Start = start;
            End = end;
            Room = room;
        }
        public string UnitCode { get; }
        public Campus Campus { get; }
        public DayOfWeek Day { get; }
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public ClassType Type { get; }
        public string Room { get; }
        public int Staff { get; }
        public string GivenName { get; }
        public string FamilyName { get; }

        // Class duratioin String for display (Start-End)
        public string Duration { get => Start.ToString(@"hh\:mm") + "-" + End.ToString(@"hh\:mm"); }

        public string TypeString { get => Type.ToString(); }
        
        public string StaffName
        {
            get
            {
                return GivenName + " " + FamilyName;
            }
        }

        public bool Overlaps(DateTime dateTime)
        {
            return dateTime.DayOfWeek == Day && dateTime.TimeOfDay >= Start && dateTime.TimeOfDay < End;
        }
    }
}
