$(function () {
    $(".datefield").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-5:+5", // last five years and future five years.  you can set the year range as per as your need.
        dateFormat: 'm/d/yy'
    });
});