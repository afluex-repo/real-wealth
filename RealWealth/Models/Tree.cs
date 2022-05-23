using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace RealWealth.Models
{
    public class Tree : Common
    {
        public string LoginId { get;  set; }
        public string RootAgentCode { get;  set; }
        public List<TreeMembers> lst { get; set; }
        public List<MemberDetails> lstMember { get; set; }
        public string PK_UserId { get; set; }
        public string Level { get; set; }
        public string TotalDirect { get; set; }
        public string TotalActive { get; set; }
        public string TotalInactive { get; set; }
        public string TotalTeam { get; set; }
        public string TotalActiveTeam { get; set; }
        public string TotalInActiveTeam { get; set; }
        public string SponsorName { get; set; }
        public string Color { get; set; }
        public string SelfBV { get; set; }
        public string TeamBV { get; set; }
        public string SelfBVDollar { get; set; }
        public string TeamBVDollar { get; set; }
        public DataSet GetLevelTreeData()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@AgentCode", LoginId),
                                      new SqlParameter("@RootAgentCode", RootAgentCode),
                                    
            };

            DataSet ds = DBHelper.ExecuteQuery("LevelTree", para);
            return ds;
        }
        public DataSet GetLevelMembersCount()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@AgentCode", LoginId),
                                      new SqlParameter("@RootAgentCode", RootAgentCode)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetLevelMembersCount", para);
            return ds;
        }
        public DataSet GetLevelMembersCountTR1()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@AgentCode", LoginId),
                                      new SqlParameter("@RootAgentCode", RootAgentCode),

            };

            DataSet ds = DBHelper.ExecuteQuery("GetLevelMembersCountTR1", para);
            return ds;
        }
        public DataSet GetLevelMembers()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@MemId", PK_UserId),
                                      new SqlParameter("@Level", Level),
            };

            DataSet ds = DBHelper.ExecuteQuery("GetLevelMembers", para);
            return ds;
        }
    }
    public class TreeMembers
    {
        public string LevelName { get; set; }
        public string NumberOfMembers { get; set; }
    }
    public class MemberDetails
    {
        public string PK_UserId { get; set; }
        public string LoginId { get; set; }
        public string MemberName { get; set; }
        public string Level { get; set; }
        public string ProfilePic { get; set; }
        public string SelfBV { get; set; }
        public string TeamBV { get; set; }
        public string SelfBVDollar { get; set; }
        public string TeamBVDollar { get; set; }
        public string Status { get; set; }
        public string SponsorName { get; set; }
        public string Color { get; set; }
    }
}