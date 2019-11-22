﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Assessment2
{
    class Sql
    {
        const string CONNECTION_STRING = "server=localhost;user=nmt_demo_user;database=nmt_demo;port=3306;password=Password1";

        private static MySqlConnection connection;

        private static void GetConnection()
        {
            connection = new MySqlConnection(CONNECTION_STRING);
            connection.Open();
        }

        private void CloseConnection()
        {
            connection.Close();
        }

        private static DataTable selectTable(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                GetConnection();

                using (MySqlDataAdapter da = new MySqlDataAdapter(sql, connection))
                {
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {

            }

            return dt;
        }

        private static bool sqlExecute(string sql)
        {
            try
            {
                using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                {
                    cmdSel.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }

            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }

            return obj;
        }

        public static T sqlSelect<T>(ulong id)
        {
            string sql = "Select * from " + typeof(T).Name;
            sql += " Where id = " + id.ToString();

            return GetItem<T>(selectTable(sql).Rows[0]);
        }

        public static List<T> sqlSelectAll<T>(string filter = "")
        {
            string sql = "Select * from " + typeof(T).Name;

            return ConvertDataTable<T>(selectTable(sql));
        }

        public static bool sqlInsert<T>(T classObj)
        {
            List<string> auxList = new List<string>();
            List<string> auxList2 = new List<string>();

            string sql = "Insert into " + typeof(T).Name + " (";

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributesData().Count == 0)
                {
                    auxList.Add(item.Name);

                    switch (item.PropertyType.Name)
                    {
                        case "String":
                            auxList2.Add(string.Format("'{0}'", item.GetValue(classObj)));
                            break;
                        case "DateTime":
                            auxList2.Add(string.Format("'{0:s}'", item.GetValue(classObj)));
                            break;
                        default:
                            auxList2.Add(item.GetValue(classObj).ToString());
                            break;
                    }
                }
            }

            sql += string.Join<string>(" ,", auxList) + ") VALUES (";
            sql += string.Join<string>(" ,", auxList2) + ") ";

            return sqlExecute(sql);
        }

        public static bool sqlUpdate<T>(T classObj)
        {
            List<string> auxList = new List<string>();

            string sql = "Update " + typeof(T).Name + " Set ";
            string sWHereClause = string.Empty;

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributesData().Count == 0)
                {
                    if (item.Name == "Id")
                    {
                        sWHereClause = " Where id = " + item.GetValue(classObj).ToString();
                    }
                    else
                    {
                        if (item.GetValue(classObj) != null && item.GetValue(classObj).ToString() != "0")
                        {
                            switch (item.PropertyType.Name)
                            {
                                case "String":
                                    auxList.Add(string.Format("{0} = '{1}'", item.Name, item.GetValue(classObj)));
                                    break;
                                case "DateTime":
                                    auxList.Add(string.Format("{0} = '{1:s}'", item.Name, item.GetValue(classObj)));
                                    break;
                                default:
                                    auxList.Add(string.Format("{0} = {1}", item.Name, item.GetValue(classObj)));
                                    break;
                            }
                        }
                    }
                }
            }

            sql += string.Join<string>(" ,", auxList) + sWHereClause;

            return sqlExecute(sql);
        }

        public static bool sqlDelete<T>(T classObj)
        {
            List<string> auxList = new List<string>();

            string sql = "Delete ";
            sql += "From " + typeof(T).Name;

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributesData().Count == 0)
                {
                    if (item.Name == "Id")
                    {
                        sql += " Where id = " + item.GetValue(classObj).ToString();
                        break;
                    }
                }
            }

            return sqlExecute(sql);
        }
    }
}
