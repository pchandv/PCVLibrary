/// <reference path="../jquery-1.10.2.js" />



//Note:This Plugin Only Works,If the data type having Two properties with "Id","ReportName"
//Provide the Proper URL for Getting And Deleting
//This plugin generates the Header as "Report Name" and records with Delete anchor link
var Control;
var settings;
(function () {
    $.fn.jqCustomTable = function (options) {
        //Setting default options for this Jquery Plugin
        //durl-deleting-pass the delete service
        //url-Getting the reports/list
        //tablecss:provide the css class
        //By default contentType is jspon
        //Editable,search are not used here.
        settings = $.extend({
            Header: ["Report Name"],
            border: '1px',
            type: "GET",
            url: "",
            durl: "",
            contentType: "application/json; charset=utf-8",
            search: "",
            tableCss: '',
            Editable: false,
            // jdata: []

        }, options);
        Control = this;
        //For Every call,the jquery ajax is used to get the saved reports
        GetData(this);


    }

    function GenerateTable(JsonData, settings, table) {

        table.attr('border', settings.border);

        var row = $('<tr></tr>').addClass("HeaderCss");
        var tbody = $('<tbody></tbody>');
        //--Reads the records from Json -------------------------------------------
        $.each(JsonData, function (i, value) {
            var row = $('<tr></tr>').addClass("rowCss").attr("rowId", "rowId_" + i);
            delete value.__type;
            //Converts the JsonData[i] to array type example:
            //JsonData=[{'Id':'101'},{'Id':'102'},{'Id':'103'}]
            //array object contains 
            //var value=array[key]
            //key="Id" property ,Value=101,102,103
            var array = $.map(value, function (value, index) {
                return [value];
            });
            //Reads the array object 
            $.each(array, function (index, value) {
                var cell;
                var p;
                //Skips the first Index-Primary Key
                if (index != 0) {
                    //Create the Cell with td
                    cell = $('<td  cellIndex ="' + i + "_" + index + '"></td>');
                    //Create paragraph tag
                    p = $('<p id="par_' + i + "_" + index + '">' + value + '</p>');
                }

                //array.length - 1 === index->Anchor link attached to the Last Index only 
                if (array.length - 1 === index) {
                    //Create Anchor link for deleting the records
                    var deleteLink = $('<a style="margin-left:10px;cursor:pointer;color: red;" title="Click to Delete this report">x</a>').click(function () {
                        var flag = confirm("Are you sure want to delete this report ?");
                        if (flag) {
                            //Gets the JsonData Index value for the current clicked record
                            var id = JsonData[i].Id;
                            var Success = false;
                            //Ajax call used to delete the record
                            $.ajax({
                                type: "POST",
                                url: settings.durl,
                                async: false,
                                dataType: 'json',
                                contentType: "application/json; charset=utf-8",
                                data: '{"Reportid":' + JSON.stringify(id) + '}',
                                success: function (data) {
                                    console.log(data);
                                    Success = true;
                                },
                                error: function (xhr, x, p) {
                                    console.log('something went wrong, please try again');
                                }
                            });
                            if (Success) {
                                alert("Deleted Successfully");
                                $('tr[rowId=rowId_' + i + ']').remove();
                                $("#ddlRptFavs option[value='" + id + "']").remove();
                                $("#ddl_PQLlist option[value='" + id + "']").remove();
                                GetData(Control);
                            }


                        }
                    });
                    if (value != "No saved reports found") {
                        p.append(deleteLink);
                    }
                    cell.append(p);
                }

                row.append(cell);
            });

            tbody.append(row);

        });
        table.append(tbody);
    }

    function GetData(jq) {
        //Removes the tbl
        $('#tbl').remove();
        //Creates the table
        var table = $('<table id="tbl"></table>').addClass(settings.tableCss);

        //---Ajax call-----------------------------
        var JsonData;//= settings.jdata;
        //Gets the saved reports using Jquery ajax

        $.ajax({
            type: settings.type,
            url: settings.url,
            async: false,
            contentType: settings.contentType,
            dataType: "json",
            success: function (data) {
                if (data.length != 0) {
                    JsonData = data;
                } else {
                    var norecords = [{ "Id": "0", "ReportName": "No saved reports found" }];
                    JsonData = norecords;
                }
            },
            error: function (xhr, x, p) {
                console.log(xhr.error.toString());
                console.log(x.toString());
            }
        });

        //Header from Json
        //Creates the Header Row Thead and Header row
        var headerRowThead = $('<thead></thead>').addClass("HeaderCss");
        var headerRow = $('<tr></tr>');


        if (settings.Header.length === 0) {

            var array = [];
            //Generates the header with JsonData object
            $.each(JsonData, function (i, value) {
                delete value.__type;
                array = $.map(value, function (value, index) {
                    return [index];
                });
                return;
            });

            $.each(array, function (index, value) {
                var cell = $('<th></th>').text(value);
                headerRow.append(cell);
                if (settings.Editable && array.length - 1 == index) {
                    var ActionCell = $('<th>Action</th>');
                    headerRow.append(ActionCell);
                }
            });

        } else {

            //Header created using Settings Header provided by user/developer
            $.each(settings.Header, function (index, value) {
                var cell = $('<th></th>').text(value);
                headerRow.append(cell);

                if (settings.Editable && settings.Header.length - 1 == index) {
                    var ActionCell = $('<th>Action</th>');
                    headerRow.append(ActionCell);
                }

            });
        }
        headerRowThead.append(headerRow);
        table.append(headerRowThead);


        GenerateTable(JsonData, settings, table);


        jq.append(table);
        return jq;
    }

})(jQuery());

