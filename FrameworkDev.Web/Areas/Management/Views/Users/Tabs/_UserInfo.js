$(document).ready(function () {
    ChangeFormStatus(false);
});

function btnAdd_onClick(e) {
    OperationType = 'add';

    if ($('#rgnbtnmains').css('display') === 'inline-block') {
        $('#rgnbtnmains').css('display', 'none');
        $('#rgnbtnactions').css('display', 'inline-block');
    }

    $('#UserName').data('kendoMaskedTextBox').enable(true);

    ChangeFormStatusAdd(true);
    ClearForm();
}

function btnEdit_onClick(e) {
    OperationType = 'edit';

    if ($('#UserId').data('kendoMaskedTextBox').value() === null || $('#UserId').data('kendoMaskedTextBox').value() === '' || $('#UserId').data('kendoMaskedTextBox').value() === 0) {
        Alert('fa fas fa-exclamation-triangle', 'اخطار', 'لطفا جهت انجام عملیات ویرایش یک كاربر را انتخاب نمایید.', 'red', '', null);
        return;
    }
    if ($('#rgnbtnmains').css('display') === 'inline-block') {
        $('#rgnbtnmains').css('display', 'none');
        $('#rgnbtnactions').css('display', 'inline-block');
    }
    ChangeFormStatus(true);
}

function btnDelete_onClick(e) {
    OperationType = 'delete';

    if ($('#UserId').data('kendoMaskedTextBox').value() === null || $('#UserId').data('kendoMaskedTextBox').value() === '' || $('#UserId').data('kendoMaskedTextBox').value() === 0) { Alert('fa fas fa-exclamation-triangle', 'اخطار', 'لطفا جهت انجام عملیات حذف، یک كاربر را انتخاب نمایید.', 'red', '', null); return; }
    if ($('#rgnbtnmains').css('display') === 'inline-block') {
        $('#rgnbtnmains').css('display', 'none');
        $('#rgnbtnactions').css('display', 'inline-block');
    }
}

function btnSave_onClick(e) {
    if (!$('#frmUserInfo').kendoValidator().data('kendoValidator').validate())
        return false;
    if (!$('#frmUserInfo').valid())
        return false;

    var data = {
        UserId: $('#UserId').data('kendoMaskedTextBox').value(),
        UserName: $('#UserName').data('kendoMaskedTextBox').value(),
        FirstName: $('#FirstName').data('kendoMaskedTextBox').value(),
        LastName: $('#LastName').data('kendoMaskedTextBox').value(),
        Password: $('#Password').data('kendoMaskedTextBox').value(),
        PasswordConfirm: $('#PasswordConfirm').data('kendoMaskedTextBox').value(),
        Email: $('#Email').data('kendoMaskedTextBox').value(),
        Roles: $('#Roles').data('kendoMultiSelect').value(),
        IsActive: $('#IsActive').prop('checked')
    };

    var urlaction = '';
    var actionType = '';
    if (OperationType === 'add') { urlaction = '/Management/Users/AddUser'; actionType = 'POST'; }
    else if (OperationType === 'edit') { urlaction = '/Management/Users/EditUser'; actionType = 'POST'; }
    else if (OperationType === 'delete') { urlaction = '/Management/Users/DeleteUser'; actionType = 'POST'; }
    $.ajax({
        url: urlaction,
        type: actionType,
        data: JSON.stringify({
            "VMUser": data,
            "userId": $('#UserId').data('kendoMaskedTextBox').value(),
            "userName": $('#UserName').data('kendoMaskedTextBox').value()
        }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (xhr) {
            Alert('fa fas fa-exclamation-triangle', 'خطا', 'خطا در انجام درخواست', 'red', '', null);
        },
        success: function (result) {
            Alert('fa fa-smile-o', 'موفق', 'درخواست شما با موفقیت انجام شد', 'green', '', null);
            if (OperationType === 'add') {
                $('#UsersGrid').data('kendoGrid').dataSource.read();
                ChangeFormStatus(false);
            }
            else if (OperationType === 'edit') {
                $('#UsersGrid').data('kendoGrid').dataSource.read();
                ChangeFormStatus(false);
            }
            else if (OperationType === 'delete') {
                $('#UsersGrid').data('kendoGrid').dataSource.read();
                ClearForm();
                ChangeFormStatus(false);
            }

            if ($('#rgnbtnmains').css('display') === 'none') {
                $('#rgnbtnmains').css('display', 'inline-block');
                $('#rgnbtnactions').css('display', 'none');
            }
        }
    });
}

function btnCancel_onClick(e) {
    OperationType = '';
    if ($('#rgnbtnmains').css('display') === 'none') {
        $('#rgnbtnmains').css('display', 'inline-block');
        $('#rgnbtnactions').css('display', 'none');
    }
    ChangeFormStatus(false);
}

function ClearForm() {
    try {
        $('#UserId').data('kendoMaskedTextBox').value('0');
        $('#UserName').data('kendoMaskedTextBox').value('');
        $('#FirstName').data('kendoMaskedTextBox').value('');
        $('#LastName').data('kendoMaskedTextBox').value('');
        $('#Password').data('kendoMaskedTextBox').value('');
        $('#PasswordConfirm').data('kendoMaskedTextBox').value('');
        $('#Email').data('kendoMaskedTextBox').value('');
        //  $('#CitizenID').data('kendoDropDownList').value('');
        $('#Roles').data('kendoMultiSelect').value('');
        $('#IsActive').attr('checked', false);
    } catch (e) { console.error(e); }
}

function ChangeFormStatus(EnbVal) {
    try {
        $('#UserId').data('kendoMaskedTextBox').enable(false);
        $('#UserName').data('kendoMaskedTextBox').enable(false);
        $('#FirstName').data('kendoMaskedTextBox').enable(EnbVal);
        $('#LastName').data('kendoMaskedTextBox').enable(EnbVal);
        $('#Password').data('kendoMaskedTextBox').enable(EnbVal);
        $('#PasswordConfirm').data('kendoMaskedTextBox').enable(EnbVal);
        $('#Email').data('kendoMaskedTextBox').enable(EnbVal);
        //   $('#CitizenID').data('kendoDropDownList').enable(EnbVal);
        $('#Roles').data('kendoMultiSelect').enable(EnbVal);
        if (EnbVal) $('#IsActive').removeAttr('disabled');
        else $('#IsActive').attr('disabled', true);
    } catch (e) { console.error(e); }
}

function ChangeFormStatusAdd(EnbVal) {
    try {
        $('#UserId').data('kendoMaskedTextBox').enable(false);
        $('#UserName').data('kendoMaskedTextBox').enable(true);
        $('#FirstName').data('kendoMaskedTextBox').enable(EnbVal);
        $('#LastName').data('kendoMaskedTextBox').enable(EnbVal);
        $('#Password').data('kendoMaskedTextBox').enable(EnbVal);
        $('#PasswordConfirm').data('kendoMaskedTextBox').enable(EnbVal);
        $('#Email').data('kendoMaskedTextBox').enable(EnbVal);
        //   $('#CitizenID').data('kendoDropDownList').enable(EnbVal);
        $('#Roles').data('kendoMultiSelect').enable(EnbVal);
        if (EnbVal) $('#IsActive').removeAttr('disabled');
        else $('#IsActive').attr('disabled', true);
    } catch (e) { console.error(e); }
}
