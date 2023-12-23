function KGWorkDesk_ApproveAccess(dataItem) {
    //if (('@User.IsInRole("admin")' == 'True') && (dataItem.RequestResultID == 1 || dataItem.RequestResultID == 5)) {
    //    return true;
    //}
    //if (('@User.IsInRole("personnel")' == 'True') && (dataItem.RequestResultID == 6)) {
    //    return true;
    //}
    //return false;
    return true;
}
function KGWorkDesk_RejectAccess(dataItem) {
    //if ('@User.IsInRole("admin")' == 'True') {
    //    if (dataItem.RequestResultID == 1 || dataItem.RequestResultID == 5) {
    //        return true;
    //    }
    //}
    //return false;
    return true;
}
function KGWorkDesk_DeleteAccess(dataItem) {
    //if ('@User.IsInRole("admin")' == 'True') {
    //    if (dataItem.RequestResultID == 1 || dataItem.RequestResultID == 5) {
    //        return true;
    //    }
    //}
    //return false;
    return true;
}
function KGWorkDesk_CancelAccess(dataItem) {
    //if ('@User.IsInRole("personnel")' == 'True') {
    //    if (dataItem.RequestResultID == 1 || dataItem.RequestResultID == 2 || dataItem.RequestResultID == 6) {
    //        return true;
    //    }
    //}
    //return false;
    return true;
}
function KGWorkDesk_ShowReqAccess(dataItem) {
    return true;
}
function KGWorkDesk_EditReqAccess(dataItem) {
    //if ((dataItem.RequestResultID == 2 || dataItem.RequestResultID == 3 || dataItem.RequestResultID == 4 || dataItem.RequestResultID == 6) && dataItem.PSTChangetimeOver) {
    //    return true;
    //}
    //return false;
    return true;
}
function CheckEditTime(personnelTourID) {
    $.ajax({
        url: '@Url.Action("CheckEditTime", "PersonnelTour", new { area = "Public" })',
        type: "POST",
        data: { personnelTourID },
        success: function (res) {
            return res.isok
        },
        error: function () {
            return false;
        }
    });
    return false;
}
function KGWorkDesk_PrintReqAccess(dataItem) {
    if (dataItem.RequestResultID == 2 || dataItem.RequestResultID == 7)
        return true;
    return false;
}
function KGWorkDesk_ReportReqAccess(dataItem) {
    if ('@User.IsInRole("WelfareCenterResponsible")' == 'True')
        if (dataItem.RequestResultID == 2 || dataItem.RequestResultID == 7) {
            return true;
        }
    return false;
}
function KGWorkDesk_EntranceReqAccess(dataItem) {
    if ('@User.IsInRole("WelfareCenterResponsible")' == 'True')
        if (dataItem.RequestResultID == 2 || dataItem.RequestResultID == 7) {
            return true;
        }
    return false;
}
function newSafarReq_OnLoad(e) {
    if ('@User.IsInRole("personnel")' == 'True') {
        e.show()
    }
    e.hide();
}
$().ready(function () {
    /* نوار ابزار بالای گرید */
    var toolbar = $('#KGWorkDesk .k-grid-toolbar');

    /* ایجاد فیلتر تا تاریخ */
    makeMDPDatePicker($('#PSTREndDFilter'), false, true, 'DFilter');
    //$('#PSTREndDFilter').change(function () {GridFilters_change(); });
    toolbar.prepend($('#divPSTREndDFilter'));

    /* ایجاد فیلتر از تاریخ */
    makeMDPDatePicker($('#PSTRstartDFilter'), true, false, 'DFilter');
    //$('#PSTRstartDFilter').change(function () {GridFilters_change(); });
    toolbar.prepend($('#divPSTRstartDFilter'));

    /* ایجاد فیلتر نوع  */
    toolbar.prepend($('#divRequestKindFilter'));
});
function GridFilters_change() {
    var grid = $("#KGWorkDesk").data("kendoGrid");
    grid.dataSource.read();
}
function search_OnClick() {
    $("#KGWorkDesk").data("kendoGrid").dataSource.data([]);
    $("#KGWorkDesk").data("kendoGrid").dataSource.read();
}
function newSafarReq_OnClick() {
    window.open('@Url.Action("Index", "PersonnelTour", new { area = "Public" })');
}
function KGWorkDesk_RowDataBound(e) {
    if ('@User.IsInRole("WelfareCenterResponsible")' == 'True') {
        var rows = e.sender.tbody.children();

        for (var j = 0; j < rows.length; j++) {
            var row = $(rows[j]);
            var dataItem = e.sender.dataItem(row);

            if (dataItem.get("TEVEnterDate") == null) {
                row.css("background-color", "#ff000087");
            }
        }
    }
}
$("#newSafarReq").on("click", function (e) {
    e.preventDefault();
    window.open('@Url.Action("", "PersonnelTour",new {area = "Public"})');
});
function KGWorkDesk_parameter() {
    var param1 = "WorkDesk,InitialUserAndNotSend";//"WorkDeskVilla";//'@ViewBag.Type';
    var param2 = $("#PSTRstartDFilter").val();
    var param3 = $("#PSTREndDFilter").val();
    return {
        type: param1,
        fromDate: param2,
        toDate: param3
    };
}
var dataItem;
function KGWorkDesk_OnConfirmation(e) {
    var item = this.dataItem($(e.currentTarget).closest("tr"));
    dataItem = item;
    confirmationWorkDeskMessage();
}
function confirmationWorkDeskMessage() {
    if (dataItem + "" == "undefined") {
        dataItem = getSelectedRow("KGWorkDesk");
    }
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new {area = "Workflow"})',
        data: {
            'workFlowType': 'NextLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Message',
            'externalCondition': ''
        },
        datatype: "json",
        type: "POST",
        success: function (res) {

            //// در صورتی كه درخواست قبلا ثبت نهایی شده باشد اجازه تغییر بر روی درخواست را نمی دهد
            //if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
            //    Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            //}
            //// در صورتی كه ایرادی جهت ارسال به مرحله بعد یا ثبت نهایی وجود نداشته باشد
            //else {
            //    ConfirmAlertCustom('تایید درخواست', res.data[0].Message, confirmationWorkDeskSend, "بله", "خیر");
            //}

            ConfirmAlertCustom('تایید درخواست', res.data[0].Message, confirmationWorkDeskSend, "بله", "خیر");
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
function confirmationWorkDeskSend() {
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new { area = "Workflow" })',
        data: {
            'workFlowType': 'NextLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Send',
            'externalCondition': ''
        },
        datatype: "json",
        type: "POST",
        success: function (res) {

            if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            else {
                Alert('fa fa-smile-o', 'موفق', res.data[0].Message, 'green', '', null);
                $("#KGWorkDesk").data("kendoGrid").dataSource.data([]);
                $("#KGWorkDesk").data("kendoGrid").dataSource.read();
            }
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
function KGWorkDesk_OnCancel(e) {
    var item = this.dataItem($(e.currentTarget).closest("tr"));
    dataItem = item;
    ConfirmAlertWithTextArea("لغو درخواست سفر", "", "", CancelRequest, item.EntityID, "لغو", "CancelRequestInputText", "دلایل لغو سفر");
}
function CancelRequest(EntityID) {
    var cancelDes = $('#CancelRequestInputText').val();
    $.ajax({
        url: '@Url.Action("CancelTourRequest", "PersonnelTour", new {area = "Workflow"})',
        data: { 'id': EntityID, 'des': cancelDes },
        datatype: "json",
        type: "POST",
        success: function (res) {
            if (res.isOk == 1) {
                Alert('fa fa-smile-o', 'موفق', res.msg, 'green', '', null);
                $("#KGWorkDesk").data("kendoGrid").dataSource.data([]);
                $("#KGWorkDesk").data("kendoGrid").dataSource.read();
            } else {
                Alert('fa fa-frown-o', ' خطا ', res.msg, 'red', '', null);
            }
        }
    });
}
function KGWorkDesk_OnDelete(e) {
    var item = this.dataItem($(e.currentTarget).closest("tr"));
    dataItem = item;
    deleteWorkDeskMessage();
}
function deleteWorkDeskMessage() {
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new { area = "Workflow" })',
        data: {
            'workFlowType': 'NextLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Message',
            'externalCondition': 'NotApprove'
        },
        datatype: "json",
        type: "POST",
        success: function (res) {

            // در صورتی كه درخواست قبلا ثبت نهایی شده باشد اجازه تغییر بر روی درخواست را نمی دهد
            if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            // در صورتی كه ایرادی جهت ارسال به مرحله بعد یا ثبت نهایی وجود نداشته باشد
            else {
                ConfirmAlertCustom('عدم تایید درخواست', res.data[0].Message, deleteWorkDeskSend, "بله", "خیر");
            }
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
function deleteWorkDeskSend() {
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new { area = "Workflow" })',
        data: {
            'workFlowType': 'NextLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Send',
            'externalCondition': 'NotApprove'
        },
        datatype: "json",
        type: "POST",
        success: function (res) {

            if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            else {
                Alert('fa fa-smile-o', 'موفق', res.data[0].Message, 'green', '', null);
                $("#KGWorkDesk").data("kendoGrid").dataSource.data([]);
                $("#KGWorkDesk").data("kendoGrid").dataSource.read();
            }
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
////////////////////////////////////////////////////////////////////////////////////////////////
function KGWorkDesk_OnReject(e) {
    var item = this.dataItem($(e.currentTarget).closest("tr"));
    dataItem = item;
    rejectWorkDeskMessage();
}
function rejectWorkDeskMessage() {
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new { area = "Workflow" })',
        data: {
            'workFlowType': 'PreviousLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Message',
            'externalCondition': ''
        },
        datatype: "json",
        type: "POST",
        success: function (res) {
            // در صورتی كه درخواست قبلا ثبت نهایی شده باشد اجازه تغییر بر روی درخواست را نمی دهد
            if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            else if (res.data[0].EngMessage == 'NoExsistPreviousStep') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            // در صورتی كه ایرادی وجود نداشته باشد
            else {
                //ConfirmAlertCustom('رد درخواست جهت ویرایش', res.data[0].Message, rejectWorkDeskSend, "بله", "خیر");
                ConfirmAlertWithTextArea('رد درخواست جهت ویرایش', res.data[0].Message, "", rejectWorkDeskSend, null, "بله", "rejectWorkDeskDsc", "پاراف")
            }
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
function rejectWorkDeskSend() {
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new { area = "Workflow" })',
        data: {
            'workFlowType': 'PreviousLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Send',
            'externalCondition': '',
            'paraph': $('#rejectWorkDeskDsc').val()
        },
        datatype: "json",
        type: "POST",
        success: function (res) {

            if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            else {
                Alert('fa fa-smile-o', 'موفق', res.data[0].Message, 'green', '', null);
                $("#KGWorkDesk").data("kendoGrid").dataSource.data([]);
                $("#KGWorkDesk").data("kendoGrid").dataSource.read();
            }
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
function KGWorkDesk_OnAddEntranceReq(e) {
    var item = this.dataItem($(e.currentTarget).closest("tr"));
    dataItem = item;
    ConfirmAlertWithDatePicker("ثبت ورود پرسنل", "", "", AddEntranceDate, item.EntityID, "ثبت", "AddEntranceDateInputText", "تاریخ ورود");
}
function AddEntranceDate(EntityID) {
    var entraceDate = $('#AddEntranceDateInputText').val();
    $.ajax({
        url: '@Url.Action("TravelEvent_AddEntrance", "TravelEvent", new {area = "Public"})',
        data: { 'id': EntityID, 'entraceDate': entraceDate },
        datatype: "json",
        type: "POST",
        success: function (res) {
            if (res.isOk == 1) {
                Alert('fa fa-smile-o', 'موفق', res.msg, 'green', '', null);
                $("#KGWorkDesk").data("kendoGrid").dataSource.data([]);
                $("#KGWorkDesk").data("kendoGrid").dataSource.read();
            } else {
                Alert('fa fa-frown-o', ' خطا ', res.msg, 'red', '', null);
            }
        }
    });
}
function RequestBody_onClose() {
    $('#KGWorkDesk').css('height', '98%');
    $('#KGWorkDesk .k-grid-content').css("height", "85%");
    $('#RequestPanel').hide();
    ShowAllKendoGridRow('KGWorkDesk');
}

function KGWorkDesk_OnShowParaphList(e) {
    var grid = $("#KGWorkDesk").data("kendoGrid");
    var dataItem = grid.dataItem(e.target.closest("tr"));
    grid.select(e.target.closest("tr"));
    var requestId = dataItem.RequestID;
    var url = '@Url.Action("_Index", "WorkDeskParaph", new {area = "Workflow"})/' + requestId;
    $('#RequestBody').load(url, function () {
        $('#KGWorkDesk').css('height', '15%');
        $('#KGWorkDesk .k-grid-content').css("height", "unset");
        HideOtherKendoGridRow('KGWorkDesk', dataItem.uid, 'RequestPanel');
        ManageAccessToControl();
    });
}
function KGWorkDesk_OnEditReq(e) {
    var grid = $("#KGWorkDesk").data("kendoGrid");
    var dataItem = grid.dataItem(e.target.closest("tr"));
    grid.select(e.target.closest("tr"));
    var entitiId = dataItem.EntityID;
    $('#RequestBody').load('@Url.Action("EARequestInfo", "EncourageAcademic", new {area = "Welfare"})/' + entitiId, function () {
        $('#KGWorkDesk').css('height', '15%');
        $('#KGWorkDesk .k-grid-content').css("height", "unset");
        HideOtherKendoGridRow('KGWorkDesk', dataItem.uid, 'RequestPanel');
        $("div#KTWelFareCenter ul li:nth-child(5)").hide();
        showSteps();
        $("#welFareCenterRulse").empty();
        showRulse().appendTo($("#welFareCenterRulse"));
        TourParticipantControled = false;
    });
}
function KGWorkDesk_OnPrintReq(e) {
    var grid = $("#KGWorkDesk").data("kendoGrid");
    var dataItem = grid.dataItem(e.target.closest("tr"));
    grid.select(e.target.closest("tr"));
    if (dataItem.RequestResultID == 2) {
        var param = dataItem.EntityID;
        window.open('@Url.Action("Print", "IntroducingLetter", new {area = "Report"})/' + param);
    }
}
function printReqMessage(dataItem) {
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new { area = "Public" })',
        data: {
            'workFlowType': 'NextLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Message',
            'externalCondition': ''
        },
        datatype: "json",
        type: "POST",
        success: function (res) {

            // در صورتی كه درخواست قبلا ثبت نهایی شده باشد اجازه تغییر بر روی درخواست را نمی دهد
            if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            // در صورتی كه ایرادی جهت ارسال به مرحله بعد یا ثبت نهایی وجود نداشته باشد
            else {
                ConfirmAlertCustom('تایید درخواست', res.data[0].Message, printReqMessageCallBack, "بله", "خیر", dataItem);
            }
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
function printReqMessageCallBack(dataItem) {
    $.ajax({
        url: '@Url.Action("SendToWorkFlow", "WorkDesk", new { area="Public"})',
        data: {
            'workFlowType': 'NextLevel',
            'requestID': dataItem.RequestID,
            'currentStepID': dataItem.WorkFlowStepID,
            'sendOrMessage': 'Send',
            'externalCondition': ''
        },
        datatype: "json",
        type: "POST",
        success: function (res) {

            if (res.data[0].ExceptionMessage == 'ExceptionMessage') {
                Alert('fa fa-frown-o', ' خطا ', res.data[0].Message, 'red', '', null);
            }
            else {
                Alert('fa fa-smile-o', 'موفق', res.data[0].Message, 'green', '', null);
                $("#KGWorkDesk").data("kendoGrid").dataSource.data([]);
                $("#KGWorkDesk").data("kendoGrid").dataSource.read();

                var param = dataItem.EntityID;
                window.open('@Url.Action("Print", "IntroducingLetter", new { area= "Report" })/' + param);
            }
        },
        error: function (ex) {
            console.log(ex.statusText);
        }
    });
}
function KGWorkDesk_OnAddReportReq(e) {
    var grid = $("#KGWorkDesk").data("kendoGrid");
    var dataItem = grid.dataItem(e.target.closest("tr"));
    grid.select(e.target.closest("tr"));
    var entitiId = dataItem.EntityID;
    $('#RequestBody').load('@Url.Action("ViewRequest", "PersonnelTour", new {area = "Workflow"})/' + entitiId, function () {
        $('#KGWorkDesk').css('height', '15%');
        $('#KGWorkDesk .k-grid-content').css("height", "unset");
        HideOtherKendoGridRow('KGWorkDesk', dataItem.uid, 'RequestPanel');
        //ManageAccessToControl();
        $('#Pesonal_TourID_fk').val(entitiId);
        showSteps();
        $("#welFareCenterRulse").empty();
        showRulse().appendTo($("#welFareCenterRulse"));
    });
}
function ManageAccessToControl() {
    $('.editFunctionality').each(function () {
        $(this).hide();
    });
}
function showRulse() {
    var bodyContetnt = $('<div></div>');
    if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCR2IsActive) {
        var row = $('<div class="row"></div>');
        if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCPIntro) {
            var lbl = $('<label class="col-sm-4"><strong>مشخصات همراهان الزاماً باییستی ارائه گردند</strong></label>');
            lbl.appendTo(row);
        }
        var lbl1 = $('<label class="col-sm-4">تعداد مجاز همراهان عادی <strong class="text-danger"><ins>' + getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCNPNum + '</ins></strong> </label>');
        lbl1.appendTo(row);
        var lbl2 = $('<label class="col-sm-4">تعداد مجاز همراهان مازاد <strong class="text-danger"><ins>' + getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCEPNum + '</ins></strong> </label>');
        lbl2.appendTo(row);
        row.appendTo(bodyContetnt);
    }
    if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCR3IsActive) {
        var row = $('<div class="row"></div>');
        if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCCIntro) {
            var lbl = $('<label class="col-sm-4"><strong>مشخصات خودروها الزاماً باییستی ارائه گردند</strong></label>');
            lbl.appendTo(row);
        }
        var lbl1 = $('<label class="col-sm-4">تعداد مجاز خودرو<strong class="text-danger"><ins>' + getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCCNum + '</ins></strong> </label>');
        lbl1.appendTo(row);
        row.appendTo(bodyContetnt);
    }
    if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCHaveVilla) {
        var row = $('<div class="row"></div>');
        var lbl = $('<label class="col-sm-4"><strong>ویلا الزاماً باییستی انتخاب گردد</strong></label>');
        lbl.appendTo(row);
        row.appendTo(bodyContetnt);
    }
    return bodyContetnt;
}
function showSteps() {
    var ts = $("#KTWelFareCenter").data("kendoTabStrip");
    if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCHaveVilla) {
        ts.enable(ts.tabGroup.children().eq(0), true);
    }
    if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCPIntro && getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCR2IsActive) {
        ts.enable(ts.tabGroup.children().eq(1), true);
    }
    if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCCIntro && getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCR3IsActive) {
        ts.enable(ts.tabGroup.children().eq(2), true);
    }
}
var KGTourParticipant_rowNumber = 0;
function KGTourParticipant_resetRowNumber(e) {
    KGTourParticipant_rowNumber = 0;
}
function KGTourParticipant_renderNumber(data) {
    return ++KGTourParticipant_rowNumber;
}
function KGTourParticipant_parameter() {
    var param1 = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.Pesonal_TourID;
    return {
        personel_TourID: param1
    };
}
function KGTourParticipant_RequestEnd(e) {
    KGTourParticipant_Create_OnClick();
}
var TourParticipantControled = false;
function KGTourParticipant_Control_OnClick() {
    var grid = $("#KGTourParticipant").data("kendoGrid");
    grid.dataSource.sort({ field: "PTPByear", dir: "asc" });
    var ordinary = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCNPNum);
    var extra = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCEPNum);

    var currYear =DateTime.Now.ToLocalTime().ToShortDateString().Substring(0, 4);
    for (var i = 0; i < grid.dataSource._view.length; i++) {
        var firstItem = grid.dataSource._view[i];
        if (i < ordinary) {
            firstItem["PTPKind"] = "عادی";
        } else if (i >= ordinary && i < ordinary + extra) {
            if ((currYear - parseInt(grid.dataSource._view[i]["PTPByear"])) > 3) {
                firstItem["PTPKind"] = "مازاد";
            }
        }
    }
    TourParticipantControled = true;
    Personnel_Tour_Cost_calculation();
    grid.dataSource.sort({ field: "PTPByear", dir: "asc" });
}
function KGTourParticipant_Create_OnClick() {
    var adultCount = 0;
    var grid = $("#KGTourParticipant").data("kendoGrid");
    for (var i = 0; i < grid.dataSource._view.length; i++) {
        if (grid.dataSource._view[i].PTPByear == "") {
            $("#KGTourParticipant_Create").preventDefault();
        }
        var currYear =DateTime.Now.ToLocalTime().ToShortDateString().Substring(0, 4);
        var thisYear = parseInt(grid.dataSource._view[i]["PTPByear"]);
        if (isNaN(thisYear)) {
            thisYear = 0;
        }
        if (currYear - thisYear > 3) {
            adultCount++;
        }
    }
    //var count = $("#KGTourParticipant").data("kendoGrid").dataSource.data().length;
    var totalAllow = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCNPNum) + parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCEPNum);
    if (adultCount >= totalAllow - 1)
        $("#KGTourParticipant_Create").hide();
}
function KGTourParticipant_OnRemove() {
    Personnel_Tour_Cost_calculation();
}
function AllowedRelationshipID_fk_onChange(e) {
    if (getSelectedRow("KGTourParticipant") == null) {
        return;
    }
    getSelectedRow("KGTourParticipant").AllowedRelationshipID_fk = parseInt(e.sender.value());
}
function Personnel_Tour_Cost_calculation() {
    var OrdinaryPrice = 0.0,
        ExtraPrice = 0.0,
        PersonnelPrice = 0.0;
    if (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TOUPickstatus) {
        PersonnelPrice = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCPTtravelcostFP;
        OrdinaryPrice = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCNPTtravelcostFP;
        ExtraPrice = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCEPTtravelcostFP;
    } else {
        PersonnelPrice = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCPTtravelcostFnP;
        OrdinaryPrice = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCNPTtravelcostFnP;
        ExtraPrice = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCEPTtravelcostFnP;
    }
    PersonnelPrice = (PersonnelPrice == null) ? 0 : PersonnelPrice;
    OrdinaryPrice = (OrdinaryPrice == null) ? 0 : OrdinaryPrice;
    ExtraPrice = (ExtraPrice == null) ? 0 : ExtraPrice;

    var grid = $("#KGTourParticipant").data("kendoGrid");
    var totalOrdinaryCost = 0.0, OrdinaryCount = 0, totalExtraCost = 0.0, ExtraCount = 0;
    for (var i = 0; i < grid.dataSource.data().length; i++) {
        var Item = grid.dataSource._view[i];
        if (Item["PTPKind"] == "عادی") {
            OrdinaryCount++;
            var tempPrice = TourParticipant_Cost_calculation(OrdinaryPrice, Item["PTPByear"]);
            grid.dataSource._view[i]["PTPCost"] = tempPrice;
            totalOrdinaryCost = totalOrdinaryCost + tempPrice;
        } else {
            ExtraCount++;
            var tempPrice = TourParticipant_Cost_calculation(ExtraPrice, Item["PTPByear"]);
            grid.dataSource._view[i]["PTPCost"] = tempPrice;
            totalExtraCost = totalExtraCost + tempPrice;
        }

    }
    var ordinary = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCNPNum);
    var extra = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCEPNum);
    if (OrdinaryCount + ExtraCount < ordinary + extra) {
        $("#KGTourParticipant_Create").show();
    }
    $("#KNPersonnelCount").data("kendoNumericTextBox").value(1); $("#KNPersonnelPrice").data("kendoNumericTextBox").value(PersonnelPrice); $("#KNPersonnelPriceTotal").data("kendoNumericTextBox").value(PersonnelPrice);
    $("#KNOrdinaryCount").data("kendoNumericTextBox").value(OrdinaryCount); $("#KNOrdinaryPrice").data("kendoNumericTextBox").value(OrdinaryPrice); $("#KNOrdinaryPriceTotal").data("kendoNumericTextBox").value(totalOrdinaryCost);
    $("#KNExtraCount").data("kendoNumericTextBox").value(ExtraCount); $("#KNExtraPrice").data("kendoNumericTextBox").value(ExtraPrice); $("#KNExtraPriceTotal").data("kendoNumericTextBox").value(totalExtraCost);
    $("#KNPriceTotal").data("kendoNumericTextBox").value(totalOrdinaryCost + totalExtraCost + PersonnelPrice);
    var ts = $("#KTWelFareCenter").data("kendoTabStrip");
    ts.enable(ts.tabGroup.children().eq(3), true);
}
function TourParticipant_Cost_calculation(price, year) {
    var currYear =DateTime.Now.ToLocalTime().ToShortDateString().Substring(0, 4);
    var thisYear = parseInt(year);
    var diff = currYear - thisYear;
    var prc = 1.0;
    if (diff <= 3) {
        prc = 0;
    } else if (diff > 3 && diff <= 7) {
        prc = 0.5;
    }
    return price * prc;
}
function KGVilla_onClick(e) {
    var grid = $("#KGVilla").data("kendoGrid");
    grid.clearSelection();
    if ($(e.target).hasClass("k-checkbox-label")) {
        return;
    }
    var row = $(e.target).closest("tr");
    var checkbox = $(row).find(".k-checkbox");

    checkbox.click();
}
function KGVilla_DataBound(e) {
    var data = this.dataSource.data();
    for (var i = 0; i < data.length; i++) {
        var dataItem = data[i];
        var RowVillaId = (getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.VillaID_fk == null) ? 0 : getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.VillaID_fk;
        if (dataItem.VillaID == RowVillaId) {
            var row = $("#KGVilla").find("[data-uid='" + dataItem.uid + "']");
            $("#KGVilla").data("kendoGrid").select(row);
        }
    }
    var grid = e.sender;
    var rows = grid.tbody.find("[role='row']");

    rows.unbind("click");
    rows.on("click", KGVilla_onClick);
}
function KGVilla_parameter() {
    var param1 = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TOUWelFareCenterID_fk;
    var param2 = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TourID_fk;
    var param3 = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.Pesonal_TourID;
    return {
        WelFareCenterID: param1,
        TourID: param2,
        personnelTourId: param3,
    };
}
var rowNumber = 0;
function resetRowNumber(e) {
    rowNumber = 0;
}
function renderNumber(data) {
    return ++rowNumber;
}
function KGCar_parameter() {
    var param1 = getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.Pesonal_TourID;
    return {
        personel_TourID: param1
    };
}
function KGCar_Create_OnClick() {
    var count = $("#KGCar").data("kendoGrid").dataSource.data().length;
    var totalAllow = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCCNum);
    if (count == totalAllow) {
        $("#KGCar_Create").hide();
    }
}
function KGCar_OnRemove() {
    $("#KGCar_Create").show();
}
function KTWelFareCenter_OnSelect(e) {
    if ($(e.item).index() == 0) {
        $("#KBNext").show(); $("#KBPrevious").hide(); $("#KBSubmit").hide(); $("#KBSubmitReport").hide();
    }
    else if ($(e.item).index() == 1) {
        $("#KBNext").show(); $("#KBPrevious").show(); $("#KBSubmit").hide(); $("#KBSubmitReport").hide();
        KGTourParticipant_Create_OnClick();
    }
    else if ($(e.item).index() == 2) {
        if (!TourParticipant_required_Validation()) {
            e.preventDefault();
        }
        KGCar_Create_OnClick();
        $("#KBNext").show(); $("#KBPrevious").show(); $("#KBSubmit").hide(); $("#KBSubmitReport").hide();

    }
    else if ($(e.item).index() == 3) {
        if (!Car_required_Validation()) {
            e.preventDefault();
        } else {
            Personnel_Tour_Cost_calculation();
            if (getSelectedRow("KGWorkDesk").RequestResultID < 7) {
                $("#KBSubmit").show();
            }
        }
        $("#KBNext").hide(); $("#KBPrevious").show(); $("#KBSubmitReport").hide();
    }
    else if ($(e.item).index() == 4) {
        $("#KBNext").hide(); $("#KBPrevious").show(); $("#KBSubmit").hide(); $("#KBSubmitReport").show();
    }
}
function KBPrevious_OnClick() {
    var ts = $("#KTWelFareCenter").data("kendoTabStrip");
    var currIndex = ts.select().index();
    if (currIndex > 0) {
        nextIndex = currIndex - 1;
        var nextTab = nextIndex + 1;
        while ($("div#KTWelFareCenter ul li:nth-child(" + nextTab + ")").attr("aria-disabled") == "true") {
            nextIndex = nextIndex - 1;
            nextTab = nextIndex + 1;
        }
        if (currIndex == 1) {
            if (!TourParticipant_required_Validation()) {
                return;
            }
        }
        if (currIndex == 2) {
            if (!Car_required_Validation()) {
                return;
            }
        }
        $("div#KTWelFareCenter ul li:nth-child(" + nextTab + ")").click();
    }
}
function KBNext_OnClick() {
    var ts = $("#KTWelFareCenter").data("kendoTabStrip");
    var currIndex = ts.select().index();
    if (currIndex < 3) {
        nextIndex = currIndex + 1;
        var nextTab = nextIndex + 1;
        while ($("div#KTWelFareCenter ul li:nth-child(" + nextTab + ")").attr("aria-disabled") == "true") {
            nextIndex = nextIndex + 1;
            nextTab = nextIndex + 1;
        }
        if (nextTab > 4) {
            KGTourParticipant_Control_OnClick();
            nextTab = 4;
        }
        if (currIndex == 1) {
            if (!TourParticipant_required_Validation()) {
                return;
            }
            if (!TourParticipantControled) {
                KGTourParticipant_Control_OnClick();
            }
        }
        if (currIndex == 2) {
            KGCar_Create_OnClick();
            if (!Car_required_Validation()) {
                return;
            }
        }
        $("div#KTWelFareCenter ul li:nth-child(" + nextTab + ")").click();
    }

}
function KBSubmit_OnClick() {
    //prepate tourparticipent data
    var tourParticipantData = [];
    var gridItems = $("#KGTourParticipant").data("kendoGrid").dataItems();
    for (var i = 0; i < gridItems.length; i++) {
        tourParticipantData.push(gridItems[i]);
    }
    //prepate car data
    var carData = [];
    var gridItems = $("#KGCar").data("kendoGrid").dataItems();
    for (var i = 0; i < gridItems.length; i++) {
        carData.push(gridItems[i]);
    }
    var selectedVillaId = (getSelectedRow("KGVilla") != null) ? getSelectedRow("KGVilla").VillaID : -1;
    $.ajax({
        url: '@Url.Action("_Update", "PersonnelTour", new {area = "Workflow"})',
        type: 'POST',
        data: {
            'personnelTourID': getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.Pesonal_TourID,
            'villaID': selectedVillaId,
            'totalCost': $("#KNPriceTotal").data("kendoNumericTextBox").value(),
            'tourParticipant_str': JSON.stringify(tourParticipantData),
            'cars_str': JSON.stringify(carData)
        },
        datatype: "json",
        success: function (res) {
            if (res.isok = 1) {
                Alert('fa fa-smile-o', 'موفق', res.msg, 'green', '', null);
            }
        }
    });
}
function TourParticipant_required_Validation() {
    if ($("div#KTWelFareCenter ul li:nth-child(2)").attr("aria-disabled") == "true") {
        return true;
    }

    //count check
    var adultCount = 0;
    var grid = $("#KGTourParticipant").data("kendoGrid");
    for (var i = 0; i < grid.dataSource._view.length; i++) {
        var currYear =DateTime.Now.ToLocalTime().ToShortDateString().Substring(0, 4);
        var thisYear = parseInt(grid.dataSource._view[i]["PTPByear"]);
        if (isNaN(thisYear)) {
            thisYear = 0;
        }
        if (currYear - thisYear > 3) {
            adultCount++;
        }
    }
    var totalAllow = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCNPNum) + parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCEPNum);
    if (adultCount > totalAllow) {
        Alert('fa fa-frown-o', 'ناموفق', " تعداد همراهان وارد شده از تعداد مجاز بیشتر است", 'red', '', null);
        return false;
    }

    //data check
    var grid = $("#KGTourParticipant").data("kendoGrid");
    var gridItems = grid.dataSource._view;
    for (var i = 0; i < gridItems.length; i++) {
        if (gridItems[i].PTPName == "" || gridItems[i].PTPLName == "" || gridItems[i].PTPByear == "" || gridItems[i].AllowedRelationshipID_fk == null) {
            Alert('fa fa-frown-o', 'ناموفق', " نام، نام خانوادگی ، سال تولد و نسبت كلیه همراهان باید وارد شود", 'red', '', null);
            return false;
        }
    }
    if (grid.dataSource.view().length > 0) {
        return true;
    }
    Alert('fa fa-frown-o', ' خطا ', "مشخصات همراهان الزاماً باییستی ارائه گردند", 'red', '', null);
    return false;
}
function Car_required_Validation() {
    if ($("div#KTWelFareCenter ul li:nth-child(3)").attr("aria-disabled") == "true") {
        return true;
    }
    //count
    var count = $("#KGCar").data("kendoGrid").dataSource.data().length;
    var totalAllow = parseInt(getSelectedRow("KGWorkDesk").TBL_Personnel_Tour.TBL_Tour.TBL_WelFareCenter.WFCCNum);
    if (count > totalAllow) {
        Alert('fa fa-frown-o', 'ناموفق', " تعداد خودرو وارد شده از تعداد مجاز بیشتر است", 'red', '', null);
        return false;
    }
    var grid = $("#KGCar").data("kendoGrid");
    var gridItems = grid.dataSource._view;
    for (var i = 0; i < gridItems.length; i++) {
        if (gridItems[i].CARplk == "" || gridItems[i].CARplk.length < 6) {
            Alert('fa fa-frown-o', 'ناموفق', " پلاك تمامی خودروها باید وارد شود", 'red', '', null);
            return false;
        }
    }
    if (grid.dataSource.view().length > 0) {
        if (grid.dataSource.view().length > totalAllow) {
            Alert('fa fa-frown-o', 'ناموفق', " تعداد خودرو وارد شده از تعداد مجاز بیشتر است", 'red', '', null);
            return false;
        }
        return true;
    }
    Alert('fa fa-frown-o', ' خطا ', "مشخصات خودرو الزاماً باییستی ارائه گردند", 'red', '', null);
    return false;
}
function KGRequestParaph_parameter() {
    var param1 = getSelectedRow("KGWorkDesk").RequestID;
    return {
        ReqID: param1
    };
}
function KGRequestParaph_ParaphEdit(dataItem) {
    if ((('@User.IsInRole("RefahExpert")' == 'True') || ('@User.IsInRole("admin")' == 'True')) && (!dataItem.IsSeen)) {
        return true;
    }
    return false;
}
function KBSubmitReport_OnClick() {
    var formContainer = $("#__TravelEventIndexForm");
    $.ajax({
        url: '@Url.Action("Create", "TravelEvent", new {area = "Public"})',
        type: 'post',
        data: formContainer.serialize(),
        success: function (data) {
            if (data.isok == 1)
                confirmationWorkDeskMessage();
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
function telerikGridAjaxErrorhandler(args, myGridId) {
    var message = "";
    if (args.errors) {
        for (var error in args.errors) {
            $.each(args.errors[error].errors, function () {
                message += this + "\n";
            });
        }
        $("#notification").data("kendoNotification").show({
            title: "خطا",
            message: message
        }, "error");
    }
}
function ShowAllKendoGridRow(gridName) {
    $('#' + gridName + ' .k-grid-content tr').each(function () {
        $(this).show();
    });
    $("#" + gridName + " .k-grid-toolbar").show();
    $("#" + gridName + " .k-pager-wrap").show();
    $("section.content").addClass("content-body")
}
var EncourageAcademicID = 0;
//to show attach list as readonly
function showDocumentDialog() {
    var EntityName = "درخواست";

    if (EncourageAcademicID <= 0) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'لطفا جهت انجام عملیات پیوست یک ' + EntityName + ' را انتخاب نمایید.', 'red', '', null);
        return;
    }

    openDialog("WLF.TBL_EncourageAcademic", EncourageAcademicID, true);
}