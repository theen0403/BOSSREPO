//OnLoad
$(document).ready(function () {
    GetPositionForm(1,0);
})
function GetPositionSPDTable() {
    $.ajax({
        url: '/FileMaintenancePosition/GetPositionSPDTable',
        success: function (result) {
            $("#PositionTableID").html(result);
        }
    })
}
function GetPositionForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenancePosition/GetPositionForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#PositionSPTempID').html(result);

            changeBtnTxt('btnAddPosition');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
            GetPositionSPDTable();
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
//Click Edit in action button
$(document).on('click', '#btnEditPositionID', function (e) {
    var PositionID = $(this).attr('PositionAttr');
    GetPositionForm(2, PositionID);
});
//Click Delete in action button
$(document).on('click', "#btnDeletePositionID", function () {
    var PrimaryID = $(this).attr('PositionAttr');
    DeleteRecord(PrimaryID, '/FileMaintenancePosition/DeletePosition', '/FileMaintenancePosition/ConfirmDelete', ' GetPositionForm(1, 0)');
});