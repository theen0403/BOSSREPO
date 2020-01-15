function isNumber(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // �-� CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // �.� CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}

$(document).on('keypress', '.noSpecialChar', function () {

    var regex = new RegExp("^[a-zA-Z0-9.,- ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }

});
$(document).on('keypress', '.numbersOnly', function () {
    var regex = new RegExp("^[0-9]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});
$(document).on('keypress', '.moneyFormat', function () {
    
    var value = $(this).val();
    value = value.replace(/^(0*)/, "");
    $(this).val(value);

    if (((event.which != 46 || (event.which == 46 && $(this).val() == '')) ||
        $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
}).on('paste', function (event) {
    event.preventDefault();
});