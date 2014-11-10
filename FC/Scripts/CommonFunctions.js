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

/*$(".delete-goal-button").click(function () {
    $.ajax({
        type: "GET",
        url: "/Goal/DeleteGoal/"+ $(this).attr("id"),
        success: function (data) {
            $('#goals-table').html(data);
        }
    });
});
*/
        