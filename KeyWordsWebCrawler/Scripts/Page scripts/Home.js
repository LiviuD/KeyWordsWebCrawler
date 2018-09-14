var getResultsUrl = null;
function onComplete(xhr, status) {
    $('button#search').removeAttr('disabled');
    $(previousResults).prepend($("<p>Url: <b>" + xhr.responseJSON.SearchResultsHistory.Url + "</b>, key: <b>" + xhr.responseJSON.SearchResultsHistory.Key + "</b>, Results: <b>" + xhr.responseJSON.SearchResultsHistory.Results + "</b> - <span style='font-size: 12px;'>" + xhr.responseJSON.FormatedDate  + "</span></p>"));
    debugger;
}
$(document).ready(function () {
    //$('button#search').click(function () {
    //    if (getResultsUrl) {
    //        $.get(getResultsUrl, function (data) {
    //            $("#results").html(data);
    //        });
    //    }
    //    else {
    //        console.log('get results url not initialized');
    //    }
    //});
});