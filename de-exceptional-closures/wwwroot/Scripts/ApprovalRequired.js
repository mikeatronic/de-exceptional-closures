$(document).ready(function () {
    let getReasonTypeId = $("#approvalForm input[type='radio']:checked").val();

    if (getReasonTypeId === "5") {
        $("#OtherReasons").show();
    }
    else {
        $("#OtherReasons").hide();
    }
});