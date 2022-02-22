using HRIS.Database;
using HRIS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HRIS.Controllers
{
    class UnitController
    {
        public List<Unit> Units { get; }
        public ObservableCollection<Unit> ViewableUnits { get; }

        public UnitController()
        {
            Units = UnitDatabase.LoadBasicUnits();
            ViewableUnits = new ObservableCollection<Unit>(Units);
        }

        public ObservableCollection<Unit> GetViewableUnits()
        {
            return ViewableUnits;
        }

        // Filter/Search unit by @keyword
        public void FilterUnit(string keyword)
        {
            var filteredUnits = from Unit u in Units
                                where
                                    u.Code.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                                    || u.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                                select u;
            ViewableUnits.Clear();
            filteredUnits.ToList().ForEach(ViewableUnits.Add);
        }

    }
}
