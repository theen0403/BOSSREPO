//OnLoad
$(document).ready(function () {
    GetOfficeTypeForm(1, 0);
});

function GetOfficeTypeForm(ActionID, PrimaryID){
    $.ajax({
        url: "/FileMaintenanceOfficeType/GetOfficeTypeForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#officetypeTempID').html(result);
            GetOfficeTypeDTable();

            changeBtnTxt('btnAddOfficeType');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetOfficeTypeDTable() {
    $.ajax({
        url: "/FileMaintenanceOfficeType/GetOfficeTypeDTable",
        success: function (result) {
            $('#officetypeTableID').html(result);
        }
    })
}
//Click Edit in action button
$(document).on('click', '#btnEditOfficeTypeID', function (e) {
    var OfficeTypeID = $(this).attr('OfficeTypeAttr');
    GetOfficeTypeForm(2, OfficeTypeID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteOfficeTypeID", function () {
    var PrimaryID = $(this).attr('OfficeTypeAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceOfficeType/DeleteOfficeType', '/FileMaintenanceOfficeType/ConfirmDelete', ' GetOfficeTypeForm(1, 0)');
});