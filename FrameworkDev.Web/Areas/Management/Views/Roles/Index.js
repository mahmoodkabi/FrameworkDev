var RoleId = 0;

function RolesGrid_onChange(arg) {
    var data = this.dataItem(this.select());
    RoleId = data.RoleId;
    fillPermissionsGrid(data);
    fillUsersGrid();
}

function fillUsersGrid() {
    $("#UsersInRoleGrid").data("kendoGrid").dataSource.read();
}

function fillPermissionsGrid(dataItem) {
    var data = $("#RolePermissions").data('kendoTreeView');

    if (data && dataItem) {
        data.dataSource.data([]);

        RoleId = dataItem.RoleId;

        data.dataSource.read();

        var inputRoleId = $('input#RoleId');
        var inputRoleName = $('input#RoleName');
        var inputRoleNameFa = $('input#RoleNameFa');

        inputRoleId.attr('readonly', 'readonly');
        inputRoleName.attr('readonly', 'readonly');
        inputRoleNameFa.attr('readonly', 'readonly');

        inputRoleId.val(dataItem.RoleId);
        inputRoleName.val(dataItem.RoleName);
        inputRoleNameFa.val(dataItem.RoleNameFa);
    }
}

//منطقه انتخاب شده برای فیلتر گرید
function additionalDataEInsUpd(e) {
    return {
        roleid: RoleId
    }
}

function UserList_onChange(arg) {
    var data = this.dataItem(this.select());
    UserId = data.UserId;

    tabstrip_main_EnableForLoading(1);

    //$('#selectedItem').text(data.CTZFirstName + ' ' + data.CTZLastName);
    //$("#selectedItem").css("visibility", "visible");
    fillgrid();

    //$.ajax({
    //    type: "post",
    //    url: '/api/Postapi/PostAllInfo',
    //    contentType: "application/json; charset=utf-8",
    //    data: JSON.stringify({ 'postID': postID }),
    //    success: function (res) {
    //        // postt(res);
    //        // $("#PostID").val(res.Post.PostID);

    //    },
    //    error: function (e) {
    //    }
    //});

    tabstrip_main_EnableForLoading(1);
}

function dataBoundRolesGrid() {
    this.table.find(".k-grid-edit").hide();
}