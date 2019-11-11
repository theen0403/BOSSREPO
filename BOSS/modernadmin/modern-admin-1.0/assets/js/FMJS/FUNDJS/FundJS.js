
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
            //GetFundDTable();
        }
    })
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
function GetFundForm(ActionID, FundID) {
    //tabSelect();
    $.ajax({
        url: "/FileMaintenanceFund/GetFundForm",
        data: { ActionID: ActionID, FundID: FundID },
        success: function (result) {
            $('#fundTempID').html(result);
            GetFundDTable(); 
            changeBtnTxt('btnAddFund');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () { 
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetFundDTable() {
    //tabSelect();
    $.ajax({
        url: '/FileMaintenanceFund/GetFundDTable',
        success: function (result) {
            $('#fundTableID').html(result);
            //GetFundForm(1, 0);
        }
    })
}
function changeBtnTxt(addBtnID) {
    if ($(".ActionID").val() == 1) {
        $("#" + addBtnID).text("Add");
    }
    else if ($(".ActionID").val() == 2) {
        $("#" + addBtnID).text("Save Changes");
        //$("#" + addBtnID).attr("disabled", true);
    }
}

//var ifExists = function (data, getForm) {
//    if (data.isExist == "true") {
//        getForm;
//        swalError("Error saving", "Record already exists.")
//    }
//    else if (data.isExist == "false") {
//        getForm;
//        swalSuccess('Success', 'Fund Saved.');
//    }
//    else {
//        getForm;
//        swalSuccess('Success', 'Updated Successfully.');
//    }
//}
$(document).on('click', '#btnEditFundID', function (e) {
    var FundID = $(this).attr('FundAttr');
    GetFundForm(2, FundID);

});

$(document).on('click', "#btnDeleteFundID", function () {
    var PrimaryID = $(this).attr('FundAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceFund/DeleteFund', '/FileMaintenanceFund/ConfirmDeleteFund', 'GetFundForm(1, 0)');
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
            changeBtnTxt('btnAddFund');
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