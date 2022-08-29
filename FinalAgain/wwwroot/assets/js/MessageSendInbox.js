function sendMessage(e) {
    e.preventDefault();
    let text = $("#messageInput").val();
    let image = $(".addImageInput").val();
    let messageId = 0;
    console.log(image);
    if ((text != "") && (text.length < 120)) {
        document.querySelector("#sendButton").firstChild.setAttribute("src", "/assets/images/FrogJump.gif");
        const audio2 = new Audio();
        audio2.src = "/assets/sounds/jump.mp3";
        audio2.play();
        let url = $("#sendButton").attr("href");
        let FUserName = $("#FUserID").val();
        let TUserID = $("#TUserID").val();
        fetch(url, {
            method: "POST",
            body: JSON.stringify({ "text": text, "FromUserName": FUserName, "ToUserId": TUserID }),
            headers: {
                'Content-type': 'application/json',
            },
        })
            .then(response => {
                if (!response.ok) {
                    console.log(response)
                    return;
                }
                return response.json();
            })
            .then(data => {
                connection.invoke("SendMessageInbox", text, FUserName, TUserID, data)
            });
        document.getElementById("messageInput").value = "";

    }
}
$("#sendButton").click(function (e) {
    sendMessage(e);
})
connection.on("ReceiveMessage", function (name, message, sendAt, FUserId ,id) {

    let FUserName = $("#FUserID").val();
    let TUserID = $("#TUserID").val();

    if ((FUserId == TUserID) || (FUserName == name)) {
        var html = `<li class="d-flex flex-wrap">
                               <div>
                                    <span class="col-12">${name}|${sendAt.slice(11, 16)}</span>
                                    <a class="deleteMessage" href="Home/DeleteMessage" data-id="${id}" id="delete${id}" ><i class="fa-solid fa-trash-can"></i></a>
                                </div>
                                <p class="col-12">${message}</p>
                            </li>`
        $(".messageArea").find("ul").first().append(html);
    }
    let elem = document.querySelector(".messageArea");
    elem.scrollTop = elem.scrollHeight;
});
document.addEventListener("keydown", (e) => {
    if (e.code === "Enter") {
        sendMessage(e);
    }
});
document.addEventListener("click", (e) => {
    if (e.target.classList.contains("deleteMessage")){
        e.target.parentNode.parentNode.remove();
    }
});