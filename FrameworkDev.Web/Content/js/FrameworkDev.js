kendo.culture("fa");

var usermenu = $('li[class="dropdown user user-menu"]');

usermenu.click(() => {
    usermenu.toggleClass("open");
});

$(window).resize(function () {
    resizeGrid();
});

$().ajaxComplete(function () {
    resizeGrid();
});

function kendoGridAjaxErrorhandler(args, myGridId) {
    if (args.errors) {
        var grid = $("#" + myGridId).data("kendoGrid");
        grid.one("dataBinding", function (e) {
            e.preventDefault();   // cancel grid rebind if error occurs

            for (var error in args.errors) {
                kendoGridMessage(grid.editable.element, error, args.errors[error].errors);
            }
        });
    }
}

function kendoGridMessage(container, name, errors) {

    //add the validation message to the form
    container.find("[data-valmsg-for=" + name + "],[data-val-msg-for=" + name + "]")
        .replaceWith(validationMessageTmpl({ field: name, message: errors[0] }))
}

function resizeGrid() {
    var gridElement = $(".k-grid");
    gridElement.each(function () {
        if ($.inArray("NotResizeable", $(this).prop("classList")) === -1) {
            var dataArea = $(this).find(".k-grid-content"),
                gridHeight = $(this).innerHeight(),
                otherElements = $(this).children().not(".k-grid-content"),
                otherElementsHeight = 0;

            otherElements.each(function () {
                otherElementsHeight += $(this).outerHeight();
            });

            dataArea.height(gridHeight - otherElementsHeight);
        }
    });
}
function shamsiDatePicker(element) {
    element.kendoDatePickerShamsi();
}

function onMyGridDataBound() {
    var grid = this;

    grid.element.on('dblclick', 'tbody tr[data-uid]', function (e) {
        grid.editRow($(e.target).closest('tr'));
    });
}

function SetPopupValue(gridid, key, value) {
    var uid = $(".k-edit-form-container").closest("[data-role=window]").data("uid");
    var model = $("#" + gridid).data("kendoGrid").dataSource.getByUid(uid);
    model.set(key, value);
}

function ShowNotification(message, title = '', dialogType = 'info', timeout = 6000) {

    var notification = $("#PopupNotification").kendoNotification({
        autoHideAfter: timeout,
        stacking: "up",
        templates: [{
            type: "error",
            template: $("#errorTemplate").html()
        }, {
            type: "success",
            template: $("#successTemplate").html()
        }, {
            type: "info",
            template: $("#infoTemplate").html()
        }]
    }).data("kendoNotification");

    var d = new Date();
    var datetime = kendo.toString(d, 'HH:MM:ss.') + kendo.toString(d.getMilliseconds(), "000");

    notification.show({ title: title, message: message, datetime: datetime }, dialogType);
}

function Str2Bool(str) {
    if (str === 'True') {
        return true;
    } else {
        return false;
    }
}

function LoadSubSystem(url, ssName) {
    $.cookie("ssName", ssName);
    window.location.href = url;
}

function GetTrue() {
    return true;
}

function GetFalse() {
    return false;
}

function onKendoDSRequestEnd(e) {
    if (e.response) {
        if (e.type === 'create') {
            ShowNotification('عملیات ثبت با موفقیت انجام شد', 'موفق', 'success', 5000);
            //Alert('fa fa-smile-o', 'موفق', 'عملیات ثبت با موفقیت انجام شد', 'green', '', null);
        }
        else if (e.type === 'update') {
            ShowNotification('عملیات بروزرسانی با موفقیت انجام شد', 'موفق', 'success', 5000);
            //Alert('fa fa-smile-o', 'موفق', 'عملیات بروزرسانی با موفقیت انجام شد', 'green', '', null);
        }
        else if (e.type === 'destroy') {
            ShowNotification('عملیات حذف با موفقیت انجام شد', 'موفق', 'success', 5000);
            //Alert('fa fa-smile-o', 'موفق', 'حذف اطلاعات با موفقیت انجام شد', 'green', '', null);
        }
    }
}

function Excel_Import_Save(e) {
    var grid = $(e).closest('[data-role=\'grid\']').getKendoGrid();
    var createUrl = grid.dataSource.transport.options.create.url;
    var importUrl = createUrl.replace('/Create','/Import');

    KendoGridFileUploadDialog(grid, 'ورود اطلاعات از فایل اکسل', importUrl, ['.xlsx'], 4194304);
}

function KendoGridFileUploadDialog(grid, title, importUrl, allowedExtensions, maxFileSize) {
    var dlg = $('<div id="dlgUpload"></div>');
    dlg.appendTo(grid.element);
    $('#dlgUpload').kendoDialog({
        width: '400px',
        title: title,
        closable: false,
        modal: true,
        content: '<a href="' + importUrl + 'Sample?tempFile=tempFile" target="_blank" class="k-button k-download-button" style="cursor:pointer; width:100%;"><span class="k-icon k-i-file-excel"></span>نمونه فایل</a><hr/><input name="files" id="files" type="file" aria-label="files" />',
        actions: [
            {
                text: 'تائید', primary: true, action: (e) => {
                    $('#dlgUpload').closest('div[role="dialog"]').block({ message: 'در حال ایجاد رکوردهای جدید روی سرور' });

                    if ($('input#files').data('kendoUpload').getFiles().length === 0) {
                        ShowNotification('لطفا یک فایل بارگزاری کنید!', 'فایل یافت نشد', 'error', 300);
                        $('#dlgUpload').closest('div[role="dialog"]').unblock();
                        return false;
                    }

                    var fileNames = $('input#files').data('kendoUpload').getFiles()[0].name;

                    if (fileNames)
                        $.ajax({
                            data: [{ name: 'fileNames', value: fileNames }],
                            url: importUrl + 'Finalize',
                            datatype: 'json',
                            type: 'POST',
                            success: function (res) {
                                $('#dlgUpload').closest('div[role="dialog"]').unblock();
                                ShowNotification('فایل ارسالی با موفقیت ثبت شد!', 'ذخیره موفقیت آمیز', 'success', 600);
                                $.ajax({
                                    data: [{ name: 'fileNames', value: fileNames }],
                                    url: importUrl + 'Remove',
                                    datatype: 'json',
                                    type: 'POST',
                                    success: function (res) {
                                        return true;
                                    },
                                    error(err) {
                                        console.error(err.responseJSON.ExceptionMessage);
                                    }
                                });
                                $('#dlgUpload').data('kendoDialog').close();
                                return true;
                            },
                            error(err) {
                                ShowNotification(err.responseJSON.ExceptionMessage, 'خطا در ذخیره سازی', 'error', 300);

                                //console.error(err.responseJSON.ExceptionMessage);
                            }
                        });

                    $('#dlgUpload').closest('div[role="dialog"]').unblock();
                    return false;
                }
            },
            {
                text: 'انصراف', action: (e) => {
                    $('#dlgUpload').closest('div[role="dialog"]').block({ message: 'در حال حذف فایل از روی سرور' });

                    var files = $('input#files').data('kendoUpload').getFiles();
                    if (files.length === 0) return true;
                    var fileNames = files[0].name;

                    if (fileNames)
                        $.ajax({
                            data: [{ name: 'fileNames', value: fileNames }],
                            url: importUrl + 'Remove',
                            datatype: 'json',
                            type: 'POST',
                            success: function (res) {
                                return true;
                            },
                            error(err) {
                                console.error(err.responseJSON.ExceptionMessage);
                            }
                        });

                    $('#dlgUpload').closest('div[role="dialog"]').unblock();
                    return true;
                }
            }
        ],
        open: function (e) {
            $("input#files").kendoUpload({
                multiple: false,
                async: {
                    saveUrl: importUrl + 'Save',
                    removeUrl: importUrl + 'Remove',
                    autoUpload: true,
                    batch: false
                },
                validation: {
                    allowedExtensions: allowedExtensions,
                    maxFileSize: maxFileSize
                },
                messages: {
                    statusUploaded: 'بارگذاری فایل به اتمام رسید.',
                    invalidMaxFileSize: 'فایل با حداکثر حجم ' + maxFileSize / 1048576 + ' مگابایت مجاز است!',
                    invalidFileExtension: 'فایل وارد شده غیرمجاز است!'
                },
                success: (e) => {
                }
            });
        },
        close: function (e) {
            $('#dlgUpload').data('kendoDialog').destroy();
        }
    });
} 

//

(function ($) {
    $('document').ready(function () {
        function number_format_currency(_value) {
            var res = _value.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            return res;
        }

        e = $('input.currency');
        for (i = 0; i < e.length; i++) {
            var res = number_format_currency(e[i].value);
            $(e).val(res);
        }


        $('input.currency').keyup(function () {
            var _val = $(this).val();
            _val = _val.replace(/\,/g, '');

            //var regex = /[0-9]|\./;
            //if (!regex.test(_val)) {
            //    alert('66666666666666');
            //}

            var res = number_format_currency(_val);
            $(this).val(res);
        })

    
        
        setTimeout("Topersian()", 200);
       

    });
})(jQuery);

//تبدیل ی و ک فارسی در همه ی ورودی های سیستم و فیلتر های جستجوی گرید 
function Topersian() {

    $('input').keyup(function () {

        if ($(this).val() == "ي") {
           // alert("ی عربی");
            $(this).val() = "ی";
           
        }
        if ($(this).val() == "ك") {
            //alert("ک عربی");
            $(this).val() = "ک";
           
        }
       
    }
    )

}


// تنظیم متن دكمه های ذخیره و انصراف در گرید تلریك
function resetButtonClick() {
    $(".k-button.k-button-icontext.k-primary.k-grid-update").html("<span class='k-icon k-i-check'></span> ذخیره"); 
    $(".k-button.k-button-icontext.k-grid-cancel").html("<span class='k-icon k-i-cancel'></span> انصراف"); 
    $(".k-command-cell").css("display", "inherit")
}

//نمایش ولیدیشن ها به صورت ریموت در گرید
(function ($, kendo) {
    $.extend(true, kendo.ui.validator, {
        rules: {
            remote: function (input) {
                if (input.val() == "" || !input.attr("data-val-remote-url")) {
                    return true;
                }

                if (input.attr("data-val-remote-recieved")) {
                    input.attr("data-val-remote-recieved", "");
                    return !(input.attr("data-val-remote"));
                }

                var url = input.attr("data-val-remote-url");
                var postData = {};
                postData[input.attr("data-val-remote-additionalfields").split(".")[1]] = input.val();

                var validator = this;
                var currentInput = input;
                input.attr("data-val-remote-requested", true);
                $.ajax({
                    url: url,
                    type: "POST",
                    data: JSON.stringify(postData),
                    dataType: "json",
                    traditional: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data == true) {
                            input.attr("data-val-remote", "");
                        }
                        else {
                            input.attr("data-val-remote", "تکراری است");
                        }
                        input.attr("data-val-remote-recieved", true);
                        validator.validateInput(currentInput);

                    },
                    error: function () {
                        input.attr("data-val-remote-recieved", true);
                        validator.validateInput(currentInput);
                    }
                });
                return true;
            }
        },
        messages: {
            remote: function (input) {
                return input.attr("data-val-remote");
            }
        }
    });
})(jQuery, kendo);