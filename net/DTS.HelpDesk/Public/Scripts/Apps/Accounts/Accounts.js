/* Namespace */
if (!this.dts || typeof this.dts !== 'object') {
    this.dts = {};
}

dts.accounts = (function ($) {
    var me = {},
        _dataTable;

    // Special column rendering
    renderColumns = function (args) {
        var $thead = $(args._tableSelector + ' thead tr');
        var cols = dts.utils.buildDataTablesColumns($thead, args._columns);

        // controls
        cols[0].render = function (data, type, row) {
            return dts.utils.renderTmplGridControlsEdit(data, args._editUrl);
        };

        // join roles into string
        cols[3].render = function(data, type, row) {
            var result = '';
            $.each(data, function (i, item) {
                result = result + item.Name
                if (i < data.length - 1) {
                    result = result + ', ';
                }
            })
            return result;
        }

        // locked out bool to yes or no
        cols[4].render = function (data, type, row) {
            return data === true ? 'Yes' : 'No';
        }


        return cols;
    }

    // initilize and setup datatables
    init = function (args) {
        
        // build datatables columns
        args._columns = renderColumns(args);

        // get datatables object and kick it back
        _dataTable = dts.utils.buildDataTables(args);

        return _dataTable;
    };

    me.init = init;

    return me;
}($));