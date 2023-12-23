var _userId;

$(document).ready(function () {
    tabstrip_main_EnableForLoading(0);
});

function UsersGrid_onChange(arg) {
    var data = this.dataItem(this.select());
    _userId = data.UserId;
    fillgrid();
    fillForm(data);
    tabstrip_main_EnableForLoading(1);
}

function fillForm(data) {
    $('#UserId').val(data.UserId);
    $('#FirstName').val(data.FirstName);
    $('#LastName').val(data.LastName);
    $('#Email').val(data.Email);
    $('#UserName').val(data.UserName);
    $('#Password').val(data.Password);
    $('#PasswordConfirm').val(data.PasswordConfirm);
    $('#IsActive').attr('checked', data.IsActive);
    $('#Roles').data('kendoMultiSelect').value($.map(data.Roles, function (role) { return role; }));
   // $('#CitizenID').data('kendoDropDownList').value(data.CitizenID);
}

function fillgrid() {
    //$('#UserZonesGrid').data('kendoGrid').dataSource.read();
  //  $('#UserUnitsGrid').data('kendoGrid').dataSource.read();
  //  $('#UserEmpTypesGrid').data('kendoGrid').dataSource.read();
  //  $('#UserAuditLogsGrid').data('kendoGrid').dataSource.read();
}

function additionalDataUser(e) {
    return {
        UserId: _userId
    };
}

function tabstrip_main_EnableForLoading(status) {
    var tabstrip = $('#tabstrip').data('kendoTabStrip');
    if (tabstrip) {
        tabstrip_main_EnableForLoading_child(tabstrip, 'li', status);
        tabstrip_main_EnableForLoading_child(tabstrip, 'checkbox', status);
    }
}

function tabstrip_main_EnableForLoading_child(tabstrip, child, status) {
    if (tabstrip)
        for (i = 1; i < 10; i++) {
            tabstrip.enable(tabstrip.tabGroup.children(child).eq(i), status === 1);
        }
}



function additionalDataUserInsUpd(e) {
    return {
        UID: UserId
    };
} 