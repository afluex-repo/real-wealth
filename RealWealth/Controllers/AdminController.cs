using RealWealth.Filter;
using RealWealth.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealWealth.Models;
using System.IO;

namespace RealWealth.Controllers
{
    public class AdminController : AdminBaseController
    {
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            Dashboard newdata = new Dashboard();
            DataSet Ds = newdata.GetDashBoardDetails();

            ViewBag.TotalUsers = Ds.Tables[1].Rows[0]["TotalUsers"].ToString();
            ViewBag.BlockedUsers = Ds.Tables[1].Rows[0]["BlockedUsers"].ToString();
            ViewBag.InactiveUsers = Ds.Tables[1].Rows[0]["InactiveUsers"].ToString();
            ViewBag.ActiveUsers = Ds.Tables[1].Rows[0]["ActiveUsers"].ToString();
            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[2].Rows.Count > 0)
            {
                ViewBag.Tr1Business = Ds.Tables[2].Rows[0]["Tr1Business"].ToString();
                ViewBag.Tr2Business = Ds.Tables[2].Rows[0]["Tr2Business"].ToString();
            }
            return View(newdata);
        }
        #region GeneratePin
        public ActionResult Generate_EPin()
        {
            Admin obj = new Admin();
            List<Admin> lst = new List<Admin>();
            #region Product Bind
            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProductForJoining();
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
            DataSet dsp = obj.GetPinGeneratedByAdmin();
            if (dsp.Tables != null && dsp.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsp.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    Objload.Fk_UserId = dr["FK_UserId"].ToString();
                    Objload.LoginId = dr["LoginId"].ToString();
                    Objload.Name = dr["Name"].ToString();
                    Objload.PK_ProductID = dr["FK_ProductId"].ToString();
                    Objload.TotalPin = dr["TotalPins"].ToString();
                    Objload.Package = dr["ProductName"].ToString();
                    Objload.UsedPin = dr["UsedPins"].ToString();
                    Objload.AvailablePin = dr["AvaliablePins"].ToString();
                    Objload.TransferPin = dr["TransferPins"].ToString();
                    Objload.PinAmount = dr["PinAmount"].ToString();
                    Objload.FinalAmount = dr["FinalAmount"].ToString();
                    Objload.AddedOn = dr["AddedOn"].ToString();
                    Objload.PaymentMode = dr["PaymentMode"].ToString();
                    lst.Add(Objload);
                }
                obj.lst = lst;
            }
            #endregion
            #region PaymentMode
            Common com = new Common();
            List<SelectListItem> ddlPayment = new List<SelectListItem>();
            DataSet ds = com.PaymentList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int paycount = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (paycount == 0)
                    {
                        ddlPayment.Add(new SelectListItem { Text = "Select Payment", Value = "0" });
                    }
                    ddlPayment.Add(new SelectListItem { Text = r["PaymentMode"].ToString(), Value = r["PK_paymentID"].ToString() });
                    paycount++;
                }
            }

            ViewBag.ddlPayment = ddlPayment;

            #endregion

            return View(obj);
        }
        [HttpPost]
        [ActionName("Generate_EPin")]
        [OnAction(ButtonName = "btn_Pin")]
        public ActionResult CreatePinAction(Admin obj)
        {
            try
            {
                obj.AddedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = obj.CreatePin();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["createpin"] = "Pin Created Successfully";
                    }
                    else
                    {
                        TempData["createpin"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }

                }
                else { }

            }
            catch (Exception ex)
            {
                TempData["createpin"] = ex.Message;
            }
            return RedirectToAction("Generate_EPin", "Admin");
        }
        public ActionResult UnUsedPin(string Fk_UserId,string Fk_ProductId)
        {
            Admin obj = new Admin();
            List<Admin> lst = new List<Admin>();
            obj.Fk_UserId = Fk_UserId;
            obj.Fk_ProductId = Fk_ProductId;
            obj.Status = "P";
            obj.Package = obj.Package == "0" ? null : obj.Package;
            DataSet ds = obj.GetUsedUnUsedPins();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    obj.LoginId = dr["LoginId"].ToString();
                    obj.Name = dr["Name"].ToString();
                    Objload.ePinNo = dr["ePinNo"].ToString();
                    Objload.Package = dr["Package"].ToString();
                    Objload.Amount = dr["PinAmount"].ToString();
                    Objload.GST = dr["GST"].ToString();
                    Objload.TotalAmount = dr["TotalAmount"].ToString();
                    Objload.AddedBy = dr["GenerateVia"].ToString();
                    Objload.ToId = dr["ToId"].ToString();
                    Objload.ToName = dr["ToName"].ToString();
                    Objload.TransferDate = dr["TransferDate"].ToString();
                    Objload.DisplayName = dr["PinUser"].ToString();
                    Objload.AddedOn = dr["CreatedDate"].ToString();
                    Objload.RegisteredTo = dr["RegisteredTo"].ToString();
                    Objload.ActivationDate = dr["UsedDate"].ToString();
                    Objload.Status = dr["PinStaus"].ToString();
                    lst.Add(Objload);
                }
                obj.lstunusedpins = lst;
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult UnUsedPin(Admin obj)
        {
            List<Admin> lst = new List<Admin>();
            obj.Package = obj.Package == "0" ? null : obj.Package;
            DataSet ds = obj.GetUsedUnUsedPins();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    obj.LoginId = dr["LoginId"].ToString();
                    obj.Name = dr["Name"].ToString();
                    Objload.ePinNo = dr["ePinNo"].ToString();
                    Objload.Package = dr["Package"].ToString();
                    Objload.Amount = dr["PinAmount"].ToString();
                    Objload.GST = dr["GST"].ToString();
                    Objload.ToId = dr["ToId"].ToString();
                    Objload.AddedBy = dr["GenerateVia"].ToString();
                    Objload.ToName = dr["ToName"].ToString();
                    Objload.TransferDate = dr["TransferDate"].ToString();
                    Objload.TotalAmount = dr["TotalAmount"].ToString();
                    Objload.DisplayName = dr["PinUser"].ToString();
                    Objload.AddedOn = dr["CreatedDate"].ToString();
                    Objload.RegisteredTo = dr["RegisteredTo"].ToString();
                    Objload.ActivationDate = dr["UsedDate"].ToString();
                    Objload.Status = dr["PinStaus"].ToString();
                    lst.Add(Objload);
                }
                obj.lstunusedpins = lst;
            }
            return View(obj);
        }
        public ActionResult FillAmount(string ProductId)
        {
            Admin obj = new Admin();
            obj.Package = ProductId;
            DataSet ds = obj.BindPriceByProduct();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                obj.Amount = ds.Tables[0].Rows[0]["ProductPrice"].ToString();
                obj.FinalAmount = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
            }
            else { }
            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        #endregion
        public ActionResult GetMemberName(string LoginId)
        {
            Common obj = new Common();
            obj.ReferBy = LoginId;
            DataSet ds = obj.GetMemberDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.DisplayName = ds.Tables[0].Rows[0]["FullName"].ToString();
                obj.Result = "Yes";
            }
            else { obj.Result = "Invalid LoginId"; }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePassword(Admin model)
        {
            try
            {
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ChangePassword();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Password Changed  Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("ChangePassword", "Admin");
        }


        public ActionResult PinTransferReportForAdmin()
        {
            AdminReports model = new AdminReports();
            List<AdminReports> lst = new List<AdminReports>();
            DataSet ds = model.GetTransferPinReport();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.ePinNo = r["EpinNo"].ToString();
                    obj.FromId = r["FromId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.ToId = r["ToId"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.TransferDate = r["TransferDate"].ToString();
                    lst.Add(obj);
                }
                model.lstPinTransfer = lst;
            }
            return View(model);
        }

        [HttpPost]
        [OnAction(ButtonName = "GetDetails")]
        [ActionName("PinTransferReportForAdmin")]
        public ActionResult PinTransferReportForAdmin(AdminReports model)
        {
            List<AdminReports> lst = new List<AdminReports>();
            model.ePinNo = model.ePinNo == "0" ? null : model.ePinNo;
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.ToLoginID = model.ToLoginID == "0" ? null : model.ToLoginID;
            DataSet ds = model.GetTransferPinReport();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.ePinNo = r["EpinNo"].ToString();
                    obj.FromId = r["FromId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.ToId = r["ToId"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.TransferDate = r["TransferDate"].ToString();
                    lst.Add(obj);
                }
                model.lstPinTransfer = lst;
            }
            return View(model);
        }



        public ActionResult WalletList()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            model.PaymentMode = "Offline";
            model.Status = "Pending";
            DataSet ds = model.GetEwalletRequestDetailsForAdmin();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RequestID = r["PK_RequestID"].ToString();
                    obj.UserId = r["FK_UserId"].ToString();
                    obj.RequestCode = r["RequestCode"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.PaymentMode = r["PaymentMode"].ToString() + "- " + r["BankName"].ToString() + "," + r["BankBranch"].ToString() + ",Txn No. -" + r["ChequeDDNo"].ToString() + ",Txn Date- " + r["ChequeDDDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.TransactionDate = r["RequestedDate"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.ChequeDDNo = r["ChequeDDNo"].ToString();
                    obj.ChequeDDDate = r["ChequeDDDate"].ToString();
                    obj.WalletId = r["WalletId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.DisplayName = r["Name"].ToString();
                    lst.Add(obj);
                }
                model.lstWallet = lst;
            }
            return View(model);
        }

        [HttpPost]
        [OnAction(ButtonName = "GetDetails")]
        [ActionName("WalletList")]
        public ActionResult WalletList(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.Status = model.Status == "0" ? null : model.Status;
            model.PaymentMode = model.PaymentMode == "0" ? null : model.PaymentMode;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetEwalletRequestDetailsForAdmin();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RequestID = r["PK_RequestID"].ToString();
                    obj.UserId = r["FK_UserId"].ToString();
                    obj.RequestCode = r["RequestCode"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.PaymentMode = r["PaymentMode"].ToString() + "- " + r["BankName"].ToString() + "," + r["BankBranch"].ToString() + ",Txn No. -" + r["ChequeDDNo"].ToString() + ",Txn Date- " + r["ChequeDDDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.TransactionDate = r["RequestedDate"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.ChequeDDNo = r["ChequeDDNo"].ToString();
                    obj.ChequeDDDate = r["ChequeDDDate"].ToString();
                    obj.WalletId = r["WalletId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.DisplayName = r["Name"].ToString();
                    lst.Add(obj);
                }
                model.lstWallet = lst;
            }
            return View(model);
        }



        public ActionResult Approve(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Approved");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ApproveDeclineEwalletRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Ewallet Request Approved Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("WalletList", "Admin");
        }

        public ActionResult DeClined(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Declined");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ApproveDeclineEwalletRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Ewallet Request Declined Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("WalletList", "Admin");
        }

        public ActionResult PaymentTypeMaster()
        {

            #region ddlSites
            Admin obj = new Admin();
            int count = 0;
            List<Admin> lst = new List<Admin>();
            List<SelectListItem> ddlPaymentRype = new List<SelectListItem>();
            DataSet ds1 = obj.GetPaymentType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlPaymentRype.Add(new SelectListItem { Text = "-Select-", Value = "" });
                    }
                    ddlPaymentRype.Add(new SelectListItem { Text = r["PaymentType"].ToString(), Value = r["PK_PaymentTypeId"].ToString() });
                    count = count + 1;
                }
            }

            ViewBag.ddlPaymentRype = ddlPaymentRype;
            #endregion
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                Admin model = new Admin();
                model.PaymentType = dr["PaymentType"].ToString();
                if (dr["IsActive"].ToString() == "True")
                {
                    model.Status = "Active";
                }
                else
                {
                    model.Status = "Inactive";
                }
                lst.Add(model);
            }
            obj.lstWallet = lst;
            return View(obj);
        }

        [HttpPost]
        [OnAction(ButtonName = "Update")]
        [ActionName("PaymentTypeMaster")]
        public ActionResult PaymentTypeMaster(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                //model.PaymentTypeId = model.PaymentTypeId;
                DataSet ds = model.UpdatePaymentType();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Payment type updated successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("PaymentTypeMaster", "Admin");
        }

        public ActionResult EPinRequestList()
        {
            Admin model = new Admin();
            List<Admin> list = new List<Admin>();
            DataSet dss = model.GetEPinRequestDetails();
            if (dss != null && dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dss.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_RequestID = r["PK_RequestID"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Fk_Paymentid = r["PaymentMode"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.TransactionNo = r["ChequeDDNo"].ToString();
                    obj.TransactionDate = r["ChequeDDDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.NoofPins = r["NoOfPins"].ToString();

                    list.Add(obj);
                }
                model.lstEpinRequest = list;
            }
            return View(model);
        }
        [HttpPost]
        [OnAction(ButtonName = "GetDetails")]
        [ActionName("EPinRequestList")]
        public ActionResult EPinRequestList(Admin model)
        {
            List<Admin> list = new List<Admin>();
            model.Name = model.Name == "0" ? null : model.Name;
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");

            DataSet dss = model.GetEPinRequestDetails();
            if (dss != null && dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dss.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_RequestID = r["PK_RequestID"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Fk_Paymentid = r["PaymentMode"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.TransactionNo = r["ChequeDDNo"].ToString();
                    obj.TransactionDate = r["ChequeDDDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    list.Add(obj);
                }
                model.lstEpinRequest = list;
            }
            return View(model);
        }


        public ActionResult AcceptedEPinRequest(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Accepted");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.AcceptRejectEPinRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "EPin Request Accepted Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("EPinRequestList", "Admin");
        }

        public ActionResult RejectedEPinRequest(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Rejected");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.AcceptRejectEPinRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "EPin Request Rejected Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("EPinRequestList", "Admin");
        }


        public ActionResult ROIWalletForAdmin()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetROIWalletDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RoiWalletId = r["Pk_ROIWalletId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstTps = lst;
            }
            return View(model);
        }


        [HttpPost]
        [ActionName("ROIWalletForAdmin")]
        [OnAction(ButtonName = "Search")]
        public ActionResult ROIWalletForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.LoginId = model.LoginId == "" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetROIWalletDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RoiWalletId = r["Pk_ROIWalletId"].ToString();

                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstTps = lst;
            }
            return View(model);

        }

        public ActionResult ROIIncomeReportsForAdmin()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetROIIncomeReportsDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Fk_UserId = r["Fk_UserId"].ToString();
                    obj.ROIId = r["Pk_ROIId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.TopUpAmount = r["TopUpAmount"].ToString();
                    obj.Date = r["TopUpDate"].ToString();
                    lst.Add(obj);
                }
                model.lstROIIncome = lst;
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("ROIIncomeReportsForAdmin")]
        [OnAction(ButtonName = "Search")]
        public ActionResult ROIIncomeReportsForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetROIIncomeReportsDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Fk_UserId = r["Fk_UserId"].ToString();
                    obj.ROIId = r["Pk_ROIId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.TopUpAmount = r["TopUpAmount"].ToString();
                    obj.Date = r["TopUpDate"].ToString();
                    lst.Add(obj);
                }
                model.lstROIIncome = lst;
            }
            return View(model);
        }

        public ActionResult ViewROIForAdmin(string Id, string UserId)
        {
            Admin model = new Admin();
            model.Pk_investmentId = Id;
            model.Fk_UserId = UserId;
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetROIDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ROIId = r["Pk_ROIId"].ToString();
                    obj.ROI = r["ROI"].ToString();
                    obj.Date = r["ROIDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    lst.Add(obj);
                }
                model.lstROI = lst;
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                ViewBag.ReceivedAmount = ds.Tables[1].Rows[0]["ReceivedAmount"].ToString();
                ViewBag.TotalAmount = ds.Tables[1].Rows[0]["TotalAmount"].ToString();
                ViewBag.BalanceAmount = ds.Tables[1].Rows[0]["BalanceAmount"].ToString();
            }


            return View(model);
        }

        public ActionResult ViewTPSForAdmin(string Id, string UserId)
        {
            Admin model = new Admin();
            model.Pk_investmentId = Id;
            model.Fk_UserId = UserId;
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetROIDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ROIId = r["Pk_ROIId"].ToString();
                    obj.ROI = r["ROI"].ToString();
                    obj.Date = r["ROIDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    lst.Add(obj);
                }
                model.lstROI = lst;
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                ViewBag.ReceivedAmount = ds.Tables[1].Rows[0]["ReceivedAmount"].ToString();
                ViewBag.TotalAmount = ds.Tables[1].Rows[0]["TotalAmount"].ToString();
                ViewBag.BalanceAmount = ds.Tables[1].Rows[0]["BalanceAmount"].ToString();
            }
            return View(model);
        }
        public ActionResult PayoutWalletLedgerForAdmin(string Fk_UserId)
        {
            List<Admin> lst = new List<Admin>();

            Admin model = new Admin();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            model.LoginId = Fk_UserId;
            DataSet ds = model.PayoutWalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                ViewBag.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_PayoutWalletId = r["PK_PayoutWalletId"].ToString();
                    obj.Fk_UserId = r["FK_UserId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.Balance = r["Balance"].ToString();
                    obj.TransactionBy = r["TransactionBy"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult PayoutWalletLedgerForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.PayoutWalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_PayoutWalletId = r["PK_PayoutWalletId"].ToString();
                    obj.Fk_UserId = r["FK_UserId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.Balance = r["Balance"].ToString();
                    obj.TransactionBy = r["TransactionBy"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }

        public ActionResult LevelIncomeTr1ForAdmin()
        {
            List<Admin> lst = new List<Admin>();
            Admin model = new Admin();
            DataSet ds = model.LevelIncomeTr1();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ToName = r["ToName"].ToString();
                    obj.ToLoginID = r["ToLoginId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstlevelIncome = lst;
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("LevelIncomeTr1ForAdmin")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult LevelIncomeTr1ForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");

            DataSet ds = model.LevelIncomeTr1();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ToName = r["ToName"].ToString();
                    obj.ToLoginID = r["ToLoginId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstlevelIncome = lst;
            }
            return View(model);
        }


        public ActionResult LevelIncomeTr2ForAdmin()
        {
            List<Admin> lst = new List<Admin>();
            Admin model = new Admin();
            DataSet ds = model.LevelIncomeTr2();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ToName = r["ToName"].ToString();
                    obj.ToLoginID = r["ToLoginId"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstlevel = lst;
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("LevelIncomeTr2ForAdmin")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult LevelIncomeTr2ForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.LoginId = model.LoginId == "" ? null : model.LoginId;
            model.Name = model.Name == "" ? null : model.Name;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.LevelIncomeTr2();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ToName = r["ToName"].ToString();
                    obj.ToLoginID = r["ToLoginId"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.FromLoginId = r["LoginId"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Percentage = r["CommissionPercentage"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstlevel = lst;
            }
            return View(model);
        }

        public ActionResult PayoutDetailForAdmin()
        {
            List<Admin> lst = new List<Admin>();
            Admin model = new Admin();
            DataSet ds = model.PayoutDetail();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Fk_UserId = r["Fk_Userid"].ToString();
                    obj.LevelIncomeTR1 = r["LevelIncomeTR1"].ToString();
                    obj.LevelIncomeTR2 = r["LevelIncomeTR2"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.ClosingDate = r["ClosingDate"].ToString();
                    obj.GrossAmount = r["GrossAmount"].ToString();
                    obj.ProcessingFee = r["AdminFee"].ToString();
                    obj.TDSAmount = r["TDSAmount"].ToString();
                    obj.NetAmount = r["NetAmount"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    lst.Add(obj);
                }
                model.lstPayout = lst;
                ViewBag.LevelIncomeTR1 = double.Parse(ds.Tables[0].Compute("sum(LevelIncomeTR1)", "").ToString()).ToString("n2");
                ViewBag.LevelIncomeTR2 = double.Parse(ds.Tables[0].Compute("sum(LevelIncomeTR2)", "").ToString()).ToString("n2");
                ViewBag.GrossAmount = double.Parse(ds.Tables[0].Compute("sum(GrossAmount)", "").ToString()).ToString("n2");
                ViewBag.AdminFee = double.Parse(ds.Tables[0].Compute("sum(AdminFee)", "").ToString()).ToString("n2");
                ViewBag.TDSAmount = double.Parse(ds.Tables[0].Compute("sum(TDSAmount)", "").ToString()).ToString("n2");
                ViewBag.NetAmount = double.Parse(ds.Tables[0].Compute("sum(NetAmount)", "").ToString()).ToString("n2");
            }
            int count = 0;
            List<SelectListItem> ddlPayout = new List<SelectListItem>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[1].Rows[0]["PayoutNo"]);
                for (int i = 1; i <= count; i++)
                {
                    ddlPayout.Add(new SelectListItem { Text = "Payout-" + i, Value = i.ToString() });
                }
                ViewBag.Payout = ddlPayout;
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("PayoutDetailForAdmin")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult PayoutDetailForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.LoginId = model.LoginId == "" ? null : model.LoginId;
            model.PayoutNo = model.PayoutNo == "" ? null : model.PayoutNo;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");

            DataSet ds = model.PayoutDetail();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Fk_UserId = r["Fk_Userid"].ToString();
                    obj.LevelIncomeTR1 = r["LevelIncomeTR1"].ToString();
                    obj.LevelIncomeTR2 = r["LevelIncomeTR2"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.ClosingDate = r["ClosingDate"].ToString();
                    obj.GrossAmount = r["GrossAmount"].ToString();
                    obj.ProcessingFee = r["AdminFee"].ToString();
                    obj.TDSAmount = r["TDSAmount"].ToString();
                    obj.NetAmount = r["NetAmount"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    lst.Add(obj);
                }
                model.lstPayout = lst;
                ViewBag.LevelIncomeTR1 = double.Parse(ds.Tables[0].Compute("sum(LevelIncomeTR1)", "").ToString()).ToString("n2");
                ViewBag.LevelIncomeTR2 = double.Parse(ds.Tables[0].Compute("sum(LevelIncomeTR2)", "").ToString()).ToString("n2");
                ViewBag.GrossAmount = double.Parse(ds.Tables[0].Compute("sum(GrossAmount)", "").ToString()).ToString("n2");
                ViewBag.AdminFee = double.Parse(ds.Tables[0].Compute("sum(AdminFee)", "").ToString()).ToString("n2");
                ViewBag.TDSAmount = double.Parse(ds.Tables[0].Compute("sum(TDSAmount)", "").ToString()).ToString("n2");
                ViewBag.NetAmount = double.Parse(ds.Tables[0].Compute("sum(NetAmount)", "").ToString()).ToString("n2");
            }
            int count = 0;
            List<SelectListItem> ddlPayout = new List<SelectListItem>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[1].Rows[0]["PayoutNo"]);
                for (int i = 1; i <= count; i++)
                {
                    ddlPayout.Add(new SelectListItem { Text = "Payout-" + i, Value = i.ToString() });
                }
                ViewBag.Payout = ddlPayout;
            }
            return View(model);
        }

        public ActionResult DistributePayment()
        {
            List<Admin> lst = new List<Admin>();
            Admin model = new Admin();
            //DataSet ds = model.DistributePayment();
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow r in ds.Tables[0].Rows)
            //    {
            //        Admin obj = new Admin();
            //        obj.Pk_UserId = r["Pk_UserId"].ToString();
            //        obj.LoginId = r["LoginId"].ToString();
            //        obj.Name = r["FirstName"].ToString();
            //        obj.TPSLevelIncome = r["TPSLevelIncome"].ToString();
            //        obj.TPPLevelIncome = r["TPPLevelIncome"].ToString();
            //        obj.GrossAmount = r["GrossIncome"].ToString();
            //        obj.ProcessingFee = r["Processing"].ToString();
            //        obj.TDSAmount = r["TDS"].ToString();
            //        obj.NetAmount = r["NetIncome"].ToString();
            //        lst.Add(obj);
            //    }
            //    model.lstDistributePayment = lst;
            //    ViewBag.TPSLevelIncome = double.Parse(ds.Tables[0].Compute("sum(TPSLevelIncome)", "").ToString()).ToString("n2");
            //    ViewBag.TPPLevelIncome = double.Parse(ds.Tables[0].Compute("sum(TPPLevelIncome)", "").ToString()).ToString("n2");
            //    ViewBag.GrossIncome = double.Parse(ds.Tables[0].Compute("sum(GrossIncome)", "").ToString()).ToString("n2");
            //    ViewBag.Processing = double.Parse(ds.Tables[0].Compute("sum(Processing)", "").ToString()).ToString("n2");
            //    ViewBag.TDS = double.Parse(ds.Tables[0].Compute("sum(TDS)", "").ToString()).ToString("n2");
            //    ViewBag.NetIncome = double.Parse(ds.Tables[0].Compute("sum(NetIncome)", "").ToString()).ToString("n2");
            //}
            //model.LastClosingDate = ds.Tables[1].Rows[0]["ClosingDate"].ToString();
            //model.PayoutNo = ds.Tables[1].Rows[0]["PayoutNo"].ToString();
            return View(model);
        }

        [HttpPost]
        [ActionName("DistributePayment")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult DistributePayment(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            //Admin model = new Admin();
            //model.ClosingDate = string.IsNullOrEmpty(model.ClosingDate) ? null : Common.ConvertToSystemDate(model.ClosingDate, "dd/MM/yyyy");
            DataSet ds = model.DistributePayment();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["FirstName"].ToString();
                    obj.TPSLevelIncome = r["TPSLevelIncome"].ToString();
                    obj.TPPLevelIncome = r["TPPLevelIncome"].ToString();
                    obj.GrossAmount = r["GrossIncome"].ToString();
                    obj.ProcessingFee = r["Processing"].ToString();
                    obj.TDSAmount = r["TDS"].ToString();
                    obj.NetAmount = r["NetIncome"].ToString();
                    lst.Add(obj);
                }
                model.lstDistributePayment = lst;
                ViewBag.TPSLevelIncome = double.Parse(ds.Tables[0].Compute("sum(TPSLevelIncome)", "").ToString()).ToString("n2");
                ViewBag.TPPLevelIncome = double.Parse(ds.Tables[0].Compute("sum(TPPLevelIncome)", "").ToString()).ToString("n2");
                ViewBag.GrossIncome = double.Parse(ds.Tables[0].Compute("sum(GrossIncome)", "").ToString()).ToString("n2");
                ViewBag.Processing = double.Parse(ds.Tables[0].Compute("sum(Processing)", "").ToString()).ToString("n2");
                ViewBag.TDS = double.Parse(ds.Tables[0].Compute("sum(TDS)", "").ToString()).ToString("n2");
                ViewBag.NetIncome = double.Parse(ds.Tables[0].Compute("sum(NetIncome)", "").ToString()).ToString("n2");
            }
            model.LastClosingDate = ds.Tables[1].Rows[0]["ClosingDate"].ToString();
            model.PayoutNo = ds.Tables[1].Rows[0]["PayoutNo"].ToString();
            return View(model);
        }





        [HttpPost]
        [ActionName("DistributePayment")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult DistributePaymentSave(Admin model)
        {
            try
            {
                model.UpdatedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = model.SaveDistributePayment();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Distribute payment successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("DistributePayment", "Admin");
        }

        public ActionResult DistributePaymentTPS()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.DistributePaymentTPS();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["FirstName"].ToString();
                    obj.TPS = r["TPS"].ToString();
                    obj.GrossAmount = r["GrossIncome"].ToString();
                    obj.ProcessingFee = r["Processing"].ToString();
                    obj.TDSAmount = r["TDS"].ToString();
                    obj.NetAmount = r["NetIncome"].ToString();
                    lst.Add(obj);
                }
                model.lstDistributePaymentTPP = lst;
                ViewBag.TPS = double.Parse(ds.Tables[0].Compute("sum(TPS)", "").ToString()).ToString("n2");
                ViewBag.GrossIncome = double.Parse(ds.Tables[0].Compute("sum(GrossIncome)", "").ToString()).ToString("n2");
                ViewBag.Processing = double.Parse(ds.Tables[0].Compute("sum(Processing)", "").ToString()).ToString("n2");
                ViewBag.TDS = double.Parse(ds.Tables[0].Compute("sum(TDS)", "").ToString()).ToString("n2");
                ViewBag.NetIncome = double.Parse(ds.Tables[0].Compute("sum(NetIncome)", "").ToString()).ToString("n2");
            }
            model.LastClosingDate = ds.Tables[1].Rows[0]["ClosingDate"].ToString();
            model.PayoutNo = ds.Tables[1].Rows[0]["PayoutNo"].ToString();
            return View(model);
        }


        [HttpPost]
        [ActionName("DistributePaymentTPS")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult DistributePaymentTPS(Admin model)
        {
            //Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            //model.ClosingDate = string.IsNullOrEmpty(model.ClosingDate) ? null : Common.ConvertToSystemDate(model.ClosingDate, "dd/MM/yyyy");
            DataSet ds = model.DistributePaymentTPS();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["FirstName"].ToString();
                    obj.TPS = r["TPS"].ToString();
                    obj.GrossAmount = r["GrossIncome"].ToString();
                    obj.ProcessingFee = r["Processing"].ToString();
                    obj.TDSAmount = r["TDS"].ToString();
                    obj.NetAmount = r["NetIncome"].ToString();
                    lst.Add(obj);
                }
                model.lstDistributePaymentTPP = lst;
                ViewBag.TPS = double.Parse(ds.Tables[0].Compute("sum(TPS)", "").ToString()).ToString("n2");
                ViewBag.GrossIncome = double.Parse(ds.Tables[0].Compute("sum(GrossIncome)", "").ToString()).ToString("n2");
                ViewBag.Processing = double.Parse(ds.Tables[0].Compute("sum(Processing)", "").ToString()).ToString("n2");
                ViewBag.TDS = double.Parse(ds.Tables[0].Compute("sum(TDS)", "").ToString()).ToString("n2");
                ViewBag.NetIncome = double.Parse(ds.Tables[0].Compute("sum(NetIncome)", "").ToString()).ToString("n2");
            }
            model.LastClosingDate = ds.Tables[1].Rows[0]["ClosingDate"].ToString();
            model.PayoutNo = ds.Tables[1].Rows[0]["PayoutNo"].ToString();
            return View(model);
        }




        public ActionResult BusinessReports()
        {
            Admin model = new Admin();
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            if (model.IsDownline == "on")
            {
                model.IsDownline = "1";
            }
            else
            {
                model.IsDownline = "0";
            }
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetBusinessReports();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["FirstName"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.Date = r["Date"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.PackageType = r["PackageType"].ToString();
                    lst.Add(obj);
                }
                model.lstBReports = lst;

                ViewBag.Amount = double.Parse(ds.Tables[0].Compute("sum(Amount)", "").ToString()).ToString("n2");
                ViewBag.BV = double.Parse(ds.Tables[0].Compute("sum(BV)", "").ToString()).ToString("n2");
            }

            #region ddlPlotSize
            int count = 0;
            List<SelectListItem> ddlProductName = new List<SelectListItem>();
            DataSet dss = model.GetProductName();
            if (dss != null && dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dss.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlProductName.Add(new SelectListItem { Text = "-Select-", Value = "" });
                    }
                    ddlProductName.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["PK_ProductID"].ToString() });
                    count = count + 1;
                }
            }

            ViewBag.ddlProductName = ddlProductName;
            #endregion
            return View(model);
        }

        [HttpPost]
        [ActionName("BusinessReports")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult BusinessReports(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            if (model.IsDownline == "on")
            {
                model.IsDownline = "1";
            }
            else
            {
                model.IsDownline = "0";
            }
            model.PK_ProductID = model.PK_ProductID == "0" ? null : model.PK_ProductID;
            model.Level = model.Level == "0" ? null : model.Level;
            model.IsDownline = model.IsDownline == "0" ? null : model.IsDownline;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetBusinessReports();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["FirstName"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.BV = r["BV"].ToString();
                    obj.Date = r["Date"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.PackageType = r["PackageType"].ToString();
                    lst.Add(obj);
                }
                model.lstBReports = lst;
                ViewBag.Amount = double.Parse(ds.Tables[0].Compute("sum(Amount)", "").ToString()).ToString("n2");
                ViewBag.BV = double.Parse(ds.Tables[0].Compute("sum(BV)", "").ToString()).ToString("n2");
            }

            #region ddlPlotSize
            int count = 0;
            List<SelectListItem> ddlProductName = new List<SelectListItem>();
            DataSet dss = model.GetProductName();
            if (dss != null && dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dss.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlProductName.Add(new SelectListItem { Text = "-Select-", Value = "" });
                    }
                    ddlProductName.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["PK_ProductID"].ToString() });
                    count = count + 1;
                }
            }

            ViewBag.ddlProductName = ddlProductName;
            #endregion
            return View(model);
        }
        public ActionResult GetTPPLevelIncome(string LoginId, string ClosingDate)
        {
            Reports model = new Reports();
            List<Reports> lst = new List<Reports>();
            model.LoginId = LoginId;
            model.ClosingDate = ClosingDate;
            DataSet dspayout = model.GetTPPAmountById();
            if (dspayout != null && dspayout.Tables[0].Rows.Count > 0)
            {
                //if (dspayout.Tables[0].Rows[0]["MSG"].ToString() == "1")
                //{
                model.Result = "yes";
                if (dspayout != null && dspayout.Tables.Count > 0 && dspayout.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dspayout.Tables[0].Rows)
                    {
                        Reports obj = new Reports();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.Amount = r["Amount"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.CurrentDate = r["CurrentDate"].ToString();
                        obj.UsedFor = r["UsedFor"].ToString();
                        lst.Add(obj);
                    }
                    model.lsttopupreport = lst;
                    //}
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTPSLevelIncome(string LoginId, string ClosingDate)
        {
            Reports model = new Reports();
            List<Reports> lst = new List<Reports>();
            model.LoginId = LoginId;
            model.ClosingDate = ClosingDate;
            DataSet dspayout = model.GetTPSAmountById();
            if (dspayout != null && dspayout.Tables[0].Rows.Count > 0)
            {
                //if (dspayout.Tables[0].Rows[0]["MSG"].ToString() == "1")
                //{
                model.Result = "yes";
                if (dspayout != null && dspayout.Tables.Count > 0 && dspayout.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dspayout.Tables[0].Rows)
                    {
                        Reports obj = new Reports();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.Amount = r["Amount"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.CurrentDate = r["CurrentDate"].ToString();
                        obj.UsedFor = r["UsedFor"].ToString();
                        lst.Add(obj);
                    }
                    model.lsttopupreporttps = lst;
                    //}
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPayoutReportforAmount(string Fk_UserId, string PayoutNo)
        {
            Reports model = new Reports();
            List<Reports> lst = new List<Reports>();
            model.Fk_UserId = Fk_UserId;
            model.PayoutNo = PayoutNo;
            DataSet dspayout = model.GetPaidPayoutDetailsByAmount();
            if (dspayout != null && dspayout.Tables[0].Rows.Count > 0)
            {
                //if (dspayout.Tables[0].Rows[0]["MSG"].ToString() == "1")
                //{
                model.Result = "yes";
                if (dspayout != null && dspayout.Tables.Count > 0 && dspayout.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dspayout.Tables[0].Rows)
                    {
                        Reports obj = new Reports();
                        obj.PayoutNo = r["PayoutNo"].ToString();
                        obj.CurrentDate = r["CurrentDate"].ToString();
                        obj.LoginId = r["FromId"].ToString();
                        obj.Name = r["FromName"].ToString();
                        obj.Amount = r["Income"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.Fk_Paymentid = r["Paymenttype"].ToString();
                        lst.Add(obj);
                    }
                    model.lsttopupreport = lst;
                    //}
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PayoutRequestList()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            model.Status = "Pending";
            DataSet ds = model.GetPayoutRequest();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_RequestID = r["Pk_RequestId"].ToString();
                    obj.Amount = r["AMount"].ToString();
                    obj.Date = r["RequestedDate"].ToString();
                    //obj.IFSCCode = r["IFSCCode"].ToString();
                    //obj.MemberAccNo = r["MemberAccNo"].ToString();
                    //obj.BankBranch = r["MemberBranch"].ToString();
                    //obj.BankName = r["MemberBankName"].ToString();
                    //obj.UPIID = r["UPIId"].ToString();
                    obj.PaymentMode = "IFSC Code - " + r["IFSCCode"].ToString() + ",AccNo- " + r["MemberAccNo"].ToString() + ",Branch- " + r["MemberBranch"].ToString() + ",Bank Name- " + r["MemberBankName"].ToString() + ",UPI Id- " + r["UPIId"].ToString();

                    obj.Status = r["Status"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.TransactionNo = r["TransactionNo"].ToString();
                    obj.GrossAmount = r["GrossAmount"].ToString();
                    obj.ProcessingFee = r["DeductionCharges"].ToString();
                    obj.TransactionDate = r["ApprovalDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("PayoutRequestList")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult PayoutRequestListBy(Admin model)
        {
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetPayoutRequest();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_RequestID = r["Pk_RequestId"].ToString();
                    obj.Amount = r["AMount"].ToString();
                    obj.Date = r["RequestedDate"].ToString();
                    //obj.IFSCCode = r["IFSCCode"].ToString();
                    //obj.UPIID = r["UPIId"].ToString();
                    //obj.MemberAccNo = r["MemberAccNo"].ToString();

                    obj.PaymentMode = "IFSC Code - " + r["IFSCCode"].ToString() + ",AccNo- " + r["MemberAccNo"].ToString() + ",Branch- " + r["MemberBranch"].ToString() + ",Bank Name- " + r["MemberBankName"].ToString() + ",UPI Id- " + r["UPIId"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.TransactionNo = r["TransactionNo"].ToString();
                    obj.GrossAmount = r["GrossAmount"].ToString();
                    obj.ProcessingFee = r["DeductionCharges"].ToString();
                    obj.TransactionDate = r["ApprovalDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        public ActionResult ApprovePayout(string id)
        {
            try
            {
                Admin model = new Admin();
                model.PK_RequestID = id;
                model.Status = (model.Status = "Approved");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ApprovePayoutRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Transfer to account approved Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("PayoutRequestList", "Admin");
        }

        public ActionResult DeclinePayout(string id)
        {
            try
            {
                Admin model = new Admin();
                model.PK_RequestID = id;
                model.Status = (model.Status = "Declined");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ApprovePayoutRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Transfer to account Declined Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("PayoutRequestList", "Admin");
        }

        [HttpPost]
        public ActionResult GetNameDetails(string LoginId)
        {
            Admin model = new Admin();
            model.LoginId = LoginId;
            DataSet ds = model.GetNameDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    model.Result = "no";
                }
                else
                {
                    model.Result = "yes";
                    model.Fk_UserId = ds.Tables[0].Rows[0]["PK_UserId"].ToString();
                    model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    model.Amount = ds.Tables[1].Rows[0]["Balance"].ToString();
                }
            }
            else
            {
                model.Result = "no";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferWallet()
        {
            return View();
        }
        [HttpPost]
        [ActionName("TransferWallet")]
        [OnAction(ButtonName = "Transfer")]
        public ActionResult TransferWallet(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.SaveTransferWallet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["TransferWallet"] = "Transferred  successfully";
                    }
                    else
                    {
                        TempData["TransferWallet"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["TransferWallet"] = ex.Message;
            }
            return RedirectToAction("TransferWallet", "Admin");
        }

        public ActionResult AdvanceDeduction()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetAdvanceDeductionReports();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Pk_AdvanceId = r["Pk_AdvanceId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.DeductionType = r["Type"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstdeduction = lst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("AdvanceDeduction")]
        [OnAction(ButtonName = "Advance")]
        public ActionResult AdvanceDeduction(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                model.TransactionDate = string.IsNullOrEmpty(model.TransactionDate) ? null : Common.ConvertToSystemDate(model.TransactionDate, "dd/MM/yyyy");
                DataSet ds = model.SaveDeduction();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["deduction"] = "Advance/Deduction done successfully";
                    }
                    else
                    {
                        TempData["deduction"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["deduction"] = ex.Message;
            }
            return RedirectToAction("AdvanceDeduction", "Admin");
        }
        public ActionResult AdvanceDeductionList()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetAdvanceDeductionReports();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Pk_AdvanceId = r["Pk_AdvanceId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.DeductionType = r["Type"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    lst.Add(obj);
                }
                model.lstdeduction = lst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("AdvanceDeductionList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult AdvanceDeductionList(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.LoginId = model.LoginId == "" ? null : model.LoginId;
            model.DeductionType = model.DeductionType == "" ? null : model.DeductionType;
            DataSet ds = model.GetAdvanceDeductionReports();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Pk_AdvanceId = r["Pk_AdvanceId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.DeductionType = r["Type"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    lst.Add(obj);
                }
                model.lstdeduction = lst;
            }
            return View(model);
        }
        public ActionResult TopUp()
        {
            Account model = new Account();
            #region Product Bind
            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProductForTopUp();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                ViewBag.FromAmount = ds1.Tables[0].Rows[0]["FromAmount"].ToString();
                ViewBag.ToAmount = ds1.Tables[0].Rows[0]["ToAmount"].ToString();
                ViewBag.InMultipleOf = ds1.Tables[0].Rows[0]["InMultipleOf"].ToString();
                ViewBag.ROIPercent = ds1.Tables[0].Rows[0]["ROIPercent"].ToString();
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

            DataSet ds = model.GetTotalWalletAmount();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.TotalWalletAmount = ds.Tables[0].Rows[0]["TotalWalletAmount"].ToString();
            }

            #region ddlpaymentType
            List<SelectListItem> ddlpaymentType = Common.BindPaymentType();
            ViewBag.ddlpaymentType = ddlpaymentType;
            #endregion
            #region ddlpaymentmode
            UserWallet obj = new UserWallet();
            int count1 = 0;
            List<SelectListItem> ddlpaymentmode = new List<SelectListItem>();
            DataSet ds2 = obj.GetPaymentMode();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlpaymentmode.Add(new SelectListItem { Text = "Select Payment Mode", Value = "" });
                    }
                    ddlpaymentmode.Add(new SelectListItem { Text = r["PaymentMode"].ToString(), Value = r["PK_paymentID"].ToString() });
                    count1 = count1 + 1;
                }
            }

            ViewBag.ddlpaymentmode = ddlpaymentmode;

            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult TopUp(Account obj)
        {
            try
            {
                obj.AddedBy = Session["Pk_AdminId"].ToString();
                obj.FK_UserId = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.TopUpByAdmin();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Top-Up Done successfully";
                    }
                    else
                    {
                        TempData["error"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else { }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("Topup", "Admin");
        }
        public ActionResult GetPackageDetails(string PackageId)
        {
            Master obj = new Master();
            try
            {
                obj.Packageid = PackageId;
                DataSet ds = obj.ProductList();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    obj.Result = "yes";
                    obj.FromAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["FromAmount"]);
                    obj.ToAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["ToAmount"]);
                    obj.InMultipleOf = Convert.ToDecimal(ds.Tables[0].Rows[0]["InMultipleOf"]);
                }
                else { }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [OnAction(ButtonName = "btnApprove")]
        [ActionName("PayoutRequestList")]
        public ActionResult ApprovePayoutRequest(Admin model)
        {
            try
            {
                string hdrows = Request["hdRows"].ToString();
                string[] result = Request["hdRows"].ToString().Split(',');
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                string chkselect = "";
                int i = 0;
                foreach (String str in result)
                {
                    try
                    {
                        chkselect = Request["chkSelect_ " + str];
                        if (chkselect == "on")
                        {
                            model.PK_RequestID = Request["PK_RequestID_ " + str].ToString();
                            model.Status = "Approved";

                            model.TransactionDate = string.IsNullOrEmpty(model.TransactionDate) ? null : Common.ConvertToSystemDate(model.TransactionDate, "dd/MM/yyyy");
                            DataSet ds = model.ApprovePayoutRequest();
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                                {
                                    TempData["msg"] = "Approved Successfully";
                                }
                                else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                                {
                                    TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                                }
                            }
                        }
                    }
                    catch { chkselect = "0"; }
                    i++;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;

            }
            return RedirectToAction("PayoutRequestList");
        }


        [HttpPost]
        [OnAction(ButtonName = "btnDecline")]
        [ActionName("PayoutRequestList")]
        public ActionResult DeclinePayoutRequest(Admin model)
        {
            try
            {
                string hdrows = Request["hdRows"].ToString();
                string[] result = Request["hdRows"].ToString().Split(',');
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                string chkselect = "";
                int i = 0;
                foreach (String str in result)
                {
                    try
                    {
                        chkselect = Request["chkSelect_ " + str];
                        if (chkselect == "on")
                        {
                            model.PK_RequestID = Request["PK_RequestID_ " + str].ToString();
                            model.Status = "Declined";
                            model.TransactionDate = string.IsNullOrEmpty(model.TransactionDate) ? null : Common.ConvertToSystemDate(model.TransactionDate, "dd/MM/yyyy");
                            DataSet ds = model.DeclinePayoutRequest();
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                                {
                                    TempData["msg"] = "Declined Successfully";
                                }
                                else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                                {
                                    TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                                }
                            }
                        }
                    }
                    catch { chkselect = "0"; }
                    i++;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;

            }
            return RedirectToAction("PayoutRequestList");
        }

        public ActionResult KYCUpdateDeatilsOfUser()
        {
            Admin model = new Admin();
            try
            {
                model.IsVerified = "0";
                List<Admin> lst = new List<Admin>();
                DataSet ds = model.GetKYCUpdateDetailsOfUser();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Admin obj = new Admin();
                        obj.Fk_UserId = r["PK_UserId"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Mobile = r["Mobile"].ToString();
                        obj.Email = r["Email"].ToString();
                        obj.AdharNo = r["AdharNumber"].ToString();
                        obj.IsVerified = r["IsVerified"].ToString();
                        obj.PanNo = r["PanNumber"].ToString();
                        obj.MemberAccNo = r["MemberAccNo"].ToString();
                        obj.IFSCCode = r["IFSCCode"].ToString();
                        obj.BankName = r["MemberBankName"].ToString();
                        obj.Status = r["PanStatus"].ToString();
                        obj.BankBranch = r["MemberBranch"].ToString();
                        obj.NomineeName = r["NomineeName"].ToString();
                        obj.NomineeAge = r["NomineeAge"].ToString();
                        obj.NomineeRelation = r["NomineeRelation"].ToString();
                        obj.UPIID = r["UPIID"].ToString();
                        obj.PanImage = r["PanImage"].ToString();
                        lst.Add(obj);
                    }
                    model.lstKycUpdate = lst;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        [OnAction(ButtonName = "Search")]
        [ActionName("KYCUpdateDeatilsOfUser")]
        public ActionResult KYCUpdateDeatilsOfUser(Admin model)
        {
            try
            {
                List<Admin> lst = new List<Admin>();
                DataSet ds = model.GetKYCUpdateDetailsOfUser();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Admin obj = new Admin();
                        obj.Fk_UserId = r["PK_UserId"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Mobile = r["Mobile"].ToString();
                        obj.Email = r["Email"].ToString();
                        obj.AdharNo = r["AdharNumber"].ToString();
                        //obj.IsVerified = Convert.ToBoolean(r["IsVerified"]).ToString();
                        obj.IsVerified = r["IsVerified"].ToString();
                        obj.PanNo = r["PanNumber"].ToString();
                        obj.MemberAccNo = r["MemberAccNo"].ToString();
                        obj.Status = r["PanStatus"].ToString();
                        obj.IFSCCode = r["IFSCCode"].ToString();
                        obj.BankName = r["MemberBankName"].ToString();
                        obj.BankBranch = r["MemberBranch"].ToString();
                        obj.NomineeName = r["NomineeName"].ToString();
                        obj.NomineeAge = r["NomineeAge"].ToString();
                        obj.NomineeRelation = r["NomineeRelation"].ToString();
                        obj.UPIID = r["UPIID"].ToString();
                        obj.PanImage = r["PanImage"].ToString();
                        lst.Add(obj);
                    }
                    model.lstKycUpdate = lst;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(model);
        }
        public ActionResult TPSListForDistributePaymentNew()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.ListForDistributePaymentTPS();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["FirstName"].ToString();
                    obj.TPS = r["TPS"].ToString();
                    obj.GrossAmount = r["GrossIncome"].ToString();
                    obj.ProcessingFee = r["Processing"].ToString();
                    obj.TDSAmount = r["TDS"].ToString();
                    obj.NetAmount = r["NetIncome"].ToString();
                    lst.Add(obj);
                }
                model.lstDistributePaymentTPP = lst;
                ViewBag.TPS = double.Parse(ds.Tables[0].Compute("sum(TPS)", "").ToString()).ToString("n2");
                ViewBag.GrossIncome = double.Parse(ds.Tables[0].Compute("sum(GrossIncome)", "").ToString()).ToString("n2");
                ViewBag.Processing = double.Parse(ds.Tables[0].Compute("sum(Processing)", "").ToString()).ToString("n2");
                ViewBag.TDS = double.Parse(ds.Tables[0].Compute("sum(TDS)", "").ToString()).ToString("n2");
                ViewBag.NetIncome = double.Parse(ds.Tables[0].Compute("sum(NetIncome)", "").ToString()).ToString("n2");
            }
            model.LastClosingDate = ds.Tables[1].Rows[0]["ClosingDate"].ToString();
            model.PayoutNo = ds.Tables[1].Rows[0]["PayoutNo"].ToString();
            return View(model);
        }
        [HttpPost]
        [ActionName("TPSListForDistributePaymentNew")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult TPSListForDistributePaymentNew(Admin model)
        {
            //Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            //model.ClosingDate = string.IsNullOrEmpty(model.ClosingDate) ? null : Common.ConvertToSystemDate(model.ClosingDate, "dd/MM/yyyy");
            DataSet ds = model.ListForDistributePaymentTPS();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["FirstName"].ToString();
                    obj.TPS = r["TPS"].ToString();
                    obj.GrossAmount = r["GrossIncome"].ToString();
                    obj.ProcessingFee = r["Processing"].ToString();
                    obj.TDSAmount = r["TDS"].ToString();
                    obj.NetAmount = r["NetIncome"].ToString();
                    lst.Add(obj);
                }
                model.lstDistributePaymentTPP = lst;
                ViewBag.TPS = double.Parse(ds.Tables[0].Compute("sum(TPS)", "").ToString()).ToString("n2");
                ViewBag.GrossIncome = double.Parse(ds.Tables[0].Compute("sum(GrossIncome)", "").ToString()).ToString("n2");
                ViewBag.Processing = double.Parse(ds.Tables[0].Compute("sum(Processing)", "").ToString()).ToString("n2");
                ViewBag.TDS = double.Parse(ds.Tables[0].Compute("sum(TDS)", "").ToString()).ToString("n2");
                ViewBag.NetIncome = double.Parse(ds.Tables[0].Compute("sum(NetIncome)", "").ToString()).ToString("n2");
            }
            model.LastClosingDate = ds.Tables[1].Rows[0]["ClosingDate"].ToString();
            model.PayoutNo = ds.Tables[1].Rows[0]["PayoutNo"].ToString();
            return View(model);
        }
        [HttpPost]
        [ActionName("TPSListForDistributePaymentNew")]
        [OnAction(ButtonName = "btnDistribute")]
        public ActionResult DistributePaymentTPSSave(Admin model)
        {
            try
            {
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.SaveDistributePaymentTPS();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "TPS payment distributed successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("TPSListForDistributePaymentNew", "Admin");
        }
        public ActionResult ViewLedger(string Fk_UserId)
        {
            Admin model = new Admin();
            model.Fk_UserId = Fk_UserId;
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetWalletLedgerDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                ViewBag.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Pk_EwalletId = r["Pk_EwalletId"].ToString();
                    obj.UserId = r["FK_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Balance = r["Balance"].ToString();
                    obj.TransactionBy = r["TransactionBy"].ToString();
                    obj.TransactionNo = r["TransactionNo"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                ViewBag.CrAmount = double.Parse(ds.Tables[0].Compute("sum(CrAmount)", "").ToString()).ToString("n2");
                ViewBag.DrAmount = double.Parse(ds.Tables[0].Compute("sum(DrAmount)", "").ToString()).ToString("n2");
                model.lstViewLedger = lst;
            }
            return View(model);
        }

        public ActionResult PaidIncomeForAdmin(string PayoutNo, string LoginId)
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            model.LoginId = LoginId;
            model.PayoutNo = PayoutNo;
            DataSet ds = model.PaidIncome();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
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

        public ActionResult UnUsedPinList()
        {
            Admin obj = new Admin();

            List<Admin> lst = new List<Admin>();
            DataSet ds = obj.GetUnusedUsedPinsForAdmin();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    Objload.Fk_UserId = dr["FK_UserId"].ToString();
                    Objload.LoginId = dr["LoginId"].ToString();
                    Objload.Name = dr["Name"].ToString();
                    Objload.PackageName = dr["ProductName"].ToString();
                    Objload.Fk_ProductId = dr["FK_ProductId"].ToString();
                    Objload.TotalPin = dr["TotalPins"].ToString();
                    Objload.UsedPin = dr["UsedPins"].ToString();
                    Objload.AvailablePin = dr["AvaliablePins"].ToString();
                    Objload.TransferPin = dr["TransferPins"].ToString();
                    lst.Add(Objload);
                }
                obj.lstView = lst;
            }
            #region Product Bind

            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProductForJoining();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlProduct.Add(new SelectListItem { Text = "Select", Value = "" });
                    }
                    ddlProduct.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["Pk_ProductId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlProduct = ddlProduct;
            #endregion

            return View(obj);
        }

        [HttpPost]
        [ActionName("UnUsedPinList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult UnUsedPinList(Admin obj)
        {
            List<Admin> lst = new List<Admin>();
            obj.LoginId = obj.LoginId == "" ? null : obj.LoginId;
            obj.Package = obj.Package == "" ? null : obj.Package;
            DataSet ds = obj.GetUnusedUsedPinsForAdmin();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    Objload.Fk_UserId = dr["FK_UserId"].ToString();
                    Objload.LoginId = dr["LoginId"].ToString();
                    Objload.Name = dr["Name"].ToString();
                    Objload.PackageName = dr["ProductName"].ToString();
                    Objload.Package = dr["FK_ProductId"].ToString();
                    Objload.TotalPin = dr["TotalPins"].ToString();
                    Objload.UsedPin = dr["UsedPins"].ToString();
                    Objload.AvailablePin = dr["AvaliablePins"].ToString();
                    Objload.TransferPin = dr["TransferPins"].ToString();
                    lst.Add(Objload);
                }
                obj.lstView = lst;
            }
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
                        ddlProduct.Add(new SelectListItem { Text = "Select", Value = "" });
                    }
                    ddlProduct.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["Pk_ProductId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlProduct = ddlProduct;
            #endregion
            return View(obj);
        }

        public ActionResult BannerImageUpload()
        {
            List<Admin> lst = new List<Admin>();
            Admin obj = new Admin();
            DataSet ds = obj.GetBannerImage();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin model = new Admin();
                    model.PK_BannerId = r["PK_BannerId"].ToString();
                    model.BannerImage = r["BannerImage"].ToString();
                    lst.Add(model);
                }
                obj.lst = lst;
            }
            return View(obj);
        }
        [HttpPost]
        [OnAction(ButtonName = "btnsave")]
        [ActionName("BannerImageUpload")]
        public ActionResult BannerImageUpload(Admin model, HttpPostedFileBase BannerImage)
        {
            try
            {
                if (BannerImage != null)
                {

                    model.BannerImage = "/BannerImage/" + Guid.NewGuid() + Path.GetExtension(BannerImage.FileName);
                    BannerImage.SaveAs(Path.Combine(Server.MapPath(model.BannerImage)));
                }
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.SaveBannerImage();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["BannerImage"] = "Banner image upload successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                    {
                        TempData["BannerImage"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["BannerImage"] = ex.Message;
            }
            return RedirectToAction("BannerImageUpload", "Admin");
        }
        public ActionResult DeleteBanner(string id)
        {
            Admin obj = new Admin();
            obj.PK_BannerId = id;
            obj.AddedBy = Session["Pk_adminId"].ToString();
            DataSet ds = obj.DeleteBanner();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                {
                    TempData["msg"] = "Banner Deleted Successfully";
                }
                else
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            return RedirectToAction("BannerImageUpload", "Admin");
        }
        public ActionResult SetMenuPermissionForUser()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetFormMasterList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.FormId = r["PK_FormId"].ToString();
                    obj.FormName = r["FormName"].ToString();
                    obj.Permission = r["Permission"].ToString();

                    lst.Add(obj);
                }
                model.lstForUserPermission = lst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("SetMenuPermissionForUser")]
        [OnAction(ButtonName = "btnSubmit")]
        public ActionResult InActiveUser(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.InActiveUser();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "User in-activate successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("SetMenuPermissionForUser", "Admin");
        }
        public ActionResult ActiveUser(string id)
        {
            try
            {
                Admin model = new Admin();
                model.FormId = id;
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ActiveUser();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "User activated successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("SetMenuPermissionForUser", "Admin");
        }
        public ActionResult CreateTransaction()
        {
            Admin model = new Admin();
            List<SelectListItem> lst = new List<SelectListItem>();
            ViewBag.Wallet = Common.BindAllWallet();
            DataSet ds = model.GetAdvanceDeductionReports();

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateTransaction(Admin model)
        {
            try
            {
                List<SelectListItem> lst = new List<SelectListItem>();
                ViewBag.Wallet = Common.BindAllWallet();
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.CreateTransaction();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Transaction created successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("CreateTransaction", "Admin");
        }
        public ActionResult DistributedTPSList(string PayoutNo, string LoginId, string Fk_InvestmentId)
        {
            List<Admin> lst = new List<Admin>();
            Admin model = new Admin();
            model.PayoutNo = PayoutNo;
            model.LoginId = LoginId;
            model.Fk_InvestmentId = Fk_InvestmentId;
            DataSet ds = model.GetDistributedTPSList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
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
                model.lstTps = lst;
            }
            int count = 0;
            List<SelectListItem> ddlPayout = new List<SelectListItem>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[1].Rows[0]["PayoutNo"]);
                for (int i = 1; i <= count; i++)
                {
                    ddlPayout.Add(new SelectListItem { Text = "Payout-" + i, Value = i.ToString() });
                }
                ViewBag.Payout = ddlPayout;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("DistributedTPSList")]
        [OnAction(ButtonName = "Search")]
        public ActionResult DistributedTPSList(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetDistributedTPSList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
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
                model.lstTps = lst;
            }
            return View(model);
        }
        public ActionResult ViewGenerateEpinDetails(string Fk_UserId,string Fk_ProductId)
        {
            Admin obj = new Admin();
            List<Admin> lst = new List<Admin>();
            obj.Fk_UserId = Fk_UserId;
            obj.Fk_ProductId = Fk_ProductId;
            obj.ePinNo = obj.ePinNo == "" ? null : obj.ePinNo;
            obj.Status = "T";
            DataSet ds = obj.GetGeneratedEpinDetails();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    obj.Fk_UserId = dr["Fk_UserId"].ToString();
                    obj.LoginId = dr["LoginId"].ToString();
                    obj.Name = dr["Name"].ToString();
                    Objload.PinAmount = dr["PinAmount"].ToString();
                    Objload.GST = dr["GST"].ToString();
                    Objload.TotalAmount = dr["TotalAmount"].ToString();
                    Objload.Package = dr["Package"].ToString();
                    Objload.ProductPrice = dr["ProductPrice"].ToString();
                    Objload.GenerateVia = dr["GenerateVia"].ToString();
                    Objload.ePinNo = dr["ePinNo"].ToString();
                    Objload.RegisteredTo = dr["RegisteredTo"].ToString();
                    Objload.PinUser = dr["PinUser"].ToString();
                    Objload.UsedFor = dr["UsedFor"].ToString();
                    Objload.UsedDate = dr["UsedDate"].ToString();
                    Objload.AddedOn = dr["CreatedDate"].ToString();
                    Objload.AddedBy = dr["CreatedBy"].ToString();
                    Objload.TransactionNo = dr["TransactionNo"].ToString();
                    Objload.TransactionDate = dr["TransactionDate"].ToString();
                    Objload.BankName = dr["BankName"].ToString();
                    Objload.BankBranch = dr["BranchName"].ToString();
                    Objload.PinStaus = dr["PinStaus"].ToString();
                    Objload.PaymentMode = dr["PaymentMode"].ToString();
                    Objload.ToName = dr["ToName"].ToString();
                    Objload.ToId = dr["ToId"].ToString();
                    Objload.TransferDate = dr["TransferDate"].ToString();
                    lst.Add(Objload);
                }
                obj.lstgeneratepin = lst;
            }
            return View(obj);
        }
        [HttpPost]
        [ActionName("ViewGenerateEpinDetails")]
        [OnAction(ButtonName = "Search")]
        public ActionResult ViewGenerateEpinDetails(Admin obj)
        {
            List<Admin> lst = new List<Admin>();
            obj.ePinNo = obj.ePinNo == "" ? null : obj.ePinNo;
            obj.Status = obj.Status == "" ? null : obj.Status;
            DataSet ds = obj.GetGeneratedEpinDetails();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    obj.Fk_UserId = dr["Fk_UserId"].ToString();
                    obj.LoginId = dr["LoginId"].ToString();
                    obj.Name = dr["Name"].ToString();
                    Objload.PinAmount = dr["PinAmount"].ToString();
                    Objload.GST = dr["GST"].ToString();
                    Objload.TotalAmount = dr["TotalAmount"].ToString();
                    Objload.Package = dr["Package"].ToString();
                    Objload.ProductPrice = dr["ProductPrice"].ToString();
                    Objload.GenerateVia = dr["GenerateVia"].ToString();
                    Objload.ePinNo = dr["ePinNo"].ToString();
                    Objload.RegisteredTo = dr["RegisteredTo"].ToString();
                    Objload.PinUser = dr["PinUser"].ToString();
                    Objload.UsedFor = dr["UsedFor"].ToString();
                    Objload.UsedDate = dr["UsedDate"].ToString();
                    Objload.AddedOn = dr["CreatedDate"].ToString();
                    Objload.AddedBy = dr["CreatedBy"].ToString();
                    Objload.TransactionNo = dr["TransactionNo"].ToString();
                    Objload.TransactionDate = dr["TransactionDate"].ToString();
                    Objload.BankName = dr["BankName"].ToString();
                    Objload.BankBranch = dr["BranchName"].ToString();
                    Objload.PinStaus = dr["PinStaus"].ToString();
                    Objload.PaymentMode = dr["PaymentMode"].ToString();
                    Objload.ToName = dr["ToName"].ToString();
                    Objload.ToId = dr["ToId"].ToString();
                    Objload.TransferDate = dr["TransferDate"].ToString();
                    lst.Add(Objload);
                }
                obj.lstgeneratepin = lst;
            }
            return View(obj);
        }

        public ActionResult WalletLedger()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.WalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
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
        public ActionResult WalletLedger(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.WalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
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


        #region
        public ActionResult DistributedTPSDetails()
        {
          
            List<Admin> lst = new List<Admin>();
            Admin model = new Admin();
        
            DataSet ds = model.TPSPayoutDetail();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Fk_UserId = r["PK_UserId"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.TopUpAmount = r["TopUpAmount"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.CommissionPercentage = r["CommissionPercentage"].ToString();
                    obj.Fk_InvestmentId = r["Fk_InvestmentId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    lst.Add(obj);
                }
                model.lstPayout = lst;
                ViewBag.BusinessAmount = double.Parse(ds.Tables[0].Compute("sum(BusinessAmount)", "").ToString()).ToString("n2");
                ViewBag.Amount = double.Parse(ds.Tables[0].Compute("sum(Amount)", "").ToString()).ToString("n2");
               
            }
            int count = 0;
            List<SelectListItem> ddlPayout = new List<SelectListItem>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[1].Rows[0]["PayoutNo"]);
                for (int i = 1; i <= count; i++)
                {
                    ddlPayout.Add(new SelectListItem { Text = "Payout-" + i, Value = i.ToString() });
                }
                ViewBag.Payout = ddlPayout;
            }
            return View(model);

        }
        [HttpPost]
        [ActionName("DistributedTPSDetails")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult DistributedTPSDetailList(Admin model)
        {

            List<Admin> lst = new List<Admin>();
          
            DataSet ds = model.TPSPayoutDetail();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.Fk_UserId = r["PK_UserId"].ToString();
                    obj.PayoutNo = r["PayoutNo"].ToString();
                    obj.TopUpAmount = r["TopUpAmount"].ToString();
                    obj.BusinessAmount = r["BusinessAmount"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.CommissionPercentage = r["CommissionPercentage"].ToString();
                    obj.Fk_InvestmentId = r["Fk_InvestmentId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    lst.Add(obj);
                }
                model.lstPayout = lst;
                ViewBag.BusinessAmount = double.Parse(ds.Tables[0].Compute("sum(BusinessAmount)", "").ToString()).ToString("n2");
                ViewBag.Amount = double.Parse(ds.Tables[0].Compute("sum(Amount)", "").ToString()).ToString("n2");

            }
            int count = 0;
            List<SelectListItem> ddlPayout = new List<SelectListItem>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[1].Rows[0]["PayoutNo"]);
                for (int i = 1; i <= count; i++)
                {
                    ddlPayout.Add(new SelectListItem { Text = "Payout-" + i, Value = i.ToString() });
                }
                ViewBag.Payout = ddlPayout;
            }
            return View(model);


        }
        #endregion
    }
}