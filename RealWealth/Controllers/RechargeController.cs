using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using RealWealth.Models;
using RealWealth.Controllers;
using System.Data;
using Newtonsoft.Json.Linq;

namespace RealWealth.Controllers
{
    public class RechargeController : UserBaseController
    {
        // GET: Recharge
        public ActionResult Recharge()
        {
            List<BillPayment> lst = new List<BillPayment>();
            BillPayment model = new BillPayment();
            DataSet ds = model.GetBillPayment();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow r in ds.Tables[0].Rows)
                {
                    BillPayment obj = new BillPayment();
                    obj.Name = r["Name"].ToString();
                    obj.Id = r["PK_Id"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }
        public ActionResult GetOperator()
        {
            var client = new RestClient("https://www.kwikapi.com/api/v2/operator_codes.php?api_key=" + RechargeModel.APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoFillOperator(string MobileNo)
        {
            var client = new RestClient("https://www.kwikapi.com/api/v2/operator_fetch.php");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("api_key", RechargeModel.APIKey);
            request.AddParameter("number", MobileNo);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCircle()
        {
            var client = new RestClient("https://www.kwikapi.com/api/v2/circle_codes.php?api_key=" + RechargeModel.APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPlan(string state_code, string opid)
        {
            var client = new RestClient("https://www.kwikapi.com/api/v2/recharge_plans.php");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("api_key", RechargeModel.APIKey);
            request.AddParameter("state_code", state_code);
            request.AddParameter("opid", opid);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WalletBalance()
        {
            var client = new RestClient("https://www.kwikapi.com/api/v2/balance.php");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddParameter("api_key", RechargeModel.APIKey);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateOrder(string Amount, string MobileNo,string Type)
        {
            UserRecharge model = new UserRecharge();
            model.FK_UserId = Session["Pk_UserId"].ToString();
            model.Amount = Convert.ToDecimal(Amount);
            model.TransactionFor = MobileNo;
            DataSet dsss = model.GetWalletBalance();
            if (dsss != null && dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                if (model.Amount <= Convert.ToDecimal(dsss.Tables[0].Rows[0]["amount"]))
                {
                    model.TransactionType = Type;
                    DataSet ds = model.CreateOrder();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                        {
                            model.Result = "Yes";
                            model.OrderNo = ds.Tables[0].Rows[0]["OrderNo"].ToString();
                            model.Message = "Order Created Successfully";
                        }
                        else
                        {
                            model.Result = "No";
                            model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        }
                    }
                    else
                    {
                        model.Result = "No";
                        model.Message = "Temporarily issues occurred. Please Try after some time";
                    }
                }
                else
                {
                    model.Result = "No";
                    model.Message = "You have insufficient balance in your wallet for this plan. Kindly choose another plan";
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrepaidandDTHRecharge(string Amount, string OrderNo, string MobileNo, string state_code, string opid)
        {
            UserRecharge model = new UserRecharge();
            var client = new RestClient("https://www.kwikapi.com/api/v2/recharge.php?api_key=" + RechargeModel.APIKey + "&number=" + MobileNo + "&amount=" + Amount + "&opid=" + opid + "&state_code=" + state_code + "&order_id=" + OrderNo + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("api_key", RechargeModel.APIKey);
            request.AddParameter("number", MobileNo);
            IRestResponse response = client.Execute(request);
            JObject userObj = JObject.Parse(response.Content);
           
            if (userObj["status"].ToString() == "SUCCESS" || userObj["status"].ToString()== "PENDING" || userObj["status"].ToString() == "FAILED")
            {
                model.Amount = Convert.ToDecimal(Amount);
                model.OrderNo = OrderNo;
                model.FK_UserId = Session["Pk_UserId"].ToString();
                model.OperatorId = opid;
                model.CircleId = state_code;
                model.TransactionFor = MobileNo;
                model.Provider = userObj["provider"].ToString();
                model.ChargedAmount = Convert.ToDecimal(userObj["charged_amount"]);
                model.ServerOrderId = userObj["order_id"].ToString();
                model.Opr_Id = userObj["opr_id"].ToString();
                model.Status = userObj["status"].ToString();
                model.Message = userObj["message"].ToString();
                DataSet ds = model.SaveBillPaymentResponse();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        model.Message = "Recharge done successfully";
                        model.Result = "Yes";
                    }
                    else
                    {
                        model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        model.Result = "No";
                    }
                }
                else
                {
                    model.Message = "Some issues occurred";
                    model.Result = "No";
                }
            }
            else
            {
                model.Message = "Temporarily issues occurred. Please try after some times.";
                model.Result = "No";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UtilityPayments(string Amount, string OrderNo, string number,  string opid,string MobileNo,string opt1,string opt2,string opt3,string opt4,string opt5, string opt6, string opt7, string opt8, string opt9, string opt10, string refrence_id )
        {
            UserRecharge model = new UserRecharge();
            var client = new RestClient("https://www.kwikapi.com/api/v2/bills/payments.php?api_key="+RechargeModel.APIKey+"&number="+ number + "&amount="+ Amount + "&opid="+ opid + "&order_id="+ OrderNo + "&opt1="+ opt1 + "&opt2="+ opt2 + "&opt3="+ opt3 + "&opt4="+ opt4 + "&opt5="+ opt5 + "&opt6="+ opt6 + "&opt7="+ opt7 + "&opt8="+ opt8 + "&opt9="+ opt9 + "&opt10="+ opt10 + "&refrence_id="+ refrence_id + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject userObj = JObject.Parse(response.Content);

            if (userObj["status"].ToString() == "SUCCESS" || userObj["status"].ToString() == "PENDING" || userObj["status"].ToString() == "FAILED")
            {
                model.Amount = Convert.ToDecimal(Amount);
                model.OrderNo = OrderNo;
                model.FK_UserId = Session["Pk_UserId"].ToString();
                model.OperatorId = opid;
                model.TransactionFor = MobileNo;
                model.Provider = userObj["provider"].ToString();
                model.ChargedAmount = Convert.ToDecimal(userObj["charged_amount"]);
                model.ServerOrderId = userObj["order_id"].ToString();
                model.Opr_Id = userObj["opr_id"].ToString();
                model.Status = userObj["status"].ToString();
                model.Message = userObj["message"].ToString();
                model.Opt1 = userObj["optional1"].ToString();
                model.Opt2 = userObj["optional2"].ToString();
                model.Opt3 = userObj["optional3"].ToString();
                model.Opt4 = userObj["optional4"].ToString();
                model.Opt5 = userObj["optional5"].ToString();
                model.Opt6 = userObj["optional6"].ToString();
                model.Opt7 = userObj["optional7"].ToString();
                model.Opt8 = userObj["optional8"].ToString();
                model.Opt9 = userObj["optional9"].ToString();
                model.Opt10 = userObj["optional10"].ToString();
                DataSet ds = model.SaveBillPaymentResponse();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        model.Message = "Recharge done successfully";
                        model.Result = "Yes";
                    }
                    else
                    {
                        model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        model.Result = "No";
                    }
                }
                else
                {
                    model.Message = "Some issues occurred";
                    model.Result = "No";
                }
            }
            else
            {
                model.Message = "Temporarily issues occurred. Please try after some times.";
                model.Result = "No";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOperatorDetails(string opid)
        {
            var client = new RestClient("https://www.kwikapi.com/api/v2/operatorFetch.php");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("api_key",RechargeModel.APIKey);
            request.AddParameter("opid", opid);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PostpaidRecharge(string Amount, string OrderNo, string MobileNo, string state_code, string opid)
        {
            UserRecharge model = new UserRecharge();
            var client = new RestClient("https://www.kwikapi.com/api/v2/bills/recharge.php?api_key=" + RechargeModel.APIKey + "&number=" + MobileNo + "&amount=" + Amount + "&opid=" + opid + "&order_id=" + OrderNo + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("api_key", RechargeModel.APIKey);
            request.AddParameter("number", MobileNo);
            IRestResponse response = client.Execute(request);
            JObject userObj = JObject.Parse(response.Content);

            if (userObj["status"].ToString() == "SUCCESS" || userObj["status"].ToString() == "PENDING" || userObj["status"].ToString() == "FAILED")
            {
                model.Amount = Convert.ToDecimal(Amount);
                model.OrderNo = OrderNo;
                model.FK_UserId = Session["Pk_UserId"].ToString();
                model.OperatorId = opid;
                model.CircleId = state_code;
                model.TransactionFor = MobileNo;
                model.Provider = userObj["provider"].ToString();
                model.ChargedAmount = Convert.ToDecimal(userObj["charged_amount"]);
                model.ServerOrderId = userObj["order_id"].ToString();
                model.Opr_Id = userObj["opr_id"].ToString();
                model.Status = userObj["status"].ToString();
                model.Message = userObj["message"].ToString();
                DataSet ds = model.SaveBillPaymentResponse();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        model.Message = "Recharge done successfully";
                        model.Result = "Yes";
                    }
                    else
                    {
                        model.Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        model.Result = "No";
                    }
                }
                else
                {
                    model.Message = "Some issues occurred";
                    model.Result = "No";
                }
            }
            else
            {
                model.Message = "Temporarily issues occurred. Please try after some times.";
                model.Result = "No";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}