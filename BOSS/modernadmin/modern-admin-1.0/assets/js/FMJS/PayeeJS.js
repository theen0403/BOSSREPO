//OnLoad
$(document).ready(function () {
    GetPayeeForm(1, 0);
})

function GetPayeeDTable() {
    $.ajax({
        url: '/FileMaintenancePayee/GetPayeeDTable',
        success: function (result) {
            $('#payeeTableID').html(result);
        }
    })
}
function GetPayeeForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenancePayee/GetPayeeForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#payeeTempID').html(result);

            GetPayeeDTable();
            changeBtnTxt('btnAddPayee');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
//Click Edit in action button
$(document).on('click', '#btnEditPayeeID', function (e) {
    var PayeeID = $(this).attr('PayeeAttr');
    GetPayeeForm(2, PayeeID);
});
//Click Delete in action button
$(document).on('click', "#btnDeletePayeeID", function () {
    var PrimaryID = $(this).attr('PayeeAttr');
    DeleteRecord(PrimaryID, '/FileMaintenancePayee/DeletePayee', '/FileMaintenancePayee/ConfirmDelete', ' GetPayeeForm(1, 0)');
});