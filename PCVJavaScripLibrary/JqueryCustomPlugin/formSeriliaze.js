//<script src="//code.jquery.com/jquery-2.1.0.min.js"></script>

//Form Serialize to C# object Model based on [name] of the input attribute.
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

//Paritial Load of div through ajax call with input parameters.
$.fn.reloadView = function (url, divId, data) {
    //debugger;
    $.ajax({
        url: url,
        type: 'post',
        data: data,//{ "empID": empID },
        success: function (data) {
            $("#" + divId).html(data);
        }
    });
}
//Parital load of div through ajax call without parameters.
$.fn.reload = function (url, divId) {
    debugger;
    $.ajax({
        url: url,
        type: 'post',
        success: function (data) {
            $("#" + divId).html(data);
        }
    });
}
//Clear form inputs
$.fn.clearform = function () {
    // debugger;
    console.log(this);
    this.find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
}

//Binds of table row data to target tags
function BindSelectedDataToPreview($Source, $target) {
    //  debugger;
    var html_table_data = "";
    var bRowStarted = true;
    $Source.find('tbody').first().find('tr').each(function () {
        if ($(this).find('input[type="checkbox"]').is(':checked')) {
            $('td', this).each(function () {
                if (html_table_data.length == 0 || bRowStarted == true) {
                    html_table_data += "<li>" + $(this).text().trim();
                    bRowStarted = false;
                }
                else if ($(this).text().trim() != "")
                    html_table_data += $(this).text() + " ,";
            });
            html_table_data = removeLastComma(html_table_data);
            html_table_data += "</li> \n";
            bRowStarted = true;
        }
    });
    $target.html("<ul>" + html_table_data + "</ul>");
    //alert(html_table_data);
}
//Binds of table row data to target tags
function BindSelectedDataToPreviewParagraphTags($Source, $target) {
    //  debugger;
    var html_table_data = "";
    var bRowStarted = true;
    $Source.find('tbody').first().find('tr').each(function () {
        if ($(this).find('input[type="checkbox"]').is(':checked')) {
            $('td', this).each(function () {
                if (html_table_data.length == 0 || bRowStarted == true) {
                    html_table_data += '<p href="#" style="color: #fff; text-decoration: none; background-color: transparent;line-height: 16px;">' + $(this).text().trim();
                    bRowStarted = false;
                }
                else if ($(this).text().trim() != "")
                    html_table_data += $(this).text() + " ,";
            });
            html_table_data = removeLastComma(html_table_data);
            html_table_data += "</p> \n";
            bRowStarted = true;
        }
    });
    $target.html(html_table_data);
    //alert(html_table_data);
}
//Remove's Last Comma in a string
function removeLastComma(str) {
    return str.replace(/,(\s+)?$/, '');
}