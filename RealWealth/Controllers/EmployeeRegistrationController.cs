using RealWealth.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealWealth.Controllers
{
    public class EmployeeRegistrationController : AdminBaseController
    {
        // GET: EmployeeRegistration
        public ActionResult EmployeeRegistration(string LoginId)
        {
            EmployeeRegistrations emp = new EmployeeRegistrations();
            if (LoginId != null)
            {

                emp.LoginId = LoginId;

                DataSet ds = emp.GetEmployeeData();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    emp.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    emp.LoginId = ds.Tables[0].Rows[0].ToString();
                    emp.Mobile = ds.Tables[0].Rows[0]["Contact"].ToString();
                    emp.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    emp.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    emp.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    emp.EducationQualififcation = ds.Tables[0].Rows[0]["EducationQualifiacation"].ToString();

                }
            }



            #region ddlUserType
            Common obj = new Common();
            List<SelectListItem> ddlUserType = new List<SelectListItem>();
            DataSet ds11 = obj.BindUserTypeForRegistration();
            if (ds11 != null && ds11.Tables.Count > 0 && ds11.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds11.Tables[0].Rows)
                { ddlUserType.Add(new SelectListItem { Text = r["UserType"].ToString(), Value = r["PK_UserTypeId"].ToString() }); }
            }

            ViewBag.ddlUserType = ddlUserType;
            #endregion

            return View(emp);
        }
        public ActionResult EmployeeDetails()
        {

            List<EmployeeRegistrations> lst = new List<EmployeeRegistrations>();
            EmployeeRegistrations emp = new EmployeeRegistrations();
            DataSet ds = emp.GetEmployeeData();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmployeeRegistrations Objload = new EmployeeRegistrations();
                    Objload.Name = dr["Name"].ToString();
                    Objload.LoginId = dr["LoginId"].ToString();
                    Objload.Mobile = dr["Contact"].ToString();
                    Objload.Email = dr["Email"].ToString();
                    Objload.Password = dr["Password"].ToString();
                    Objload.Status = dr["Status"].ToString();
                    Objload.EducationQualififcation = dr["EducationQualifiacation"].ToString();

                    lst.Add(Objload);
                }
                emp.lstemp = lst;
            }
            return View(emp);
        }

        public ActionResult ActivateEmployee(string LoginId)
        {
            EmployeeRegistrations model = new EmployeeRegistrations();
            try
            {
                model.LoginId = LoginId;
                model.CreatedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = model.ActivateEmployeeByAdmin();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["EmployeeDetails"] = "Employee activated successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["EmployeeDetails"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["EmployeeDetails"] = ex.Message;
            }
            return RedirectToAction("EmployeeDetails", "EmployeeRegistration");
        }
        public ActionResult DeactivateEmployee(string LoginId)
        {
            EmployeeRegistrations model = new EmployeeRegistrations();
            try
            {
                model.LoginId = LoginId;
                model.CreatedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = model.DeActivateEmployeeByAdmin();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["EmployeeDetails"] = "Employee deactivated successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["EmployeeDetails"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["EmployeeDetails"] = ex.Message;
            }
            return RedirectToAction("EmployeeDetails", "EmployeeRegistration");
        }

        public ActionResult SaveEmployeeRegistration(string Name, string Mobile, string Email, string Qualification, string Fk_UserTypeId)
        {

            #region ddlUserType
            Common obj = new Common();
            List<SelectListItem> ddlUserType = new List<SelectListItem>();
            DataSet ds11 = obj.BindUserTypeForRegistration();
            if (ds11 != null && ds11.Tables.Count > 0 && ds11.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds11.Tables[0].Rows)
                { ddlUserType.Add(new SelectListItem { Text = r["UserType"].ToString(), Value = r["PK_UserTypeId"].ToString() }); }
            }

            ViewBag.ddlUserType = ddlUserType;
            #endregion
            EmployeeRegistrations objregi = new EmployeeRegistrations();
            try
            {

                objregi.Name = Name;
                objregi.Mobile = Mobile;
                objregi.Email = Email;

                //objregi.DOB = string.IsNullOrEmpty(objregi.DOB) ? null : Common.ConvertToSystemDate(objregi.DOB, "dd-MM-yyyy");
                objregi.EducationQualififcation = Qualification;

                objregi.Fk_UserTypeId = Fk_UserTypeId;
                objregi.CreatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = objregi.SaveEmpoyeeData();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        objregi.Message = "Employee Registration successfully";
                        obj.Result = "1";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "2")
                    {
                        objregi.Message = ds.Tables[0].Rows[0]["Result"].ToString();
                        obj.Result = "2";
                    }
                    else
                    {
                        objregi.Message = ds.Tables[0].Rows[0]["Result"].ToString();
                        obj.Result = "0";
                        return Json(objregi, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex) { }
            return Json(objregi, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateEmployeeRegistration(string Name, string Mobile, string Email, string Qualification, string Fk_UserTypeId, string LoginId)
        {

            #region ddlUserType
            Common obj = new Common();
            List<SelectListItem> ddlUserType = new List<SelectListItem>();
            DataSet ds11 = obj.BindUserTypeForRegistration();
            if (ds11 != null && ds11.Tables.Count > 0 && ds11.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds11.Tables[0].Rows)
                { ddlUserType.Add(new SelectListItem { Text = r["UserType"].ToString(), Value = r["PK_UserTypeId"].ToString() }); }
            }

            ViewBag.ddlUserType = ddlUserType;
            #endregion
            EmployeeRegistrations objregi = new EmployeeRegistrations();
            try
            {

                objregi.Name = Name;
                objregi.Mobile = Mobile;
                objregi.Email = Email;
                objregi.LoginId = LoginId;
                //objregi.DOB = string.IsNullOrEmpty(objregi.DOB) ? null : Common.ConvertToSystemDate(objregi.DOB, "dd-MM-yyyy");
                objregi.EducationQualififcation = Qualification;

                objregi.Fk_UserTypeId = Fk_UserTypeId;
                objregi.CreatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = objregi.UpdateEmpoyeeData();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        objregi.Message = "Employee Details Updated successfully";
                    }
                    else
                    {
                        objregi.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        return Json(objregi, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex) { }
            return Json(objregi, JsonRequestBehavior.AllowGet);
        }
    }
}