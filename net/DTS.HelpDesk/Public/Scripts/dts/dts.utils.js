/*global sdge */

//Namespace
if (!this.dts || typeof this.dts !== 'object') {
	this.dts = {};
}

dts.utils = (function ($) {
	var me = {};


	// Build DataTables columns
	me.buildDataTablesColumns = function ($thead, columns) {
		var cols = [];
		var buildHead = $thead.find('th').length === 0;
		$.each(columns, function (idx, item) {
			if (buildHead) {
				$('<th>').text(item.name).appendTo($thead);
			}
			cols.push(me.parseColumn(item));
		});

		return cols;
	};

    // parse columns 
	me.parseColumn = function (item) {
	    var col = {
	        data: item.data,
	        name: item.name,
	        sortable: item.sortable,
	        visible: item.visible,
	        width: item.width,
            className : item.className
	    };
	    return col;
	}

	me.renderTmplGridControlsEdit = function (data, url) {
	    var obj = { id: data, editUrl: url };
	    var html = ich.tmplGridControlsEdit(obj);
	    return $(html).wrap('<div></div>').html();
	}

	me.renderGridControlsCheckbox = function (data, selector) {
	    var obj = { value: true, selector: selector, checked : data === true ? 'checked' : '' };
	    var html = ich.tmplGridControlsCheckbox(obj);
	    return $(html).wrap('<div></div>').html();
	}

	me.formatJsonDateTime = function(data, dateTimeFormat) {
	    return moment(data).format(dateTimeFormat);
	}

    // build datatables
	me.buildDataTables = function (args) {
	    var _dataTable = $(args._tableSelector).DataTable({
	        columns: args._columns,
	        order: [[1, 'asc']],
	        pageLength: 20,
	        lengthMenu: [10, 20, 50, 100],
	        pagingType: 'simple_numbers',
	        processing: true,
	        serverSide: true,
	        language: {
	            search: 'Search: ',
	            zeroRecords: function () {
	                if ($('div.dataTables_filter input').val()) {
	                    return 'No data available.';
	                } else {
	                    return 'No data available.';
	                }
	            },
	            processing: 'Fetching data...'
	        },
	        ajax: {
	            url: args._getUrl,
	            type: 'POST',
	            error: function (xhr, textStatus, error) {
	                if (textStatus === 'timeout') {
	                    alert('The server took too long to send the data.');
	                }
	                else {
	                    alert('An error occurred on the server. Please try again in a minute.');
	                }
	                console.error(error);
	            }
	        }
	    });

	    $(args._tableSelector).delegate('.delete-button', 'click', function () {
	        return confirm('Are you sure you want to delete this item?');
	    });
	    return _dataTable;

	}

    // date time picker
	me.setupDateTimePicker = function(selector, dateTimeFormat) {
	    return $(selector).datetimepicker({
	        useSeconds: false,
	        useCurrent: true,
            format : dateTimeFormat
	    });
	}
    return me;
}($));
