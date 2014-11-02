function RenderPartial(url, idOfElement) {
    $.ajax({
        url: url,
        type: "GET",
        dataType: "html",
        success: function(data) {
            $(idOfElement).html(data);
        }
});
}

