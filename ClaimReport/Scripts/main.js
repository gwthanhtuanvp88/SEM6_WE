$(document).on("ready", function () {
    // Confirm
    // ===============================
    $(".confirm-submit").click(function (e) {
        var content = "Please Verify Your Informations: \n";
        $(".confirm").each(function () {
            if ($(this).find(".form-control").val() != "") {
                if ($(this).find(".form-control").is("select")) {
                    content += $(this).find(".control-label").html() + ": " + $(this).find(".form-control").find("option:selected").text() + "\n";
                } else {
                    content += $(this).find(".control-label").html() + ": " + $(this).find(".form-control").val() + "\n";
                }
            }
        });

        e.preventDefault();
        var result = confirm(content);
        if (result) {
            $("form").submit();
        }

    });

    $(".confirm-delete").click(function (e) {
        e.preventDefault();
        var href = $(this).attr("href");
        var content = $(this).attr("confirm");
        var result = confirm(content);
        if (result) {
            window.location.href = href;
        }
    });

    // Selected menu
    // ===============================
    var selectedMenu = $("body").attr("selected-menu");
    $(selectedMenu).addClass("active");

});