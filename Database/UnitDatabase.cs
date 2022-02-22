using HRIS.Controllers;
using HRIS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HRIS.Database
{
    class UnitDatabase : SchoolDBAdapter
    {
        // Load all units but with only basic information that are needed for the unit list (code, title)
        public static List<Unit> LoadBasicUnits()
        {
            List<Unit> units = new List<Unit>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT code, title FROM unit", conn);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    units.Add(
                        new Unit(
                            code: dataReader.GetString(0),
                            title: dataReader.GetString(1)
                        )
                    );
                }
            }
            catch (MySqlException e)
            {
                ReportError("Loading Unit", e);
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

        // Load unit details ( specifically coordinator) for selected unit
        public static Unit LoadUnitDetails(Unit unit)
        {
            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;
            Unit detailedUnit = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT coordinator FROM unit WHERE code=?code", conn);
                cmd.Parameters.AddWithValue("code", unit.Code);
                dataReader = cmd.ExecuteReader();

                dataReader.Read();
                detailedUnit = new Unit(
                    code: unit.Code,
                    title: unit.Title,
                    coordinator: dataReader.GetInt32(0)
                );
            }
            catch (MySqlException e)
            {
                ReportError("Loading Unit", e);
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
            return detailedUnit;
        }

        // Load classes for the unit as well as the teaching staff name
        public static Unit LoadUnitClasses(Unit unit)
        {
            MySqlConnection conn = GetConnection();
            MySqlDataReader dataReader = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "SELECT c.campus, c.day, c.start, c.end, c.type, c.room, c.staff, s.given_name, s.family_name " +
                    "FROM class AS c " +
                    "INNER JOIN staff AS s ON c.staff = s.id " +
                    "WHERE unit_code=?unit_code"
                    , conn);
                cmd.Parameters.AddWithValue("unit_code", unit.Code);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    unit.Classes.Add(
                        new UnitClass(
                            unit_code: unit.Code,
                            campus: ParseEnum<Campus>(dataReader.GetString(0)),
                            day: ParseEnum<DayOfWeek>(dataReader.GetString(1)),
                            start: dataReader.GetTimeSpan(2),
                            end: dataReader.GetTimeSpan(3),
                            type: ParseEnum<ClassType>(dataReader.GetString(4)),
                            room: dataReader.GetString(5),
                            staff: dataReader.GetInt32(6),
                            gname: dataReader.GetString(7),
                            fname: dataReader.GetString(8)
                        )
                    );
                    unit.FilterClassesByCampus();
                }
            }
            catch (MySqlException e)
            {
                ReportError("Loading UnitClasses", e);
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
            return unit;
        }
    }
}

