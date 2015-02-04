/* Namespace */
if (!this.dts || typeof this.dts !== 'object') {
    this.dts = {};
}

dts.account = (function ($) {
    var me = {}

    initDeleteDialog = function (args) {
        $(args._selector).click(function (e) {
            bootbox.dialog({
                message: args._message,
                title: args._title,
                buttons: {
                    main: {
                        label: "Yes",
                        className: "btn-primary",
                        callback: function () {
                            $('<input />').attr('type', 'hidden')
                                .attr('name', "toDo")
                                .attr('value', "delete")
                                .appendTo('form');                           
                            $('form.editform').submit();
                        }
                    },
                    cancel: {
                        label: "Cancel",
                        className: "btn-link",
                        callback: function () {
                            $(this).modal('hide');
                        }
                    }
                }
            });
        });
    }


    // initilize and setup datatables
    init = function (args) {
        initDeleteDialog(args);
    };

    me.init = init;

    return me;
}($));