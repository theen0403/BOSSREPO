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
function GetSectorForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceSector/GetSectorForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
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
//Click Edit in action button
$(document).on('click', '#btnEditSectorID', function (e) {
    var SectorID = $(this).attr('SectorAttr');
    GetSectorForm(2, SectorID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteSectorID", function () {
    var PrimaryID = $(this).attr('SectorAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceSector/DeleteSector', '/FileMaintenanceSector/ConfirmDelete', ' GetSectorForm(1, 0)');
});
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
function GetSubSectorForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceSector/GetSubSectorForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
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
        //btnDeleteSectorID
        url: '/FileMaintenanceSector/GetSubSectorDTable',
        success: function (result) {
            $("#subsectorTableID").html(result);
        }
    })
}
//Click Edit in action button
$(document).on('click', '#btnEditSubSectorID', function (e) {
    var SubSectorID = $(this).attr('SubSectorAttr');
    GetSubSectorForm(2, SubSectorID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteSubSectorID", function () {
    var PrimaryID = $(this).attr('SubSectorAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceSector/DeleteSubSector', '/FileMaintenanceSector/ConfirmDeleteSubFund', ' GetSubSectorForm(1, 0)');
});
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