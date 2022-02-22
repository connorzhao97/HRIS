using System;
using System.Collections.Generic;
using System.Linq;

namespace HRIS.Models
{
    public enum Campus { All, Hobart, Launceston };
    public enum Category { All, Academic, Technical, Admin, Casual };
    class Staff
    {
        public int Id { get; }
        public string GivenName { get; }
        public string FamilyName { get; }
        public string Title { get; }
        public Campus Campus { get; }
        public string Phone { get; }
        public string Room { get; }
        public string Email { get; }
        public string Photo { get; }
        public Category Category { get; }

        public string Name
        {
            get { return Title + " " + GivenName + " " + FamilyName; }
        }
        public List<Consultation> ConsultationTimes { get; set; }
        public List<Unit> TeachingUnits { get; set; }
        public List<UnitClass> TeachingClasses { get; set; }

        //get availability of the staff
        public string Availability { get {
                string availability = "free";
                if (ConsultationTimes != null && TeachingClasses != null)
                {
                    //check consultations overlap or not
                    DateTime currentTime = DateTime.Now;
                    var overlapConsultation = from Consultation c in ConsultationTimes
                                              where c.Overlaps(currentTime)
                                              select c;
                    if (overlapConsultation.Count() > 0)
                    {
                        availability = "consulting";
                    }

                    //check teaching classes overlap or not
                    var overlapClasses = from UnitClass u in TeachingClasses
                                         where u.Overlaps(currentTime)
                                         select u;

                    if (overlapClasses.Count() > 0)
                    {
                        List<UnitClass> classes = new List<UnitClass>();
                        overlapClasses.ToList().ForEach(classes.Add);
                        availability = "teaching: ";
                        foreach (var c in classes)
                        {
                            availability += "(" + c.UnitCode + ", " + c.Room + ") ";
                        }
                    }
                }
                return availability;
            } }


        public Staff(int id, string givenName, string familyName, string title, Category category)
        {
            Id = id;
            GivenName = givenName;
            FamilyName = familyName;
            Title = title;
            Category = category;
        }

        public Staff(int id, string givenName, string familyName, string title, Campus campus, string phone, string room, string email, string photo, Category category)
        {
            Id = id;
            GivenName = givenName;
            FamilyName = familyName;
            Title = title;
            Campus = campus;
            Phone = phone;
            Room = room;
            Email = email;
            Photo = photo;
            Category = category;
        }

        public override string ToString()
        {
            return FamilyName + ", " + GivenName + " (" + Title + ") ";
        }
    }
}
