//$(document).ready(function () {
//    GetSupplierForm(1, 0);
//})

function GetSupplierForm(ActionID, SupplierID) {
    $.ajax({
        url: "/FileMaintenanceSupplier/GetSupplierForm",
        data: { ActionID: ActionID, SupplierID: SupplierID },
        success: function (result) {
            $("#supplierTempID").html(result);
            
            GetSupplierDTable();
            changeBtnTxt('btnAddSupplier');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetSupplierDTable() {
    $.ajax({
        url: "/FileMaintenanceSupplier/GetSupplierDTable",
        success: function (result) {
            $("#tablesupplierID").html(result);

        }
    })
}
$(document).on('click', '#btnEditSupplierID', function (e) {
    var SupplierID = $(this).attr('SupplierAttr');
    GetSupplierForm(2, SupplierID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteSupplierID", function () {
    var PrimaryID = $(this).attr('SupplierAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceSupplier/DeleteSupplier', '/FileMaintenanceSupplier/ConfirmDeleteSupplier', ' GetSupplierForm(1, 0)');
});
  