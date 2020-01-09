//TABS
$(document).on('click', '#deptNavID', function (e) {
    GetDeptTab();
    TabActive('tabDeptID');
});
$(document).on('click', '#funcNavID', function (e) {
    GetFunctionTab();
    TabActive('tabFuncID');
});
$(document).on('click', '#secNavID', function (e) {
    GetSecTab();
    TabActive('tabSecID');
});
function clearRCTab() {
    $("#tabDeptID").html("");
    $("#tabFuncID").html("");
    $("#tabSecID").html("");
}
//===============================================================================================
//Department Tab
//===============================================================================================
function GetDeptTab() {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetDeptTab",
        success: function (result) {
            clearRCTab();
            $("#tabDeptID").html(result);
            GetDeptForm(1, 0);
        }
    })
}
function GetDeptForm(ActionID, DeptID) {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetDeptForm",
        data: { ActionID: ActionID, DeptID: DeptID },
        success: function (result) {
            $("#departmentTempID").html(result);

            GetDeptDTable();
            changeBtnTxt('btnAddDept');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetDeptDTable() {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetDeptDTable",
        success: function (result) {
            $("#tableDepartmentID").html(result);
        }
    })
}
$(document).on('click', '#btnEditDeptID', function (e) {
    var TaxID = $(this).attr('DeptAttr');
    GetDeptForm(2, TaxID);
});
$(document).on('click', "#btnDeleteDeptID", function () {
    var PrimaryID = $(this).attr('DeptAttr');
    var RecordAttr = $(this).attr("RecordAttr");
    DeleteRecord(PrimaryID, '/FileMaintenanceResponsibility/DeleteDept', '/FileMaintenanceResponsibility/ConfirmDeleteDept', ' GetDeptTab()', RecordAttr);
});
//===============================================================================================
//FUNCTION Tab
//===============================================================================================
function GetFunctionTab() {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetFunctionTab",
        success: function (result) {
            clearRCTab();
            $("#tabFuncID").html(result);
            GetFuncForm(1, 0);
        }
    })
}
function GetFuncForm(ActionID, FunctionID) {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetFuncForm",
        data: { ActionID: ActionID, FunctionID: FunctionID },
        success: function (result) {
            $("#functionTempID").html(result);

            GetFuncDTable();
            changeBtnTxt('btnAddFunc');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetFuncDTable() {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetFuncDTable",
        success: function (result) {
            $("#functionTableID").html(result);
        }
    })
}
$(document).on('click', '#btnEditFuncID', function (e) {
    var FunctionID = $(this).attr('FuncAttr');
    GetFuncForm(2, FunctionID);
});
$(document).on('click', "#btnDeleteFuncID", function () {
    var PrimaryID = $(this).attr('FuncAttr');
    var RecordAttr = $(this).attr("RecordAttr");
    DeleteRecord(PrimaryID, '/FileMaintenanceResponsibility/DeleteFunction', '/FileMaintenanceResponsibility/ConfirmDeleteFunction', ' GetFunctionTab()', RecordAttr);
});
//===============================================================================================
//SECTION Tab
//===============================================================================================
function GetSecTab() {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetSecTab",
        success: function (result) {
            clearRCTab();
            $("#tabSecID").html(result);
            GetSectionForm(1, 0);
        }
    })
}
function GetSectionForm(ActionID, SectionID) {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetSectionForm",
        data: { ActionID: ActionID, SectionID: SectionID },
        success: function (result) {
            $("#sectionTempID").html(result);
            GetSectionDTable();
            changeBtnTxt('btnAddSection');
            $('form').removeData("validator");
            $.validator.unobtrusive.parse(document);
        }
    })
}
function GetSectionDTable() {
    $.ajax({
        url: "/FileMaintenanceResponsibility/GetSectionDTable",
        success: function (result) {
            $("#sectionTableID").html(result);

        }
    })
}
$(document).on('click', '#btnEditSectionID', function (e) {
    var SectionID = $(this).attr('SectionAttr');
    GetSectionForm(2, SectionID);
});
$(document).on('click', "#btnDeleteSectionID", function () {
    var PrimaryID = $(this).attr('SectionAttr');
    var RecordAttr = $(this).attr("RecordAttr");
    DeleteRecord(PrimaryID, '/FileMaintenanceResponsibility/DeleteSection', '/FileMaintenanceResponsibility/ConfirmDeleteSection', ' GetSecTab()', RecordAttr);
});
//===============================================================================================
// ONCHANGE JS
//===============================================================================================
function onChangeSector_SubSector() { // revision year drop down changes allotment class drop down
    var selectedSector = $(".sectorDD").val();
    $.ajax({
        url: "/FileMaintenanceResponsibility/ChangeSector_SubSector",
        data: { SectorID: selectedSector },
        success: function (result) {
            $(".subSectorDD > option").remove();
            var items = "";

            $.each(result, function (i, List) {
                items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
            });

            if (items != "") {
                $(".subSectorDD").append(items);
                $(".subSectorDD").append("<option value=0>N/A</option>");

            }
            else {
                $(".subSectorDD > option").remove();
                $(".subSectorDD").append("<option value=0>N/A</option>");
            }

        }
    });
}
function ChangeDepartment_FundDeptCode() {
    var selectedDept = $(".deptID").val();
    if (selectedDept != null || selectedDept != 0) {
        $.ajax({
            url: "/FileMaintenanceResponsibility/ChangeDepartment_FundDeptCode",
            data: { DeptID: selectedDept },
            success: function (result) {
                if (result.fundTitle != "" && result.deptCode != "") {
                    $(".fundTitle").val(result.fundTitle);
                    $(".deptCode").val(result.DeptOfficeCodefunc);
                }
                else {
                    $(".fundTitle").val("");
                    $(".deptCode").val("");
                }
            }
        });
    }
    else {
        $(".fundTitle").val("");
        $(".deptCode").val("");
    }

}
function ChangeDepartment_Function() { // major account group drop down changes sub major account group drop down
    var selctedDept = $(".deptDD").val();
    if (selctedDept != null) {
        $.ajax({
            url: "/FileMaintenanceResponsibility/ChangeDepartment_Function",
            data: { DeptID: selctedDept },
            success: function (result) {
                $(".functionDD > option").remove();
                var items = "";

                $.each(result, function (i, List) {
                    items += "<option value ='" + List.Value + "'>" + List.Text + "</option>";
                });

                if (items != "") {
                    $(".functionDD").append(items);

                }
                else {
                    $(".functionDD > option").remove();
                }
            }
        });
    }
    else {
        $(".functionDD > option").remove();
    }
}
