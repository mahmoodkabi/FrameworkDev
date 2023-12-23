using FrameworkDev.Web.Areas.Document.Models;
using FrameworkDev.Web.Areas.Document.Repos;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.Document.Controllers
{
    [CustomAuthorize(PermissionKey = "DOC:R", PermissionName = "نمایش")]
    public class DocumentController : Helpers.CustomController
    {

        //[Authorize]
       [CustomAuthorize(PermissionKey = "DOC:DOC:R", PermissionName = "نمایش")]
       // [MenuItem(Title = "لیست پیوست ها", Order = 1, ParentController = typeof(DocumentMenuController), CssIcon = "fa fa-superpowers", SubSystems = "")]

        public ActionResult Index()
        {
            return View();
        }

        public class TempDoc
        {
            public string DocDescription { get; set; }

            public string TableName { get; set; }

            public string RowId { get; set; }
        }
    
        public HttpPostedFileBase FileSession
        {
            get
            {
                object value = System.Web.HttpContext.Current.Session["FileSession"];
                return (HttpPostedFileBase)value;
            }

            set => System.Web.HttpContext.Current.Session["FileSession"] = value;
        }
       
        public void AutoUploadFile(HttpPostedFileBase file)
        {
            if (file != null)
            {
                FileSession = file;
            }
        }

        [CustomAuthorize(PermissionKey = "DOC:DOC:R", PermissionName = "نمایش")]
        [HttpPost]
        public JsonResult GetDocList(TempDoc DocInfo)
        {
            FileSession = null;

            VM_Document Doc = new VM_Document
            {
                TableNameId = new TableNameRepository().GetByTableName(DocInfo.TableName).TablesNameID
            };
            ;
            Doc.TableRowId = Convert.ToInt32(DocInfo.RowId);

            IQueryable<VM_Document> DocList = new DocumentRepository().GetByTableRow(Doc);

            return Json(DocList, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "DOC:DOC:C", PermissionName = "ایجاد")]
        public JsonResult CreateDocNew(TempDoc NewDoc)
        {
            if (FileSession != null)
            {
                VM_FrameworkDevDocs HRDoc = new VM_FrameworkDevDocs();
                VM_Document DocResult = new VM_Document();
                string HRDocId = string.Empty;
                try
                {
                    
                    DateTime dateNow = DateTime.Now;
                    string strDateTime = string.Format("{0}{1}{2} {3}{4}{5}",
                                        dateNow.Year.ToString(),
                                        dateNow.Month.ToString(),
                                        dateNow.Day.ToString(),
                                        dateNow.Hour.ToString(),
                                        dateNow.Minute.ToString(),
                                        dateNow.Second.ToString()
                                     ).ToString();

                    BinaryReader rdr = new BinaryReader(FileSession.InputStream);

                    if (NewDoc.TableName == "Web.____PersonelPictures")
                    {
                        HRDoc.name = NewDoc.TableName.Substring(4) + '-' + 
                                     NewDoc.RowId;
                    }
                    else
                    {
                        HRDoc.name =  NewDoc.TableName + '-' +
                                      NewDoc.RowId + '-' +
                                      strDateTime + '_' +
                                      FileSession.FileName;                        
                    }

                    StringBuilder sb = new StringBuilder(HRDoc.name);
                    sb.Remove(0, 4);
                    HRDoc.name = sb.ToString();

                    HRDoc.file_stream = rdr.ReadBytes(FileSession.ContentLength);

                    HRDocId = new FrameworkDevDocsRepository().Create(HRDoc);

                    VM_Document Doc = new VM_Document
                    {
                        DocDesc = NewDoc.DocDescription,
                        TableNameId = new TableNameRepository().GetByTableName(NewDoc.TableName).TablesNameID,
                        TableRowId = Convert.ToInt32(NewDoc.RowId),
                        DocHRDocId_fk = HRDocId,
                        DocActive = 1,
                        DocCreDate = DateTime.Now
                    };

                    DocResult = new DocumentRepository().Insert(Doc);

                    FileSession = null;

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception EX )
                {
                    if (DocResult.DocId > 0)
                    {
                        if (HRDocId.Equals(string.Empty))
                        {
                            new FrameworkDevDocsRepository().Delete(HRDoc.stream_id.ToString());
                        }
                    }

                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(PermissionKey = "DOC:DOC:R", PermissionName = "نمایش")]
        public ActionResult ViewDoc(int DocId)
        {
            VM_FrameworkDevDocs HRDoc = new FrameworkDevDocsRepository()
                            .SelectById(
                                    new DocumentRepository().GetByID(Convert.ToInt32(DocId)).DocHRDocId_fk
                                    );

            byte[] fileBytes = HRDoc.file_stream;
            string fileName = HRDoc.name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

        [CustomAuthorize(PermissionKey = "DOC:DOC:D", PermissionName = "حذف")]
        public ActionResult DeleteDoc([DataSourceRequest] DataSourceRequest request, VM_Document Doc)
        {
            if (Doc != null)
            {
                bool HRDocDelSt = new FrameworkDevDocsRepository().Delete(Doc.DocHRDocId_fk);

                if (HRDocDelSt)
                {
                    VM_Document DocDel = new DocumentRepository().Delete(Doc.DocId);
                }
            }

            return Json(new[] { Doc }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(PermissionKey = "DOC:DOC:C", PermissionName = "ایجاد")]
        public void AddDoc()
        {
            VM_FrameworkDevDocs HRDoc = new VM_FrameworkDevDocs
            {
                name = "تست",
                file_stream = System.IO.File.ReadAllBytes("F:\\Learning\\Create FileTable.txt")
            };

            FrameworkDevDocsRepository HRDocRepos = new FrameworkDevDocsRepository();
            string HRDocId = HRDocRepos.Create(HRDoc);

            if (HRDocId != "0")
            {
                VM_Document Doc = new VM_Document
                {
                    DocDesc = "تست",
                    TableNameId = 1,
                    TableRowId = 1,
                    DocHRDocId_fk = HRDocId
                };

                DocumentRepository DocRepos = new DocumentRepository();
                DocRepos.Insert(Doc);
            }

        }


    }
}
