using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

using System;
using System.Web;
using System.Web.Mvc;

namespace FrameworkDev.Web.Helpers.KendoMVC
{
    public static class MyGridHelper
    {
        public static GridBuilder<T> MyGrid<T>(this HtmlHelper helper, string name, GridEditMode? editableMode = null, bool groupable = false, bool pageable = true, bool defaultToolbar = false, bool virtualScrolling = true, bool fiterable = true, GridFilterMode gridFilterMode = GridFilterMode.Menu) where T : class
        {
            string prjName = Utility.GetProjectName();
            string grdName = name.ToLower().Replace("grid", "");
            string pdtString = Utility.ToPersianDateTimeString(DateTime.Now);
            string usrString = HttpContext.Current.User.Identity.Name;

            GridBuilder<T> grid = helper
                .Kendo()
                .Grid<T>()
                .ColumnMenu()
                .Excel(excel => excel.FileName(string.Format("{0}-{1}-{2}-Export-{3}.xlsx", usrString, prjName, grdName, pdtString)).Filterable(true).AllPages(true))
                .Pdf(pdf => pdf.FileName(string.Format("{0}-{1}-{2}-Export-{3}.pdf", usrString, prjName, grdName, pdtString)))
                .Filterable(x => x.Enabled(fiterable).Mode(gridFilterMode))
                .Groupable(g => g.Enabled(groupable))
                .Name(name)
                .Navigatable(x => x.Enabled(true))
                .NoRecords("داده ای موجود نیست.")
                .Pageable(p => p.Enabled(pageable).Refresh(pageable))
                .Scrollable(s => s.Enabled(true).Virtual(virtualScrolling).Height("auto"))
                .Selectable(selectable => { selectable.Mode(GridSelectionMode.Single); selectable.Type(GridSelectionType.Row); })
                .Sortable(sortable => { sortable.SortMode(GridSortMode.SingleColumn); });

            switch (editableMode)
            {
                case GridEditMode.InLine:
                    grid.Editable(editable => editable.Mode(GridEditMode.InLine).ConfirmDelete("حذف اطلاعات").DisplayDeleteConfirmation("آیا از حذف اطلاعات اطمینان دارید؟"));

                    if (defaultToolbar)
                    {
                        grid.ToolBar(toolbar => { toolbar.Create().Text(string.Empty); toolbar.Excel().Text(string.Empty); toolbar.Pdf().Text(string.Empty); });
                    }
                    break;

                case GridEditMode.PopUp:
                    grid.Editable(editable => editable.Mode(GridEditMode.PopUp).ConfirmDelete("حذف اطلاعات").DisplayDeleteConfirmation("آیا از حذف اطلاعات اطمینان دارید؟"));

                    if (defaultToolbar)
                    {
                        grid.ToolBar(toolbar => { toolbar.Create().Text(string.Empty); toolbar.Excel().Text(string.Empty); toolbar.Pdf().Text(string.Empty); });
                    }
                    break;

                case GridEditMode.InCell:
                    grid.Editable(editable => editable.Mode(GridEditMode.InCell).ConfirmDelete("حذف اطلاعات").DisplayDeleteConfirmation("آیا از حذف اطلاعات اطمینان دارید؟"));
                    if (defaultToolbar)
                    {
                        grid.ToolBar(toolbar => { toolbar.Create().Text(string.Empty); toolbar.Save().Text(string.Empty).CancelText(string.Empty).SaveText(string.Empty); toolbar.Excel().Text(string.Empty); toolbar.Pdf().Text(string.Empty); });
                    }
                    break;

                case null:
                    grid.Editable(editable => editable.Enabled(false));
                    if (defaultToolbar)
                    {
                        grid.ToolBar(toolbar => { toolbar.Excel().Text(string.Empty); toolbar.Pdf().Text(string.Empty); });
                    }
                    break;

                default:
                    break;
            }

            return grid;
        }
    }
}
