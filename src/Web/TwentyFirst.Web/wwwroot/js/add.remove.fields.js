(function ($) {
    $('document').ready(function () {
        $("#add").click(function () {
            var lastField = $("#options div:last");
            var intId = (lastField && lastField.length && lastField.data("idx") + 1) || 2;
            var fieldWrapper = $("<div class=\"fieldwrapper\" id=\"field" + intId + "\"/>");
            fieldWrapper.data("idx", intId);
            var fName = $("<input type=\"text\" class=\"fieldname form-control\" name=\"options\" />");
            var removeButton = $("<input type=\"button\" class=\"remove btn btn-sm btn-warning text-white\" value=\"–\" />");
            removeButton.click(function () {
                $(this).parent().remove();
            });
            fieldWrapper.append(fName);
            fieldWrapper.append(removeButton);
            $("#options").append(fieldWrapper);
        });
    });
})(jQuery);