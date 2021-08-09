$('#Input_Search').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        getSchools();
        e.preventDefault();
        return false;
    }
});

function getSchools()
{
    let name = $("#Input_Search").val();
    $("#institutionError").hide();
  
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
                $("#institutionError").hide();
                $("#Input_Search").removeClass("govuk-input--error");
                $("#InstituteSearchComponent").removeClass("govuk-form-group--error");

                $("#Input_SearchInstitutions").append($("<option></option>").val(this["referenceNumber"]).html(this["name"] + ',' + this["referenceNumber"]));
            });
            if (data.length === 0)
            {
                $(".govuk-error-summary").show();

                if ($(".error-items").length === 0)
                {
                    $(".govuk-error-summary__list").append("<li><a class='error-items' href='#Input_Search'>Institution not found. Institution could not be found.</a></li>");
                }

                $("#InstituteSearchComponent").addClass("govuk-form-group--error");
                $("#Input_Search").addClass("govuk-input--error");
                $("#institutionError").show();
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

            $("#InstituteSearchComponent").addClass("govuk-form-group--error");
            $("#Input_Search").addClass("govuk-input--error");
            $("#institutionError").show();
            $("#loadSpinner").hide();
            $("#SearchInstitutionList").hide();
        });
    }
}

function fillTextBoxes()
{
    let myText = $("#Input_SearchInstitutions :selected").text();

    if (myText != "Select institution")
    {
        let schoolArray = myText.split(',');

        $("#Input_InstitutionName").val("");
        $("#Input_InstitutionReference").val("");

        $("#Input_InstitutionName").val(schoolArray[0]);
        $("#Input_InstitutionReference").val(schoolArray[1]);
    }
}