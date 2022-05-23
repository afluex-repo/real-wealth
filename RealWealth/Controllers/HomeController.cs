using RealWealth.Filter;
using RealWealth.Models;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace RealWealth.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //return Redirect("~/RealWealthWebsite/index.html");
            return View();
        }
        public ActionResult GetBanner()
        {
            Home model = new Home();
            DataSet ds = model.GetBannerImageList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Result = "yes";
                model.BannerImage = "https://RealWealth.co.in/" + ds.Tables[0].Rows[0]["BannerImage"].ToString();
            }
            else
            {
                model.Result = "no";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            Session.Abandon();
            return View();
        }
        public ActionResult LoginAction(Home obj)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                Home Modal = new Home();
                DataSet ds = obj.Login();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        if ((ds.Tables[0].Rows[0]["UserType"].ToString() == "Associate"))
                        {
                            if (obj.Password == Crypto.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString()))
                            {
                                Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                                Session["Pk_UserId"] = ds.Tables[0].Rows[0]["Pk_userId"].ToString();
                                Session["UserType"] = ds.Tables[0].Rows[0]["UserType"].ToString();
                                Session["FullName"] = ds.Tables[0].Rows[0]["FullName"].ToString();
                                Session["Password"] = ds.Tables[0].Rows[0]["Password"].ToString();
                                Session["TransPassword"] = ds.Tables[0].Rows[0]["TransPassword"].ToString();
                                Session["Profile"] = ds.Tables[0].Rows[0]["Profile"].ToString();
                                Session["Gender"] = ds.Tables[0].Rows[0]["Sex"].ToString();
                                Session["Branch"] = ds.Tables[0].Rows[0]["MemberBranch"].ToString();
                                Session["Email"] = ds.Tables[0].Rows[0]["Email"].ToString();
                                Session["Contact"] = ds.Tables[0].Rows[0]["Mobile"].ToString();
                                Session["Bank"] = ds.Tables[0].Rows[0]["MemberBankName"].ToString();
                                Session["Status"] = ds.Tables[0].Rows[0]["Status"].ToString();
                                if (ds.Tables[0].Rows[0]["TeamPermanent"].ToString() == "O" || ds.Tables[0].Rows[0]["TeamPermanent"].ToString() == "P")
                                {
                                    Session["IdActivated"] = true;
                                    FormName = "UserDashBoard";
                                    Controller = "User";
                                }
                                else
                                {
                                    Session["IdActivated"] = false;
                                    FormName = "CompleteRegistration";
                                    Controller = "Home";
                                }
                            }
                            else
                            {
                                TempData["Login"] = "Incorrect Password";
                                FormName = "Login";
                                Controller = "Home";
                            }
                        }
                        else if (ds.Tables[0].Rows[0]["UserType"].ToString() == "Admin")
                        {
                            Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                            Session["Pk_AdminId"] = ds.Tables[0].Rows[0]["Pk_adminId"].ToString();
                            Session["UsertypeName"] = ds.Tables[0].Rows[0]["UsertypeName"].ToString();
                            Session["Name"] = ds.Tables[0].Rows[0]["Name"].ToString();

                            if (ds.Tables[0].Rows[0]["isFranchiseAdmin"].ToString() == "True")
                            {
                                Session["FranchiseAdminID"] = ds.Tables[0].Rows[0]["Pk_adminId"].ToString();
                                FormName = "Registration";
                                Controller = "FranchiseAdmin";
                            }
                            else
                            {
                                FormName = "AdminDashBoard";
                                Controller = "Admin";
                            }
                        }
                        else if (ds.Tables[0].Rows[0]["UserType"].ToString() == "Back Office")
                        {
                            Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                            Session["Pk_AdminId"] = ds.Tables[0].Rows[0]["Pk_adminId"].ToString();
                            Session["UsertypeName"] = ds.Tables[0].Rows[0]["UsertypeName"].ToString();
                            Session["Name"] = ds.Tables[0].Rows[0]["Name"].ToString();

                            if (ds.Tables[0].Rows[0]["isFranchiseAdmin"].ToString() == "True")
                            {
                                Session["FranchiseAdminID"] = ds.Tables[0].Rows[0]["Pk_adminId"].ToString();
                                FormName = "Registration";
                                Controller = "FranchiseAdmin";
                            }
                            else
                            {
                                FormName = "AdminDashBoard";
                                Controller = "Admin";
                            }
                        }
                        else
                        {
                            TempData["Login"] = "Incorrect LoginId Or Password";
                            FormName = "Login";
                            Controller = "Home";
                        }
                    }
                    else
                    {
                        TempData["Login"] = "Incorrect LoginId Or Password";
                        FormName = "Login";
                        Controller = "Home";
                    }
                }
                else
                {
                    TempData["Login"] = "Incorrect LoginId Or Password";
                    FormName = "Login";
                    Controller = "Home";
                }
            }
            catch (Exception ex)
            {
                TempData["Login"] = ex.Message;
                FormName = "Login";
                Controller = "Home";
            }
            return RedirectToAction(FormName, Controller);
            //return View();
        }
        public ActionResult Registration(string PId)
        {
            Home obj = new Home();
            List<SelectListItem> Gender = Common.BindGender();
            ViewBag.Gender = Gender;
            if (!string.IsNullOrEmpty(PId))
            {
                obj.Fk_UserId = PId;
                // var d = Crypto.Decrypt(PId);
                DataSet ds = obj.GetMemberNameWithUserId();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.SponsorId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                }
                // ViewBag.SponsorId = d.Split('|')[0];
                //ViewBag.Leg = d.Split('|')[1];
            }
            return View();
        }

        public ActionResult RegistrationAction(string SponsorId, string FirstName, string LastName, string MobileNo, string PinCode, string Leg, string Password, string Email, string Gender, string State, string City)
        {
            Home obj = new Home();
            try
            {
                obj.SponsorId = SponsorId;
                obj.FirstName = FirstName;
                obj.LastName = LastName;
                obj.MobileNo = MobileNo;
                obj.RegistrationBy = "Web";
                obj.PinCode = PinCode;
                obj.Leg = null;
                obj.Password = Crypto.Encrypt(Password);
                obj.Email = Email;
                obj.Gender = Gender;
                obj.State = State;
                obj.City = City;
                DataSet ds = obj.Registration();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        Session["Pk_UserId"] = ds.Tables[0].Rows[0]["Pk_userId"].ToString();
                        Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                        Session["DisplayName"] = ds.Tables[0].Rows[0]["Name"].ToString();
                        Session["PassWord"] = Crypto.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString());
                        Session["Transpassword"] = Crypto.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString());
                        Session["MobileNo"] = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                        Session["Email"] = ds.Tables[0].Rows[0]["Email"].ToString();
                        Session["Profile"] = "";
                        obj.Result = "1";
                        if (obj.Email != "" && obj.Email != null)
                        {
                            try
                            {
                                BLMail.SendRegistrationMail(Session["DisplayName"].ToString(), Session["LoginId"].ToString(), Session["PassWord"].ToString(), "Registration Successful", obj.Email);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        obj.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Result = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmRegistration()
        {
            return View();
        }
        public ActionResult CompleteRegistration()
        {
            if (Session["LoginId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            #region Product Bind
            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProductForJoiningForUser();
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
            return View();
        }
        [HttpPost]
        public ActionResult CompleteRegistration(Home model)
        {
            OrderModel orderModel = new OrderModel();
            string random = Common.GenerateRandom();
            CreateOrderResponse obj1 = new CreateOrderResponse();
            if (Session["Pk_UserId"].ToString() == "209")
            {
                model.Amount = "100";
            }
            else
            {
                model.Amount = (Convert.ToInt32(model.Amount) * 100).ToString();
            }
            try
            {
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", Convert.ToInt32(model.Amount)); // amount in the smallest currency unit
                options.Add("receipt", random);
                options.Add("currency", "INR");
                options.Add("payment_capture", "1");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                RazorpayClient client = new RazorpayClient(PaymentGateWayDetails.KeyName, PaymentGateWayDetails.SecretKey);
                Razorpay.Api.Order order = client.Order.Create(options);
                obj1.OrderId = order["id"].ToString();
                obj1.Status = "0";
                model.OrderId = order["id"].ToString();
                model.Fk_UserId = Session["Pk_UserId"].ToString();
                model.AddedBy = Session["Pk_UserId"].ToString();
                model.PaymentMode = "12";
                orderModel.orderId = order.Attributes["id"];
                orderModel.razorpayKey = "rzp_live_k8z9ufVw0R0MLV";
                orderModel.amount = Convert.ToInt32(model.Amount);
                orderModel.currency = "INR";
                orderModel.description = "Activate Account";
                orderModel.name = Session["FullName"].ToString();
                orderModel.contactNumber = Session["Contact"].ToString();
                orderModel.email = Session["Email"].ToString();
                orderModel.FK_ProductId = model.Package;
                //orderModel.image = "http://RealWealth.co.in/RealWealthWebsite/assets/img/logo.png";
                DataSet ds = model.SaveOrderDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        return View("ActivationPayment", orderModel);
                    }
                    else
                    {
                        return RedirectToAction("CompleteRegistration", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("CompleteRegistration", "Home");
                }
                // Return on PaymentPage with Order data
            }
            catch (Exception ex)
            {
                obj1.Status = "1";
                TempData["error"] = ex.Message;
                return RedirectToAction("CompleteRegistration", "Home");
            }
        }
        public ActionResult FillAmount(string ProductId)
        {
            Admin obj = new Admin();
            obj.Package = ProductId;
            DataSet ds = obj.BindPriceByProduct();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                obj.Amount = Convert.ToInt32(ds.Tables[0].Rows[0]["FinalAmount"]).ToString();
            }
            else { }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FetchActivationPaymentByOrder(OrderModel model)
        {
            FetchPaymentByOrder obj = new FetchPaymentByOrder();
            FetchPaymentByOrderResponse obj1 = new FetchPaymentByOrderResponse();
            string random = Common.GenerateRandom();
            try
            {
                obj.OrderId = model.orderId;
                obj1.Pk_UserId = Session["Pk_UserId"].ToString();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                RazorpayClient client = new RazorpayClient(PaymentGateWayDetails.KeyName, PaymentGateWayDetails.SecretKey);
                List<Razorpay.Api.Payment> orderdetails = client.Order.Payments(obj.OrderId);
                if (orderdetails.Count > 0)
                {
                    for (int i = 0; i <= orderdetails.Count - 1; i++)
                    {
                        dynamic rr = orderdetails[i].Attributes;
                        obj1.PaymentId = rr["id"];
                        obj1.entity = rr["entity"];
                        obj1.amount = rr["amount"];
                        obj1.currency = rr["currency"];
                        obj1.status = rr["status"];
                        obj1.OrderId = rr["order_id"];
                        obj1.invoice_id = rr["invoice_id"];
                        obj1.international = rr["international"];
                        obj1.method = rr["method"];
                        obj1.amount_refunded = rr["amount_refunded"];
                        obj1.refund_status = rr["refund_status"];
                        obj1.captured = rr["captured"];
                        obj1.description = rr["description"];
                        obj1.card_id = rr["card_id"];
                        obj1.bank = rr["bank"];
                        obj1.wallet = rr["wallet"];
                        obj1.vpa = rr["vpa"];
                        obj1.email = rr["email"];
                        obj1.contact = rr["contact"];
                        obj1.fee = rr["fee"];
                        obj1.tax = rr["tax"];
                        obj1.error_code = rr["error_code"];
                        obj1.error_description = rr["error_description"];
                        obj1.error_source = rr["error_source"];
                        obj1.error_step = rr["error_step"];
                        obj1.error_reason = rr["error_reason"];
                        obj1.created_at = rr["created_at"];
                        obj1.FK_ProductId = model.FK_ProductId;
                        DataSet ds = obj1.UpdateRazorpayStatus();
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                            {
                                if (obj1.status == "captured")
                                {
                                    TempData["Msg"] = "Id activated successfully. Order Id : " + obj1.OrderId + " and PaymentId : " + obj1.PaymentId;
                                    BLMail.SendActivationMail(Session["FullName"].ToString(), Session["LoginId"].ToString(), Crypto.Decrypt(Session["Password"].ToString()), "Activation Successful", ds.Tables[0].Rows[0]["Email"].ToString());
                                    return RedirectToAction("UserDashBoard", "User");
                                }
                                else
                                {
                                    TempData["error"] = "Payment Failed";
                                    return RedirectToAction("CompleteRegistration", "Home");
                                }
                            }
                        }
                    }
                }
                else
                {
                    obj1.OrderId = obj.OrderId;
                    obj1.captured = "Failed";
                    TempData["error"] = "Payment Failed";
                    obj1.Pk_UserId = obj.Pk_UserId;
                    DataSet ds = obj1.UpdateRazorpayStatus();
                    return RedirectToAction("CompleteRegistration", "Home");
                }
            }
            catch (Exception ex)
            {
                obj1.OrderId = obj.OrderId;
                obj1.captured = ex.Message;
                TempData["error"] = ex.Message;
                obj1.Pk_UserId = obj.Pk_UserId;
                DataSet ds = obj1.UpdateRazorpayStatus();
                return RedirectToAction("CompleteRegistration", "Home");
            }
            return RedirectToAction("CompleteRegistration", "Home");
        }
        public ActionResult emailtemplate()
        {
            return View();
        }
        public ActionResult GetSponserDetails(string ReferBy)
        {
            Common obj = new Common();
            obj.ReferBy = ReferBy;
            DataSet ds = obj.GetMemberDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["TeamPermanent"].ToString() == "P" || ds.Tables[0].Rows[0]["TeamPermanent"].ToString() == "O")
                {
                    obj.DisplayName = ds.Tables[0].Rows[0]["FullName"].ToString();
                    obj.Result = "Yes";
                }
                else
                {
                    obj.Result = "Sponsor Id is not Active";
                }

            }
            else { obj.Result = "Invalid SponsorId"; }
            return Json(obj, JsonRequestBehavior.AllowGet);
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
        #region Menu
        public virtual ActionResult Menu()
        {
            Home Menu = null;

            if (Session["_Menu"] != null)
            {
                Menu = (Home)Session["_Menu"];
            }
            else
            {
                if (Session["Pk_AdminId"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    Menu = Home.GetMenus(Session["Pk_AdminId"].ToString(), Session["UserTypeName"].ToString()); // pass employee id here
                    Session["_Menu"] = Menu;
                }

            }
            return PartialView("_Menu", Menu);
        }
        #endregion

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ForgetPassword")]
        [OnAction(ButtonName = "forgetpassword")]
        public ActionResult ForgetPassword(Home model)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            try
            {
                DataSet ds = model.ForgetPassword();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                        model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                        model.Password = Crypto.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString());

                        string signature = " &nbsp;&nbsp;&nbsp; Dear  " + model.Name + ",<br/>&nbsp;&nbsp;&nbsp; Your Password Is : " + model.Password;

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("email@gmail.com");
                            mail.To.Add(model.Email);
                            mail.Subject = "Forget Password";
                            mail.Body = signature;
                            mail.IsBodyHtml = true;
                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("developer2.afluex@gmail.com", "deve@486");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            }
                        }
                        TempData["Login"] = "password sent your email-id successfully.";
                    }

                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Login"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["Login"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["Login"] = ex.Message;
            }
            return RedirectToAction("Login", "Home");
        }
        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult CalculateLevelIncomeTr2()
        {
            Home model = new Home();
            DataSet ds = model.CalculateLevelIncomeTr2();
            return View();
        }

        public ActionResult CalculateROI()
        {
            Home model = new Home();
            DataSet ds = model.CalculateROI();
            return View();
        }

        public ActionResult CalculateDistributePaymentTPS()
        {
            Home model = new Home();
            DataSet ds = model.CalculateDistributePaymentTPS();
            return View();
        }

        public ActionResult SaveContact(string name, string email, string phone, string subject, string message)
        {
            Home model = new Home();
            try
            {
                model.Name = name;
                model.Email = email;
                model.MobileNo = phone;
                model.Subject = subject;
                model.Message = message;
                model.AddedBy = "1";
                DataSet ds = model.SaveContact();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        model.Result = "1";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        model.Result = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
                else
                {
                    model.Result = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoDistributeLevelIncome()
        {
            Home model = new Home();
            DataSet ds = model.AutoDistributeLevelIncome();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}