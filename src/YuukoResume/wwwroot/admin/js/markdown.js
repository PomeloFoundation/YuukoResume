

function DropEnable() {
    $('.markdown-textbox').unbind().each(function () {
        var editor = $(this);
        $(this).dragDropOrPaste(function () {
            var pos = editor.getCursorPosition();
            var str = editor.val();
            if (pos == 0 && !editor.is(':focus'))
                pos = str.length;
            editor.val(str.substr(0, pos) + '\r\n![Upload](Uploading...)\r\n' + str.substr(pos));
        },
        function (result) {
            var content = editor.val().replace('![Upload](Uploading...)', '![' + result.FileName + '](/file/download/' + result.Id + ')');
            editor.val(content);
        });
    });
}

function uploadAttachment() {
    $('#uploadFile').unbind('change').change(function () {
        uploadFile();
    });
    $('#uploadFile').click();
}

function uploadFile() {
    var formData = new FormData($('#frmAjaxUpload')[0]);
    var editor = $('.markdown-textbox');
    var pos = editor.getCursorPosition();
    var str = editor.val();
    if (pos == 0 && !editor.is(':focus'))
        pos = str.length;
    editor.val(str.substr(0, pos) + '\r\n![Upload](Uploading...)\r\n' + str.substr(pos));

    $.ajax({
        url: '/file/upload',
        type: 'POST',
        data: formData,
        dataType: 'json',
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            var content = editor.val().replace('![Upload](Uploading...)', '![' + result.FileName + '](/file/download/' + result.Id + ')');
            editor.val(content);
        },
        error: function (returndata) {
        }
    });
}

$(document).ready(function () {
    DropEnable();
});