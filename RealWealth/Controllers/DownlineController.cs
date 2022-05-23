using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealWealth.Models;
using RealWealth.Filter;
using RealWealth.Controllers;

namespace RealWealth.Controllers
{
    public class DownlineController : UserBaseController
    {
        //
        // GET: /Downline/

        public ActionResult DirectList(string Ids)
        {
            List<SelectListItem> AssociateStatus = Common.AssociateStatus();
            ViewBag.ddlStatus = AssociateStatus;
            List<SelectListItem> Leg = Common.LegType();
            ViewBag.ddlleg = Leg;

            Reports model = new Reports();
           
            List<Reports> lst = new List<Reports>();
            if (Ids == null || Ids == "")
            {
                model.Ids = Session["Pk_UserId"].ToString();
               
            }
            else
            {
                model.Ids = Ids;
              
            }
            model.DirectStatus = "Self";
            model.Fk_UserId = Session["Pk_UserId"].ToString();
           
            DataSet ds = model.GetDirectList();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if(ds.Tables[0].Rows[0][0].ToString()!="0")
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
        [ActionName("DirectList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult DirectListBy(Reports model)
        {

            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            List<Reports> lst = new List<Reports>();

            if (model.Ids == null || model.Ids == "")
            {
                model.Ids = Session["Pk_UserId"].ToString();

            }
            model.Fk_UserId = Session["Pk_UserId"].ToString();
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


        public ActionResult DownLineList()
        {
            List<SelectListItem> AssociateStatus = Common.AssociateStatus();
            ViewBag.ddlStatus = AssociateStatus;
            List<SelectListItem> Leg = Common.LegType();
            ViewBag.ddlleg = Leg;

            Reports model = new Reports();
            List<Reports> lst = new List<Reports>();
            model.LoginId = Session["LoginId"].ToString();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetDownlineList();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Reports obj = new Reports();
                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.Leg = r["Leg"].ToString();
                    obj.PermanentDate = (r["PermanentDate"].ToString());
                    obj.Status = (r["Status"].ToString());
                    obj.Mobile = (r["Mobile"].ToString());
                    obj.Package = (r["ProductName"].ToString());
                    lst.Add(obj);
                }
                model.lstassociate = lst;


            }
            return View(model);
        }
        [HttpPost]
        [ActionName("DownLineList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult DownLineListBy(Reports model)
        {

            List<Reports> lst = new List<Reports>();
            model.LoginId = Session["LoginId"].ToString();
            DataSet ds = model.GetDownlineList();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Reports obj = new Reports();
                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.Leg = r["Leg"].ToString();
                    obj.PermanentDate = (r["PermanentDate"].ToString());
                    obj.Status = (r["Status"].ToString());
                    obj.Mobile = (r["Mobile"].ToString());
                    obj.Package = (r["ProductName"].ToString());
                    lst.Add(obj);
                }
                model.lstassociate = lst;
            }
            List<SelectListItem> AssociateStatus = Common.AssociateStatus();
            ViewBag.ddlStatus = AssociateStatus;
            List<SelectListItem> Leg = Common.LegType();
            ViewBag.ddlleg = Leg;
            return View(model);
        }
        #region  associate tree
        public ActionResult AssociateTree(AssociateBooking model, string AssociateID)
        {
            if (AssociateID != null && AssociateID != "")
            {
                model.Fk_UserId = AssociateID;
            }
            else
            {
                model.Fk_UserId = Session["Pk_UserId"].ToString();
            }
            model.FK_RootId = Session["Pk_UserId"].ToString();
            List<AssociateBooking> lst = new List<AssociateBooking>();
            DataSet ds = model.GetDownlineTree();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
            return View(model);
        }
        #endregion
    }
}
