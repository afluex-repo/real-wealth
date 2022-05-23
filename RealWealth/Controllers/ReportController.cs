using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealWealth.Models;
using System.Data.SqlClient;
using RealWealth.Controllers;
using System.Data;

namespace RealWealth.Controllers
{
    public class ReportController : UserBaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PayoutWalletLedger()
        {
            List<UserReports> lst = new List<UserReports>();
            UserReports model = new UserReports();
            model.LoginId = Session["LoginId"].ToString();
            DataSet ds = model.PayoutWalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.PK_PayoutWalletId = r["PK_PayoutWalletId"].ToString();
                    obj.FK_UserId = r["FK_UserId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult PayoutWalletLedger(UserReports model)
        {
            List<UserReports> lst = new List<UserReports>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            model.LoginId = Session["LoginId"].ToString();
            DataSet ds = model.PayoutWalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.PK_PayoutWalletId = r["PK_PayoutWalletId"].ToString();
                    obj.FK_UserId = r["FK_UserId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        public ActionResult LevelIncomeTr1Total()
        {
            List<UserReports> lst = new List<UserReports>();
            UserReports model = new UserReports();
            model.FK_UserId = Session["Pk_UserId"].ToString();
            DataSet ds = model.LevelIncomeTr1Total();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        public ActionResult LevelIncomeTr1(string Lvl)
        {
            List<UserReports> lst = new List<UserReports>();
            UserReports model = new UserReports();
            model.LoginId = Session["LoginId"].ToString();
            model.Status ="0";
            model.Level = Lvl;
            DataSet ds = model.LevelIncomeTr1();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LevelIncomeTr1(UserReports model)
        {
            List<UserReports> lst = new List<UserReports>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            model.LoginId = Session["LoginId"].ToString();
            DataSet ds = model.LevelIncomeTr1();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        public ActionResult LevelIncomeTr2Total()
        {
            List<UserReports> lst = new List<UserReports>();
            UserReports model = new UserReports();
            model.FK_UserId = Session["Pk_UserId"].ToString();
            DataSet ds = model.LevelIncomeTr2Total();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        public ActionResult LevelIncomeTr2(string Lvl)
        {
            List<UserReports> lst = new List<UserReports>();
            UserReports model = new UserReports();
            model.LoginId = Session["LoginId"].ToString();
            model.Status = "0";
            model.Level = Lvl;
            DataSet ds = model.LevelIncomeTr2();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LevelIncomeTr2(UserReports model)
        {
            List<UserReports> lst = new List<UserReports>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            model.LoginId = Session["LoginId"].ToString();
            DataSet ds = model.LevelIncomeTr2();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        public ActionResult PayoutDetail()
        {
            List<UserReports> lst = new List<UserReports>();
            UserReports model = new UserReports();
            model.FK_UserId = Session["Pk_UserId"].ToString();
            DataSet ds = model.PayoutDetail();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.FK_UserId = r["Fk_Userid"].ToString();
                    obj.LevelIncomeTR1 = r["LevelIncomeTR1"].ToString();
                    obj.LevelIncomeTR2 = r["LevelIncomeTR2"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.ClosingDate = r["ClosingDate"].ToString();
                    obj.GrossAmount = r["GrossAmount"].ToString();
                    obj.ProcessingFee = r["AdminFee"].ToString();
                    obj.TDSAmount = r["TDSAmount"].ToString();
                    obj.NetAmount = r["NetAmount"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult PayoutDetail(UserReports model)
        {
            List<UserReports> lst = new List<UserReports>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            model.FK_UserId = Session["Pk_UserId"].ToString();
            DataSet ds = model.PayoutDetail();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.FK_UserId = r["Fk_Userid"].ToString();
                    obj.LevelIncomeTR1 = r["LevelIncomeTR1"].ToString();
                    obj.LevelIncomeTR2 = r["LevelIncomeTR2"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.ClosingDate = r["ClosingDate"].ToString();
                    obj.GrossAmount = r["GrossAmount"].ToString();
                    obj.ProcessingFee = r["AdminFee"].ToString();
                    obj.TDSAmount = r["TDSAmount"].ToString();
                    obj.NetAmount = r["NetAmount"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
     

        public ActionResult PaidIncome(string PayoutNo)
        {
            UserReports model = new UserReports();
            List<UserReports> lst = new List<UserReports>();
            model.LoginId = Session["LoginId"].ToString();
            model.PayoutNo = PayoutNo;
            DataSet ds = model.PaidIncome();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    UserReports obj = new UserReports();
                    obj.FromName = r["FromName"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    obj.CommissionPercentage = r["CommissionPercentage"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
    }
}