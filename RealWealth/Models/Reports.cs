using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealWealth.Models
{
    public class Reports : Common
    {
        public string SiteId { get; set; }
        public string Total { get; set; }
        public string ReceiptNo { get; set; }
        public string DirectBusiness { get; set; }
        public string TotalBusiness { get; set; }
        public string RightBusiness { get; set; }
        public string LeftBusiness { get; set; }
        public List<Reports> lstDateWiseBusiness { get; set; }
        public List<Reports> lstassociatenew { get; set; }
        public string PK_LoginLogId { get; set; }
        public string IP { get; set; }
        public string LoginDateTime { get; set; }
        public string UserType { get; set; }
        public List<Reports> lstAssociateLoginLog { get; set; }
        public string SectorName { get; set; }
        public string SiteName { get; set; }
        public string BlockName { get; set; }
        public string LoginIDD { get; set; }
        public string UserName { get; set; }
        public string EncryptPayoutNo { get; set; }
        public string BatchNo { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string MRP { get; set; }
        public string TaxableAmount { get; set; }
        public string FinalAmount { get; set; }
        public string FK_InvestmentID { get; set; }
        public string MemberAccNo { get; set; }
        public string IFSCCode { get; set; }
        //public string BankName { get; set; }
        public string MaturityDate { get; set; }
        public string Description { get; set; }
        public string Pk_PaidBoosterId { get; set; }
        //public string TransactionNo { get; set; }
        public string EncryptName { get; set; }
        public string AchievementDate { get; set; }
        public bool IsDownline { get; set; }
        public string FromActivationDate { get; set; }
        public string EPinNo { get; set; }
        public string TransferDate { get; set; }
        public string ToActivationDate { get; set; }
        public string PAN { get; set; }
        //public string Leg { get; set; }
        public string LoginId { get; set; }
        public string PayoutLoginId { get; set; }
        public string Ids { get; set; }
        public string Name { get; set; }

        public string JoiningDate { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }
        public string CssClass { get; set; }
        public string SponsorId { get; set; }
        public string PayoutNo { get; set; }
        public string SponsorName { get; set; }
        public List<Reports> lstassociate { get; set; }
        public List<Reports> lstBusinessStatus { get; set; }

        public string PermanentDate { get; set; }

        public string Status { get; set; }
        public string FromLoginId { get; set; }

        public string FromUserName { get; set; }

        public string IncomeType { get; set; }

        public string Date { get; set; }

        public List<Reports> lstunpaidincomes { get; set; }

        //public string TransactionDate { get; set; }
        public string BankBranch { get; set; }

        public string FromName { get; set; }

        public string ToName { get; set; }

        public string ToLoginID { get; set; }

        public string ClosingDate { get; set; }

        public string BinaryIncome { get; set; }

        public string GrossAmount { get; set; }

        public string TDSAmount { get; set; }

        public string ProcessingFee { get; set; }

        public string NetAmount { get; set; }
        public string Password { get; set; }

        public string isBlocked { get; set; }

        public string PK_DocumentID { get; set; }

        public string DocumentNumber { get; set; }

        public string DocumentType { get; set; }

        public string DocumentImage { get; set; }

        public List<Reports> lsttopupreport { get; set; }
        public List<Reports> lsttopupreporttps { get; set; }
        public List<Reports> lstRewardList { get; set; }
        public List<Reports> lstAdvancePaymentReport { get; set; }

        public string UpgradtionDate { get; set; }

        //public string Package { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
        public string Level { get; set; }
        public string CurrentDate { get; set; }
        public string UsedFor { get; set; }
        public decimal Amount1 { get; set; }
        public decimal TDSAmount1 { get; set; }
        public decimal GrossAmount1 { get; set; }
        public string TopupBy { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<Reports> lstpermission { get; set; }
        public string FormName { get; set; }
        public string FormType { get; set; }
        
        public string LastTopUpAmount { get; set; }
        public bool IsNewBusiness { get; set; }
        public string LastTopUpDate { get; set; }
        public string BusinessType { get; set; }
        public bool IsInclude { get; set; }
        public string Reward { get; set; }
        public string DirectStatus { get; set; }
        public string Lvl { get; set; }


        public List<Reports> lstDefaultAssociateList { get; set; }

        public DataSet GetPayoutReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginID", ToLoginID),
                                    new SqlParameter("@PayoutNo", PayoutNo),
                                    new SqlParameter("@Name", Name),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                    new SqlParameter("@IsDownline", IsDownline),
                                    new SqlParameter("@Leg", Leg) };


            DataSet ds = DBHelper.ExecuteQuery("PayoutReportForMember", para);
            return ds;
        }

        public DataSet AssociateLoginLogList()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                new SqlParameter("@Name", Name)
            };
            DataSet ds = DBHelper.ExecuteQuery("AssociateLoginLogList", para);
            return ds;
        }
        public DataSet GetProductPayoutReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@PayoutNo", PayoutNo),
                                    new SqlParameter("@Name", Name),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                    new SqlParameter("@IsDownline", IsDownline),
                                    new SqlParameter("@Leg", Leg) };
            DataSet ds = DBHelper.ExecuteQuery("GetProductPayoutReport", para);
            return ds;
        }
        public DataSet GetIncomeReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@ToLoginId", ToLoginID),
                                    new SqlParameter("@FromName", FromName),
                                    new SqlParameter("@ToName", ToName),
                                    new SqlParameter("@IncomeType", IncomeType),
                                    new SqlParameter("@Status", Status),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("IncomeReport", para);
            return ds;
        }




        public DataSet GetProductIncomeReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@ToLoginId", ToLoginID),
                                    new SqlParameter("@FromName", FromName),
                                    new SqlParameter("@ToName", ToName),
                                    new SqlParameter("@IncomeType", IncomeType),
                                    new SqlParameter("@Status", Status),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("ProductIncomeReport", para);
            return ds;
        }

        public DataSet GetDatebusinessdetails()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@LoginId", ToLoginID),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                       new SqlParameter("@IsDownline", IsDownline)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetBusinessDetails", para);
            return ds;
        }
        public DataSet GetAssociateList()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@Name", Name),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                    new SqlParameter("@SponsorID", SponsorId),
                                    new SqlParameter("@SponsorName", SponsorName),
                                    new SqlParameter("@Status", Status),
                                    new SqlParameter("@IsDownline", IsDownline),
                                    new SqlParameter("@Leg", Leg)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetAssociateList", para);
            return ds;
        }
        public DataSet AssociateListForKYC()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                      new SqlParameter("@Status", Status)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetAgentListForKYC", para);
            return ds;
        }

        public DataSet GetDirectList()
        {

            SqlParameter[] para = {
                                    new SqlParameter("@PK_UserIds", Ids),
                                    new SqlParameter("@FK_UserId", Fk_UserId),
                                    new SqlParameter("@LoginId", FromLoginId),
                                    new SqlParameter("@Name", Name),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                    new SqlParameter("@FromActivationDate", FromActivationDate),
                                    new SqlParameter("@ToActivationDate", ToActivationDate),
                                    new SqlParameter("@Leg", Leg),
                                    new SqlParameter("@Status", Status),
                                       new SqlParameter("@DirectStatus", DirectStatus),
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetDirectList", para);
            return ds;
        }

        public DataSet GetDownlineList()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@Name", Name),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                    new SqlParameter("@Leg", Leg),
                                    new SqlParameter("@Status", Status), };
            DataSet ds = DBHelper.ExecuteQuery("DownlineList", para);
            return ds;
        }
        public DataSet ApproveKYC()
        {
            SqlParameter[] para = { new SqlParameter("@LoginID", LoginId),
                                      new SqlParameter("@PK_DocumentID", PK_DocumentID),
                                      new SqlParameter("@DocumentType", DocumentType),
                                      new SqlParameter("@Status", Status),
                                      new SqlParameter("@UpdatedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("ApproveKYC", para);
            return ds;
        }

        public DataSet GetTopupReport()
        {
            SqlParameter[] para = {   new SqlParameter("@LoginID", LoginId),
                                      new SqlParameter("@Name", Name),
                                      new SqlParameter("@FromDate", FromDate),
                                      new SqlParameter("@ToDate", ToDate),
                                      new SqlParameter("@Package", Package),
                                      new SqlParameter("@SiteId", SiteId),
                                      new SqlParameter("@ClaculationStatus", Status),
                                      new SqlParameter("@Fk_BusinessId", BusinessType)
                                  };

            DataSet ds = DBHelper.ExecuteQuery("GetTopupreport", para);
            return ds;
        }
        public DataSet GetTopupreportProduct()
        {
            SqlParameter[] para = {   new SqlParameter("@LoginID", LoginId),
                                      new SqlParameter("@Name", Name),
                                      new SqlParameter("@FromDate", FromDate),
                                      new SqlParameter("@ToDate", ToDate),
                                      new SqlParameter("@Package", Package),
                                      new SqlParameter("@ClaculationStatus", Status),
                                  };

            DataSet ds = DBHelper.ExecuteQuery("GetTopupreportProduct", para);
            return ds;
        }
        public DataSet GetTopupReportByKit()
        {
            SqlParameter[] para = {   new SqlParameter("@LoginID", LoginId),
                                      new SqlParameter("@Name", Name),
                                      new SqlParameter("@FromDate", FromDate),
                                      new SqlParameter("@ToDate", ToDate),
                                      new SqlParameter("@Package", Package),
                                      new SqlParameter("@ClaculationStatus", Status),
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetTopupreportByKit", para);
            return ds;
        }

        public List<Reports> lsttransactionlog { get; set; }
        public List<Reports> lstVoucherLedger { get; set; }
        public string Action { get; set; }

        public string Remarks { get; set; }

        public DataSet GetTransactionLog()
        {
            SqlParameter[] para = {     new SqlParameter("@Action", Action),
                                        new SqlParameter("@FromDate", FromDate),
                                        new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetTransactionLog", para);
            return ds;
        }

        public DataSet GetUnPaidIncomes()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId) };
            DataSet ds = DBHelper.ExecuteQuery("GetUnPaidIncomes", para);
            return ds;
        }
        public DataSet GetUnPaidProductIncomes()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId) };
            DataSet ds = DBHelper.ExecuteQuery("GetUnPaidProductIncomes", para);
            return ds;
        }
        public DataSet GetBoosterLedger()
        {
            SqlParameter[] para = { new SqlParameter("@FK_UserID", Fk_UserId) };
            DataSet ds = DBHelper.ExecuteQuery("BoosterLedger", para);
            return ds;
        }
        public DataSet GetBoosterAchieverForAssociate()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                   new SqlParameter("@FromDate", FromDate),
                                  new SqlParameter("@ToDate", ToDate),};
            DataSet ds = DBHelper.ExecuteQuery("GetBoosterAchieverForAssociate", para);
            return ds;
        }
        public DataSet GetPrintData()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                   new SqlParameter("@PrintingDate", PrintingDate)};
            DataSet ds = DBHelper.ExecuteQuery("GetPrintData", para);
            return ds;
        }

        public string PrintingDate { get; set; }
        public string PlotNumber { get; set; }

        public string ProductName { get; set; }
        public string HSNCode { get; set; }
        public string Fk_FormId { get; set; }
        public string Fk_FormTypeId { get; set; }

        public DataSet GetBoosterAchieverCurrent()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),};
            DataSet ds = DBHelper.ExecuteQuery("GetBoosterAchieverCurrent", para);
            return ds;
        }
        public DataSet GetPayBoosterAchieverCurrent()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate ),
                                    new SqlParameter("@IsDownline", IsDownline),
                                    new SqlParameter("@Leg", Leg),};
            DataSet ds = DBHelper.ExecuteQuery("GetPayBoosterAchieverCurrent", para);
            return ds;
        }

        public DataSet PayBooster()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@Pk_PaidBoosterId", Pk_PaidBoosterId) ,
                                      new SqlParameter("@TransactionNo", TransactionNo),
                                      new SqlParameter("@TransactionDate", TransactionDate) ,
                                      new SqlParameter("@Description", Description) ,
                                      new SqlParameter("@AddedBy", AddedBy)

                                  };
            DataSet ds = DBHelper.ExecuteQuery("PayBoosterAchiever", para);
            return ds;
        }
        #region IncomeStatement
        public DataSet FetchNextDate()
        {
            DataSet ds = DBHelper.ExecuteQuery("SelectDateForDailyIncomeNew");
            return ds;
        }
        public DataSet GenerateDailyIncomeNew()
        {
            SqlParameter[] para = { new SqlParameter("@Date", ClosingDate) };
            DataSet ds = DBHelper.ExecuteQuery("_AutoGenerateIncomeDateWise", para);
            return ds;
        }

        public string DirectIncome { get; set; }
        public DataSet GetDailyIncomeNewReport()
        {
            //SqlParameter[] para = { new SqlParameter("@Date", ClosingDate) };
            DataSet ds = DBHelper.ExecuteQuery("GetDailyIncomeReportNew");
            return ds;
        }
        public DataSet PublishDailyIncome()
        {
            SqlParameter[] para = { new SqlParameter("@PublishDate", ClosingDate),
                                      new SqlParameter("@PublishBy", Fk_UserId) };
            DataSet ds = DBHelper.ExecuteQuery("PublishDailyIncome", para);
            return ds;
        }
        public DataSet GetDailyIncomeReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId) ,
                                  new SqlParameter("@FromDate", FromDate),
                                  new SqlParameter("@ToDate", ToDate)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetDailyIncome", para);
            return ds;
        }
        #endregion
        public DataSet DailyIncomeReport()
        {
            SqlParameter[] para = { new SqlParameter("@Fk_ToId", Fk_UserId) ,
                                    new SqlParameter("@FromDate", FromDate) ,
                                    new SqlParameter("@ToDate", ToDate) ,

                                  };
            DataSet ds = DBHelper.ExecuteQuery("DailyIncomeReportForAssociate", para);
            return ds;
        }
        public string ProductWallet { get; set; }
        public string LeadershipBonus { get; set; }
        public string RewardID { get; set; }
        public string QualifyDate { get; set; }
        public string RewardImage { get; set; }
        public string RewardName { get; set; }
        public string Contact { get; set; }
        public string PK_RewardItemId { get; set; }
        public DataSet GettingUserProfile()
        {
            SqlParameter[] para = {
                                        new SqlParameter("@LoginId", LoginId)};
            DataSet ds = DBHelper.ExecuteQuery("GetUserProfile", para);
            return ds;
        }

        public DataSet RewardList()
        {
            SqlParameter[] para = {
                                        new SqlParameter("@Fk_RewardId", RewardID),
                                        new SqlParameter("@FK_UserId", Fk_UserId)};
            DataSet ds = DBHelper.ExecuteQuery("_GetRewardData", para);
            return ds;
        }
        public DataSet ClaimReward()
        {
            SqlParameter[] para = {
                                        new SqlParameter("@Fk_RewardItemId", PK_RewardItemId),
                                        new SqlParameter("@FK_UserId", Fk_UserId),
                                        new SqlParameter("@Status", Status),
            };
            DataSet ds = DBHelper.ExecuteQuery("ClaimReward", para);
            return ds;
        }

        public DataSet GetTDSReport()
        {
            SqlParameter[] para = { new SqlParameter("@PanNumber", PAN) ,
                                  new SqlParameter("@FromDate", FromDate),
                                  new SqlParameter("@ToDate", ToDate)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetTDSReport", para);
            return ds;
        }

        public DataSet GetTDSReportByLoginID()
        {
            SqlParameter[] para = { new SqlParameter("@PanNumber", PAN) ,
                                  new SqlParameter("@FromDate", FromDate),
                                  new SqlParameter("@ToDate", ToDate),
                                  new SqlParameter("@LoginID", ToLoginID)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetTDSReportByLoginID", para);
            return ds;
        }
        public DataSet GetTransferReport()
        {
            SqlParameter[] para = { new SqlParameter("@FromLoginId", FromLoginId),
                                    new SqlParameter("@ToLoginId", ToLoginID) };
            DataSet ds = DBHelper.ExecuteQuery("GetPinTransferReport", para);
            return ds;
        }

        #region PayPayout
        public DataSet GetPayPayout()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@IsDownline", IsDownline),
                                    new SqlParameter("@Leg", Leg), };
            DataSet ds = DBHelper.ExecuteQuery("GetBalancePayoutforPayment", para);
            return ds;
        }

        public DataSet SavePayPayout()
        {
            SqlParameter[] para = { new SqlParameter("@Fk_UserId", Fk_UserId),
                                    new SqlParameter("@TransactionNo", TransactionNo),
                                    new SqlParameter("@TransactionDate", TransactionDate),
                                    new SqlParameter("@Amount", Amount),
                                    new SqlParameter("@AddedBy", AddedBy),
                                    new SqlParameter("@BankName", BankName),
                                    new SqlParameter("@BankBranch", BankBranch),
                                    new SqlParameter("@PaymentMode", PaymentMode),
                                    new SqlParameter("@Remarks", Remarks),
            };
            DataSet ds = DBHelper.ExecuteQuery("PayPayout", para);
            return ds;
        }
        #endregion
        public DataSet PrintTopUp()
        {
            SqlParameter[] para = { new SqlParameter("@Pk_InvestmentId", ToLoginID), };
            DataSet ds = DBHelper.ExecuteQuery("PrintTopUpReport", para);
            return ds;
        }

        public DataSet AdvancePaymentReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate), };
            DataSet ds = DBHelper.ExecuteQuery("AdvancePaymentReport", para);
            return ds;
        }
        public DataSet BusinessReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                     new SqlParameter("@Leg", Leg),
                                    new SqlParameter("@IsDownline", IsDownline)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetBusiness", para);
            return ds;
        }
        public DataSet ProductBusinessReport()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),

                                     new SqlParameter("@Leg", Leg),
                                    new SqlParameter("@IsDownline", IsDownline),

            };
            DataSet ds = DBHelper.ExecuteQuery("GetProductBusiness", para);
            return ds;
        }
        public DataSet UserProductReward()
        {
            SqlParameter[] para = {

                                        new SqlParameter("@FK_UserId", Fk_UserId)};
            DataSet ds = DBHelper.ExecuteQuery("_GetProductRewardData", para);
            return ds;
        }
        public DataSet ClaimProductReward()
        {
            SqlParameter[] para = {
                                        new SqlParameter("@Fk_RewardItemId", PK_RewardItemId),
                                        new SqlParameter("@FK_UserId", Fk_UserId),
                                        new SqlParameter("@Status", Status),
            };
            DataSet ds = DBHelper.ExecuteQuery("ClaimProductReward", para);
            return ds;
        }

        #region productbooster
        public DataSet ProductBoosterAchieverDetails()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),};
            DataSet ds = DBHelper.ExecuteQuery("GetProductBoosterAchiever", para);
            return ds;
        }

        public DataSet GetPayProductBoosterAchiever()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate ),
                                    new SqlParameter("@IsDownline", IsDownline),
                                    new SqlParameter("@Leg", Leg),};
            DataSet ds = DBHelper.ExecuteQuery("GetPayProductBoosterAchiever", para);
            return ds;
        }
        #endregion
        public DataSet ProductRewardList()
        {

            DataSet ds = DBHelper.ExecuteQuery("RewardList");
            return ds;
        }
        public DataSet RewardListForWebsite()
        {

            DataSet ds = DBHelper.ExecuteQuery("RewardListForWebsite");
            return ds;
        }
        public DataSet RewardListForAchiever()
        {
            SqlParameter[] para = { new SqlParameter("@LoginID", LoginId),
                                    new SqlParameter("@FK_RewardId", RewardID), }
                                    ;
            DataSet ds = DBHelper.ExecuteQuery("RewardListForAchiever", para);
            return ds;
        }
        public DataSet ApprovePayment()
        {
            SqlParameter[] para = { new SqlParameter("@PKID", PK_RewardItemId),
                                    new SqlParameter("@TxnNo", TransactionNo),
                                    new SqlParameter("@TxnDate", TransactionDate),
                                    new SqlParameter("@PaidDate", PaymentDate),
                                    new SqlParameter("@AddedBy", AddedBy),


            }
                                    ;
            DataSet ds = DBHelper.ExecuteQuery("ApproveReward", para);
            return ds;
        }

        public DataSet GettingUserDetails()
        {

            DataSet ds = DBHelper.ExecuteQuery("GetEmployeeDetails");
            return ds;
        }

        public DataSet GettingUserFormPermission()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@FK_UserId", Fk_UserId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetUserFormPermission", para);
            return ds;
        }
        public DataSet GetpayoutByAmount()
        {
            SqlParameter[] para = { new SqlParameter("@LoginID", PayoutLoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("PayoutReportForAmount", para);
            return ds;
        }




        public DataSet GetDefaulterList()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@LoginId", LoginId),
                                       new SqlParameter("@Status",Status),
                                        new SqlParameter("@IsDownline", IsDownline),
                                          new SqlParameter("@FromDate", FromDate),
                                      new SqlParameter("@ToDate", ToDate)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetdefaulterList", para);
            return ds;
        }


        public DataSet GetBusinessStatus()
        {
            SqlParameter[] para = {   new SqlParameter("@LoginID", LoginId),
                                      new SqlParameter("@Name", Name),
                                      new SqlParameter("@FromDate", FromDate),
                                      new SqlParameter("@ToDate", ToDate),
                                      new SqlParameter("@Package", Package),
                                      new SqlParameter("@SiteId", SiteId),
                                      new SqlParameter("@ClaculationStatus", Status),
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetTopupreportForUpdateBusinessStatus", para);
            return ds;
        }
        public DataSet UpdateBusinessStatus()
        {
            SqlParameter[] para = { new SqlParameter("@PK_InvestmentId", ToLoginID),
                                    new SqlParameter("@IsNewBusiness", IsNewBusiness),
                                    new SqlParameter("@IsInclude", IsInclude),
                                    new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateBusinessStatus", para);
            return ds;
        }


        public DataSet GetRewardIncludedDetails()
        {
            SqlParameter[] para = {
                                       new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@ToLoginId", ToLoginID),
                                    new SqlParameter("@FromName", FromName),
                                    new SqlParameter("@ToName", ToName),
                                    new SqlParameter("@IncomeType", IncomeType),
                                   new SqlParameter("@IsReward", Reward),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetRewardIncludedDetails", para);
            return ds;
        }
        public DataSet GetTPPAmountById()
        {
            SqlParameter[] para = {
                new SqlParameter("@LoginID", LoginId),
                new SqlParameter("@ClosingDate",ClosingDate)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetTTPIncomeForDistributePaymentById", para);
            return ds;
        }
        public DataSet GetTPSAmountById()
        {
            SqlParameter[] para = {
                new SqlParameter("@LoginID", LoginId),
                new SqlParameter("@ClosingDate",ClosingDate)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetTPSIncomeForDistributePaymentById", para);
            return ds;
        }
        public DataSet GetPaidPayoutDetailsByAmount()
        {
            SqlParameter[] para = { new SqlParameter("@Fk_UserId", Fk_UserId),
                            new SqlParameter("@PayoutNo", PayoutNo)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPayoutReportByID", para);
            return ds;
        }
    }
}

