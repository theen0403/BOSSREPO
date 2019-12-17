$(document).ready(function () {
    GetTaxForm(1, 0);
})
function GetTaxForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceTax/GetTaxForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#taxTempID').html(result);

            GetTaxDTableForm();
            changeBtnTxt('btnAddTax');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetTaxDTableForm() {
    $.ajax({
        url: '/FileMaintenanceTax/GetTaxDTableForm',
        success: function (result) {
            $('#taxTableID').html(result);
        }
    })
}
$(document).on('click', '#btnEditTaxID', function (e) {
    var TaxID = $(this).attr('TaxAttr');
    GetTaxForm(2, TaxID);
});
$(document).on('click', "#btnDeleteTaxID", function () {
    var PrimaryID = $(this).attr('TaxAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceTax/DeleteTax', '/FileMaintenanceTax/ConfirmDeleteTax', ' GetTaxForm(1, 0)');
});