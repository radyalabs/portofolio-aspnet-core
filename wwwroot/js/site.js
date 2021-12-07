function toggleSidenav(open) {
    if (open == "true") {
        $("#sidenav").attr("data-open", "false");
        $("#sidenav").addClass("sidenav-off");
        
        $("#sidenav-ul").addClass("nav-flush text-center");
        $(".nav-link").addClass("py-3 border-bottom");
        $(".nav-text").addClass("d-none");

        $("main").addClass("off");
    } else {
        $("#sidenav").attr("data-open", "true");
        $("#sidenav").removeClass("sidenav-off");

        $("#sidenav-ul").removeClass("nav-flush text-center");
        $(".nav-link").removeClass("py-3 border-bottom");
        $(".nav-text").removeClass("d-none");

        $("main").removeClass("off");
    }
}

function setTextareaHeight(e) {
    console.log(e.scrollHeight);
    e.style.height = "5px";
    e.style.height = e.scrollHeight + "px";
}

$(document).ready(function() {
    var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
    toggleSidenav(width <= 768 ? "true" : "false")

    $(window).resize(function() {
        var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
        toggleSidenav(width <= 768 ? "true" : "false");
        
        for (var e of $("textarea")) {
            setTextareaHeight(e);
        }
    });

    $("#sidenav-button").on("click", function() {
        toggleSidenav($("#sidenav").attr("data-open"));
    });

    if (window.location.pathname.startsWith("/admin/categories")) {
        $("#nav-category").addClass("active");
    } else if (window.location.pathname.startsWith("/admin/members")) {
        $("#nav-member").addClass("active");
    } else if (window.location.pathname.startsWith("/admin/projects")) {
        $("#nav-project").addClass("active");
    } else {
        $("#nav-home").addClass("active");
    }

    $(".delete-form").on("submit", function(e) {
        e.preventDefault();

        if (window.confirm("Are you sure you want to delete this data?")) {
            e.target.submit();
        }
    });

    for (var e of $("textarea")) {
        setTextareaHeight(e)
    }
    $("textarea").on("input", function(e) {
        setTextareaHeight(e.target);
    });
});