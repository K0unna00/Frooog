$(".fooormListA").click(function (e) {
    document.querySelector(".fooormInputDiv").classList.remove("displayN");
    //document.querySelector(".placeHolderImg").classList.add("displayN");

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
            $(".askQuestion").attr("data-id", data.userId);
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
                if (x.isUseful == true) {
                    bodyHTML += `<li>
                            <div class="d-flex justify-content-between">
                                <div>
                                    <img src="/uploads/users/${x.user.userPP}">
                                    <span class="col-12">${x.user.userName}|  ${x.createdAt.slice(11, 16)} </span>
                                </div>
                                <div>
                                    <i class="fa-solid fa-check usefulMessage text-success" data-id="${x.id}" data-userId="${x.user.id}"></i>
                                </div>
                            </div>
                            <p class="col-12">${x.text}</p>
                            <hr>
                            </li>`;
                }
                else {
                    bodyHTML += `<li>
                            <div class="d-flex justify-content-between">
                                <div>
                                    <img src="/uploads/users/${x.user.userPP}">
                                    <span class="col-12">${x.user.userName}|  ${x.createdAt.slice(11, 16)} </span>
                                </div>
                                <div>
                                    <i class="fa-solid fa-check usefulMessage" data-id="${x.id}" data-userId="${x.user.id}"></i>
                                </div>
                            </div>
                            <p class="col-12">${x.text}</p>
                            <hr>
                            </li>`;
                }
            })
            $(".answerList").empty();
            $(".answerList").append(bodyHTML);
            
        })
        .then(() => {
            let elem = document.querySelector(".answerList");
            elem.scrollTop = elem.scrollHeight;
        })
})
$(document).on("click", ".usefulMessage", function (e) {
    e.preventDefault();
    let id = $(this).attr("data-id");
    let userId = $(".askQuestion").attr("data-id");
    fetch("/Fooorm/SetUsefulMessage", {
        method: "post",
        body: JSON.stringify({ "TextId":Number(id) , "UserId": userId }),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            return response.json();
        })
        .then(data => {
            if (data == true) {
                $(this).toggleClass("text-success");
            }
        });
});
