using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Copy
{
    [RoutePrefix("api/Files")]
    public class FilesController : ApiController
    {
        [Route("GetQrCode")]
        [HttpGet]
        public HttpResponseMessage GetQrCode(string url)
        {
            System.IO.MemoryStream MStream1 = new System.IO.MemoryStream();
            QrCodeHelper.BuildQrCode(url, MStream1, "~/wdi.ico");
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(MStream1.ToArray())
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return resp;
        }
    }
}