$(document).ready(function () {
    $(".confirm-submit").click(function (e) {
        var content = "Please Verify Your Informations: \n";
        var flag = true;
        $(".confirm").each(function () {
            console.log($(this).find(".form-control").is("input"));
            if ($(this).find(".form-control").val() == "") {
                flag = false;
                return false;
            } else {
                if ($(this).find(".form-control").is("select")) {
                    content += $(this).find(".control-label").html() + ": " + $(this).find(".form-control").find("option:selected").text() + "\n";
                } else {
                    content += $(this).find(".control-label").html() + ": " + $(this).find(".form-control").val() + "\n";
                }
                
            }
        });

        if (flag) {
            e.preventDefault();
            var result = confirm(content);
            if (result) {
                $("form").submit();
            }
        }
    });

    $(".confirm-delete").click(function (e) {
        e.preventDefault();
        var href = $(this).attr("href");
        var content = $(this).attr("confirm") ;
        var result = confirm(content);
        if (result) {
            window.location.href = href;
        }
    });
});