function RolePermissions_additionalData() {
    return {
        roleId: RoleId
    };
}

function RolePermissions_Check(e) {
    var dataItem = this.dataItem($(e.node));
    var data = {
        RoleId: RoleId,
        checked: dataItem.checked,
        PermissionKey: dataItem.id
    };

    $.ajax({
        url: '/api/RolePermissionsapi/' + RoleId,
        data: data,
        datatype: 'json',
        type: 'PUT',
        success: function (res) {
            ShowNotification('ذخیره با موفقیت انجام شد.', 'موفقیت آمیز', 'success', 1000);
        },
        error(err) {
            ShowNotification(err.responseJSON.MessageDetail, err.responseJSON.Message, 'error', 5000);
        }
    });
}
