using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealWealth.Models
{
    public class AgentIncomes
    {
        public string FK_MemID { get; set; }
        public string FK_PlanId { get; set; }
        public string CurrentDate { get; set; }
        public string LoginID { get; set; }
        public string MemberName { get; set; }
        public string BusinessAmount { get; set; }
        public string programme { get; set; }
        public string GrossTotal { get; set; }


    }
}