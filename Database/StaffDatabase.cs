using HRIS.Controllers;
using HRIS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HRIS.Database
{
    class StaffDatabase : SchoolDBAdapter
    {

        //Load staff basic information
        public static List<Staff> LoadStaffBasic()
        {
            List<Staff> staff = new List<Staff>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT id, given_name, family_name, title, category FROM staff ORDER BY family_name, given_name", conn);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    staff.Add(
                        new Staff(
                        id: dataReader.GetInt32(0),
                        givenName: dataReader.GetString(1),
                        familyName: dataReader.GetString(2),
                        title: dataReader.GetString(3),
                        category: ParseEnum<Category>(dataReader.GetString(4))
                        ));
                }
            }
            catch (MySqlException e)
            {
                ReportError("Loading Staff", e);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return staff;
        }

        //Load staff detail information
        public static Staff LoadStaffDetail(Staff staff)
        {
            Staff staffDetail = null;

            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT campus, phone, room, email, photo FROM staff WHERE id=?id ", conn);
                cmd.Parameters.AddWithValue("id", staff.Id);
                dataReader = cmd.ExecuteReader();
                dataReader.Read();

                staffDetail = new Staff(
                        id: staff.Id,
                        givenName: staff.Name.Split(' ')[1],
                        familyName: staff.Name.Split(' ')[2],
                        title: staff.Name.Split(' ')[0],
                        campus: ParseEnum<Campus>(dataReader.GetString(0)),
                        phone: dataReader.GetString(1),
                        room: dataReader.GetString(2),
                        email: dataReader.GetString(3),
                        photo: dataReader.GetString(4),
                        category: staff.Category
                    );

            }
            catch (MySqlException e)
            {
                ReportError("Loading StaffDetails", e);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            staffDetail.ConsultationTimes = LoadConsultations(staff.Id);
            staffDetail.TeachingUnits = LoadTeachingUnits(staff.Id);
            staffDetail.TeachingClasses = LoadTeachingClasses(staff.Id);
            return staffDetail;
        }

        //load consultation times
        public static List<Consultation> LoadConsultations(int id)
        {
            List<Consultation> consultations = new List<Consultation>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT day, start, end FROM consultation WHERE staff_id=?id", conn);
                cmd.Parameters.AddWithValue("id", id);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    consultations.Add(new Consultation(
                        day: ParseEnum<DayOfWeek>(dataReader.GetString(0)),
                        start: dataReader.GetTimeSpan(1),
                        end: dataReader.GetTimeSpan(2))
                    );
                }
            }
            catch (MySqlException e)
            {
                ReportError("Loading Consultations", e);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return consultations;
        }

        //Load units teaching this semester
        public static List<Unit> LoadTeachingUnits(int id)
        {
            List<Unit> units = new List<Unit>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT unit.code, unit.title FROM unit WHERE unit.coordinator=?id UNION SELECT unit.code, unit.title FROM unit JOIN class on unit.code = class.unit_code where class.staff=?id GROUP BY code ORDER BY code", conn);
                cmd.Parameters.AddWithValue("id", id);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    units.Add(new Unit(
                        code: dataReader.GetString(0),
                        title: dataReader.GetString(1)
                        ));
                }
            }
            catch (MySqlException e)
            {
                ReportError("Loading Teaching Units", e);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return units;
        }

        //Load teaching classes of teaching this semester
        public static List<UnitClass> LoadTeachingClasses(int id)
        {
            List<UnitClass> unitClasses = new List<UnitClass>();
            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT unit_code, day, start, end, room FROM class where staff=?id order by unit_code", conn);
                cmd.Parameters.AddWithValue("id", id);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    unitClasses.Add(new UnitClass(
                            unit_code: dataReader.GetString(0),
                            day: ParseEnum<DayOfWeek>(dataReader.GetString(1)),
                            start: dataReader.GetTimeSpan(2),
                            end: dataReader.GetTimeSpan(3),
                            room: dataReader.GetString(4)
                        ));
                }
            }
            catch (MySqlException e)
            {
                ReportError("Loading Teaching Classes", e);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return unitClasses;
        }
    }
}
