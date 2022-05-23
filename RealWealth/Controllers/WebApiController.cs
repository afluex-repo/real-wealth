using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealWealth.Models;
using System.Data;
using System.IO;
using System.Net;
using Razorpay.Api;
using System.Net.Http;
using System.Net.Mail;

namespace RealWealth.Controllers
{
    public class WebApiController : Controller
    {
        // GET: API
        #region Registration
        public ActionResult Registration(RegistrationAPI model)
        {
            RegistrationAPI obj = new RegistrationAPI();
            if (model.SponsorId == "" || model.SponsorId == null)
            {
                obj.Status = "1";
                obj.Message = "Please Enter Sponsor Id";
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            if (model.FirstName == "" || model.FirstName == null)
            {
                obj.Status = "1";
                obj.Message = "Please Enter First Name";
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            if (model.MobileNo == "" || model.MobileNo == null)
            {
                obj.Status = "1";
                obj.Message = "Please Enter Mobile No";
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            //if (model.Leg == "" || model.Leg == null)
            //{
            //    obj.Status = "1";
            //    obj.Message = "Please Select Leg";
            //    return Json(obj, JsonRequestBehavior.AllowGet);
            //}
            model.SponsorId = model.SponsorId;
            try
            {
                model.RegistrationBy = "Mobile";
                model.Password = Crypto.Encrypt(model.Password);
                DataSet ds = model.Registration();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        obj.PK_UserId = ds.Tables[0].Rows[0]["PK_UserId"].ToString();
                        obj.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                        obj.FullName = ds.Tables[0].Rows[0]["Name"].ToString();
                        obj.Password = Crypto.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString());
                        obj.TransPassword = Crypto.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString());
                        obj.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                        //obj.Leg = model.Leg;
                        obj.RegistrationBy = model.RegistrationBy;
                        obj.SponsorId = model.SponsorId;
                        obj.LastName = model.LastName;
                        obj.PinCode = model.PinCode;
                        obj.Email = model.Email;
                        obj.ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString();
                        obj.Status = "0";
                        obj.Gender = model.Gender;
                        obj.Message = "Registered Successfully";
                        if (obj.Email != "" && obj.Email != null)
                        {
                            string Body = "Dear " + obj.FullName + ",\t\nThank you for your registration. Your Details are as Below: \t\nLogin ID: " + obj.LoginId + "\t\nPassword: " + obj.Password;
                            BLMail.SendMail(obj.Email, "Registration Successful", Body, false);
                        }
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region SponsporName
        public ActionResult GetSponsorName(SponsorNameAPI sponsorname)
        {
            SponsorNameA obj = new SponsorNameA();
            DataSet ds = sponsorname.GetMemberDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                obj.SponsorName = ds.Tables[0].Rows[0]["FullName"].ToString();
                obj.Status = "0";
                obj.Message = "Sponsor Name Fetched";
                return Json(obj, JsonRequestBehavior.AllowGet);


            }
            else
            {
                sponsorname.Status = "1";
                sponsorname.Message = "Invalid SponsorId"; return Json(sponsorname, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region getState
        public ActionResult GetState(Pincode Pindetails)
        {
            StateDetails obj = new StateDetails();
            DataSet ds = Pindetails.GetStateCity();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                obj.State = ds.Tables[0].Rows[0]["State"].ToString();
                obj.City = ds.Tables[0].Rows[0]["City"].ToString();
                obj.Status = "0";
                obj.Message = "Details Fetched";
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Pindetails.Status = "1";
                Pindetails.Message = "Invalid PinCode"; return Json(Pindetails, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Login
        public ActionResult Login(LoginAPI model)
        {
            LoginAPI obj = new LoginAPI();
            Reponse res = new Reponse();
            if (model.LoginId == "" || model.LoginId == null)
            {
                obj.Status = "1";
                obj.Message = "Please Enter Login Id";
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            if (model.Password == "" || model.Password == null)
            {
                obj.Status = "1";
                obj.Message = "Please Enter Password";
            }
            try
            {

                DataSet dsResult = model.Login();
                {
                    if (dsResult.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        if (model.Password == Crypto.Decrypt(dsResult.Tables[0].Rows[0]["Password"].ToString()))
                        {
                            if ((dsResult.Tables[0].Rows[0]["UserType"].ToString() == "Associate"))
                            {
                                obj.LoginId = dsResult.Tables[0].Rows[0]["LoginId"].ToString();
                                obj.UserId = dsResult.Tables[0].Rows[0]["Pk_userId"].ToString();
                                obj.UserType = dsResult.Tables[0].Rows[0]["UserType"].ToString();
                                obj.FullName = dsResult.Tables[0].Rows[0]["FullName"].ToString();
                                obj.Password = Crypto.Decrypt(dsResult.Tables[0].Rows[0]["Password"].ToString());
                                obj.Profile = dsResult.Tables[0].Rows[0]["Profile"].ToString();
                                obj.Status = dsResult.Tables[0].Rows[0]["Status"].ToString();
                                obj.TeamPermanent = dsResult.Tables[0].Rows[0]["TeamPermanent"].ToString();
                                obj.Gender = dsResult.Tables[0].Rows[0]["Sex"].ToString();
                                obj.Email = dsResult.Tables[0].Rows[0]["Email"].ToString();
                                obj.Mobile = dsResult.Tables[0].Rows[0]["Mobile"].ToString();
                                obj.Status = "0";
                                obj.Message = "Successfully Logged in";
                                return Json(obj, JsonRequestBehavior.AllowGet);
                            }
                            else if (dsResult.Tables[0].Rows[0]["UserType"].ToString() == "Admin")
                            {
                                obj.Status = "0";
                                obj.Message = "Successfully Logged in";
                                obj.LoginId = dsResult.Tables[0].Rows[0]["LoginId"].ToString();
                                obj.Pk_adminId = dsResult.Tables[0].Rows[0]["Pk_adminId"].ToString();
                                obj.UserType = dsResult.Tables[0].Rows[0]["UsertypeName"].ToString();
                                obj.FullName = dsResult.Tables[0].Rows[0]["Name"].ToString();

                                if (dsResult.Tables[0].Rows[0]["isFranchiseAdmin"].ToString() == "True")
                                {
                                    obj.FranchiseAdminID = dsResult.Tables[0].Rows[0]["Pk_adminId"].ToString();
                                }
                            }
                            else
                            {
                                res.Status = "1";
                                res.Message = "Incorrect LoginId Or Password";
                                return Json(res, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {

                            res.Status = "1";
                            res.Message = "Invalid LoginId or Password.";
                            return Json(res, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {

                        res.Status = "1";
                        res.Message = "Invalid LoginId or Password.";
                        return Json(res, JsonRequestBehavior.AllowGet);
                    }

                }


                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = "Invalid LoginId or Password.";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region ActivateUser

        public ActionResult ActivateUser(EpinDetails model)
        {
            EpinDetails obj = new EpinDetails();

            if (model.EPin == "" || model.EPin == null)
            {
                obj.Status = "1";
                obj.Message = "Please Enter EPin No";
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            try
            {
                DataSet dsResult = model.ActivateUser();
                {
                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows[0]["Msg"].ToString() == "1")
                        {
                            string Email = dsResult.Tables[0].Rows[0]["Email"].ToString();
                            string Name = dsResult.Tables[0].Rows[0]["Name"].ToString();
                            string LoginId = dsResult.Tables[0].Rows[0]["LoginId"].ToString();
                            string Password = dsResult.Tables[0].Rows[0]["Password"].ToString();
                            obj.Status = "0";
                            obj.Message = "User Activated Successfully";
                            if (Email != null && Email != "")
                            {
                                BLMail.SendActivationMail(Name, LoginId, Crypto.Decrypt(Password), "Activation Successful", Email);
                            
                            }
                            return Json(obj, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {

                            obj.Status = "1";
                            obj.Message = dsResult.Tables[0].Rows[0]["ErrorMessage"].ToString();
                            return Json(obj, JsonRequestBehavior.AllowGet);
                        }
                    }


                }


                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Dashboard
        public ActionResult GetDashboard(AssociateDashBoard associate)
        {
            List<Reward> lst = new List<Reward>();
            DashboardResponse obj = new DashboardResponse();
            DataSet ds = associate.GetAssociateDashboard();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Data Fetched";
                obj.TotalDownline = ds.Tables[0].Rows[0]["TotalDownline"].ToString();
                obj.TotalBusiness = ds.Tables[0].Rows[0]["TotalBusiness"].ToString();
                obj.TeamBusiness = ds.Tables[0].Rows[0]["TeamBusiness"].ToString();
                obj.SelfBusiness = ds.Tables[0].Rows[0]["SelfBusiness"].ToString();
                obj.TotalDirect = ds.Tables[0].Rows[0]["TotalDirect"].ToString();
                obj.TotalActive = ds.Tables[0].Rows[0]["TotalActive"].ToString();
                obj.TotalInActive = ds.Tables[0].Rows[0]["TotalInActive"].ToString();
                obj.TPSId = ds.Tables[0].Rows[0]["TPSId"].ToString();
                obj.TotalTeam = ds.Tables[0].Rows[0]["TotalTeam"].ToString();
                obj.TotalTeamActive = ds.Tables[0].Rows[0]["TotalTeamActive"].ToString();
                obj.TotalTeamInActive = ds.Tables[0].Rows[0]["TotalTeamInActive"].ToString();
                obj.TotalIncome = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalLevelIncomeTTP"]) + Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalLevelIncomeTPS"]);
                obj.LevelIncomeTr1 = ds.Tables[0].Rows[0]["TotalLevelIncomeTTP"].ToString();
                obj.LevelIncomeTr2 = ds.Tables[0].Rows[0]["TotalLevelIncomeTPS"].ToString();
                obj.UsedPins = ds.Tables[0].Rows[0]["UsedPins"].ToString();
                obj.AvailablePins = ds.Tables[0].Rows[0]["AvailablePins"].ToString();
                obj.TotalPins = ds.Tables[0].Rows[0]["TotalPins"].ToString();
                obj.ActiveStatus = ds.Tables[2].Rows[0]["Status"].ToString();
                obj.ActivationDate = ds.Tables[0].Rows[0]["ActivationDate"].ToString();
                obj.TotalPayoutWallet = ds.Tables[0].Rows[0]["TotalPayoutWalletAmount"].ToString();
                obj.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalPayoutWalletAmount"]) + 0;
                obj.WalletBalance = ds.Tables[0].Rows[0]["TotalWalletAmount"].ToString();
                if (obj.ActiveStatus == "Active")
                {
                    obj.ReferralLink = "http://RealWealth.co.in/Home/Registration?Pid=" + associate.Fk_UserId;
                }
                else
                {
                    obj.ReferralLink = "";
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    obj.Tr1Business = ds.Tables[1].Rows[0]["Tr1Business"].ToString();
                    obj.Tr2Business = ds.Tables[1].Rows[0]["Tr2Business"].ToString();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
                {
                    obj.TotalTPSAmountTobeReceived = double.Parse(ds.Tables[3].Compute("sum(TopUpAmount)", "").ToString()).ToString("n2");
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[4].Rows.Count > 0)
                {
                    obj.TotalTPSAmountReceived = double.Parse(ds.Tables[4].Compute("sum(TotalROI)", "").ToString()).ToString("n2");
                    obj.TotalTPSBalanceAmount = Convert.ToDecimal(obj.TotalTPSAmountTobeReceived) - Convert.ToDecimal(obj.TotalTPSAmountReceived);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[5].Rows)
                    {
                        Reward obj1 = new Reward();
                        obj1.PK_RewardId = r["PK_RewardId"].ToString();
                        obj1.Title = r["Title"].ToString();
                        obj1.Image = r["postedFile"].ToString();
                        lst.Add(obj1);
                    }
                    obj.lstReward = lst;
                }
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Tree
        public ActionResult Tree(TreeAPI model)
        {

            UpdateProfile sta = new UpdateProfile();
            TreeAPI obj = new TreeAPI();
            if (model.LoginId == "" || model.LoginId == null)
            {
                model.Status = "1";
                model.Message = "Please enter LoginId";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            if (model.Fk_headId == "" || model.Fk_headId == null)
            {
                model.Status = "1";
                model.Message = "Please enter headId";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DataSet ds = model.GetTree();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    if (ds.Tables[0].Rows[0]["msg"].ToString() == "0")
                    {

                        List<Tree1> GetGenelogy = new List<Tree1>();
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            Tree1 obj1 = new Tree1();
                            obj1.Fk_UserId = r["Fk_UserId"].ToString();
                            obj1.Fk_ParentId = r["Fk_ParentId"].ToString();
                            obj1.Fk_SponsorId = r["Fk_SponsorId"].ToString();
                            obj1.SponsorId = r["SponsorId"].ToString();
                            obj1.LoginId = r["LoginId"].ToString();
                            obj1.TeamPermanent = r["TeamPermanent"].ToString();
                            obj1.MemberName = r["MemberName"].ToString();
                            obj1.MemberLevel = r["MemberLevel"].ToString();
                            obj1.Leg = r["Leg"].ToString();
                            obj1.Id = r["Id"].ToString();

                            obj1.ActivationDate = r["ActivationDate"].ToString();
                            obj1.ActiveLeft = r["ActiveLeft"].ToString();
                            obj1.ActiveRight = r["ActiveRight"].ToString();
                            obj1.InactiveLeft = r["InactiveLeft"].ToString();
                            obj1.InactiveRight = r["InactiveRight"].ToString();
                            obj1.BusinessLeft = r["BusinessLeft"].ToString();
                            obj1.BusinessRight = r["BusinessRight"].ToString();
                            obj1.ImageURL = r["ImageURL"].ToString();
                            GetGenelogy.Add(obj1);
                        }
                        obj.GetGenelogy = GetGenelogy;
                        obj.Message = "Tree";
                        obj.Status = "0";
                        obj.LoginId = model.LoginId;
                        obj.Fk_headId = model.Fk_headId;

                    }
                    else
                    {
                        sta.Status = "1";
                        sta.Message = "No Data Found";
                        return Json(sta, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    sta.Status = "1";
                    sta.Message = "No Data Found";
                    return Json(sta, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                sta.Status = "1";
                sta.Message = ex.Message;
                return Json(sta, JsonRequestBehavior.AllowGet);
            }


            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ActivateUser

        public ActionResult Topup(TopupByUser model)
        {
            TopupResponse obj = new TopupResponse();
            //model.TopUpDate = string.IsNullOrEmpty(model.TopUpDate) ? null : Common.ConvertToSystemDate(model.TopUpDate, "dd/mm/yyyy");
            //model.TransactionDate = string.IsNullOrEmpty(model.TransactionDate) ? null : Common.ConvertToSystemDate(model.TransactionDate, "dd/mm/yyyy");
            try
            {
                DataSet dsResult = model.TopUp();
                {
                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows[0]["Msg"].ToString() == "1")
                        {
                            obj.Status = "0";
                            obj.Message = "Top-Up Done successfully";
                            return Json(obj, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {

                            obj.Status = "1";
                            obj.Message = dsResult.Tables[0].Rows[0]["ErrorMessage"].ToString();
                            return Json(obj, JsonRequestBehavior.AllowGet);
                        }
                    }


                }


                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region getPaymentMode
        public ActionResult GetPaymentMode()
        {
            List<PaymentMode> lst = new List<PaymentMode>();
            PaymentModeResponse obj = new PaymentModeResponse();
            DataSet ds = obj.PaymentList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    PaymentMode model = new PaymentMode();
                    model.PK_PaymentModeId = r["PK_paymentID"].ToString();
                    model.PaymentModeName = r["PaymentMode"].ToString();
                    lst.Add(model);
                }
                obj.lst = lst;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    obj.UPIId = ds.Tables[1].Rows[0]["UPIId"].ToString();
                    obj.UPIImage = ds.Tables[1].Rows[0]["Image"].ToString();
                }
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult GetPackage()
        {
            List<Package> lst = new List<Package>();
            PackageResponse obj = new PackageResponse();
            DataSet ds = obj.PackageListAll();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    Package model = new Package();
                    model.PK_PackageId = r["Pk_ProductId"].ToString();
                    model.PackageName = r["ProductName"].ToString();
                    lst.Add(model);
                }
                obj.lst = lst;
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DownteamTree(DirectRequest req)
        {
            DirectReponse model = new DirectReponse();
            List<DirectList> lst = new List<DirectList>();
            DataSet ds = req.GetDirectList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Status = "0";
                model.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    DirectList obj = new DirectList();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.Level = r["Lvl"].ToString();
                    obj.PermanentDate = (r["PermanentDate"].ToString());
                    obj.Status = (r["Status"].ToString());
                    obj.LoginId = (r["LoginId"].ToString());
                    obj.Name = (r["Name"].ToString());
                    obj.Package = (r["ProductName"].ToString());

                    obj.FK_UserId = (r["PK_UserId"].ToString());
                    obj.SponsorId = (r["SponsorId"].ToString());
                    obj.SponsorName = (r["SponsorName"].ToString());
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            else
            {
                model.Status = "1";
                model.Message = "No Record Found";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult DownlineList(DirectRequest req)
        //{
        //    DirectReponse model = new DirectReponse();
        //    List<DirectList> lst = new List<DirectList>();
        //    DataSet ds = req.GetDownlineList();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        model.Status = "0";
        //        model.Message = "Record Found";
        //        foreach (DataRow r in ds.Tables[0].Rows)
        //        {
        //            DirectList obj = new DirectList();
        //            obj.Mobile = r["Mobile"].ToString();
        //            //obj.Email = r["Email"].ToString();
        //            obj.JoiningDate = r["JoiningDate"].ToString();
        //            obj.Leg = r["Leg"].ToString();
        //            obj.PermanentDate = (r["PermanentDate"].ToString());
        //            obj.Status = (r["Status"].ToString());
        //            obj.LoginId = (r["LoginId"].ToString());
        //            obj.Name = (r["Name"].ToString());
        //            obj.Package = (r["ProductName"].ToString());
        //            lst.Add(obj);
        //        }
        //        model.lst = lst;
        //    }
        //    else
        //    {
        //        model.Status = "1";
        //        model.Message = "No Record Found";
        //    }
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult PinList(PinRequest req)
        {
            PinAPI model = new PinAPI();
            List<PinAPIResponse> lst = new List<PinAPIResponse>();
            DataSet ds = req.GetPinList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Status = "0";
                model.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    PinAPIResponse obj = new PinAPIResponse();
                    obj.ePinNo = r["ePinNo"].ToString();
                    obj.PinAmount = r["PinAmount"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    obj.PinStatus = r["PinStatus"].ToString();
                    obj.RegisteredTo = r["RegisteredTo"].ToString();
                    obj.Amount = r["TotalAmount"].ToString();
                    obj.PinGenerationDate = r["PinGenerationDate"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    obj.GST = r["IGST"].ToString();

                    //obj.IsRegistered = r["IsRegistered"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            else
            {
                model.Status = "1";
                model.Message = "No Record Found";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult LevelTree(LevelTreeReq req)
        //{
        //    LevelTreeAPI model = new LevelTreeAPI();
        //    List<LevelTreeResponse> lst = new List<LevelTreeResponse>();
        //    DataSet ds = req.GetLevelTreeData();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        model.Status = "0";
        //        model.Message = "Record Found";
        //        foreach (DataRow r in ds.Tables[0].Rows)
        //        {
        //            LevelTreeResponse obj = new LevelTreeResponse();
        //            obj.FK_ParentId = r["Parentid"].ToString();
        //            obj.PK_UserId = r["PK_UserId"].ToString();
        //            obj.FK_SponsorId = r["FK_SponsorID"].ToString();
        //            obj.LoginId = r["LoginId"].ToString();
        //            obj.MemberName = r["MemberName"].ToString();
        //            obj.AssociateMemberName = r["AssociateMemberName"].ToString();
        //            obj.ProfilePic = r["ProfilePic"].ToString();
        //            lst.Add(obj);
        //        }
        //        model.lst = lst;
        //    }
        //    else
        //    {
        //        model.Status = "1";
        //        model.Message = "No Record Found";
        //    }
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult AssociateTree(AssociateBookingRequest req)
        {
            AssociateBookingAPI model = new AssociateBookingAPI();
            List<AssciateBookingResponse> lst = new List<AssciateBookingResponse>();
            DataSet ds = req.GetDownlineTree();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Status = "0";
                model.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AssciateBookingResponse obj = new AssciateBookingResponse();
                    obj.Fk_UserId = r["Pk_UserId"].ToString();
                    obj.Fk_SponsorId = r["Fk_SponsorId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.FirstName = r["FirstName"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.ActiveStatus = r["ActiveStatus"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            else
            {
                model.Status = "1";
                model.Message = "No Record Found";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult WalletBalance(Wallet model)
        {
            WalletBalanceAPI obj = new WalletBalanceAPI();
            DataSet ds = model.GetWalletBalance();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                obj.Balance = ds.Tables[0].Rows[0]["amount"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                obj.KYCStatus = ds.Tables[1].Rows[0]["PanStatus"].ToString();
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TransferPin(TransferPin model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.ePinTransfer();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "Transfer Successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PinTransferReport(PinReport req)
        {
            PinResponse model = new PinResponse();
            List<PinDetails> lst = new List<PinDetails>();
            DataSet ds = req.GetTransferPinReport();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Status = "0";
                model.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    PinDetails obj = new PinDetails();
                    obj.ePinNo = r["EpinNo"].ToString();
                    obj.FromId = r["FromId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.ToId = r["ToId"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.TransferDate = r["TransferDate"].ToString();
                    obj.PinAmount = r["PinAmount"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    obj.BV = r["BV"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            else
            {
                model.Status = "1";
                model.Message = "No Record Found";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UserProfile(Request model)
        {
            ProfileAPI obj = new ProfileAPI();
            DataSet ds = model.UserProfile();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                obj.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                obj.FK_UserId = ds.Tables[0].Rows[0]["PK_UserId"].ToString();
                obj.SponsorId = ds.Tables[0].Rows[0]["SponsorId"].ToString();
                obj.SponsorName = ds.Tables[0].Rows[0]["SponsorName"].ToString();
                obj.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                obj.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                obj.Gender = ds.Tables[0].Rows[0]["Sex"].ToString();
                obj.MobileNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                obj.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                obj.PinCode = ds.Tables[0].Rows[0]["PinCode"].ToString();
                obj.State = ds.Tables[0].Rows[0]["State"].ToString();
                obj.City = ds.Tables[0].Rows[0]["City"].ToString();
                obj.PanNo = ds.Tables[0].Rows[0]["PanNumber"].ToString();
                obj.AadharNo = ds.Tables[0].Rows[0]["AdharNumber"].ToString();
                obj.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                obj.ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString();
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UpdateProfile(ProfileAPI model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.UpdateProfile();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "Profile Updated Successfully";
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChangePassword(Password model)
        {
            Reponse obj = new Reponse();
            try
            {
                model.OldPassword = Crypto.Encrypt(model.OldPassword);
                model.NewPassword = Crypto.Encrypt(model.NewPassword);
                DataSet ds = model.ChangePassword();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "Password Changed  Successfully";
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ActivateUserByPin(ActivateUser model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.ActivateUserByPin();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "User Activated Successfully";
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetBankDetails(BankDetailsUpdateRequest model)
        {
            BankDetailsUpdateAPIResponse obj = new BankDetailsUpdateAPIResponse();
            DataSet ds = model.BankDetailsEdit();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                obj.AdharNo = ds.Tables[0].Rows[0]["AdharNumber"].ToString();
                obj.Fk_UserId = model.FK_UserId;
                obj.PanNumber = ds.Tables[0].Rows[0]["PanNumber"].ToString();
                obj.BankName = ds.Tables[0].Rows[0]["MemberBankName"].ToString();
                obj.AccountNo = ds.Tables[0].Rows[0]["MemberAccNo"].ToString();
                obj.BranchName = ds.Tables[0].Rows[0]["MemberBranch"].ToString();
                obj.IFSCCode = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                obj.NomineeName = ds.Tables[0].Rows[0]["NomineeName"].ToString();
                obj.NomineeRelation = ds.Tables[0].Rows[0]["NomineeRelation"].ToString();
                obj.NomineeAge = ds.Tables[0].Rows[0]["NomineeAge"].ToString();
                obj.UPIId = ds.Tables[0].Rows[0]["UPIID"].ToString();
                obj.PanImage = ds.Tables[0].Rows[0]["PanImage"].ToString();
                obj.IsVerified = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISVerified"]);
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateBankDetails(BankDetailsUpdateAPIResponse model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.BankUpdate();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "Bank Details Updated Successfully";
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddWallet(AddWalletRequest model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.AddWallet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "E-Wallet save successfully";
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult WalletRequestList(WalletRequestList model)
        {
            WalletResponse res = new WalletResponse();
            List<WalletDetails> lst = new List<WalletDetails>();
            try
            {
                DataSet ds = model.GetEwalletRequestDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        WalletDetails obj = new WalletDetails();
                        obj.RequestID = r["PK_RequestID"].ToString();
                        obj.UserId = r["FK_UserId"].ToString();
                        obj.RequestCode = r["RequestCode"].ToString();
                        obj.Amount = r["Amount"].ToString();
                        obj.PaymentMode = r["PaymentMode"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.BankName = r["BankName"].ToString();
                        obj.TransactionDate = r["RequestedDate"].ToString();
                        obj.BankBranch = r["BankBranch"].ToString();
                        obj.ChequeDDNo = r["ChequeDDNo"].ToString();
                        obj.ChequeDDDate = r["ChequeDDDate"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.DisplayName = r["Name"].ToString();
                        obj.Remark = r["Remark"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult WalletLedger(WalletRequestList req)
        {
            UserWalletAPIResponse res = new UserWalletAPIResponse();
            try
            {
                List<UserWalletAPI> lst = new List<UserWalletAPI>();
                DataSet ds = req.GetEWalletDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        UserWalletAPI obj = new UserWalletAPI();
                        obj.Pk_EwalletId = r["Pk_EwalletId"].ToString();
                        obj.CrAmount = r["CrAmount"].ToString();
                        obj.DrAmount = r["DrAmount"].ToString();
                        obj.Narration = r["Narration"].ToString();
                        obj.TransactionDate = r["TransactionDate"].ToString();
                        obj.Balance = r["Balance"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                    res.TotalCr = double.Parse(ds.Tables[0].Compute("sum(CrAmount)", "").ToString()).ToString("n2");
                    res.TotalDr = double.Parse(ds.Tables[0].Compute("sum(DrAmount)", "").ToString()).ToString("n2");
                    res.AvailableBalance = double.Parse(ds.Tables[0].Compute("sum(CrAmount)-sum(DrAmount)", "").ToString()).ToString("n2");
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GenerateEPin(PinAPIRequest model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.SaveEpinRequest();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "E_pin generated successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    obj.Status = "1";
                    obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TopUp(TopUpModel model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.TopUp();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "Top-Up Done successfully";
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    obj.Status = "1";
                    obj.Message = "Some Error Occurred";
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TopUpList(WalletRequestList model)
        {
            TopUpListRes res = new TopUpListRes();
            try
            {
                List<TopUpListModel> lst = new List<TopUpListModel>();
                DataSet ds1 = model.GetTopUpDetails();
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds1.Tables[0].Rows)
                    {
                        TopUpListModel obj = new TopUpListModel();
                        obj.InvestmentId = r["Pk_InvestmentId"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.PinAmount = r["PinAmount"].ToString();
                        obj.UsedFor = r["UsedFor"].ToString();
                        obj.BV = r["BV"].ToString();
                        obj.IsCalculated = r["IsCalculated"].ToString();
                        obj.TransactionBy = r["TransactionBy"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.ROIPercentage = r["ROIPercentage"].ToString();
                        obj.TopUpDate = r["TopUpDate"].ToString();
                        obj.ProductName = r["ProductName"].ToString();
                        obj.PackageDays = r["PackageDays"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTTPPackage()
        {
            List<Package> lst = new List<Package>();
            List<Level> lstLevel = new List<Level>();
            PackageResponse obj = new PackageResponse();
            DataSet ds = obj.BindProductForJoining();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Package model = new Package();
                    model.PK_PackageId = r["Pk_ProductId"].ToString();
                    model.PackageName = r["ProductName"].ToString();
                    model.PackagePrice = r["ProductPrice"].ToString();
                    model.MinimumAmount = Convert.ToDecimal(r["FromAmount"]);
                    model.MaximumAmount = Convert.ToDecimal(r["ToAmount"]);
                    model.InMultipleOf = Convert.ToString(r["InMultipleOf"]);
                    model.AmountWithGST = Convert.ToDecimal(r["AmountWithGST"]);
                    lst.Add(model);
                }
                obj.lst = lst;
                for (int i = 1; i <= 10; i++)
                {
                    Level lev = new Level();
                    lev.Value = i.ToString();
                    lev.Text = "Level-" + i.ToString();
                    lstLevel.Add(lev);
                }
                obj.lstLevel = lstLevel;
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTPSPackage()
        {
            List<Package> lst = new List<Package>();
            List<Level> lstLevel = new List<Level>();
            PackageResponse obj = new PackageResponse();
            DataSet ds = obj.PackageList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Package model = new Package();
                    model.PK_PackageId = r["Pk_ProductId"].ToString();
                    model.PackageName = r["ProductName"].ToString();
                    model.PackagePrice = r["ProductPrice"].ToString();
                    model.MinimumAmount = Convert.ToDecimal(r["FromAmount"]);
                    model.MaximumAmount = Convert.ToDecimal(r["ToAmount"]);
                    model.InMultipleOf = Convert.ToString(r["InMultipleOf"]);
                    model.AmountWithGST = Convert.ToDecimal(r["AmountWithGST"]);
                    model.ROIPercent = Convert.ToDecimal(r["ROIPercent"]);
                    lst.Add(model);
                }
                obj.lst = lst;
                for (int i = 1; i <= 12; i++)
                {
                    Level lev = new Level();
                    lev.Value = i.ToString();
                    lev.Text = "Level-" + i.ToString();
                    lstLevel.Add(lev);
                }
                obj.lstLevel = lstLevel;
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DirectList(RequestForDirect model)
        {
            DirectListAPIRes res = new DirectListAPIRes();
            try
            {
                List<DirectListAPI> lst = new List<DirectListAPI>();
                DataSet ds = model.GetDownlineTree();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    res.FK_SponsorId = ds.Tables[0].Rows[0]["Fk_SponsorId"].ToString();
                    res.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        DirectListAPI obj = new DirectListAPI();
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
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PaymentType()
        {
            List<PaymentTypeAPI> lst = new List<PaymentTypeAPI>();
            PaymentTypeAPIResponse obj = new PaymentTypeAPIResponse();
            DataSet ds = obj.PaymentList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Status = "0";
                obj.Message = "Record Found";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    PaymentTypeAPI model = new PaymentTypeAPI();
                    model.PK_PaymentTypeId = r["PK_PaymentTypeId"].ToString();
                    model.PaymentType = r["PaymentType"].ToString();
                    lst.Add(model);
                }
                obj.lst = lst;
            }
            else
            {
                obj.Status = "1";
                obj.Message = "No Record Found";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTTPMembersCountLevelWise(LevelTreeReq req)
        {
            LevelTreeAPI res = new LevelTreeAPI();
            try
            {
                List<LevelTreeMembers> lst = new List<LevelTreeMembers>();
                List<LevelTreeMemberDetails> lstMember = new List<LevelTreeMemberDetails>();
                DataSet ds = req.GetLevelMembersCountTR1();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        res.Status = "1";
                        res.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                    else
                    {
                        res.Status = "0";
                        res.Message = "Record Found";
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            LevelTreeMembers obj = new LevelTreeMembers();
                            obj.Level = r["LevelNo"].ToString();
                            obj.NumberOfMembers = r["TotalAssociate"].ToString();
                            lst.Add(obj);
                        }
                        res.lst = lst;
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            if (ds.Tables[1].Rows[0]["Lvl"].ToString() == "10")
                            {

                            }
                            res.Level = ds.Tables[1].Rows[0]["Lvl"].ToString();
                            res.ActiveStatus = ds.Tables[1].Rows[0]["Status"].ToString();
                            res.Color = ds.Tables[1].Rows[0]["Color"].ToString();
                            res.DisplayName = ds.Tables[1].Rows[0]["Name"].ToString();
                            res.PK_UserId = ds.Tables[1].Rows[0]["PK_UserId"].ToString();
                            res.ProfilePic = ds.Tables[1].Rows[0]["ProfilePic"].ToString();
                            res.TotalDirect = ds.Tables[1].Rows[0]["TotalDirect"].ToString();
                            res.TotalActive = ds.Tables[1].Rows[0]["TotalActive"].ToString();
                            res.TotalInactive = ds.Tables[1].Rows[0]["TotalInActive"].ToString();
                            res.TotalTeam = ds.Tables[1].Rows[0]["TotalTeam"].ToString();
                            res.SelfBV = "";
                            res.TeamBV = "";
                            res.TotalActiveTeam = ds.Tables[1].Rows[0]["TotalActiveTeam"].ToString();
                            res.TotalInActiveTeam = ds.Tables[1].Rows[0]["TotalInActiveTeam"].ToString();
                            res.SponsorName = ds.Tables[1].Rows[0]["SponsorName"].ToString();
                        }
                        DataSet ds1 = req.GetLevelMembers("1", res.PK_UserId);
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow r in ds1.Tables[0].Rows)
                            {
                                LevelTreeMemberDetails obj = new LevelTreeMemberDetails();
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
                            res.lstDetails = lstMember;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTreeMembers(string Level, string PK_UserId)
        {
            LevelMembers res = new LevelMembers();
            try
            {
                LevelTreeReq req = new LevelTreeReq();
                List<LevelTreeMemberDetails> lst = new List<LevelTreeMemberDetails>();
                DataSet ds = req.GetLevelMembers(Level, PK_UserId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LevelTreeMemberDetails obj = new LevelTreeMemberDetails();
                        obj.PK_UserId = r["PK_UserId"].ToString();
                        obj.MemberName = r["MemberName"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.ProfilePic = r["ProfilePic"].ToString();
                        obj.SelfBV = r["SelfBV"].ToString();
                        obj.TeamBV = r["TeamBV"].ToString();
                        obj.SponsorName = r["SponsorName"].ToString();
                        obj.Color = r["Color"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTPSMembersCountLevelWise(LevelTreeReq req)
        {
            LevelTreeAPI res = new LevelTreeAPI();
            try
            {
                List<LevelTreeMembers> lst = new List<LevelTreeMembers>();
                List<LevelTreeMemberDetails> lstMember = new List<LevelTreeMemberDetails>();
                DataSet ds = req.GetLevelMembersCount();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LevelTreeMembers obj = new LevelTreeMembers();
                        obj.Level = r["LevelNo"].ToString();
                        obj.NumberOfMembers = r["TotalAssociate"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    res.Level = ds.Tables[1].Rows[0]["Lvl"].ToString();
                    res.ActiveStatus = ds.Tables[1].Rows[0]["Status"].ToString();
                    res.Color = ds.Tables[1].Rows[0]["Color"].ToString();
                    res.DisplayName = ds.Tables[1].Rows[0]["Name"].ToString();
                    res.PK_UserId = ds.Tables[1].Rows[0]["PK_UserId"].ToString();
                    res.ProfilePic = ds.Tables[1].Rows[0]["ProfilePic"].ToString();
                    res.TotalDirect = ds.Tables[1].Rows[0]["TotalDirect"].ToString();
                    res.TotalActive = ds.Tables[1].Rows[0]["TotalActive"].ToString();
                    res.TotalInactive = ds.Tables[1].Rows[0]["TotalInActive"].ToString();
                    res.TotalTeam = ds.Tables[1].Rows[0]["TotalTeam"].ToString();
                    res.SelfBV = ds.Tables[1].Rows[0]["SelfBV"].ToString();
                    res.TeamBV = ds.Tables[1].Rows[0]["TeamBV"].ToString();
                    res.TotalActiveTeam = ds.Tables[1].Rows[0]["TotalActiveTeam"].ToString();
                    res.TotalInActiveTeam = ds.Tables[1].Rows[0]["TotalInActiveTeam"].ToString();
                    res.SponsorName = ds.Tables[1].Rows[0]["SponsorName"].ToString();
                }
                DataSet ds1 = req.GetLevelMembers("1", res.PK_UserId);
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds1.Tables[0].Rows)
                    {
                        LevelTreeMemberDetails obj = new LevelTreeMemberDetails();
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
                    res.lstDetails = lstMember;
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTreeMembersTPS(string Level, string PK_UserId)
        {
            LevelMembers res = new LevelMembers();
            try
            {
                LevelTreeReq req = new LevelTreeReq();
                List<LevelTreeMemberDetails> lst = new List<LevelTreeMemberDetails>();
                DataSet ds = req.GetLevelMembers(Level, PK_UserId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LevelTreeMemberDetails obj = new LevelTreeMemberDetails();
                        obj.PK_UserId = r["PK_UserId"].ToString();
                        obj.MemberName = r["MemberName"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.ProfilePic = r["ProfilePic"].ToString();
                        obj.SelfBV = r["SelfBV"].ToString();
                        obj.TeamBV = r["TeamBV"].ToString();
                        obj.SponsorName = r["SponsorName"].ToString();
                        obj.Color = r["Color"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteWalletRequest(WalletRequest model)
        {
            Reponse res = new Reponse();
            try
            {
                DataSet ds = model.DeleteWallet();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        res.Status = "0";
                        res.Message = "Wallet requested deleted successfully";
                    }
                    else
                    {
                        res.Status = "1";
                        res.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    res.Status = "1";
                    res.Message = "Some error occured";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BusinessReportsForUser(BusinessRequest model)
        {
            BusinessResponse res = new BusinessResponse();
            try
            {
                List<BusinessDetails> lst = new List<BusinessDetails>();
                DataSet ds = model.GetBusinessReports();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        BusinessDetails obj = new BusinessDetails();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Name = r["FirstName"].ToString();
                        obj.Amount = Convert.ToDecimal(r["Amount"].ToString());
                        obj.BV = r["BV"].ToString();
                        obj.Date = r["Date"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.PackageType = r["PackageType"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                    res.TotalAmount = double.Parse(ds.Tables[0].Compute("sum(Amount)", "").ToString()).ToString("n2");
                    res.TotalBV = double.Parse(ds.Tables[0].Compute("sum(BV)", "").ToString()).ToString("n2");
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TPSWallet(WalletRequestList model)
        {
            ROIWalletAPIResponse res = new ROIWalletAPIResponse();
            try
            {
                List<ROIWalletAPI> lst = new List<ROIWalletAPI>();
                DataSet ds = model.GetROIWalletDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ROIWalletAPI obj = new ROIWalletAPI();
                        obj.RoiWalletId = r["Pk_ROIWalletId"].ToString();
                        obj.CrAmount = r["CrAmount"].ToString();
                        obj.DrAmount = r["DrAmount"].ToString();
                        obj.Narration = r["Narration"].ToString();
                        obj.TransactionDate = r["TransactionDate"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ROIIncomeReports(WalletRequestList model)
        {
            ROIIncomeResponse res = new ROIIncomeResponse();
            try
            {
                List<ROIIncome> lst = new List<ROIIncome>();
                DataSet ds = model.GetROIIncomeReportsDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ROIIncome obj = new ROIIncome();
                        obj.ROIId = r["Pk_ROIId"].ToString();
                        obj.Pk_InvestmentId = r["Pk_InvestmentId"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.TopUpAmount = r["TopUpAmount"].ToString();
                        obj.Date = r["TopUpDate"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                    res.TotalTopUpAmount = double.Parse(ds.Tables[0].Compute("sum(TopUpAmount)", "").ToString()).ToString("n2");
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LevelIncomeTr1Total(PinRequest model)
        {
            LevelIncomeSumResponse res = new LevelIncomeSumResponse();
            try
            {
                List<LevelIncomeSum> lst = new List<LevelIncomeSum>();
                DataSet ds = model.LevelIncomeTr1Total();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LevelIncomeSum obj = new LevelIncomeSum();
                        obj.BusinessAmount = r["BusinessAmount"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.Amount = r["Amount"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = "No Record Found";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LevelIncomeTr1(LevelIncomeRequest model)
        {
            LevelIncomeResponse res = new LevelIncomeResponse();
            try
            {
                List<LevelIncome> lst = new List<LevelIncome>();
                DataSet ds = model.LevelIncomeTr1();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LevelIncome obj = new LevelIncome();
                        obj.FromName = r["FromName"].ToString();
                        obj.FromLoginId = r["LoginId"].ToString();
                        obj.BusinessAmount = r["BusinessAmount"].ToString();
                        obj.Percentage = r["CommissionPercentage"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.Amount = r["Amount"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.TransactionDate = r["TransactionDate"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = "No Record Found";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LevelIncomeTr2Total(PinRequest model)
        {
            LevelIncomeSumResponse res = new LevelIncomeSumResponse();
            try
            {
                List<LevelIncomeSum> lst = new List<LevelIncomeSum>();
                DataSet ds = model.LevelIncomeTr2Total();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LevelIncomeSum obj = new LevelIncomeSum();
                        obj.BusinessAmount = r["BusinessAmount"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.Amount = r["Amount"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = "No Record Found";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LevelIncomeTr2(LevelIncomeRequest model)
        {
            LevelIncomeResponse res = new LevelIncomeResponse();
            try
            {
                List<LevelIncome> lst = new List<LevelIncome>();
                DataSet ds = model.LevelIncomeTr2();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LevelIncome obj = new LevelIncome();
                        obj.FromName = r["FromName"].ToString();
                        obj.FromLoginId = r["LoginId"].ToString();
                        obj.BusinessAmount = r["BusinessAmount"].ToString();
                        obj.Percentage = r["CommissionPercentage"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.Amount = r["Amount"].ToString();
                        obj.Level = r["Lvl"].ToString();
                        obj.TransactionDate = r["TransactionDate"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = "No Record Found";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PayoutWalletLedger(PayoutWalletReq model)
        {
            PayoutWalletRes res = new PayoutWalletRes();
            try
            {
                List<PayoutWallet> lst = new List<PayoutWallet>();
                DataSet ds = model.PayoutWalletLedger();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        PayoutWallet obj = new PayoutWallet();
                        obj.PK_PayoutWalletId = r["PK_PayoutWalletId"].ToString();
                        obj.FK_UserId = r["FK_UserId"].ToString();
                        obj.CrAmount = r["CrAmount"].ToString();
                        obj.DrAmount = r["DrAmount"].ToString();
                        obj.Narration = r["Narration"].ToString();
                        obj.TransactionDate = r["TransactionDate"].ToString();
                        lst.Add(obj);
                    }
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PayoutDetail(PayoutDetailRequest model)
        {
            PayoutDetailResponse res = new PayoutDetailResponse();
            try
            {
                List<PayoutDetail> lst = new List<PayoutDetail>();
                model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
                model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
                DataSet ds = model.PayoutDetail();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        PayoutDetail obj = new PayoutDetail();
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
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetROIDetails(string InvId, string FK_UserId)
        {
            UserWallet req = new UserWallet();
            ROIResponse model = new ROIResponse();
            try
            {
                req.Pk_InvestmentId = InvId;
                List<ROIDetails> lst = new List<ROIDetails>();
                req.FK_UserId = FK_UserId;
                DataSet ds = req.GetROIDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    model.Status = "0";
                    model.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ROIDetails obj = new ROIDetails();
                        obj.Pk_ROIId = r["Pk_ROIId"].ToString();
                        obj.ROI = r["ROI"].ToString();
                        obj.Date = r["ROIDate"].ToString();
                        obj.ROIStatus = r["Status"].ToString();
                        lst.Add(obj);
                    }
                    model.lst = lst;
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    model.ReceivedAmount = ds.Tables[1].Rows[0]["ReceivedAmount"].ToString();
                    model.TotalAmount = ds.Tables[1].Rows[0]["TotalAmount"].ToString();
                    model.BalanceAmount = ds.Tables[1].Rows[0]["BalanceAmount"].ToString();
                }
            }
            catch (Exception ex)
            {
                model.Status = "1";
                model.Message = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TransferToAccountList(PayoutRequest req)
        {
            PayoutRequestResponse model = new PayoutRequestResponse();
            try
            {
                List<PayoutDetailsForAPI> lst = new List<PayoutDetailsForAPI>();
                DataSet ds = req.GetPayoutRequest();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    model.Status = "0";
                    model.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        PayoutDetailsForAPI obj = new PayoutDetailsForAPI();
                        obj.PK_RequestID = r["Pk_RequestId"].ToString();
                        obj.Amount = Convert.ToDecimal(r["AMount"].ToString());
                        obj.Date = r["RequestedDate"].ToString();
                        obj.IFSCCode = r["IFSCCode"].ToString();
                        obj.AccountNo = r["MemberAccNo"].ToString();
                        obj.Status = r["Status"].ToString();
                        obj.LoginId = r["LoginId"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.ROIPercentage = r["BackColor"].ToString();
                        obj.TransactionNo = r["TransactionNo"].ToString();
                        obj.GrossAmount = r["GrossAmount"].ToString();
                        obj.ProcessingFee = r["DeductionCharges"].ToString();
                        lst.Add(obj);
                    }
                    model.lst = lst;
                }
            }
            catch (Exception ex)
            {
                model.Status = "1";
                model.Message = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TransferToAccount(PayoutRequest req)
        {
            Reponse model = new Reponse();
            try
            {
                DataSet ds = req.SavePayoutRequest();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        model.Status = "0";
                        model.Message = "Transfer To Account Initiated Successfully.";
                    }
                    else
                    {
                        model.Status = "1";
                        model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else { }
            }
            catch (Exception ex)
            {
                model.Status = "1";
                model.Message = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PaidIncome(PayoutDetailRequest model)
        {
            PaidIncomeResponse res = new PaidIncomeResponse();
            try
            {
                List<PaidIncomeDetails> lst = new List<PaidIncomeDetails>();

                DataSet ds = model.PaidIncome();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        PaidIncomeDetails obj = new PaidIncomeDetails();
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
                    res.lst = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ActivationByPayment(JoiningPayment model)
        {
            OrderModel orderModel = new OrderModel();
            string random = Common.GenerateRandom();
            model.Amount = (Convert.ToInt32(model.Amount) * 100).ToString();
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
                model.OrderId = order["id"].ToString();
                model.PaymentMode = "12";
                orderModel.orderId = order.Attributes["id"];
                orderModel.razorpayKey = "rzp_live_k8z9ufVw0R0MLV";
                orderModel.amount = Convert.ToInt32(model.Amount);
                orderModel.currency = "INR";
                orderModel.description = "Activate Account";
                orderModel.name = model.Name;
                orderModel.contactNumber = model.MobileNo;
                orderModel.email = model.Email;
                DataSet ds = model.SaveOrderDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        model.Status = "0";
                        model.Message = "Order created successfully";
                    }
                }

            }
            catch (Exception ex)
            {
                model.Status = "1";
                model.Message = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult FetchPaymentResposne(JoiningPayment model)
        {
            FetchPaymentByOrderResponse obj1 = new FetchPaymentByOrderResponse();
            string random = Common.GenerateRandom();
            try
            {
                obj1.Pk_UserId = model.FK_UserId;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                RazorpayClient client = new RazorpayClient(PaymentGateWayDetails.KeyName, PaymentGateWayDetails.SecretKey);
                List<Razorpay.Api.Payment> orderdetails = client.Order.Payments(model.OrderId);
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

                        DataSet ds = obj1.UpdateRazorpayStatus();
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                            {
                                if (obj1.status == "captured")
                                {
                                    model.Status = "0";
                                    model.Message = "Id activated successfully. Order Id : " + obj1.OrderId + " and PaymentId : " + obj1.PaymentId;
                                    model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                                    if (model.Email != "")
                                    {
                                        BLMail.SendActivationMail(model.Name, model.LoginId, model.Password, "Activation Successful", model.Email);
                                    }
                                }
                                else
                                {
                                    model.Status = "1";
                                    model.Message = "Payment Failed";
                                }
                            }
                            else
                            {
                                model.Status = "1";
                                model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                            }
                        }
                    }
                }
                else
                {
                    obj1.OrderId = model.OrderId;
                    obj1.captured = "Failed";
                    model.Status = "1";
                    model.Message = "Payment Failed";
                    obj1.Pk_UserId = model.FK_UserId;
                    DataSet ds = obj1.UpdateRazorpayStatus();
                }
            }
            catch (Exception ex)
            {
                obj1.OrderId = model.OrderId;
                model.Status = "1";
                obj1.captured = ex.Message;
                model.Message = ex.Message;
                obj1.Pk_UserId = model.FK_UserId;
                DataSet ds = obj1.UpdateRazorpayStatus();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RechargeWallet(JoiningPayment model)
        {
            OrderModel orderModel = new OrderModel();
            string random = Common.GenerateRandom();
            try
            {
                decimal amount = Convert.ToDecimal(model.Amount) * 100;
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", Convert.ToInt32(amount)); // amount in the smallest currency unit
                options.Add("receipt", random);
                options.Add("currency", "INR");
                options.Add("payment_capture", "1");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                RazorpayClient client = new RazorpayClient(PaymentGateWayDetails.KeyName, PaymentGateWayDetails.SecretKey);
                Razorpay.Api.Order order = client.Order.Create(options);
                model.OrderId = order["id"].ToString();
                model.PaymentMode = "12";
                model.Amount = amount.ToString();
                orderModel.orderId = order.Attributes["id"];
                orderModel.razorpayKey = "rzp_live_k8z9ufVw0R0MLV";
                orderModel.amount = Convert.ToInt32(amount);
                orderModel.currency = "INR";
                orderModel.description = "Recharge Wallet";
                orderModel.name = model.Name;
                orderModel.contactNumber = model.MobileNo;
                orderModel.email = model.Email;
                DataSet ds = model.SaveEwalletRequestNew();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        model.Status = "0";
                        model.Message = "Order created successfully";
                    }
                }

            }
            catch (Exception ex)
            {
                model.Status = "1";
                model.Message = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult FetchPaymentForRechargeWallet(JoiningPayment model)
        {
            FetchPaymentByOrderResponse obj1 = new FetchPaymentByOrderResponse();
            string random = Common.GenerateRandom();
            try
            {
                obj1.Pk_UserId = model.FK_UserId;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                RazorpayClient client = new RazorpayClient(PaymentGateWayDetails.KeyName, PaymentGateWayDetails.SecretKey);
                List<Razorpay.Api.Payment> orderdetails = client.Order.Payments(model.OrderId);
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
                        DataSet ds = obj1.SaveFetchPaymentResponse();
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                            {
                                if (obj1.status == "captured")
                                {
                                    model.Status = "0";
                                    model.Message = "Amount credited successfully. Order Id : " + obj1.OrderId + " and PaymentId : " + obj1.PaymentId;
                                }
                                else
                                {
                                    model.Status = "1";
                                    model.Message = "Payment Failed";
                                }
                            }
                            else
                            {
                                model.Status = "1";
                                model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                            }
                        }
                    }
                }
                else
                {
                    obj1.OrderId = model.OrderId;
                    obj1.captured = "Failed";
                    model.Status = "1";
                    model.Message = "Payment Failed";
                    obj1.Pk_UserId = model.FK_UserId;
                    DataSet ds = obj1.SaveFetchPaymentResponse();
                }
            }
            catch (Exception ex)
            {
                obj1.OrderId = model.OrderId;
                model.Status = "1";
                obj1.captured = ex.Message;
                model.Message = ex.Message;
                obj1.Pk_UserId = model.FK_UserId;
                DataSet ds = obj1.SaveFetchPaymentResponse();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult ForgetPassword(ForgetPassword model)
        {
            ForgetPasswordList res = new ForgetPasswordList();
            List<ForgetPasswordResponse> lst = new List<ForgetPasswordResponse>();
            try
            {
                DataSet ds = model.ForgetPasword();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    res.Status = "0";
                    res.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ForgetPasswordResponse obj = new ForgetPasswordResponse();
                        obj.Email = r["Email"].ToString();
                        obj.Name = r["Name"].ToString();
                        obj.Password = Crypto.Decrypt(r["Password"].ToString());
                        string signature = " &nbsp;&nbsp;&nbsp; Dear  " + obj.Name + ",<br/>&nbsp;&nbsp;&nbsp; Your Password Is : " + obj.Password;

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("email@gmail.com");
                            mail.To.Add(model.Email);
                            mail.Subject = "Forget Password";
                            mail.Body = signature;
                            mail.IsBodyHtml = true;
                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("coustomer.RealWealth@gmail.com", "RealWealth@2022");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            }
                        }
                        lst.Add(obj);
                    }
                    res.lsForgetPassword = lst;
                }
                else
                {
                    res.Status = "1";
                    res.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = "1";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateAddress(ProfileAPI model)
        {
            Reponse obj = new Reponse();
            try
            {
                DataSet ds = model.UpdateProfile();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        obj.Status = "0";
                        obj.Message = "Profile Updated Successfully";
                    }
                    else
                    {
                        obj.Status = "1";
                        obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Status = "1";
                obj.Message = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadFile()
        {
            Download obj1 = new Download();
            DownloadResponse model = new DownloadResponse();
            List<Download> lst = new List<Download>();
            try
            {
                DataSet ds = obj1.GetFileDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    model.Status = "0";
                    model.Message = "Record Found";
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Download obj = new Download();
                        obj.PK_FileId = r["PK_RewardId"].ToString();
                        obj.Title = r["Title"].ToString();
                        obj.File = r["postedFile"].ToString();
                        lst.Add(obj);
                    }
                    model.lst = lst;
                }
                else
                {
                    model.Status = "1";
                    model.Message = "No Record Found";
                }
            }
            catch(Exception ex)
            {
                model.Status = "1";
                model.Message = ex.Message;
            }
            return Json(model,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateOrder(string Amount, string MobileNo, string Type,string FK_UserId)
        {
            UserRecharge obj = new UserRecharge();
            UserRechargeAPI model = new UserRechargeAPI();
            obj.FK_UserId =FK_UserId;
            obj.Amount = Convert.ToDecimal(Amount);
            obj.TransactionFor = MobileNo;
            DataSet dsss = obj.GetWalletBalance();
            if (dsss != null && dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                if (obj.Amount <= Convert.ToDecimal(dsss.Tables[0].Rows[0]["amount"]))
                {
                    obj.TransactionType = Type;
                    DataSet ds = obj.CreateOrder();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                        {
                            model.Status = "0";
                            model.OrderNo = ds.Tables[0].Rows[0]["OrderNo"].ToString();
                            model.Message = "Order Created Successfully";
                        }
                        else
                        {
                            model.Status = "1";
                            model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        }
                    }
                    else
                    {
                        model.Status = "1";
                        model.Message = "Temporarily issues occurred. Please Try after some time";
                    }
                }
                else
                {
                    model.Status = "1";
                    model.Message = "You have insufficient balance in your wallet for this plan. Kindly choose another plan";
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveRechageOrBillPaymentResponse(UserRecharge model)
        {
            UserRechargeAPI obj = new UserRechargeAPI();
            DataSet ds = model.SaveBillPaymentResponse();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                {
                    obj.Message = "Recharge done successfully";
                    obj.Status = "0";
                }
                else
                {
                    obj.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    obj.Status = "1";
                }
            }
            else
            {
                obj.Message = "Some issues occurred";
                obj.Status = "1";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}