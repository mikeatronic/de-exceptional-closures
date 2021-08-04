$(document).ready(function () {
    let getReasonTypeId = $("#approvalForm input[type='radio']:checked").val();

    if (getReasonTypeId === "5") {
        $("#OtherReasons").show();
    }
    else {
        $("#OtherReasons").hide();
    }
});

function showHideOtherReason(id) {
    if (id === 5) {
        $("#OtherReasons").show();
    }
    else {
        $("#OtherReasons").hide();
    }
}