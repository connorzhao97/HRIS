using HRIS.Controllers;
using HRIS.Database;
using HRIS.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HRIS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string STAFF = "viewableStaffs";
        private const string UNIT = "viewableUnits";
        private StaffController staffController;
        private UnitController unitController;
        private Unit selectedUnit = null;
        private Staff selectedStaff = null;
        private bool ItemSelected { get => selectedUnit != null || selectedStaff != null; }

        public MainWindow()
        {
            InitializeComponent();

            staffController = (StaffController)(Application.Current.FindResource(STAFF) as ObjectDataProvider).ObjectInstance;
            unitController = (UnitController)(Application.Current.FindResource(UNIT) as ObjectDataProvider).ObjectInstance;
        }



        /*************************************
        **************** Staff ***************
        *************************************/


        //Select a particular staff
        private void StaffListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                staffDetailsPanel.DataContext = StaffDatabase.LoadStaffDetail((Staff)e.AddedItems[0]);
            }
        }

        private void TeachingUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemSelected || TeachingUnits.SelectedItem == null)
            {
                return;
            }
            selectedStaff = (Staff)staffDetailsPanel.DataContext;
            UnitBackBtn.Visibility = Visibility.Visible;
            this.tabControl.SelectedIndex = 1;

            Unit selectedTeachingUnit = (Unit)TeachingUnits.SelectedItem;
            Unit detailedUnit = UnitDatabase.LoadUnitDetails(selectedTeachingUnit);
            detailedUnit = UnitDatabase.LoadUnitClasses(detailedUnit);
            unitDetailsPanel.DataContext = detailedUnit;
            TeachingUnits.SelectedIndex = -1;
        }



        /*************************************
        **************** Unit ****************
        *************************************/

        // Update unit list when search text changed
        private void UnitSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            unitController.FilterUnit(UnitSearchBox.Text.Trim());
        }

        // When a particular unit is selected
        private void UnitListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Unit unit = UnitDatabase.LoadUnitDetails((Unit)e.AddedItems[0]);
                unit = UnitDatabase.LoadUnitClasses(unit);
                Campus selectedCampus = (Campus)UnitCampusBox.SelectedIndex;
                unit.FilterClassesByCampus(selectedCampus);
                unitDetailsPanel.DataContext = unit;
            }
        }

        // Refresh unit class list after selecting campus
        private void UnitCampusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (unitDetailsPanel != null && unitDetailsPanel.DataContext != null)
            {
                ((Unit)unitDetailsPanel.DataContext).FilterClassesByCampus((Campus)e.AddedItems[0]);
            }
        }

        // Jump to the selected staff when user clicks on it
        // No multi level jump (no jump if it is currently a selected teaching unit from staff details
        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ItemSelected)
            {
                return;
            }
            selectedUnit = (Unit)unitDetailsPanel.DataContext;
            StaffBackBtn.Visibility = Visibility.Visible;
            this.tabControl.SelectedIndex = 0;

            int staffId = ((UnitClass)UnitClassesGrid.SelectedItem).Staff;
            Staff basicStaff = staffController.GetStaffById(staffId);
            staffDetailsPanel.DataContext = StaffDatabase.LoadStaffDetail(basicStaff);
        }

        // Go back to the previous page (after jumping
        // switch back to the tab, load saved details, and change the visibility of Back button
        private void GoBack(object sender, RoutedEventArgs e)
        {
            // Go back to staff details panel
            if (selectedStaff != null)
            {
                this.tabControl.SelectedIndex = 0;
                staffDetailsPanel.DataContext = selectedStaff;
                selectedStaff = null;
                UnitBackBtn.Visibility = Visibility.Hidden;
            }

            // Go back to unit details panel
            else if (selectedUnit != null)
            {
                this.tabControl.SelectedIndex = 1;
                unitDetailsPanel.DataContext = selectedUnit;
                selectedUnit = null;
                StaffBackBtn.Visibility = Visibility.Hidden;
            }
        }

    }
}
