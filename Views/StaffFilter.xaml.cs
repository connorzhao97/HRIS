using HRIS.Controllers;
using HRIS.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HRIS.Views
{
    /// <summary>
    /// Interaction logic for StaffFilter.xaml
    /// </summary>
    public partial class StaffFilter : UserControl
    {
        private const string STAFF = "viewableStaffs";
        private StaffController staffController;
        public StaffFilter()
        {
            InitializeComponent();
            staffController = (StaffController)(Application.Current.FindResource(STAFF) as ObjectDataProvider).ObjectInstance;
        }

        //Refresh staff list after selecting categoty
        private void StaffCatetoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (staffController != null && e.AddedItems.Count > 0)
            {
                staffController.FilterStaff((Category)StaffCatetoryBox.SelectedItem, StaffFilterBox.Text.Trim());
            }
        }

        //Refresh staff list by text
        private void StaffFilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (staffController != null)
            {
                staffController.FilterStaff((Category)StaffCatetoryBox.SelectedItem, StaffFilterBox.Text.Trim());
            }
        }
    }
}
