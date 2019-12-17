//Onlaoad JS
$(document).on('click', '#BankNavID', function (e) {
    GetBankTab();
    TabActive('tabBankID');
});
$(document).on('click', '#AccountTypeNaviID', function (e) {
    GetAccntTypeTab();
    TabActive('tabAccountTypeID');
});
$(document).on('click', '#BankAccountNaviID', function (e) {
    GetBankAccntTab();
    TabActive('tabBankAccntID');
});
function ClearTabContentFMBanks() {
    $("#tabBankID").html("");
    $("#tabAccountTypeID").html("");
    $("#tabBankAccntID").html("");
}
//---------------------------------------------------------------------------------------------------------------------
//Bank Tab
//---------------------------------------------------------------------------------------------------------------------
function GetBankTab() {
    $.ajax({
        url: "/FileMaintenanceBank/GetBankTab",
        success: function (result) {
            ClearTabContentFMBanks();
            $('#tabBankID').html(result);
            GetBankForm(1, 0);
        }
    })
}
function GetBankForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceBank/GetBankForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#bankTempID').html(result);

            GetBankDTable();
            changeBtnTxt('btnAddBank');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetBankDTable() {
    $.ajax({
        url: '/FileMaintenanceBank/GetBankDTable',
        success: function (result) {
            $('#bankTableID').html(result);
        }
    })
}
$(document).on('click', '#btnEditBankID', function (e) {
    var BankID = $(this).attr('BankAttr');
    GetBankForm(2, BankID);
});
$(document).on('click', "#btnDeleteBankID", function () {
    var PrimaryID = $(this).attr('BankAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceBank/DeleteBank', '/FileMaintenanceBank/ConfirmDeleteBank', ' GetBankForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Account Type Tab
//---------------------------------------------------------------------------------------------------------------------
function GetAccntTypeTab() {
    $.ajax({
        url: "/FileMaintenanceBank/GetAccntTypeTab",
        success: function (result) {
            ClearTabContentFMBanks();
            $('#tabAccountTypeID').html(result);
            GetAccntTypeForm(1, 0);
        }
    })
}
function GetAccntTypeForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceBank/GetAccntTypeForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#accntTypeTempID').html(result);

            GetAccntTypeDTable();
            changeBtnTxt('btnAddAccnType');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetAccntTypeDTable() {
    $.ajax({
        url: '/FileMaintenanceBank/GetAccntTypeDTable',
        success: function (result) {
            $('#accntTypeTableID').html(result);
        }
    })
}
$(document).on('click', '#btnEditAccntTypeID', function (e) {
    var AccntTypeID = $(this).attr('AccntTypeAttr');
    GetAccntTypeForm(2, AccntTypeID);
});
$(document).on('click', "#btnDeleteAccntTypeID", function () {
    var PrimaryID = $(this).attr('AccntTypeAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceBank/DeleteAccntType', '/FileMaintenanceBank/ConfirmDeleteAccntType', ' GetAccntTypeForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Bank Accounts Tab
//---------------------------------------------------------------------------------------------------------------------
function GetBankAccntTab() {
    $.ajax({
        url: "/FileMaintenanceBank/GetBankAccntTab",
        success: function (result) {
            ClearTabContentFMBanks();
            $('#tabBankAccntID').html(result);
            GetBankAccntForm(1, 0);
        }
    })
}
function GetBankAccntForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceBank/GetBankAccntForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#bankaccntTempID').html(result);

            GetBankAccntDTable();
            changeBtnTxt('btnAddBankAccnt');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetchangeGACode(GAID = $('.GenAccntdropdown option:selected').val()) { // sub major account group drop down changes sub major account code
    if (GAID != null) {
        $.ajax({
            url: "/FileMaintenanceBank/onchangeGACode",
            data: { GAID: GAID },
            success: function (result) {
                if (result.passCon != null) {
                    $(".GACodetext").val(result.passCon);

                }

            }
        })
    }
    else {
        $(".GACodetext").val("");
    }
}
function GetBankAccntDTable() {
    $.ajax({
        url: '/FileMaintenanceBank/GetBankAccntDTable',
        success: function (result) {
            $('#bankaccntTableID').html(result);
        }
    })
}
$(document).on('click', '#btnEditBankAccntID', function (e) {
    var BankAccntID = $(this).attr('BankAccntAttr');
    GetBankAccntForm(2, BankAccntID);
});
$(document).on('click', "#btnDeleteBankAccntID", function () {
    var PrimaryID = $(this).attr('BankAccntAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceBank/DeleteBankAccnt', '/FileMaintenanceBank/ConfirmDeleteBankAccnt', ' GetBankAccntForm(1, 0)');
});