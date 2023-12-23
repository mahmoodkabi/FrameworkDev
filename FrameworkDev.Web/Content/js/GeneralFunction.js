function makeComponentDisabled(form, flag) {
    $("." + form + " select").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });
    $("." + form + " input").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            if ($(this).attr("data-role") == "datepickershamsi") {
                $(this).data('kendoDatePickerShamsi').enable(!flag);
            }
            $(this).prop('disabled', flag);
        }
    });
    $("." + form + " textarea").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });

    $("." + form + " k-list-scroller").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });

    $("." + form + " k-datepicker").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });

    $("." + form + " k-checkbox").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });

    $("." + form + " k-dropdowntree").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });

    $("." + form + " .k-dropdown").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });

    $("." + form + " k-textbox").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).prop('disabled', flag);
        }
    });

    $("." + form + " k-combobox").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            if (flag == true) { $(this).css("pointer-events", "none"); }
            else { $(this).css("pointer-events", "auto"); }


            //$(this).prop('disabled', flag);
        }
    });

}
function makeInputClear(form) {
    $("." + form + " input").each(function () {
        if ($.inArray("NotEditable", $(this).prop("classList")) == -1) {
            $(this).val("");
        }
    });
    $("." + form + " textarea").each(function () {
        $(this).val("");
    });
    $("." + form + " select").each(function () {
        $(this).val("");
    });
    $("." + form + " checkbox").each(function () {
        $(this).prop('checked', false);
    });
    $("." + form + " k-dropdowntree").each(function () {
        $(this).val("");
    });
    $("." + form + " k-textbox").each(function () {
        $(this).val("");
    });
    $("." + form + " k-dropdown").each(function () {
        $(this).val("");
    });
    $("." + form + " k-editor").each(function () {
        $(this).val("");
    });
}
function Alert(picon, ptitle, ptext, ptype, pautoclose, pclosefunc) {
    $.confirm({
        theme: 'material',
        closeIcon: true,
        animation: 'none',
        draggable: true,
        title: ptitle,
        closeAnimation: 'none',
        backgroundDismissAnimation: 'none',
        content: '' +
            '<div class="alert form-group">' +
            '<hr>' +
            '<span style="font-size: large;">' + ptext + '</span>' +
            '</div>',
        type: ptype,
        buttons: {
            cancelAction: {
                text: 'بستن',
                action: function () {
                }
            }
        },
        onClose: function () {

            // before the modal is hidden.
            //location.reload();
            if (pclosefunc != null) {
                pclosefunc();
            }
        }
    });
}
function ConfirmAlert(pramTitle, pramContent, pramOkFunctionName, pramOkText) {

    //to resolve default value problem
    if (pramOkText === undefined || pramOkText === '') pramOkText = 'حذف';

    $.confirm({
        title: pramTitle,
        closeAnimation: 'none',
        backgroundDismissAnimation: 'none',
        //closeIcon: true,
        animation: 'none',
        draggable: true,
        content: '' +
            '<form action="" class="formName">' +
            '<div class="form-group">' +
            '<lable>' + pramContent + '</lable>' +
            '</div>' +
            '</form>',
        buttons: {
            sayMyName: {
                text: pramOkText,
                btnClass: 'btn-success',
                icon: 'fa fa-check-circle',
                action: function () {
                    pramOkFunctionName();
                }
            },
            later: {
                text: 'انصراف',
                btnClass: 'btn-red',
                icon: 'fa fa-times',
                action: function () {

                    // do nothing.
                    //location.reload();
                }
            }
        }
    });
}

function ConfirmAlertWithTextBox(pramTitle, pramContent, pramOkFunctionName, pramOkText) {

    //to resolve default value problem
    if (pramOkText === undefined || pramOkText === '') pramOkText = 'حذف';

    $.confirm({
        title: pramTitle,
        closeAnimation: 'none',
        backgroundDismissAnimation: 'none',
        //closeIcon: true,
        animation: 'none',
        draggable: true,
        content: '' +
        '<form action="" class="formName">' +
        '<div class="form-group">' +
            '<lable>' + pramContent + '</lable>' +
            '<br/>'+

            '<div class="form-group">' +
                '<label for="idConfirmAlertWithTextBox" class="col-sm-2 control-label">عنوان</label>' +
                '<div class="col-sm-10">' +
                    '<input type="textbox" class="form-control" id="idConfirmAlertWithTextBox" style = "box-sizing: border-box; width: 100%; font-size: medium;">' +
                '</div>' +
            '</div>' +

            '<div class="form-group">' +
                '<label for="idConfirmAlertWithTextBox_Description" class="col-sm-2 control-label">شرح</label>' +
                '<div class="col-sm-10">' +
            '<textarea id="idConfirmAlertWithTextBox_Description"  rows="4" cols="35" style = "box-sizing: border-box; width: 100%; font-size: medium;"></textarea>' +
                '</div>' +
            '</div>' +

        '</div>' +
        '</form>',
        buttons: {
            sayMyName: {
                text: pramOkText,
                btnClass: 'btn-success',
                icon: 'fa fa-check-circle',
                action: function () {
                    pramOkFunctionName();
                }
            },
            later: {
                text: 'انصراف',
                btnClass: 'btn-red',
                icon: 'fa fa-times',
                action: function () {

                    // do nothing.
                    //location.reload();
                }
            }
        }
    });
}

function getSelectedRow(girdid) {

    var grid = $("#" + girdid).data("kendoGrid");
    if (grid != undefined)

        return grid.dataItem(grid.select());
    return null;
}
function prepareToolBarAction(form, btn_new, btn_edit, btn_delete, btn_accept, btn_cancel) {
    if (btn_new) {
        $("#btnNewRow" + form).show();
    } else {
        $("#btnNewRow" + form).hide();
    }
    if (btn_edit) {
        $("#btnEditRow" + form).show();
    } else {
        $("#btnEditRow" + form).hide();
    }
    if (btn_delete) {
        $("#btnDelete" + form).show();
    } else {
        $("#btnDelete" + form).hide();
    }
    if (btn_accept) {
        $("#btnAcceptChanges" + form).show();
    } else {
        $("#btnAcceptChanges" + form).hide();
    }
    if (btn_cancel) {
        $("#btnCancelChanges" + form).show();
    } else {
        $("#btnCancelChanges" + form).hide();
    }
}

function checkNationalCode(nationalCode) {
    var L = nationalCode.length;

    if (L < 8 || parseInt(nationalCode, 10) == 0) return false;
    nationalCode = ('0000' + nationalCode).substr(L + 4 - 10);
    if (parseInt(nationalCode.substr(3, 6), 10) == 0) return false;
    var c = parseInt(nationalCode.substr(9, 1), 10);
    var s = 0;
    for (var i = 0; i < 9; i++)
        s += parseInt(nationalCode.substr(i, 1), 10) * (10 - i);
    s = s % 11;
    return (s < 2 && c == s) || (s >= 2 && c == (11 - s));

    return true; ///unreachable code after return statement
}

function checkNationalMeliCode(nationalCode) {
    if (nationalCode.substr(0, 1) == '0') {
        return false;
    }
}

function calcDuration2ShamsiDate(startDate, endDate) {

    startDate = startDate.replace("/", "").replace("/", "");
    endDate = endDate.replace("/", "").replace("/", "");

    if (startDate.length != 8)
        return 0;
    if (endDate.length != 8)
        return 0;
    var Difference = 0;
    var year1 = startDate.substr(0, 4);
    var month1 = startDate.substr(4, 2);
    var day1 = startDate.substr(6, 2);
    var year2 = endDate.substr(0, 4);
    var month2 = endDate.substr(4, 2);
    var day2 = endDate.substr(6, 2);
    var dayYear1 = 0;
    var dayYear2 = 0;
    var dayMon1 = 0;
    var dayMon2 = 0;
    var sumDay1 = 0;
    var sumDay2 = 0;

    if (parseInt(year1) % 4 != 0)
        dayYear1 = 366;
    else
        dayYear1 = 365;

    if (parseInt(year2) % 4 != 0)
        dayYear2 = 366;
    else
        dayYear2 = 365;

    if (parseInt(month1) >= 1 & parseInt(month1) <= 6)
        dayMon1 = (parseInt(month1) * 31);
    if (parseInt(month1) >= 7 & parseInt(month1) < 12)
        dayMon1 = 186 + ((parseInt(month1) - 6) * 30);
    if (parseInt(month1) == 12 & dayYear1 == 366)
        dayMon1 = 336 + 30;
    if (parseInt(month1) == 12 & dayYear1 == 365)
        dayMon1 = 336 + 29;

    if (parseInt(month2) >= 1 & parseInt(month2) <= 6)
        dayMon2 = (parseInt(month2) * 31);
    if (parseInt(month2) >= 7 & parseInt(month2) < 12)
        dayMon2 = 186 + ((parseInt(month2) - 6) * 30);
    if (parseInt(month2) == 12 & dayYear2 == 366)
        dayMon2 = 336 + 30;
    if (parseInt(month2) == 12 & dayYear2 == 365)
        dayMon2 = 336 + 29;

    sumDay1 = (parseInt(year1) * dayYear1) + dayMon1 + parseInt(day1);
    sumDay2 = (parseInt(year2) * dayYear2) + dayMon2 + parseInt(day2);

    Difference = sumDay2 - sumDay1;

    return Difference;
}
function makeMDPDatePicker(element, isFromDate, isToDate, groupId, enableTimePicker) {
    element.MdPersianDateTimePicker({
        Placement: 'bottom',
        Trigger: 'click',
        EnableTimePicker: enableTimePicker,
        TargetSelector: '#' + $(element).prop('id'),
        GroupId: groupId,
        FromDate: isFromDate,
        ToDate: isToDate,
        DisableBeforeToday: false,
        Disabled: $(element).hasClass('readonly'),
        Format: 'yyyy/MM/dd',
        IsGregorian: false,
        EnglishNumber: true,
        Inline: false
    });

    //element.inputmask("1399/99/99");
}
function endDateIsGrater(startDate, endDate) {
    // alert('testii :)');
    var dif = calcDuration2ShamsiDate(startDate, endDate);
    if (dif >= 0)
        return true;
    else return false;


}

function HideOtherKendoGridRow(gridName, rowuid, detailPanelid) {
    $('#' + gridName + ' .k-grid-content tr').each(function () {
        if ($(this).attr("data-uid") !== rowuid) {
            $(this).hide();
        } else {
            $(this).show();
        }
        $('#' + detailPanelid).show();
    });
    $("#" + gridName + " .k-grid-toolbar").hide();
    $("#" + gridName + " .k-pager-wrap").hide();
    $("section.content").removeClass("content-body")
}

function ShowAllKendoGridRow(gridName) {
    $('#' + gridName + ' .k-grid-content tr').each(function () {
        $(this).show();
    });
    $("#" + gridName + " .k-grid-toolbar").show();
    $("#" + gridName + " .k-pager-wrap").show();
    $("section.content").addClass("content-body")
}

function ConfirmAlertCustom(pramTitle, pramContent, pramOkFunctionName, pramOkText, pramNotOkText, pramOkFuncPram) {

    //to resolve default value problem
    if (pramOkText === undefined || pramOkText === '') pramOkText = 'حذف';

    //to resolve default value problem
    if (pramNotOkText === undefined || pramNotOkText === '') pramNotOkText = 'انصراف';

    $.confirm({
        title: pramTitle,
        icon: 'fa fa-commenting-o',
        content: '' +
            '<form action="" class="formName">' +
            '<div class="form-group">' +
            '<lable>' + pramContent + '</lable>' +
            '</div>' +
            '</form>',
        buttons: {
            sayMyName: {
                text: pramOkText,
                btnClass: 'btn-orange',
                icon: 'fa fa-check-circle',
                action: function () {
                    pramOkFunctionName(pramOkFuncPram);
                }
            },
            later: {
                text: pramNotOkText,
                btnClass: 'btn-red',
                icon: 'fa fa-times',
                action: function () {

                    // do nothing.
                    //location.reload();
                }
            }
        }
    });
}

function ConfirmAlertWithTextArea(pramTitle, pramAlert, pramContent, pramOkFunctionName, pramOkFuncPram, pramOkText, pramInputName, pramPlaceholderText) {

    //to resolve default value problem
    if (pramOkText === undefined || pramOkText === '') pramOkText = 'حذف';

    $.confirm({
        title: pramTitle,
        icon: 'fa fa-commenting-o',
        content: '' +
            '<form action="" class="formName">' +
            '<div class="form-group">' +
            '<lable>' + pramAlert + '</lable>' +
            '<textarea class="form-control text-box multi-line" id="' + pramInputName + '" name="' + pramInputName + '" rows="3" placeholder="' + pramPlaceholderText + '">' + pramContent + '</textarea>' +
            '</div>' +
            '</form>',
        buttons: {
            sayMyName: {
                text: pramOkText,
                btnClass: 'btn-orange',
                icon: 'fa fa-check-circle',
                action: function () {
                    pramOkFunctionName(pramOkFuncPram);
                }
            },
            later: {
                text: 'انصراف',
                btnClass: 'btn-red',
                icon: 'fa fa-times',
                action: function () {

                    // do nothing.
                    //location.reload();
                }
            }
        }
    });
}

function tabstrip_main_EnableForLoading(status) {
    try {
        for (i = 1; i < 10; i++) {
            var tab = $("#tabstrip").data("kendoTabStrip").tabGroup.children("li").eq(i);
            if (status === 1)
                $("#tabstrip").data("kendoTabStrip").enable(tab, true);//فعال
            else
                $("#tabstrip").data("kendoTabStrip").enable(tab, false);//غیر فعال
        }
    } catch (e) { console.error(e); }
}


function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

//function getCookie(name) {
//    const value = `; ${document.cookie}`;
//    const parts = value.split(`; ${name}=`);
//    if (parts.length === 2)
//        return parts.pop().split(';').shift();
//}


