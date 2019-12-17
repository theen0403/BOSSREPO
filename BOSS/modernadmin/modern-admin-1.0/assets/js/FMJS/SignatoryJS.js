//OnLoad
$(document).ready(function () {
    GetSignatoryForm(1, 0);
})
function GetSignatoryForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceSignatory/GetSignatoryForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#SignatoryTempID').html(result);

            GetSignatoryDTable();
            changeBtnTxt('btnAddSignatory');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetSignatoryDTable() {
    $.ajax({
        url: '/FileMaintenanceSignatory/GetSignatoryDTable',
        success: function (result) {
            $('#SignatoryTableID').html(result);
        }
    })
}
//Onchange Function Field
function onChangeSignatory() {
    var selectedDept = $("#DeptDropDown").val();
    if (selectedDept != null) {
        $.ajax({
            url: "/FileMaintenanceSignatory/onChangeSignatory",
            data: { DeptID: selectedDept },
            success: function (result) {
                $("#FunctionID > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $("#FunctionID").append(items);

                }
                else {
                    $("#FunctionID > option").remove();
                }
            }
        });
    }
    else {
        $("#FunctionID > option").remove();
    }
}
//Click Edit in action button
$(document).on('click', '#btnEditSignatoryID', function (e) {
    var SignatoryID = $(this).attr('SignatoryAttr');
    GetSignatoryForm(2, SignatoryID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteSignatoryID", function () {
    var PrimaryID = $(this).attr('SignatoryAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceSignatory/DeleteSignatory', '/FileMaintenanceSignatory/ConfirmDelete', ' GetSignatoryForm(1, 0)');
});