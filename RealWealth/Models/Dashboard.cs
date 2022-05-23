using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace RealWealth.Models
{
    public class Dashboard :Common
    {
        public string cssclass { get; set; }
        public List<Dashboard> lstmessages { get; set; }
        public string FK_UserId { get; set; }
        public string MemberName { get;  set; }
        public string Message { get;  set; }
        public string MessageTitle { get;  set; }
        public string Pk_MessageId { get;  set; }


        public string Image { get; set; }
        public string Title { get; set; }
        public List<Dashboard> lstReward { get; set; }
        public string PK_RewardId { get; set; }



        public DataSet GetAssociateDashboard()
        {
            SqlParameter[] para = { new SqlParameter("@Fk_UserId", FK_UserId), };
            DataSet ds = DBHelper.ExecuteQuery("GetDashBoardDetailsForAssociate", para);
            return ds;
        }
        public DataSet GetDashBoardDetails()
        {

            DataSet ds = DBHelper.ExecuteQuery("GetDashBoardDetails");
            return ds;
        }


        public DataSet GetRewarDetails()
        {
            SqlParameter[] para = {
                new SqlParameter("@Title",Title)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetRewarDetails", para);
            return ds;
        }
    }
}