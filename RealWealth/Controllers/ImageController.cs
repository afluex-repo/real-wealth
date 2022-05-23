using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealWealth.Models;
using System.Web;
using System.IO;
using System.Data;

namespace RealWealth.Controllers
{
    public class ImageController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage UploadPhoto()
        {
            Photo model = new Photo();
            try
            {
                Random rn = new Random();
                string Results = string.Empty;
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                model.PK_UserId = HttpContext.Current.Request.Params["PK_UserId"];
                var file = HttpContext.Current.Request.Files[0];
                model.Image = rn.Next(10, 100) + "_photo_" + file.FileName;
                string folderPath = HttpContext.Current.Server.MapPath("~/ProfilePicture/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                file.SaveAs(folderPath + model.Image);
                model.Image = "/ProfilePicture/" + model.Image;
                DataSet ds = model.UploadProfilePic();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.OK,
                           Message = "Profile Pic Uploaded Successfully",
                           ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString(),
                       });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.OK,
                           Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString(),
                       });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.InternalServerError,
                           Message = "Error Occurred",
                       });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.InternalServerError,
                           Message = ex.Message,
                       });
            }
        }
        [HttpPost]
        public HttpResponseMessage UploadPan()
        {
            Photo model = new Photo();
            try
            {
                Random rn = new Random();
                string Results = string.Empty;
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                model.PK_UserId = HttpContext.Current.Request.Params["PK_UserId"];
                var file = HttpContext.Current.Request.Files[0];
                model.Image = rn.Next(10, 100) + "_pan_" + file.FileName;
                string folderPath = HttpContext.Current.Server.MapPath("~/PanUpload/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                file.SaveAs(folderPath + model.Image);
                model.Image = "/PanUpload/" + model.Image;
                DataSet ds = model.UploadPan();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.OK,
                           Message = "Pan Uploaded Successfully",
                           ProfilePic = ds.Tables[0].Rows[0]["PanImage"].ToString(),
                       });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.OK,
                           Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString(),
                       });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.InternalServerError,
                           Message = "Error Occurred",
                       });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                       new
                       {
                           StatusCode = HttpStatusCode.InternalServerError,
                           Message = ex.Message,
                       });
            }
        }
    }
}
