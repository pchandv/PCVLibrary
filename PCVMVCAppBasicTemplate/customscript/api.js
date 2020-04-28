(function () {
    $.fn.api = function () {
        var get = function (url) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    return data;
                }, error: function (e, x) {
                    console.log(e);
                }
            });
        };
        var Getparam = function (url, data) {
            debugger;
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
        var Post = function (url) {
            $.ajax({
                url: url,
                type: 'Post',
                success: function (data) {
                    return data;
                }, error: function (e, x) {
                    console.log(e);
                }
            });
        }
        var Postdata = function (url, data) {
            $.ajax({
                url: url,
                type: 'Post',
                data: data,
                success: function (data) {
                    return data;
                }, error: function (e, x) {
                    console.log(e);
                }
            });
        }
    }
})(jQuery());