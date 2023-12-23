using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FrameworkDev.Web.Helpers
{
    public static class FilesHelper
    {
        public static bool RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public static HttpResponseMessage ImportRemove(HttpRequestMessage _request, IPrincipal _user, string _moduleName, string _filenameField)
        {
            string fileNames = _request.Content.ReadAsFormDataAsync().Result[_filenameField];

            if (fileNames != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + _moduleName + "/" + _user.Identity.Name + "-" + Path.GetFileName(fileNames));

                if (!FilesHelper.RemoveFile(filePath))
                {
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound };
                }
            }

            return new HttpResponseMessage() { Content = new StringContent("") };
        }

        public static async Task<HttpResponseMessage> ImportSave(HttpRequestMessage _request, IPrincipal _user, string _moduleName, string _filenameField, string _fileTypes = "")
        {
            if (!_request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + _moduleName);
            string tempRoot = HttpContext.Current.Server.MapPath("~/App_Data/uploads/temp");

            Directory.CreateDirectory(root);
            Directory.CreateDirectory(tempRoot);

            MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(tempRoot);

            HttpResponseMessage task = await _request.Content.ReadAsMultipartAsync(provider).
                ContinueWith(o =>
                {
                    if (o.IsFaulted || o.IsCanceled)
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }

                    MultipartFileData upFile = provider.FileData[0];

                    string[] disp = upFile.Headers.First(x => x.Key == "Content-Disposition").Value.ToList()[0].Split('"');

                    string realFileName = _user.Identity.Name + "-" + disp[disp.Length - 2];

                    if (realFileName.Contains("\\"))
                    {
                        string[] realFileNameSplitted = realFileName.Split('\\');
                        realFileName = realFileNameSplitted[realFileNameSplitted.Length - 1];
                    }

                    foreach (string fileType in _fileTypes.Split(','))
                    {
                        if (!realFileName.EndsWith(fileType))
                        {
                            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                        }
                    }

                    string srcPath = upFile.LocalFileName;
                    string dstPath = root + "\\" + realFileName;

                    FilesHelper.RemoveFile(dstPath);

                    File.Move(srcPath, dstPath);

                    return new HttpResponseMessage() { Content = new StringContent("") };
                }
            ).ConfigureAwait(false);

            return task;
        }

        public static List<T> ImportFinalize<T>(HttpRequestMessage _request, IPrincipal _user, string _moduleName, string _filenameField) where T : new()
        {
            Task<NameValueCollection> form = _request.Content.ReadAsFormDataAsync();

            string fileNames = form.Result[_filenameField];

            if (string.IsNullOrEmpty(fileNames))
            {
                return null;
            }

            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + _moduleName + "/" + _user.Identity.Name + "-" + Path.GetFileName(fileNames));

            if (File.Exists(filePath))
            {
                var xlsxFile = Utility.ReadExcelFile(filePath, "Sheet1");

                return xlsxFile?.ToList<T>();
            }

            return null;
        }

        public static HttpResponseMessage ImportSample<T>(HttpRequestMessage _request, IPrincipal _user, string _moduleName, IQueryable<T> data)
        {
            string prjName = Utility.GetProjectName();
            string pdtString = Utility.ToPersianDateTimeString(DateTime.Now);
            string usrString = _user.Identity.Name;
            string fileName = string.Format("{0}-{1}-{2}-ImportSample-{3}.xlsx", usrString, prjName, _moduleName, pdtString);

            HttpResponseMessage response;
            response = _request.CreateResponse(HttpStatusCode.OK);
            MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content = new StreamContent(Utility.GetExcelSheet(data.Take(100)));
            response.Content = response.Content;
            response.Content.Headers.ContentType = mediaType;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return response;
        }
    }
}
