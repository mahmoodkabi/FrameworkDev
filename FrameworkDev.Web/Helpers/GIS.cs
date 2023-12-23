using DotSpatial.Projections;
using FrameworkDev.Web.Areas.WebGIS.Models;
using FrameworkDev.Web.Models;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace FrameworkDev.Web.Helpers
{
    public static class GIS
    {
        public static int WKID
        {
            get { return 4326; }
        }

        public static List<VM_GeometryType> ExtractDataFromShapeFile(DotSpatial.Data.Shapefile shapeFile)
        {
            try
            {
                string strShape = "";
                List<VM_GeometryType> lstGeometries = new List<VM_GeometryType>();
                List<VM_Feature> lstAttributes = new List<VM_Feature>();
                int id = 0;
                string attrValue = "";

                //اگر فی و لاندا نبود به پروجکشن فی و لاندا تبدیل کن
                // WGS 4326
                if (!shapeFile.Projection.IsLatLon)
                {
                    shapeFile.Reproject(
                        DotSpatial.Projections.KnownCoordinateSystems.Geographic.World.WGS1984);
                }

                //اگر شیپ فایل پولیگون باشد
                if (shapeFile.FeatureType.ToString() == "Polygon")
                {
                    foreach (var feature in shapeFile.Features)
                    {
                        id += 1;

                        //خواندن فیچرهای عارضه
                        lstAttributes = new List<VM_Feature>();
                        for (int i = 0; i < feature.DataRow.Table.Columns.Count; i++)
                        {
                            // تبدیل به کارکتر فارسی
                            attrValue = Utility.ConvertCodepageToPersian(feature.DataRow[i].ToString());

                            lstAttributes.Add(new VM_Feature()
                            {
                                AttributeName = feature.DataRow.Table.Columns[i].ColumnName,
                                AttributeValue = attrValue,
                            });
                        }

                        //خواندن مختصات عارضه
                        strShape = "";
                        foreach (var coordinate in feature.BasicGeometry.Coordinates)
                        {
                            strShape += coordinate.X.ToString() + " " + coordinate.Y.ToString() + ", ";
                        }

                        strShape = strShape.Remove(strShape.Length - 2, 1);
                        strShape = strShape.Replace('/', '.');

                        var polygeoGeometry = DbGeometry.PolygonFromText("POLYGON((" + strShape + "))", GIS.WKID);
                        lstGeometries.Add(new VM_GeometryType()
                        {
                            id = id,
                            FileGeometry = polygeoGeometry,
                            StringFileGeometry = strShape,
                            Type = "POLYGON",
                            Features = lstAttributes,
                            Message = new VM_Message()
                            {
                                Result = "OK"
                            }
                        });
                    }
                }
                //اگر شیپ فایل خط باشد
                else if (shapeFile.FeatureType.ToString() == "Line")
                {
                    foreach (var feature in shapeFile.Features)
                    {
                        id += 1;
                        //خواندن فیچرهای عارضه
                        lstAttributes = new List<VM_Feature>();
                        for (int i = 0; i < feature.DataRow.Table.Columns.Count; i++)
                        {
                            // تبدیل به کارکتر فارسی
                            attrValue = Utility.ConvertCodepageToPersian(feature.DataRow[i].ToString());

                            lstAttributes.Add(new VM_Feature()
                            {
                                AttributeName = feature.DataRow.Table.Columns[i].ColumnName,
                                AttributeValue = attrValue,
                            });
                        }

                        //خواندن مختصات عارضه
                        strShape = "";
                        foreach (var coordinate in feature.BasicGeometry.Coordinates)
                        {
                            strShape += coordinate.X.ToString() + " " + coordinate.Y.ToString() + ", ";
                        }

                        strShape = strShape.Remove(strShape.Length - 2, 1);
                        strShape = strShape.Replace('/', '.');

                        var lineGeometry = DbGeometry.LineFromText("LINESTRING(" + strShape + ")", GIS.WKID);
                        lstGeometries.Add(new VM_GeometryType()
                        {
                            id = id,
                            FileGeometry = lineGeometry,
                            StringFileGeometry = strShape,
                            Type = "LINESTRING",
                            Features = lstAttributes,
                            Message = new VM_Message()
                            {
                                Result = "OK"
                            }
                        });
                    }
                }
                //اگر شیپ فایل نقطه باشد
                else if (shapeFile.FeatureType.ToString() == "Point")
                {
                    foreach (var feature in shapeFile.Features)
                    {
                        id += 1;
                        //خواندن فیچرهای عارضه
                        lstAttributes = new List<VM_Feature>();
                        for (int i = 0; i < feature.DataRow.Table.Columns.Count; i++)
                        {
                            // تبدیل به کارکتر فارسی
                            attrValue = Utility.ConvertCodepageToPersian(feature.DataRow[i].ToString());

                            lstAttributes.Add(new VM_Feature()
                            {
                                AttributeName = feature.DataRow.Table.Columns[i].ColumnName,
                                AttributeValue = attrValue,
                            });
                        }

                        //خواندن مختصات عارضه
                        strShape = "";
                        foreach (var coordinate in feature.BasicGeometry.Coordinates)
                        {
                            strShape += coordinate.X.ToString() + " " + coordinate.Y.ToString() + ", ";
                        }

                        strShape = strShape.Remove(strShape.Length - 2, 1);
                        strShape = strShape.Replace('/', '.');

                        var pointGeometry = DbGeometry.PointFromText("POINT(" + strShape + ")", GIS.WKID);
                        lstGeometries.Add(new VM_GeometryType()
                        {
                            id = id,
                            FileGeometry = pointGeometry,
                            StringFileGeometry = strShape,
                            Type = "POINT",
                            Features = lstAttributes,
                            Message = new VM_Message()
                            {
                                Result = "OK"
                            }
                        });
                    }
                }

                return lstGeometries;
            }
            catch (Exception ex)
            {
                List<VM_GeometryType> lstGeometries = new List<VM_GeometryType>();
                lstGeometries.Add(new VM_GeometryType()
                {
                    Message = new VM_Message()
                    {
                        Result = "Error",
                        Description = ex.ToString() + (ex.Message ?? "") + (ex.Message == null ? "" : (ex.InnerException.Message ?? "")),
                    }
                });

                return lstGeometries;
            }
        }



        public static List<VM_GeometryType> ExtractDataFromKML(KmlFile kmlFile)
        {
            try
            {
                string strShape = "";
                List<VM_GeometryType> lstGeometries = new List<VM_GeometryType>();

                //پلیگون
                foreach (var poly in kmlFile.Root.Flatten().OfType<SharpKml.Dom.Polygon>())
                {
                    var rings = poly.OuterBoundary.LinearRing.Coordinates.ToList();
                    foreach (var item in rings)
                    {
                        strShape += item.Longitude + " " + item.Latitude + ", ";
                    }
                    strShape = strShape.Remove(strShape.Length - 2, 1);
                    strShape = strShape.Replace('/', '.');

                    var polygeoGeometry = DbGeometry.PolygonFromText("POLYGON((" + strShape + "))", GIS.WKID);
                    lstGeometries.Add(new VM_GeometryType()
                    {
                        FileGeometry = polygeoGeometry,
                        Type = "POLYGON",
                        Message = new VM_Message()
                        {
                            Result = "OK"
                        }
                    });
                }


                // نقطه
                foreach (var poly in kmlFile.Root.Flatten().OfType<SharpKml.Dom.Point>())
                {
                    var rings = poly.Coordinate;
                    strShape = rings.Longitude + " " + rings.Latitude;
                    strShape = strShape.Replace('/', '.');

                    var pointGeometry = DbGeometry.PointFromText("POINT(" + strShape + ")", GIS.WKID);
                    lstGeometries.Add(new VM_GeometryType()
                    {
                        FileGeometry = pointGeometry,
                        Type = "POINT",
                        Message = new VM_Message()
                        {
                            Result = "OK"
                        }
                    });
                }


                //خط
                foreach (var poly in kmlFile.Root.Flatten().OfType<SharpKml.Dom.LineString>())
                {
                    var rings = poly.Coordinates.ToList();
                    foreach (var item in rings)
                    {
                        strShape += item.Longitude + " " + item.Latitude + ", ";
                    }
                    strShape = strShape.Remove(strShape.Length - 2, 1);
                    strShape = strShape.Replace('/', '.');

                    var lineGeometry = DbGeometry.LineFromText("LINESTRING(" + strShape + ")", GIS.WKID);
                    lstGeometries.Add(new VM_GeometryType()
                    {
                        FileGeometry = lineGeometry,
                        Type = "LINESTRING",
                        Message = new VM_Message()
                        {
                            Result = "OK"
                        }
                    });
                }

                return lstGeometries;
            }
            catch (Exception ex)
            {
                List<VM_GeometryType> lstGeometries = new List<VM_GeometryType>();
                lstGeometries.Add(new VM_GeometryType()
                {
                    Message = new VM_Message()
                    {
                        Result = "Error",
                        Description = ex.ToString() + (ex.Message ?? "") + (ex.Message == null ? "" : (ex.InnerException.Message ?? "")),
                    }
                });

                return lstGeometries;

            }
        }

        /// <summary>
        /// Extracts the content from a .zip file inside an specific folder.
        /// </summary>
        /// <param name="FileZipPath"></param>
        /// <param name="password"></param>
        /// <param name="OutputFolder"></param>
        public static void ExtractZipContent(string FileZipPath, string password, string OutputFolder)
        {
            ZipFile file = null;
            try
            {
                FileStream fs = System.IO.File.OpenRead(FileZipPath);
                file = new ZipFile(fs);

                if (!String.IsNullOrEmpty(password))
                {
                    // AES encrypted entries are handled automatically
                    file.Password = password;
                }

                foreach (ZipEntry zipEntry in file)
                {
                    if (!zipEntry.IsFile)
                    {
                        // Ignore directories
                        continue;
                    }

                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    // 4K is optimum
                    byte[] buffer = new byte[4096];
                    Stream zipStream = file.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(OutputFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = System.IO.File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (file != null)
                {
                    file.IsStreamOwner = true; // Makes close also shut the underlying stream
                    file.Close(); // Ensure we release resources
                }
            }
        }


        /// <summary>
        /// تبديل از يك پروجكشن به پروجكشن ديگر
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="fromEpsgCode"></param>
        /// <param name="toEpsgCode"></param>
        /// <returns></returns>
        public static string ConvertPoint(string xy, int fromEpsgCode, int toEpsgCode)
        {
            double[] xyDouble = new double[2];
            xyDouble[0] = Convert.ToDouble(xy.Split(' ')[0].Split('.')[0]);
            xyDouble[1] = Convert.ToDouble(xy.Split(' ')[1].Split('.')[0]);
            //An array for the z coordinate
            double[] z = new double[1];
            z[0] = 0;
            var fEpsgCode = ProjectionInfo.FromEpsgCode(fromEpsgCode);
            var tEpsgCode = ProjectionInfo.FromEpsgCode(toEpsgCode);
            Reproject.ReprojectPoints(xyDouble, z, fEpsgCode, tEpsgCode, 0, 1);

            var result = xyDouble[0].ToString() + " " + xyDouble[1].ToString();
            return result;
        }


        /// <summary>
        /// تبديل از يك پروجكشن به پروجكشن ديگر
        /// </summary>
        /// <param name="lstXY"></param>
        /// <param name="fromEpsgCode"></param>
        /// <param name="toEpsgCode"></param>
        /// <returns></returns>
        public static List<string> ConvertListOfPoint(List<string> lstXY, int fromEpsgCode, int toEpsgCode)
        {
            List<string> res = new List<string>();
            //An array for the z coordinate
            double[] z = new double[1];
            z[0] = 0;
            var fEpsgCode = ProjectionInfo.FromEpsgCode(fromEpsgCode);
            var tEpsgCode = ProjectionInfo.FromEpsgCode(toEpsgCode);

            foreach (var item in lstXY)
            {
                double[] xyDouble = new double[2];
                xyDouble[0] = Convert.ToDouble(item.Split(' ')[0]);
                xyDouble[1] = Convert.ToDouble(item.Split(' ')[1]);

                Reproject.ReprojectPoints(xyDouble, z, fEpsgCode, tEpsgCode, 0, 1);
                res.Add(xyDouble[0].ToString() + " " + xyDouble[1].ToString());
            }

            return res;
        }

    }


}