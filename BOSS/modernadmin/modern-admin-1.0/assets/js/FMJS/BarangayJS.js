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

//----------------------------------------------------------------------------------------------------------
// BARANGAY NAME
//----------------------------------------------------------------------------------------------------------
function GetBrgyNameTab() {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyNameTab",
        success: function (result) {
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
    DeleteRecord(PrimaryID, '/FileMaintenanceBarangay/DeleteBrgyName', '/FileMaintenanceBarangay/ConfirmDeleteBrgyName', ' GetBrgyNameDTable(1, 0)');
});
//----------------------------------------------------------------------------------------------------------
// BARANGAY COLLECTOR
//----------------------------------------------------------------------------------------------------------
function GetBrgyCollectorTab() {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyCollectorTab",
        success: function (result) {
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
    DeleteRecord(PrimaryID, '/FileMaintenanceBarangay/DeleteBrgyCollector', '/FileMaintenanceBarangay/ConfirmDeleteBrgyCollector', ' GetBrgyCollertorForm(1, 0)');
});
















function GetBrgyBankAccntTab() {
    $.ajax({
        url: "/FileMaintenanceBarangay/GetBrgyBankAccntTab",
        success: function (result) {
            $('#tabBrgyBankAccntID').html(result);
        }
    })
}
