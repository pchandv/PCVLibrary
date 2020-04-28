var $api = function () {
    function Get(url) {
        $.aj
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
    function Getparam(url, data) {
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
    function Post(url) {
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
    function Postdata(url, data) {
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