using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealWealth.Models
{
    public class Profile : Common
    {
        public string LoginIDD { get; set; }
        public string UserName { get; set; }
        public string EncryptLoginID { get; set; }
        public string EncryptPayoutNo { get; set; }
        public string RealtionName { get; set; }
        public string LoginId { get; set; }
        public string ProductID { get; set; }
        public string Gender { get; set; }
        public string Relation { get; set; }
        public string PK_UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JoiningDate { get; set; }
        public string Mobile { get; set; }
        public string EmailId { get; set; }
        public List<Profile> profilelst { get; set; }
        public string SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string BankBranch { get; set; }
        public string IFSC { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string LeadershipBonus { get; set; }
        public string AccountHolder { get; set; }
        public string AdharNo { get; set; }
        public string cssStatus { get; set; }


        public DataSet GetUserProfile()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId) };
            DataSet ds = DBHelper.ExecuteQuery("UserProfile", para);
            return ds;
        }
     
        public DataSet GettingUserProfile()
        {
            SqlParameter[] para = {
                                        new SqlParameter("@LoginId", LoginId)};
            DataSet ds = DBHelper.ExecuteQuery("GetUserProfile", para);
            return ds;
        }
        public string PayoutNo { get; set; }
        public string ClosingDate { get; set; }
        public string BinaryIncome { get; set; }
        public string GrossAmount { get; set; }
        public string TDSAmount { get; set; }
        public string ProcessingFee { get; set; }
        public string NetAmount { get; set; }
        public string ProductWallet { get; set; }
        public List<Profile> lstPayoutDetail { get; set; }
        public string PanNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DirectIncome { get; set; }
        public DataSet BlockAssociate()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                  new SqlParameter("@BlockedBy", UpdatedBy)};
            DataSet ds = DBHelper.ExecuteQuery("BlockAssociate", para);
            return ds;
        }
        public DataSet UnblockAssociate()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                  new SqlParameter("@BlockedBy", UpdatedBy)};
            DataSet ds = DBHelper.ExecuteQuery("UnblockAssociate", para);
            return ds;
        }
        public DataSet DeactivateUserByAdmin()
        {
            SqlParameter[] para = { new SqlParameter("@LoginID", LoginId),
                                   new SqlParameter("@UpdatedBy", UpdatedBy) };
            DataSet ds = DBHelper.ExecuteQuery("DeactivateUser", para);
            return ds;
        }
        public DataSet ActivateUserByAdmin()
        {
            SqlParameter[] para = { new SqlParameter("@FK_UserID", Fk_UserId),
                                    new SqlParameter("@FK_ProductID", ProductID),
                                    new SqlParameter("@UpdatedBy", UpdatedBy)};
            DataSet ds = DBHelper.ExecuteQuery("ActivateUserByAdmin", para);
            return ds;
        }

    }
}