using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealWealth.Models
{
    public class RechargeModel
    {
        public static string APIKey = "be2609-04f9b3-b9e057-2aa5a1-f7bd1c";
    }
    public class UserRecharge
    {
        public string FK_UserId { get; set; }
        public string TransactionFor { get; set; }
        public decimal Amount { get; set; }
        public string OrderNo { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public string OperatorId { get; set; }
        public string CircleId { get; set; }
        public string Provider { get; set; }
        public decimal ChargedAmount { get; set; }
        public string ServerOrderId { get; set; }
        public string Opr_Id { get; set; }
        public string Opt1 { get; set; }
        public string Opt2 { get; set; }
        public string Opt3 { get; set; }
        public string Opt4 { get; set; }
        public string Opt5 { get; set; }
        public string Opt6 { get; set; }
        public string Opt7 { get; set; }
        public string Opt8 { get; set; }
        public string Opt9 { get; set; }
        public string Opt10 { get; set; }
        public DataSet GetWalletBalance()
        {
            SqlParameter[] para = { new SqlParameter("@PK_USerID", FK_UserId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetWalletBalance", para);
            return ds;
        }
        public DataSet CreateOrder()
        {
            SqlParameter[] para = { new SqlParameter("@FK_UserId", FK_UserId),
                 new SqlParameter("@TransactionFor", TransactionFor),
                new SqlParameter("@OrderType", TransactionType),
                new SqlParameter("@Amount", Amount),
            };
            DataSet ds = DBHelper.ExecuteQuery("CreateOrderForRecharge", para);
            return ds;
        }
        public DataSet SaveBillPaymentResponse()
        {
            SqlParameter[] para = { new SqlParameter("@OrderNo", OrderNo),
                new SqlParameter("@TransactionFor", TransactionFor),
                new SqlParameter("@FK_UserId", FK_UserId),
                new SqlParameter("@Amount", Amount),
                new SqlParameter("@Status", Status),
                new SqlParameter("@CircleId", CircleId),
                new SqlParameter("@OperatorId", OperatorId),
                new SqlParameter("@Provider",Provider),
                new SqlParameter("@ChargedAmount",ChargedAmount),
                new SqlParameter("@Message",Message),
                new SqlParameter("@ServerOrderId",ServerOrderId),
                new SqlParameter("@Opr_Id",Opr_Id),
                new SqlParameter("@Opt1",Opt1),
                new SqlParameter("@Opt2",Opt2),
                new SqlParameter("@Opt3",Opt3),
                new SqlParameter("@Opt4",Opt4),
                new SqlParameter("@Opt5",Opt5),
                new SqlParameter("@Opt6",Opt6),
                new SqlParameter("@Opt7",Opt7),
                new SqlParameter("@Opt8",Opt8),
                new SqlParameter("@Opt9",Opt9),
                new SqlParameter("@Opt10",Opt10),
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveBillPaymentResponse", para);
            return ds;
        }
    }
    public class BillPayment
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<BillPayment> lst { get; set; }
        public DataSet GetBillPayment()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetBillPaymentMaster");
            return ds;
        }
    }
    public class UserRechargeAPI
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string OrderNo { get; set; }
    }
}