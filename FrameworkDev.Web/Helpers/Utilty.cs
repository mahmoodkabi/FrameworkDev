using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using FrameworkDev.Web;
using OfficeOpenXml;

public static class Utility
{
    public static bool IsDebug(this HtmlHelper htmlHelper)
    {
#if DEBUG
        return true;
#else
      return false;
#endif
    }

    public static DateTime ToPersianDateTime(this DateTime dt)
    {
        PersianCalendar pc = new PersianCalendar();
        int year = pc.GetYear(dt);
        int month = pc.GetMonth(dt);
        int day = pc.GetDayOfMonth(dt);
        int hour = pc.GetHour(dt);
        int min = pc.GetMinute(dt);

        return new DateTime(year, month, day, hour, min, 0);
    }

    public static DateTime ToPersianDate(this DateTime dt)
    {
        PersianCalendar pc = new PersianCalendar();
        return pc.ToDateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
    }

    public static string ToPersianDateString(this DateTime? dt)
    {
        if (dt.HasValue)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                var month = pc.GetMonth(dt.Value).ToString().Length == 2 ? pc.GetMonth(dt.Value).ToString() :  "0" + pc.GetMonth(dt.Value).ToString();
                var day = pc.GetDayOfMonth(dt.Value).ToString().Length == 2 ? pc.GetDayOfMonth(dt.Value).ToString() :  "0" + pc.GetDayOfMonth(dt.Value).ToString();
                return string.Format("{0}/{1}/{2}", pc.GetYear(dt.Value), month, day);

            }
            catch (Exception)
            {

                return "";
            }
        }
        else { return ""; }
    }

    public static string ToPersianDateTimeString(this DateTime? dt)
    {
        if (dt.HasValue)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}", pc.GetYear(dt.Value), pc.GetMonth(dt.Value), pc.GetDayOfMonth(dt.Value), dt?.Hour, dt?.Minute, dt?.Second);
        }
        else { return ""; }
    }

    public static DateTime? ToMiladiDate(this string dt)
    {
        if (dt == null || dt == "" || dt.Length != 10)
        {
            return null;
        }

        try
        {
            PersianCalendar pc = new PersianCalendar();
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(5, 2));
            int day = Convert.ToInt32(dt.Substring(8, 2));
            return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
        catch (Exception)
        {

            return null;
        }

    }


    public static DateTime? PersianToMiladiDate(this string dt)
    {
        if (dt == null || dt == "" || dt.Length != 10)
        {
            return null;
        }

        try
        {
            PersianCalendar pc = new PersianCalendar();
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(5, 2));
            int day = Convert.ToInt32(dt.Substring(8, 2));
            DateTime dtM = new DateTime(year, month, day, pc);

            return DateTime.Parse(dtM.ToString(CultureInfo.CreateSpecificCulture("en-US"))); ;

        }
        catch (Exception)
        {

            return null;
        }

    }


    public static DateTime? Create_en_US_Culture(DateTime? datetime)
    {
        if (datetime == null || datetime.ToString().Length < 20)
        {
            return null;
        }

        return DateTime.Parse(datetime.ToString().Substring(0, 19), CultureInfo.CreateSpecificCulture("en-US"));
    }

    public static MvcHtmlString ClientIdFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
    {
        return MvcHtmlString.Create(
              htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(
                  ExpressionHelper.GetExpressionText(expression)));
    }

    public static TimeSpan CompareStringDate(string date1, string date2)
    {
        DateTime realdt1 = DateTime.ParseExact(date1, "yyyy/MM/dd", CultureInfo.InvariantCulture);
        PersianCalendar pc1 = new PersianCalendar();
        DateTime dt1 = pc1.ToDateTime(realdt1.Year, realdt1.Month, realdt1.Day, realdt1.Hour, realdt1.Minute, realdt1.Second, realdt1.Millisecond);
        DateTime realdt2 = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
        PersianCalendar pc2 = new PersianCalendar();
        DateTime dt2 = pc2.ToDateTime(realdt2.Year, realdt2.Month, realdt2.Day, realdt2.Hour, realdt2.Minute, realdt2.Second, realdt2.Millisecond);
        TimeSpan diff = dt2 - dt1;
        return diff;
    }

    public static TimeSpan CompareStringWithToday(string date1)
    {
        DateTime today = DateTime.Now.ToLocalTime();
        DateTime realdt1 = DateTime.ParseExact(date1, "yyyy/MM/dd", CultureInfo.InvariantCulture);
        PersianCalendar pc1 = new PersianCalendar();
        DateTime dt1 = pc1.ToDateTime(realdt1.Year, realdt1.Month, realdt1.Day, realdt1.Hour, realdt1.Minute, realdt1.Second, realdt1.Millisecond);
        TimeSpan diff = dt1 - today;
        return diff;
    }

    public static string GetProjectName()
    {
        return new RouteConfig().GetType().Namespace.Split('.')[0];
    }

    public static string GetAuthCookieName()
    {
        return GetProjectName() + "Auth";
    }

    public static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

    public static byte[] Zip(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);

        MemoryStream msi = new MemoryStream(bytes);

        MemoryStream mso = new MemoryStream();

        GZipStream gs = new GZipStream(mso, CompressionMode.Compress);

        try
        {
            CopyTo(msi, gs);

            return mso.ToArray();
        }
        finally
        {
            gs.Dispose();
            msi.Dispose();
        }
    }

    public static string Unzip(byte[] bytes)
    {
        using (MemoryStream msi = new MemoryStream(bytes))
        using (MemoryStream mso = new MemoryStream())
        using (GZipStream gs = new GZipStream(msi, CompressionMode.Decompress))
        {
            CopyTo(gs, mso);
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }

    public static string Base64Zip(string str)
    {
        return Convert.ToBase64String(Zip(str));
    }

    public static string Base64Unzip(string str)
    {
        return Unzip(Convert.FromBase64String(str));
    }

    public static DataTable ReadExcelFile(string filePath, string sheetname = "Sheet1")
    {
        DataTable dt = new DataTable();


        string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=YES;\";";

        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        excelConnection.Open();

        string query = string.Format("Select * from [{0}$]", sheetname);
        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
        {
            dataAdapter.Fill(dt);
        }

        excelConnection.Close();

        return dt;
    }

    public static MemoryStream GetExcelSheet<T>(IQueryable<T> data)
    {
        using (ExcelPackage package = new ExcelPackage())
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells["A1"].LoadFromCollection(data, true, OfficeOpenXml.Table.TableStyles.Dark11).AutoFitColumns();
            package.Save();
            MemoryStream stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }
    }

    public static List<T> ToList<T>(this DataTable tbl) where T : new()
    {
        List<T> results = new List<T>();

        foreach (DataRow row in tbl.Rows)
        {
            results.Add(row.ToObject<T>());
        }

        return results;
    }

    public static T ToObject<T>(this DataRow dataRow) where T : new()
    {
        T item = new T();
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            PropertyInfo property = item.GetType().GetProperty(column.ColumnName);

            if (property != null && dataRow[column] != DBNull.Value)
            {
                try
                {
                    object result = null;
                    string src = dataRow[column].ToString();

                    if (property.PropertyType.FullName.Contains("System.Byte"))
                    {
                        result = byte.Parse(src);
                    }
                    else if (property.PropertyType.FullName.Contains("System.Int16"))
                    {
                        result = short.Parse(src);
                    }
                    else if (property.PropertyType.FullName.Contains("System.Int32"))
                    {
                        result = int.Parse(src);
                    }
                    else if (property.PropertyType.FullName.Contains("System.Int64"))
                    {
                        result = long.Parse(src);
                    }
                    else if (property.PropertyType.FullName.Contains("System.Boolean"))
                    {
                        result = bool.Parse(src);
                    }
                    else
                    {
                        result = Convert.ChangeType(dataRow[column], property.PropertyType);
                    }

                    property.SetValue(item, result, null);
                }
                catch (Exception) { }
            }
        }

        return item;
    }

    public static DataTable CopyGenericToDataTable<T>(this IEnumerable<T> items)
    {
        var properties = typeof(T).GetProperties();
        var result = new DataTable();

        //Build the columns
        foreach (var prop in properties)
        {
            result.Columns.Add(prop.Name, prop.PropertyType);
        }

        //Fill the DataTable
        foreach (var item in items)
        {
            var row = result.NewRow();

            foreach (var prop in properties)
            {
                var itemValue = prop.GetValue(item, new object[] { });
                row[prop.Name] = itemValue;
            }

            result.Rows.Add(row);
        }

        return result;
    }

    /// <summary>
    /// تبدیل به کارکتر فارسی
    /// </summary>
    /// <param name="attrValue"></param>
    /// <returns></returns>
    public static string ConvertCodepageToPersian(string attrValue)
    {
        string asPersianString = "";
        byte[] asIso88591Bytes;

        asIso88591Bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(attrValue);
        asPersianString = Encoding.GetEncoding("windows-1256").GetString(asIso88591Bytes);

        //اگر به درستی به کارکتر فارسی تبدیل نشده بود
        // متن اصلی را قرار بده
        if (asPersianString.Contains("??"))
            asPersianString = attrValue;

        return asPersianString;
    }
}
