//Onlaoad JS
$(document).on('click', '#sectorNavID', function (e) {
    GetSectorTab();
    TabActive('tabSectorID');
});
$(document).on('click', '#subsectorNavID', function (e) {
    GetSubSectorTab();
    TabActive('tabSubSectorID');
});
function ClearTabContentFMSector() {
    $("#tabSectorID").html("");
    $("#tabSubSectorID").html("");
}
function TabActive(tabID) {
    $("#" + tabID).addClass('active');
}
//---------------------------------------------------------------------------------------------------------------------
//Sector Tab
//---------------------------------------------------------------------------------------------------------------------
function GetSectorTab() {
    $.ajax({
        url: "/FileMaintenanceSector/GetSectorTab",
        success: function (result) {
            ClearTabContentFMSector();
            $('#tabSectorID').html(result);
            GetSectorForm(1, 0);
        }
    })
}
function GetSectorForm(ActionID, SectorID) {
    $.ajax({
        url: "/FileMaintenanceSector/GetSectorForm",
        data: { ActionID: ActionID, SectorID: SectorID },
        success: function (result) {
            $('#sectortempID').html(result);
            GetSectorDTable();
            changeBtnTxt('btnAddSector');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetSectorDTable() {
    $.ajax({
        url: '/FileMaintenanceSector/GetSectorDTable',
        success: function (result) {
            $("#sectorTableID").html(result);
        }
    })
}
function ConfirmDeleteSector(SectorID) {
    $.ajax({
        url: "/FileMaintenanceSector/ConfirmDeleteSector",
        data: { SectorID: SectorID },
        success: function (result) {
            GetSectorForm(1, 0);
        }
    })
}
function DeleteSector(SectorID) {
    var titleMess = "Warning";
    var confirmMess = "This record is used by other table. Deleting this will also delete the records on other table. Are you sure to delete this?";
    var successDeleteMess = "Succsessfully deleted!";
    var confirmDelSector = "ConfirmDeleteSector(" + SectorID + ")";
    $.ajax({
        url: "/FileMaintenanceSector/DeleteSector",
        data: { SectorID: SectorID },
        success: function (result) {
            if (result.confirmDeleteSector == "confirm") {

                swalConfirmation(titleMess, confirmMess, successDeleteMess, confirmDelSector);
            }
            else if (result.confirmDeleteSector == "delete") {
                GetSectorForm(1, 0);
                swalSuccess('Success', 'Sector Deleted.');
            }
        }
    })
}
$(document).on('click', '#btnEditSectorID', function (e) {
    var SectorID = $(this).attr('SectorAttr');
    GetSectorForm(2, SectorID);
});
function changeBtnTxt(addBtnID) {
    if ($(".ActionID").val() == 1) {
        $("#" + addBtnID).text("Add");
    }
    else if ($(".ActionID").val() == 2) {
        $("#" + addBtnID).text("Save Changes");
        //$("#" + addBtnID).attr("disabled", true);
    }
}
var ifExists = function (data, getForm) {
    if (data.isExist == "true") {
        getForm;
        swalError("Error saving", "Record already exists.")
    }
    else if (data.isExist == "false") {
        getForm;
        swalSuccess('Success', 'Successfully Saved.');
    }
    else {
        getForm;
        swalSuccess('Success', 'Updated Successfully.');
    }
}
//---------------------------------------------------------------------------------------------------------------------
//SubSector Tab
//---------------------------------------------------------------------------------------------------------------------
function GetSubSectorTab() {
    $.ajax({
        url: "/FileMaintenanceSector/GetSubSectorTab",
        success: function (result) {
            ClearTabContentFMSector();
            $('#tabSubSectorID').html(result);
            GetSubSectorForm(1, 0);
        }
    })
}
function GetSubSectorForm(ActionID, SubSectorID) {
    $.ajax({
        url: "/FileMaintenanceSector/GetSubSectorForm",
        data: { ActionID: ActionID, SubSectorID: SubSectorID },
        success: function (result) {
            $('#subsectortempID').html(result);
            GetSubSectorDTable();
            GetSectorCodeField();
            changeBtnTxt('btnAddSubSector');

            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetSubSectorDTable() {
    $.ajax({
        url: '/FileMaintenanceSector/GetSubSectorDTable',
        success: function (result) {
            $("#subsectorTableID").html(result);
        }
    })
}
function GetSectorCodeField(SectorID = $('#SectorIDtemp option:selected').val()) {
    $.ajax({
        url: "/FileMaintenanceSector/GetSectorCodeField",
        data: { SectorID: SectorID },
        success: function (result) {
            if (result.passCon != null) {
                $("#sectorCodeID").text(result.passCon + " -");
            }
            else {
                alert(null)
            }
        }
    })
}
$(document).on('click', '#btnEditSubSectorID', function (e) {
    var SubSectorID = $(this).attr('SubSectorAttr');
    GetSubSectorForm(2, SubSectorID);
});

function DeleteSubSector(SubSectorID) {
    $.ajax({
        url: "/FileMaintenanceSector/DeleteSubSector",
        data: { SubSectorID: SubSectorID },
        success: function (result) {
            GetSubSectorForm(1, 0);
        }
    })
}