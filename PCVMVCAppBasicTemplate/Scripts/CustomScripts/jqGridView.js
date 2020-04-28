/// <reference path="Scripts/jquery-2.2.0.js" />
/// <reference path="notify.js" />
/// <reference path="../../../pcvjavascriplibrary/jquerycustomplugin/api.js" />
/// <reference path="../../../pcvjavascriplibrary/jquerycustomplugin/formseriliaze.js" />
/// <reference path="../../customscript/api.js" />

//https://notifyjs.jpillora.com/
(function () {

    $.fn.jqGridView = function (options) {
        settings = $.extend({
            datasourceurl: "",
            data:""
        }, options);
        this.append(Getparam(settings.datasourceurl, settings.data));


    }

    var Getparam = function (url, data) {
    //    debugger;
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            success: function (data) {
                return data;
            }, error: function (e, x) {
                console.log(e);
            }
        });
    }
})(jQuery());

