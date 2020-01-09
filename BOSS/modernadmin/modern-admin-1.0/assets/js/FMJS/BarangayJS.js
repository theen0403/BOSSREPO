//Onlaoad JS
$(document).on('click', '#BrgyNameNaviID', function (e) {
    GetBrgyNameTab();
    TabActive('tabBrgyNameID');
});
$(document).on('click', '#BrgyCollectorNaviID', function (e) {
    GetBrgyCollectorTab();
    TabActive('tabBrgyCollectorID');
});
$(document).on('click', '#BrgyBankAccntNaviID', function (e) {
    GetBrgyBankAccntTab();
    TabActive('tabBrgyBankAccntID');
});
function ClearTabContentFMBrgy() {
    $("#tabBrgyNameID").html("");
    $("#tabBrgyCollectorID").html("");
    $("#tabBrgyBankAccntID").html("");
}
//----------------------------------------------------------------------------------------------------------
// BARANGAY NAME
//----------------------------------------------------------------------------------------------------------
function GetBrgyNameTab() {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyNameTab",
        success: function (result) {
            ClearTabContentFMBrgy();
            $('#tabBrgyNameID').html(result);
            GetBrgyNameForm(1, 0);
        }
    })
}
function GetBrgyNameForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyNameForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#brgyNameTempID').html(result);

            GetBrgyNameDTable();
            changeBtnTxt('btnAdBrgyName');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetBrgyNameDTable() {
    $.ajax({
        url: '/FileMaintenanceBarangay/GetBrgyNameDTable',
        success: function (result) {
            $('#brgyNameTableID').html(result);
        }
    })
}
$(document).on('click', '#btnEditBrgyID', function (e) {
    var BrgyID = $(this).attr('BrgyAttr');
    GetBrgyNameForm(2, BrgyID);
});
$(document).on('click', "#btnDeleteBrgyID", function () {
    var PrimaryID = $(this).attr('BrgyAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceBarangay/DeleteBrgyName', '/FileMaintenanceBarangay/ConfirmDeleteBrgyName', ' GetBrgyNameTab();');
});
//----------------------------------------------------------------------------------------------------------
// BARANGAY COLLECTOR
//----------------------------------------------------------------------------------------------------------
function GetBrgyCollectorTab() {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyCollectorTab",
        success: function (result) {
            ClearTabContentFMBrgy();
            $('#tabBrgyCollectorID').html(result);
            GetBrgyCollertorForm(1,0);
        }
    })
}
function GetBrgyCollertorForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyCollertorForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#brgyCollectorTempID').html(result);

            GetBrgyCollectorDTable();
            changeBtnTxt('btnAddCollector');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetBrgyCollectorDTable() {
    $.ajax({
        url: '/FileMaintenanceBarangay/GetBrgyCollectorDTable',
        success: function (result) {
            $('#brgyCollectorTableID').html(result);
        }
    })
}
$(document).on('click', '#btnEditBrgyCollectorID', function (e) {
    var BrgyCollectorID = $(this).attr('BrgyCollectorAttr');
    GetBrgyCollertorForm(2, BrgyCollectorID);
});
$(document).on('click', "#btnDeleteBrgyCollectorID", function () {
    var PrimaryID = $(this).attr('BrgyCollectorAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceBarangay/DeleteBrgyCollector', '/FileMaintenanceBarangay/ConfirmDeleteBrgyCollector', ' GetBrgyCollectorTab()');
});
//----------------------------------------------------------------------------------------------------------
// BARANGAY BANK ACCOUNT
//----------------------------------------------------------------------------------------------------------
function GetBrgyBankAccntTab() {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyBankAccntTab",
        success: function (result) {
            ClearTabContentFMBrgy();
            $('#tabBrgyBankAccntID').html(result);
            GetBrgyBankAccntForm(1,0);
        }
    })
}
function GetBrgyBankAccntForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyBankAccntForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#brgyBankAccntTempID').html(result);

            GetBrgyBankAccntDTable();
            changeBtnTxt('btnAddBrgyBnkAccnt');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function onChangeAccntNo() {
    var selectedBankName = $("#BankID").val();
    if (selectedBankName != null) {
        $.ajax({
            url: "/FileMaintenanceBarangay/onChangeAccntNo",
            data: { BankID: selectedBankName },
            success: function (result) {
                $("#BankAccntID > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $("#BankAccntID").append(items);

                }
                else {
                    $("#BankAccntID > option").remove();
                }
            }
        });
    }
    else {
        $("#BankAccntID > option").remove();
    }
}
function GetBrgyBankAccntDTable() {
    $.ajax({
        url: '/FileMaintenanceBarangay/GetBrgyBankAccntDTable',
        success: function (result) {
            $('#brgyBankAccntTableID').html(result);
        }
    })
}
$(document).on('click', '#btnEditBrgyBankAccntID', function (e) {
    var BrgyBankAccntID = $(this).attr('BrgyBankAccntAttr');
    GetBrgyBankAccntForm(2, BrgyBankAccntID);
});
$(document).on('click', "#btnDeleteBrgyBankAccntID", function () {
    var PrimaryID = $(this).attr('BrgyBankAccntAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceBarangay/DeleteBrgyBankAccnt', '/FileMaintenanceBarangay/ConfirmDeleteBrgyBankAccnt', ' GetBrgyBankAccntTab()');
});









