//Onlaoad JS
$(document).on('click', '#fundSourceNavID', function (e) {
    GetFundSourceTab();
    TabActive('tabFundSource');
});
$(document).on('click', '#AppropSourceNavID', function (e) {
    GetAppSourceTab();
    TabActive('tabApp');
});
function ClearTabContentFMAppropSource() {
    $("#tabFundSource").html("");
    $("#tabApp").html("");
}
//---------------------------------------------------------------------------------------------------------------------
//Fund Source Tab
//---------------------------------------------------------------------------------------------------------------------
function GetFundSourceTab() {
    $.ajax({
        url: "/FileMaintenanceAppropriation/GetFundSourceTab",
        success: function (result) {
            ClearTabContentFMAppropSource();
            $('#tabFundSource').html(result);
            GetFundSourceForm(1, 0);
        }
    })
}
function GetFundSourceForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceAppropriation/GetFundSourceForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#fundSourceTampID').html(result);

            changeBtnTxt('btnAddFundSource');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
            GetFundSourceDTable();
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetFundSourceDTable() {
    $.ajax({
        url: '/FileMaintenanceAppropriation/GetFundSourceDTable',
        success: function (result) {
            $('#fundSourceTableID').html(result);
        }
    })
}
//Click Edit in action button
$(document).on('click', '#btnEditFundSourceID', function (e) {
    var FundSourceID = $(this).attr('FundSourceAttr');
    GetFundSourceForm(2, FundSourceID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteFundSourceID", function () {
    var PrimaryID = $(this).attr('FundSourceAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceAppropriation/DeleteFundSource', '/FileMaintenanceAppropriation/ConfirmDelete', ' GetFundSourceForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Approp Source Tab
//---------------------------------------------------------------------------------------------------------------------
function GetAppSourceTab() {
    $.ajax({
        url: "/FileMaintenanceAppropriation/GetAppSourceTab",
        success: function (result) {
            ClearTabContentFMAppropSource();
            $('#tabApp').html(result);
            GetAppropSourceForm(1, 0);
        }
    })
}
function GetAppropSourceForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceAppropriation/GetAppropSourceForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#AppropSoureIDTemp').html(result);

            changeBtnTxt('btnAddAppropSource');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
            GetAppropSourceDTable();
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetAppropSourceDTable() {
    $.ajax({
        url: '/FileMaintenanceAppropriation/GetAppropSourceDTable',
        success: function (result) {
            $('#ApproSourceID').html(result);
        } 
    })
}
function onChangeFundSource() { // sub major account group drop down changes general account group drop down
    var selectedAppropSource = $("#AppropSourceDropDown").val();
    if (selectedAppropSource != null) {
        $.ajax({
            url: "/FileMaintenanceAppropriation/onChangeFundSource",
            data: { AppropSourceTypeID: selectedAppropSource },
            success: function (result) {
                $("#FundSourceID > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $("#FundSourceID").append(items);

                }
                else {
                    $("#FundSourceID > option").remove();
                }
            }
        });
    }
    else {
        $("#FundSourceID > option").remove();
    }
}
//Edit data in table
$(document).on('click', '#btnEditAppropSourceID', function (e) {
    var AppropriationID = $(this).attr('AppropSourceAttr');
    GetAppropSourceForm(2, AppropriationID);
});
//Delete data in table
function DeleteAppropSource(AppropriationID) {
    $.ajax({
        url: "/FileMaintenanceAppropriation/DeleteAppropSource",
        data: { AppropriationID: AppropriationID },
        success: function (result) {
            GetAppropSourceForm(1, 0);
        }
    })
}