
//Onlaoad JS
$(document).on('click', '#fundNaviID', function (e) {
    GetFundTab();
    TabActive('tabFundID');
});
$(document).on('click', '#sfundNaviID', function (e) {
    GetSubFundTab();
    TabActive('tabSubFundID');
});
function ClearTabContentFMFund() {
    $("#tabFundID").html("");
    $("#tabSubFundID").html("");
}
function TabActive(tabID) {
    $("#" + tabID).addClass('active');
}
//---------------------------------------------------------------------------------------------------------------------
//Fund Tab
//---------------------------------------------------------------------------------------------------------------------
function GetFundTab() {
    $.ajax({
        url: "/FileMaintenanceFund/GetFundTab",
        success: function (result) {
            ClearTabContentFMFund();
            $('#tabFundID').html(result);
            GetFundForm(1, 0);
        }
    })
}
function GetFundDTable() {
    $.ajax({
        url: '/FileMaintenanceFund/GetFundDTable',
        success: function (result) {
            $('#fundTableID').html(result);
        }
    })
}
function GetFundForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceFund/GetFundForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#fundTempID').html(result);

            changeBtnTxt('btnAddFund');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
            GetFundDTable(); 
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
//Click Edit in action button
$(document).on('click', '#btnEditFundID', function (e) {
    var FundID = $(this).attr('FundAttr');
    GetFundForm(2, FundID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteFundID", function () {
    var PrimaryID = $(this).attr('FundAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceFund/DeleteFund', '/FileMaintenanceFund/ConfirmDelete', ' GetFundForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Sub Fund Tab
//---------------------------------------------------------------------------------------------------------------------
function GetSubFundTab() {
    $.ajax({
        url: "/FileMaintenanceFund/GetSubFundTab",
        success: function (result) {
            ClearTabContentFMFund();
            $("#tabSubFundID").html(result);
            GetSubFundForm(1, 0);
        }
    })
}
function GetSubFundForm(ActionID, SubFundID) {
    $.ajax({
        url: "/FileMaintenanceFund/GetSubFundForm",
        data: { ActionID: ActionID, SubFundID: SubFundID },
        success: function (result) {
            $('#subfundTempID').html(result);
            GetSubFundDTable();
            changeBtnTxt('btnAddSubFund');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetSubFundDTable() {
    $.ajax({
        url: '/FileMaintenanceFund/GetSubFundDTable',
        success: function (result) {
            $("#subfundTableID").html(result);
        }
    })
}
function DeleteSubFund(SubFundID) {
    $.ajax({
        url: "/FileMaintenanceFund/DeleteSubFund",
        data: { SubFundID: SubFundID },
        success: function (result) {
            GetSubFundForm(1, 0);
        }
    })
}
$(document).on('click', '#btnEditSubFundID', function (e) {
    var SubFundID = $(this).attr('SubFundAttr');
    GetSubFundForm(2, SubFundID);

});
// Dynamic var --------------------------------------------------
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

//var tabSelect = function (tabid) {
//    var tabactive = $(".tab-pane.active").attr("id");
//    if (tabactive == "tabFundID") {
//        tableid = '#fundTableID';
//        tempid = '#fundTempID';
//        tabid = 1;
//    } else {
//        tableid = '#subfundTableID';
//        tempid = '#subfundTempID';
//        tabid = 2;
//    }
//}