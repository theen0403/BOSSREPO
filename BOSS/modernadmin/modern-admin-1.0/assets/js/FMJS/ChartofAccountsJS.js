//Onlaoad
$(document).on('click', '#RevisionNavTabID', function (e) {
    GetRevisionTab();
    TabActive('tabRevision');
});
$(document).on('click', '#AllotmentNavTabID', function (e) {
    AllotmentClassTab();
    TabActive('tabAllotment');
});
$(document).on('click', '#AccountGroupNavTabID', function (e) {
    AGTab();
    TabActive('tabAccntGroup');
});
$(document).on('click', '#MajorAccountGroupNavTabID', function (e) {
    MAGTab();
    TabActive('tabMajorAccnt');
});
$(document).on('click', '#SubMajorAccountGroupNavTabID', function (e) {
    SMAGTab();
    TabActive('tabSubMajorAccnt');
});
$(document).on('click', '#GeneralAccountNavTabID', function (e) {
    GATab();
    TabActive('tabGenAccnt');
});
$(document).on('click', '#SubsiLedgerNavTabID', function (e) {
    GetSubsidiaryTab();
    TabActive('tabSubsiLedger');
});
function ClearTabContentFileMaintenanceAccounts() {
    $("#tabRevision").html("");
    $("#tabAllotment").html("");
    $("#tabAccntGroup").html("");
    $("#tabMajorAccnt").html("");
    $("#tabSubMajorAccnt").html("");
    $("#tabGenAccnt").html("");
    $("#tabSubsiLedger").html("");}
//---------------------------------------------------------------------------------------------------------------------
//Revision Tab
//---------------------------------------------------------------------------------------------------------------------
function GetRevisionTab() {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetRevisionTab",
        success: function (result) {
            ClearTabContentFileMaintenanceAccounts();
            $('#tabRevision').html(result);
            GetRevisionForm(1, 0);
        }
    })
}
function GetRevisionForm(ActionID, PrimaryID) {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetRevisionForm",
        data: { ActionID: ActionID, PrimaryID: PrimaryID },
        success: function (result) {
            $('#RCOATempID').html(result);

            GetRevisionDTable();
            changeBtnTxt('btnAddRev');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            alert('An error occured while attempting to get the content');
        }
    })
}
function GetRevisionDTable() {
    $.ajax({
        url: '/FileMaintenanceAccounts/GetRevisionDTable',
        success: function (result) {
            $('#tableRCOAID').html(result);
        }
    })
}
$(document).on('click', '#btnEditRevID', function (e) {
    var RevID = $(this).attr('RevAttr');
    GetRevisionForm(2, RevID);
});
//Click Delete in action button
$(document).on('click', "#btnDeleteRevID", function () {
    var PrimaryID = $(this).attr('RevAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceAccounts/DeleteRevision', '/FileMaintenanceAccounts/ConfirmDelete', ' GetRevisionForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Allotment Tab
//---------------------------------------------------------------------------------------------------------------------
function AllotmentClassTab() {
    $.ajax({
        url: "/FileMaintenanceAccounts/AllotmentClassTab",
        success: function (result) {
            ClearTabContentFileMaintenanceAccounts();
            $('#tabAllotment').html(result);
            GetAllotClassForm(1, 0);
        }
    })
}
function GetAllotClassForm(ActionID, AllotmentClassID) {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetAllotClassForm",
        data: { ActionID: ActionID, AllotmentClassID: AllotmentClassID },
        success: function (result) {
            $("#allotmentTempID").html(result);

            GetAllotmentClassDT();
            changeBtnTxt('btnAddAllot');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetAllotmentClassDT() {
    var RevID = $("#RevID").val();
    $.ajax({
        url: "/FileMaintenanceAccounts/GetAllotmentClassDT",
        data: { RevID: RevID},
        success: function (result) {
            $(".allotClassList-container").html(result)

        }
    })
}
$(document).on('click', '#btnEditAllotID', function (e) {
    var AllotmentClassID = $(this).attr('AllotAttr');
    GetAllotClassForm(2, AllotmentClassID);
});
$(document).on('click', "#btnDeleteAllotID", function () {
    var PrimaryID = $(this).attr('AllotAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceAccounts/DeleteAllotClass', '/FileMaintenanceAccounts/ConfirmDeleteAllot', ' GetAllotClassForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Acount Group Tab
//---------------------------------------------------------------------------------------------------------------------
function AGTab() {
    $.ajax({
        url: "/FileMaintenanceAccounts/AGTab",
        success: function (result) {
            ClearTabContentFileMaintenanceAccounts();
            $('#tabAccntGroup').html(result);
            GetAGForm(1, 0);
        }
    })
}
function GetAGForm(ActionID, AGID) {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetAGForm",
        data: { ActionID: ActionID, AGID: AGID },
        success: function (result) {
            $("#accntGrpTempID").html(result);

            GetAGDTable();
            changeBtnTxt('btnAddAG');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetAGDTable() {
    var RevID = $("#RevID").val();
    $.ajax({
        url: "/FileMaintenanceAccounts/GetAGDTable",
        data: { RevID: RevID },
        success: function (result) {
            $("#AccntGrpTableID").html(result)

        }
    })
}
$(document).on('click', '#btnEditAGID', function (e) {
    var AGID = $(this).attr('AGAttr');
    GetAGForm(2, AGID);
});
$(document).on('click', "#btnDeleteAGID", function () {
    var PrimaryID = $(this).attr('AGAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceAccounts/DeleteAG', '/FileMaintenanceAccounts/ConfirmDeleteAG', ' GetAGForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Major Acount Group Tab
//---------------------------------------------------------------------------------------------------------------------
function MAGTab() {
    $.ajax({
        url: "/FileMaintenanceAccounts/MAGTab",
        success: function (result) {
            ClearTabContentFileMaintenanceAccounts();
            $('#tabMajorAccnt').html(result);
            GetMAGForm(1, 0);
        }
    })
}
function GetMAGForm(ActionID, MAGID) {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetMAGForm",
        data: { ActionID: ActionID, MAGID: MAGID },
        success: function (result) {
            $("#MAGTempID").html(result);

            GetMAGDTable()
            changeBtnTxt('btnAddMAG');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetMAGDTable() {
    var RevID = $("#RevID").val();
    $.ajax({
        url: "/FileMaintenanceAccounts/GetMAGDTable",
        data: { RevID: RevID },
        success: function (result) {
            $("#tableMAGTempID").html(result)

        }
    })
}
$(document).on('click', '#btnEditMAGID', function (e) {
    var MAGID = $(this).attr('MAGAttr');
    GetMAGForm(2, AGID);
});
$(document).on('click', "#btnDeleteMAGID", function () {
    var PrimaryID = $(this).attr('MAGAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceAccounts/DeleteMAG', '/FileMaintenanceAccounts/ConfirmDeleteMAG', ' GetMAGForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//Sub Major Acount Group Tab
//---------------------------------------------------------------------------------------------------------------------
function SMAGTab() {
    $.ajax({
        url: "/FileMaintenanceAccounts/SMAGTab",
        success: function (result) {
            ClearTabContentFileMaintenanceAccounts();
            $('#tabSubMajorAccnt').html(result);
            GetSMAGForm(1, 0);
        }
    })
}
function GetSMAGForm(ActionID, SMAGID) {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetSMAGForm",
        data: { ActionID: ActionID, SMAGID: SMAGID },
        success: function (result) {
            $("#SMAGTempID").html(result);

            GetSMAGDTable();
            ChangeMajorAccountCode();
            changeBtnTxt('btnAddSMAG');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetSMAGDTable() {
    var RevID = $("#RevID").val();
    $.ajax({
        url: "/FileMaintenanceAccounts/GetSMAGDTable",
        data: { RevID: RevID },
        success: function (result) {
            $("#tableSMAGTempID").html(result)

        }
    })
}
$(document).on('click', '#btnEditSMAGID', function (e) {
    var SMAGID = $(this).attr('SMAGAttr');
    GetSMAGForm(2, SMAGID);
});
$(document).on('click', "#btnDeleteSMAGID", function () {
    var PrimaryID = $(this).attr('SMAGAttr');
    DeleteRecord(PrimaryID, '/FileMaintenanceAccounts/DeleteSMAG', '/FileMaintenanceAccounts/ConfirmDeleteSMAG', ' GetSMAGForm(1, 0)');
});
//---------------------------------------------------------------------------------------------------------------------
//General Account Tab
//---------------------------------------------------------------------------------------------------------------------
function GATab() {
    $.ajax({
        url: "/FileMaintenanceAccounts/GATab",
        success: function (result) {
            ClearTabContentFileMaintenanceAccounts();
            $('#tabGenAccnt').html(result);
            GetGAForm(1, 0);
        }
    })
}
function GetGAForm(ActionID, GAID) {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetGAForm",
        data: { ActionID: ActionID, GAID: GAID },
        success: function (result) {
            $("#GATempID").html(result);

            GetGADTable();
            ChangeSubMajorAccountCode();
            ChangeGenAccountCode();
            changeBtnTxt('btnAddGA');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetGADTable() {
    var RevID = $("#RevID").val();
    $.ajax({
        url: '/FileMaintenanceAccounts/GetGADTable',
        data: { RevID: RevID },
        success: function (result) {
            $("#tableGAID").html(result);
        }
    })
}
$(document).on('click', "#contraAccntIDCheckBox", function () {
    var chk = $(this).is(':checked');
    if (chk == true) {
        ;
        $("#subAccntIDCheckBox").prop("checked", false);
        $("#genAccntGrpDropDownID").prop("disabled", false);
        // $("#genIDError").removeClass("hidden");
        $(".lblGenAccnt").html("Contra Account");

        $(".subMajAccntCode").removeClass("hidden");
        $(".genAccntCode").addClass("hidden");

    }
    else if (chk == false) {
        $("#genAccntGrpDropDownID").prop("disabled", true);
        // $("#genIDError").addClass("hidden");
        $(".lblGenAccnt").html("General Ledger Account");

        $(".subMajAccntCode").removeClass("hidden");
        $(".genAccntCode").addClass("hidden");
    }
    onChangeSubMajAccntGrp_GenAccntGrp();
});
$(document).on('click', "#subAccntIDCheckBox", function () {
    var chk = $(this).is(':checked');
    if (chk == true) {
        $("#contraAccntIDCheckBox").prop("checked", false);
        $("#genAccntGrpDropDownID").prop("disabled", false);

        $(".lblGenAccnt").html("Sub Account");

        $(".subMajAccntCode").addClass("hidden");
        $(".genAccntCode").removeClass("hidden");

        $("#accntCodeID").removeClass("numbersOnly");
    }
    else if (chk == false) {
        $("#genAccntGrpDropDownID").prop("disabled", true);
        $("#genAccntGrpDropDownID-error").addClass("hidden");

        $(".lblGenAccnt").html("General Ledger Account");

        $(".subMajAccntCode").removeClass("hidden");
        $(".genAccntCode").addClass("hidden");

        $("#accntCodeID").addClass("numbersOnly");
    }
    onChangeSubMajAccntGrp_GenAccntGrp();
});
$(document).on('change', ".withReserve", function () {
    var reserve = $('.withReserve option:selected').val();

    switch (reserve) {
        case "true":
            $(".rsrvedPercent").prop("disabled", false);
            break;
        case "false":
            $(".rsrvedPercent").prop("disabled", true);
            break;
        default:
            $(".rsrvedPercent").prop("disabled", true);
            break;
    }
});
$(document).on('click', '#btnEditGAID', function (e) {
    var GenAccntID = $(this).attr('GAAttr');
    GetGenAccForm2(2, GAID);
});

$(document).on('click', "#btnDeleteGAID", function () {
    var PrimaryID = $(this).attr('GAAttr');
    DeleteRecord(PrimaryID, '/FMCOA/DeleteGA', '/FMCOA/ConfirmDeleteGA', ' GetGAForm(1, 0)');
});
//--------------------------
function GetSubsidiaryTab() {
    $.ajax({
        url: "/FileMaintenanceAccounts/GetSubsidiaryTab",
        success: function (result) {
            ClearTabContentFileMaintenanceAccounts();
            $('#tabSubsiLedger').html(result);
        }
    })
}












































//--------------------------------------------------
// Onchange
//--------------------------------------------------
//first used in Account Tab 
function onChangeRevisionYear_AllotClass() { // revision year drop down changes allotment class drop down
    var selectedRevYear = $(".revYearDropDown").val();
    $.ajax({
        url: "/FileMaintenanceAccounts/ChangeRevisionYear_AllotClass",
        data: { RevID: selectedRevYear },
        success: function (result) {
            $(".allotClassDropDown > option").remove();
            var items = "";

            $.each(result, function (i, List) {
                items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
            });

            if (items != "") {
                $(".allotClassDropDown").append(items);
                $(".allotClassDropDown").append("<option value=0>N/A</option>");

            }
            else {
                $(".allotClassDropDown > option").remove();
                $(".allotClassDropDown").append("<option value=0>N/A</option>");
            }
           onChangeAllotClass_AccntGrp();

        }
    });
}
function onChangeAllotClass_AccntGrp() {  // allotment class drop down changes account group drop down
    var selectedAllotClass = $(".allotClassDropDown").val();
    var selectedRevYear = $(".revYearDropDown").val();
    if (selectedAllotClass != null || selectedRevYear != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeAllotClass_AccntGrp",
            data: { AllotmentClassID: selectedAllotClass, RevID: selectedRevYear },
            success: function (result) {
                $(".accntGrpDropDown > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $(".accntGrpDropDown").append(items);

                }
                else {
                    $(".accntGrpDropDown > option").remove();
                }

                if ($("#tabMajorAccnt").hasClass("active")) {
                    ChangeAccountCode();
                }
                onChangeAccntGrp_MajAccntGrp();
            }
        });
    }
   
}
function onChangeAccntGrp_MajAccntGrp() { // account group drop down changes major account group drop down
    var selectedAccntGrp = $(".accntGrpDropDown").val();
    if (selectedAccntGrp != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeAccntGrp_MajAccntGrp",
            data: { AGID: selectedAccntGrp },
            success: function (result) {
                $(".majAccntGrpDropDown > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $(".majAccntGrpDropDown").append(items);

                }
                else {
                    $(".majAccntGrpDropDown > option").remove();
                }
                if ($("#tabSubMajorAccnt").hasClass("active")) {
                    ChangeMajorAccountCode();
                }

                onChangeMajAccntGrp_SubMajAccntGrp();
            }
        });

    }
    else {
        $(".majAccntGrpDropDown > option").remove();
        $(".subMajAccntGrpDropDown > option").remove();
        $(".majAccntCode").text("");
        $(".subMajAccntCode").text("");
    }

}
function onChangeMajAccntGrp_SubMajAccntGrp() { // major account group drop down changes sub major account group drop down
    var selectedMajAccntGrp = $(".majAccntGrpDropDown").val();
    if (selectedMajAccntGrp != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeMajAccntGrp_SubMajAccntGrp",
            data: { MAGID: selectedMajAccntGrp },
            success: function (result) {
                $(".subMajAccntGrpDropDown > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $(".subMajAccntGrpDropDown").append(items);

                }
                else {
                    $(".subMajAccntGrpDropDown > option").remove();
                }
                if ($("#tabGenAccID").hasClass("active")) {
                    ChangeSubMajorAccountCode();
                    ChangeGenAccountCode();
                }
                //onChangeSubMajAccntGrp_GenAccntGrp();
            }
        });
    }
    else {
        $(".subMajAccntGrpDropDown > option").remove();
        $(".subMajAccntCode").text("");
    }
}
function onChangeSubMajAccntGrp_GenAccntGrp() { // sub major account group drop down changes general account group drop down
    var selectedMajAccntGrp = $(".subMajAccntGrpDropDown").val();
    var chckBoxContra = $("#contraAccntIDCheckBox").is(':checked');
    var chckBoxSub = $("#subAccntIDCheckBox").is(':checked');
    if (selectedMajAccntGrp != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeSubMajAccntGrp_GenAccntGrp",
            data: { SubMajAccntID: selectedMajAccntGrp, chckBoxContra: chckBoxContra, chckBoxSub: chckBoxSub },
            success: function (result) {
                $(".genAccntGrpDropDown > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $(".genAccntGrpDropDown").append(items);
                    $("#genIDError").addClass("hidden");
                }
                else {

                    $("#genIDError").removeClass("hidden");
                }

                ChangeSubMajorAccountCode();
                ChangeGenAccountCode();
            }
        });
    }
    else {
        $(".genAccntGrpDropDown > option").remove();
        $(".subMajAccntCode").text("");
    }
}

//===========================================
//ONCHANGE CODES
//===========================================
function ChangeAccountCode(AGID = $('.accntGrpDropDown option:selected').val()) { // account group drop down changes account code
    if (AGID != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeAccountCode",
            data: { AGID: AGID },
            success: function (result) {
                if (result.passCon != null) {
                    $(".accntGrpCode").text(result.passCon + "-");
                }
            }
        })
    }
    else {
        $(".accntGrpCode").text("");
    }
}
function ChangeMajorAccountCode(MAGID = $('.majAccntGrpDropDown option:selected').val()) { // major account group drop down changes major account code
    if (MAGID != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeMajAccountCode",
            data: { MAGID: MAGID },
            success: function (result) {
                if (result.passCon != null) {
                    $(".majAccntCode").text(result.passCon);
                }

            }
        })
    }
    else {
        $(".majAccntCode").text("");
    }
}
function ChangeSubMajorAccountCode(SMAGID = $('.subMajAccntGrpDropDown option:selected').val()) { // sub major account group drop down changes sub major account code
    if (SMAGID != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeSubMajorAccountCode",
            data: { SMAGID: SMAGID },
            success: function (result) {
                if (result.passCon != null) {
                    $(".subMajAccntCode").text(result.passCon);

                }

            }
        })
    }
    else {
        $(".subMajAccntCode").text("");
    }
}
function ChangeGenAccountCode(GAID = $('.genAccntGrpDropDown option:selected').val()) { // sub major account group drop down changes sub major account code
    if (GAID != null) {
        $.ajax({
            url: "/FileMaintenanceAccounts/ChangeGenAccountCode",
            data: { GAID: GAID },
            success: function (result) {
                if (result.passCon != null) {
                    $(".genAccntCode").html(result.passCon + "-");
                    $(".genAccntCode1").val(result.passCon);
                }
            }
        })
    }
    else {
        $(".genAccntCode").html("");
        $(".genAccntCode1").val("");
    }
}