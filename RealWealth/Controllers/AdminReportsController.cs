using RealWealth.Filter;
using RealWealth.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealWealth.Controllers
{
    public class AdminReportsController : AdminBaseController
    {
        // GET: AdminReports
        #region AssociateList
        #endregion
        public ActionResult AssociateList(AdminReports model, string Status)
        {
            #region ddlstatus
            List<SelectListItem> ddlstatus = Common.AssociateStatus();
            ViewBag.ddlstatus = ddlstatus;
            #endregion
            List<SelectListItem> Leg = Common.LegType();
            ViewBag.ddlleg = Leg;
            if (Status != "" && Status != null)
            {
                model.Status = Status;
            }
            List<AdminReports> lst = new List<AdminReports>();

            DataSet ds = model.GetAssociateList();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.Fk_UserId = r["Pk_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.Password = Crypto.Decrypt(r["Password"].ToString());
                    obj.Mobile = (r["Mobile"].ToString());
                    obj.Email = (r["Email"].ToString());
                    obj.SponsorId = (r["SponsorId"].ToString());
                    obj.SponsorName = (r["SponsorName"].ToString());
                    obj.isBlocked = (r["isBlocked"].ToString());
                    obj.Status = r["MemberStatus"].ToString();
                    obj.MemberStatus = r["MemberStatus"].ToString();
                    obj.ActivationMode = r["ActivationMode"].ToString();
                    lst.Add(obj);
                }
                model.lstassociate = lst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("AssociateList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult AssociateListBy(AdminReports model)
        {
            if (model.LoginId == null)
            {
                model.ToLoginID = null;
            }
            List<AdminReports> lst = new List<AdminReports>();
            List<SelectListItem> Leg = Common.LegType();
            ViewBag.ddlleg = Leg;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            // model.LoginId = model.ToLoginID;
            model.MemberStatus = model.MemberStatus == "0" ? null : model.MemberStatus;
            DataSet ds = model.GetAssociateList();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.Fk_UserId = r["Pk_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.Password = Crypto.Decrypt(r["Password"].ToString());
                    obj.Mobile = (r["Mobile"].ToString());
                    obj.Email = (r["Email"].ToString());
                    obj.SponsorId = (r["SponsorId"].ToString());
                    obj.SponsorName = (r["SponsorName"].ToString());
                    obj.isBlocked = (r["isBlocked"].ToString());
                    obj.Status = r["MemberStatus"].ToString();
                    obj.MemberStatus = r["MemberStatus"].ToString();
                    obj.ActivationMode = r["ActivationMode"].ToString();
                    lst.Add(obj);
                }
                model.lstassociate = lst;
            }
            #region ddlstatus
            List<SelectListItem> ddlstatus = Common.AssociateStatus();
            ViewBag.ddlstatus = ddlstatus;
            #endregion
            return View(model);
        }
        public ActionResult BlockAssociate(Profile obj, string LoginID)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                obj.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.BlockAssociate();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BlockUnblock"] = "User Blocked";
                        FormName = "AssociateList";
                        Controller = "AdminReports";
                    }
                    else
                    {
                        TempData["BlockUnblock"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "AssociateList";
                        Controller = "AdminReports";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["BlockUnblock"] = ex.Message;
                FormName = "AssociateList";
                Controller = "AdminReports";
            }
            return RedirectToAction(FormName, Controller);
        }
        public ActionResult UnblockAssociate(Profile obj, string LoginID)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                obj.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.UnblockAssociate();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BlockUnblock"] = "User Blocked";
                        FormName = "AssociateList";
                        Controller = "AdminReports";
                    }
                    else
                    {
                        TempData["BlockUnblock"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "AssociateList";
                        Controller = "AdminReports";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["BlockUnblock"] = ex.Message;
                FormName = "AssociateList";
                Controller = "AdminReports";
            }
            return RedirectToAction(FormName, Controller);
        }
        public ActionResult ActivateUser(string FK_UserID)
        {
            Profile model = new Profile();
            try
            {
                model.Fk_UserId = FK_UserID;
                model.ProductID = "1";
                model.UpdatedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = model.ActivateUserByAdmin();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BlockUnblock"] = "User activated successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["BlockUnblock"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["BlockUnblock"] = ex.Message;
            }
            return RedirectToAction("AssociateList", "AdminReports");
        }
        public ActionResult DeactivateUser(string lid)
        {
            Profile model = new Profile();
            try
            {
                model.LoginId = lid;
                model.UpdatedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = model.DeactivateUserByAdmin();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BlockUnblock"] = "User deactivated successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["BlockUnblock"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["BlockUnblock"] = ex.Message;
            }
            return RedirectToAction("AssociateList", "AdminReports");
        }
        #region topupreport
        public ActionResult TopupReport()
        {
            AdminReports newdata = new AdminReports();
            List<AdminReports> lst1 = new List<AdminReports>();
            DataSet ds11 = newdata.GetTopupReport();

            if (ds11 != null && ds11.Tables.Count > 0 && ds11.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds11.Tables[0].Rows)
                {
                    AdminReports Obj = new AdminReports();
                    //Obj.ToLoginID = r["Pk_InvestmentId"].ToString();
                    Obj.Fk_UserId = r["PK_UserId"].ToString();
                    Obj.Pk_investmentId = r["Pk_InvestmentId"].ToString();
                    Obj.LoginId = r["LoginId"].ToString();
                    Obj.DisplayName = r["Name"].ToString();
                    Obj.UpgradtionDate = r["UpgradtionDate"].ToString();
                    Obj.Package = r["Package"].ToString();
                    Obj.Amount = r["Amount"].ToString();
                    Obj.TopupBy = r["TopupBy"].ToString();
                    Obj.Status = r["Status"].ToString();
                    Obj.PrintingDate = r["PrintingDate"].ToString();
                    Obj.Description = r["Description"].ToString();
                    Obj.PaymentMode = r["PaymentMode"].ToString();
                    Obj.PackageDays = r["PackageDays"].ToString();
                    Obj.PinAmount = r["PinAmount"].ToString();
                    Obj.BV = r["BV"].ToString();
                    Obj.IsCalculated = r["IsCalculated"].ToString();
                    Obj.ROIPercentage = r["ROIPercentage"].ToString();
                    Obj.Package = r["Package"].ToString();
                    Obj.TransactionBy = r["TransactionBy"].ToString();
                    Obj.Status = r["Statuss"].ToString();
                    Obj.TopUpDate = r["TopUpDate"].ToString();
                    Obj.TopupVia = r["TopupVia"].ToString();
                    ViewBag.Total = ds11.Tables[1].Rows[0]["Total"].ToString();
                    lst1.Add(Obj);
                }
                newdata.lsttopupreport = lst1;
            }
            #region ddlstatus
            List<SelectListItem> ddlstatus = Common.BindTopupStatus();
            ViewBag.ddlstatus = ddlstatus;
            #endregion
            #region Product Bind
            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProduct();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlProduct.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlProduct.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["Pk_ProductId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlProduct = ddlProduct;

            #endregion

            return View(newdata);
        }
        [HttpPost]
        [ActionName("TopupReport")]
        [OnAction(ButtonName = "Search")]
        public ActionResult TopupReportBy(AdminReports newdata)
        {
            //if (newdata.LoginId == null)
            //{
            //    newdata.ToLoginID = null;
            //}
            List<AdminReports> lst1 = new List<AdminReports>();

            newdata.BusinessType = newdata.BusinessType == "" ? null : newdata.BusinessType;
            newdata.FromDate = string.IsNullOrEmpty(newdata.FromDate) ? null : Common.ConvertToSystemDate(newdata.FromDate, "dd/MM/yyyy");
            newdata.ToDate = string.IsNullOrEmpty(newdata.ToDate) ? null : Common.ConvertToSystemDate(newdata.ToDate, "dd/MM/yyyy");
            //newdata.LoginId = newdata.ToLoginID;

            newdata.LoginId = newdata.LoginId == "" ? null : newdata.LoginId;

            DataSet ds11 = newdata.GetTopupReport();

            if (ds11 != null && ds11.Tables.Count > 0 && ds11.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds11.Tables[0].Rows)
                {
                    AdminReports Obj = new AdminReports();
                    Obj.ToLoginID = r["Pk_InvestmentId"].ToString();
                    Obj.LoginId = r["LoginId"].ToString();
                    Obj.DisplayName = r["Name"].ToString();
                    Obj.UpgradtionDate = r["UpgradtionDate"].ToString();
                    Obj.Package = r["Package"].ToString();
                    Obj.Amount = r["Amount"].ToString();
                    Obj.TopupBy = r["TopupBy"].ToString();
                    Obj.Status = r["Status"].ToString();
                    Obj.PrintingDate = r["PrintingDate"].ToString();
                    Obj.Description = r["Description"].ToString();
                    Obj.PaymentMode = r["PaymentMode"].ToString();
                    Obj.PackageDays = r["PackageDays"].ToString();
                    Obj.PinAmount = r["PinAmount"].ToString();
                    Obj.BV = r["BV"].ToString();
                    Obj.IsCalculated = r["IsCalculated"].ToString();
                    Obj.ROIPercentage = r["ROIPercentage"].ToString();
                    Obj.Package = r["Package"].ToString();
                    Obj.TransactionBy = r["TransactionBy"].ToString();
                    Obj.Status = r["Statuss"].ToString();
                    Obj.TopUpDate = r["TopUpDate"].ToString();
                    Obj.TopupVia = r["TopupVia"].ToString();
                    ViewBag.Total = ds11.Tables[1].Rows[0]["Total"].ToString();
                    lst1.Add(Obj);
                }
                newdata.lsttopupreport = lst1;
            }
            #region ddlstatus
            List<SelectListItem> ddlstatus = Common.BindTopupStatus();
            ViewBag.ddlstatus = ddlstatus;
            #endregion
            #region Product Bind
            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProduct();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlProduct.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlProduct.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["Pk_ProductId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlProduct = ddlProduct;

            #endregion

            return View(newdata);
        }
        #endregion

        public ActionResult DownTeamTree(string Ids, string FK_UserId)
        {
            List<SelectListItem> AssociateStatus = Common.AssociateStatus();
            ViewBag.ddlStatus = AssociateStatus;
            List<SelectListItem> Leg = Common.LegType();
            ViewBag.ddlleg = Leg;

            Reports model = new Reports();

            List<Reports> lst = new List<Reports>();
            if (Ids == null || Ids == "")
            {
                model.Ids = "1";
            }
            else
            {
                model.Ids = Ids;
            }
            if (FK_UserId != null || FK_UserId != "")
            {
                model.Fk_UserId = FK_UserId;
            }
            model.DirectStatus = "Self";
            DataSet ds = model.GetDirectList();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() != "0")
                {
                    Ids = "";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Reports obj = new Reports();
                        obj.Mobile = r["Mobile"].ToString();
                        obj.Email = r["Email"].ToString();
                        obj.SponsorId = r["SponsorId"].ToString();
                        obj.SponsorName = r["SponsorName"].ToString();
                        obj.JoiningDate = r["JoiningDate"].ToString();
                        obj.Leg = r["Leg"].ToString();
                        obj.PermanentDate = (r["PermanentDate"].ToString());
                        obj.Status = (r["Status"].ToString());
                        obj.LoginId = (r["LoginId"].ToString());
                        obj.Name = (r["Name"].ToString());
                        obj.Level = (r["Lvl"].ToString());
                        obj.Package = (r["ProductName"].ToString());
                        Ids = Ids + r["PK_UserId"].ToString() + ",";
                        lst.Add(obj);
                    }
                    model.lstassociate = lst;
                    model.Ids = Ids;
                }
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("DownTeamTree")]
        [OnAction(ButtonName = "Search")]
        public ActionResult DownTeamTree(Reports model)
        {

            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            List<Reports> lst = new List<Reports>();

            if (model.Ids == null || model.Ids == "")
            {
                model.Ids = model.Fk_UserId;
            }
            model.DirectStatus = "Self";
            DataSet ds = model.GetDirectList();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string Ids = "";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Reports obj = new Reports();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.SponsorId = r["SponsorId"].ToString();
                    obj.SponsorName = r["SponsorName"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.Leg = r["Leg"].ToString();
                    obj.PermanentDate = (r["PermanentDate"].ToString());
                    obj.Status = (r["Status"].ToString());
                    obj.LoginId = (r["LoginId"].ToString());
                    obj.Name = (r["Name"].ToString());
                    obj.Level = (r["Lvl"].ToString());
                    obj.Package = (r["ProductName"].ToString());
                    Ids = Ids + r["PK_UserId"].ToString() + ",";
                    lst.Add(obj);
                }
                model.lstassociate = lst;
                model.Ids = Ids;
            }
            List<SelectListItem> AssociateStatus = Common.AssociateStatus();
            ViewBag.ddlStatus = AssociateStatus;
            List<SelectListItem> Leg = Common.LegType();
            ViewBag.ddlleg = Leg;
            return View(model);
        }



        //[HttpPost]
        //[ActionName("DirectListForAdmin")]
        //[OnAction(ButtonName = "Search")]
        //public ActionResult DirectListForAdmin(AdminReports model)
        //{

        //    model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
        //    model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
        //    List<AdminReports> lst = new List<AdminReports>();
        //    //model.LoginId = Session["LoginId"].ToString();
        //    DataSet ds = model.GetDirectList();

        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow r in ds.Tables[0].Rows)
        //        {
        //            AdminReports obj = new AdminReports();
        //            obj.Mobile = r["Mobile"].ToString();
        //            obj.Email = r["Email"].ToString();
        //            obj.Leg = r["Leg"].ToString();
        //            obj.JoiningDate = r["JoiningDate"].ToString();
        //            obj.PermanentDate = (r["PermanentDate"].ToString());
        //            obj.Status = (r["Status"].ToString());
        //            obj.SponsorId = (r["LoginId"].ToString());
        //            obj.SponsorName = (r["Name"].ToString());
        //            obj.Package = (r["ProductName"].ToString());
        //            lst.Add(obj);
        //        }
        //        model.lstDirect = lst;
        //    }
        //    List<SelectListItem> AssociateStatus = Common.AssociateStatus();
        //    ViewBag.ddlStatus = AssociateStatus;
        //    List<SelectListItem> Leg = Common.LegType();
        //    ViewBag.ddlleg = Leg;
        //    return View(model);
        //}


        public ActionResult ViewProfileForAdmin(string Id)
        {
            AdminReports model = new AdminReports();
            List<SelectListItem> Gender = Common.BindGender();
            ViewBag.Gender = Gender;
            if (Id != null)
            {
                model.Fk_UserId = Id;
                DataSet ds = model.GetAdminProfileDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    model.Fk_UserId = ds.Tables[0].Rows[0]["PK_UserId"].ToString();
                    model.SponsorId = ds.Tables[0].Rows[0]["SponsorId"].ToString();
                    model.SponsorName = ds.Tables[0].Rows[0]["SponserName"].ToString();
                    model.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    model.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    model.Gender = ds.Tables[0].Rows[0]["Sex"].ToString();
                    model.AdharNo = ds.Tables[0].Rows[0]["AdharNumber"].ToString();
                    model.PanNo = ds.Tables[0].Rows[0]["PanNumber"].ToString();
                    model.PinCode = ds.Tables[0].Rows[0]["PinCode"].ToString();
                    model.State = ds.Tables[0].Rows[0]["State"].ToString();
                    model.City = ds.Tables[0].Rows[0]["City"].ToString();
                    model.MobileNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    model.NomineeName = ds.Tables[0].Rows[0]["NomineeName"].ToString();
                    model.NomineeAge = ds.Tables[0].Rows[0]["NomineeAge"].ToString();
                    model.NomineeRelation = ds.Tables[0].Rows[0]["NomineeRelation"].ToString();
                    model.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    model.BankName = ds.Tables[0].Rows[0]["MemberBankName"].ToString();
                    model.AccountNo = ds.Tables[0].Rows[0]["MemberAccNo"].ToString();
                    model.IFSCCode = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    model.BranchName = ds.Tables[0].Rows[0]["MemberBranch"].ToString();
                    model.UPIID = ds.Tables[0].Rows[0]["UPIID"].ToString();
                    model.Image = ds.Tables[0].Rows[0]["PanImage"].ToString();

                }
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("ViewProfileForAdmin")]
        [OnAction(ButtonName = "Update")]
        public ActionResult ViewProfileForAdmin(AdminReports model, HttpPostedFileBase Image)
        {
            try
            {
                List<SelectListItem> Gender = Common.BindGender();
                ViewBag.Gender = Gender;
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                if (Image != null)
                {
                    model.Image = "/PanUpload/" + Guid.NewGuid() + Path.GetExtension(Image.FileName);
                    Image.SaveAs(Path.Combine(Server.MapPath(model.Image)));
                }
                DataSet ds = model.UpdateAdminProfile();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["AdminProfile"] = "User profile updated successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["AdminProfile"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AdminProfile"] = ex.Message;
            }
            return RedirectToAction("ViewProfileForAdmin", "AdminReports", new { Id = model.Fk_UserId });
        }
        public ActionResult DeleteUerDetails(string Id)
        {
            try
            {
                AdminReports model = new AdminReports();
                model.Fk_UserId = Id;
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.DeleteUerDetails();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "User deleted successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("AssociateList", "AdminReports");
        }

        public ActionResult ViewProfile(string Id)
        {
            AdminReports model = new AdminReports();
            List<SelectListItem> Gender = Common.BindGender();
            ViewBag.Gender = Gender;
            if (Id != null)
            {
                model.Fk_UserId = Id;
                DataSet ds = model.GetAdminProfileDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.Fk_UserId = ds.Tables[0].Rows[0]["PK_UserId"].ToString();
                    ViewBag.SponsorId = ds.Tables[0].Rows[0]["SponsorId"].ToString();
                    ViewBag.SponsorName = ds.Tables[0].Rows[0]["SponserName"].ToString();
                    ViewBag.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    ViewBag.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    model.Gender = ds.Tables[0].Rows[0]["Sex"].ToString();
                    ViewBag.AdharNo = ds.Tables[0].Rows[0]["AdharNumber"].ToString();
                    ViewBag.PanNo = ds.Tables[0].Rows[0]["PanNumber"].ToString();
                    ViewBag.PinCode = ds.Tables[0].Rows[0]["PinCode"].ToString();
                    ViewBag.State = ds.Tables[0].Rows[0]["State"].ToString();
                    ViewBag.City = ds.Tables[0].Rows[0]["City"].ToString();
                    ViewBag.MobileNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    ViewBag.NomineeName = ds.Tables[0].Rows[0]["NomineeName"].ToString();
                    ViewBag.NomineeAge = ds.Tables[0].Rows[0]["NomineeAge"].ToString();
                    ViewBag.NomineeRelation = ds.Tables[0].Rows[0]["NomineeRelation"].ToString();
                    ViewBag.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    ViewBag.BankName = ds.Tables[0].Rows[0]["MemberBankName"].ToString();
                    ViewBag.BranchName = ds.Tables[0].Rows[0]["MemberBranch"].ToString();
                    ViewBag.AccountNo = ds.Tables[0].Rows[0]["MemberAccNo"].ToString();
                    ViewBag.IFSCCode = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    ViewBag.PanImage = ds.Tables[0].Rows[0]["PanImage"].ToString();
                    ViewBag.ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString();
                    ViewBag.UPI = ds.Tables[0].Rows[0]["UPIID"].ToString();
                    ViewBag.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                }
            }
            return View(model);
        }
        public ActionResult ViewProfileVeriFy(string Id)
        {
            AdminReports model = new AdminReports();
            try
            {
                model.Fk_UserId = Id;
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ViewProfileVeriFy();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["verify"] = "Profile verified successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["verify"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["verify"] = ex.Message;
            }
            return RedirectToAction("KYCUpdateDeatilsOfUser", "Admin");
        }


        public ActionResult WalletLedger()
        {
            AdminReports model = new AdminReports();
            List<AdminReports> lst = new List<AdminReports>();
            DataSet ds = model.WalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.UserId = r["FK_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.AvailableBalance = r["AvailableBalance"].ToString();
                    lst.Add(obj);
                }
                ViewBag.CrAmount = double.Parse(ds.Tables[0].Compute("sum(CrAmount)", "").ToString()).ToString("n2");
                ViewBag.DrAmount = double.Parse(ds.Tables[0].Compute("sum(DrAmount)", "").ToString()).ToString("n2");
                ViewBag.AvailableBalance = double.Parse(ds.Tables[0].Compute("sum(AvailableBalance)", "").ToString()).ToString("n2");
                model.lstWalletLedger = lst;

            }
            return View(model);
        }

        [HttpPost]
        [ActionName("WalletLedger")]
        [OnAction(ButtonName = "Search")]
        public ActionResult WalletLedger(AdminReports model)
        {
            List<AdminReports> lst = new List<AdminReports>();
            DataSet ds = model.WalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.UserId = r["FK_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.AvailableBalance = r["AvailableBalance"].ToString();
                    lst.Add(obj);
                }
                ViewBag.CrAmount = double.Parse(ds.Tables[0].Compute("sum(CrAmount)", "").ToString()).ToString("n2");
                ViewBag.DrAmount = double.Parse(ds.Tables[0].Compute("sum(DrAmount)", "").ToString()).ToString("n2");
                ViewBag.AvailableBalance = double.Parse(ds.Tables[0].Compute("sum(AvailableBalance)", "").ToString()).ToString("n2");
                model.lstWalletLedger = lst;
            }
            return View(model);
        }


        public ActionResult ActivateByPaymentList()
        {
            AdminReports model = new AdminReports();
            List<AdminReports> lst = new List<AdminReports>();
            model.LoginId = model.LoginId == "" ? null : model.LoginId;
            model.Name = model.Name == "" ? null : model.Name;
            DataSet ds = model.GetActivateByPaymentDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.Pk_EwalletId = r["Pk_InvestmentId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.PinAmount = r["PinAmount"].ToString();
                    obj.UsedFor = r["UsedFor"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.IsCalculated = r["IsCalculated"].ToString();
                    obj.TransactionBy = r["TransactionBy"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.TransactionDate = r["ActivatationDate"].ToString();
                    lst.Add(obj);
                }
                model.lstActivateByPayment = lst;
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("ActivateByPaymentList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult ActivateByPaymentList(AdminReports model)
        {
            List<AdminReports> lst = new List<AdminReports>();
            model.LoginId = model.LoginId == "" ? null : model.LoginId;
            model.Name = model.Name == "" ? null : model.Name;
            model.UsedFor = model.UsedFor == "" ? null : model.UsedFor;
            DataSet ds = model.GetActivateByPaymentDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.Pk_EwalletId = r["Pk_InvestmentId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.PinAmount = r["PinAmount"].ToString();
                    obj.UsedFor = r["UsedFor"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.IsCalculated = r["IsCalculated"].ToString();
                    obj.TransactionBy = r["TransactionBy"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.TransactionDate = r["ActivatationDate"].ToString();
                    lst.Add(obj);
                }
                model.lstActivateByPayment = lst;
            }
            return View(model);
        }

        public ActionResult GetStateCity(string PinCode)
        {
            Common obj = new Common();
            obj.PinCode = PinCode;
            DataSet ds = obj.GetStateCity();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                obj.State = ds.Tables[0].Rows[0]["State"].ToString();
                obj.City = ds.Tables[0].Rows[0]["City"].ToString();
                obj.Result = "1";
            }
            else
            {
                obj.Result = "Invalid PinCode";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeclineKyc(string Id)
        {
            AdminReports model = new AdminReports();
            try
            {
                model.Fk_UserId = Id;
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.DeclinedKyc();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["verify"] = "Kyc declined successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["verify"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["verify"] = ex.Message;
            }
            return RedirectToAction("KYCUpdateDeatilsOfUser", "Admin");
        }
        public ActionResult DeleteTopUp(string Id)
        {
            AdminReports model = new AdminReports();
            try
            {
                model.Pk_investmentId = Id;
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.DeleteTopUp();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Top-Up deleted successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("TopUpReport", "AdminReports");
        }



        public ActionResult ContactList()
        {
            AdminReports model = new AdminReports();
            List<AdminReports> lst = new List<AdminReports>();
            model.Name = model.Name == "" ? null : model.Name;
            DataSet ds = model.ContactList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.Name = r["Name"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Subject = r["Subject"].ToString();
                    obj.Message = r["Message"].ToString();
                    obj.Date = r["Date"].ToString();
                    lst.Add(obj);
                }
                model.lstcontact = lst;
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("ContactList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult ContactList(AdminReports model)
        {
            List<AdminReports> lst = new List<AdminReports>();
            model.Name = model.Name == "" ? null : model.Name;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.ContactList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.Name = r["Name"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Subject = r["Subject"].ToString();
                    obj.Message = r["Message"].ToString();
                    obj.Date = r["Date"].ToString();
                    lst.Add(obj);
                }
                model.lstcontact = lst;
            }
            return View(model);
        }
        public ActionResult DirectListForAdmin(string AssociateID, string FK_UserId)
        {
            AssociateBooking model = new AssociateBooking();
            if (AssociateID != null && AssociateID != "")
            {
                model.Fk_UserId = AssociateID;
            }
            else 
            {
                
            }
            model.FK_RootId = FK_UserId;
            List<AssociateBooking> lst = new List<AssociateBooking>();
            DataSet ds = model.GetDownlineTree();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {

                }
                else
                {
                    ViewBag.Fk_SponsorId = ds.Tables[0].Rows[0]["Fk_SponsorId"].ToString();
                    ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        AssociateBooking obj = new AssociateBooking();
                        obj.Fk_UserId = r["Pk_UserId"].ToString();
                        obj.Fk_SponsorId = r["Fk_SponsorId"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.FirstName = r["FirstName"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.ActiveStatus = r["ActiveStatus"].ToString();
                        obj.SponsorID = r["SponsorId"].ToString();
                        obj.SponsorName = r["SponsorName"].ToString();
                        obj.ActivationDate = r["PermanentDate"].ToString();
                        obj.Lvl = r["Level"].ToString();
                        lst.Add(obj);
                    }
                    model.lstPlot = lst;
                }

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult DirectListForAdmin(AssociateBooking model, string AssociateID)
        {
            if (AssociateID != null && AssociateID != "")
            {
                model.Fk_UserId = AssociateID;
            }
            else
            {

            }
            model.FK_RootId = model.Fk_UserId;
            List<AssociateBooking> lst = new List<AssociateBooking>();
            DataSet ds = model.GetDownlineTree();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {

                }
                else
                {
                    ViewBag.Fk_SponsorId = ds.Tables[0].Rows[0]["Fk_SponsorId"].ToString();
                    ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        AssociateBooking obj = new AssociateBooking();
                        obj.Fk_UserId = r["Pk_UserId"].ToString();
                        obj.Fk_SponsorId = r["Fk_SponsorId"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.FirstName = r["FirstName"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.ActiveStatus = r["ActiveStatus"].ToString();
                        obj.SponsorID = r["SponsorId"].ToString();
                        obj.SponsorName = r["SponsorName"].ToString();
                        obj.ActivationDate = r["PermanentDate"].ToString();
                        obj.Lvl = r["Level"].ToString();
                        lst.Add(obj);
                    }
                    model.lstPlot = lst;
                }

            }
            return View(model);
        }
        #region TreeTTP ForAdmin
        public ActionResult TreeTTPForAdmin(string LoginId, string Id)
        {
            Tree model = new Tree();
            if (LoginId != "" && LoginId != null)
            {
                model.RootAgentCode = LoginId;
                model.LoginId = LoginId;
                model.PK_UserId = Id;
            }
            else
            {
                model.RootAgentCode = "RealWealth";
                model.PK_UserId = "1";
                model.LoginId = "RealWealth";
                model.DisplayName = "RealWealth";
            }
            List<TreeMembers> lst = new List<TreeMembers>();
            List<MemberDetails> lstMember = new List<MemberDetails>();
            DataSet ds = model.GetLevelMembersCountTR1();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {

                }
                else
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        TreeMembers obj = new TreeMembers();
                        obj.LevelName = r["LevelNo"].ToString();
                        obj.NumberOfMembers = r["TotalAssociate"].ToString();
                        lst.Add(obj);
                    }
                    model.lst = lst;
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.Level = ds.Tables[1].Rows[0]["Lvl"].ToString();
                        ViewBag.Status = ds.Tables[1].Rows[0]["Status"].ToString();
                        model.Color = ds.Tables[1].Rows[0]["Color"].ToString();
                        model.DisplayName = ds.Tables[1].Rows[0]["Name"].ToString();
                        model.PK_UserId = ds.Tables[1].Rows[0]["PK_UserId"].ToString();
                        model.ProfilePic = ds.Tables[1].Rows[0]["ProfilePic"].ToString();
                        model.TotalDirect = ds.Tables[1].Rows[0]["TotalDirect"].ToString();
                        model.TotalActive = ds.Tables[1].Rows[0]["TotalActive"].ToString();
                        model.TotalInactive = ds.Tables[1].Rows[0]["TotalInActive"].ToString();
                        model.TotalTeam = ds.Tables[1].Rows[0]["TotalTeam"].ToString();
                        model.TotalActiveTeam = ds.Tables[1].Rows[0]["TotalActiveTeam"].ToString();
                        model.TotalInActiveTeam = ds.Tables[1].Rows[0]["TotalInActiveTeam"].ToString();
                        model.SponsorName = ds.Tables[1].Rows[0]["SponsorName"].ToString();
                    }
                    model.Level = "1";
                    DataSet ds1 = model.GetLevelMembers();
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow r in ds1.Tables[0].Rows)
                        {
                            MemberDetails obj = new MemberDetails();
                            obj.PK_UserId = r["PK_UserId"].ToString();
                            obj.MemberName = r["MemberName"].ToString();
                            obj.LoginId = r["LoginId"].ToString();
                            obj.Level = r["Lvl"].ToString();
                            obj.ProfilePic = r["ProfilePic"].ToString();
                            obj.Status = r["Status"].ToString();
                            obj.SelfBV = r["SelfBV"].ToString();
                            obj.TeamBV = r["TeamBV"].ToString();
                            obj.SponsorName = r["SponsorName"].ToString();
                            obj.Color = r["Color"].ToString();
                            lstMember.Add(obj);
                        }
                        model.lstMember = lstMember;
                    }
                }
            }
            return View(model);
        }
         #endregion
        public ActionResult TreeForAdmin(string LoginId, string Id)
        {
            Tree model = new Tree();
            if (LoginId != "" && LoginId != null)
            {
                model.RootAgentCode = LoginId;
                model.LoginId = LoginId;
                model.PK_UserId = Id;
            }
            else
            {
                model.RootAgentCode = "RealWealth";
                model.PK_UserId = "1";
                model.LoginId = "RealWealth";
                model.DisplayName = "RealWealth";
            }
            List<TreeMembers> lst = new List<TreeMembers>();
            List<MemberDetails> lstMember = new List<MemberDetails>();
            DataSet ds = model.GetLevelMembersCount();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    TreeMembers obj = new TreeMembers();
                    obj.LevelName = r["LevelNo"].ToString();
                    obj.NumberOfMembers = r["TotalAssociate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                ViewBag.Level = ds.Tables[1].Rows[0]["Lvl"].ToString();
                ViewBag.Status = ds.Tables[1].Rows[0]["Status"].ToString();
                model.Color = ds.Tables[1].Rows[0]["Color"].ToString();
                model.DisplayName = ds.Tables[1].Rows[0]["Name"].ToString();
                model.PK_UserId = ds.Tables[1].Rows[0]["PK_UserId"].ToString();
                model.ProfilePic = ds.Tables[1].Rows[0]["ProfilePic"].ToString();
                model.TotalDirect = ds.Tables[1].Rows[0]["TotalDirect"].ToString();
                model.TotalActive = ds.Tables[1].Rows[0]["TotalActive"].ToString();
                model.TotalInactive = ds.Tables[1].Rows[0]["TotalInActive"].ToString();
                model.TotalTeam = ds.Tables[1].Rows[0]["TotalTeam"].ToString();
                model.TotalActiveTeam = ds.Tables[1].Rows[0]["TotalActiveTeam"].ToString();
                model.TotalInActiveTeam = ds.Tables[1].Rows[0]["TotalInActiveTeam"].ToString();
                model.SponsorName = ds.Tables[1].Rows[0]["SponsorName"].ToString();
                model.SelfBV = ds.Tables[1].Rows[0]["SelfBV"].ToString();
                model.TeamBV = ds.Tables[1].Rows[0]["TeamBV"].ToString();
                model.SelfBVDollar = Math.Round((Convert.ToDouble(ds.Tables[1].Rows[0]["SelfBV"]) / 76.805), 2).ToString();
                model.TeamBVDollar = Math.Round((Convert.ToDouble(ds.Tables[1].Rows[0]["TeamBV"]) / 76.805), 2).ToString();
                model.SponsorName = ds.Tables[1].Rows[0]["SponsorName"].ToString();
            }
            model.Level = "1";
            DataSet ds1 = model.GetLevelMembers();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    MemberDetails obj = new MemberDetails();
                    obj.PK_UserId = r["PK_UserId"].ToString();
                    obj.MemberName = r["MemberName"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.ProfilePic = r["ProfilePic"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.SelfBV = r["SelfBV"].ToString();
                    obj.TeamBV = r["TeamBV"].ToString();
                    //obj.SelfBVDollar = (Convert.ToDouble(r["SelfBV"]) / 76.805).ToString();
                    //obj.TeamBVDollar = (Convert.ToDouble(r["TeamBV"]) / 76.805).ToString();
                    obj.SponsorName = r["SponsorName"].ToString();
                    obj.Color = r["Color"].ToString();
                    lstMember.Add(obj);
                }
                model.lstMember = lstMember;
            }
            return View(model);
        }


    }
}