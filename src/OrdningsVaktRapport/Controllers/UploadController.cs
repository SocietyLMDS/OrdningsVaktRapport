using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Auth;
using System.Drawing;

namespace OrdningsVaktRapport.Controllers
{
    public class UploadController : ApiController
    {
        //
        // GET: /Upload/
        private IRepository _repository;

        public UploadController(IRepository repository)
        {
            _repository = repository;
        }

        [System.Web.Mvc.HttpPost]
        [DigestAuthorize(Role = "Company, Employee")]
        public async Task<string> UploadLogo()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var fileSaveLocation = HttpContext.Current.Server.MapPath("~/Images");
                var provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                await Request.Content.ReadAsMultipartAsync(provider);
                var filename = provider.FormData.GetValues("filename")[0];
                var id = provider.FormData.GetValues("id")[0];
                var imageBytes = File.ReadAllBytes(provider.FileData[0].LocalFileName);
                var ms = new MemoryStream(imageBytes);
                var image = Image.FromStream(ms);
                var fileList = Directory.GetFiles(fileSaveLocation);

                foreach (var file in fileList.Where(file => file.Contains(id)))
                {
                    File.Delete(file);
                }

                var resizeSettings = new ImageResizer.Instructions()
                {
                    Scale = ImageResizer.ScaleMode.Both,
                    Width = 250,
                    Height = 250,
                    Mode = ImageResizer.FitMode.Crop
                };

                var imageSized = new ImageResizer.ImageJob(image, "~/Images/" + filename, resizeSettings);
                imageSized.CreateParentDirectory = true;
                imageSized.Build();

                return _repository.UpdateCompanyLogoLink(ConfigurationManager.AppSettings["ApiBaseUri"]+"Images/" + filename, Guid.Parse(id));

            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        { }
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }
}
