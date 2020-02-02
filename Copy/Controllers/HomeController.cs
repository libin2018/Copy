using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Copy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public JsonResult ImportData(HttpPostedFileBase file)
        {
            var checkResult = new CheckDataResult { Result = false };

            if (file == null)
            {
                checkResult.ErrorMessages.Add("找不到上传的文件！");
                return Json(checkResult, JsonRequestBehavior.AllowGet);
            }

            var fileName = file.FileName;
            var ext = fileName.Substring(fileName.LastIndexOf("."));
            if (!ext.InStr(".txt", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".png", ".xmind", ".rar", ".zip"))
            {
                checkResult.ErrorMessages.Add("文件格式不正确，仅支持：txt、doc、xls、jpg、png、xmind、rar、zip文件！");
                return Json(checkResult, JsonRequestBehavior.AllowGet);
            }

            var entity = new FilesDTO();
            entity.file_name = fileName.Replace(ext, "");
            entity.file_ext = ext;
            entity.file_key = Guid.NewGuid().ToString("N").ToUpper() + ext;
            entity.file_timestamp = DateTime.Now;

            byte[] fileBytes = null;
            fileBytes = new byte[file.ContentLength];
            file.InputStream.Read(fileBytes, 0, file.ContentLength);
            entity.file_bytes = fileBytes;

            CacheHelper.SetCacheValue(entity.file_key, entity, 24 * 60);

            checkResult.Result = true;
            checkResult.Entity = new FilesDTO() { file_key = entity.file_key, file_name = entity.file_name };

            return Json(checkResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult Download(string key)
        {
            var value = CacheHelper.GetCacheValue(key);
            if (value != null)
            {
                var entity = value as FilesDTO;

                CacheHelper.RemoveCache(key);

                CacheHelper.SetCacheValue(key, entity, 1);

                return File(entity.file_bytes, System.Net.Mime.MediaTypeNames.Application.Octet, Url.Encode(entity.file_name + entity.file_ext));
            }
            else
            {
                return Content("无文件或已失效", "text/html");
            }
        }
    }
}