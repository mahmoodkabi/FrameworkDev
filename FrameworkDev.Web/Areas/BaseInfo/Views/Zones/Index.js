let entityTitle = 'منطقه';
let baseApiAddr = '/api/Zonesapi/';

$(function () {

    //enable kendo validator for all form elements

    $('#ZoneForm').validate({ ignore: [] });
    $.validator.setDefaults({ ignore: [] });
});

$().ready(function () {
    readZone();
});

//نمایش اطلاعات پس از انتخاب نود از درخت
//let entityId = 0;
//let tName;
//let editmodeZone = false;

//load UC form based on selected item
function filterZone(e) {
    let dt = getSelectedNod(tName);
    entityId = dt.id;
    loadZonedata(dt);
    editmodeZone = false;
}

//handle click event of treeview
function treeViewClick() {
    tName = 'ZoneTreeview';
    filterZone();
}

//load selected UC data to form
function loadZonedata(dt) {
    makeComponentDisabled('Zone', true);
    prepareToolBarAction('Zone', true, true, true, false, false);
    $.ajax({
        url: baseApiAddr + dt.id,
        datatype: 'json',
        type: 'GET',
        success: function (res) {
            $('#ZoneId').val(res.ZoneId);
            $('#ParentId').val(res.ParentId);
           // $('#ZONPayBranchID_fk').val(res.ZONPayBranchID_fk);
           // $('#ZONInsBranchName').val(res.ZONInsBranchName);
            $('#Name').val(res.Name);
            $('#Code').val(res.Code);
            $('#WorkShopNo').val(res.WorkShopNo);
            $('#AccNo').val(res.AccNo);
            $('#Address').val(res.Address);
            $('#TelNo').val(res.TelNo);
          //  $('#ZONNote').val(res.ZONNote);
         //   $('#ZonActive').prop('checked', res.ZonActive);
        },
        error(err) {
            console.error(err.responseJSON.ExceptionMessage);
        }
    });
}
//event : handle click event of new UC button
function New() {
    if (getSelectedNod(tName) === null) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'یك ' + entityTitle + ' را انتخاب كنید', 'red', '', null);
        return;
    }
    //
    $('#ZoneId').val('');
    $('#ParentId').val('');
  //  $('#ZONPayBranchID_fk').val('');
   // $('#ZONInsBranchName').val('');
    $('#Name').val('');
    $('#Code').val('');
    $('#WorkShopNo').val('');
    $('#AccNo').val('');
    $('#Address').val('');
    $('#TelNo').val('');
    $('#ZONNote').val('');
    //$('#ZonActive').prop('checked', false);
    editmodeZone = false;
    makeComponentDisabled('Zone', false);
    makeInputClear('Zone');
    $('#ZoneId').prop('disabled', true);
    $('#ParentId').val(entityId);
    $('#ParentId').prop('disabled', true);
    prepareToolBarAction('Zone', false, false, false, true, true);
}

function Edit() {
    if (getSelectedNod(tName) === null) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'یك ' + entityTitle + ' را انتخاب كنید', 'red', '', null);
        return;
    }
    editmodeZone = true;
    makeComponentDisabled('Zone', false);
    prepareToolBarAction('Zone', false, false, false, true, true);
    $('#ZoneId').prop('disabled', true);
    $('#ParentId').prop('disabled', true);
}

function Delete() {
    if (getSelectedNod(tName) === null) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'یك ' + entityTitle + ' برای حذف انتخاب كنید', 'red', '', null);
    } else {
        ConfirmAlert('حذف ' + entityTitle + '', 'آیا از حذف این ' + entityTitle + ' اطمینان دارید؟', delItem);
    }
}

function Save() {
    if (getSelectedNod(tName) === null) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'یك ' + entityTitle + ' را انتخاب كنید', 'red', '', null);
        return;
    }

    if ($('#ZoneForm').valid()) {
        if (editmodeZone) {
            api_action('update');
        } else {
            api_action('insert');
        }
    }

    // makeInputClear('Workplace');
}

function Cancel() {
    prepareToolBarAction('Zone', true, true, true, false, false);
    editmodeZone = false;
    makeComponentDisabled('Zone', true);
}

let ds;
function readZone() {
    prepareToolBarAction("Zone", true, true, true, false, false);

    //editmodeChart = false;
    makeComponentDisabled("Zone", true);
    ds = new kendo.data.HierarchicalDataSource({
        transport: {
            read: {
                url: baseApiAddr + 'a/ZonesTree',
                datatype: 'json',

                type: 'GET'
            }
        },
        schema: {
            model: {
                id: 'Id',
                hasChildren: 'HasChildren'
            }
        }
    });

    $('#ZoneTreeview').kendoTreeView({
        dataSource: ds,
        loadOnDemand: true,
        dataTextField: 'Name',
        change: function () {
            treeViewClick();
        }
    });
}

function reloadTreeView() {
    let tree = $('#ZoneTreeview').data('kendoTreeView');
    tree.dataSource.read();
}

//make ajax calls to api layer
function api_action(_actiontype) {
    let _actionTitle = "";
    let _actionMethod = "";
    switch (_actiontype) {
        case "insert":
            _actionTitle = "ایجاد";
            _actionMethod = "POST";
            break;
        case "update":
            _actionTitle = "بروزرسانی";
            _actionMethod = "PUT";
            break;
        case "delete":
            _actionTitle = "حذف";
            _actionMethod = "DELETE";
            break;
        default: break;
    }
    $.ajax({
        url: baseApiAddr + entityId,
        type: _actionMethod,
        data: getBindedModel(),
        datatype: 'json',
        success: function (result) {
            if (!result) {
                ShowNotification('اشکال در ثبت تغییرات', 'مشكلی در ' + _actionTitle + ' ' + entityTitle + '', '' + entityTitle + ' ' + _actionTitle + ' نشد', 'error', 5000);
            } else {
                ShowNotification('موفق', '' + entityTitle + ' با موفقیت ' + _actionTitle + ' شد', 'success', 3000);
                prepareToolBarAction('Zone', true, true, true, false, false);
                makeComponentDisabled('Zone', true);
                reloadTreeView();
            }
        },
        error(err) {
            ShowNotification(err);
        }
    });
}

function delItem() {
    let _actionTitle = "حذف";
    let _actionMethod = "DELETE";

    $.ajax({
        url: baseApiAddr + entityId,
        type: _actionMethod,
        data: getBindedModel(),
        datatype: 'json',
        success: function (result) {
            if (!result) {
                Alert('fa fa-frown-o', 'مشكلی در ' + _actionTitle + ' ' + entityTitle + '', '' + entityTitle + ' ' + _actionTitle + ' نشد', 'red', '', null);
            } else {
                if (result.ZoneId === 0) {
                    Alert('fa fa-frown-o', 'پیغام هشدار', 'به دلیل در گردش بودن اطلاعات این ركورد قابل حذف نمی باشد', 'orange', '', null);
                    return false;
                }

                Alert('fa fa-smile-o', 'موفق', '' + entityTitle + ' با موفقیت ' + _actionTitle + ' شد', 'green', '', null);
                prepareToolBarAction('Zone', true, true, true, false, false);
                makeComponentDisabled('Zone', true);
                reloadTreeView();
            }
        },
        error(err) {

            //alert(err.responseJSON.ExceptionMessage);
            //console.error(err.responseJSON.ExceptionMessage);
        }
    });
}

//reverse model binding
function getBindedModel() {
    let request = {
        ZoneId: entityId,
        ParentId: $('#ParentId').val(),
     //   ZONPayBranchID_fk: $('#ZONPayBranchID_fk').val(),
      //  ZONInsBranchName: $('#ZONInsBranchName').val(),
        WorkShopNo: $('#WorkShopNo').val(),
        Name: $('#Name').val(),
        Code: $('#Code').val(),
        AccNo: $('#AccNo').val(),
        Address: $('#Address').val(),
        TelNo: $('#TelNo').val(),
      // ZONNote: $('#ZONNote').val(),
       // ZONActive: ($('#ZonActive').val() === "" ? false : true),
        Type: 0,
    };
    return request;
}

function renderResult(res) {
    let description = '' + '\n\n';
    if (res.length > 0) {  // فیلدهایی كه دارای ولیدیشن هستند
        for (let i = 0; i < res.length; i++) {
            description += res[i].Massege + '\n';
        }
        return description;
    }
}

function getSelectedNod(treeviewid) {
    try {
        let treeview = $('#' + treeviewid).data('kendoTreeView');
        return treeview.dataItem(treeview.select());
    } catch (err) {
        return null;
    }
}
