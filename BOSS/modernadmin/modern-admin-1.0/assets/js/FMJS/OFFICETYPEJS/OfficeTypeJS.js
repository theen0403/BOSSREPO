//OnLoad
$(document).ready(function () {
    GetOfficeTypeForm(1, 0);
});

function GetOfficeTypeForm(ActionID, OfficeTypeID){
    $.ajax({
        url: "/FileMaintenanceOfficeType/GetOfficeTypeForm",
        data: { ActionID: ActionID, OfficeTypeID: OfficeTypeID },
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
function changeBtnTxt(addBtnID) {
    if ($(".ActionID").val() == 1) {
        $("#" + addBtnID).text("Add");
    }
    else if ($(".ActionID").val() == 2) {
        $("#" + addBtnID).text("Save Changes");
    }
}

$(document).on('click', '#btnEditOfficeTypeID', function (e) {
    var OfficeTypeID = $(this).attr('OfficeTypeAttr');
    GetOfficeTypeForm(2, OfficeTypeID);

});