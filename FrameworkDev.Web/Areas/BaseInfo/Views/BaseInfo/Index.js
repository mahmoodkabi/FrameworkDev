let entityTitle = 'اطلاعات پایه';
let basePageAddr = '/BaseInfo/BaseInfo/';
var url = "";

$(function () {

    //enable kendo validator for all form elements
    $('#BaseInfoForm').kendoValidator();
    $('#BaseInfoForm').validate({ ignore: [] });
    $.validator.setDefaults({ ignore: [] });
});

$().ready(function () {
    readBaseInfo();
});

//نمایش اطلاعات پس از انتخاب نود از درخت اطلاعات پایه
let entityId = 0;
let tName;
let editmodeBaseInfo = false;


//handle click event of treeview
function treeViewClick() {
    tName = 'BINTreeview';
    filterBaseInfo();
}

//load UC form based on selected item
function filterBaseInfo(e) {
    let dt = getSelectedNod(tName);
    entityId = dt.id;
    loadBaseInfodata(dt);
    editmodeBaseInfo = false;
}

//load selected UC data to form
function loadBaseInfodata(dt) {
    makeComponentDisabled('BaseInfo', true);
    prepareToolBarAction('BaseInfo', true, true, true, false, false);
    $.ajax({
        url: basePageAddr + "GetBaseInfo?id=" + dt.id,
        datatype: 'json',
        type: 'GET',
        success: function (res) {
            $('#BaseID').val(res.BaseID);
            $('#ParentID').val(res.ParentID);
            $('#BaseName').val(res.BaseName);
            $('#BaseCode').val(res.BaseCode);
            $('#Description').val(res.Description);
            $('#Active').prop('checked', res.Active);
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
    $('#BaseID').val('');
    $('#BaseName').val('');
    $('#BaseCode').val('');
    $('#Description').val('');
    $('#Active').prop('checked', false);
    editmodeBaseInfo = false;
    makeComponentDisabled('BaseInfo', false);
    makeInputClear('BaseInfo');
    $('#BaseID').prop('disabled', true);
    $('#ParentID').val(entityId);
    $('#ParentID').prop('disabled', true);
    prepareToolBarAction('BaseInfo', false, false, false, true, true);
}

//event : handle click event of edit UC button
function Edit() {
    if (getSelectedNod(tName) === null) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'یك ' + entityTitle + ' را انتخاب كنید', 'red', '', null);
        return;
    }
    editmodeBaseInfo = true;
    makeComponentDisabled('BaseInfo', false);
    prepareToolBarAction('BaseInfo', false, false, false, true, true);
    $('#BaseID').prop('disabled', true);
    $('#ParentID').prop('disabled', true);
}

function Delete() {
    if (getSelectedNod(tName) === null) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'یك ' + entityTitle + ' برای حذف انتخاب كنید', 'red', '', null);
    } else {
        ConfirmAlert('حذف ' + entityTitle + '', 'آیا از حذف این ' + entityTitle + ' اطمینان دارید؟', delItem);
    }
}

//انتخاب دکمه ذخیره
function Save() {
    if ($('#BaseInfoForm').valid()) {
        if (editmodeBaseInfo) {
            api_action('update')
        } else {
            api_action('insert');
        }
    }

    // makeInputClear('BaseInfo');
}

function Cancel() {
    prepareToolBarAction('BaseInfo', true, true, true, false, false);
    editmodeBaseInfo = false;
    makeComponentDisabled('BaseInfo', true);
}

let ds;
function readBaseInfo() {
    prepareToolBarAction("BaseInfo", true, true, true, false, false);
    makeComponentDisabled("BaseInfo", true);
    ds = new kendo.data.HierarchicalDataSource({
        transport: {
            read: {
                url: basePageAddr + 'GetBaseInfoTree',
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

    $('#BINTreeview').kendoTreeView({
        dataSource: ds,
        loadOnDemand: true,
        dataTextField: 'Name',
        change: function () {
            treeViewClick();
        }
    });
}

function reloadTreeView() {
    let tree = $('#BINTreeview').data('kendoTreeView');
    tree.dataSource.read();
}

//make ajax calls to api layer
function api_action(_actiontype) {
    let _actionTitle = "";
    let _actionMethod = "";
    url = ""

    switch (_actiontype) {
        case "insert":
            _actionTitle = "ایجاد";
            _actionMethod = "POST";
            url = basePageAddr + "InsertBaseInfo";
            break;
        case "update":
            _actionTitle = "بروزرسانی";
            _actionMethod = "POST";
            url = basePageAddr + "UpdateBaseInfo";
            break;
        case "delete":
            _actionTitle = "حذف";
            _actionMethod = "POST";
            url = basePageAddr + "DeleteBaseInfo";
            break;
        default: break;
    }
    $.ajax({
        url: url ,
        type: _actionMethod,
        data: getBindedModel(),
        datatype: 'json',
        success: function (result) {
            if (!result) {
                Alert('fa fa-frown-o', 'مشكلی در ' + _actionTitle + ' ' + entityTitle + '', '' + entityTitle + ' ' + _actionTitle + ' نشد', 'red', '', null);
            } else {
                Alert('fa fa-smile-o', 'موفق', '' + entityTitle + ' با موفقیت ' + _actionTitle + ' شد', 'green', '', null);
                prepareToolBarAction('BaseInfo', true, true, true, false, false);
                makeComponentDisabled('BaseInfo', true);
                reloadTreeView();
            }
        },
        error(err) {
        }
    });
}

function delItem() {
    let _actionTitle = "حذف";
    let _actionMethod = "POST";
    url = basePageAddr + "DeleteBaseInfo";

    $.ajax({
        url: url,
        type: _actionMethod,
        data: getBindedModel(),
        datatype: 'json',
        success: function (result) {
            if (!result) {
                Alert('fa fa-frown-o', 'مشكلی در ' + _actionTitle + ' ' + entityTitle + '', '' + entityTitle + ' ' + _actionTitle + ' نشد', 'red', '', null);
            } else {
                if (result.BaseID === 0) {
                    Alert('fa fa-frown-o', 'پیغام هشدار', 'به دلیل در گردش بودن اطلاعات این ركورد قابل حذف نمی باشد', 'orange', '', null);
                    return false;
                }

                Alert('fa fa-smile-o', 'موفق', '' + entityTitle + ' با موفقیت ' + _actionTitle + ' شد', 'green', '', null);
                prepareToolBarAction('BaseInfo', true, true, true, false, false);
                makeComponentDisabled('BaseInfo', true);
                reloadTreeView();
            }
        },
        error(err) {
        }
    });
}

//reverse model binding
function getBindedModel() {
    let request = {
        BaseID: entityId,
        ParentID: $('#ParentID').val(),
        BaseName: $('#BaseName').val(),
        BaseCode: $('#BaseCode').val(),
        Description: $('#Description').val(),
        Active: ($('#Active').val() === "" ? false : true),
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
