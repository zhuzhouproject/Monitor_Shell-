$(function () {
    $(".mchart").click(function () {
        var url = "/UI_Monitor/TrendTool/TrendlineRenderer.aspx#" + this.id;
        window.open(url, "WindowChart", "width=800,height=600,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no");
    })
})