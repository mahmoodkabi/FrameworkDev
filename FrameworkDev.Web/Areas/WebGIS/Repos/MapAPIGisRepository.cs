using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
//using System.Web.Http;
using System.Web.Mvc;

using FrameworkDev.Web.Areas.WebGIS.Models;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Models;
using Microsoft.SqlServer.Server;

namespace FrameworkDev.Web.Areas.WebGIS.Repos
{
    //[RoutePrefix("api/MapAPIGis")]
    public class MapAPIGisRepository : CustomRepository<VM_MapModel, int>
    {

        public MapAPIGisRepository()
        {

        }


        [HttpPost]
        public IQueryable<Service> MapLayer(VM_MapModel model)
        {
            var res = context.Services.Where(x => x.GroupService == model.groupService && x.Active == true).OrderBy(c => c.ArcServiceID);
            return res;
        }

        [HttpPost]
        public VM_Message AddAllBookMark(List<VM_MapModel> itemsBookmark, int userID)
        {
            //حذف همه بوک مارک های قبلی  
            var userBookMarks = context.BookMarks.Where(p => p.UserID_fk == userID).ToList();
            context.BookMarks.RemoveRange(userBookMarks);

            //اضافه کردن مجدد همه بوک مارک ها
            foreach (var item in itemsBookmark)
            {
                context.BookMarks.Add(new BookMark()
                {
                    Description = item.Description,
                    ModifyDate = DateTime.Now,
                    Name = item.Name,
                    UserID_fk = userID,
                    Wkid = item.Wkid,
                    Xmax = item.Xmax,
                    Xmin = item.Xmin,
                    Ymax = item.Ymax,
                    Ymin = item.Ymin,
                    Extent = item.Extent
                });
            }

            context.SaveChanges();

            return new VM_Message()
            {
                Result = "OK",
                Message = Resources.Messages.InsertSuccessful
            };
        }



        public BookMark AddBookMark(VM_MapModel itemsBookmark, int userID)
        {
            var res = context.BookMarks.Add(new BookMark()
            {
                Description = itemsBookmark.Description,
                ModifyDate = DateTime.Now,
                Name = itemsBookmark.Name,
                UserID_fk = userID,
                Wkid = itemsBookmark.Wkid,
                Xmax = itemsBookmark.Xmax,
                Xmin = itemsBookmark.Xmin,
                Ymax = itemsBookmark.Ymax,
                Ymin = itemsBookmark.Ymin,
                Extent = itemsBookmark.Extent,
                CenterPoint = itemsBookmark.CenterPoint,
                Sacle = itemsBookmark.Sacle,
                Rings = itemsBookmark.Rings
            });

            context.SaveChanges();

            return res;
        }

        internal VM_Message DeleteBookMark(int bookMarkId)
        {
            var bookmark = context.BookMarks.First(p => p.BookMarkID == bookMarkId);
            var res = context.BookMarks.Remove(bookmark);
            context.SaveChanges();

            return new VM_Message()
            {
                Result = "OK",
                Message = Resources.Messages.DeleteSuccessful
            };
        }

        public IQueryable<VM_Bookmark> GetBookMark(int userID)
        {
            var userBookMarks = context.BookMarks.Where(p => p.UserID_fk == userID)
                .Select(p => new VM_Bookmark()
                {
                    BookMarkID = p.BookMarkID,
                    CenterPoint = p.CenterPoint,
                    Description = p.Description,
                    Extent = p.Extent,
                    LatestWkid = p.LatestWkid,
                    ModifyDate = p.ModifyDate,
                    Name = p.Name,
                    Note = p.Note,
                    Rings = p.Rings,
                    Sacle = p.Sacle,
                    UserId = p.UserId,
                    UserID_fk = p.UserID_fk,
                    Wkid = p.Wkid,
                    Xmax = p.Xmax,
                    Xmin = p.Xmin,
                    Ymax = p.Ymax,
                    Ymin = p.Ymin,
                });
            return userBookMarks;
        }


        //public List<sp_AddressBuffer_Result> GetAddressBuffer(VM_MapModel model)
        //{

        //    GeoDBEntities db1 = new GeoDBEntities();
        //    var res = db1.sp_AddressBuffer(model.Search, model.Take, model.LayerName);
        //    return res.ToList();
        //}

        public void GetAddressBuffer(VM_MapModel model)
        {

        }

        public VM_Message EditFeatures(List<VM_ShapeFeature> features, int userId)
        {
            if (features == null || features.Count == 0)
            {
                return new VM_Message()
                {
                    Result = "Error",
                    Message = "یک عارضه جهت ویرایش انتخاب کنید"
                };
            }


            if (features.First().Type == "point")
            {
                foreach (var item in features)
                {
                    var feature = context.UserLayerPointFeatures.First(p => p.UserLayerPointId_fk == item.UserLayerShapeId && p.AttributeName == item.FeatureName);
                    feature.AttributeValue = item.FeatureValue ?? "";
                }
            }
            else if (features.First().Type == "polygon")
            {
                foreach (var item in features)
                {
                    var feature = context.UserLayerPolygonFeatures.First(p => p.UserLayerPolygonId_fk == item.UserLayerShapeId && p.AttributeName == item.FeatureName);
                    feature.AttributeValue = item.FeatureValue ?? "";
                }
            }
            else if (features.First().Type == "polyline")
            {
                foreach (var item in features)
                {
                    var feature = context.UserLayerLineFeatures.First(p => p.UserLayerLineId_fk == item.UserLayerShapeId && p.AttributeName == item.FeatureName);
                    feature.AttributeValue = item.FeatureValue ?? "";
                }
            }

            context.SaveChanges();

            return new VM_Message()
            {
                Result = "OK",
                Message = Resources.Messages.UpdateSuccessful
            };
        }

        internal VM_Message AddFeatures(List<VM_ShapeFeature> features, int userId)
        {
            if (features == null || features.Count == 0)
            {
                return new VM_Message()
                {
                    Result = "Error",
                    Message = "یک عارضه جهت اضافه کردن فیچر انتخاب کنید"
                };
            }

            //فیچر جدید با تایپ "ایز نیو فیچر" مشخص می شود
            // در هر بار فقط یک فیچر جدید در لیست ورودی داریم


            var type = new SqlParameter("Type", SqlDbType.NVarChar, 50);
            type.Value = features.First(p => p.IsNewFeature == false).Type;

            var userLayerShapeId = new SqlParameter("UserLayerShapeId", SqlDbType.Int);
            userLayerShapeId.Value = features.First(p => p.IsNewFeature == false).UserLayerShapeId;

            var featureName = new SqlParameter("FeatureName", SqlDbType.NVarChar, 2000);
            featureName.Value = features.First(p => p.IsNewFeature == true).FeatureName;

            var featureValue = new SqlParameter("FeatureValue", SqlDbType.NVarChar, 4000);
            featureValue.Value = features.First(p => p.IsNewFeature == true).FeatureValue;

            var userId_sp = new SqlParameter("UserId", SqlDbType.Int);
            userId_sp.Value = userId;

            var sqlParams = new SqlParameter[5]
            {
                     type, userLayerShapeId, featureName, featureValue, userId_sp
            };

            var res = context.Database.SqlQuery<sp_InsertFeature_Result>("dbo.sp_InsertFeature @Type, @UserLayerShapeId, @FeatureName, @FeatureValue, @UserId", sqlParams).Single();


            return new VM_Message()
            {
                Result = res.Result,
                Message = res.Message
            };
        }

        internal VM_Message DeleteFeatures(List<VM_ShapeFeature> features, int userId)
        {
            if (features == null || features.Count == 0)
            {
                return new VM_Message()
                {
                    Result = "Error",
                    Message = "یک عارضه جهت حذف کردن انتخاب کنید"
                };
            }

            if (features.First(p => p.IsNewFeature == false).Type == "point")
            {
                var id = features.First(x => x.IsNewFeature == false).UserLayerShapeId;
                var shp = context.USERLAYERPOINTs.First(p => p.UserLayerPointId == id);
                context.USERLAYERPOINTs.Remove(shp);
            }
            else if (features.First(p => p.IsNewFeature == false).Type == "polyline")
            {
                var id = features.First(x => x.IsNewFeature == false).UserLayerShapeId;
                var shp = context.USERLAYERLINEs.First(p => p.UserLayerLineId == id);
                context.USERLAYERLINEs.Remove(shp);
            }
            else if (features.First(p => p.IsNewFeature == false).Type == "polygon")
            {
                var id = features.First(x => x.IsNewFeature == false).UserLayerShapeId;
                var shp = context.USERLAYERPOLYGONs.First(p => p.UserLayerPolygonId == id);
                context.USERLAYERPOLYGONs.Remove(shp);
            }


            context.SaveChanges();

            return new VM_Message()
            {
                Result = "OK",
                Message = Resources.Messages.DeleteSuccessful
            };
        }


        /// <summary>
        ///  اینسرت فایل های جئومتری کاربران   Procedure
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        internal VM_Message AddFile(VM_File vm)
        {
            List<USERLAYERPOLYGON> lstUserLayerPolygon = new List<USERLAYERPOLYGON>();
            List<USERLAYERPOINT> lstUserLayerPoints = new List<USERLAYERPOINT>();
            List<USERLAYERLINE> lstUserLayerLines = new List<USERLAYERLINE>();
            var objectIdUserLayerPolygon = context.USERLAYERPOLYGONs.Max(p => p.OBJECTID);
            var objectIdUserLayerPoint = context.USERLAYERPOINTs.Max(p => p.OBJECTID);
            var objectIdUserLayerLine = context.USERLAYERLINEs.Max(p => p.OBJECTID);

            var userLayerPolygonId = context.USERLAYERPOLYGONs.Max(p => p.UserLayerPolygonId);
            var userLayerPointId = context.USERLAYERPOINTs.Max(p => p.UserLayerPointId);
            var userLayerLineId = context.USERLAYERLINEs.Max(p => p.UserLayerLineId);

            int mainId = 0;

            //-----------------------------------------------------------------------------------
            //---------------------------------UserLayer-----------------------------------------
            var userLayerSchema = new List<SqlMetaData>(1)
                {
                  new SqlMetaData("UserId_fk", SqlDbType.Int),
                  new SqlMetaData("FileName", SqlDbType.Text),
                  new SqlMetaData("Description", SqlDbType.Text),
                  new SqlMetaData("ModifyDate", SqlDbType.DateTime),
                  new SqlMetaData("LayerType", SqlDbType.Text),
                }.ToArray();

            var tableUserLayer = new List<SqlDataRecord>()
            {

            };

            var tableRowUserLayer = new SqlDataRecord(userLayerSchema);
            tableRowUserLayer.SetInt32(0, vm.UserId);
            tableRowUserLayer.SetString(1, vm.File.FileName);
            tableRowUserLayer.SetString(2, "");
            tableRowUserLayer.SetDateTime(3, DateTime.Now);
            tableRowUserLayer.SetString(4, vm.FileGeometries.First().Type);
            tableUserLayer.Add(tableRowUserLayer);


            var userLayerList = new SqlParameter("UserLayer", SqlDbType.Structured);
            userLayerList.Value = tableUserLayer;
            userLayerList.TypeName = "dbo.UserLayer";

            //------------------------------UserLayerShape-------------------------------------------

            var userLayerShapeSchema = new List<SqlMetaData>(1)
                            {
                              new SqlMetaData("MainId", SqlDbType.Int),
                              new SqlMetaData("SHAPE", SqlDbType.Text),
                              new SqlMetaData("ModifyDate", SqlDbType.DateTime),
                            }.ToArray();

            var tableUserLayerShape = new List<SqlDataRecord>()
            {

            };

            var userLayerShapeList = new SqlParameter("UserLayerShape", SqlDbType.Structured);


            //--------------------------UserLayerShapeFeature-------------------------------------------

            var userLayerShapeFeatureSchema = new List<SqlMetaData>(1)
                                {
                                    new SqlMetaData("MainId_fk", SqlDbType.Int),
                                    new SqlMetaData("AttributeName", SqlDbType.Text),
                                    new SqlMetaData("AttributeValue", SqlDbType.NVarChar, 4000),
                                    new SqlMetaData("UserId", SqlDbType.Int),
                                }.ToArray();

            var tableUserLayerShapeFeature = new List<SqlDataRecord>()
            {

            };

            var userLayerShapeFeatureList = new SqlParameter("UserLayerShapeFeature", SqlDbType.Structured);

            //------------------------------------------------------

            foreach (var itemFileGeometries in vm.FileGeometries)
            {
                mainId += 1;

                switch (itemFileGeometries.Type)
                {
                    case "POLYGON":
                        var rowUserLayerShape = new SqlDataRecord(userLayerShapeSchema);
                        rowUserLayerShape.SetInt32(0, mainId);
                        rowUserLayerShape.SetString(1, itemFileGeometries.StringFileGeometry);
                        rowUserLayerShape.SetDateTime(2, DateTime.Now);
                        tableUserLayerShape.Add(rowUserLayerShape);

                        //ایجاد اتریبیوت های شیپ فایل
                        if (itemFileGeometries.Features != null)
                        {
                            foreach (var itemAttributes in itemFileGeometries.Features)
                            {
                                var rowUserLayerShapeFeature = new SqlDataRecord(userLayerShapeFeatureSchema);
                                rowUserLayerShapeFeature.SetInt32(0, mainId);
                                rowUserLayerShapeFeature.SetString(1, itemAttributes.AttributeName);
                                rowUserLayerShapeFeature.SetString(2, itemAttributes.AttributeValue);
                                rowUserLayerShapeFeature.SetInt32(3, vm.UserId);
                                tableUserLayerShapeFeature.Add(rowUserLayerShapeFeature);
                            }
                        }
                        break;

                    case "POINT":
                        var rowUserLayerShape1 = new SqlDataRecord(userLayerShapeSchema);
                        rowUserLayerShape1.SetInt32(0, mainId);
                        rowUserLayerShape1.SetString(1, itemFileGeometries.StringFileGeometry);
                        rowUserLayerShape1.SetDateTime(2, DateTime.Now);
                        tableUserLayerShape.Add(rowUserLayerShape1);

                        //ایجاد اتریبیوت های شیپ فایل
                        if (itemFileGeometries.Features != null)
                        {
                            foreach (var itemAttributes in itemFileGeometries.Features)
                            {
                                var rowUserLayerShapeFeature = new SqlDataRecord(userLayerShapeFeatureSchema);
                                rowUserLayerShapeFeature.SetInt32(0, mainId);
                                rowUserLayerShapeFeature.SetString(1, itemAttributes.AttributeName);
                                rowUserLayerShapeFeature.SetString(2, itemAttributes.AttributeValue);
                                rowUserLayerShapeFeature.SetInt32(3, vm.UserId);
                                tableUserLayerShapeFeature.Add(rowUserLayerShapeFeature);
                            }
                        }
                        break;

                    case "LINESTRING":
                        var rowUserLayerShape2 = new SqlDataRecord(userLayerShapeSchema);
                        rowUserLayerShape2.SetInt32(0, mainId);
                        rowUserLayerShape2.SetString(1, itemFileGeometries.StringFileGeometry);
                        rowUserLayerShape2.SetDateTime(2, DateTime.Now);
                        tableUserLayerShape.Add(rowUserLayerShape2);

                        //ایجاد اتریبیوت های شیپ فایل
                        if (itemFileGeometries.Features != null)
                        {
                            foreach (var itemAttributes in itemFileGeometries.Features)
                            {
                                var rowUserLayerShapeFeature = new SqlDataRecord(userLayerShapeFeatureSchema);
                                rowUserLayerShapeFeature.SetInt32(0, mainId);
                                rowUserLayerShapeFeature.SetString(1, itemAttributes.AttributeName);
                                rowUserLayerShapeFeature.SetString(2, itemAttributes.AttributeValue);
                                rowUserLayerShapeFeature.SetInt32(3, vm.UserId);
                                tableUserLayerShapeFeature.Add(rowUserLayerShapeFeature);
                            }
                        }

                        break;
                    default:
                        break;
                }

            }

            userLayerShapeList.Value = tableUserLayerShape;
            userLayerShapeList.TypeName = "dbo.UserLayerShape";

            userLayerShapeFeatureList.Value = tableUserLayerShapeFeature;
            userLayerShapeFeatureList.TypeName = "dbo.UserLayerShapeFeature";

            var sqlParams = new SqlParameter[3]
               {
                        userLayerList, userLayerShapeList, userLayerShapeFeatureList
               };

            context.Database.CommandTimeout = 180;
            var res = context.Database.SqlQuery<sp_UserLayer_Result>("dbo.sp_UserLayer @UserLayer, @UserLayerShape, @UserLayerShapeFeature", sqlParams).Single();


            return new VM_Message()
            {
                Result = res.Result,
                Message = res.Message,
            };
        }

        internal List<VM_SearchResult> GetSearchResult(string search, int userId, int page, int pageSize)
        {
            var res = context.sp_SearchInAllFeature(search, userId, page, pageSize).Select(p => new VM_SearchResult()
            {
                AttributeValue = p.AttributeValue,
                Field = p.Field,
                LayerId = p.LayerId,
                RowId = p.RowId,
                typeShow = p.typeShow,
                Url = p.Url,
                Value = p.Value,
                Description = p.Description,
                AttributeName = p.AttributeName,
                UserLayerShapeId = p.UserLayerShapeId
            }).ToList();

            return res;
        }

        internal List<VM_Feature> GetUserLayerShapeWithXY(string shapeType, string strSHAPE, int userId)
        {
            var res = context.sp_GetUserLayerShapeWithXY(shapeType, strSHAPE, userId).Select(p => new VM_Feature()
            {
                Id = p.Id,
                AttributeName = p.AttributeName,
                AttributeValue = p.AttributeValue
            }).ToList();

            return res;
        }

        public string GeoCoding(string county, string city, string zone, string mabar1, string mabar2, string mabar3, int userId)
        {
            OSMEntities osmContext = new OSMEntities();
            osmContext.Database.CommandTimeout = 300;
            var res = osmContext.sp_GeoCoding(county, city, zone, mabar1, mabar2, mabar3).FirstOrDefault();

            return res;
        }

        public string ReverseGeoCoding(string xy, int userId)
        {
            OSMEntities osmContext = new OSMEntities();
            osmContext.Database.CommandTimeout = 300;
            var res = osmContext.sp_ReverseGeocoding(xy).FirstOrDefault();

            return res;
        }

        internal IQueryable<VM_Feature> GetFeaturePolygon(int id)
        {
            var res = context.UserLayerPolygonFeatures
                .Where(p => p.UserLayerPolygonId_fk == id)
                .Select(p => new VM_Feature()
                {
                    Id = p.UserLayerPolygonId_fk,
                    AttributeName = p.AttributeName,
                    AttributeValue = p.AttributeValue
                });

            return res;
        }

        internal IQueryable<VM_Feature> GetFeatureLine(int id)
        {
            var res = context.UserLayerLineFeatures
                .Where(p => p.UserLayerLineId_fk == id)
                .Select(p => new VM_Feature()
                {
                    Id = p.UserLayerLineId_fk,
                    AttributeName = p.AttributeName,
                    AttributeValue = p.AttributeValue
                });

            return res;
        }

        internal IQueryable<VM_Feature> GetFeaturePoint(int id)
        {
            var res = context.UserLayerPointFeatures
                .Where(p => p.UserLayerPointId_fk == id)
                .Select(p => new VM_Feature()
                {
                    Id = p.UserLayerPointId_fk,
                    AttributeName = p.AttributeName,
                    AttributeValue = p.AttributeValue
                });

            return res;
        }

        public IQueryable<VM_UserLayer> GetUserLayer(int userID)
        {
            var userBookMarks = context.UserLayers.Where(p => p.UserId_fk == userID)
                .Select(p => new VM_UserLayer()
                {
                    Description = p.Description,
                    ModifyDate = p.ModifyDate,
                    Note = p.Note,
                    FileName = p.FileName,
                    UserId_fk = p.UserId_fk,
                    UserLayerId = p.UserLayerId,
                    UserLayerLinesCount = p.USERLAYERLINEs.Count(),
                    UserLayerPointsCount = p.USERLAYERPOINTs.Count(),
                    UserLayerPolygonsCount = p.USERLAYERPOLYGONs.Count()
                });
            return userBookMarks;
        }

        internal object DeleteUserLayer(int userLayerId)
        {
            var userLayerPoint = context.USERLAYERPOINTs.Where(p => p.UserLayerId_fk == userLayerId).ToList();
            var userLayerLine = context.USERLAYERLINEs.Where(p => p.UserLayerId_fk == userLayerId).ToList();
            var userLayerPolygon = context.USERLAYERPOLYGONs.Where(p => p.UserLayerId_fk == userLayerId).ToList();
            var userLayer = context.UserLayers.First(p => p.UserLayerId == userLayerId);

            if (userLayerPoint.Count != 0)
            {
                //var id = userLayerPoint.Select(p => p.UserLayerPointId).ToList();
                //var userLayerPointFeatures  = context.UserLayerPointFeatures.Where(p => id.Contains(p.UserLayerPointId_fk)).ToList();
                //if(userLayerPointFeatures.Count != 0)
                //    context.UserLayerPointFeatures.RemoveRange(userLayerPointFeatures);

                context.USERLAYERPOINTs.RemoveRange(userLayerPoint);
            }

            if (userLayerLine.Count != 0)
            {
                //var id = userLayerLine.Select(p => p.UserLayerLineId).ToList();
                //var userLayerLineFeatures = context.UserLayerLineFeatures.Where(p => id.Contains(p.UserLayerLineId_fk)).ToList();
                //if (userLayerLineFeatures.Count != 0)
                //    context.UserLayerLineFeatures.RemoveRange(userLayerLineFeatures);

                context.USERLAYERLINEs.RemoveRange(userLayerLine);
            }

            if (userLayerPolygon.Count != 0)
            {
                //var id = userLayerPolygon.Select(p => p.UserLayerPolygonId).ToList();
                //var userLayerPolygonFeatures = context.UserLayerPolygonFeatures.Where(p => id.Contains(p.UserLayerPolygonId_fk)).ToList();
                //if (userLayerPolygonFeatures.Count != 0)
                //    context.UserLayerPolygonFeatures.RemoveRange(userLayerPolygonFeatures);

                context.USERLAYERPOLYGONs.RemoveRange(userLayerPolygon);
            }

            context.UserLayers.Remove(userLayer);
            context.SaveChanges();

            return new VM_Message()
            {
                Result = "OK",
                Message = Resources.Messages.DeleteSuccessful
            };
        }
    }
}
