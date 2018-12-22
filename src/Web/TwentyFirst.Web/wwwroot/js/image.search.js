// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function ($) {
    $('document').ready(function () {
        $("#search-button").click(function (e) {
            e.preventDefault();
            var search = $(".search-term").val();
            $.get("/Administration/Images/Search?search=" + search)
                .done(function GetNextData(data) {
                    $("#results-table").html(data);
                    var rows = document.querySelector("#results-table").getElementsByClassName("selected-image");
                    for (i = 0; i < rows.length; i++) {
                        rows[i].onclick = function () {
                            var imageId = $(this).attr("data-item-id");
                            $(".image-id").val(imageId);
                            var imageUrl = $(this).attr("data-item-url");
                            $(".image-url").val(imageUrl);
                            $('#show-image').html($('<img>', { src: imageUrl, class: "img-thumbnail" }));
                            $(".modal").modal("toggle");
                        }
                    }

                    $(".pagination li a").click(function (e) {
                        if (e.target.href) {
                            e.preventDefault();
                            $.get(e.target).done(function (nextData) {
                                GetNextData(nextData);
                            });
                        }
                    });
                });
        });

        $(".search-term").keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                $("#search-button").click();
            }
        });
    });
})(jQuery);