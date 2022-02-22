using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HRIS.Models
{
    class Unit
    {

        public Unit(string code, string title, int coordinator)
        {
            Code = code;
            Title = title;
            Coordinator = coordinator;
        }
        public Unit(string code, string title)
        {
            Code = code;
            Title = title;
        }

        public string Code { get; }
        public string Title { get; }
        public int Coordinator { get; }
        public List<UnitClass> Classes { get; } = new List<UnitClass>();
        public ObservableCollection<UnitClass> FilteredClasses { get; set; } = new ObservableCollection<UnitClass>();

        public override string ToString()
        {
            return Code + " " + Title;
        }

        // Filter entered campus for @Classes and save in @FilteredClasses
        public void FilterClassesByCampus(Campus campus = Campus.All)
        {
            var filteredClasses = from UnitClass cls in Classes
                                  where cls.Campus == campus || campus == Campus.All
                                  select cls;
            FilteredClasses.Clear();
            filteredClasses.ToList().ForEach(FilteredClasses.Add);
        }
    }
}
