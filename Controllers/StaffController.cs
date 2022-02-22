using HRIS.Database;
using HRIS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HRIS.Controllers
{
    class StaffController
    {
        public List<Staff> Staff { get; set; }
        public ObservableCollection<Staff> ViewableStaff { get; set; }


        public StaffController()
        {
            Staff = StaffDatabase.LoadStaffBasic();

            ViewableStaff = new ObservableCollection<Staff>(Staff);
        }

        //Staff filter by name and category
        public void FilterStaff(Category category, string name)
        {
            var filteredStaff = from Staff s in Staff
                                where (s.GivenName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 || s.FamilyName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) && (category == Category.All || s.Category == category)
                                select s;
            ViewableStaff.Clear();
            filteredStaff.ToList().ForEach(ViewableStaff.Add);
        }

        //show staff list in StaffListBox
        public ObservableCollection<Staff> GetViewableStaffs()
        {
            return ViewableStaff;
        }

        //navigate from staff tab to unit tab
        public Staff GetStaffById(int id)
        {
            var staff = from Staff s in Staff
                        where s.Id == id
                        select s;
            return staff.First();
        }
    }
}
