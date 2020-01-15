$(function () {
    GetIndexProgram();
});

function GetIndexProgram() {
    $.ajax({
        url: "/PreparationPerOffice/GetIndexProgram",
        success: function (result) {
            $('#tempindexprogramid').html(result);
            GetProgramlistDTable();
        }
    })
}
function GetProgramlistDTable() {
    $.ajax({
        url: "/PreparationPerOffice/GetProgramlistDTable",
        success: function (result) {
            $('#tableProgramID').html(result);
        }
    })
}
function GetProgramForm(ActionID, ProgramID) {
    $.ajax({
        url: "/PreparationPerOffice/GetProgramForm",
        data: { ActionID: ActionID, ProgramID: ProgramID },
        success: function (result) {
            $('#ViewAddProgram').html(result);
            $('#ViewAddProgramModal').modal('toggle');
        }
    })
}






//PRINT PREVIEW
function OpenViewUpdatePrintPreviewModal() {
    //var ID = $("#SupplyAccountCodeList").val();
    var IDTemp = "1";
    $.ajax({
        url: '/PreparationPerOffice/GetViewPrintPreview',
        data: { ID: IDTemp },
        success: function (result) {
            $('#ViewPrintPreview').html(result);
        }
    })
}

$('#dpValue').text($('#dp option:selected').text());
$(document).ready(function () {
    $('#dp').on('change', function () {
        var dpValue = $(this).find('option:selected').text();
        $('#dpValue').text(dpValue);
    });
});
function CloseModal() {
    $('#ViewPrintPreviewModal').modal('toggle');
}




