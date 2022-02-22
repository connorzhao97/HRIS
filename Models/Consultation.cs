using System;

namespace HRIS.Models
{
    class Consultation
    {

        public DayOfWeek Day { get; }
        public TimeSpan Start { get; }
        public TimeSpan End { get; }

        public Consultation(DayOfWeek day, TimeSpan start, TimeSpan end)
        {
            Day = day;
            Start = start;
            End = end;
        }


        //Check consulation time overlaps
        public bool Overlaps(DateTime dateTime)
        {
            return dateTime.DayOfWeek == Day && dateTime.TimeOfDay >= Start && dateTime.TimeOfDay < End;
        }


        public override string ToString()
        {
            return string.Format("{0} {1:hh':'mm}--{2:hh':'mm}", Day, Start, End);
        }
    }
}
