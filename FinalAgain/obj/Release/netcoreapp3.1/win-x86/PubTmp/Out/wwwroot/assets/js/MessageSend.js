$("#sendButton").click(function (e) {
    e.preventDefault();
    let text = $("#messageInput").val();
    let url = $(this).attr("href");
    let channelId = $("#ChannelId").val();
    connection.invoke("SendMessage", text);
    fetch(url, {
        method: "POST",
        body: JSON.stringify({ "text": text, "ChannelId": Number(channelId) }),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                console.log(response)
                return;
            }
            return response;
        })
})
connection.on("ReceiveMessage", function (name, message) {
    console.log(connection);
    var html = `<li>
                               <p>${name}:${message}</p>
                            </li> `;
    $("#messageBox").append(html);
})