/* Namespace */
if (!this.dts || typeof this.dts !== 'object') {
    this.dts = {};
}

dts.faq = (function ($) {
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
                                .appendTo('form.editform');
                            var form = $('form.editform');
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


    initEditor = function (args) {
        $(args._textAreaSelector).summernote({
            height: args._textAreaHeight
        });
    }

    // initilize and setup datatables
    init = function (args) {
        initDeleteDialog(args);
        initEditor(args);
    };

    me.init = init;

    return me;
}($));