(function ($) {
    $("document").ready(function () {
        var href = window.location.href;

        $("a.show-active").each(function () {
            if (this.href == href) {
                $(this).addClass("text-admin-twenty-first font-weight-bold");
            }
        });

        $("#subscribe-button").click(function (e) {
            e.preventDefault();
            var email = $("#subscriber-email").val();
            $.post("/Subscribers/Subscribe", { email: email })
                .done(function (data) {
                    $("#subscribe-result").html(data);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    alert("Изникна проблем при изпращенето на формата.");
                });
        });

        $("#subscriber-email").keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                $("#subscribe-button").click();
            }
        });

        $("#poll-form").submit(function (e) {
            e.preventDefault();

            $.getJSON('https://ipapi.co/json/',
                function (data) {
                    var ip = data.ip;
                    var url = $("#poll-form").attr("action");
                    var poll = $("#poll-form").serialize() + "&voteIp=" + ip;

                    $.post(url, poll).done(function (result) {
                        $("#pole-vote-result").html(result);
                    }).fail(function () {
                        alert("Изникна проблем при изпращенето на формата.");
                    });
                    return false;
                });
        });

        function translateDescription(description) {
            var xhttp = new XMLHttpRequest();
            xhttp.open("GET",
                "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=bg&dt=t&q=" + description,
                false);
            xhttp.send();
            var response = JSON.parse(xhttp.responseText);
            return response[0][0][0];
        };

        var geo = {};

        //Sofia, Bulgaria Lat Long Coordinates
        geo.lat = "42.698334";
        geo.lng = "23.319941";
        showWeather(geo.lat, geo.lng);

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success);
        }

        function showWeather(lat, lng) {
            $.ajax({
                url: "https://api.openweathermap.org/data/2.5/weather?units=metric&lang=bg&lat=" +
                    lat + "&lon=" + lng + "&APPID=127b3297581e099bd5ecc657bf34b3fb",
                dataType: "jsonp",
                success: function (data) {
                    var location = translateDescription(data["name"]);
                    var temp = data["main"]["temp"];
                    var icon = data["weather"][0]["icon"];
                    var iconUrl = "https://openweathermap.org/img/w/" + icon + ".png";
                    var desc = translateDescription(data["weather"][0]["description"]);
                    $("#location").html(location);
                    $("#temp").html(temp);
                    $("#desc").html(desc);
                    $("#img").attr("src", iconUrl);
                }
            });
        }

        function success(position) {
            geo.lat = position.coords.latitude;
            geo.lng = position.coords.longitude;
            showWeather(geo.lat, geo.lng);
        }
    });
})(jQuery);