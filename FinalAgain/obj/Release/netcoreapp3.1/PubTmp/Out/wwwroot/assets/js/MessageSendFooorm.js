function sendMessage(e) {
    e.preventDefault();
    let text = $(".fooormInput").val();
    if ((text != "") && (text.length < 120)) {
        document.querySelector("#sendButton").firstChild.setAttribute("src", "/assets/images/FrogJump.gif");
        const audio2 = new Audio();
        audio2.src = "/assets/sounds/jump.mp3";
        audio2.play();
        let url = $("#sendButton").attr("formaction");
        let fooormId = $(".fooormInput").attr("id");
        connection.invoke("SendMessageFooorm", text, Number(fooormId));
        fetch(url, {
            method: "POST",
            body: JSON.stringify({ "Text": text, "FooormId": Number(fooormId) }),
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
        document.querySelector(".fooormInput").value = "";
    }
}
$("#sendButton").click(function (e) {
    sendMessage(e);
})
connection.on("ReceiveMessageFooorm", function (text, fooormId, user, date) {
    let fooormID = $(".fooormInput").attr("id");
    if (fooormID == fooormId) {
        var html = `<li>
                            <div>
                            <img src="/uploads/users/${user.userPP}">
                            <span class="col-12">${user.userName} |  ${date.slice(11, 16)} </span>
                            </div>
                            <p class="col-12">${text}</p>
                            <hr>
                            </li>`;
        $(".answerList").append(html);
    }
    let elem = document.querySelector(".answerList");
    elem.scrollTop = elem.scrollHeight;
});
document.addEventListener("keydown", (e) => {
    if (e.code === "Enter") {
        sendMessage(e);
    }
});
