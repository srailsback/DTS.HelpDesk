/* Namespace */
if (!this.dts || typeof this.dts !== 'object') {
    this.dts = {};
}

dts.articles = (function ($) {
    var me = {},
        _dataTable,
        _dateTimeFormat = 'MM/DD/YYYY hh:mm A';

    // Special column rendering
    renderColumns = function (args) {
        var $thead = $(args._tableSelector + ' thead tr');
        var cols = dts.utils.buildDataTablesColumns($thead, args._columns);

        // editButton
        cols[0].render = function (data, type, row) {
            return dts.utils.renderTmplGridControlsEdit(data, args._editUrl);
        };

        // publush checkbox
        cols[3].render = function (data, type, row) {
            return dts.utils.renderGridControlsCheckbox(data, 'ispublished');
        }

        // createdAtUTC
        cols[5].render = function (data, type, row) {
            return dts.utils.formatJsonDateTime(data, _dateTimeFormat);
        }

        // updatedAtUTC
        cols[6].render = function (data, type, row) {
            return dts.utils.formatJsonDateTime(data, _dateTimeFormat);
        }
        return cols;
    }


    handlePublish = function (args) {
        $(args._tableSelector).delegate('input:checkbox', 'change', function () {
            var row = _dataTable.row().data();
            var url = args._publishUrl + "/" + row.Id;
            $.ajax({
                url: url,
                type: 'POST'
            });

        });
    }

    // initilize and setup datatables
    init = function (args) {
        
        // build datatables columns
        args._columns = renderColumns(args);

        // get datatables object and kick it back
        _dataTable = dts.utils.buildDataTables(args);

        handlePublish(args);

        return _dataTable;
    };

    me.init = init;

    return me;
}($));