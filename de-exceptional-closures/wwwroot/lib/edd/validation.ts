
if ($('form').length > 0) {
    
    let submitted = false;
    
    $("form").bind("invalid-form.validate", function () {
        submitted = true;
    });

    $('form').removeData("validator")
             .removeData("unobtrusiveValidation");
    
    $.validator.setDefaults({
        errorClass: 'govuk-error-message',
        ignore: 'input[type=hidden]:not(.monthtimehidden)',
        highlight: function (element, errorClass, validClass) {
            let el: JQuery = $(element);
            let fg = el.closest(".govuk-form-group");
            fg.addClass("govuk-form-group--error");
            el.addClass("govuk-input--error");
        },
        unhighlight: function (element, errorClass, validClass) {
            let el: JQuery = $(element);
            let fg = el.closest(".govuk-form-group");
            fg.removeClass("govuk-form-group--error");
            el.removeClass("govuk-input--error");
        },
        showErrors: function (errorMap, errorList) {
            if (submitted) {
                let summary = '';
                let errorHint = '';
                if (errorList.length > 0) {
                    $('.govuk-error-summary').show();
                }
                else {
                    $('.govuk-error-summary').hide();
                }
                $.each(errorList, function () { errorHint += this.message });
                $.each(errorList, function () { summary += "<li><a href='#"+ this.element.id +"'>" + this.message + "</a></li>\n"; });
                $('.govuk-error-summary__list').html(summary);
                $('.govuk-error-message').append(errorHint);
                submitted = false;
            }
            this.defaultShowErrors();
        },
        invalidHandler: function (event, validator) {
            submitted = true;
        }
    });
    $.validator.unobtrusive.parse("form");
}