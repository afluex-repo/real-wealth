using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealWealth.Models
{
    public class EmployeeRegistrations :Common
    {
        public List<EmployeeRegistrations> lstemp { get; set; }
        public string LoginId { get; set; }
        public string PK_AdminId { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }
        public string Password { get; set; }
        public string Name { get;  set; }
        public string Mobile { get;  set; }
        public string Email { get;  set; }
        public string Status { get;  set; }
        public string EducationQualififcation { get;  set; }
        public string CreatedBy { get;  set; }
        public object DOB { get;  set; }
        public string Fk_UserTypeId { get;  set; }
        public string Message { get;  set; }
        public string FathersName { get;  set; }
        public DataSet SaveEmpoyeeData()
        {
            SqlParameter[] para = {
            new SqlParameter("@Name", Name),
            new SqlParameter("@Mobile", Mobile),
            new SqlParameter("@Email", Email),
            new SqlParameter("@Address", Address),
            new SqlParameter("@DOB", DOB),
            new SqlParameter("@Qualification", EducationQualififcation),
            new SqlParameter("@FathName", FathersName),
            new SqlParameter("@Fk_UserTypeId", Fk_UserTypeId),
            new SqlParameter("@CreatedBy", CreatedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("EmployeeRegistration", para);
            return ds;
        }
        public DataSet GetEmployeeData()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@LoginId", LoginId)};
            DataSet ds = DBHelper.ExecuteQuery("GetEmployeeDetails", para);
            return ds;
        }
        public DataSet UpdateEmpoyeeData()
        {
            SqlParameter[] para = {
            new SqlParameter("@Name", Name),
            new SqlParameter("@Mobile", Mobile),
            new SqlParameter("@Email", Email),
            new SqlParameter("@Address", Address),
            new SqlParameter("@DOB", DOB),
            new SqlParameter("@Qualification", EducationQualififcation),
            new SqlParameter("@FathName", FathersName),
            new SqlParameter("@Fk_UserTypeId", Fk_UserTypeId),
            new SqlParameter("@UpdateBy", CreatedBy),
             new SqlParameter("@LoginId", LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateEmployee", para);
            return ds;
        }
        public DataSet UpdateEmpPassword()
        {
            SqlParameter[] para = {
            new SqlParameter("@LoginId", LoginId),
            new SqlParameter("@NewPassword", NewPassword),
            new SqlParameter("@UpdatedBy", CreatedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateEmployeePassword", para);
            return ds;
        }
        public DataSet ActivateEmployeeByAdmin()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@UpdatedBy", CreatedBy)};
            DataSet ds = DBHelper.ExecuteQuery("ActivateEmployeeByAdmin", para);
            return ds;
        }
        public DataSet DeActivateEmployeeByAdmin()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@UpdatedBy", CreatedBy)};
            DataSet ds = DBHelper.ExecuteQuery("DeActivateEmployeeByAdmin", para);
            return ds;
        }
    }
}