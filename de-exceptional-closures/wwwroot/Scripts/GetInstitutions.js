$('#SearchPostCode').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        getAddresses();
        e.preventDefault();
        return false;
    }
});

function getAddresses()
{
    let name = $("#Input_Search").val();
    $("#addressError").hide();

    if (name != "")
    {
        $("#loadSpinner").show();

        $.get('/Institution/GetInstitutions/', { name: name }, function (data)
        {
            $("#Input_SearchInstitutions").empty();
            $("#Input_SearchInstitutions").append($("<option value=''>Select institution</option>"));

            $.each(data, function ()
            {
                $(".govuk-error-summary").hide();
                $("#loadSpinner").hide();
                $("#SearchInstitutionList").show();
                $("#addressError").hide();
                $("#Input_SearchInstitutions").append($("<option></option>").val(this["referenceNumber"]).html(this["name"] + ',' + this["referenceNumber"]));
            });
            if (data.length === 0)
            {
                $(".govuk-error-summary").show();

                if ($(".error-items").length === 0)
                {
                    $(".govuk-error-summary__list").append("<li><a class='error-items' href='#Input_Search'>Not a real institution. Instiution could not be found.</a></li>");
                }

                $("#PostCodeSearchComponent").addClass("govuk-form-group--error");
                $("#Input_Search").addClass("govuk-input--error");
                $("#Input_Search").val("Not an institution")
                $("#addressError").show();
                $("#loadSpinner").hide();
                $("#SearchInstitutionList").hide();
            }

        }).fail(function ()
        {
            $(".govuk-error-summary").show();

            if ($(".error-items").length === 0)
            {
                $(".govuk-error-summary__list").append("<li><a class='error-items' href='#Input_Search'>Not a real inst. Instiution could not be found.</a></li>");
            }

            $("#PostCodeSearchComponent").addClass("govuk-form-group--error");
            $("#Input_Search").addClass("govuk-input--error");
            $("#Input_Search").val("Not an institution")
            $("#addressError").show();
            $("#loadSpinner").hide();
            $("#SearchInstitutionList").hide();
        });
    }
}

//function fillAddressTextBoxes() {
//    let myText = $("#SearchAddress :selected").text();

//    if (myText != "Select Address") {
//        let addressArray = myText.split(',');

//        $("#Address1").val("");
//        $("#Address2").val("");
//        $("#Address3").val("");
//        $("#TownCity").val("");
//        $("#PostCode").val("");

//        $("#Address1").val(addressArray[0]);
//        $("#TownCity").val(addressArray[1]);
//        $("#PostCode").val(addressArray[2]);
//    }
//}