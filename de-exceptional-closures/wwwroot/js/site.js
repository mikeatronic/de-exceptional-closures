﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // Show error_summary if there any validation errors, this is so the summary shows for both front and back end validation.
    if ($(".error-items").length) {
        $(".govuk-error-summary").show();
    }
});