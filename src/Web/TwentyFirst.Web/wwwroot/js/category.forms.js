(function ($) {
    $('document').ready(function () {
        $('.create').click(function () {
            $.get("/Administration/Categories/Create")
                .done(function (data) {
                    var container = document.getElementById("create-container");
                    $(container).html(data);
                    var forms = container.getElementsByTagName("form");
                    var newForm = forms[forms.length - 1];
                    $.validator.unobtrusive.parse(newForm);
                    $('#create-modal').modal('show');
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus + ": Изникна проблем при зареждането на формата. " + errorThrown);
                });
        });

        $('.edit').click(function (e) {
            var categoryId = $(e.target).attr("data-item-id");
            $.get("/Administration/Categories/Edit/" + categoryId
            ).done(function (data) {
                var container = document.getElementById("edit-container");
                $(container).html(data);
                var forms = container.getElementsByTagName("form");
                var newForm = forms[forms.length - 1];
                $.validator.unobtrusive.parse(newForm);
                $('#edit-modal').modal('show');
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("Изникна проблем при зареждането на формата.");
            });
        });

        $('.archive').click(function (e) {
            var categoryId = $(e.target).attr("data-item-id");
            $.get("/Administration/Categories/Archive/" + categoryId
            ).done(function (data) {
                var container = document.getElementById("archive-container");
                $(container).html(data);
                $('#archive-modal').modal('show');
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("Изникна проблем при зареждането на формата.");
            });
        });

        $('.recover').click(function (e) {
            var categoryId = $(e.target).attr("data-item-id");
            $.get("/Administration/Categories/Recover/" + categoryId
            ).done(function (data) {
                var container = document.getElementById("recover-container");
                $(container).html(data);
                $('#recover-modal').modal('show');
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("Изникна проблем при зареждането на формата.");
            });
        });
    });
})(jQuery);