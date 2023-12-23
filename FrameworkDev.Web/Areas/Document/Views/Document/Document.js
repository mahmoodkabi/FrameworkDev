
//انتخاب دکمه نمایش لیست پیوست
function openDialog(tblName, rowId,viewMode=false) {
    TableName = tblName;

    RowId = rowId;

    //set in global value
    DocViewMode = viewMode;
    
    //open panel
    var Doc = $('#DocumentDialog').data("kendoWindow");
    Doc.maximize();
    Doc.open();

    //manage View panel mode
    if (DocViewMode) {
        $("#AddDocumentPanel").hide();
    }
    else {
        $("#AddDocumentPanel").show();
    }

    //load datat in grid
    LoadGrid();
}

// نمایش لیست پیوست در حالت بدون دیالوگ
function openDoc(tblName, rowId, viewMode=false) {
    TableName = tblName;

    RowId = rowId;

    //set in global value
    DocViewMode = viewMode;

    
    //manage View panel mode
    if (DocViewMode) {
        $("#AddDocumentPanel").hide();
    }
    else {
        $("#AddDocumentPanel").show();
    }

    //load datat in grid
    LoadGrid();
}

//بستن دیالوگ
function onClose() {
    $("#showDialogBtn").fadeIn();
}

//نمایش دیالوگ
function onOpen() {
    $("#showDialogBtn").fadeOut();
}

//Document ==============================
//Golobal Variable
var DocId = 0;
var TableName = "";
var RowId = "";
var DocName = "پیوست";
//فلک بررسی نمایشی بودن لیست 
var DocViewMode = false;

//بارگذاری جدول پیوست های مرتبط
function LoadGrid() {
    var param = prepareDocumentParam();
    $.ajax(
        {
            type: "POST",
            url: '/Document/Document/GetDocList',
            data: JSON.stringify(param),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $("#DocumentGrid").data("kendoGrid").dataSource.data(result);
                //بررسی نمایش یا عدم نمایش ستون عملیات
                OperationColumnAccess();
            },
            error: function (message) {
            }
        });
}

// افزودن فایل
function AddDocument() {
    $.ajax({
        url: '/Document/Document/CreateDocNew',
        data: { NewDoc: prepareDocumentParam() },
        datatype: "json",
        type: "POST",
        success: function (res) {
            data = JSON.parse(res);
            if (!data) {
                Alert('fa fa-frown-o', 'مشكلی در ایجاد ' + DocName, DocName + ' ثبت نشد', 'red', '', null);
                return false;
            } else {
                Alert('fa fa-smile-o', 'موفق', DocName + ' با موفقیت ثبت شد', 'green', '', null);
                ClearAttachForm();
                LoadGrid();
                return true;
            }
        },
        error(err) {
            Alert('fa fa-frown-o', 'خطا در ' + DocName, 'عملیات ' + DocName + ' انجام نشد ', 'red', '', null);
        }
    });
}

//// نمایش فایل
//function ViewDocFile() {
//    var grid = $("#DocumentGrid").data("kendoGrid");
//    var dataItem = grid.dataItem(grid.select());

//    $.ajax({
//        url: "/Document/Document/ViewDoc",
//        data: { DocId: dataItem.DocDocumentID},
//        datatype: "json",
//        type: "POST",
//        success: function (result) {
//            window.location = result;
//        },
//        error(err) {
//            Alert('fa fa-frown-o', 'خطا در نمایش ' + DocName, 'عملیات نمایش ' + EntityName + ' انجام نشد ', 'red', '', null);
//        }
//    });
//}
//var Sitem = 0;

//$("#DocumentGrid").click(
//    function DocumentGrid_onChange(arg) {
//    var grid = $("#DocumentGrid").data("kendoGrid");
//    Sitem = grid.dataItem(grid.select()).DocDocumentID;
//});

//function SetID() {
//    var grid = $("#DocumentGrid").data("kendoGrid");
//    var Sitem = grid.dataItem(grid.select()).DocDocumentID;

//    $.ajax({
//        url: "/Document/Document/SetDocId",
//        data: { DocId: Sitem},
//        datatype: "json",
//        type: "POST",
//        success: function (result) {
//        }
//    });

//}

function ClearAttachForm() {

    //$("#file").val("");
    $(".k-upload-files").remove();
    $(".k-upload-status").remove();
    $(".k-upload.k-header").addClass("k-upload-empty");
    $(".k-upload-button").removeClass("k-state-focused");

    $("#DocDesc").val("");
}

//ساختار اطلاعات جهت ارسال پیوست
function prepareDocumentParam() {
    var request = {
        DocDescription: $("#DocDesc").val(),
        TableName: TableName,
        RowId: RowId
    }
    return request;
}

//بررسی دسترسی حذف در لیست پیوست
//function KGDocList_DeleteAccess(e) {
//    //if ('@currentUser.HasPermissionUI("DOC:D")' == 'True')
//        //if (!DocViewMode) // وضعیت فرم غیر نمایشی
//        //{
//        //    return true;
//        //}
//    return !DocViewMode;
//}


//بررسی نمایش ستون عملیات در لیست پیوست
function OperationColumnAccess() {
    var grid = $("#DocumentGrid").data("kendoGrid");
    if (DocViewMode) {
        grid.hideColumn(grid.columns[3]);
    }
    else {
        grid.showColumn(grid.columns[3]);
    }
}