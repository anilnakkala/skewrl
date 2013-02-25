(function ($) {
    // register namespace
    Skewrl = {}
    Skewrl.Util = {}
    Skewrl.Chart = {}

    Skewrl.Util.Formatters = {
        DateTimeUtcToLocal: function (row, cell, value, columnDef, dataContext) {
            if (value == null || value === "" || value < 0) { return ""; }

            //var dd = new Date(value);
            var dd = new Date(parseInt(value.replace(/\/Date\((\d+)\)\//gi, "$1")));
            return dd.toLocaleString();
        },
        HideColumn: function (row, cell, value, columnDef, dataContext) { return "<div style='display: none;'>" + row + "," + cell + "," + value + "," + columnDef + "," + dataContext + "</div>"; }
    }

    Skewrl.Url = {
        grid: null,
        elemId: '',
        columns: [
                    { id: "ShortUrl", name: "Short URL", field: "ShortUrl", width: 250 },
                    { id: "LongUrl", name: "Long URL", field: "LongUrl", width: 300 },
                    { id: "Created", name: "Created", field: "Created", width: 250, formatter: Skewrl.Util.Formatters.DateTimeUtcToLocal },
                    { id: "Clicks", name: "# Clicks", field: "Clicks" },
                    { id: "IsActive", name: "", field: "IsActive", width: 25, formatter: function (r, c, val, def, datactx) {
                        if (val == true)
                            return '<a href="#" onclick="Skewrl.Url.MakeActive(' + "'" + datactx['ShortUrlCode'] + "'," + !datactx['IsActive'] + ')"><img src="/Images/delete.png" title="Your link is currently active. Click here to deactivate." alt="Deactivate" /></a>';
                        else
                            return '<a href="#" onclick="Skewrl.Url.MakeActive(' + "'" + datactx['ShortUrlCode'] + "'," + !datactx['IsActive'] + ')"><img src="/Images/accept.png" title="Your link is currently inactive. Click here to activate." alt="Activate" /></a>';
                    }
                    },
                    { id: "Details", name: "", field: "Details", width: 25, formatter: function (r, c, id, def, datactx) { return '<a href="#" onclick="Skewrl.Url.Details(' + "'" + datactx['ShortUrlCode'] + "'" + ')"><img src="/Images/page_world.png" title="Details" alt="Details" /></a>'; } },
                    { id: "Remove", name: "", field: "Remove", width: 25, formatter: function (r, c, id, def, datactx) { return '<a href="#" onclick="Skewrl.Url.Remove(' + "'" + datactx['ShortUrlCode'] + "'" + ')"><img src="/Images/Cross.png" title="Delete" alt="Delete" /></a>'; } }
                ],

        options: { enableCellNavigation: true, enableColumnReorder: false, enableTextSelectionOnCells: true },
        GetElementId: function () { return this.elemId; },
        GetColumns: function () { return this.columns; },
        GetOptions: function () { return this.options; },
        LoadUrlGrid: function (htmlId) {
            if (htmlId != "") this.elemId = "#" + htmlId;

            if (this.elemId == "") return;

            var myData = [];
            $.ajaxSetup({ cache: false });

            $.getJSON('/Home/MyUrlsJson', function (data) {
                if (data.Length > 0) {
                    $("#NoUrlDiv").hide();
                    myData = data.Urls;
                    grid = new Slick.Grid(Skewrl.Url.GetElementId(), myData, Skewrl.Url.GetColumns(), Skewrl.Url.GetOptions());
                    grid.render();
                } else {
                    $("#NoUrlDiv").show();
                    if (typeof grid != 'undefined') $(Skewrl.Url.GetElementId()).hide();
                }
            });
        },
        Remove: function (rid) {
            $.ajax({
                url: '/Home/MyUrl', type: 'delete', data: { id: rid }, success: function (data, status) { $("#status").text(data.message); Skewrl.Url.LoadUrlGrid("urlGridView"); }
            });

            return false;
        },
        Details: function (rid) {
            window.location.href = '/Home/MyUrl/' + rid;
        },
        MakeActive: function (rid, active) {
            $.ajax({
                url: '/Home/MyUrl', type: 'post', data: { id: rid, flag: active }, success: function (data, status) { $("#status").text(data.message); Skewrl.Url.LoadUrlGrid("urlGridView"); }
            });
            return false;
        }
    }
})(jQuery);