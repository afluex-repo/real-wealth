using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealWealth.Models.DAL
{

    public class AgentModel
    {
        public string Fk_MemID { get; set; }
        public string SessionId { get; set; }
        public string LoginId { get; set; }
        public string ParentId { get; set; }
        public string ParentLoginId { get; set; }
        public string Leg { get; set; }
        public string Status { get; set; }
        public string MemberName { get; set; }
        public string CssClass { get; set; }
        public string SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string SelfBusiness { get; set; }
        public string TeamBusiness { get; set; }
        public string BlockStatus { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductAmount { get; set; }
        public string DisplayName { get; set; }
        public string BookingAmtPt { get; set; }
        public string JoiningDate { get; set; }
        //-------------for Search User---------//
        public string prefixText { get; set; }
        public int count { get; set; }
        public string contextKey { get; set; }

    }

    public class AgentIncomes
    {
        public string FK_MemID { get; set; }
        public string FK_PlanId { get; set; }
        //Get:Incomes
        public string CurrentDate { get; set; }
        public string LoginID { get; set; }
        public string MemberName { get; set; }
        public string BusinessAmount { get; set; }
        public string programme { get; set; }
        public string GrossTotal { get; set; }
    }

    public class Login
    {
        public string LoginId { get; set; }
        public string Password { get; set; }

        public string PK_AdminId { get; set; }
        public string UserType { get; set; }
        public string BranchID { get; set; }
        public string FirstName { get; set; }
        public string FK_MemId { get; set; }
        public string MobileNo { get; set; }
        public string Tempermanent { get; set; }
        public string DisplayName { get; set; }
        public string PKFranchiseRegistrationId { get; set; }
        public string ContactDetail { get; set; }
        public string MemID { get; set; }
        public string MemberPhoto { get; set; }
        public string MemberFullName { get; set; }
        public string profilepic { get; set; }
        public string FK_UserTypeId { get; set; }



    }

    public class AutoCompletes
    {
        public string Id { get; set; }
        public string label { get; set; }
        public string value { get; set; }
    }

    public class FormPermission
    {
        public string FormName { get; set; }
        public string AdminId { get; set; }
        public string MemberID { get; set; }
    }

    public class FormPermissions
    {
        public string FormName { get; set; }
        public string AdminId { get; set; }
        public string MemberID { get; set; }
    }

    public class RecoverPassword
    {
        public string LoginId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string UpdatedBy { get; set; }
        public string FormName { get; set; }
        public string FK_UserTypeId { get; set; }
        public string FKMemId { get; set; }
    }

    public class GetTransactionPassword
    {
        public string LoginID { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

    }

    public class TPassword
    {
        public string LoginId { get; set; }
        public string TransactionPassword { get; set; }

    }

    public class Country
    {
        public List<Country> tblCountry { get; set; }
        public int PK_CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class GetState
    {
        public string FK_CountryId { get; set; }

    }

    public class UpdateMembersProfile
    {
        public string FK_CountryId { get; set; }
        public string FK_MemID { get; set; }
        public string I_Title { get; set; }
        public string I_FirstName { get; set; }
        public string I_MiddleName { get; set; }
        public string I_LastName { get; set; }
        public string I_Education { get; set; }
        public string I_FatherName { get; set; }
        public string I_DOB { get; set; }
        public string I_Gender { get; set; }
        public string I_MaritalStatus { get; set; }
        public string I_HusbandWifeName { get; set; }
        public string I_PCountryID { get; set; }
        public string I_PState { get; set; }
        public string I_PAddressLine1 { get; set; }
        public string I_PAddressLine2 { get; set; }
        public string I_PAddressLine3 { get; set; }
        public string I_PCity { get; set; }
        public string I_PPinCode { get; set; }
        public string I_PMobile { get; set; }
        public string I_PEmailID { get; set; }
        public string I_CCountryID { get; set; }
        public string I_CState { get; set; }
        public string I_CAddressLine1 { get; set; }
        public string I_CAddressLine2 { get; set; }
        public string I_CAddressLine3 { get; set; }
        public string I_CCity { get; set; }
        public string I_CPinCode { get; set; }
        public string I_CMobile { get; set; }
        public string I_CEmailID { get; set; }
        public string I_PANNo { get; set; }
        public string I_PANAckNo { get; set; }
        public string I_NomineeName { get; set; }
        public string I_NomineeOccupation { get; set; }
        public string I_NomineeAnualIncome { get; set; }
        public string I_NomineeDOB { get; set; }
        public string I_NomineeRelation { get; set; }
        public string I_Apointee { get; set; }
        public string I_BankAccountNo { get; set; }
        public string I_BankName { get; set; }
        public string I_BranchName { get; set; }
        public string I_AccountType { get; set; }
        public string I_MICRNo { get; set; }
        public string I_IFSCNo { get; set; }
        public string CreatedBy { get; set; }
        public string MemberId { get; set; }
        public string GaurdianRelation { get; set; }
        public string CancelledChequeAttach { get; set; }
        public string PanCardAttached { get; set; }
        public string AadharNo { get; set; }
        public string AdhaarCardAttach { get; set; }
        public string GSTNo { get; set; }
        public string EntityName { get; set; }
        public string EntityAddress { get; set; }
        public string EntityPhoneNo { get; set; }
        public string EntityEmailId { get; set; }

    }




    public class DownLineUserDetails
    {
        public string JoiningDate { get; set; }
        public string LoginId { get; set; }
        public string MemberId { get; set; }
        public string DisplayName { get; set; }
        public string TotalMemberLeft { get; set; }
        public string TotalMemberRight { get; set; }
        public string TotalMemberTotal { get; set; }

        public string ActiveLeft { get; set; }
        public string ActiveRight { get; set; }
        public string ActiveTotal { get; set; }

        public string NonActiveLeft { get; set; }
        public string NonActiveRight { get; set; }
        public string NonActiveTotal { get; set; }
        public string ProfilePic { get; set; }
        public string RightBusiness { get; set; }
        public string LeftBusiness { get; set; }
        public string TotalBusiness { get; set; }
        public string SponserName { get; set; }
        public string ProductName { get; set; }
    }


    public class CheckSponsor
    {
        public string SponsorID { get; set; }
        public string SponsorName { get; set; }
        public string ParentName { get; set; }
        public string UnderPlaceName { get; set; }
        public string ParentId { get; set; }
        public string Leg { get; set; }
        public string Msg { get; set; }
        public string Msg1 { get; set; }
        public string rbtnLeft { get; set; }
        public string rbtnRight { get; set; }
        public string FK_MemId { get; set; }
        public string FK_ParentId { get; set; }
        public string Response { get; set; }
    }

    public class CheckParent
    {
        public string SponsorID { get; set; }
        public string SponsorName { get; set; }
        public string ParentName { get; set; }
        public string UnderPlaceName { get; set; }
        public string ParentId { get; set; }
        public string Leg { get; set; }
        public string Msg { get; set; }
        public string Msg1 { get; set; }
        public string rbtnLeft { get; set; }
        public string rbtnRight { get; set; }
        public string FK_MemId { get; set; }
        public string FK_ParentId { get; set; }
        public string Response { get; set; }
    }


    public class Registration
    {
        public string ePinNo { get; set; }
        public string SponsorCode { get; set; }
        public string ParentCode { get; set; }
        public string JoiningLeg { get; set; }
        public string A_Title { get; set; }
        public string A_FirstName { get; set; }
        public string A_MiddleName { get; set; }
        public string A_LastName { get; set; }
        public string A_GaurdianRelation { get; set; }
        public string A_FatherName { get; set; }
        public string A_HusbandWifeName { get; set; }
        public string A_PMobile { get; set; }
        public string A_Gender { get; set; }
        public string A_PEmailID { get; set; }
        public string A_DOB { get; set; }
        public string A_PAddressLine1 { get; set; }
        public string A_PPinCode { get; set; }
        public string A_PCity { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string A_PANNo { get; set; }
        public string CreatedBy { get; set; }
    }


    public class GetMemberDetail
    {

        public string FK_MemID { get; set; }
        public string LoginId { get; set; }
        public string MemberName { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string FatherName { get; set; }
        public string MaritalStatus { get; set; }
        public string Education { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Address_C { get; set; }
        public string PinCode_C { get; set; }
        public string City_C { get; set; }
        public string State_C { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelation { get; set; }
        public string NomineeDob { get; set; }
        public string GSTNo { get; set; }
        public string EntityName { get; set; }
        public string FirstName { get; set; }
        public string EntityAddress { get; set; }
        public string EntityPhone { get; set; }
        public string EntityEmail { get; set; }
        public string AccNo { get; set; }
        public string IFSCCode { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string Pan { get; set; }
        public string Aadhar { get; set; }
        public string ChequeAttch { get; set; }
        public string PanAttach { get; set; }
        public string AadharAttach { get; set; }
        public string IsApprovedCheque { get; set; }
        public string IsApprovedPanCard { get; set; }
        public string IsApprovedAdhaar { get; set; }
        public string Response { get; set; }
    }

    public class AssociateRepurchaseIncomes
    {
        public string FK_MemID { get; set; }
        //Get:Incomes
        public string Fk_OrderId { get; set; }
        public string OrderNo { get; set; }
        public string Products { get; set; }
        public string PaymentType { get; set; }
        public string CurrentDate { get; set; }
        public string BusinessAmount { get; set; }
        public string Status { get; set; }

        //Get:Incomes Details
        public string ProductName { get; set; }
        public string ProductQty { get; set; }
        public string ProductAmt { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }



    }

}