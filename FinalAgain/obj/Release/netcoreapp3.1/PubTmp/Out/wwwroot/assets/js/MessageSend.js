    function sendMessage(e){
        e.preventDefault();
        let text = $("#messageInput").val();
        if ((text != "") && (text.length < 120)) {
            document.querySelector("#sendButton").firstChild.setAttribute("src", "/assets/images/FrogJump.gif");
            const audio2 = new Audio();
            audio2.src = "/assets/sounds/jump.mp3";
            audio2.play();
            let url = $("#sendButton").attr("href");
            let channelId = $("#ChannelId").val();
            connection.invoke("SendMessage", text, Number(channelId));
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
                });
            document.getElementById("messageInput").value = "";
        }
    }
    
$("#sendButton").click(function (e) {
        sendMessage(e);
})
connection.on("ReceiveMessage", function (user, message , channelId,date) {

    let currentChannelId = $("#ChannelId").val();
    if (currentChannelId == channelId) {
        var html = `<li>
                        <div>
                            <img src="/uploads/users/${user.userPP}">
                            <span class="col-12">${user.userName} | ${date.slice(11, 16) }</span>
                        </div>
                        <p >${message}</p>
                     </li>`;
        $(".messageArea").find("ul").first().append(html);
    }
    let elem = document.querySelector(".messageArea");
    elem.scrollTop = elem.scrollHeight;
})
document.addEventListener("keydown",(e)=>{
    if(e.code === "Enter"){
        sendMessage(e);
    }
});