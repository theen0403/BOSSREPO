function LoadDatePicker() {
    $(".pickadate").pickadate();
}
function LoadDropzone() {
    $('.dropzone').dropzone();
}
function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}
function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.

    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        input_val = left_side + "." + right_side;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);
        input_val = input_val;

        // final formatting
        if (blur === "blur") {
            input_val += ".00";
        }
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}

//Select All checkbox
function Checkall() {
    $('#checkedAll').change(function () {
        $('.cb-element').prop('checked', this.checked);
    });

    $('.cb-element').change(function () {
        if ($('.cb-element:checked').length == $('.cb-element').length) {
            $('#checkedAll').prop('checked', true);
        }
        else {
            $('#checkedAll').prop('checked', false);
        }
    });
}

//Popover
function LoadPopOver() {
    $(".popover-trigger").popover({
        html: true,
        container: 'body',
    });
}
function LoadDataTable() {
    $('.zero-configuration').DataTable({
        destroy: true,
        "aLengthMenu": [[5, 10, 25, -1], [5, 10, 25, "All"]],
        "iDisplayLength": 5,
        drawCallback: function () {
            LoadPopOver();
        }
    });
}
function LoadICheck() {
    $('.skin-square input').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
        increaseArea: '10%'
    });
    $(".select2").select2();
}
function select2nosearch() {
    $(".select2nosearch").select2({
       minimumResultsForSearch: Infinity
    });
}
function PopoveLostFocus() {
    $('[data-toggle="popover"]').popover('hide');
}
function swalConfirmation(questionMessage, confirmationMessage, successTitle, action) {
    swal({
        title: questionMessage,
        text: confirmationMessage,
        icon: "warning",
        buttons: {
            cancel: {
                text: "Cancel",
                value: null,
                visible: true,
                className: "btn-danger",
                closeModal: true
            },
            confirm: {
                text: "Yes, I am sure",
                value: true,
                visible: true,
                className: "bg-active",
                closeModal: true
            }
        }
    })
        .then((willDelete) => {
            if (willDelete) {
                eval(action)
                swalSuccess(successTitle)
            } else {
            }
        });
}
function swalSuccess(title, message) {
    swal({
        title: title,
        text: message,
        icon: 'success',
        //timer: 3000,
        button: {
            text: 'Close',
            className: 'btn-danger'
        }
    });
}
function swalError(title, message) {
    swal({
        title: title,
        text: message,
        icon: 'error',
        button: {
            text: 'Close',
            className: 'btn-danger'
        }
    });
}
//Remove or Delete a Document, Image, Offense
var animationEvent = 'webkitAnimationEnd oanimationend msAnimationEnd animationend';
$('.ft-delete').attr('title', 'Delete');
$(document).on('click', '.ft-delete', function () {
    $(this).parent().addClass('animated flipOutX');
    $(this).parent().one(animationEvent, function (event) {
        $(this).remove();
    });
});
//Angular------------------------------------------------------------------------------
function AngularGlobalFunctions(ControllerName, ActionName, IDParams) {
    var scope = angular.element(document.getElementById(ControllerName)).scope();
    scope.$apply(function () {
        eval("scope." + ActionName + "(" + IDParams + ");");
    })
}
function AngularGlobalFunctionsTwoParams(ControllerName, ActionName, IDParams1, IDParams2) {
    var scope = angular.element(document.getElementById(ControllerName)).scope();
    scope.$apply(function () {
        eval("scope." + ActionName + "(" + IDParams1 + "," + IDParams2 + ");");
    })
}
function AngularGlobalEdit(ControllerName, ActionName, IDParams) {
    var scope = angular.element(document.getElementById(ControllerName)).scope();
    scope.$apply(function () {
        eval("scope." + ActionName + "(" + IDParams + ");");
    })
}
function AngularGlobalView(ControllerName, ActionName, IDParams) {
    var scope = angular.element(document.getElementById(ControllerName)).scope();
    scope.$apply(function () {
        eval("scope." + ActionName + "(" + IDParams + ");");
    })
}
function AngularGlobalDelete(IDParams, ActionName, ControllerName) {
    var scope = angular.element(document.getElementById(ControllerName)).scope();
    scope.$apply(function () {
        eval("scope." + ActionName + "(" + IDParams + ");");
    })
}
function AngularGlobalAlertsCalling(ControllerName, ActionName, ModalName, SuccessMess) {
    var scope = angular.element(document.getElementById(ControllerName)).scope();
    scope.$apply(function () {
        eval("scope." + ActionName + "();");
        eval("$('#" + ModalName + "').modal('toggle');");
        swalSuccess("Success", SuccessMess);
    });
}
function AngularGlobalAlertsCallingwoModal(ControllerName, ActionName, SuccessMess) {
    var scope = angular.element(document.getElementById(ControllerName)).scope();
    scope.$apply(function () {
        eval("scope." + ActionName + "();");
        swalSuccess("Success", SuccessMess);
    });
}
//End Angular------------------------------------------------------------------------------
//Gloabal Function-------------------------------------------------------------------------
function GlobalEdit(ActionName, IDParams) {
    eval(ActionName + "(" + IDParams + ");");
}
function GlobalDelete(IDParams, ActionName) {
    eval(ActionName + "(" + IDParams + ");");
}
function GlobalAlertsCallingwoModal( ActionName, SuccessMess,Param1) {
    eval(ActionName + "(" + Param1+");");
    swalSuccess("Success", SuccessMess);
}
function GlobalFunctionsTwoParams( ActionName, IDParams1, IDParams2) {
        eval(ActionName + "(" + IDParams1 + "," + IDParams2 + ");");
}
//End Gloabal Function---------------------------------------------------------------------

//FROM OSCAR
function DeleteRecord(PrimaryID, ControllerUrl, ControllerUrl2, FormField) {
    var ActionDelete = " ConfirmDelete(" + PrimaryID + ",'" + ControllerUrl2 + "'," + FormField + ")";
    var confirmMess2 = "Are you sure you want to delete this?";
    var titleMess = "Warning";
    var confirmMess = "This record is used by other table. Once deleted, you will not be able to recover this record! Are you sure you want to delete this?";
    var successDeleteMess = "Your record has been deleted!";
    var warningMsgTitle = "Warning";
    var cancelMsg = "Cancelled";
    $.ajax({
        url: ControllerUrl,
        data: { PrimaryID: PrimaryID },
        success: function (result) {
            if (result.confirmDelete == "true") {
                swalConfirmation(titleMess, confirmMess, successDeleteMess, ActionDelete);
            }
            else if (result.confirmDelete == "false") {
                swalConfirmation(titleMess, confirmMess2, successDeleteMess, ActionDelete);
            }
            else if (result.confirmDelete == "restricted") {
                swalError("Error", "Unable to delete. This record is used by other table.");
            }
        }
    })
}
function ConfirmDelete(PrimaryID, ControllerUrl2, FormField) {
    $.ajax({
        url: ControllerUrl2,
        data: { PrimaryID: PrimaryID },
        success: function (result) {
        FormField;
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
//END FROM OPPA OSCAR

//Start Class Numbers only
function numbersOnly() {
    $('.numbersOnly').keypress(validateNumber);
}
function validateNumber(event) {
    var key = window.event ? event.keyCode : event.which;
    if (event.keyCode === 8 || event.keyCode === 46) {
        return true;
    } else if (key < 48 || key > 57) {
        return false;
    } else {
        return true;
    }
};
// Do not allow Special Characters
function noSpeChar() {
    $(".noSpeChar").keypress(function (e) {
        var keyCode = e.which;
        /* 
        48-57 - (0-9)Numbers
        65-90 - (A-Z)
        97-122 - (a-z)
        8 - (backspace)
        32 - (space)
        */
        // Not allow special 
        if (!((keyCode < 48 && keyCode > 57)
            || (keyCode >= 65 && keyCode <= 90)
            || (keyCode >= 97 && keyCode <= 122))
            && keyCode != 8 && keyCode != 32) {
            e.preventDefault();
        }
    });
}
function validateCharacters() {
    $("#sub").click(function () {
        var fn = $("#folderName").val();
        var regex = /^[0-9a-zA-Z\_]+$/
    });
}

(function (window, undefined) {
  'use strict';

    $(function () {
        $(document).on('click', '.t-has-destination tbody tr', function (e) {
            e.preventDefault();

            //Get table destination
            var destination = $(this).parent().parent().attr('destination');
            console.log(destination);

            //Get target desc and table
            var targetDesc = $(this).find('td.target-desc').text();
            var targetTable = $('.card#' + destination + '');

            console.log(targetDesc);
            console.log(targetTable);

            //Remove tr that has a .bg-active
            $(this).parent().find('tr.active').removeClass('active');

            //Add .bg-active to $(this)
            $(this).addClass('active');

            $(targetTable).find('.card-title span').html("<strong>[" + targetDesc + "]</strong>");

            // var target = this.hash;
            var target = '#' + destination;
            var $target = $(target);

            console.log(target);
            var scrollTo = ($target.offset().top - 100);
            console.log(scrollTo);
            //scroll and show hash
            $('html, body').delay(500).animate({
                'scrollTop': scrollTo
            }, 500, 'swing', function () {
                //window.location.hash = target;
            });
        });
    });
  /*
  NOTE:
  ------
  PLACE HERE YOUR OWN JAVASCRIPT CODE IF NEEDED
  WE WILL RELEASE FUTURE UPDATES SO IN ORDER TO NOT OVERWRITE YOUR JAVASCRIPT CODE PLEASE CONSIDER WRITING YOUR SCRIPT HERE.  */

})(window);









