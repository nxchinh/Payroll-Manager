const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const UserId = $("#my-user-id").val();


connection.on("ReceiveMessage", function (user, message) {
    const isMine = UserId == user.id;

    if (isMine) {

        $('#messages-container').append(
            `
            <div class="media w-50 ml-auto mb-3">
                <div class="media-body">
                    <div class="bg-primary rounded py-2 px-3 mb-2">
                        <p class="text-small mb-0 text-white">${ message}</p>
                    </div>
                    <p class="small text-muted">${ new Date().toLocaleString()}</p>
                </div>
            </div>
            `
        );
    } else {
        $('#messages-container').append(
            `<div class="media w-50 mb-3">
                <img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle">
                    <div class="media-body ml-3">
                        <p class="small text-muted">${user.lastName || ''} ${user.firstName || ''} ${user.middleName || ''}</p>
                        <div class="bg-light rounded py-2 px-3 mb-2">
                            <p class="text-small mb-0 text-muted">${ message}</p>
                        </div>
                        <p class="small text-muted">${ new Date().toLocaleString()}</p>
                    </div>
                </div>`
        );
    }
});

connection.on("UserConnected", function (user) {
    const isMine = UserId == user.id;

    if (isMine) {
        if ($(`[data-user-id].active`).length > 0) {
            $(`[data-user-id].active`).each(function () {
                $(this).remove();
            });
        }
    }
    console.log($(`[data-user-id].active`))


    $('#online-list').append(
        `
        <a href="#" data-user-id="${user.id}" class="list-group-item list-group-item-action list-group-item-light rounded-0 ${isMine ? 'active' : ''}">
            <div class="media">
                <img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle">
                <div class="media-body ml-4">
                    <div class="d-flex align-items-center justify-content-between mb-1">
                        <h6 class="mb-0">${user.lastName} ${user.firstName}</h6>
                    </div>
                </div>
            </div>
        </a>
        `
    )
});

connection.on("UserDisconnected", function (user) {
    $('a[data-user-id="' + user.id + '"]').remove();
});

connection.start()
    .then(function () {

    })
    .catch(function (err) {
        return console.error(err.toString());
    });

$('.chat-form').submit(function (e) {
    e.preventDefault();
    const message = $('#message-box').val().trim();
    var thoigian = new Date();
    if (message && message.length > 0) {
        connection.invoke("SendMessage", UserId, message,thoigian).then(() => $('#message-box').val(''));
    }

});