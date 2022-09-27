$("button[data-dismiss=modal]").click(function () {
    $(".modal").modal('hide');
});
        
(function () {
    $("#loaderbody").addClass('hide');

   $(document).bind('ajaxStart', function () {
       $("#loaderbody").removeClass('hide');
   }).bind('ajaxStop', function () {
$("#loaderbody").addClass('hide');
   });
});

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
           // const modal = $('#form-modal');
                $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
           let form = document.querySelector('#testform');
            console.log(form);
            $.validator.unobtrusive.parse(form);
        
            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

showPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            // const modal = $('#form-modal');
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            let form = document.querySelector('#test');
            console.log(form);
            $.validator.unobtrusive.parse(form);

            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}
showPop = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            // const modal = $('#form-modal');
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            let form = document.querySelector('#tst');
            console.log(form);
            $.validator.unobtrusive.parse(form);

            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}
jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}
