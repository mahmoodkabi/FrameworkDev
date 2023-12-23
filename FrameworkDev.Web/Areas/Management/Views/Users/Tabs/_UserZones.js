function showZoneDialog(e) {
    $('#ZoneDialog').data('kendoDialog').open();
}

function ZoneDialog_Open() {
    $('#ZoneGrid').data('kendoGrid').dataSource.read();
}

function ZoneDialog_Close(e) {
    var UserZoneGrid = $("#UserZonesGrid").data("kendoGrid");
    UserZoneGrid.dataSource.read();
}

function ZoneDialog_Ok(e) {
    var grid = $('#ZoneGrid').data('kendoGrid');
    var selectedZones = grid.selectedKeyNames();

    if ($(selectedZones).length > 0) {
        var exlist = [];

        $(selectedZones).each((index, zoneId) => {
            var model = {
                USZUserId_fk: _userId,
                USZZoneId_fk: zoneId
            };
            $.ajax({
                type: 'POST',
                url: '/api/UserZonesApi',
                data: JSON.stringify(model),
                contentType: 'application/json',
                success: function (res) {

                    //success = true;
                },
                error: function (e) {
                    $(exlist).push(e);
                    success = false;
                }
            });
        });

        if (exlist.length === 0) {
            $('#UserId').val(_userId);
            $('#ZoneGrid').data('kendoGrid').clearSelection();
            $('#UserZonesGrid').data('kendoGrid').dataSource.read();
            $('#ZoneDialog').data('kendoDialog').close();
            return true;
        } else {
            $(exlist).each((index, err) => { console.error(Err); });
            ShowNotification('در اجرای عملیات خطایی رخ داده است.', 'خطا', 'error', 300);
            return false;
        }
    } else {

        //ShowNotification('لطفا یک یا چند منطقه انتخاب کنید.', 'عدم انتخاب منطقه', 'error', 300);
        return false;
    }
}
function ZoneDialog_Cancel(e) { }
function getZoneGridCheckedItems(e) {

    //var UserZonesGrid = $("#UserZonesGrid").data("kendoGrid");
    //var selectedItem = UserZonesGrid.dataItem(UserZonesGrid.select());
    //  UserId= selectedItem.UserId;

    var grid = $("#ZoneGrid").data("kendoGrid");

    var selected_zones = grid.selectedKeyNames();

    //  alert("The selected product ids are: [" + grid.selectedKeyNames().join(", ") + "]");




    $.ajax({
        type: "post",
        //url: '/api/CitizenInfoApi/CitizenAllInfo',
        url: '@Url.Action("SaveZones", "UserZones")',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'UserId': UserId, 'selected_zones': selected_zones }),
        success: function (res) {
            $("#UserId").val(UserId);
            $('#ZoneDialog').data("kendoDialog").close();
            userZonesGrid = $('#UserZonesGrid').data("kendoGrid");
            userZonesGrid.dataSource.read();
            ZoneGrid = $("#ZoneGrid").data("kendoGrid");
            ZoneGrid.clearSelection();

        },
        error: function (e) {

        }

    });

}
