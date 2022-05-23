﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealWealth.Models
{
    public class Master : Common
    {
        public List<Master> lstpackage { get; set; }

        public decimal BinaryPercent { get; set; }
        public decimal BV { get; set; }
        public decimal CGST { get; set; }
        public decimal DirectPercent { get; set; }
        public decimal IGST { get; set; }
        public string Packageid { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ROIPercent { get; set; }
        public decimal SGST { get; set; }
        public string PackageTypeId { get; set; }
        public decimal ToAmount { get; set; }
        public decimal FromAmount { get; set; }
        public string PackageTypeName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public List<Master> lstReward { get; set; }
        public string PK_RewardId { get; set; }
        public string Days { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public decimal InMultipleOf { get; set; }
        public string HSNCode { get; set; }
        public decimal FinalAmount { get; set; }
        #region ProductMaster

        public DataSet SaveProduct()
        {
            SqlParameter[] para = { new SqlParameter("@ProductName", ProductName),
                                  new SqlParameter("@ProductPrice", ProductPrice),
                                  new SqlParameter("@IGST", IGST),
                                  //new SqlParameter("@CGST", CGST),
                                  //new SqlParameter("@SGST", SGST),
                                  //new SqlParameter("@BinaryPercent", BinaryPercent),
                                  //new SqlParameter("@DirectPercent", DirectPercent),
                                  new SqlParameter("@ROIPercent", ROIPercent),
                                   new SqlParameter("@Days", Days),
                                  new SqlParameter("@BV",BV),
                                  new SqlParameter("@AddedBy", AddedBy),
                                  new SqlParameter("@PackageTypeId", PackageTypeId),
                                   new SqlParameter("@FromAmount", FromAmount),
                                    new SqlParameter("@ToAmount", ToAmount),
                                     new SqlParameter("@InMultipleOf", InMultipleOf),
                                      new SqlParameter("@HSNCode", HSNCode),
                                 new SqlParameter("@FinalAmount",FinalAmount)
            };

            DataSet ds = DBHelper.ExecuteQuery("AddProduct", para);
            return ds;
        }
        public DataSet ProductList()
        {
            SqlParameter[] para = { new SqlParameter("@ProductID", Packageid),
                 new SqlParameter("@PackageTypeId", PackageTypeId)
            };
            DataSet ds = DBHelper.ExecuteQuery("ProductList", para);
            return ds;
        }
        public DataSet DeleteProduct()
        {
            SqlParameter[] para = { new SqlParameter("@ProductID", Packageid),
                                  new SqlParameter("@DeletedBy", AddedBy),};

            DataSet ds = DBHelper.ExecuteQuery("DeleteProduct", para);
            return ds;
        }
        public DataSet ActivateDeactivatePackage()
        {
            SqlParameter[] para = { new SqlParameter("@ProductID", Packageid),
                new SqlParameter("@IsActive", IsActive),
                                  new SqlParameter("@UpdatedBy", AddedBy),

            };

            DataSet ds = DBHelper.ExecuteQuery("ActivateDeactivateProduct", para);
            return ds;
        }
        public DataSet UpdateProduct()
        {
            SqlParameter[] para = { new SqlParameter("@ProductID", Packageid),
                                  new SqlParameter("@ProductName", ProductName),
                                  new SqlParameter("@ProductPrice", ProductPrice),
                                  new SqlParameter("@IGST", IGST),
                                  //new SqlParameter("@CGST", CGST),
                                  //new SqlParameter("@SGST", SGST),
                                  //new SqlParameter("@BinaryPercent", BinaryPercent),
                                  //new SqlParameter("@DirectPercent", DirectPercent),
                                  new SqlParameter("@ROIPercent", ROIPercent),
                                  new SqlParameter("@Days",Days),
                                  new SqlParameter("@BV", BV),
                                  new SqlParameter("@UpdatedBy", UpdatedBy),
                                     new SqlParameter("@PackageTypeId", PackageTypeId),
                                   new SqlParameter("@FromAmount", FromAmount),
                                    new SqlParameter("@ToAmount", ToAmount),
                                 new SqlParameter("@InMultipleOf", InMultipleOf),
                                 new SqlParameter("@HSNCode", HSNCode),
                                 new SqlParameter("@FinalAmount",FinalAmount)
            };

            DataSet ds = DBHelper.ExecuteQuery("UpdateProduct", para);
            return ds;
        }


        public DataSet Upload()
        {
            SqlParameter[] para = { new SqlParameter("@Title", Title),
                                  new SqlParameter("@postedFile", Image),
                                  new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("Upload", para);
            return ds;
        }
        public DataSet UploadFile()
        {
            SqlParameter[] para = { new SqlParameter("@Title", Title),
                                  new SqlParameter("@postedFile", Image),
                                  new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("UploadFile", para);
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
        public DataSet GetFilesDetails()
        {
            SqlParameter[] para = {
                new SqlParameter("@Title",Title)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetFilesDetails", para);
            return ds;
        }
        public DataSet DeleteReward()
        {
            SqlParameter[] para = {
                new SqlParameter("@PK_RewardId",PK_RewardId),
                new SqlParameter("@DeletedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteReward", para);
            return ds;
        }
        public DataSet DeleteFile()
        {
            SqlParameter[] para = {
                new SqlParameter("@PK_RewardId",PK_RewardId),
                new SqlParameter("@DeletedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteFile", para);
            return ds;
        }
        #endregion
    }
}