﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Gwenchana.DataAccess.DTO;

namespace Gwenchana.DataAccess.DAL
{
    public class EmployeeDAL
    {
        private readonly DBConnect.DbConnect _db = new DBConnect.DbConnect();
        public List<DTO.Employee> GetAllEmployees()
        {
            var list = new List<DTO.Employee>();
            string sql = "select * from Employee";
            DataTable dt = _db.GetData(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DTO.Employee
                {
                    Empolyee_Id = Convert.ToInt32(row["Employee_Id"]),
                    employeeName = row["employeeName"].ToString(),
                    Age = Convert.ToInt32(row["age"]),
                    phoneNumber = row["phoneNumber"].ToString(),
                    Account_Id = Convert.ToInt32(row["Account_Id"]),
                    TrangThai = row["EmploymentStatus"] != DBNull.Value ? row["EmploymentStatus"].ToString() : null // Handle null value for TrangThai
                });
            }
            return list;
        }
        public DataTable GetAllEmployeesDataTable()
        {
            string sql = "select * from Employee";
            return _db.GetData(sql);
        }
        public bool UpdateEmployee(DTO.Employee employee)
        {
            try
            {
                string sql = "UPDATE Employee SET employeeName = @employeeName, age = @age, phoneNumber = @phoneNumber, EmploymentStatus = @employmentStatus WHERE Employee_Id = @Employee_Id";
                var parameters = new[]
                {
                    new SqlParameter("@employeeName", employee.employeeName),
                    new SqlParameter("@age", employee.Age),
                    new SqlParameter("@phoneNumber", employee.phoneNumber),
                    new SqlParameter("@Employee_Id", employee.Empolyee_Id),
                    new SqlParameter("@employmentStatus", employee.TrangThai ?? (object)DBNull.Value) // Handle null TrangThai
                };
                return _db.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteEmployee(int id)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int? accountId = null;
                        using (var cmd = new SqlCommand("SELECT Account_Id FROM Employee WHERE Employee_Id = @Employee_Id", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Employee_Id", id);
                            var result = cmd.ExecuteScalar();
                            if (result != DBNull.Value && result != null)
                                accountId = Convert.ToInt32(result);
                        }
                        using (var cmd = new SqlCommand("DELETE FROM Employee WHERE Employee_Id = @Employee_Id", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Employee_Id", id);
                            cmd.ExecuteNonQuery();
                        }
                        if (accountId.HasValue)
                        {
                            using (var cmd = new SqlCommand("DELETE FROM Account WHERE id = @AccountId", connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@AccountId", accountId.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public Employee GetEmployeeById(int id) 
        {
            string sql = "SELECT * FROM Employee WHERE Account_Id = @Employee_Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Employee_Id", id)
            };
            DataTable dt = _db.GetData(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Employee
                {
                    Empolyee_Id = Convert.ToInt32(row["Employee_Id"]),
                    employeeName = row["employeeName"].ToString(),
                    Age = Convert.ToInt32(row["age"]),
                    phoneNumber = row["phoneNumber"].ToString(),
                    Account_Id = Convert.ToInt32(row["Account_Id"]),
                };
            }
            return null;
        }
    }
}
