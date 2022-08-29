$(".fooormListA").click(function (e) {
    document.querySelector(".fooormInputDiv").classList.remove("displayN");
    e.preventDefault();
    let url = $(this).attr("href");
    let id = $(this).attr("id");
    fetch(url, {
        method: "POST",
        body: JSON.stringify(Number(id)),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            return response.json();
        })
        .then(data => {
            document.querySelector(".fooormInput").setAttribute("id", data.id);
            $(".headerHTML").empty();
            let headerHTML = `<div class="Title text-center mt-1">
                        <h3>${data.header}</h3>
                        </div>
                        <div class="Body">
                        <h4>
                        ${data.text}
                        </h4>
                        </div>
                        <hr class="mt-2 fooormChatHr mx-auto" />`;
            $(".headerHTML").append(headerHTML);
        })
    fetch("/Fooorm/ShowFroomAnswers", {
        method: "POST",
        body: JSON.stringify(Number(id)),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            return response.json();
        })
        .then(data => {
            let bodyHTML="";
            data.forEach(x => {
                bodyHTML += `<li>
                            <div>
                            <img src="/uploads/users/${x.user.userPP}">
                            <span class="col-12">${x.user.userName} |  ${x.createdAt.slice(11, 16)} </span>
                            </div>
                            <p class="col-12">${x.text}</p>
                            <hr>
                            </li>`;
            })
            $(".answerList").empty();
            $(".answerList").append(bodyHTML);
            
        })
        .then(() => {
            let elem = document.querySelector(".answerList");
            elem.scrollTop = elem.scrollHeight;
        })
})
