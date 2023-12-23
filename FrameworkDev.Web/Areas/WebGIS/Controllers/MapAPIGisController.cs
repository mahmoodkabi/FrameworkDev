using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DotSpatial.Projections;
using FrameworkDev.Web.Areas.BaseInfo.Controllers;
using FrameworkDev.Web.Areas.WebGIS.Models;
using FrameworkDev.Web.Areas.WebGIS.Repos;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using FrameworkDev.Web.Models;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SharpKml.Dom;
using SharpKml.Engine;


namespace FrameworkDev.Web.Areas.WebGIS.Controllers
{
    //[RoutePrefix("api/MapAPIGis")]
    [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS", PermissionName = "مديريت وب جي آي اس")]
    [MenuItem(Title = "اطلاعات پایه", Order = 8, ParentController = typeof(WebGISMenuController), CssIcon = "fa fa-optin-monster", SubSystems = "WebGIS")]
    public class MapAPIGisController : FrameworkDev.Web.Helpers.CustomController
    {


        private readonly MapAPIGisRepository repo = new MapAPIGisRepository();

        [HttpPost]
        public JsonResult MapLayer(VM_MapModel model)
        {
            var res = repo.MapLayer(model).Where(x => x.GroupService == model.groupService && x.Active == true).OrderBy(c => c.ArcServiceID).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddAllBookMark(List<VM_MapModel> itemsBookmark)
        {
            var res = repo.AddAllBookMark(itemsBookmark, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddBookMark(VM_MapModel itemsBookmark)
        {
            //double[] xy = new double[2] { Convert.ToDouble(itemsBookmark.Point_X), Convert.ToDouble(itemsBookmark.Point_Y) };
            //DotSpatial.Projections.Reproject.ReprojectPoints(xy, 0, DotSpatial.Projections.ProjectionInfo.FromEpsgCode., 4326, 0, 1);

            //DotSpatial.Projections.Reproject.ReprojectPoints(
            //    new[] { Convert.ToDouble(itemsBookmark.Point_X), Convert.ToDouble(itemsBookmark.Point_Y) },
            //    new[] { 0d },
            //    ProjectionInfo.FromEpsgCode(102100), ProjectionInfo.FromEpsgCode(4326),
            //    0,
            //    1);

            ////EPSG 3035
            //ProjectionInfo fromProjection = KnownCoordinateSystems.Projected.Europe.GetProjection("102100");
            ////EPSG 4326
            //ProjectionInfo toProjection = KnownCoordinateSystems.Geographic.World.WGS1984;

            //Reproject.ReprojectPoints(new[] { Convert.ToDouble(itemsBookmark.Point_X), Convert.ToDouble(itemsBookmark.Point_Y) }, new[] { 0d }, fromProjection, toProjection, 0, 1);


            var res = repo.AddBookMark(itemsBookmark, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBookMark(string search, int page, int pageSize)
        {
            DataSourceRequest request = new DataSourceRequest()
            {
                Page = page,
                PageSize = pageSize
            };

            var res = repo.GetBookMark((User as CustomPrincipal).UserId).Where(p => p.Name.Contains(search));
            //.Where(p => p.Name.Contains(search)).Skip(page).Take(pageSize).ToList();

            DataSourceResult result = res.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAddressBuffer(VM_MapModel model)
        {
            //var res = repo.GetAddressBuffer(model);
            //return Json(res, JsonRequestBehavior.AllowGet);

            return null;
        }

        [HttpPost]
        public ActionResult DeleteBookMark(int bookMarkId)
        {
            var res = repo.DeleteBookMark(bookMarkId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult AddFile(VM_File vm)
        {
            //return Json("", JsonRequestBehavior.AllowGet);
            try
            {
                List<VM_GeometryType> lstGeometries = null;

                if (vm.File == null)
                {
                    return Json("هیچ فایلی انتخاب نشده است", JsonRequestBehavior.AllowGet);
                }


                //--------------------Read KML File----------------------------------------------
                if (vm.File.FileName.Split('.')[1].ToUpper() == "KML")
                {
                    var kmlFile = KmlFile.Load(vm.File.InputStream);
                    lstGeometries = GIS.ExtractDataFromKML(kmlFile);
                }
                //--------------------Read KMZ File----------------------------------------------
                else if (vm.File.FileName.Split('.')[1].ToUpper() == "KMZ")
                {
                    var kmzFile = KmzFile.Open(vm.File.InputStream);
                    KmlFile kmlFile = kmzFile.GetDefaultKmlFile();
                    lstGeometries = GIS.ExtractDataFromKML(kmlFile);
                }
                //--------------------Read Shape File--------------------------------------------
                else if (vm.File.FileName.Split('.')[1].ToUpper() == "ZIP")
                {
                    //مسیر آپلود فایل ها 
                    var userFilePath = ControllerContext.HttpContext.Server.MapPath(@"..\..\UserFile");

                    //ایجاد فولدر و ذخیره فایل
                    var strGUID = Guid.NewGuid().ToString();
                    string targetFolder = userFilePath + "//" + strGUID;
                    Directory.CreateDirectory(targetFolder);
                    string targetPath = Path.Combine(targetFolder, vm.File.FileName);
                    vm.File.SaveAs(targetPath);

                    //آن زیپ فایل
                    GIS.ExtractZipContent(targetPath, null, targetFolder);

                    //خواندن فایل
                    var shapeFile = DotSpatial.Data.Shapefile.OpenFile(targetFolder + "\\" + vm.File.FileName.Split('.')[0] + ".shp");
                    lstGeometries = GIS.ExtractDataFromShapeFile(shapeFile);

                    ////حذف فولدر و فایل کاربر
                    Directory.Delete(targetFolder, true);
                }
                else
                {
                    return Json(new VM_Message()
                    {
                        Result = "Error",
                        Message = "لطفا فایل KML، KMZ  و یا ShapeFile(زیپ شده) جهت آپلود انتخاب کنید"
                    },
                    JsonRequestBehavior.AllowGet);
                }


                //در صورتی که خطایی رخ داده باشد
                if (lstGeometries != null && lstGeometries.Count == 0)
                {
                    return Json(new VM_Message()
                    {
                        Result = "Error",
                        Message = "لطفا فایل KML، KMZ  و یا ShapeFile(زیپ شده) جهت آپلود انتخاب کنید"
                    },
                    JsonRequestBehavior.AllowGet);
                }
                else if (lstGeometries.First().Message.Result == "Error")
                {
                    return Json(new VM_Message()
                    {
                        Result = "Error",
                        Message = "لطفا فایل KML، KMZ  و یا ShapeFile(زیپ شده) جهت آپلود انتخاب کنید",
                        Description = lstGeometries.First().Message.Description
                    },
                   JsonRequestBehavior.AllowGet);
                }


                vm.FileGeometries = lstGeometries; ;
                vm.UserId = (User as CustomPrincipal).UserId;
                vm.ModifyDate = DateTime.Now;
                var res = repo.AddFile(vm);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:Show", PermissionName = "نمایش")]
        [HttpPost]
        public JsonResult GetUserLayer(string search, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            DataSourceRequest request = new DataSourceRequest()
            {
                Page = page,
                PageSize = pageSize
            };

            var res = repo.GetUserLayer((User as CustomPrincipal).UserId).Where(p => p.FileName.Contains(search));
            //.Where(p => p.Name.Contains(search)).Skip(page).Take(pageSize).ToList();

            DataSourceResult result = res.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteUserLayer(int userLayerId)
        {
            var res = repo.DeleteUserLayer(userLayerId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:Show", PermissionName = "نمایش")]
        [HttpPost]
        public JsonResult GetFeatureLayer(int id, string type)
        {
            if (type == "polygon")
            {
                var res = repo.GetFeaturePolygon(id).ToList();
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else if (type == "polyline")
            {
                var res = repo.GetFeatureLine(id).ToList();
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else if (type == "point")
            {
                var res = repo.GetFeaturePoint(id).ToList();
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet); ;
        }


        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:Show", PermissionName = "نمایش")]
        [HttpPost]
        public ActionResult EditFeatures(List<VM_ShapeFeature> features)
        {
            var res = repo.EditFeatures(features, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:Show", PermissionName = "نمایش")]
        [HttpPost]
        public ActionResult AddFeatures(List<VM_ShapeFeature> features)
        {
            var res = repo.AddFeatures(features, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:Show", PermissionName = "نمایش")]
        [HttpPost]
        public ActionResult DeleteFeatures(List<VM_ShapeFeature> features)
        {
            var res = repo.DeleteFeatures(features, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:Show", PermissionName = "نمایش")]
        [HttpPost]
        public JsonResult GetSearchResult(string search, int page, int pageSize)
        {
            var res = repo.GetSearchResult(search, (User as CustomPrincipal).UserId, page, pageSize);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:Show", PermissionName = "نمایش")]
        [HttpPost]
        public JsonResult GetUserLayerShapeWithXY(string shapeType, string strSHAPE)
        {
            var res = repo.GetUserLayerShapeWithXY(shapeType, strSHAPE, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:GeoCoding", PermissionName = "نمایش")]
        [HttpPost]
        public JsonResult GeoCoding(string county, string city, string zone, string mabar1, string mabar2, string mabar3)
        {
            var res = repo.GeoCoding(county, city, zone, mabar1, mabar2, mabar3, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "WebGIS:ManageWebGIS:GeoCoding", PermissionName = "نمایش")]
        [HttpPost]
        public JsonResult ReverseGeoCoding(string xy)
        {
            var convertedXY = GIS.ConvertPoint(xy, 3857, 4326);

            var res = repo.ReverseGeoCoding(convertedXY, (User as CustomPrincipal).UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

      
    }
}
