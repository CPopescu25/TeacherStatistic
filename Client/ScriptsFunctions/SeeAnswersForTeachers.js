function seeFA(e) {
    var i = $(e).data("id");
    var SetType = 1;
    window.open("/Answer/FirstSetAnswersUser?id=" + i + "&SetType=" + SetType);
}
function seeSA(e) {
    var i = $(e).data("id");
    var SetType = 2;
    window.open("/Answer/FirstSetAnswersUser?id=" + i + "&SetType=" + SetType);
}