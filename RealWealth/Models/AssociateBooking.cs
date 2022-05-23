using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealWealth.Models
{
    public class AssociateBooking : Common
    {
        #region Properties
        public List<AssociateBooking> ClosingWisePayoutlist { get; set; }
        public string SMS { get; set; }
        public string SenderID { get; set; }
        public string Details { get; set; }
        public string SenderAccountNo { get; set; }
        public string SenderName { get; set; }
        public string CommPercentage { get; set; }
        public string PK_PaidPayoutId { get; set; }
        public string Fk_SponsorId { get; set; }
        public string ActiveStatus { get; set; }
        public string ProfilePic { get; set; }
        public string PK_BookingId { get; set; }
        public string UserID { get; set; }
        public string BranchID { get; set; }
        public string BranchName { get; set; }
        public string PlotID { get; set; }
        public string PlotNumber { get; set; }
        public string CustomerID { get; set; }
        public string CustomerLoginID { get; set; }
        public string CustomerName { get; set; }
        public string AssociateID { get; set; }
        public string AssociateLoginID { get; set; }
        public string AssociateName { get; set; }
        public string SiteID { get; set; }
        public string SectorID { get; set; }
        public string BlockID { get; set; }
        public string PlotAmount { get; set; }
        public string NetPlotAmount { get; set; }
        public string PK_PLCCharge { get; set; }
        public string PlotRate { get; set; }
        public string PaymentPlanID { get; set; }
        public string BookingAmount { get; set; }
        public string PayAmount { get; set; }
        public string Discount { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionDate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string Remark { get; set; }
        public string TotalPLC { get; set; }
        public string LoginId { get; set; }
        public List<SelectListItem> lstBlock { get; set; }
        public List<SelectListItem> ddlSite { get; set; }
        public List<SelectListItem> ddlSector { get; set; }
        public string BookingDate { get; set; }
        public string ActualPlotRate { get; set; }
        public string DevelopmentCharge { get; set; }
        public List<AssociateBooking> lstPlot { get; set; }
        public string BookingStatus { get; set; }
        public string SiteTypeID { get; set; }
        public string BlockName { get; set; }
        public string SectorName { get; set; }
        public string ColorCSS { get; set; }
        public string Rate { get; set; }
        public string SiteName { get; set; }
        public string PLCCharge { get; set; }
        public string PLCName { get; set; }
        public string PLCID { get; set; }
        public List<AssociateBooking> lstPLC { get; set; }
        public List<AssociateBooking> lstVistor { get; set; }
        public string VisitDate { get; set; }
        public string EncryptKey { get; set; }
        public string VisitorId { get; set; }
        public string Mobile { get; set; }
        public string PK_VisitorId { get; set; }
        public string FK_RootId { get; set; }
        public string ActivationDate { get; set; }
        public string Lvl { get; set; }
        #endregion
        public DataSet List()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@PK_BookingId", PK_BookingId),
                                     new SqlParameter("@AssociateID", AssociateID),
                                     new SqlParameter("@CustomerLoginID",CustomerID),
                                     new SqlParameter("@Loginid",LoginId),
                                    new SqlParameter("@CustomerName", CustomerName)   ,
                                    new SqlParameter("@PK_SiteID", SiteID)   ,
                                    new SqlParameter("@PK_SectorID", SectorID)   ,
                                    new SqlParameter("@PK_BlockID", BlockID)   ,
                                    new SqlParameter("@PlotNumber", PlotNumber)   ,
                                    new SqlParameter("@BookingNumber", BookingNumber)   ,
                                    new SqlParameter("@FromDate", FromDate)   ,
                                    new SqlParameter("@ToDate", ToDate)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetPlotBookingForAssociate", para);
            return ds;
        }
        public DataSet GetDownlineDetails()
        {
            SqlParameter[] para = {

                                      new SqlParameter("@LoginId", LoginId) };
            DataSet ds = DBHelper.ExecuteQuery("GetDownlineAssociateDetails", para);
            return ds;
        }
        public string DesignationName { get; set; }
        public string Percentage { get; set; }
        public string PK_RewardItemId { get; set; }
        public string Contact { get; set; }
        public string BookingNumber { get; set; }
        public string PaidAmount { get; set; }
        public string PlanName { get; set; }
        public string TotalAllotmentAmount { get; set; }
        public string PaidAllotmentAmount { get; set; }
        public string BalanceAllotmentAmount { get; set; }
        public string TotalInstallment { get; set; }
        public string InstallmentAmount { get; set; }
        public string PlotArea { get; set; }
        public string Balance { get; set; }
        public string PK_BookingDetailsId { get; set; }
        public string InstallmentNo { get; set; }
        public string InstallmentDate { get; set; }
        public string DueAmount { get; set; }
        public DataSet FillDetails()
        {
            SqlParameter[] para =
                            {
                                 new SqlParameter("@BookingNo",BookingNumber),
                                  new SqlParameter("@LoginId",LoginId),

                                   new SqlParameter("@FK_SiteID",SiteID),
                                    new SqlParameter("@FK_SectorID",SectorID),
                                     new SqlParameter("@FK_BlockID",BlockID),
                                      new SqlParameter("@PlotNumber",PlotNumber)


                            };
            DataSet ds = DBHelper.ExecuteQuery("GetLedgerDetailsForAssociate", para);
            return ds;
        }
        public DataSet GetBookingDetailsList()
        {
            SqlParameter[] para = {

                                      new SqlParameter("@PK_BookingId", PK_BookingId),
                                        new SqlParameter("@CustomerID", CustomerID),
                                          new SqlParameter("@AssociateID", AssociateID)

                                  };

            DataSet ds = DBHelper.ExecuteQuery("GetPlotBooking", para);
            return ds;
        }

        public DataSet GetDetails()
        {
            SqlParameter[] para = {
                                        new SqlParameter("@LoginId", LoginId) };
            DataSet ds = DBHelper.ExecuteQuery("GetUplineAssociateDetails", para);
            return ds;
        }
        public DataSet UpdatePassword()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@OldPassword", Password) ,
                                      new SqlParameter("@NewPassword", NewPassword) ,
                                      new SqlParameter("@UpdatedBy", UpdatedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("ChangePasswordAssociate", para);
            return ds;
        }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string Total { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string TotalBooking { get; set; }
        public string Month { get; set; }
        public DataSet GetDetailsForBarGraph()
        {
            SqlParameter[] para = {

                                      new SqlParameter("@Fk_AssociateId", AssociateID)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetPlotBookingForGraphOnAssociateDashboard", para);
            return ds;
        }
        public List<AssociateBooking> dataList3 { get; set; }
        public List<AssociateBooking> ListInstallment { get; set; }

        public DataSet GetDueInstallmentList()
        {
            SqlParameter[] para = {

                                      new SqlParameter("@Fk_AssociateId", AssociateID)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetDueInstallmentForAssociateDashboard", para);
            return ds;
        }
        public DataSet BindGraphDetails()
        {
            SqlParameter[] para = {

                                      new SqlParameter("@LoginId", LoginId)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("PlotDetailsOnGraphForAssociateDashboard", para);
            return ds;
        }
        public string SponsorID { get; set; }
        public string SponsorName { get; set; }
        public string DesignationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PanNo { get; set; }
        public string Address { get; set; }
        public string ActionStatus { get; set; }
        public string NewsFor { get; set; }

        #region EditProfile
        public DataSet GetList()
        {
            SqlParameter[] para = {
                                     new SqlParameter("@PK_UserId", UserID) ,
                                      new SqlParameter("@AssociateId", LoginId)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetAssociateDetailsForEditProfile", para);
            return ds;
        }
        public string PK_NewsID { get; set; }
        public string NewsHeading { get; set; }
        public string NewsBody { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataSet GetSiteList()
        {
            DataSet ds = DBHelper.ExecuteQuery("SiteList");
            return ds;
        }
        public DataSet GetDownlineTree()
        {
            SqlParameter[] para = {
                                        new SqlParameter("@Pk_UserId", Fk_UserId),
                                          new SqlParameter("@LoginId", LoginId),
                                            new SqlParameter("@Fk_RootId", FK_RootId),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetAssociateDownlineTree", para);
            return ds;
        }
        public string Fk_RewardID { get; set; }
        public string RewardID { get; set; }
        public string QualifyDate { get; set; }
        public string Target { get; set; }
        public string RewardName { get; set; }
        public string RewardImage { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public string ToID { get; set; }
        public string ToName { get; set; }
        public string Amount { get; set; }
        public string DifferencePercentage { get; set; }
        public string Income { get; set; }
        public string PayoutWalletID { get; set; }
        public string Narration { get; set; }
        public string Credit { get; set; }
        public string TType { get; set; }
        public string Debit { get; set; }
        public string PayOutNo { get; set; }
        public string ClosingDate { get; set; }
        public string GrossAmount { get; set; }
        public string TDS { get; set; }
        public string Processing { get; set; }
        public string NetAmount { get; set; }
        public string RequestID { get; set; }
        public string Action { get; set; }
        public string AdharNumber { get; set; }
        public string AdharImage { get; set; }
        public string AdharStatus { get; set; }
        public string PanNumber { get; set; }
        public string PanImage { get; set; }
        public string PanStatus { get; set; }
        public string DocumentImage { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentStatus { get; set; }
        public string PK_DocumentID { get; set; }
        public string AccountStatus { get; set; }
        public string DocumentType { get; set; }
        public string BankAccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string AccountHolderName { get; set; }
        public string PlotStatus { get; set; }
    }
}
#endregion


